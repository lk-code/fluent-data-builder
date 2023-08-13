using System.Collections;
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

            if (entry.Value is string stringValue)
            {
                element.InnerText = stringValue;
            }
            else if (entry.Value is int intValue)
            {
                element.InnerText = intValue.ToString();
            }
            else if (entry.Value is double doubleValue)
            {
                element.InnerText = doubleValue.ToString();
            }
            else if (entry.Value is bool boolValue)
            {
                element.InnerText = boolValue.ToString();
            }
            else if (entry.Value is Dictionary<string, object> nestedData)
            {
                CreateXmlElements(nestedData, element, xmlDocument);
            }
            else if (entry.Value is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                {
                    XmlElement itemElement = xmlDocument.CreateElement("Item");
                    itemElement.InnerText = item.ToString();
                    element.AppendChild(itemElement);
                }
            }

            parentElement.AppendChild(element);
        }
    }
}