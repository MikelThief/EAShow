using System;
using System.Collections.Generic;
using System.Text;
using EAShow.Shared.Abstractions.Bases;

namespace EAShow.Shared.Models
{
    public class Mutation : PrimitiveValueObject<decimal>
    {
        public Mutation(decimal value) : base(value)
        {
        }
    }
}
