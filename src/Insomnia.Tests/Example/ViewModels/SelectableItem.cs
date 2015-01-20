using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia.Tests.Example.ViewModels
{
    class SelectableItem<T> 
    {
        public T ID { get; set; }
        public string Name { get; set; }
    }
}
