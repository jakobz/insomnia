using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insomnia;
using Insomnia.Mappers;
using NUnit.Framework;

namespace Tests.UnitTests
{
    [TestFixture]
    public class NullablesMapping
    {
        public class Model
        {
            public int? A { get; set; }
            public decimal B { get; set; }
        }

        public class ViewModel
        {
            public int A { get; set; }
            public decimal? B { get; set; }
        }

        public void Map(ClassMapper<Model, ViewModel> map)
        {
            map.Scalar(m => m.A, vm => vm.A);
            map.Scalar(m => m.B, vm => vm.B);
        }

        [Test]
        public void NullablesMappingTest()
        {
            var model = new Model()
            {
                A = 1, B = 2
            };
            
            var viewModel = new ViewModel();

            Map(new GetMapper<Model, ViewModel>(model, viewModel));

            Assert.AreEqual(1, viewModel.A);
            Assert.AreEqual(2, viewModel.B);

            viewModel.A = 3;
            viewModel.B = 4;
            Map(new UpdateMapper<Model, ViewModel>(model, viewModel));

            Assert.AreEqual(3, model.A);
            Assert.AreEqual(4, model.B);
        }
    }
}
