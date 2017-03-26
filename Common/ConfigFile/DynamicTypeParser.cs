using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Common.ConfigFile
{
    public class DynamicTypeParser
    {
        public const BindingFlags MemberAccess =
          BindingFlags.Public | BindingFlags.NonPublic |
          BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;

        public const BindingFlags MemberPublicInstanceAccess =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;


        public void ParseExpandoObject<T>(T obj, ExpandoObject expandoObject)
        {
            var members =(obj.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property, MemberAccess, null, null)).ToList();

            var dictionaryExpandoObject = (IDictionary<string, object>)expandoObject;
            foreach (var key in dictionaryExpandoObject)
            {
                var existingMember = members.FirstOrDefault(x => x.Name == key.Key);
                if (existingMember == null) continue;
                var propertyInfo = obj.GetType().GetProperty(existingMember.Name);

                var value = StringToTypeConvertor.Convert(key.Value.ToString(), propertyInfo.PropertyType);
                propertyInfo.SetValue(obj, value);
            }
        }

        public void Parse<T>(T obj, IDictionary<string, List<object>> lookupDictionary)
        {
            var objLookupValues = lookupDictionary[obj.GetType().Name];
            var objProperties = (obj.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property, MemberAccess, null, null)).ToList();
            foreach (var objLookupValue in objLookupValues)
            {
                (new TypeSwitch()
                    .Case((ExpandoObject parentNodes) => ParseExpandoObject(obj, parentNodes))
                    .Case((Dictionary<string, List<object>> parentNodes) =>
                    {
                        foreach (var parentNode in parentNodes)
                        {
                            var parentProperty = objProperties.FirstOrDefault(x => x.Name == parentNode.Key);
                            if (parentProperty == null) continue;
                            var parentPropertyInfo = obj.GetType().GetProperty(parentProperty.Name);

                            var parentObject = Activator.CreateInstance(parentPropertyInfo.PropertyType);
                            foreach (var childNodeLookup in parentNode.Value)
                            {
                                (new TypeSwitch()
                                 .Case((ExpandoObject childNodes) => ParseExpandoObject(parentObject, childNodes))
                                 .Case((Dictionary<string, List<object>> childNodes) =>
                                 {
                                     var tempImmediateParentObject = Activator.CreateInstance(parentObject.GetType().GetGenericArguments()[0]);
                                     Parse(tempImmediateParentObject, childNodes);
                                     ((IList)parentObject).Add(tempImmediateParentObject);
                                 })).Switch(childNodeLookup);
                            }
                            parentPropertyInfo.SetValue(obj, parentObject);
                        }
                    })).Switch(objLookupValue);
            }
        }
    }
}
