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
    class AddProjectViewModel : ViewModelBase
    {
        AddProjectView addProject;

        // properties
        private tblProject project;
        public tblProject Project
        {
            get { return project; }
            set { project = value; OnPropertyChanged("Project"); }
        }

        private tblManager manager;
        public tblManager Manager
        {
            get { return manager; }
            set { manager = value; OnPropertyChanged("Manager"); }
        }

        // property for updating the project list view
        private bool isUpdateProject;
        public bool IsUpdateProject
        {
            get
            {
                return isUpdateProject;
            }
            set
            {
                isUpdateProject = value;
            }
        }

        // constructor
        public AddProjectViewModel(AddProjectView addProjectOpen, tblManager managerToPass)
        {
            manager = managerToPass;
            addProject = addProjectOpen;
            project = new tblProject();
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
            if (String.IsNullOrEmpty(project.ProjectName) || String.IsNullOrEmpty(project.ClientName)
                || String.IsNullOrEmpty(project.ContractManager) || !project.ClientName.All(Char.IsLetter))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// method for saving the data to the database
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {
                    tblProject newProject = new tblProject();

                    // inputs and validations
                    newProject.ProjectName = project.ProjectName;
                    newProject.ProjectDescription = project.ProjectDescription;

                    if (project.ClientName.All(Char.IsLetter))
                    {
                        newProject.ClientName = project.ClientName;
                    }
                    else
                    {
                        MessageBox.Show("Wrong Client Name input, please try again.");
                    }

                    newProject.ContractDate = DateTime.Now;

                    if (project.ContractManager.All(Char.IsLetter))
                    {
                        newProject.ContractManager = project.ContractManager;
                    }
                    else
                    {
                        MessageBox.Show("Wrong Contract Manager input, please try again.");
                    }

                    newProject.ProjectStartDate = DateTime.Now.AddDays(7);
                    newProject.ProjectDeadline = DateTime.Now.AddDays(30);

                    newProject.HourlyRate = project.HourlyRate;

                    string a = project.Realisation;

                    if (a == "1" || a == "2" || a =="0")
                    {
                        newProject.Realisation = project.Realisation;
                    }
                    else
                    {
                        MessageBox.Show("Wrong Realisation input, please try again. \n(0/1/2)");
                    }

                    // getting the ManagerID for the project
                    tblManager viaManager = (from x in context.tblManagers where x.ManagerID == manager.ManagerID select x).FirstOrDefault();
                    newProject.ManagerID = viaManager.ManagerID;
                    newProject.ProjectID = project.ProjectID;

                    // saving data
                    context.tblProjects.Add(newProject);
                    context.SaveChanges();

                    MessageBox.Show("The project created successfully.");

                    // logging action
                    FileActions.FileActions.Instance.Adding(FileActions.FileActions.path, FileActions.FileActions.actions, "project", newProject.ProjectName);

                    // updating the project list view
                    IsUpdateProject = true;
                }
                addProject.Close();
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
                addProject.Close();
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
