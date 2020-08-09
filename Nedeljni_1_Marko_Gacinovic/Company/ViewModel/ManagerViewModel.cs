using Company.Commands;
using Company.Models;
using Company.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ManagerViewModel(ManagerView managerViewOpen)
        {
            managerView = managerViewOpen;
            position = new tblPosition();
            project = new tblProject();
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
                AddProjectView addProject = new AddProjectView();
                addProject.ShowDialog();
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
