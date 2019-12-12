using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Core.Core.Models;
using EAShow.Core.Helpers;
using EAShow.Infrastructure.Commands.DelegateCommand;
using LiteDB;
using Nito.Mvvm;

namespace EAShow.Core.ViewModels
{
    public class RunnerInstanceViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private short _index;
        private string _header;
        private string _profileName;

        private readonly CancellationTokenSource _cancellationTokenSource;

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

        public string ProfileName
        {
            get => _profileName;
            set => Set(oldValue: ref _profileName, newValue: value.Trim(), propertyName: nameof(ProfileName));
        }

        public CustomAsyncCommand OpenProfileCommand { get; set; }

        public RunnerInstanceViewModel(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
        }

        private async Task OpenProfile()
        {
            using (var db = new LiteRepository(connectionString: LiteDbConnectionStringHelper.GetConnectionString()))
            {
                var queryResult = db.Query<Profile>().SingleOrDefault();

                if (queryResult == null)
                {
                    // raise some notification
                }
                else
                {
                    await ActivateItemAsync(item: new EAViewModel(), _cancellationTokenSource.Token);
                }
            }
        }

        private bool CanOpenProfile => string.IsNullOrEmpty(value: ProfileName);
    }
}
