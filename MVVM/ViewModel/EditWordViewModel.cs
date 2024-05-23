using Dictionar.Core;
using Dictionar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dictionar.MVVM.ViewModel
{
    public class EditWordViewModel : Core.ViewModel
    {
        private readonly IWordDataService _wordDataService;
        private INavigationService _navigation;
        private string _selectedWord;
        private string _selectedWordDefinition;
        private string _selectedImage;
        private string _selectedCatgory;
        private List<string> _categories;
        public event Action WordDeleted;

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        private string _newCategory;
        public ICommand AddCategoryCommand { get; private set; }

        public EditWordViewModel(IWordDataService wordDataService, INavigationService navigationService)
        {
            _wordDataService = wordDataService;
            _navigation = navigationService;
            Categories = _wordDataService.GetCategories();
            SaveCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            DeleteCommand = new RelayCommand(DeleteWord, CanDeleteWord);
            AddCategoryCommand = new RelayCommand(AddNewCategory, CanAddNewCategory);
        }

        private bool CanAddNewCategory(object parameter)
        {
            return !string.IsNullOrEmpty(NewCategory) && !Categories.Contains(NewCategory);
        }

        private void AddNewCategory(object parameter)
        {
            Categories.Add(NewCategory);
            SelectedCatgory = NewCategory; // Automatically select the newly added category
            NewCategory = string.Empty; // Reset the new category input
            OnPropertyChanged(nameof(Categories));

            // Optionally, save the updated categories list to your data source
           
        }

        private void DeleteWord(object obj)
        {
            _wordDataService.DeleteWord(SelectedWord);
            _wordDataService.SaveChanges();
            // refresh the list of words
            Categories = _wordDataService.GetCategories();
            // clear the selected word
            SelectedWord = string.Empty;
            // clear the description
            SelectedDescription = string.Empty;
            // clear the image
            SelectedImage = string.Empty;
            // clear the category
            SelectedCatgory = string.Empty;
            // raise the event
            WordDeleted?.Invoke();
            // navigate to the home page
            Navigation.NavigateTo<EditViewModel>();
        }
        
        private bool CanDeleteWord(object obj)
        {
            // if the selected word is not empty, return true
            return !string.IsNullOrEmpty(SelectedWord);
        }

        private bool CanSaveChanges(object obj)
        {
            return true;
        }

        private void LoadWord()
        {
            SelectedDescription = _wordDataService.GetDescription(SelectedWord);
            SelectedImage = _wordDataService.GetImage(SelectedWord);
            SelectedCatgory = _wordDataService.GetCategory(SelectedWord);
        }
        public List<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
        public string SelectedCatgory
        {
            get => _selectedCatgory;
            set
            {
                _selectedCatgory = value;
                OnPropertyChanged(nameof(SelectedCatgory));
            }
        }
        public string SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                if(value == null)
                {
                    _selectedImage = "E:\\AN II Sem 2\\MVP\\Dictionar\\Data\\Images\\NOIMG.jpg";
                }
                OnPropertyChanged(nameof(SelectedImage));
            }
        }
        public string SelectedWord
        {
            get => _selectedWord;
            set
            {
                _selectedWord = value;
                OnPropertyChanged(nameof(SelectedWord));
            }
        }
        public string SelectedDescription
        {
            get => _selectedWordDefinition;
            set
            {
                _selectedWordDefinition = value;
                OnPropertyChanged(nameof(SelectedDescription));
            }
        }

        public override void SetParameter(object parameter)
        {
            if (parameter is string word)
            {
                SelectedWord = word;
                LoadWord();

            }
        }

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        private void SaveChanges(object parameter)
        {
            // Check if the word exists and it is not the currently selected word
            if (_wordDataService.WordExists(SelectedWord) && _selectedWord != SelectedWord)
            {
                // Optionally, inform the user that the word already exists
                MessageBox.Show("This word already exists. Please choose a different word.", "Word Exists");
            }
            else
            {
                // If the word does not exist or it is the currently selected word, proceed to add or update
                if (_wordDataService.WordExists(SelectedWord))
                {
                    // Update existing word
                    _wordDataService.UpdateWord(SelectedWord, SelectedDescription, SelectedImage, SelectedCatgory);
                }
                else
                {
                    // Add new word
                    _wordDataService.AddWord(SelectedWord, SelectedDescription, SelectedImage, SelectedCatgory);
                }
                _wordDataService.SaveChanges();

                // Optionally, navigate back to the word list or clear the form
            }
        }


        public string NewCategory
        {
            get => _newCategory;
            set
            {
                _newCategory = value;
                OnPropertyChanged(nameof(NewCategory));
            }
        }


    }
}
