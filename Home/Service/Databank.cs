using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Home.Service;

namespace Home
{
    internal class Databank
    {
        public static string username;
        public static string password;

        public static int CurrentUserId;

        public static List<StorageItem> StorageItems = new List<StorageItem>();
        public static List<DiscountsList> DiscountsList = new List<DiscountsList>();
        public static List<User> UsersList = new List<User>();
    }
}
