using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessObjects;

namespace BusinessLogic
{
    public class WorkorderManager
    {
        public List<Workorder> GetWorkorderList(string contractType, string status)
        {
            var workorderList = WorkorderAccessor.FetchWorkorder(contractType, status);

            if (workorderList.Count > 0)
            {
                return workorderList;
            }
            else
            {
                return workorderList;
            }

        }
        public static int UpdateWorkorder(Workorder wOld, Workorder wNew)
        {
            try
            {
                int i = WorkorderAccessor.SetWorkorder(wOld, wNew);
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
        public static int AddWorkorder(Workorder wNew)
        {
            try
            {
                int i = WorkorderAccessor.NewWorkorder(wNew);
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
        private List<WorkorderStatus> _status;
        public List<WorkorderStatus> Status
        {
            get
            {
                if (_status == null)
                {
                    _status = new List<WorkorderStatus>();

                    _status.Add(new WorkorderStatus()
                    {
                        StatusID = "A",
                        Description = "Active Workorder"
                    });

                    _status.Add(new WorkorderStatus()
                    {
                        StatusID = "C",
                        Description = "Cancelled Workorder"
                    });

                    _status.Add(new WorkorderStatus()
                    {
                        StatusID = "I",
                        Description = "Invoiced Workorder"
                    }); 
                }
                return _status;
            }
        }
    }
}
