using Company.Models;
using Company.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.ViewModel
{
    class LocalAdminViewModel : ViewModelBase
    {
        LocalAdminView localAdminView;

        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }
    }
}
