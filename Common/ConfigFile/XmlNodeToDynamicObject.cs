using System.Collections.Generic;
using System.Dynamic;
using System.Xml;

namespace Common.ConfigFile
{
    public class XmlNodeToDynamicObject
    {
        public static void Parse(dynamic dynamicObject, XmlNode xmlParentNode)
        {
            if (xmlParentNode.HasChildNodes)
            {
                if (xmlParentNode.FirstChild.NodeType == XmlNodeType.Text)
                {
                    var innerTextTag = new ExpandoObject();
                    AddProperty(innerTextTag, xmlParentNode.Name, xmlParentNode.InnerText);
                    AddProperty(dynamicObject, xmlParentNode.Name, innerTextTag);
                }
                else
                {
                    var dynamicChildObject= new List<dynamic>();

                    foreach (XmlNode childNode in xmlParentNode.ChildNodes)
                    {
                        Parse(dynamicChildObject, childNode);
                    }

                    var parentChildNodeLookup = new Dictionary<string, List<dynamic>> { { xmlParentNode.Name, dynamicChildObject } };
                    var attributeTag = new ExpandoObject();
                    if (xmlParentNode.Attributes != null && xmlParentNode.Attributes.Count != 0)
                    {
                        foreach (XmlAttribute attribute in xmlParentNode.Attributes)
                        {
                            AddProperty(attributeTag, attribute.Name.ToString(), attribute.Value.Trim());
                        }
                        parentChildNodeLookup[xmlParentNode.Name].Add(attributeTag);
                    }

                    AddProperty(dynamicObject, xmlParentNode.Name, parentChildNodeLookup);
                }
            }
            else
            {
                var attributeTag = new ExpandoObject();

                if (xmlParentNode.Attributes != null)
                {
                    foreach (XmlAttribute attribute in xmlParentNode.Attributes)
                    {
                        AddProperty(attributeTag, attribute.Name.ToString(), attribute.Value.Trim());
                    }
                    AddProperty(dynamicObject, xmlParentNode.Name.ToString(), attributeTag);
                }

                if (!string.IsNullOrWhiteSpace(xmlParentNode.InnerText))
                {
                    AddProperty(dynamicObject, xmlParentNode.ParentNode.Name.ToString(), xmlParentNode.InnerText);
                }
            }
        }

        private static void AddProperty(dynamic parent, string name, object value)
        {
            if (parent is List<dynamic>)
            {
                (parent as List<dynamic>).Add(value);
            }
            else if (parent is IDictionary<string, object>)
            {
                (parent as IDictionary<string, object>).Add(name, value);
            }
            else
            {
                (parent as IDictionary<string, object>)[name] = value;
            }
        }
    }
}

