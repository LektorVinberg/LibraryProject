using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for LibrarianWindow.xaml
    /// </summary>
    public partial class LibrarianWindow : Window
        {
            private LibraryLogic _libraryLogic;

        public LibrarianWindow(LibraryLogic libraryLogic)
        {
            InitializeComponent();
            // Fetch the library logic from the main window and retrieve the lists of books and customers
            _libraryLogic = libraryLogic;
            _libraryLogic.RetrieveLists();
        }

        private void CustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow(_libraryLogic);
            customerWindow.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Save the current state of the library before exiting
            _libraryLogic.BackupLists();
            Close();    
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            BookWindow bookWindow = new BookWindow(_libraryLogic);
            bookWindow.ShowDialog();
        }
    }
}
