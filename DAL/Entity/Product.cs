using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOProject.DAL.Entity
{
    public class Product
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = null!;
        public String Description { get; set; } = null!;
        public String Price { get; set; } = null!;
        public String Quantity { get; set; } = null!;
        public String DeleteDt { get; set; } = null!;
    }
}
