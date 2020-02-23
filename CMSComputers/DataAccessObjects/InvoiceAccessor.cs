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
    public class InvoiceAccessor
    {
        public static List<Invoice> FetchInvoice(string contractType)
        {
            var invoiceList = new List<Invoice>();

            var conn = DBConnection.GetDBConnection();

            string query = @"select i.invoiceNo, i.invoiceDate, c.businessname, c.lastname, i.customerId, i.description, i.employeeId, i.invoiceTotal, i.contractType ";
            query += @" from invoice i inner join customer c on i.customerid = c.customerID";

            var cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    return null;
                }

                else if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Invoice currentInvoice = new Invoice()
                        {
                            InvoiceNo = reader.GetInt32(0),
                            InvoiceDate = reader.GetDateTime(1),
                            BusinessName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            CustomerID = reader.GetInt32(4),
                            Description = reader.GetString(5),
                            EmployeeID = reader.GetInt32(6),
                            InvoiceTotal = reader.GetDouble(7),
                            ContractType = reader.GetString(8)
                        };
                        invoiceList.Add(currentInvoice);
                    }
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
            return invoiceList;
        }
    }
}
