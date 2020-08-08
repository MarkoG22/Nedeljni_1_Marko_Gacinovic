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
    class CreateEmployeeViewModel : ViewModelBase
    {
        CreateEmployeeView createEmployeeView;

        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private tblWorker worker;
        public tblWorker Worker
        {
            get { return worker; }
            set { worker = value; OnPropertyChanged("Worker"); }
        }


        private tblSector sector;
        public tblSector Sector
        {
            get { return sector; }
            set { sector = value; OnPropertyChanged("Sector"); }
        }

        private tblPosition position;
        public tblPosition Position
        {
            get { return position; }
            set { position = value; OnPropertyChanged("Position"); }
        }

        private List<tblSector> sectorList;
        public List<tblSector> SectorList
        {
            get { return sectorList; }
            set { sectorList = value; OnPropertyChanged("SectorList"); }
        }

        private List<tblPosition> positionList;
        public List<tblPosition> PositionList
        {
            get { return positionList; }
            set { positionList = value; OnPropertyChanged("PositionList"); }
        }

        public CreateEmployeeViewModel(CreateEmployeeView employeeOpen)
        {
            user = new tblUser();
            worker = new tblWorker();
            createEmployeeView = employeeOpen;
            SectorList = GetAllSector();
            PositionList = GetAllPosition();
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

        private bool CanSaveExecute()
        {
            if (String.IsNullOrEmpty(user.FirstName) || String.IsNullOrEmpty(user.LastName)
                || String.IsNullOrEmpty(user.JMBG) || String.IsNullOrEmpty(user.Gender) || String.IsNullOrEmpty(user.Residence)
                || String.IsNullOrEmpty(user.MarriageStatus) || String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.UserPassword)                
                || String.IsNullOrEmpty(worker.EducationDegree) || JmbgInputValidation(user.JMBG) == false
                || !user.FirstName.All(Char.IsLetter) || !user.LastName.All(Char.IsLetter))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SaveExecute()
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {
                    tblUser newUser = new tblUser();
                    tblWorker newWorker = new tblWorker();

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

                    newWorker.YearsOfService = worker.YearsOfService;

                    string education = worker.EducationDegree;

                    if (education == "I" || education == "II" || education == "III" || education == "IV" || education == "V" || education == "VI" || education == "VII")
                    {
                        newWorker.EducationDegree = education;
                    }
                    else
                    {
                        MessageBox.Show("Wrong Education degree input. Please try again.\n (I - VII)");
                    }

                    newWorker.WorkerID = worker.WorkerID;
                    newWorker.UserID = newUser.UserID;
                    newWorker.SectorID = Sector.SectorID;
                    newWorker.PositionID = Position.PositionID;

                    context.tblUsers.Add(newUser);
                    context.tblWorkers.Add(newWorker);
                    context.SaveChanges();
                }
                createEmployeeView.Close();
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
                createEmployeeView.Close();
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

        private List<tblSector> GetAllSector()
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {
                    List<tblSector> list = new List<tblSector>();
                    list = (from x in context.tblSectors select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        private List<tblPosition> GetAllPosition()
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {
                    List<tblPosition> list = new List<tblPosition>();
                    list = (from x in context.tblPositions select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
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
