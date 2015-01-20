using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Insomnia.Common;

namespace Insomnia.Mappers
{
    public class GetMapper<From, To> : ClassMapper<From, To>
    {
        protected From from;
        protected To to;

        public GetMapper(From from, To to)
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
            ToProp value = transform.Get(fromPropMeta.Getter(from));
            toPropMeta.Setter(to, value);
        }

        public override void Object<FromClass, ToClass>(
            Expression<Func<From, FromClass>> fromProp,
            Expression<Func<To, ToClass>> toProp,
            Action<ClassMapper<FromClass, ToClass>> map
        )
        {
            var fromPropMeta = PropertyMetadata<From, FromClass>.Get(fromProp);
            var toPropMeta = PropertyMetadata<To, ToClass>.Get(toProp);

            FromClass fromClass = fromPropMeta.Getter(from);

            if (fromClass != null)
            {
                ToClass toClass = new ToClass();

                var mapper = new GetMapper<FromClass, ToClass>(fromClass, toClass);
                map(mapper);
                toPropMeta.Setter(to, toClass);
            }
        }

        public override void MergeMutableCollection<FromProp, ToProp, FromItem, ToItem, Key>(
            Expression<Func<From, FromProp>> fromProp,
            Expression<Func<To, ToProp>> toProp,
            Func<FromProp> getBlankModelCollection,
            Func<IEnumerable<ToItem>, ToProp> convertToViewCollection,
            Func<FromItem, Key> getModelKey,
            Func<ToItem, Key> getViewModelKey,
            Action<ClassMapper<FromItem, ToItem>> mapItem
        )
        {
            var fromPropMeta = PropertyMetadata<From, FromProp>.Get(fromProp);
            var toPropMeta = PropertyMetadata<To, ToProp>.Get(toProp);

            var fromValue = fromPropMeta.Getter(from);
            var toItems = fromValue.Select(fromItem => {
                var toItem = new ToItem();
                var itemMapper = new GetMapper<FromItem, ToItem>(fromItem, toItem);
                mapItem(itemMapper);
                return toItem;
            });

            toPropMeta.Setter(to, convertToViewCollection(toItems));
        }
    }
}
