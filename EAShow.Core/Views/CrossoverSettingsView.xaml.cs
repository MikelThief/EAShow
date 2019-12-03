using System;
using System.Collections.Generic;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace EAShow.Core.Views
{
    public sealed partial class CrossoverSettingsView : UserControl
    {
        public CrossoverSettingsViewModel ViewModel => DataContext as CrossoverSettingsViewModel;

        public CrossoverSettingsView()
        {
            this.InitializeComponent();
        }
    }
}
