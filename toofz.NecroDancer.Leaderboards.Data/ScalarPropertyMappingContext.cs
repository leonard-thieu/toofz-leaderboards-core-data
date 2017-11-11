using System;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq.Expressions;
using System.Reflection;

namespace toofz.NecroDancer.Leaderboards
{
    internal sealed class ScalarPropertyMappingContext
    {
        public ScalarPropertyMappingContext(ScalarPropertyMapping propertyMapping, Type entityDeclaringType)
        {
            Property = propertyMapping.Property;
            EntityDeclaringType = entityDeclaringType;
            PropertyInfo = entityDeclaringType.GetTypeInfo().GetDeclaredProperty(Property.Name) ??
                throw new ArgumentException($"Unable to find property '{Property.Name}' on '{entityDeclaringType.Name}'.");
            Column = propertyMapping.Column;
        }

        public EdmProperty Property { get; }
        public Type EntityDeclaringType { get; }
        public PropertyInfo PropertyInfo { get; }
        public EdmProperty Column { get; }

        public Func<object, object> ValueGetter => valueGetter ?? (valueGetter = CreatePropertyGetter());
        private Func<object, object> valueGetter;

        private Func<object, object> CreatePropertyGetter()
        {
            var getter = PropertyInfo.GetMethod;
            var propertyType = PropertyInfo.PropertyType;
            var entityParameter = Expression.Parameter(typeof(object), "entity");
            Expression getterExpression = Expression.Property(Expression.Convert(entityParameter, EntityDeclaringType), PropertyInfo);

            if (propertyType.GetTypeInfo().IsValueType)
            {
                getterExpression = Expression.Convert(getterExpression, typeof(object));
            }

            return Expression.Lambda<Func<object, object>>(getterExpression, entityParameter).Compile();
        }
    }
}
