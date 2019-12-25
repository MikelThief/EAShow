using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.GeneticAlgorithms.Services;
using EAShow.Shared.Models;

namespace EAShow.Core.ViewModels
{
    public class ProfileViewModel : PropertyChangedBase
    {
        private Profile _profile;

        public string Name => _profile == null ? _profile.Name : string.Empty;

        private readonly FunctionOptimizationGaService _gaService;

        public ProfileViewModel(FunctionOptimizationGaService gaService)
        {
            _gaService = gaService;
        }

        public void InjectProfile(Profile profile)
        {
            _profile = profile;
            _gaService.InjectProfile(profile: profile);
            _gaService.Start();
        }
    }
}
