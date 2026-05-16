using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Libra
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private LibraryLogic _libraryLogic;
        public ReportWindow(LibraryLogic libraryLogic)
        {
            InitializeComponent();
            _libraryLogic = libraryLogic;
            LoadReport();
        }

        private void LoadReport()
        {
            ReportListBox.ItemsSource = _libraryLogic.GetLoanReport();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
