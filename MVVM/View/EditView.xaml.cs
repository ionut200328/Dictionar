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
    /// Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : Page
    {
        public EditView()
        {
            InitializeComponent();
            var wordDataService = new WordDataService(); // Assuming you have a parameterless constructor
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var viewModel = DataContext as EditViewModel;

            // Check if the ViewModel or its Words property is null before proceeding
            if (viewModel == null || viewModel.Words == null)
                return; // Exit if no ViewModel or Words collection is available

            var filterText = textBox?.Text;
            if (string.IsNullOrWhiteSpace(filterText))
            {
                // Ensure resetting the ItemsSource doesn't cause a null reference
                listBox.ItemsSource = viewModel.Words; // Reset to all words
            }
            else
            {
                // Perform filtering only if there is text to filter by
                var filteredWords = viewModel.Words
                    .Where(word => word.StartsWith(filterText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                listBox.ItemsSource = filteredWords;
            }
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedWord = e.AddedItems[0] as string; // Assuming the items are strings
                if (DataContext is EditViewModel viewModel)
                {
                    if (viewModel.DetailsCommand.CanExecute(selectedWord)) // Pass the selectedWord instead of null
                    {
                        viewModel.DetailsCommand.Execute(selectedWord); // Pass the selectedWord
                    }
                }
            }

        }
    }
}
