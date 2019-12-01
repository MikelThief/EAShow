using System;

using Caliburn.Micro;

using EAShow.Core.Helpers;

namespace EAShow.Core.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel(
            MutationSettingsViewModel mutationSettingsViewModel,
            PopulationSettingsViewModel populationSettingsViewModel)
        {
            MutationSettingsViewModel = mutationSettingsViewModel;
            PopulationSettingsViewModel = populationSettingsViewModel;
        }

        public MutationSettingsViewModel MutationSettingsViewModel { get; }
        public PopulationSettingsViewModel PopulationSettingsViewModel { get; }
    }
}
