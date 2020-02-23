using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class WorkorderHours
    {
        public int WorkorderID { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeID { get; set; }
        public int HoursBid { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
