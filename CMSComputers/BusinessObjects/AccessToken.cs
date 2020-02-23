using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public sealed class AccessToken : Employee
    {
        public List<Role> Roles { get; private set; }
        public AccessToken() { }
        public AccessToken(Employee emp, List<Role> roles)
        {
            if (emp == null || roles == null || roles.Count == 0 || !emp.Active)
            {
                throw new ApplicationException("Invalid Employee");
            }
            base.EmployeeID = emp.EmployeeID;
            base.FirstName = emp.FirstName;
            base.LastName = emp.LastName;
            base.LocalPhone = emp.LocalPhone;
            base.EmailAddress = emp.EmailAddress;
            base.UserName = emp.UserName;
            base.Active = emp.Active;

            Roles = roles;
        }
    }
}
