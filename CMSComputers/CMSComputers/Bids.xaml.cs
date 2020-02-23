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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLogic;
using BusinessObjects;

namespace CMSComputers
{
    /// <summary>
    /// Interaction logic for Bids.xaml
    /// </summary>
    public partial class Bids : Window
    {
        Bid _bidOld = new Bid();
        Bid _bidNew = new Bid();
        Workorder workorderNew = new Workorder();
        CustomerManager _customerManager = new CustomerManager();
        EmployeeManager _employeeManager = new EmployeeManager();
        BidManager _bidManager = new BidManager();
        List<ContractType> _contractType = new List<ContractType>();
        List<Customer> _customers = new List<Customer>();
        List<Employee> _employees = new List<Employee>();
        List<BidStatus> _bidStatus = new List<BidStatus>();

        String AddEdit;

        public Bids(Bid b)
        {
            _bidOld = b;
            InitializeComponent();

            //show bid id
            lblBidID.Visibility = Visibility.Visible;
            lblBidID2.Visibility = Visibility.Visible;

            //denotes this is an edit screen
            AddEdit = "edit";

            //populate textbox and datepicker fields
            lblBidID2.Content = b.BidID.ToString();
            txtDescription.Text = b.Description.ToString();
            dpBidDate.SelectedDate = DateTime.Parse(b.BidDate.ToString());
            dpExpectedDate.SelectedDate = b.ExpectedDate;
            txtDescription.Text = b.Description;
            txtContractAmount.Text = b.ContractAmount.ToString("N2") ;
            txtPartsMarkup.Text = b.PartsMarkup.ToString();
            txtHourlyRate.Text = b.HourlyRate.ToString("N2");

            //bind and populate status comboboxes
            BindBidStatus();
            this.cmbStatus.SelectedValue = b.Status;
            if (_bidOld.Status == "W")
            {
                cmbStatus.IsHitTestVisible = false;
                cmbStatus.Focusable = false;
            }

            //bind and populate contract type combobox
            BindContractType();
            this.cmbContractType.SelectedValue = b.ContractType;

            //bind and populate customers combobox
            BindCustomers();
            this.cmbCustID.SelectedValue = b.CustomerID;

            //bind and populate employees combobox
            BindEmployees();
            this.cmbEmployeeID.SelectedValue = b.EmployeeID;
        }

