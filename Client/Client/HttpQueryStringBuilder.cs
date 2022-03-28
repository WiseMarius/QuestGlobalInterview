using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

/// <summary>
///  Helps up build a query string by converting an object into
///  a set of named-values and making a query string out of it.
///
/// The following code is a black box implementation of the PHP's http_build_query function
/// http://php.net/manual/en/function.http-build-query.php
/// http://stackoverflow.com/questions/34043266/c-sharp-equivalent-of-php-http-build-query
/// </summary>
/// 

public class HttpQueryStringBuilder
{
    private readonly List<KeyValuePair<string, object>> _keyValuePairs
        = new List<KeyValuePair<string, object>>();

    /// <summary> Builds the query string from the given instance. </summary>
    public static string BuildQueryString(object queryData, string argSeperator = "&")
    {
        var encoder = new HttpQueryStringBuilder();
        encoder.AddEntry(null, queryData, allowObjects: true);

        return encoder.GetUriString(argSeperator);
    }

    /// <summary> Convert the key-value pairs that we've collected into an actual query string. </summary>
    private string GetUriString(string argSeperator)
    {
        return string.Join(argSeperator,
                            _keyValuePairs.Select(kvp => {
                                var key = Uri.EscapeDataString(kvp.Key);
                                var value = Uri.EscapeDataString(kvp.Value != null ? kvp.Value.ToString() : "");
                                return $"{key}={value}";
                            }));
    }

    /// <summary> Adds a single entry to the collection. </summary>
    /// <param name="prefix"> The prefix to use when generating the key of the entry. Can be null. </param>
    /// <param name="instance"> The instance to add.
    ///  
    ///  - If the instance is a dictionary, the entries determine the key and values.
    ///  - If the instance is a collection, the keys will be the index of the entries, and the value
    ///  will be each item in the collection.
    ///  - If allowObjects is true, then the object's properties' names will be the keys, and the
    ///  values of the properties will be the values.
    ///  - Otherwise the instance is added with the given prefix to the collection of items. </param>
    /// <param name="allowObjects"> true to add the properties of the given instance (if the object is
    ///  not a collection or dictionary), false to add the object as a key-value pair. </param>
    private void AddEntry(string prefix, object instance, bool allowObjects)
    {
        var dictionary = instance as IDictionary;
        var collection = instance as ICollection;

        if (dictionary != null)
        {
            Add(prefix, GetDictionaryAdapter(dictionary));
        }
        else if (collection != null)
        {
            Add(prefix, GetArrayAdapter(collection));
        }
        else if (allowObjects)
        {
            Add(prefix, GetObjectAdapter(instance));
        }
        else
        {
            _keyValuePairs.Add(new KeyValuePair<string, object>(prefix, instance));
        }
    }

    /// <summary> Adds the given collection of entries. </summary>
    private void Add(string prefix, IEnumerable<Entry> datas)
    {
        foreach (var item in datas)
        {
            var newPrefix = string.IsNullOrEmpty(prefix)
                ? item.Key
                : $"{prefix}[{item.Key}]";

            AddEntry(newPrefix, item.Value, allowObjects: false);
        }
    }

    private struct Entry
    {
        public string Key;
        public object Value;
    }

    /// <summary>
    ///  Returns a collection of entries that represent the properties on the object.
    /// </summary>
    private IEnumerable<Entry> GetObjectAdapter(object data)
    {
        var properties = data.GetType().GetProperties();

        foreach (var property in properties)
        {
            yield return new Entry()
            {
                Key = property.Name,
                Value = property.GetValue(data)
            };
        }
    }

    /// <summary>
    ///  Returns a collection of entries that represent items in the collection.
    /// </summary>
    private IEnumerable<Entry> GetArrayAdapter(ICollection collection)
    {
        int i = 0;
        foreach (var item in collection)
        {
            yield return new Entry()
            {
                Key = i.ToString(),
                Value = item,
            };
            i++;
        }
    }

    /// <summary>
    ///  Returns a collection of entries that represent items in the dictionary.
    /// </summary>
    private IEnumerable<Entry> GetDictionaryAdapter(IDictionary collection)
    {
        foreach (DictionaryEntry item in collection)
        {
            yield return new Entry()
            {
                Key = item.Key.ToString(),
                Value = item.Value,
            };
        }
    }
}

public class HttpQueryStringParser
{
    public static T GetFromQueryString<T>(NameValueCollection collection) where T : new()
    {
        var obj = new T();
        var properties = typeof(T).GetProperties(); // to get all properties from Class(Object)  
        foreach (var property in properties)
        {
            var valueAsString = collection[property.Name];
            object value = Parse(property.PropertyType, valueAsString); // parse data types  

            if (value == null) { continue; }

            property.SetValue(obj, value, null); //set values to properties.  
        }
        return obj;
    }
    // all in one parse method.   
    public static object Parse(Type dataType, string ValueToConvert)
    {
        TypeConverter obj = TypeDescriptor.GetConverter(dataType);
        object value = obj.ConvertFromString(null, CultureInfo.InvariantCulture, ValueToConvert);
        return value;
    }
}