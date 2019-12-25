using EAShow.Shared.Abstractions.Bases;

namespace EAShow.Shared.Models
{
    public class Population : PrimitiveValueObject<int>
    {
        public Population(int value) : base(value)
        {
        }
    }
}
