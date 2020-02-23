using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessObjects;

namespace BusinessLogic
{
    public class BidManager
    {
        public List<Bid> GetBidList()
        {
            try
            {
                var bidList = BidAccessor.FetchBid();

                if (bidList.Count > 0)
                {
                    return bidList;
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
        public static int UpdateBid(Bid bOld, Bid bNew)
        {
            try
            {
                int i = BidAccessor.SetBid(bOld, bNew);
                if (i != 0)
                {
                    return i;
                }
                else
                {
                    throw new ApplicationException("Record not updated! An error occured.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static int UpdateBidStatusNewWO(int bidid, string status)
        {
            try
            {
                int i = BidAccessor.SetBidStatusNewWO(bidid, status);
                if (i != 0)
                {
                    return i;
                }
                else
                {
                    throw new ApplicationException("Record not updated! An error occured.");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static int AddBid(Bid bNew)
        {
            try
            {
                int i = BidAccessor.NewBid(bNew);
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
        private List<BidStatus> _status;
        public List<BidStatus> Status
        {
            get
            {
                if (_status == null)
                {
                    _status = new List<BidStatus>();

                    _status.Add(new BidStatus()
                    {
                        StatusID = "A",
                        Description = "Active Bid"
                    });

                    _status.Add(new BidStatus()
                    {
                        StatusID = "L",
                        Description = "Bid Lost"
                    });

                    _status.Add(new BidStatus()
                    {
                        StatusID = "I",
                        Description = "Bid Inctive"
                    });

                    _status.Add(new BidStatus()
                    {
                        StatusID = "W",
                        Description = "Workorder Created"
                    });
                }
                return _status;
            }            
        }
    }
}
