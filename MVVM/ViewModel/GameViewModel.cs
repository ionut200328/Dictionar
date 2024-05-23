using Dictionar.Services;
using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Dictionar.Core;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Dictionar.MVVM.ViewModel
{
    public class NullToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue; // Or return a default image path here

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class LevelToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int currentLevel && parameter is string minLevelString && int.TryParse(minLevelString, out int minLevel))
            {
                return currentLevel > minLevel;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CompositeCommand : ICommand
    {
        private readonly List<ICommand> commands = new List<ICommand>();

        public void AddCommand(ICommand command)
        {
            if (command != null)
            {
                commands.Add(command);
            }
        }

        public bool CanExecute(object parameter)
        {
            return commands.All(c => c.CanExecute(parameter));
        }

        public void Execute(object parameter)
        {
            foreach (var command in commands)
            {
                command.Execute(parameter);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
    public class GameViewModel : Core.ViewModel
    {
        private readonly IWordDataService _wordDataService;
        private INavigationService _navigationService;
        private int _currentLevel = 1;
        private const int MaxLevel = 5;
        private string _imageSource;
        private string _description;
        private string _currentWord;
        private int _score;
        private string _userGuess;


        public ICommand NextCommand { get; private set; }
        //public ICommand PreviousCommand { get; private set; }
        public ICommand GuessCommand { get; private set; }

        public CompositeCommand Advance { get; private set; }

        private string _guessResultMessage;
        public string GuessResultMessage
        {
            get => _guessResultMessage;
            set
            {
                _guessResultMessage = value;
                OnPropertyChanged();
            }
        }


        public GameViewModel(IWordDataService wordDataService, INavigationService navigationService)
        {
            _wordDataService = wordDataService;
            _navigationService = navigationService;
            Advance = new CompositeCommand();
            NextCommand = new RelayCommand(NextLevel, _ => CanGoToNextLevel());
            //PreviousCommand = new RelayCommand(PreviousLevel, _ => CanGoToPreviousLevel());
            GuessCommand = new RelayCommand(GuessWord,CanGuess);
            Advance.AddCommand(GuessCommand);
            Advance.AddCommand(NextCommand);
            


            LoadLevel();
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }

        private bool CanGuess(object obj)
        {
            return true;
        }

        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string CurrentWord
        {
            get => _currentWord;
            set
            {
                _currentWord = value;
                OnPropertyChanged();
            }
        }

        public string UserGuess
        {
            get => _userGuess;
            set
            {
                _userGuess = value;
                OnPropertyChanged();
            }
        }

        private void LoadLevel()
        {
            var words = _wordDataService.GetWords();

            var random = new Random();
            // Ensure there are at least 5 words to avoid an exception
            if (words.Count >= 5)
            {
                var selectedWords = words.OrderBy(x => random.Next()).Take(5).ToList();

                // Now, selectedWords contains 5 random word entries from the original words collection
                // You can proceed with your logic, using selectedWords for the current level

                // Example of selecting one of the words randomly for the current level
                var wordEntry = selectedWords[random.Next(selectedWords.Count)];
                _currentWord = wordEntry.Key;

                // Decide randomly to show the image or the description
                if (random.Next(2) == 0 && _wordDataService.GetWordDetail(wordEntry.Key).Image != null && _wordDataService.GetImage(wordEntry.Key) != null)
                {
                    ImageSource = wordEntry.Value.Image;
                    Description = null;
                }
                else
                {
                    ImageSource = null;
                    Description = wordEntry.Value.Description;
                }
            }
        }
        public string NextButtonText
        {
            get => _currentLevel < MaxLevel ? "Next" : "Finish";
        }

        private void NextLevel(object obj)
        {
            if (_currentLevel <= MaxLevel)
            {
                _currentLevel++;
                LoadLevel();
                OnPropertyChanged(nameof(CurrentLevel)); // Ensure OnPropertyChanged is called
                OnPropertyChanged(nameof(NextButtonText));
            }
        }

        private bool CanGoToNextLevel() => _currentLevel <= MaxLevel;

        //private void PreviousLevel(object obj)
        //{
        //    if (_currentLevel > 1)
        //    {
        //        _currentLevel--;
        //        LoadLevel();
        //        OnPropertyChanged(nameof(CurrentLevel)); // Ensure OnPropertyChanged is called
        //    }
        //    CommandManager.InvalidateRequerySuggested(); // Add this line
        //}

        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                if (_currentLevel != value)
                {
                    _currentLevel = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(NextButtonText)); // This should update your button text
                }
            }
        }

        //private bool CanGoToPreviousLevel() => _currentLevel > 1;

        private void GuessWord(object parameter)
        {
            string message;
            if (!string.IsNullOrEmpty(UserGuess) && UserGuess.Equals(_currentWord, StringComparison.OrdinalIgnoreCase))
            {
                _score++;
                message = $"{CurrentLevel}   Correct! The word was: {_currentWord}.";
            }
            else
            {
                message = $"{CurrentLevel}   Incorrect. The correct word was: {_currentWord}.";
            }

            MessageBox.Show(message, "Guess Result");
            if(CurrentLevel == MaxLevel)
            {
                MessageBox.Show($"Your score is {_score}", "Game Over");
                CurrentLevel = 0;
                
                Navigation.NavigateTo<HomeViewModel>();
            }
        }



    }
}
