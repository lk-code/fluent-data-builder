using System.Collections;
using System.Globalization;
using System.Xml;

namespace FluentDataBuilder.Xml;

public static class DataBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static XmlDocument Build(this IDataBuilder builder)
    {
        XmlDocument xmlDocument = new XmlDocument();
        XmlElement rootElement = xmlDocument.CreateElement("Root");
        xmlDocument.AppendChild(rootElement);

        CreateXmlElements(builder.GetProperties(), rootElement, xmlDocument);

        return xmlDocument;
    }

    private static void CreateXmlElements(Dictionary<string, object> data, XmlElement parentElement,
        XmlDocument xmlDocument)
    {
        foreach (KeyValuePair<string, object> entry in data)
        {
            XmlElement element = xmlDocument.CreateElement(entry.Key);

            switch (entry.Value)
            {
                case sbyte sbyteValue:
                case byte byteValue:
                case short shortValue:
                case ushort ushortValue:
                case int intValue:
                case uint uintValue:
                case long longValue:
                case ulong ulongValue:
                case float floatValue:
                case double doubleValue:
                case decimal decimalValue:
                case char charValue:
                case bool boolValue:
                case string stringValue:
                {
                    element.InnerText = Convert.ToString(entry.Value, CultureInfo.InvariantCulture);
                }
                    break;
                case Dictionary<string, object> items:
                {
                    CreateXmlElements(items, element, xmlDocument);
                } break;
                case IEnumerable items:
                {
                    foreach (object item in items)
                    {
                        XmlElement itemElement = xmlDocument.CreateElement("Item");
                        itemElement.InnerText =  Convert.ToString(item, CultureInfo.InvariantCulture);
                        element.AppendChild(itemElement);
                    }
                } break;
                default:
                {
                    element.InnerText = Convert.ToString(entry.Value, CultureInfo.InvariantCulture);
                }
                    break;
            }

            parentElement.AppendChild(element);
        }
    }
}