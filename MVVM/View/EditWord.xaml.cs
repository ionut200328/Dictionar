using Dictionar.MVVM.ViewModel;
using Dictionar.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dictionar.MVVM.View
{
    /// <summary>
    /// Interaction logic for EditWord.xaml
    /// </summary>
    public partial class EditWord : Page
    {
        public EditWord()
        {
            InitializeComponent();
        }
        private void OnChooseImageClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg)|*.png;*.jpg"
            };
            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFileName = fileDialog.FileName;
                // Assuming EditWordViewModel has a method to update the image path
                ((EditWordViewModel)this.DataContext).SelectedImage = selectedFileName;
            }
        }
    }
}
