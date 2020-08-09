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
    class AddPositionViewModel : ViewModelBase
    {
        AddPositionView addPosition;

        // properties
        private tblPosition position;
        public tblPosition Position
        {
            get { return position; }
            set { position = value; OnPropertyChanged("Position"); }
        }

        // constructor
        public AddPositionViewModel(AddPositionView addPositionOpen)
        {
            addPosition = addPositionOpen;
            position = new tblPosition();
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

        private bool CanSaveExecute()
        {
            if (String.IsNullOrEmpty(position.PositionName) || String.IsNullOrEmpty(position.PositionDescription))
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
                    tblPosition newPosition = new tblPosition();

                    newPosition.PositionName = position.PositionName;
                    newPosition.PositionDescription = position.PositionDescription;
                    newPosition.PositionID = position.PositionID;

                    // saving data
                    context.tblPositions.Add(newPosition);
                    context.SaveChanges();

                    // logging the action
                    FileActions.FileActions.Instance.Adding(FileActions.FileActions.path, FileActions.FileActions.actions, "position", newPosition.PositionName);
                }
                addPosition.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong inputs. Please try again.");
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
                addPosition.Close();
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
    }
}
