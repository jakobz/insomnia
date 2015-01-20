using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia
{
    public interface IMap<From, To>
    {
        To Get(From from);
    }
}
