using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessLogic;
using BusinessObjects;
using System.IO;

namespace CMSComputers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EmployeeManager myEmployeeManager = new EmployeeManager();
        CustomerManager myCustomerManager = new CustomerManager();
        BidManager myBidManager = new BidManager();
        WorkorderManager myWorkorderManager = new WorkorderManager();
        InvoiceManager myInvoiceManager = new InvoiceManager();
        private string User;
        private bool Auth = false;
        List<BidStatus> _bidStatus = new List<BidStatus>();

        public MainWindow()
        {
            InitializeComponent();
            lblStatusDate.Content = DateTime.Today.ToShortDateString();
            disableAllTabs();
        }
        private void BindBidStatus()
        {
            _bidStatus = myBidManager.Status;
            this.cmbStatus.ItemsSource = from s in _bidStatus
                                         orderby s.Description
                                         select new { s.StatusID, s.Description };
            this.cmbStatus.DisplayMemberPath = "Description";
            this.cmbStatus.SelectedValuePath = "StatusID";
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = new Login();
            login.DialogFinished += new EventHandler<WindowEventArgs>(login_DialogFinished);
            login.Show();
        }

        private void btnNewBid_Click(object sender, RoutedEventArgs e)
        {
            var editBid = new Bids();
            editBid.DialogFinished += new EventHandler<WindowEventArgs>(bid_DialogFinished);
            editBid.Show();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            disableAllTabs();
            User = null;
            Auth = false;
            this.lblLoginMessage.Content = "Not Logged In";
            lblStatusMessage.Content = "Logged Out";
        }

        private void btnNewCust_Click(object sender, RoutedEventArgs e)
        {
            var editCust = new EditCustomer();
            editCust.DialogFinished += new EventHandler<WindowEventArgs>(customer_DialogFinished);
            editCust.Show();
        }

        private void cmbActive_sc(object sender, RoutedEventArgs e)
        {            
            PopulateCustomerGrid();
        }

        void disableAllTabs()
        {
            this.tabCustomer.Visibility = Visibility.Hidden;
            this.tabEmployee.Visibility = Visibility.Hidden;
            this.tabAdmin.Visibility = Visibility.Hidden;
            this.tabBid.Visibility = Visibility.Hidden;
            this.tabWorkorder.Visibility = Visibility.Hidden;
            this.tabInvoice.Visibility = Visibility.Hidden;
            this.btnNewCustomer.Visibility = Visibility.Hidden;
            this.gridBid.Visibility = Visibility.Hidden;
            this.gridCustomers.Visibility = Visibility.Hidden;
            this.gridWorkorder.Visibility = Visibility.Hidden;
            this.gridInvoice.Visibility = Visibility.Hidden;
            this.gridEmployees.Visibility = Visibility.Hidden;
        }

        private void gridBid_doubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridBid.SelectedItem == null) return;
            else
            {
                var selectedBid = gridBid.SelectedItem as Bid;
                var bid = new Bids(selectedBid);
                bid.DialogFinished += new EventHandler<WindowEventArgs>(bid_DialogFinished);
                bid.Show();
            }
        }
        private void gridCustomers_doubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridCustomers.SelectedItem == null) return;
            else
            {
                var selectedCustomer = gridCustomers.SelectedItem as Customer;
                var EditCust = new EditCustomer(selectedCustomer);
                EditCust.DialogFinished += new EventHandler<WindowEventArgs>(customer_DialogFinished);
                EditCust.Show();
            }
        }
        private void gridWorkorders_doubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridWorkorder.SelectedItem == null) return;
            else
            {
                var selectedWorkorders = gridWorkorder.SelectedItem as Workorder;
                var wo = new Workorders(selectedWorkorders);
                wo.DialogFinished += new EventHandler<WindowEventArgs>(workorder_DialogFinished);
                wo.Show();
            }
        }
        void workorder_DialogFinished(object sender, WindowEventArgs e)
        {
            //PopulateBidGrid();
            PopulateWorkorderGrid();
            
        }
        //gridWorkorders_doubleClick
        void bid_DialogFinished(object sender, WindowEventArgs e)
        {
            PopulateBidGrid(this.cmbStatus.SelectedValue.ToString());
            PopulateWorkorderGrid();
        }
        void customer_DialogFinished(object sender, WindowEventArgs e)
        {
            PopulateCustomerGrid();
        }
        void login_DialogFinished(object sender, WindowEventArgs e)
        {
            if (e.Authenticated)
            {
                User = e.Username;
                Auth = e.Authenticated;
                lblLoginMessage.Content = "Logged in as " + e.Username;
                lblStatusMessage.Content = "Logged in as " + e.Username;
                var em = new EmployeeManager();
                var rolelist = em.GetEmployeeRoles(e.Username);

                disableAllTabs();

                foreach (Role r in rolelist)
                {
                    if (r.RoleName == "Administrator")
                    {
                        //this.tabAdmin.Visibility = Visibility.Visible;
                        this.tabEmployee.Visibility = Visibility.Visible;
                        this.gridEmployees.Visibility = Visibility.Visible;
                        PopulateEmployeeGrid();
                    }
                    if (r.RoleName == "User" || r.RoleName == "Administrator")
                    {
                        this.tabCustomer.Visibility = Visibility.Visible;
                        this.gridCustomers.Visibility = Visibility.Visible;
                        this.btnNewCustomer.Visibility = Visibility.Visible;
                        PopulateCustomerGrid();
                        this.tabCustomer.Focus();
                        this.tabBid.Visibility = Visibility.Visible;
                        this.gridBid.Visibility = Visibility.Visible;
                        BindBidStatus();
                        
                        this.tabWorkorder.Visibility = Visibility.Visible;
                        this.gridWorkorder.Visibility = Visibility.Visible;
                        PopulateWorkorderGrid();
                        this.tabInvoice.Visibility = Visibility.Visible;
                        this.gridInvoice.Visibility = Visibility.Visible;
                        populateInvoiceGrid();
                    }                    
                }
            }
            else
            {
                lblStatusMessage.Content = "Login Failed";
                lblLoginMessage.Content = "Login Failed";
            }
        }
        private void PopulateBidGrid(string statusID)
        {
            try
            {
                var bids = myBidManager.GetBidList();
                gridBid.ItemsSource = from b in bids
                                      where b.Status == statusID
                                      //select new { b.BidDate, b.BidID, b.BusinessName, b.Description, b.ContractAmount, b.ContractType, b.Status };
                                      select b;
                this.lblStatusMessage.Content = "Populated Bid grid.";
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void PopulateCustomerGrid()
        {
            try
            {
                var customers = myCustomerManager.GetCustomerList();
                gridCustomers.ItemsSource = customers;
                this.lblStatusMessage.Content = "Populated customer grid.";
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void PopulateEmployeeGrid()
        {
            try
            {
                var employees = myEmployeeManager.GetEmployeeList(Active.active);
                gridEmployees.ItemsSource = employees;
                this.lblStatusMessage.Content = "Populated employee grid.";
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void populateInvoiceGrid()
        {
            var invoices = myInvoiceManager.GetInvoiceList("C");
            gridInvoice.ItemsSource = invoices;
            this.lblStatusMessage.Content = "Populated Invoice grid.";
        }
        private void PopulateWorkorderGrid()
        {
            
                var workorders = myWorkorderManager.GetWorkorderList("C", "A");
                gridWorkorder.ItemsSource = workorders;
                this.lblStatusMessage.Content = "Populated Workorder grid.";
           
        }
        public class WindowEventArgs : EventArgs
        {
            //private readonly string username;
            //private readonly bool authenticated;
            public string Username { get; private set; }
            public bool Authenticated { get; private set; }
            public WindowEventArgs() { }
            public WindowEventArgs(string username, bool authenticated)
            {
                this.Username = username;
                this.Authenticated = authenticated;
            }
        }
        private void cmbStatus_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateBidGrid(cmbStatus.SelectedValue.ToString());
        }

        private void MAin_Loaded(object sender, RoutedEventArgs e)
        {
            this.cmbStatus.SelectedValue = "A";
        }
    }
            
}
