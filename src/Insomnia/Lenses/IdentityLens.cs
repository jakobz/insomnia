using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia.Lenses
{
    public class IdentityLens<T> : ILens<T, T>
    {
        public T Get(T from)
        {
            return from;
        }

        public T Put(T to)
        {
            return to;
        }
    }
}
