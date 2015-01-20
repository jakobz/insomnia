using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia.Tests.Example.Infrastructure
{
    class FormRequest<T>
    {
        public int? ID { get; set; }
        public T ViewState { get; set; }
    }
}
