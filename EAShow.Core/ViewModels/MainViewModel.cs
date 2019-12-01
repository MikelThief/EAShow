using System;

using Caliburn.Micro;

using EAShow.Core.Helpers;

namespace EAShow.Core.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel(MutationSettingsViewModel mutationSettingsViewModel)
        {
            MutationSettingsViewModel = mutationSettingsViewModel;
        }

        public MutationSettingsViewModel MutationSettingsViewModel { get; }
    }
}
