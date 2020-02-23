using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessObjects
{

    public class EmployeeAccessor
    {
        public static List<Employee> FetchEmployeeList(Active group = Active.active)
        {
            // create list to hold return data
            var employeelist = new List<Employee>();

            //get connection to db
            var conn = DBConnection.GetDBConnection();

            //create a query to send thru conn
            string query = @"select EmployeeID, FirstName, LastName, LocalPhone, EmailAddress, UserName, " +
                @"Password, Active, lastname + ', ' + firstname as FullName from Employee ";
            if (group == Active.active)
            {
                query += @" where active =1";
            }
            else if (group == Active.inactive)
            {
                query += @" where active =0";
            }
            //both no restrictions
            query += @" order by lastname";
            //create command object
            var cmd = new SqlCommand(query, conn);

            //always use try catch
            try
            {
                conn.Open();
                //execute command return datareader
                SqlDataReader reader = cmd.ExecuteReader();

                //before trying to read, be sure it has data
                if (reader.HasRows)
                {
                    //just need a loop to process reader
                    while (reader.Read())
                    {
                        Employee currentEmployee = new Employee()
                        {
                            EmployeeID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            LocalPhone = reader.GetString(3),
                            EmailAddress = reader.GetString(4),
                            UserName = reader.GetString(5),
                            Password = reader.GetString(6),
                            Active = reader.GetBoolean(7),
                            FullName = reader.GetString(8)
                        };

                        employeelist.Add(currentEmployee);
                    }
                }
                //else
                //{
                //    //possibly throw an exception
                //    var ax = new ApplicationException("No Employees Found");
                //}
            }
            catch (Exception)
            {
                //rethrow, let logic layer sort out
                throw;
            }
            finally
            {
                conn.Close();
            }
            //list may be empty, if so logic layer will have to deal with it
            return employeelist;
        }

        public static int FetchEmployeeCount(Active group = Active.active)
        {
            int count = 0;

            //SCALAR QUERY

            //conn object
            var conn = DBConnection.GetDBConnection();

            //write command text
            string query = @"select count(*) from employees ";

            //include where logic
            if (group == Active.active)
            {
                query += @"where active = 1 ";
            }
            else if (group == Active.inactive)
            {
                query += @"where active = 0 ";
            }


            //create command obj
            var cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();

                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }

            return count;
        }

        public static int InsertEmployee(Employee emp)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();

            string query = @"insert into employees (firstname, lastname, localphone, emailaddress, username, password) " +
                @" values ('" + emp.FirstName + @"', '" + emp.LastName + @"', '" + emp.LocalPhone + @"', '" + emp.EmailAddress +
                @"', '" + emp.UserName + @"', '" + emp.Password + "')";

            var cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }

            return count;
        }

        public static int UpdateEmployeeEmailAdress(int employeeID, string newEmailAddress)
        {
            int rowCount = 0;

            var conn = DBConnection.GetDBConnection();

            //commandtxt

            string cmdText = "sp_Employee_UpdateEmailAddress";
            var cmd = new SqlCommand(cmdText, conn);

            //things change here
            //first set commandtype
            cmd.CommandType = CommandType.StoredProcedure;

            //construct and add params
            //all-in one waY
            cmd.Parameters.Add(new SqlParameter("EmployeeID", SqlDbType.Int)).Value = employeeID;

            //step by step way
            var p = new SqlParameter("EmailAddress", SqlDbType.VarChar, 100);
            p.Value = newEmailAddress;
            cmd.Parameters.Add(p);

            //return (output) params
            var o = new SqlParameter("RowCount", SqlDbType.Int);
            o.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(o);

            try
            {
                conn.Open();
                rowCount = cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {


                throw;
            }
            finally
            {
                conn.Close();
            }

            return rowCount;
        }

        public static bool CheckPassword(string username, string password)
        {

            var conn = DBConnection.GetDBConnection();
            string c;
            int d;
            bool count = false;

            var cmd = new SqlCommand("sp_checkpassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;



            cmd.Parameters.Add(new SqlParameter("username", SqlDbType.VarChar, 20)).Value = username;
            cmd.Parameters.Add(new SqlParameter("password", SqlDbType.VarChar, 128)).Value = password;

            try
            {
                conn.Open();
                c = cmd.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                throw;
            }
            if (Int32.TryParse(c, out d))
            {
                if (d > 0)
                {
                    count = true;
                }
                else
                {
                    count = false;
                }
            }
            else
            {
                count = false;
            }


            return count;
        }

        public static int ChangePassword(string userName, string newPassword, string oldPassword)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();

            var cmd = new SqlCommand("sp_UpdatePassword", conn);
            cmd.Parameters.Add(userName, SqlDbType.VarChar, 20);
            cmd.Parameters.Add(newPassword, SqlDbType.VarChar, 128);
            cmd.Parameters.Add(oldPassword, SqlDbType.VarChar, 128);
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            return count;
        }

        public static List<Role> FetchRoles(string userName)
        {
            var rolesList = new List<Role>();

            var conn = DBConnection.GetDBConnection();

            var cmd = new SqlCommand("sp_GetRoles", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("username", SqlDbType.VarChar, 20)).Value = userName;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Role r = new Role()
                        {
                            RoleName = reader.GetString(0),
                            RoleDescription = reader.GetString(0)
                        };
                        rolesList.Add(r);
                    }
                }
                conn.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return rolesList;
        }

    }
}

