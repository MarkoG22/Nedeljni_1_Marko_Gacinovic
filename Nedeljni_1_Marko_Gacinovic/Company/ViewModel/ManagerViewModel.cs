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
    class ManagerViewModel : ViewModelBase
    {
        ManagerView managerView;

        private tblPosition position;
        public tblPosition Position
        {
            get { return position; }
            set { position = value; OnPropertyChanged("Position"); }
        }

        private tblProject project;
        public tblProject Project
        {
            get { return project; }
            set { project = value; OnPropertyChanged("Project"); }
        }

        private List<tblProject> projectList;
        public List<tblProject> ProjectList
        {
            get { return projectList; }
            set { projectList = value; OnPropertyChanged("ProjectList"); }
        }

        private tblManager manager;
        public tblManager Manager
        {
            get { return manager; }
            set { manager = value; OnPropertyChanged("Manager"); }
        }
        
        public ManagerViewModel(ManagerView managerViewOpen, tblManager managerToPass)
        {
            manager = managerToPass;
            managerView = managerViewOpen;            
            ProjectList = GetAllProjects();
        }

        private ICommand addNewProject;
        public ICommand AddNewProject
        {
            get
            {
                if (addNewProject == null)
                {
                    addNewProject = new RelayCommand(param => AddNewProjectExecute(), param => CanAddNewProjectExecute());
                }
                return addNewProject;
            }
        }

        private bool CanAddNewProjectExecute()
        {
            return true;
        }

        private void AddNewProjectExecute()
        {
            try
            {
                AddProjectView addProject = new AddProjectView(manager);
                addProject.ShowDialog();
                if ((addProject.DataContext as AddProjectViewModel).IsUpdateProject == true)
                {
                    ProjectList = GetAllProjects().ToList();                    
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private ICommand addNewPosition;
        public ICommand AddNewPosition
        {
            get
            {
                if (addNewPosition == null)
                {
                    addNewPosition = new RelayCommand(param => AddNewPositionExecute(), param => CanAddNewPositionExecute());
                }
                return addNewPosition;
            }
        }

        private bool CanAddNewPositionExecute()
        {
            return true;
        }

        private void AddNewPositionExecute()
        {
            try
            {
                AddPositionView addPosition = new AddPositionView();
                addPosition.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private ICommand deleteProject;
        public ICommand DeleteProject
        {
            get
            {
                if (deleteProject == null)
                {
                    deleteProject = new RelayCommand(param => DeleteProjectExecute(), param => CanDeleteProjectExecute());
                }
                return deleteProject;
            }
            
        }

        private bool CanDeleteProjectExecute()
        {
            return true;
        }

        private void DeleteProjectExecute()
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {
                    int id = project.ProjectID;

                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the project?", "Delete Confirmation", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        tblProject projectToDelete = (from x in context.tblProjects where x.ProjectID == id select x).First();

                        context.tblProjects.Remove(projectToDelete);
                        context.SaveChanges();

                        ProjectList = GetAllProjects().ToList();

                        FileActions.FileActions.Instance.Deleting(FileActions.FileActions.path, FileActions.FileActions.actions, "project", projectToDelete.ProjectName);
                    }
                }               
            }
            catch (Exception)
            {
                MessageBox.Show("The project can not be deleted, please try again.");
            }
        }

        private List<tblProject> GetAllProjects()
        {
            try
            {
                using (CompanyDBEntities context = new CompanyDBEntities())
                {
                    List<tblProject> list = new List<tblProject>();
                    list = (from x in context.tblProjects select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }
    }
}
