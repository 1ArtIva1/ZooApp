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
            _database = "vkr";
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
            JOIN Category c ON s.category_id = c.id";
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

        public List<DiscountsList> LoadDiscounts()
        {
            var discounts = new List<DiscountsList>();
            try
            {
                OpenConnection();
                string query = "SELECT id, name, value FROM discounts";
                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        discounts.Add(new DiscountsList
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Value = reader.GetInt32(2),
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

        public void AddDiscounts(DiscountsList discounts)
        {
            try
            {
                OpenConnection();
                string query = @"
                    INSERT INTO discounts (name, value)
                    VALUES (@name, @value)";
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("name", discounts.Name);
                    cmd.Parameters.AddWithValue("value", discounts.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public void DeleteDiscount(int id)
        {
            OpenConnection();
            using (var cmd = new NpgsqlCommand("DELETE FROM discounts WHERE id = @id", _connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            CloseConnection();
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
        INSERT INTO Storage (name, unit, qty, wholesale_price, retail_price, category_id)
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

        public List<User> LoadUsers() 
        {
            var users = new List<User>();

            try
            { 
                OpenConnection();
                string query = @"
            SELECT e.id, e.login, e.name, e.surname             
            FROM employees e";
                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Login = reader.GetString(1),
                            Name = reader.GetString(2),
                            Surname = reader.GetString(3),
                        });
                    }
                }
            }
            finally
            {
                CloseConnection();
            }

            return users;
        }
          
        public void UpdateStorageItemQuantity(int id, int newQuantity)
        {
            try
            {
                OpenConnection();
                using (var cmd = new NpgsqlCommand("UPDATE storage SET qty = @qty WHERE id = @id", _connection))
                {
                    cmd.Parameters.AddWithValue("@qty", newQuantity);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public int AddReceipt(Receipt receipt)
        {
            int newId = 0;
            try
            {
                OpenConnection();
                using (var cmd = new NpgsqlCommand(
                    @"INSERT INTO receipts (employee_id, date, total)
                      VALUES (@employee_id, @date, @total)
                      RETURNING id;", _connection))
                {
                    cmd.Parameters.AddWithValue("@employee_id", receipt.EmployeeId);
                    cmd.Parameters.AddWithValue("@date", receipt.Date);
                    cmd.Parameters.AddWithValue("@total", receipt.Total);

                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            finally
            {
                CloseConnection();
            }
            return newId;


        }

        public int AddReceiptItem(ReceiptItem item)
        {
            int newId = 0;
            try
            {
                OpenConnection();
                using (var cmd = new NpgsqlCommand(
                    @"INSERT INTO receiptitems (receipt_id, storage_id, discount_id, quantity, price)
              VALUES (@receipt_id, @storage_id, @discount_id, @quantity, @price)
              RETURNING id;", _connection))
                {
                    cmd.Parameters.AddWithValue("@receipt_id", item.ReceiptId);
                    cmd.Parameters.AddWithValue("@storage_id", item.StorageId);
                    cmd.Parameters.AddWithValue("@discount_id", (object)item.DiscountId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                    cmd.Parameters.AddWithValue("@price", item.Price);

                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            finally
            {
                CloseConnection();
            }
            return newId;
        }

        public List<Receipt> LoadReceipts()
        {
            var receipts = new List<Receipt>();
            try
            {
                OpenConnection();
                string query = @"SELECT id, employee_id, date, total FROM receipts ORDER BY date DESC";
                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        receipts.Add(new Receipt
                        {
                            Id = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                            Total = reader.GetDecimal(3)
                        });
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
            return receipts;
        }

        public List<ReceiptItemViewModel> LoadReceiptItemsByReceiptId(int receiptId)
        {
            var items = new List<ReceiptItemViewModel>();
            try
            {
                OpenConnection();
                string query = @"
                    SELECT s.name, ri.quantity, ri.price
                    FROM receiptitems ri
                    JOIN storage s ON ri.storage_id = s.id
                    WHERE ri.receipt_id = @receiptId";
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@receiptId", receiptId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new ReceiptItemViewModel
                            {
                                Name = reader.GetString(0),
                                Quantity = reader.GetInt32(1),
                                Price = reader.GetDecimal(2)
                            });
                        }
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
            return items;
        }

        public void ReturnReceipt(int receiptId)
        {
            try
            {
                OpenConnection();

                // Получаем все позиции чека
                var items = new List<(int StorageId, int Quantity)>();
                using (var cmd = new NpgsqlCommand(
                    "SELECT storage_id, quantity FROM receiptitems WHERE receipt_id = @receiptId", _connection))
                {
                    cmd.Parameters.AddWithValue("@receiptId", receiptId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add((reader.GetInt32(0), reader.GetInt32(1)));
                        }
                    }
                }

                // Возвращаем товар на склад
                foreach (var item in items)
                {
                    // Получаем текущее количество
                    int currentQty = 0;
                    using (var cmd = new NpgsqlCommand(
                        "SELECT qty FROM storage WHERE id = @id", _connection))
                    {
                        cmd.Parameters.AddWithValue("@id", item.StorageId);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            currentQty = Convert.ToInt32(result);
                    }

                    // Обновляем количество
                    using (var cmd = new NpgsqlCommand(
                        "UPDATE storage SET qty = @qty WHERE id = @id", _connection))
                    {
                        cmd.Parameters.AddWithValue("@qty", currentQty + item.Quantity);
                        cmd.Parameters.AddWithValue("@id", item.StorageId);
                        cmd.ExecuteNonQuery();
                    }
                }

                
            }
            finally
            {
                CloseConnection();
            }
        }

        public void DeleteReceipt(int receiptId)
        {
            try
            {
                OpenConnection();
                // Сначала удаляем все позиции чека
                using (var cmd = new NpgsqlCommand("DELETE FROM receiptitems WHERE receipt_id = @id", _connection))
                {
                    cmd.Parameters.AddWithValue("@id", receiptId);
                    cmd.ExecuteNonQuery();
                }
                // Затем удаляем сам чек
                using (var cmd = new NpgsqlCommand("DELETE FROM receipts WHERE id = @id", _connection))
                {
                    cmd.Parameters.AddWithValue("@id", receiptId);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CloseConnection();
            }
        }
    }

    public class ReceiptItemViewModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Sum => Price * Quantity;
    }
}