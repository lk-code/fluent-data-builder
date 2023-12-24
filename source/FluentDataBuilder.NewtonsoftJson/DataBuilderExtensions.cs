using Newtonsoft.Json.Linq;

namespace FluentDataBuilder.NewtonsoftJson;

public static class DataBuilderExtensions
{
    /// <summary>
    /// Builds a JObject from the properties of an IDataBuilder object.
    /// </summary>
    /// <param name="builder">The IDataBuilder object whose properties are used to create the JObject.</param>
    /// <returns>A JObject containing the properties of the IDataBuilder object.</returns>
    /// <remarks>
    /// This method creates a JObject by serializing the properties of the IDataBuilder object using JObject.FromObject.
    /// </remarks>
    public static JObject Build(this IDataBuilder builder)
    {
        // Serialize the properties of the IDataBuilder object into a JObject
        JObject jObject = JObject.FromObject(builder.GetProperties());

        return jObject;
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
    public static IDataBuilder LoadFrom(this IDataBuilder builder, JObject json)
    {
        if (json is null)
        {
            return builder;
        }

        builder = json
            .Properties()
            .Aggregate(builder, (currentBuilder, jsonProperty) => ConvertToIDataBuilder(currentBuilder, jsonProperty.Name, jsonProperty.Value));

        return builder;
    }

    /// <summary>
    /// Loads data into an IDataBuilder object from a JSON string.
    /// </summary>
    /// <param name="builder">The IDataBuilder object to load data into.</param>
    /// <param name="json">A JSON string containing the data to load into the IDataBuilder object.</param>
    /// <returns>The IDataBuilder object with data loaded from the JSON string.</returns>
    /// <remarks>
    /// This method parses the input JSON string into a JsonDocument and then iterates over the properties of the JsonDocument's root element.
    /// For each property, it converts the property into IDataBuilder properties and updates the builder accordingly.
    /// </remarks>
    public static IDataBuilder LoadFrom(this IDataBuilder builder, string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return builder;
        }

        builder = LoadFrom(builder, JObject.Parse(json));
        
        return builder;
    }

    private static IDataBuilder ConvertToIDataBuilder(IDataBuilder builder, string key, JToken json)
    {
        builder.Add(key, GetJsonNode(json));

        return builder;
    }

    private static object? GetJsonNode(JToken jtoken)
    {
        switch (jtoken.Type)
        {
            case JTokenType.Null:
                return null;
            case JTokenType.Object:
                return new DataBuilder()
                    .LoadFrom((JObject)jtoken)
                    .GetProperties();
            case JTokenType.Array:
                return jtoken.Children()
                    .Select(GetJsonNode)
                    .ToArray();
            case JTokenType.Integer:
                return jtoken.ToObject<int>();
            case JTokenType.Float:
                return jtoken.ToObject<float>();
            case JTokenType.Boolean:
                return jtoken.ToObject<bool>();
            case JTokenType.Guid:
                return jtoken.ToObject<Guid>();
            case JTokenType.Undefined:
            case JTokenType.Date:
            case JTokenType.Raw:
            case JTokenType.String:
            case JTokenType.Uri:
                return jtoken.ToString();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}