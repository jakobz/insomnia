using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Insomnia.Common;

namespace Insomnia.Mappers
{
    public class UpdateMapper<From, To> : ClassMapper<From, To>
    {
        protected From from;
        protected To to;

        public UpdateMapper(From from, To to)
        {
            this.from = from;
            this.to = to;
        }

        public override void Scalar<FromProp, ToProp>(
            Expression<Func<From, FromProp>> fromProp,
            Expression<Func<To, ToProp>> toProp,
            ILens<FromProp, ToProp> transform)
        {
            var fromPropMeta = PropertyMetadata<From, FromProp>.Get(fromProp);
            var toPropMeta = PropertyMetadata<To, ToProp>.Get(toProp);
            FromProp value = transform.Put(toPropMeta.Getter(to));
            fromPropMeta.Setter(from, value);
        }

        public override void Object<FromClass, ToClass>(
            Expression<Func<From, FromClass>> fromProp,
            Expression<Func<To, ToClass>> toProp,
            Action<ClassMapper<FromClass, ToClass>> map
        )
        {
            var fromPropMeta = PropertyMetadata<From, FromClass>.Get(fromProp);
            var toPropMeta = PropertyMetadata<To, ToClass>.Get(toProp);

            ToClass toClass = toPropMeta.Getter(to);

            if (toClass != null)
            {
                FromClass fromClass = fromPropMeta.Getter(from);

                if (fromClass == null)
                {
                    fromClass = new FromClass();
                    fromPropMeta.Setter(from, fromClass);
                }

                var mapper = new UpdateMapper<FromClass, ToClass>(fromClass, toClass);
                map(mapper);
            }
            else
            {
                fromPropMeta.Setter(from, null);
            }
        }

        public override void MergeMutableCollection<FromProp, ToProp, FromItem, ToItem, Key>(
            Expression<Func<From, FromProp>> fromProp, 
            Expression<Func<To, ToProp>> toProp,
            Func<FromProp> getBlankModelCollection,
            Func<IEnumerable<ToItem>, ToProp> convertViewCollection,
            Func<FromItem, Key> getModelKey,
            Func<ToItem, Key> getViewModelKey,
            Action<ClassMapper<FromItem, ToItem>> mapItem)
        {
            var fromPropMeta = PropertyMetadata<From, FromProp>.Get(fromProp);
            var toPropMeta = PropertyMetadata<To, ToProp>.Get(toProp);

            var modelCollection = fromPropMeta.Getter(from);

            if (modelCollection == null)
            {
                modelCollection = getBlankModelCollection();
                fromPropMeta.Setter(from, modelCollection);
            }

            var viewModelItems = toPropMeta.Getter(to);

            var modelDictionary = modelCollection.ToDictionary(getModelKey);
            var viewModelItemsWithKeys = viewModelItems.Select(i => new { Key = getViewModelKey(i), Item = i });

            // Added
            foreach (var addedItem in viewModelItemsWithKeys.Where(i => !modelDictionary.ContainsKey(i.Key)))
            {
                var newModelItem = new FromItem();
                var itemMapper = new UpdateMapper<FromItem, ToItem>(newModelItem, addedItem.Item);
                mapItem(itemMapper);
                modelCollection.Add(newModelItem);
                modelDictionary.Remove(addedItem.Key);
            }

            // Updated
            foreach (var updatedItem in viewModelItemsWithKeys.Where(i => modelDictionary.ContainsKey(i.Key)))
            {
                var modelItem = modelDictionary[updatedItem.Key];
                var itemMapper = new UpdateMapper<FromItem, ToItem>(modelItem, updatedItem.Item);
                mapItem(itemMapper);
                modelDictionary.Remove(updatedItem.Key);
            }

            // Deleted
            foreach (var deletedModelItem in modelDictionary.Values)
            {
                modelCollection.Remove(deletedModelItem);
            }
        }
    }
}
