#if NET5_0 || NET45 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sheleski.DelimitedFile
{
    public interface IDelimitedFileObjectColumnMapping<TSource>
    {
        string Header { get; }
        string GetValue(int rowIndex, TSource source);
    }

    public class DelimitedFileObjectColumnComputedMapping<TSource> : IDelimitedFileObjectColumnMapping<TSource>
    {
        public DelimitedFileObjectColumnComputedMapping(string header, Func<TSource, int, string> valueAccessor)
        {
            this.Header = header;
            this.ValueAccessor = valueAccessor;
        }

        

        public string Header { get; }
        public Func<TSource, int, string> ValueAccessor { get; }

        public string GetValue(int rowIndex, TSource source) => this.ValueAccessor.Invoke(source, rowIndex);
    }

    public abstract class DelimitedFileObjectPropertyMappingBase<TSource> : IDelimitedFileObjectColumnMapping<TSource>
    {
        public string Header { get; set; }
        public abstract string GetValue(int rowIndex, TSource source);
    }

    public class DelimitedFileObjectPropertyMapping<TSource> : DelimitedFileObjectPropertyMappingBase<TSource>
    {
        

        private Func<int, TSource, string> ValueAccessor
        {
            get; set;
        }

        public DelimitedFileObjectPropertyMapping(PropertyInfo property, string header)
        {
            this.Header = header;

            var accessor = GetValueAccessor(property);

            this.ValueAccessor = (i, x) => accessor(x);

        }

        static readonly Type stringType = typeof(string);
        static readonly Type nullableType = typeof(Nullable<>);

        private Func<TSource, string> GetValueAccessor(PropertyInfo property)
        {

            Type sourceType = typeof(TSource);

            Type returnType = property.PropertyType;

            if (returnType == stringType)
            {
                return GetValueAccessorFromString(property, sourceType);
            }
            else if (IsNullableType(returnType))
            {
                return GetValueAccessorFromNullableType(property, sourceType, returnType);
            }
            else if (returnType.IsValueType)
            {
                return GetValueAccessorFromValueType(property, sourceType, returnType);
            }
            else
            {
                return GetValueAccessorFromReferenceType(property, sourceType, returnType);
            }

        }

        private bool IsNullableType(Type returnType)
        {
            return returnType.IsGenericType && returnType.GetGenericTypeDefinition() == nullableType;
        }

        private Func<TSource, string> GetValueAccessorFromNullableType(PropertyInfo sourceProperty, Type sourceType, Type returnType)
        {
            // source => source.Property.HasValue ? source.Property.Value.ToString() : null

            ParameterExpression source = Expression.Parameter(sourceType, "source");
            MemberExpression property = Expression.Property(source, sourceProperty);

            Expression propertyHasvalue = Expression.Property(property, "HasValue");

            Expression propertyValue = Expression.Property(property, "Value");
            MethodCallExpression toString = Expression.Call(propertyValue, "ToString", Type.EmptyTypes);
            ConstantExpression theNull = Expression.Constant(null, stringType);

            ConditionalExpression ternary = Expression.Condition(propertyHasvalue, toString, theNull);

            Expression<Func<TSource, string>> lambda = Expression.Lambda<Func<TSource, string>>(ternary, source);

            return lambda.Compile();
        }


        private Func<TSource, string> GetValueAccessorFromReferenceType(PropertyInfo sourceProperty, Type sourceType, Type returnType)
        {
            // source => source.Property == null ? null : source.Property.ToString()

            ParameterExpression source = Expression.Parameter(sourceType, "source");
            MemberExpression property = Expression.Property(source, sourceProperty);

            BinaryExpression condition = Expression.Equal(property, Expression.Constant(null, returnType));
            MethodCallExpression toString = Expression.Call(property, "ToString", Type.EmptyTypes);
            ConstantExpression theNull = Expression.Constant(null, stringType);
            ConditionalExpression ternary = Expression.Condition(condition, theNull, toString);

            Expression<Func<TSource, string>> lambda = Expression.Lambda<Func<TSource, string>>(ternary, source);

            return lambda.Compile();
        }

        private Func<TSource, string> GetValueAccessorFromValueType(PropertyInfo sourceProperty, Type sourceType, Type returnType)
        {
            // source => source.Property.ToString();

            ParameterExpression source = Expression.Parameter(sourceType, "source");
            MemberExpression property = Expression.Property(source, sourceProperty);
            MethodCallExpression toString = Expression.Call(property, "ToString", Type.EmptyTypes);

            Expression<Func<TSource, string>> lambda = Expression.Lambda<Func<TSource, string>>(toString, source);

            return lambda.Compile();
        }

        private Func<TSource, string> GetValueAccessorFromString(PropertyInfo sourceProperty, Type sourceType)
        {
            // source => source.Property

            ParameterExpression parameter = Expression.Parameter(sourceType, "source");
            MemberExpression property = Expression.Property(parameter, sourceProperty);

            Expression<Func<TSource, string>> lambda = Expression.Lambda<Func<TSource, string>>(property, parameter);

            return lambda.Compile();
        }

        public override string GetValue(int rowIndex, TSource source)
        {
            return this.ValueAccessor?.Invoke(rowIndex, source);
        }
    }

    public class DelimitedFileObjectPropertyMapping<TSource, TProperty> : DelimitedFileObjectPropertyMappingBase<TSource>
    {
        public DelimitedFileObjectPropertyMapping(Expression<Func<TSource, TProperty>> propertyAccessor) : this(propertyAccessor.Compile(), GetPropertyInfo(propertyAccessor).Name)
        {


        }
        public DelimitedFileObjectPropertyMapping(Func<TSource, TProperty> propertyAccessor, string header)
        {
            this.Header = header;

            this.PropertyAccessor = propertyAccessor;
        }


        public delegate string ValueAccessorDelegateValue(TProperty value);
        public delegate string ValueAccessorDelegate(int rowIndex, TSource source, TProperty value);

        private Func<TSource, TProperty> PropertyAccessor { get; }

        public ValueAccessorDelegate ValueAccessor { get; set; } = (i, source, val) => val?.ToString();


        public DelimitedFileObjectPropertyMapping<TSource, TProperty> WithHeader(string header)
        {
            this.Header = header;

            return this;
        }

        public DelimitedFileObjectPropertyMapping<TSource, TProperty> WithValue(ValueAccessorDelegateValue valueAccessor)
        {
            return WithValue((i, source, val) => valueAccessor(val));
        }

        public DelimitedFileObjectPropertyMapping<TSource, TProperty> WithValue(ValueAccessorDelegate func)
        {
            this.ValueAccessor = func;

            return this;
        }

        public override string GetValue(int rowIndex, TSource source)
        {
            return ValueAccessor?.Invoke(rowIndex, source, PropertyAccessor(source));
        }

        static PropertyInfo GetPropertyInfo(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }
    }
}
#endif