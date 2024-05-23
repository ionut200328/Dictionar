using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dictionar.Core;

namespace Dictionar.Services
{
    public interface INavigationService
    {
        ViewModel CurrentViewModel { get; }
        void NavigateTo<T>(object parameter = null) where T : ViewModel;
    }
    public class NavigationService: ObservableObject, INavigationService
    {
        private ViewModel _currentView;
        private readonly Func<Type, ViewModel> _viewModelFactory;

        public ViewModel CurrentViewModel
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public NavigationService(Func<Type, ViewModel> viewModelFactory)
        {
           _viewModelFactory = viewModelFactory;
        }
        public void NavigateTo<TViewModel>(object p) where TViewModel : ViewModel
        {
            ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentViewModel = viewModel;
            viewModel.SetParameter(p);

        }

    }
}
