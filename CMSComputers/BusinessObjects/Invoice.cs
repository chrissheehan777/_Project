using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Invoice
    {
        public int InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BusinessName { get; set; }
        public string LastName { get; set; }
        public int CustomerID { get; set; }
        public string Description { get; set; }
        public int EmployeeID { get; set; }
        public double InvoiceTotal { get; set; }
        public string ContractType { get; set; }

        public Invoice() { }
    }
}
