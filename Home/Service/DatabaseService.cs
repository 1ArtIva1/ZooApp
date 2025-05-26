using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using Home.Service;

namespace Home.Services
{
    public class DatabaseService : IDisposable
    {
        private NpgsqlConnection _connection;
        private string _host;
        private string _database;
        private int _port;

        public DatabaseService()
        {
            _host = "localhost";
            _port = 5432;
            _database = "postgres";
        }

        public void InitializeConnection(string username, string password)
        {
            var connectionString =
                $"Host={_host};Port={_port};Database={_database};Username={username};Password={password}";

            _connection = new NpgsqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public NpgsqlConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public List<string> GetUserRoles()
        {
            var roles = new List<string>();

            try
            {
                OpenConnection();
                using (var cmd = new NpgsqlCommand(
                    "SELECT rolname FROM pg_roles WHERE pg_has_role(current_user, oid, 'member')",
                    _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader.GetString(0));
                        }
                    }
                }
                return roles;
            }
            finally
            {
                CloseConnection();
            }
        }


        public List<StorageItem> LoadStorageItems()
        {
            var items = new List<StorageItem>();
            try
            {
                OpenConnection();
                string query = @"
            SELECT s.id, s.name, s.unit, s.qty, c.name as category, s.retail_price
            FROM storage s
            JOIN Category c ON s.id_category = c.id";
                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new StorageItem
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Unit = reader.GetString(2), // добавлено!
                            Quantity = reader.GetInt32(3),
                            Category = reader.GetString(4),
                            Price = reader.GetDecimal(5)
                        });
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
            return items;
        }
        

        public string GetUserMainRole()
        {
            try
            {
                OpenConnection();
                using (var cmd = new NpgsqlCommand(
                    "SELECT rolname FROM pg_roles WHERE rolname = current_user",
                    _connection))
                {
                    return cmd.ExecuteScalar()?.ToString() ?? "не определена";
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public List<Discounts> LoadDiscounts()
        {
            var discounts = new List<Discounts>();
            try
            {
                OpenConnection();
                string query = "SELECT id, name, value FROM discount";
                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        discounts.Add(new Discounts
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            DiscountValue = reader.GetInt32(2)
                        });
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
            return discounts;
        }

        public void AddDiscounts(Discounts discounts)
        {
            try
            {
                OpenConnection();
                string query = @"
                    INSERT INTO discount (name, value)
                    VALUES (@name, @value)";
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("name", discounts.Name);
                    cmd.Parameters.AddWithValue("value", discounts.DiscountValue);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public void AddStorageItem(StorageItem item)
        {
            try
            {
                OpenConnection();

                // Получаем id категории "Без категории" (можно заменить или убрать если не используешь категории)
                int categoryId;
                using (var cmd = new NpgsqlCommand("SELECT id FROM Category WHERE name = @name", _connection))
                {
                    cmd.Parameters.AddWithValue("name", item.Category);
                    var result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        using (var insertCmd = new NpgsqlCommand("INSERT INTO Category (name) VALUES ('Без категории') RETURNING id", _connection))
                        {
                            categoryId = (int)insertCmd.ExecuteScalar();
                        }
                    }
                    else
                    {
                        categoryId = (int)result;
                    }
                }

                // Вставка в таблицу Storage
                string insertQuery = @"
        INSERT INTO Storage (name, unit, qty, wholesale_price, retail_price, id_category)
        VALUES (@name, @unit, @qty, @purchase, @retail, @cat)";
                using (var cmd = new NpgsqlCommand(insertQuery, _connection))
                {
                    cmd.Parameters.AddWithValue("name", item.Name);
                    cmd.Parameters.AddWithValue("unit", item.Unit);
                    cmd.Parameters.AddWithValue("qty", item.Quantity);
                    cmd.Parameters.AddWithValue("purchase", item.PurchasePrice);
                    cmd.Parameters.AddWithValue("retail", item.Price);
                    cmd.Parameters.AddWithValue("cat", categoryId);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CloseConnection();
            }
        }
        public List<string> GetAllCategories()
        {
            var categories = new List<string>();
            try
            {
                OpenConnection();
                string query = "SELECT name FROM Category";
                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(reader.GetString(0));
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
            return categories;
        }

        public void DeleteStorageItem(int id)
        {
            OpenConnection();
            using (var cmd = new NpgsqlCommand("DELETE FROM storage WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            CloseConnection();
        }
    }
}