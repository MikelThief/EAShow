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
    public sealed partial class RunnerInstanceView : UserControl
    {
        public RunnerInstanceViewModel ViewModel => DataContext as RunnerInstanceViewModel;

        public RunnerInstanceView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handler for the pointer entered event.
        /// Displays the delete item "hover" buttons.
        /// </summary>
        /// <param name="sender">Source of the pointer entered event</param>
        /// <param name="e">Event args for the pointer entered event</param>
        private void ProfileItem_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType ==
                Windows.Devices.Input.PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType ==
                Windows.Devices.Input.PointerDeviceType.Pen)
            {
                VisualStateManager.GoToState(
                    sender as Control, stateName: "HoverButtonsShown", useTransitions: true);
            }
        }

        /// <summary>
        /// Handler for the pointer exited event.
        /// Hides the delete item "hover" buttons.
        /// </summary>
        /// <param name="sender">Source of the pointer exited event</param>
        /// <param name="e">Event args for the pointer exited event</param>
        private void ProfileItem_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType ==
                Windows.Devices.Input.PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType ==
                Windows.Devices.Input.PointerDeviceType.Pen)
            {
                VisualStateManager.GoToState(
                    sender as Control, stateName: "HoverButtonsHidden", useTransitions: true);
            }

        }
    }
}
