using System;
using System.Collections.Generic;
using System.Text;
using EAShow.Shared.Abstractions.Bases;

namespace EAShow.Shared.Models
{
    public class Crossover : PrimitiveValueObject<Crossovers>
    {
        public Crossover(Crossovers value) : base(value)
        {

        }
    }
}
