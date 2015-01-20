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
    public class CollectionMapping
    {
        public class ModelItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public class Model
        {
            public List<ModelItem> ModelItems { get; set; }
        }

        public class ViewModelItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public class ViewModel
        {
            public ViewModelItem[] ViewModelItems { get; set; }  
        }

        public void Map(ClassMapper<Model, ViewModel> map)
        {
            map.ListToArray(m => m.ModelItems, vm => vm.ViewModelItems, m => m.ID, vm => vm.ID, mapItem =>
            {
                mapItem.Scalar(m => m.ID, vm => vm.ID);
                mapItem.Scalar(m => m.Name, vm => vm.Name);
            });
        }

        [Test]
        public void IsListToArrayWorksInViewDirection()
        {
            var model = new Model()
            {
                ModelItems = new List<ModelItem>(
                    new ModelItem[] {
                        new ModelItem { ID = 1, Name = "One" },
                        new ModelItem { ID = 2, Name = "Two"}
                    }
                )
            };
            
            var viewModel = new ViewModel();

            Map(new GetMapper<Model, ViewModel>(model, viewModel));

            Assert.AreEqual("One", viewModel.ViewModelItems[0].Name);
            Assert.AreEqual(2, viewModel.ViewModelItems[1].ID);
        }

        [Test]
        public void IsListToArrayWorksInModelDirection()
        {
            var model = new Model()
            {
                ModelItems = new List<ModelItem>(
                    new ModelItem[] {
                        new ModelItem { ID = 1, Name = "One" },
                        new ModelItem { ID = 2, Name = "Two"}
                    }
                )
            };

            var viewModel = new ViewModel()
            {
                ViewModelItems = new ViewModelItem[] {
                    new ViewModelItem { ID = 2, Name = "Updated" },
                    new ViewModelItem { ID = 3, Name = "Three"}
                }
            };

            Map(new UpdateMapper<Model, ViewModel>(model, viewModel));

            Assert.IsNull(model.ModelItems.SingleOrDefault(m => m.ID == 1));
            Assert.AreEqual("Updated", model.ModelItems.Single(m => m.ID == 2).Name);
            Assert.AreEqual("Three", model.ModelItems.Single(m => m.ID == 3).Name);
        }

    }
}
