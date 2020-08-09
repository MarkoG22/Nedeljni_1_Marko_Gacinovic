using Company.Models;
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
    /// Interaction logic for AddProjectView.xaml
    /// </summary>
    public partial class AddProjectView : Window
    {
        public AddProjectView(tblManager manager)
        {
            InitializeComponent();
            this.DataContext = new AddProjectViewModel(this, manager);
        }

        private void NumbersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RealisationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^012]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
