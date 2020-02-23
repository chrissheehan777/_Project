using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string LocalPhone { get; set; }
        public string EmailAddress { get; set; }
        public bool Active { get; set; }
        public Customer() { }

        public Customer(int customerID,
                        string firstName,
                        string lastName,
                        string businessName,
                        string address,
                        string city, 
                        string state,
                        string zip,
                        string localPhone,
                        string emailAddress,                        
                        bool active)
        {
            CustomerID = customerID;
            FirstName = firstName;
            LastName = lastName;
            BusinessName = businessName;
            Address = address;
            City = city;
            State = state;
            Zip = zip;
            LocalPhone = localPhone;
            EmailAddress = emailAddress;
            Active = active;
        }
    }
}
