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
    public class CustomerAccessor
    {
        public static List<Customer> FetchCustomerList()
        {
            var customerlist = new List<Customer>();

            //get connection to db
            var conn = DBConnection.GetDBConnection();

            //create a query to send thru conn
            string query = @"SELECT [CustomerID]
                  ,[FirstName]
                  ,[LastName]
                  ,[BusinessName]
                  ,[Address]
                  ,[City]
                  ,[State]
                  ,[Zip]
                  ,[LocalPhone]
                  ,[EmailAddress]
                  ,[Active]
              FROM [dbo].[Customer] order by businessname, lastname ";

            var cmd = new SqlCommand(query, conn);

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
                        Customer currentCustomer = new Customer()
                        {
                            CustomerID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            BusinessName = reader.GetString(3),
                            Address = reader.GetString(4),
                            City = reader.GetString(5),
                            State = reader.GetString(6),
                            Zip = reader.GetString(7),
                            LocalPhone = reader.GetString(8),
                            EmailAddress = reader.GetString(9),
                            Active = reader.GetBoolean(10)
                        };

                        customerlist.Add(currentCustomer);
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
            return customerlist;
        }

        public static List<ContractType> FetchContractTypeList()
        {
            var contractTypeList = new List<ContractType>();

            var conn = DBConnection.GetDBConnection();

            string query = "select contracttypeid, contracttypename, status from contracttype where status = 'A'";

            var cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContractType ct = new ContractType()
                        {
                            ContractTypeID = reader.GetString(0),
                            ContractTypeName = reader.GetString(1),
                            Status = reader.GetString(2)
                        };
                        contractTypeList.Add(ct);
                    }
                }
                else
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return contractTypeList;
        }


        public static int SetCustomer(Customer cold, Customer cnew)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("spupdatecustomer", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(@"CustomerID", SqlDbType.Int).Value = cold.CustomerID;
            cmd.Parameters.Add(@"FirstName", SqlDbType.VarChar, 50).Value = cnew.FirstName;
            cmd.Parameters.Add(@"LastName", SqlDbType.VarChar, 100).Value = cnew.LastName;
            cmd.Parameters.Add(@"BusinessName", SqlDbType.VarChar, 100).Value = cnew.BusinessName;
            cmd.Parameters.Add(@"Address", SqlDbType.VarChar, 100).Value = cnew.Address;
            cmd.Parameters.Add(@"City", SqlDbType.VarChar, 30).Value = cnew.City;
            cmd.Parameters.Add(@"State", SqlDbType.VarChar, 2).Value = cnew.State;
            cmd.Parameters.Add(@"Zip", SqlDbType.VarChar, 9).Value = cnew.Zip;
            cmd.Parameters.Add(@"LocalPhone", SqlDbType.VarChar, 10).Value = cnew.LocalPhone;
            cmd.Parameters.Add(@"EmailAddress", SqlDbType.VarChar, 100).Value = cnew.EmailAddress;
            cmd.Parameters.Add(@"Active", SqlDbType.Bit).Value = cnew.Active;
            cmd.Parameters.Add(@"original_FirstName", SqlDbType.VarChar, 50).Value = cold.FirstName;
            cmd.Parameters.Add(@"original_LastName", SqlDbType.VarChar, 100).Value = cold.LastName;
            cmd.Parameters.Add(@"original_BusinessName", SqlDbType.VarChar, 100).Value = cold.BusinessName;
            cmd.Parameters.Add(@"original_Address", SqlDbType.VarChar, 100).Value = cold.Address;
            cmd.Parameters.Add(@"original_City", SqlDbType.VarChar, 30).Value = cold.City;
            cmd.Parameters.Add(@"original_State", SqlDbType.VarChar, 2).Value = cold.State;
            cmd.Parameters.Add(@"original_Zip", SqlDbType.VarChar, 9).Value = cold.Zip;
            cmd.Parameters.Add(@"original_LocalPhone", SqlDbType.VarChar, 10).Value = cold.LocalPhone;
            cmd.Parameters.Add(@"original_EmailAddress", SqlDbType.VarChar, 100).Value = cold.EmailAddress;
            cmd.Parameters.Add(@"original_Active", SqlDbType.Bit).Value = cold.Active;

            try
            {
                conn.Open();

                count = cmd.ExecuteNonQuery();
            }
            catch(SqlException s)
            {
                throw new ApplicationException(s.Message);
            }
            catch (Exception)
            {
                throw;
            }
            finally { conn.Close(); }
            return count;
        }
        public static int NewCustomer(Customer cnew)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("spInsertCustomer", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add(@"CustomerID", SqlDbType.Int).Value = cnew.CustomerID;
            cmd.Parameters.Add(@"FirstName", SqlDbType.VarChar, 50).Value = cnew.FirstName;
            cmd.Parameters.Add(@"LastName", SqlDbType.VarChar, 100).Value = cnew.LastName;
            cmd.Parameters.Add(@"BusinessName", SqlDbType.VarChar, 100).Value = cnew.BusinessName;
            cmd.Parameters.Add(@"Address", SqlDbType.VarChar, 100).Value = cnew.Address;
            cmd.Parameters.Add(@"City", SqlDbType.VarChar, 30).Value = cnew.City;
            cmd.Parameters.Add(@"State", SqlDbType.VarChar, 2).Value = cnew.State;
            cmd.Parameters.Add(@"Zip", SqlDbType.VarChar, 9).Value = cnew.Zip;
            cmd.Parameters.Add(@"LocalPhone", SqlDbType.VarChar, 10).Value = cnew.LocalPhone;
            cmd.Parameters.Add(@"EmailAddress", SqlDbType.VarChar, 100).Value = cnew.EmailAddress;
            cmd.Parameters.Add(@"Active", SqlDbType.Bit).Value = cnew.Active;
            
            try
            {
                conn.Open();

                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException s)
            {
                throw new ApplicationException(s.Message);
            }
            catch (Exception)
            {
                throw;
            }
            finally { conn.Close(); }
            return count;
        }
    }
}
