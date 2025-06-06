using Home.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.Service
{
    public class DeferredReceipt
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<Sales.Product_Sale> Products { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
