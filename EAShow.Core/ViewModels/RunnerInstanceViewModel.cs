using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Shared.Models;
using EAShow.Core.Helpers;
using EAShow.Infrastructure.Commands.AsyncCommand;
using EAShow.Infrastructure.Commands.DelegateCommand;
using EAShow.Shared.Events;
using EAShow.Shared.Models.DTOs;
using LiteDB;
using Nito.Mvvm;

namespace EAShow.Core.ViewModels
{
    public class RunnerInstanceViewModel : Screen
    {
        private short _index;
        private string _header;

        private readonly CancellationTokenSource _cancellationTokenSource;
        private ProfileViewModel _loadedProfile;

        public ProfileViewModel LoadedProfile
        {
            get => _loadedProfile;
            set => Set(oldValue: ref _loadedProfile, newValue: value, propertyName: nameof(LoadedProfile));
        }

        public short Index
        {
            get => _index;
            set => Set(oldValue: ref _index, newValue: value, propertyName: nameof(Index));
        }

        public string Header
        {
            get => _header;
            set => Set(oldValue: ref _header, newValue: value, nameof(Header));
        }

        public BindableCollection<PresetsProfile> Profiles { get; }



        public DelegateCommand<PresetsProfile> OpenProfileCommand { get; }

        public DelegateCommand<PresetsProfile> DeleteProfileCommand { get; }

        public readonly WinRTContainer _container;

        public RunnerInstanceViewModel(WinRTContainer container)
        {
            _container = container;
            _cancellationTokenSource = new CancellationTokenSource();
            Profiles = new BindableCollection<PresetsProfile>();
            DeleteProfileCommand = new DelegateCommand<PresetsProfile>(executeMethod: DeleteProfile);
            OpenProfileCommand = new DelegateCommand<PresetsProfile>(executeMethod: OpenProfile);
        }

        private void OpenProfile(PresetsProfile selectedPresetsProfile)
        {
            // load presetsProfile. switch to charts and start fun
            var profileVM = _container.GetInstance<ProfileViewModel>();
            profileVM.InjectProfile(profile: selectedPresetsProfile);
            LoadedProfile = profileVM;
            Header = selectedPresetsProfile.Name;
            Profiles.Clear();
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (LoadedProfile == null)
            {
                using (var db =
                    new LiteRepository(connectionString: LiteDbConnectionStringHelper.GetRoamingDbConnectionString()))
                {
                    var queryResult = db.Query<ProfileDbDto>().ToEnumerable();
                    Profiles.AddRange(items: queryResult.Select(selector: PresetsProfile.From));

                }
            }

            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            Profiles.Clear();
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        private void DeleteProfile(PresetsProfile selectedPresetsProfile)
        {
            using (var db = new LiteRepository(connectionString: LiteDbConnectionStringHelper.GetRoamingDbConnectionString()))
            {
                db.Delete<ProfileDbDto>(predicate: x => x.Id == selectedPresetsProfile.Id);
                var profileToDelete = Profiles.Single(x => x.Id == selectedPresetsProfile.Id);
                Profiles.Remove(item: profileToDelete);
            }
        }
    }
}
