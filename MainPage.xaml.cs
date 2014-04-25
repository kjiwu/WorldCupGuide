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
            Debug.WriteLine("OK");
            TeamToggleButton.IsChecked = false;
            TimeToggleButton.IsChecked = false;
            (sender as ToggleButton).IsChecked = true;
        }
    }
}