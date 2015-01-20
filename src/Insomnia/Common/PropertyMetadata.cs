using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Insomnia.Common
{
    public class PropertyMetadata<Model, Prop>
    {
        public readonly Func<Model, Prop> Getter;
        public readonly Action<Model, Prop> Setter;

        public static PropertyMetadata<Model, Prop> Get(Expression<Func<Model, Prop>> expression)
        {
            return new PropertyMetadata<Model, Prop>(expression);
        }

        PropertyMetadata(Expression<Func<Model, Prop>> expression)
        {
            Getter = expression.Compile();
            Setter = ExpressionTools.GetterToSetter(expression);
        }
    }
}
