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
    /// Interaction logic for EditCustomer.xaml
    /// </summary>
    public partial class EditCustomer : Window
    {
        Customer customer = new Customer();
        Customer customerNew = new Customer();
        String AddEdit;
        //string authenticated;

        public EditCustomer(Customer c)
        {
            
            customer = c;
            AddEdit = "edit";
            InitializeComponent();
            lblCustID.Visibility = Visibility.Visible;
            lblCustomerID.Visibility = Visibility.Visible;
            lblCustomerID.Content = c.CustomerID.ToString();
            txtBusinessName.Text = c.BusinessName.ToString();
            txtFirstName.Text = c.FirstName.ToString();
            txtLastName.Text = c.LastName.ToString();
            txtAddress.Text = c.Address.ToString();
            txtCity.Text = c.City.ToString();
            txtState.Text = c.State.ToString();
            txtZip.Text = c.Zip.ToString();
            txtPhone.Text = c.LocalPhone.ToString();
            txtEmail.Text = c.EmailAddress.ToString();
            chkActive.IsChecked = c.Active;
        }
        public EditCustomer()
        {
            InitializeComponent();
            lblCustID.Visibility = Visibility.Hidden;
            lblCustomerID.Visibility = Visibility.Hidden;
            AddEdit = "add";
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            customerNew.CustomerID = customer.CustomerID;
            customerNew.FirstName = txtFirstName.Text.ToString();
            customerNew.LastName = txtLastName.Text.ToString();
            customerNew.BusinessName = txtBusinessName.Text.ToString();
            customerNew.Address = txtAddress.Text.ToString();
            customerNew.City = txtCity.Text.ToString();
            customerNew.State = txtState.Text.ToString();
            customerNew.Zip = txtZip.Text.ToString();
            customerNew.LocalPhone = txtPhone.Text.ToString();
            customerNew.EmailAddress = txtEmail.Text.ToString();
            customerNew.Active = chkActive.IsChecked.Value;

            if (AddEdit == "edit")
            {
                try
                {
                    int i = CustomerManager.UpdateCustomer(customer, customerNew);
                    if (i != 0)
                    {
                        MessageBox.Show("Update Succeeded");
                        OnDialogFinished();
                        this.Close();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Update Failed");
                }
            }
            else if (AddEdit == "add")
            {
                try
                {
                    int i = CustomerManager.AddCustomer(customerNew);
                    if (i != 0)
                    {
                        MessageBox.Show("New Customer Insert Succeeded");
                        OnDialogFinished();
                        this.Close();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert Failed");
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
