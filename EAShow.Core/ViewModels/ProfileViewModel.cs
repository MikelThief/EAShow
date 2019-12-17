using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Shared.Models;

namespace EAShow.Core.ViewModels
{
    public class ProfileViewModel : PropertyChangedBase
    {
        private Profile _profile;

        public string Name => _profile.Name;

        public ProfileViewModel(Profile profile)
        {
            _profile = profile;
        }
    }
}
