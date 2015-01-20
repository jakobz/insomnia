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
    public class PropertyMapping
    {
        public class Model
        {
            public int ModelId { get; set; }
            public string ModelName { get; set; }
        }

        public class ViewModel
        {
            public int ViewId { get; set; }
            public string ViewName { get; set; }
        }

        public void Map(ClassMapper<Model, ViewModel> map)
        {
            map.Scalar(m => m.ModelId, vm => vm.ViewId);
            map.Scalar(m => m.ModelName, vm => vm.ViewName);
        }

        [Test]
        public void BasicPropertiesMappingTest()
        {
            var model = new Model()
            {
                ModelId = 42,
                ModelName = "Hello"
            };
            
            var viewModel = new ViewModel();

            Map(new GetMapper<Model, ViewModel>(model, viewModel));

            Assert.AreEqual(42, viewModel.ViewId);
            Assert.AreEqual("Hello", viewModel.ViewName);

            var editedModel = new ViewModel();

            editedModel.ViewId = 100500;
            editedModel.ViewName = "Hi there";
            Map(new UpdateMapper<Model, ViewModel>(model, editedModel));

            Assert.AreEqual(100500, model.ModelId);
            Assert.AreEqual("Hi there", model.ModelName);
        }
    }
}
