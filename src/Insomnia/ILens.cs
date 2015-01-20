using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia
{
    public interface ILens<From, To>: IMap<From, To>
    {
        From Put(To to);
    }
}
