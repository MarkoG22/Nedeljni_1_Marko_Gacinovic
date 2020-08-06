using Company.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.ViewModel
{
    class MasterViewModel : ViewModelBase
    {
        MasterView masterView;

        public MasterViewModel(MasterView masterViewOpen)
        {
            masterView = masterViewOpen;
        }
    }
}
