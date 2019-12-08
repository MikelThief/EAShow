using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EAShow.Core.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EAShow.Core.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MutationSettingsView : UserControl
    {
        public MutationSettingsViewModel ViewModel => DataContext as MutationSettingsViewModel;

        public MutationSettingsView()
        {
            this.InitializeComponent();
        }

        private void Mutation1NumericUpDown_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if ((decimal) Mutation1NumericUpDown.Value < 0.01M)
                Mutation1Switch.IsOn = false;
        }

        private void SelectorsGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            Mutation1NumericUpDown.Culture = CultureInfo.CurrentUICulture;
            Mutation2NumericUpDown.Culture = CultureInfo.CurrentUICulture;
        }

        private void Mutation2NumericUpDown_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if ((decimal)Mutation2NumericUpDown.Value < 0.01M)
                Mutation2Switch.IsOn = false;
        }
    }
}
