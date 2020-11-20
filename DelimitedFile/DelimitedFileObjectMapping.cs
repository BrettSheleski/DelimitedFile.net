#if NET5_0 || NET45 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sheleski.DelimitedFile
{
    public class DelimitedFileObjectMapping<T>
    {


        public List<IDelimitedFileObjectColumnMapping<T>> Columns { get; } = new List<IDelimitedFileObjectColumnMapping<T>>();

        public static DelimitedFileObjectMapping<T> GetDefault()
        {
            var mapping = new DelimitedFileObjectMapping<T>();

            var mapData = GetProperties();

            foreach (var propertyMapping in mapData.OrderBy(x => x.Order))
            {
                mapping.AddProperty(propertyMapping.Property, propertyMapping.Header);
            }

            return mapping;
        }

        

        public IDelimitedFileObjectColumnMapping<T> AddProperty(PropertyInfo property, string header)
        {
            var propertyMap = new DelimitedFileObjectPropertyMapping<T>(property, header);

            this.Columns.Add(propertyMap);

            return propertyMap;
        }

        private class PropertyMappingInfo
        {
            public PropertyInfo Property { get; set; }
            public int Order { get; set; }
            public string Header { get; internal set; }
        }

        static IEnumerable<PropertyMappingInfo> GetProperties()
        {
            IEnumerable<PropertyInfo> properties;
            DisplayAttribute displayAttribute;

            var theType = typeof(T);

            properties = theType.GetProperties();

            int order;
            string header;

            foreach (var property in properties)
            {
                header = property.Name;
                order = 0;

                displayAttribute = property.GetCustomAttributes(true).OfType<DisplayAttribute>().FirstOrDefault();

                if (displayAttribute != null)
                {
                    order = displayAttribute.GetOrder() ?? 0;

                    if (order < 0)
                    {
                        // hidden property, do not include
                        continue;
                    }

                    header = displayAttribute.GetName() ?? header;
                }

                yield return new PropertyMappingInfo
                {
                    Order = order,
                    Header = header,
                    Property = property
                };
            }
        }

        public DelimitedFileObjectPropertyMapping<T, TProp> AddProperty<TProp>(Expression<Func<T, TProp>> propertyExpression)
        {
            DelimitedFileObjectPropertyMapping<T, TProp> propertyMapping;

            propertyMapping = new DelimitedFileObjectPropertyMapping<T, TProp>(propertyExpression);

            Columns.Add(propertyMapping);

            return propertyMapping;
        }

        public DelimitedFileObjectColumnComputedMapping<T> Add(string header, Func<T, int, string> func)
        {
            DelimitedFileObjectColumnComputedMapping<T> column = new DelimitedFileObjectColumnComputedMapping<T>(header, func);

            this.Columns.Add(column);

            return column;
        }
    }
}
#endif