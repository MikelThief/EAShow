using System;
using System.Collections.Generic;
using System.Text;
using EAShow.Shared.Models;

namespace EAShow.Shared.Events
{
    public class PresetEnabledCountChangedEvent
    {
        public Presets Preset { get; set; }

        public short Count { get; set; }
    }
}
