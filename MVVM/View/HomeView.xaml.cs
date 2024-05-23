using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Dictionar.MVVM.ViewModel;
using Dictionar.Services;

namespace Dictionar.MVVM.View
{
    public partial class HomeView : Page
    {
        public HomeView()
        {
            InitializeComponent();
            var wordDataService = new WordDataService(); // Assuming you have a parameterless constructor
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var viewModel = DataContext as HomeViewModel;

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
                if (DataContext is HomeViewModel viewModel)
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
