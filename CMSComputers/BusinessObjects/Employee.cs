using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LocalPhone { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public string FullName { get; set; }
        public Employee() { }

        public Employee(int employeeID,
                        string firstName,
                        string lastName,
                        string localPhone,
                        string emailAddress,
                        string userName,
                        string password,
                        bool active,
                        string fullName)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            LocalPhone = localPhone;
            EmailAddress = emailAddress;
            UserName = userName;
            Password = password;
            Active = active;
            FullName = fullName;
        }
    }
}
