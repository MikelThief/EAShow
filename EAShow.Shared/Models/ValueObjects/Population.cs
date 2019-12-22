using EAShow.Shared.Abstractions.Bases;

namespace EAShow.Shared.Models
{
    public class Population : PrimitiveValueObject<decimal>
    {
        public Population(decimal value) : base(value)
        {
        }
    }
}
