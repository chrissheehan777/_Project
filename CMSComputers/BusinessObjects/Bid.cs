using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Bid
    {
        public string BusinessName { get; set; }
        public string LastName { get; set; }
        public int BidID { get; set; }
        public string Description { get; set; }
        public DateTime BidDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int CustomerID { get; set; }
        
        public int EmployeeID { get; set; }
        public string Status { get; set; }
        public string ContractType { get; set; }
        public decimal ContractAmount { get; set; }
        public int PartsMarkup { get; set; }
        public decimal HourlyRate { get; set; }

        public Bid(){}

    }
}
