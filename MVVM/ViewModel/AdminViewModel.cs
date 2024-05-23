using Dictionar.Core;
using Dictionar.Services;
using System;
using System.Windows;
using System.Windows.Input; // For ICommand

namespace Dictionar.MVVM.ViewModel
{
    public class AdminViewModel : Core.ViewModel
    {
        private string _username;
        private string _password;
        private INavigationService _navigationService;

        public INavigationService Navigation
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; private set; }

        public AdminViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
            LoginCommand = new RelayCommand(Login, CanLogin);
        }


        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void Login(object parameter)
        {
            AdminService adminService = new AdminService();
            if (adminService.IsValidAdmin(Username, Password))
            {
                Navigation.NavigateTo<EditViewModel>();
            }
            else
            {
                MessageBox.Show("Incorrect username or password.");
            }
        }

        public bool CheckCredentials()
        {
            return Username == "admin" && Password == "admin";
        }
    }
}
