using System;
using System.Collections.Generic;
using System.Reflection;

public static class ObjectToDictionaryHelper
{
    public static Dictionary<string, object> ToDictionary(object obj)
    {
        if (obj == null) return null;

        var dictionary = new Dictionary<string, object>();

        foreach (PropertyInfo property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.CanRead)
            {
                var value = property.GetValue(obj, null);

                // Recursively convert nested objects
                dictionary[property.Name] = value != null && !IsSimpleType(value.GetType())
                    ? ToDictionary(value)
                    : value;
            }
        }

        foreach (FieldInfo field in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = field.GetValue(obj);

            // Recursively convert nested objects
            dictionary[field.Name] = value != null && !IsSimpleType(value.GetType())
                ? ToDictionary(value)
                : value;
        }

        return dictionary;
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal);
    }

}
