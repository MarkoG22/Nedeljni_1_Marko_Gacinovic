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
    class CreateManagerViewModel : ViewModelBase
    {
        CreateManagerView createManagerView;

        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private tblManager manager;
        public tblManager Manager
        {
            get { return manager; }
            set { manager = value; OnPropertyChanged("Manager"); }
        }

        public CreateManagerViewModel(CreateManagerView managerOpen)
        {
            user = new tblUser();
            manager = new tblManager();
            createManagerView = managerOpen;
        }

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

        private void SaveExecute()
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {
                    tblUser newUser = new tblUser();
                    tblManager newManager = new tblManager();

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

                    newManager.Email = manager.Email;
                    string reservedPassword = manager.ReservedPassword;

                    if (reservedPassword.Length >= 5)
                    {
                        newManager.ReservedPassword = reservedPassword + "WPF";
                    }
                    else
                    {
                        MessageBox.Show("Wrong Reserved Password input. Please try again.\n (Minimum 5 characters)");
                    }                    
                                        
                    newManager.SuccessfulProjects = manager.SuccessfulProjects;
                    newManager.OfficeNumber = manager.OfficeNumber;
                    newManager.ManagerID = manager.ManagerID;

                    context.tblUsers.Add(newUser);
                    context.tblManagers.Add(newManager);
                    context.SaveChanges();
                }
                createManagerView.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong inputs, please check your inputs or try again.");
            }
        }

        private bool CanSaveExecute()
        {
            if (String.IsNullOrEmpty(user.FirstName) || String.IsNullOrEmpty(user.LastName)
                || String.IsNullOrEmpty(user.JMBG) || String.IsNullOrEmpty(user.Gender) || String.IsNullOrEmpty(user.Residence)
                || String.IsNullOrEmpty(user.MarriageStatus) || String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.UserPassword)
                || String.IsNullOrEmpty(manager.Email) || JmbgInputValidation(user.JMBG) == false
                ||  !user.FirstName.All(Char.IsLetter) || !user.LastName.All(Char.IsLetter))
            {
                return false;
            }
            else
            {
                return true;
            }
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
