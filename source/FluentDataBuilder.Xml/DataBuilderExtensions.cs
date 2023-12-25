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
        XmlDocument xmlDocument = new();
        XmlElement rootElement = xmlDocument.CreateElement("Root");
        xmlDocument.AppendChild(rootElement);

        CreateXmlElements(builder.GetProperties()!, rootElement, xmlDocument);

        return xmlDocument;
    }

    private static void CreateXmlElements(Dictionary<string, object> data, 
        XmlElement parentElement,
        XmlDocument xmlDocument)
    {
        foreach (KeyValuePair<string, object> entry in data)
        {
            XmlElement element = xmlDocument.CreateElement(entry.Key);

            switch (entry.Value)
            {
                case sbyte:
                case byte:
                case short:
                case ushort:
                case int:
                case uint:
                case long:
                case ulong:
                case float:
                case double:
                case decimal:
                case char:
                case bool:
                case string:
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

    /// <summary>
    /// Loads data into an IDataBuilder object from a JsonDocument.
    /// </summary>
    /// <param name="builder">The IDataBuilder object to load data into.</param>
    /// <param name="json">The JsonDocument containing the data to load into the IDataBuilder object.</param>
    /// <returns>The IDataBuilder object with data loaded from the JsonDocument.</returns>
    /// <remarks>
    /// This method loads data into the IDataBuilder object from the provided JsonDocument.
    /// If the JsonDocument is null, the method returns the original builder unchanged.
    /// The method uses an aggregation to iterate over the properties of the JsonDocument's root element and
    /// convert each property into IDataBuilder properties, updating the builder.
    /// </remarks>
    public static IDataBuilder LoadFrom(this IDataBuilder builder, XmlDocument xmlDocument)
    {
        if (xmlDocument is null)
        {
            return builder;
        }
        
        foreach (XmlNode childNode in xmlDocument.ChildNodes)
        {
            childNode.ChildNodes
                .OfType<XmlElement>()
                .Aggregate(builder, (currentBuilder, xmlElement) => ConvertToIDataBuilder(currentBuilder, xmlElement.Name, xmlElement));

        }

        return builder;
    }

    private static IDataBuilder ConvertToIDataBuilder(IDataBuilder builder,
        string key,
        XmlElement xmlElement)
    {
        builder.Add(key, GetXmlNode(xmlElement));
        
        return builder;
    }

    private static object? GetXmlNode(XmlElement xmlElement)
    {
        switch (xmlElement.NodeType)
        {
            case XmlNodeType.Text:
                return xmlElement.InnerText;
            case XmlNodeType.Element:
                return xmlElement.InnerText;
            case XmlNodeType.None:
                return null;
            // case XmlNodeType.Element:
            // case XmlNodeType.Document:
            //     return new DataBuilder()
            //         .LoadFrom(xmlElement)
            //         .GetProperties();
            default:
                throw new ArgumentOutOfRangeException($"unknown data type: {xmlElement.NodeType}");
        }
    }
}