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
    public class InnerClassMapping
    {
        public class ModelInner
        {
            public int Id { get; set; }
            public int InternalId { get; set; }
            public string Name { get; set; }
        }

        public class Model
        {
            public ModelInner ModelInner { get; set; }
        }

        public class ViewModelInner
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class ViewModel
        {
            public ViewModelInner ViewModelInner { get; set;}
        }

        public void Map(ClassMapper<Model, ViewModel> map)
        {
            map.Object(m => m.ModelInner, vm => vm.ViewModelInner, im =>
            {
                im.Scalar(m => m.Id, vm => vm.Id);
                im.Scalar(m => m.Name, vm => vm.Name);
            });
        }

        [Test]
        public void InnerClassGetTest()
        {
            var model = new Model()
            {
                ModelInner = new ModelInner() 
                {
                    Id = 42,
                    Name = "Hello"
                }
            };
            
            var viewModel = new ViewModel();

            Map(new GetMapper<Model, ViewModel>(model, viewModel));

            Assert.AreEqual(42, viewModel.ViewModelInner.Id);
            Assert.AreEqual("Hello", viewModel.ViewModelInner.Name);
        }

        [Test]
        public void InnerClassUpdateTest()
        {
            var model = new ViewModel()
            {
                ViewModelInner = new ViewModelInner() 
                {
                    Id = 100500,
                    Name = "Hi there"
                }
            };

            var newModel = new Model();

            Map(new UpdateMapper<Model, ViewModel>(newModel, model));

            Assert.AreEqual(100500, newModel.ModelInner.Id);
            Assert.AreEqual("Hi there", newModel.ModelInner.Name);
        }

        [Test]
        public void InnerClassNullGetTest()
        {
            var model = new Model()
            {
                ModelInner = null
            };

            var viewModel = new ViewModel();

            Map(new GetMapper<Model, ViewModel>(model, viewModel));

            Assert.AreEqual(null, viewModel.ViewModelInner);
        }

        [Test]
        public void InnerClassNullSetTest()
        {
            var model = new Model()
            {
                ModelInner = new ModelInner()
                {
                    Name = "Test"
                }
            };

            var editedModel = new ViewModel()
            {
                ViewModelInner = null
            };

            Map(new UpdateMapper<Model, ViewModel>(model, editedModel));

            Assert.AreEqual(null, model.ModelInner);
        }

        [Test]
        public void InnerClassOverwriteTest()
        {
            var model = new Model()
            {
                ModelInner = new ModelInner()
                {
                    InternalId = 42,
                    Name = "Test"
                }
            };

            var editedModel = new ViewModel()
            {
                ViewModelInner = new ViewModelInner()
                {
                    Name = "Hello"
                }
            };

            Map(new UpdateMapper<Model, ViewModel>(model, editedModel));

            Assert.AreEqual(42, model.ModelInner.InternalId);
            Assert.AreEqual("Hello", model.ModelInner.Name);
        }
    }
}
