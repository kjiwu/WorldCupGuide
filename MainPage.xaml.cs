using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WorldCupGuide.Resources;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace WorldCupGuide
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            TeamToggleButton.IsChecked = false;
            TimeToggleButton.IsChecked = false;
            (sender as ToggleButton).IsChecked = true;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            if (MessageBox.Show(AppResources.QuitPrompt, AppResources.ApplicationTitle, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.Current.Terminate();
            }

            base.OnBackKeyPress(e);
        }
    }
}