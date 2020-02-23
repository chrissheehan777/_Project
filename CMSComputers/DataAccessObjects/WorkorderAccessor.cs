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
    public class WorkorderAccessor
    {
        public List<WorkorderParts> FetchWorkorderParts(int workorderID)
        {
            var WorkorderParts = new List<WorkorderParts>();
            var conn = DBConnection.GetDBConnection();
            string query = @"SELECT [WorkorderID],[Date],[ItemName],[EmployeeID],[Quantity],[ItemCost],[ItemSalePrice] FROM [dbo].[WorkorderItems] where workorderid = " + workorderID;
            var cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        WorkorderParts currentWorkorderParts = new WorkorderParts()
                        {
                            WorkorderID = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            ItemName = reader.GetString(2),
                            EmployeeID = reader.GetInt32(3),
                            Quantity = reader.GetInt32(4),
                            ItemCost = reader.GetDecimal(5),
                            ItemSalePrice = reader.GetDecimal(6)
                        };

                        WorkorderParts.Add(currentWorkorderParts);
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
            conn.Close();
            //list may be empty, if so logic layer will have to deal with it
            return WorkorderParts;
        }
        public List<WorkorderHours> FetchWorkorderHours(int workorderID)
        {
            var WorkorderHours = new List<WorkorderHours>();
            var conn = DBConnection.GetDBConnection();
            string query = @"SELECT [WorkorderID]
                            ,[Date]
                            ,[EmployeeID]
                            ,[HoursBid]
                            ,[PricePerHour]
                            FROM [dbo].[WorkorderHours] where workorderid = " + workorderID;
            var cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        WorkorderHours currentWorkorderHours = new WorkorderHours()
                        {
                            WorkorderID = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            EmployeeID = reader.GetInt32(2),
                            HoursBid = reader.GetInt32(4),
                            PricePerHour = reader.GetDecimal(5)
                        };

                        WorkorderHours.Add(currentWorkorderHours);
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
            conn.Close();
            //list may be empty, if so logic layer will have to deal with it
            return WorkorderHours;
        }
        public static List<Workorder> FetchWorkorder(string contractType, string status)
        {
            var workorderlist = new List<Workorder>();

            var conn = DBConnection.GetDBConnection();

            string query = @"select c.businessname, c.lastname, w.WorkorderID, w.Description, CONVERT(date, w.WorkorderDate) as WorkorderDate, convert(date, w.ExpectedDate) as ExpectedDate , w.CustomerID,  w.EmployeeID, w.WorkOrderStatus, w.ContractType, w.contractamount, w.partsmarkup, w.hourlyrate, w.bidid ";
            query += @"  from workorder w inner join customer c on w.customerid = c.customerID";
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
                        Workorder currentWorkorder = new Workorder()
                        {
                            BusinessName = reader.IsDBNull(0) ? null : reader.GetString(0),
                            LastName = reader.GetString(1),
                            WorkorderID = reader.GetInt32(2),
                            Description = reader.GetString(3),
                            WorkorderDate = reader.GetDateTime(4),
                            ExpectedDate = reader.GetDateTime(5),
                            CustomerID = reader.GetInt32(6),
                            EmployeeID = reader.GetInt32(7),
                            Status = reader.GetString(8),
                            ContractType = reader.IsDBNull(9) ? null : reader.GetString(9),
                            ContractAmount = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10),
                            PartsMarkup = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                            HourlyRate = reader.IsDBNull(12) ? 0 : reader.GetDecimal(12),
                            BidID = reader.GetInt32(13)
                            
                        };

                        workorderlist.Add(currentWorkorder);
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
            conn.Close();
            //list may be empty, if so logic layer will have to deal with it
            return workorderlist;
        }

        public static int NewWorkorder(Workorder wNew)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("spinsertworkorder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(@"workorderdate", SqlDbType.DateTime).Value = wNew.WorkorderDate.Date;
            cmd.Parameters.Add(@"bidid", SqlDbType.Int).Value = wNew.BidID;
            cmd.Parameters.Add(@"expecteddate", SqlDbType.DateTime).Value = wNew.ExpectedDate.Date;
            cmd.Parameters.Add(@"customerid", SqlDbType.Int).Value = wNew.CustomerID;
            cmd.Parameters.Add(@"description", SqlDbType.VarChar, 150).Value = wNew.Description;
            cmd.Parameters.Add(@"employeeid", SqlDbType.Int).Value = wNew.EmployeeID;
            cmd.Parameters.Add(@"workorderstatus", SqlDbType.Char, 1).Value = wNew.Status;
            cmd.Parameters.Add(@"contracttype", SqlDbType.Char, 1).Value = wNew.ContractType;
            cmd.Parameters.Add(@"contractamount", SqlDbType.Money).Value = wNew.ContractAmount;
            cmd.Parameters.Add(@"partsmarkup", SqlDbType.Int).Value = wNew.PartsMarkup;
            cmd.Parameters.Add(@"hourlyrate", SqlDbType.Money).Value = wNew.HourlyRate;
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

        public static int SetWorkorder(Workorder wOld, Workorder wNew)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("spupdateworkorder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(@"workorderid", SqlDbType.Int).Value = wOld.WorkorderID;
            cmd.Parameters.Add(@"workorderdate", SqlDbType.DateTime).Value = wNew.WorkorderDate;
            cmd.Parameters.Add(@"bidid", SqlDbType.Int).Value = wNew.BidID;
            cmd.Parameters.Add(@"expecteddate", SqlDbType.DateTime).Value = wNew.ExpectedDate;
            cmd.Parameters.Add(@"customerid", SqlDbType.Int).Value = wNew.CustomerID;
            cmd.Parameters.Add(@"description", SqlDbType.VarChar, 150).Value = wNew.Description;
            cmd.Parameters.Add(@"employeeid", SqlDbType.Int).Value = wNew.EmployeeID;
            cmd.Parameters.Add(@"workorderstatus", SqlDbType.Char, 1).Value = wNew.Status;
            cmd.Parameters.Add(@"contracttype", SqlDbType.Char, 1).Value = wNew.ContractType;
            cmd.Parameters.Add(@"contractamount", SqlDbType.Money).Value = wNew.ContractAmount;
            cmd.Parameters.Add(@"partsmarkup", SqlDbType.Int).Value = wNew.PartsMarkup;
            cmd.Parameters.Add(@"hourlyrate", SqlDbType.Money).Value = wNew.HourlyRate;
            cmd.Parameters.Add(@"Original_workorderdate", SqlDbType.DateTime).Value = wOld.WorkorderDate;
            cmd.Parameters.Add(@"Original_bidid", SqlDbType.Int).Value = wOld.BidID;
            cmd.Parameters.Add(@"Original_expecteddate", SqlDbType.DateTime).Value = wOld.ExpectedDate;
            cmd.Parameters.Add(@"Original_customerid", SqlDbType.Int).Value = wOld.CustomerID;
            cmd.Parameters.Add(@"Original_description", SqlDbType.VarChar, 150).Value = wOld.Description;
            cmd.Parameters.Add(@"Original_employeeid", SqlDbType.Int).Value = wOld.EmployeeID;
            cmd.Parameters.Add(@"Original_workorderstatus", SqlDbType.Char, 1).Value = wOld.Status;
            cmd.Parameters.Add(@"Original_contracttype", SqlDbType.Char, 1).Value = wOld.ContractType;
            cmd.Parameters.Add(@"Original_contractamount", SqlDbType.Money).Value = wOld.ContractAmount;
            cmd.Parameters.Add(@"Original_partsmarkup", SqlDbType.Int).Value = wOld.PartsMarkup;
            cmd.Parameters.Add(@"Original_hourlyrate", SqlDbType.Money).Value = wOld.HourlyRate;

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
