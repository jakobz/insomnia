using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia
{
    interface IMutateLens<From, To>: IMap<From, To>
    {
        void Update(From from, To to);
    }
}
