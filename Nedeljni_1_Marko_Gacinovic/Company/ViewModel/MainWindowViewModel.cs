using Company.Commands;
using Company.Models;
using Company.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Company.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        MainWindow main;

        // properties for username and password
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }

        }

        private string userPassword;
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                userPassword = value;
                OnPropertyChanged("UserPassword");
            }
        }

        private tblManager manager;
        public tblManager Manager
        {
            get { return manager; }
            set { manager = value; OnPropertyChanged("Manager"); }
        }



        // constructor
        public MainWindowViewModel(MainWindow mainOpen)
        {            
            main = mainOpen;
        }

        // command for the login button
        private ICommand logIn;
        public ICommand LogIn
        {
            get
            {
                if (logIn == null)
                {
                    logIn = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return logIn;
            }
        }

        private bool CanSaveExecute()
        {
            return true;
        }

        /// <summary>
        /// method for checking username and password and opening the windows
        /// </summary>
        private void SaveExecute()
        {
            string adminType = AdminType(Username, UserPassword);

            if (username == "WPFMaster" && userPassword == "WPFAccess")
            {
                MasterView master = new MasterView();
                master.ShowDialog();
            }
            else if (adminType == "TEAM")
            {
                TeamAdminView team = new TeamAdminView();
                team.ShowDialog();
            }
            else if (adminType == "SYSTEM")
            {
                SystemAdminView system = new SystemAdminView();
                system.ShowDialog();
            }
            else if (adminType == "LOCAL")
            {
                LocalAdminView local = new LocalAdminView();
                local.ShowDialog();
            }
            else if (IsManager(username,userPassword))
            {
                try
                {
                    using (CompanyDBEntities context = new CompanyDBEntities())
                    {
                        tblUser user = (from x in context.tblUsers where x.Username == username select x).First();
                        manager = (from y in context.tblManagers where y.UserID == user.UserID select y).First();

                        ManagerView managerView = new ManagerView(manager);
                        managerView.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Wrong username or password, please try again.");
            }
        }

        // command for closing the window
        private ICommand logout;
        public ICommand Logout
        {
            get
            {
                if (logout == null)
                {
                    logout = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return logout;
            }
        }

        /// <summary>
        /// method for closing the window
        /// </summary>
        private void CloseExecute()
        {
            try
            {
                main.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private bool CanCloseExecute()
        {
            return true;
        }

        private ICommand createEmployee;
        public ICommand CreateEmployee
        {
            get
            {
                if (createEmployee == null)
                {
                    createEmployee = new RelayCommand(param => CreateEmployeeExecute(), param => CanCreateEmployeeExecute());
                }
                return createEmployee;
            }
            
        }

        private bool CanCreateEmployeeExecute()
        {
            return true;
        }

        private void CreateEmployeeExecute()
        {
            CreateEmployeeView employee = new CreateEmployeeView();
            employee.ShowDialog();
        }

        private ICommand createManager;
        public ICommand CreateManager
        {
            get
            {
                if (createManager == null)
                {
                    createManager = new RelayCommand(param => CreateManagerExecute(), param => CanCreateManagerExecute());
                }
                return createManager;
            }

        }

        private bool CanCreateManagerExecute()
        {
            return true;
        }

        private void CreateManagerExecute()
        {
            CreateManagerView manager = new CreateManagerView();
            manager.ShowDialog();
        }

        private string AdminType(string username, string password)
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {                    
                    tblUser user = (from x in context.tblUsers where x.Username == username && x.UserPassword == password select x).First();
                    int id = user.UserID;
                    tblAdmin admin = (from y in context.tblAdmins where y.UserID == id select y).First();
                    string type = admin.AdminType;
                    return type;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        private bool IsManager(string username, string password)
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {
                    tblUser user = (from x in context.tblUsers where x.Username == username && x.UserPassword == password select x).First();
                    int id = user.UserID;
                    tblManager manager = (from y in context.tblManagers where y.UserID == id select y).First();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return false;
            }
        }
    }
}
