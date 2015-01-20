using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia.Lenses
{
    public class NonNullableToNullableLens<T> : ILens<T, T?> where T: struct
    {
        T defaultValue;

        public NonNullableToNullableLens(T defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public T? Get(T from)
        {
            return from;
        }

        public T Put(T? to)
        {
            return to ?? defaultValue;
        }
    }
}
