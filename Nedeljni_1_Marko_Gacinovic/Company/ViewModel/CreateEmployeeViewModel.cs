using Company.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.ViewModel
{
    class CreateEmployeeViewModel : ViewModelBase
    {
        CreateEmployeeView createEmployeeView;

        public CreateEmployeeViewModel(CreateEmployeeView employeeOpen)
        {
            createEmployeeView = employeeOpen;
        }
    }
}
