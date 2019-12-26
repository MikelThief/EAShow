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

        public readonly Guid Invoker;

        public GAGenerationCompletedEvent(FOGenerationCompletedDto dto, Guid sender, Guid invoker)
        {
            Dto = dto;
            Sender = sender;
            Invoker = invoker;
        }
    }
}
