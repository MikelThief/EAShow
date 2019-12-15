using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAShow.Shared.Abstractions.Interfaces
{
    public interface IPreset
    {
        Task PublishEnabledCount();
    }
}
