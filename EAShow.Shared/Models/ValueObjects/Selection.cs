using System;
using System.Collections.Generic;
using System.Text;
using EAShow.Shared.Abstractions.Bases;

namespace EAShow.Shared.Models
{
    public class Selection : PrimitiveValueObject<Selections>
    {
        public Selection(Selections value) : base(value)
        {
        }
    }
}
