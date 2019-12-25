using System;
using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;

namespace EAShow.Shared.Abstractions.Bases
{
    public abstract class PrimitiveValueObject<T> : ValueObject
    {
        public T Value { get; }

        protected PrimitiveValueObject(T value) { Value = value; }

        protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }
    }
}
