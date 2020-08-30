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
    class MasterViewModel : ViewModelBase
    {
        MasterView masterView;

        // properties
        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private tblAdmin admin;
        public tblAdmin Admin
        {
            get { return admin; }
            set { admin = value; OnPropertyChanged("Admin"); }
        }

        // constructor
        public MasterViewModel(MasterView masterViewOpen)
        {
            user = new tblUser();
            admin = new tblAdmin();
            masterView = masterViewOpen;
        }

        // commands
        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }

        /// <summary>
        /// method for Save button disabled
        /// </summary>
        /// <returns></returns>
        private bool CanSaveExecute()
        {
            if (String.IsNullOrEmpty(user.FirstName) || String.IsNullOrEmpty(user.LastName)
                || String.IsNullOrEmpty(user.JMBG) || String.IsNullOrEmpty(user.Gender) || String.IsNullOrEmpty(user.Residence)
                || String.IsNullOrEmpty(user.MarriageStatus) || String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.UserPassword)
                || String.IsNullOrEmpty(admin.AdminType) || JmbgInputValidation(user.JMBG) == false
                || !user.FirstName.All(Char.IsLetter) || !user.LastName.All(Char.IsLetter))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// method for saving data to the database
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {                    
                    tblUser newUser = new tblUser();
                    tblAdmin newAdmin = new tblAdmin();

                    // inputs and validations
                    if (user.FirstName.All(Char.IsLetter))
                    {
                        newUser.FirstName = user.FirstName;
                    }
                    else
                    {
                        MessageBox.Show("Wrong First Name input, please try again.");
                    }

                    if (user.LastName.All(Char.IsLetter))
                    {
                        newUser.LastName = user.LastName;
                    }
                    else
                    {
                        MessageBox.Show("Wrong Last Name input, please try again.");
                    }

                    newUser.JMBG = user.JMBG;

                    // JMBG validation
                    if (JmbgInputValidation(newUser.JMBG) == false)
                    {
                        MessageBox.Show("Wrong input, please check your JMBG (13 characters).");
                    }

                    string sex = user.Gender.ToUpper();

                    // gender validation
                    if ((sex == "M" || sex == "Z" || sex == "X" || sex == "N"))
                    {
                        newUser.Gender = sex;
                    }
                    else
                    {
                        MessageBox.Show("Wrong Gender input, please enter M, Z, X or N.");
                    }

                    user.UserID = newUser.UserID;

                    newUser.Residence = user.Residence;

                    string marriage = user.MarriageStatus.ToUpper();

                    if ((marriage == "MARRIED" || marriage == "UNMARRIED" || marriage == "DIVORCED"))
                    {
                        newUser.MarriageStatus = marriage;
                    }
                    else
                    {
                        MessageBox.Show("Wrong Marriage status input. Please try again.\n (Married/Unmarried/Divorced)");
                    }

                    newUser.Username = user.Username;
                    newUser.UserPassword = user.UserPassword;

                    string adminType = admin.AdminType.ToUpper();

                    if ((adminType == "TEAM" || adminType == "SYSTEM" || adminType == "LOCAL"))
                    {
                        newAdmin.AdminType = adminType;
                    }
                    else
                    {
                        MessageBox.Show("Wrong Admin type input. Please try again.\n (Team/System/Local)");
                    }

                    newAdmin.AdminID = admin.AdminID;
                    newAdmin.ExpirationDate = DateTime.Now.AddDays(7);
                    newAdmin.UserID = newUser.UserID;

                    // saving data to the database
                    context.tblUsers.Add(newUser);
                    context.tblAdmins.Add(newAdmin);
                    context.SaveChanges();

                    MessageBox.Show("The admin created successfully.");

                    // logging the action
                    FileActions.FileActions.Instance.Adding(FileActions.FileActions.path, FileActions.FileActions.actions, "user", newUser.FirstName + " " + newUser.LastName);
                    FileActions.FileActions.Instance.Adding(FileActions.FileActions.path, FileActions.FileActions.actions, "admin", newUser.FirstName + " " + newUser.LastName);
                }
                masterView.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong inputs, please check your inputs or try again.");
            }
        }

        // command for closing the window
        private ICommand close;
        public ICommand Close
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }

        /// <summary>
        /// method for closing the window
        /// </summary>
        private void CloseExecute()
        {
            try
            {
                masterView.Close();
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

        /// <summary>
        /// JMBG validation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool JmbgInputValidation(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input.Length == 13 && input[i] >= '0' && input[i] <= '9' && input.All(Char.IsDigit))
                {
                    if (input[0] <= '3' && input[2] < '2')
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
