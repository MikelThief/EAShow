using System;

using EAShow.Core.ViewModels;

using Windows.UI.Xaml.Controls;

namespace EAShow.Core.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
    }
}
