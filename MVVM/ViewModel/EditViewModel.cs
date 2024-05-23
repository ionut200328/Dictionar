using Dictionar.Core;
using Dictionar.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dictionar.MVVM.ViewModel
{
    public class EditViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        private readonly IWordDataService _wordDataService;
        private ObservableCollection<string> _words;
        private List<string> _categories;
        private string _selectedCategory;
        private string _selectedWord;

        public ICommand AddCommand { get; private set; }

        public EditViewModel(INavigationService navigationService, IWordDataService wordDataService, EditWordViewModel editWordViewModel)
        {
            _navigation = navigationService;
            _wordDataService = wordDataService;
            DetailsCommand = new Core.RelayCommand(ShowDetails, CanShowDetails);
            AddCommand = new Core.RelayCommand(AddWord, CanAddWord);
            LoadWords();
            // subscribe to the event
            editWordViewModel.WordDeleted += LoadWords;
        }

        private void AddWord(object obj)
        {

            NavigationService.NavigateTo<EditWordViewModel>(string.Empty);
        }

        private bool CanAddWord(object obj)
        {
            return true;
        }

        public ObservableCollection<string> Words
        {
            get => _words;
            set
            {
                _words = value;
                OnPropertyChanged();
            }
        }
        public string SelectedWord
        {
            get => _selectedWord;
            set
            {
                _selectedWord = value;
                OnPropertyChanged();
            }
        }

        public List<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();

                    // Filter the words based on the selected category
                    if (!string.IsNullOrEmpty(_selectedCategory))
                    {
                        var filteredWords = _wordDataService.GetWords()
                            .Where(pair => pair.Value.Category.Equals(_selectedCategory))
                            .Select(pair => pair.Key);

                        Words = new ObservableCollection<string>(filteredWords);
                    }
                    else
                    {
                        // If no category is selected, or the selection is cleared, show all words
                        LoadWords();
                    }
                }
            }
        }


        private void LoadWords()
        {
            var wordsDictionary = _wordDataService.GetWords();
            Words = new ObservableCollection<string>(wordsDictionary.Keys);
            Categories = _wordDataService.GetCategories();
        }

        public ICommand DetailsCommand { get; private set; }

        private bool CanShowDetails(object parameter)
        {
            // You can add more logic here if needed
            return parameter is string selectedWord && !string.IsNullOrWhiteSpace(selectedWord);
        }


        private void ShowDetails(object parameter)
        {
            var selectedWord = parameter as string;
            // Assuming your navigation service accepts a parameter for navigation
            NavigationService.NavigateTo<EditWordViewModel>(selectedWord);
        }

        public INavigationService NavigationService
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }
    }
}
