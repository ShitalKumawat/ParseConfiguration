using Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Xml;

namespace Common.ConfigFile
{
    public class ConfigurationSectionHandler: IConfigurationSectionHandler 
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ExpandoObject root = new ExpandoObject();
            XmlNodeToDynamicObject.Parse(root, section);

            var obj = GetInstance(section.Name);

            var dynamicTypeParser = new DynamicTypeParser();
            var defaultMember = (IDictionary<String, List<object>>)(root.ToList()[0].Value);
            dynamicTypeParser.Parse(obj, defaultMember);

            return obj;
        }

        public object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetTypes().FirstOrDefault(assemblyType=> assemblyType.Name==strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
