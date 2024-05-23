using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dictionar.Core;
using Dictionar.Services;

namespace Dictionar.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {
        private INavigationService _navigationService;

        public INavigationService NavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToHomeCommand { get; set; }
        public RelayCommand NavigateToAdminCommand { get; set; }
        public RelayCommand NavigateToGameCommand { get; set; }
        public RelayCommand NavigateToLoginCommand { get; set; }
        public RelayCommand NavigateToDetailsCommand { get; set; }
        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateToHomeCommand = new RelayCommand(execute: o => { NavigationService.NavigateTo<HomeViewModel>(); }, canExecute: o => true);
            NavigateToAdminCommand = new RelayCommand(execute: o => { NavigationService.NavigateTo<AdminViewModel>(); }, canExecute: o => true);
            NavigateToGameCommand = new RelayCommand(execute: o => { NavigationService.NavigateTo<GameViewModel>(); }, canExecute: o => true);
            NavigateToLoginCommand = new RelayCommand(execute: o => { NavigationService.NavigateTo<EditViewModel>(); }, canExecute: o => true);
            NavigateToDetailsCommand = new RelayCommand(execute: o => { NavigationService.NavigateTo<DetailsViewModel>(); }, canExecute: o => true);
        }
    }
}
