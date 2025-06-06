using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.Service
{
    public class ReceiptItem
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; } 
        public int StorageId { get; set; }
        public int? DiscountId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
