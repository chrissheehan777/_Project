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
    public class BidAccessor
    {
        public static List<Bid> FetchBid()
        {
            var bidlist = new List<Bid>();

            var conn = DBConnection.GetDBConnection();

            string query = @"select c.businessname, c.lastname, b.BidID, b.Description, b.BidDate, b.ExpectedDate, b.CustomerID,  b.EmployeeID, b.Status, b.ContractType, b.contractamount, b.partsmarkup, b.hourlyrate "; 
            query += @"  from bid b inner join customer c on b.customerid = c.customerID";
            //query += @" where ContractType = " + contractType.ToString();
            //query += @" and status= " + status;
            //query += @" order by BidDate desc";

            var cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Bid currentBid = new Bid()
                        {
                            BusinessName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            BidID = reader.GetInt32(2),
                            Description = reader.GetString(3),
                            BidDate = reader.GetDateTime(4),
                            ExpectedDate = reader.GetDateTime(5),
                            CustomerID = reader.GetInt32(6),                            
                            EmployeeID = reader.GetInt32(7),
                            Status = reader.GetString(8),
                            ContractType = reader.GetString(9),
                            ContractAmount = reader.GetDecimal(10),
                            PartsMarkup = reader.GetInt32(11),
                            HourlyRate = reader.GetDecimal(12)

                            //BusinessName = reader.IsDBNull(0) ? null : reader.GetString(0),
                        };

                        bidlist.Add(currentBid);
                    }
                }

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
            return bidlist;
        }

        public static int SetBid(Bid bOld, Bid bNew)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("spupdatebid", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(@"bidid", SqlDbType.Int).Value = bOld.BidID;
            cmd.Parameters.Add(@"biddate", SqlDbType.DateTime).Value = bNew.BidDate.Date;
            cmd.Parameters.Add(@"expecteddate", SqlDbType.DateTime).Value = bNew.ExpectedDate.Date;
            cmd.Parameters.Add(@"CustomerID", SqlDbType.Int).Value = bNew.CustomerID;
            cmd.Parameters.Add(@"Description", SqlDbType.VarChar, 100).Value = bNew.Description;
            cmd.Parameters.Add(@"EmployeeID", SqlDbType.Int).Value = bNew.EmployeeID;
            cmd.Parameters.Add(@"status", SqlDbType.Char, 1).Value = bNew.Status;
            cmd.Parameters.Add(@"contracttype", SqlDbType.Char, 1).Value = bNew.ContractType;
            cmd.Parameters.Add(@"contractamount", SqlDbType.Money).Value = bNew.ContractAmount;
            cmd.Parameters.Add(@"partsmarkup", SqlDbType.Int).Value = bNew.PartsMarkup;
            cmd.Parameters.Add(@"hourlyrate", SqlDbType.Money).Value = bNew.HourlyRate;
            cmd.Parameters.Add(@"Original_biddate", SqlDbType.DateTime).Value = bOld.BidDate;
            cmd.Parameters.Add(@"Original_expecteddate", SqlDbType.DateTime).Value = bOld.ExpectedDate;
            cmd.Parameters.Add(@"Original_CustomerID", SqlDbType.Int).Value = bOld.CustomerID;
            cmd.Parameters.Add(@"Original_Description", SqlDbType.VarChar, 100).Value = bOld.Description;
            cmd.Parameters.Add(@"Original_EmployeeID", SqlDbType.Int).Value = bOld.EmployeeID;
            cmd.Parameters.Add(@"Original_status", SqlDbType.VarChar, 1).Value = bOld.Status;
            cmd.Parameters.Add(@"Original_contracttype", SqlDbType.VarChar, 1).Value = bOld.ContractType;
            cmd.Parameters.Add(@"Original_contractamount", SqlDbType.Money).Value = bOld.ContractAmount;
            cmd.Parameters.Add(@"Original_partsmarkup", SqlDbType.Int).Value = bOld.PartsMarkup;
            cmd.Parameters.Add(@"Original_hourlyrate", SqlDbType.Money).Value = bOld.HourlyRate;

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
            return count;

        }
        public static int NewBid(Bid bNew)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("spinsertbid", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(@"bidDate", SqlDbType.DateTime).Value = bNew.BidDate;
            cmd.Parameters.Add(@"ExpectedDate", SqlDbType.DateTime).Value = bNew.ExpectedDate;
            cmd.Parameters.Add(@"CustomerID", SqlDbType.Int).Value = bNew.CustomerID;
            cmd.Parameters.Add(@"Description", SqlDbType.VarChar, 100).Value = bNew.Description;
            cmd.Parameters.Add(@"EmployeeID", SqlDbType.Int).Value = bNew.EmployeeID;
            cmd.Parameters.Add(@"status", SqlDbType.VarChar, 1).Value = "A";
            cmd.Parameters.Add(@"contracttype", SqlDbType.VarChar, 1).Value = bNew.ContractType;
            cmd.Parameters.Add(@"contractamount", SqlDbType.Money).Value = bNew.ContractAmount;
            cmd.Parameters.Add(@"partsmarkup", SqlDbType.Int).Value = bNew.PartsMarkup;
            cmd.Parameters.Add(@"hourlyrate", SqlDbType.Money).Value = bNew.HourlyRate;
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
        public static int SetBidStatusNewWO(int bidid, string status)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            //spUpdateBidStatus
            var cmd = new SqlCommand("spUpdateBidStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(@"bidID", SqlDbType.Int).Value = bidid;
            cmd.Parameters.Add(@"Status", SqlDbType.Char, 1).Value = status;
            try
            {
                conn.Open();

                count = cmd.ExecuteNonQuery();
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
