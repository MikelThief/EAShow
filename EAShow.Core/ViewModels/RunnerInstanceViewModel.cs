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
using LiteDB;
using Nito.Mvvm;

namespace EAShow.Core.ViewModels
{
    public class RunnerInstanceViewModel : Screen
    {
        private short _index;
        private string _header;
        private Profile _selectedProfile;
        private bool _isProfileLoaded;

        private readonly CancellationTokenSource _cancellationTokenSource;


        public bool IsProfileLoaded
        {
            get => _isProfileLoaded;
            set => Set(oldValue: ref _isProfileLoaded, newValue: value, propertyName: nameof(IsProfileLoaded));
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

        public Profile SelectedProfile
        {
            get => _selectedProfile;
            set => Set(oldValue: ref _selectedProfile, newValue: value, propertyName: nameof(SelectedProfile));
        }

        public BindableCollection<Profile> Profiles { get; set; }

        public AsyncCommand<Profile> OpenProfileCommand { get; set; }

        public DelegateCommand<Profile> DeleteProfileCommand { get; set; }

        public RunnerInstanceViewModel()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Profiles = new BindableCollection<Profile>();
            DeleteProfileCommand = new DelegateCommand<Profile>(executeMethod: DeleteProfile);
            OpenProfileCommand = new AsyncCommand<Profile>(executeAsync: OpenProfile);
        }

        private Task OpenProfile(Profile selectedProfile)
        {
            // load profile. switch to charts and start fun
            return Task.CompletedTask;
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (!IsProfileLoaded)
            {
                using (var db = new LiteRepository(connectionString: LiteDbConnectionStringHelper.GetConnectionString()))
                {
                    var queryResult = db.Query<Profile>().ToEnumerable();
                    Profiles.AddRange(items: queryResult);
                }
            }

            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            Profiles.Clear();
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        private void DeleteProfile(Profile selectedProfile)
        {
            using (var db = new LiteRepository(connectionString: LiteDbConnectionStringHelper.GetConnectionString()))
            {
                db.Delete<Profile>(predicate: x => x.Id == selectedProfile.Id);
                var profileToDelete = Profiles.Single(x => x.Id == selectedProfile.Id);
                Profiles.Remove(item: profileToDelete);
            }
        }
    }
}
