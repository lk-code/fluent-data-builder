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

        builder = json.Properties().Aggregate(builder, ConvertToIDataBuilder);

        return builder;
    }

    private static IDataBuilder ConvertToIDataBuilder(IDataBuilder builder, JProperty jsonProperty)
    {
        switch (jsonProperty.Value.Type)
        {
            case JTokenType.Object:
                builder.Add(jsonProperty.Name, ConvertJObjectToDataBuilder(jsonProperty.Value));
                break;
            case JTokenType.Array:
                builder.Add(jsonProperty.Name, jsonProperty.Value.ToObject<List<string>>());
                break;
            case JTokenType.String:
                builder.Add(jsonProperty.Name, jsonProperty.Value.ToString());
                break;
            case JTokenType.Integer:
                builder.Add(jsonProperty.Name, jsonProperty.Value.ToObject<int>());
                break;
            case JTokenType.Boolean:
                builder.Add(jsonProperty.Name, jsonProperty.Value.ToObject<bool>());
                break;
            case JTokenType.Null:
            {
                object? value = null;
                builder.Add(jsonProperty.Name, value);
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return builder;
    }

    private static IDataBuilder ConvertJObjectToDataBuilder(JToken jToken)
    {
        var dataBuilder = new DataBuilder();
        foreach (var property in jToken.Children<JProperty>())
        {
            ConvertToIDataBuilder(dataBuilder, property);
        }

        return dataBuilder;
    }
}