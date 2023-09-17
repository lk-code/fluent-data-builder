using System.Collections;
using System.Globalization;
using System.Xml;

namespace FluentDataBuilder.Xml;

public static class DataBuilderExtensions
{
    /// <summary>
    /// Builds an XmlDocument from the properties of an IDataBuilder object.
    /// </summary>
    /// <param name="builder">The IDataBuilder object whose properties are used to create the XmlDocument.</param>
    /// <returns>An XmlDocument containing the properties of the IDataBuilder object.</returns>
    /// <remarks>
    /// This method creates an XmlDocument and appends a root element named "Root" to it.
    /// It then populates the XmlDocument with XML elements based on the properties of the IDataBuilder object
    /// using the CreateXmlElements method.
    /// </remarks>
    public static XmlDocument Build(this IDataBuilder builder)
    {
        XmlDocument xmlDocument = new XmlDocument();
        XmlElement rootElement = xmlDocument.CreateElement("Root");
        xmlDocument.AppendChild(rootElement);

        CreateXmlElements(builder.GetProperties()!, rootElement, xmlDocument);

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
                    element.InnerText = Convert.ToString(entry.Value, CultureInfo.InvariantCulture)!;
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
                        itemElement.InnerText =  Convert.ToString(item, CultureInfo.InvariantCulture)!;
                        element.AppendChild(itemElement);
                    }
                } break;
                default:
                {
                    element.InnerText = Convert.ToString(entry.Value, CultureInfo.InvariantCulture)!;
                }
                    break;
            }

            parentElement.AppendChild(element);
        }
    }
}