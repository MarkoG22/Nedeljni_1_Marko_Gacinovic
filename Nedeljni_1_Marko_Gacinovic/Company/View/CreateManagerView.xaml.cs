using Company.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Company.View
{
    /// <summary>
    /// Interaction logic for CreateManagerView.xaml
    /// </summary>
    public partial class CreateManagerView : Window
    {
        public CreateManagerView()
        {
            InitializeComponent();
            this.DataContext = new CreateManagerViewModel(this);
        }

        private void ReservedPassTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ReservedPassValidation(e.Text);
        }

        private bool ReservedPassValidation(string input)
        {
            if (input.Length >=5)
            {
                return true;
            }
            return false;
        }

        private void NumbersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
