﻿using Dictionar.MVVM.ViewModel;
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
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Page
    {
        public AdminView()
        {
            InitializeComponent();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is AdminViewModel viewModel)
            {
                if (viewModel.LoginCommand.CanExecute(null))
                {
                    viewModel.LoginCommand.Execute(null);
                }
            }
        }
    }
}