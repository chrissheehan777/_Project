using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class EmployeeManager
    {
        public List<Employee> GetEmployeeList(Active group)
        {
            try
            {
                var employeelist = EmployeeAccessor.FetchEmployeeList(group);

                if (employeelist.Count > 0)
                {
                    return employeelist;
                }
                else
                {
                    throw new ApplicationException("No records found");
                }
            }
            catch (Exception)
            {
                //*** we should sort the possible exceptions and return diff messages for each.
                throw;
            }
        }


        public int GetEmployeeCount(Active group = Active.active)
        {
            //int count = 0;

            try
            {
                return EmployeeAccessor.FetchEmployeeCount(group);

            }
            catch (Exception)
            {

                throw;
            }

            //return count;

        }

        public bool AddNewEmployee(string firstName, string lastName, string localPhone, string emailAddress, string userName, string password)
        {

            try
            {
                var emp = new Employee()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    LocalPhone = localPhone,
                    EmailAddress = emailAddress,
                    UserName = userName,
                    Password = HashMethods.HashSha256(password)
                };
                if (EmployeeAccessor.InsertEmployee(emp) == 1)
                {
                    return true;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public bool ChangeEmployeeEmail(int employeeID, string email)
        {
            if (employeeID < 1000)
            {
                throw new ApplicationException("Invalid EmployeeID");
            }
            if (email.Length > 100)
            {
                throw new ApplicationException("Email Address Too Long");
            }
            try
            {
                if (EmployeeAccessor.UpdateEmployeeEmailAdress(employeeID, email) == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;

        }

        public bool ChangeEmployeePassword(string userName, string newPassword, string oldPassword)
        {
            try
            {
                if (EmployeeAccessor.ChangePassword(userName, newPassword, oldPassword) == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public bool CheckEmployeePassword(string username, string password)
        {
            try
            {
                if (EmployeeAccessor.CheckPassword(username, password))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }


        public List<Role> GetEmployeeRoles(string userName)
        {
            try
            {
                var fetchrole = EmployeeAccessor.FetchRoles(userName);

                if (fetchrole.Count > 0)
                {
                    return fetchrole;
                }
                else
                {
                    throw new ApplicationException("No role records found!");
                }
            }
            catch (Exception)
            {
                //*** we should sort the possible exceptions and return diff messages for each.
                
                throw;
            }
        }
    }
}
