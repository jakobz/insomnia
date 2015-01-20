using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Insomnia;
using Insomnia.Common;
using Insomnia.Lenses;
using Insomnia.Mappers;

namespace Insomnia
{
    public abstract class ClassMapper<From, To>
    {
        // Interface for mappers implementaions

        public abstract void Scalar<FromProp, ToProp>(
                Expression<Func<From, FromProp>> fromProp,
                Expression<Func<To, ToProp>> toProp,
                ILens<FromProp, ToProp> transform);

        public abstract void Object<FromClass, ToClass>(
                Expression<Func<From, FromClass>> fromProp,
                Expression<Func<To, ToClass>> toProp,
                Action<ClassMapper<FromClass, ToClass>> map
            )
            where FromClass : class, new()
            where ToClass : class, new();

        public abstract void MergeMutableCollection<FromProp, ToProp, FromItem, ToItem, Key>(
            Expression<Func<From, FromProp>> fromProp,
            Expression<Func<To, ToProp>> toProp,
            Func<FromProp> getBlankModelCollection,
            Func<IEnumerable<ToItem>, ToProp> convertViewCollection,
            Func<FromItem, Key> getModelKey,
            Func<ToItem, Key> getViewModelKey,
            Action<ClassMapper<FromItem, ToItem>> mapItem
        )
            where FromProp : ICollection<FromItem>
            where ToProp : IEnumerable<ToItem>
            where ToItem : new()
            where FromItem : new();

        // Helpers

        public void ListToArray<FromItem, ToItem, Key>(
            Expression<Func<From, List<FromItem>>> fromProp,
            Expression<Func<To, ToItem[]>> toProp,
            Func<FromItem, Key> getModelKey,
            Func<ToItem, Key> getViewModelKey,
            Action<ClassMapper<FromItem, ToItem>> mapItem
        )
            where ToItem: new()
            where FromItem: new()
        {
            this.MergeMutableCollection<List<FromItem>, ToItem[], FromItem, ToItem, Key>(
                fromProp,
                toProp,
                () => new List<FromItem>(),
                e => e.ToArray(),
                getModelKey,
                getViewModelKey,
                mapItem
            );
        }

        // These two overload are picked for nullables. 
        // Without them, C# can pick a variant for the same property types, and emit the cast from null to not-null into the expression
        // This would break the getter => setter generation
        // TBD: revisit this 

        public void Scalar<T>(
            Expression<Func<From, T?>> fromProp,
            Expression<Func<To, T>> toProp)
            where T : struct
        {
            Scalar<T?, T>(fromProp, toProp, new NullableToNonNullableLens<T>(default(T)));
        }

        public void Scalar<T>(
            Expression<Func<From, T>> fromProp,
            Expression<Func<To, T?>> toProp)
            where T : struct
        {
            Scalar<T, T?>(fromProp, toProp, new NonNullableToNullableLens<T>(default(T)));
        }

        // An overload for the same item types, w/o additional Lens parameter

        public void Scalar<T>(
            Expression<Func<From, T>> fromProp,
            Expression<Func<To, T>> toProp)
        {
            Scalar<T, T>(fromProp, toProp, Lens.Identity<T>());
        }
    }
}
