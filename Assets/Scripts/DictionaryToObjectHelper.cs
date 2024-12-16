using System;
using System.Collections.Generic;

public static class DictionaryToObjectHelper
{
    public static T ToObject<T>(Dictionary<string, object> dictionary) where T : new()
    {
        var obj = new T();
        var objType = typeof(T);

        foreach (var kvp in dictionary)
        {
            var property = objType.GetProperty(kvp.Key);
            if (property != null && property.CanWrite)
            {
                // Convert the value to the correct property type if necessary
                var value = Convert.ChangeType(kvp.Value, property.PropertyType);
                property.SetValue(obj, value);
            }
        }

        return obj;
    }
}
