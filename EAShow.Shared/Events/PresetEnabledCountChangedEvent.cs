using System;
using System.Collections.Generic;
using System.Text;
using EAShow.Core.Core.Models;

namespace EAShow.Core.Core.Events
{
    public class PresetEnabledCountChangedEvent
    {
        public Presets Preset { get; set; }

        public short Count { get; set; }
    }
}
