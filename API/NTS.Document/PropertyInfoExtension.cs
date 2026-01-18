using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Document
{
    public static class PropertyInfoExtension
    {
        public static bool IsListProperty(this PropertyInfo property)
        {
            Type propertyType = property.PropertyType;

            if (propertyType.IsGenericType)
            {
                Type genericTypeDefinition = propertyType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(List<>))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
