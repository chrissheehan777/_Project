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

namespace CMSComputers
{
    /// <summary>
    /// Interaction logic for Workorders.xaml
    /// </summary>
    public partial class Workorders : Window
    {
        Workorder wOld = new Workorder();
        Workorder wNew = new Workorder();
        CustomerManager _customerManager = new CustomerManager();
        EmployeeManager _employeeManager = new EmployeeManager();
        WorkorderManager _workorderManager = new WorkorderManager();
        List<Employee> _employees = new List<Employee>();
        List<ContractType> _contractType = new List<ContractType>();
        List<Customer> _customers = new List<Customer>();
        List<WorkorderStatus> _workorderStatus = new List<WorkorderStatus>();

        public Workorders(Workorder w)
        {
            InitializeComponent();
            wOld = w;
            lblWorkorderID.Content = w.WorkorderID;
            dpWorkorderDate.SelectedDate = DateTime.Parse(w.WorkorderDate.ToString());
            lblBidID.Content = w.BidID.ToString();
            dpExpectedDate.SelectedDate = DateTime.Parse(w.ExpectedDate.ToString());
            txtDescription.Text = w.Description;
            txtContractAmount.Text = w.ContractAmount.ToString();
            txtPartsMarkup.Text = w.PartsMarkup.ToString();
            txtHourlyRate.Text = w.HourlyRate.ToString();

            //bind and populate contract type combobox
            BindContractType();
            this.cmbContractType.SelectedValue = w.ContractType;

            //bind and populate customers combobox
            BindCustomers();
            this.cmbCustID.SelectedValue = w.CustomerID;

            //bind and populate employees combobox
            BindEmployees();
            this.cmbEmployeeID.SelectedValue = w.EmployeeID;

            //bind and populate status comboboxes
            BindWorkorderStatus();
            this.cmbStatus.SelectedValue = w.Status;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            wNew.WorkorderID = wOld.WorkorderID;
            wNew.BidID = wOld.BidID;
            wNew.Description = txtDescription.Text;
            wNew.WorkorderDate = dpWorkorderDate.SelectedDate.Value.Date;
            wNew.ExpectedDate = dpExpectedDate.SelectedDate.Value.Date;
            wNew.CustomerID = Int32.Parse(cmbCustID.SelectedValue.ToString());
            wNew.EmployeeID = Int32.Parse(cmbEmployeeID.SelectedValue.ToString());
            wNew.Status = cmbStatus.SelectedValue.ToString();
            wNew.ContractType = cmbContractType.SelectedValue.ToString();
            //wNew.ContractType = txtContractType.Text;
            wNew.ContractAmount = wOld.ContractAmount; // Decimal.Parse(txtContractAmount.Text);
            wNew.PartsMarkup = Int32.Parse(txtPartsMarkup.Text);
            wNew.HourlyRate = wOld.HourlyRate; // Decimal.Parse(txtHourlyRate.Text);

            try
            {
                int i = WorkorderManager.UpdateWorkorder(wOld, wNew);
                if (i != 0)
                {
                    //MessageBox.Show("Update Succeeded");
                    OnDialogFinished();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Update Failed");
                }

            }
            catch (Exception)
            {
                throw;
                //MessageBox.Show("Update Failed");
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { this.Close(); }

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

        private void BindWorkorderStatus()
        {
            _workorderStatus = _workorderManager.Status;
            this.cmbStatus.ItemsSource = from s in _workorderStatus
                                         orderby s.Description
                                         select s;
            this.cmbStatus.DisplayMemberPath = "Description";
            this.cmbStatus.SelectedValuePath = "StatusID";
        }

        public event EventHandler<MainWindow.WindowEventArgs> DialogFinished;

        public void OnDialogFinished()
        {
            if (DialogFinished != null)
            {
                DialogFinished(this, new MainWindow.WindowEventArgs());
            }
        }

        private void btnInsertHours_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInsertParts_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
