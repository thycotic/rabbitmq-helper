using System;
using System.Linq;

namespace Thycotic.Utility.Reflection
{
    /// <summary>
    /// Type helper methods
    /// </summary>
    public static class TypeHelper
    {
        //source: http://stackoverflow.com/questions/74616/how-to-detect-if-type-is-another-generic-type/1075059#1075059
        /// <summary>
        /// Determines whether the specified type is assignable to the specified generic.
        /// </summary>
        /// <param name="givenType">Type of the given.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <returns></returns>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
            {
                return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }
    }
}
