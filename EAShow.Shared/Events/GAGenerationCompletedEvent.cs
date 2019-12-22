using System;
using System.Collections.Generic;
using System.Text;
using EAShow.Shared.Models;

namespace EAShow.Shared.Events
{
    public class GAGenerationCompletedEvent
    {
        public readonly FOGenerationCompletedDto Dto;

        public readonly Guid Sender;

        public GAGenerationCompletedEvent(FOGenerationCompletedDto dto, Guid sender)
        {
            Dto = dto;
            Sender = sender;
        }
    }
}
