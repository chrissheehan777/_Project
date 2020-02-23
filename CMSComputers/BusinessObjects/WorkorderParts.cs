using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class WorkorderParts
    {
        public int WorkorderID { get; set; }
        public DateTime Date { get; set; }
        public string ItemName { get; set; }
        public int EmployeeID { get; set; }
        public int Quantity { get; set; }
        public decimal ItemCost { get; set; }
        public decimal ItemSalePrice { get; set; }

    }
}
