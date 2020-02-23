using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessObjects;

namespace BusinessLogic
{
    public class CustomerManager
    {
        public List<Customer> GetCustomerList()
        {
            try
            {
                var customerlist = CustomerAccessor.FetchCustomerList();

                if (customerlist.Count > 0)
                {
                    return customerlist;
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
        // get contracttypes for drop down lists
        public List<ContractType> GetContractTypeList()
        {
            try
            {
                var contractTypeList = CustomerAccessor.FetchContractTypeList();

                if (contractTypeList.Count > 0)
                {
                    return contractTypeList;
                }
                else
                {
                    throw new ApplicationException("No records found");                   
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static int UpdateCustomer(Customer cold, Customer cnew)
        {
            try
            {
                int i = CustomerAccessor.SetCustomer(cold, cnew);
                if (i != 0)
                {
                    return i;
                }
                else
                {
                    throw new ApplicationException("Record not updated! An error occured.");
                    //throw new ApplicationException(i.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static int AddCustomer(Customer cnew)
        {
            try
            {
                int i = CustomerAccessor.NewCustomer(cnew);
                if (i != 0)
                {
                    return i;
                }
                else
                {
                    throw new ApplicationException("Record not updated! An error occured.");
                    //throw new ApplicationException(i.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
