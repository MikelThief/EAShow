using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EAShow.Shared.Events;

namespace EAShow.Shared.Abstractions.Interfaces
{
    public interface IPreset : IHandle<PresetResetRequestedEvent>
    {
        Task PublishEnabledCountAsync();
        Task RestoreDefaultsAsync();
    }
}
