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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        public bool PassValid;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string hash;
            bool passwordIsValid;
            hash = HashMethods.HashSha256(txtPassword.Password.ToString());
            var em = new EmployeeManager();
            passwordIsValid = em.CheckEmployeePassword(txtUserName.Text.ToString(), hash);
            //
            txtPassword.Password = passwordIsValid.ToString();
            PassValid = passwordIsValid;
            if (PassValid)
            {
                OnDialogFinished();
                this.Close();
                //this.txtPassword.Text = PassValid.ToString();
            }
                
            else lblUsername.Content = "fail";
        }
        public event EventHandler<MainWindow.WindowEventArgs> DialogFinished;
        public void OnDialogFinished()
        {
            if (DialogFinished != null)
                DialogFinished(this, new MainWindow.WindowEventArgs(txtUserName.Text.ToString(), PassValid));
        }
    }
}
