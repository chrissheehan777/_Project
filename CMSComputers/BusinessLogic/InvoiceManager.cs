using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessObjects;
using System.Data.SqlClient;

namespace BusinessLogic
{
    public class InvoiceManager
    {
        public List<Invoice> GetInvoiceList(string contractType)
        {
            try
            {
                var invoiceList = InvoiceAccessor.FetchInvoice(contractType);

                if (invoiceList.Count > 0)
                {
                    return invoiceList;
                }
                else
                {
                    throw new ApplicationException("No records found");
                }
            }
            catch (Exception)
            {
                return null;
                //throw ;
            }
        }
    }
}
