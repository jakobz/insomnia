using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insomnia.Lenses;

namespace Insomnia
{
    public static class Lens
    {
        public static IdentityLens<T> Identity<T>()
        {
            return new IdentityLens<T>();
        }
    }
}
