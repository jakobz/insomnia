using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia.Tests.Example.ViewModels
{
    class SelectableValue<T>
    {
        public T Value { get; set; }
        public SelectableItem<T> Options { get; set; }
    }
}