        public Bids()
        {
            InitializeComponent();
            //denotes this is an add screen
            AddEdit = "add";

            //set contract amount combobox binding
            BindContractType();
            //set default contract type
            cmbContractType.SelectedValue = "N";

            //set customer combobox binding
            BindCustomers();
            //set default customer
            cmbCustID.SelectedIndex = 0;

            //set default bid date
            this.dpBidDate.SelectedDate = DateTime.Now.Date;

            //set default expected date
            this.dpExpectedDate.SelectedDate = DateTime.Now.Date;

            //set employee combobox binding
            BindEmployees();

            //set default employee
            cmbEmployeeID.SelectedIndex = 0;

            //bind and populate status comboboxes
            BindBidStatusNewBid();
            this.cmbStatus.SelectedValue = "A";

            //hide bidID labels, there is no bid ID for a new bid
            lblBidID.Visibility = Visibility.Hidden;
            lblBidID2.Visibility = Visibility.Hidden;
        }
        private void BindBidStatusNewBid()
        {
            _bidStatus = _bidManager.Status;
            this.cmbStatus.ItemsSource = from s in _bidStatus
                                         where s.StatusID == "A"
                                         orderby s.Description
                                         select s;
            this.cmbStatus.DisplayMemberPath = "Description";
            this.cmbStatus.SelectedValuePath = "StatusID"; 
        }
        private void BindBidStatus()
        {
            _bidStatus = _bidManager.Status;
            this.cmbStatus.ItemsSource = from s in _bidStatus
                                         orderby s.Description
                                         select s;
            this.cmbStatus.DisplayMemberPath = "Description";
            this.cmbStatus.SelectedValuePath = "StatusID"; 
        }
        private void BindContractType()
        {
            //set local contract type list, get from database 
            _contractType = _customerManager.GetContractTypeList();
            //bind contract amount combobox
            this.cmbContractType.ItemsSource = from c in _contractType
                                               orderby c.ContractTypeName
                                               select c;
            this.cmbContractType.DisplayMemberPath = "ContractTypeName";
            this.cmbContractType.SelectedValuePath = "ContractTypeID";
        }
        private void BindCustomers()
        {
            //set local contract type list, get from database 
            _customers = _customerManager.GetCustomerList();
            //bind customer combobox
            this.cmbCustID.ItemsSource = from c in _customers
                                        orderby c.BusinessName
                                        select c;
            this.cmbCustID.DisplayMemberPath = "BusinessName";
            this.cmbCustID.SelectedValuePath = "CustomerID";
        }
        private void BindEmployees()
        {
            //set local employee list, get from database 
            _employees = _employeeManager.GetEmployeeList(Active.active);
            //bind employee combobox
            this.cmbEmployeeID.ItemsSource = from e in _employees
                                         orderby e.LastName, e.FirstName
                                         select e;
            this.cmbEmployeeID.DisplayMemberPath = "FullName";
            this.cmbEmployeeID.SelectedValuePath = "EmployeeID";
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _bidNew.BidID = _bidOld.BidID;
            _bidNew.Description = txtDescription.Text;
            _bidNew.BidDate = dpBidDate.SelectedDate.Value.Date;
            _bidNew.ExpectedDate = dpExpectedDate.SelectedDate.Value.Date;
            _bidNew.CustomerID = int.Parse(cmbCustID.SelectedValue.ToString());
            _bidNew.EmployeeID = int.Parse(cmbEmployeeID.SelectedValue.ToString());
            _bidNew.Status = cmbStatus.SelectedValue.ToString();
            _bidNew.ContractType = cmbContractType.SelectedValue.ToString();
            _bidNew.ContractAmount = Decimal.Parse(txtContractAmount.Text);
            _bidNew.PartsMarkup = Int32.Parse(txtPartsMarkup.Text);
            _bidNew.HourlyRate = Decimal.Parse(txtHourlyRate.Text);

            if (AddEdit == "edit")
            {
                try
                {
                    int i = BidManager.UpdateBid(_bidOld, _bidNew);
                    if (i != 0)
                    {
                        OnDialogFinished();
                        this.Close();
                    }
                }
                catch (Exception)
                {
                    throw;
                    //MessageBox.Show("Update Failed");
                }
            }
            else if (AddEdit == "add")
            {
                try
                {
                    int i = BidManager.AddBid(_bidNew);
                    if (i != 0)
                    {
                        //MessageBox.Show("New Bid Insert Succeeded");
                        OnDialogFinished();
                        this.Close();
                    }
                }
                catch (Exception)
                {
                    throw;
                    //MessageBox.Show("Insert Failed");
                }
            }
        }
        private void btnCreateWO_Click(object sender, RoutedEventArgs e)
        {
            
            int bidID = Int32.Parse(lblBidID2.Content.ToString());
            string bidStatus = _bidOld.Status;
            if (bidStatus == "W")
            {
                MessageBox.Show("Workorder already created with this Bid ID");
            }
            else
            {
                workorderNew.WorkorderDate = DateTime.Now;
                workorderNew.BidID = _bidOld.BidID;
                workorderNew.ExpectedDate = dpExpectedDate.SelectedDate.Value.Date;
                workorderNew.CustomerID = Int32.Parse(cmbCustID.SelectedValue.ToString());
                workorderNew.Description = txtDescription.Text;
                workorderNew.EmployeeID = Int32.Parse(cmbEmployeeID.SelectedValue.ToString());
                workorderNew.Status = "A";
                workorderNew.ContractType = cmbContractType.SelectedValue.ToString();
                workorderNew.ContractAmount = Decimal.Parse(txtContractAmount.Text);
                workorderNew.PartsMarkup = Int32.Parse(txtPartsMarkup.Text);
                workorderNew.HourlyRate = Decimal.Parse(txtHourlyRate.Text);

                try
                {
                    int i = WorkorderManager.AddWorkorder(workorderNew);
                    int j = BidManager.UpdateBidStatusNewWO(bidID, "W");
                    
                    if (i != 0 && j != 0)
                    {
                        MessageBox.Show("New Workorder Insert Succeeded");
                        OnDialogFinished();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong: " + i + ", " +j);
                            
                    }
                }
                catch (Exception)
                {
                    throw;
                    //MessageBox.Show("Insert Failed");
                }
            }            
        }
        public event EventHandler<MainWindow.WindowEventArgs> DialogFinished;
        public void OnDialogFinished()
        {
            if (DialogFinished != null)
            {
                DialogFinished(this, new MainWindow.WindowEventArgs());
            }
        }

        
    }
}
