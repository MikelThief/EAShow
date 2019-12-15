using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Shared.Models;
using EAShow.Core.Helpers;
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

        public BindableCollection<Profile> Profiles { get; private set; }

        public CustomAsyncCommand OpenProfileCommand { get; set; }

        public RunnerInstanceViewModel()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private async Task LoadProfile()
        {
            using (var db = new LiteRepository(connectionString: LiteDbConnectionStringHelper.GetConnectionString()))
            {
                var queryResult = db.Query<Profile>().Where(x => x.Id == SelectedProfile.Id).Single();

                if (queryResult == null)
                {
                    // raise some notification
                }
                else
                {
                    // load profile
                }
            }
        }

        private bool CanOpenProfile => SelectedProfile != null;

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            using (var db = new LiteRepository(connectionString: LiteDbConnectionStringHelper.GetConnectionString()))
            {
                var queryResult = db.Query<Profile>().ToEnumerable();
                Profiles = new BindableCollection<Profile>(queryResult);
            }

            return base.OnInitializeAsync(cancellationToken);
        }
    }
}
