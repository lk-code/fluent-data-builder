using System.Text.Json;

namespace FluentDataBuilder.Json;

public static class DataBuilderExtensions
{
    /// <summary>
    /// Creates a JsonDocument object from the properties of an IDataBuilder object.
    /// </summary>
    /// <param name="builder">The IDataBuilder object whose properties are to be converted into a JsonDocument.</param>
    /// <returns>A JsonDocument object representing the properties of the IDataBuilder object.</returns>
    /// <remarks>
    /// This method serializes the properties of the IDataBuilder object into a JSON string and then converts it into a JsonDocument.
    /// The WriteIndented option is used to return the JSON string without indentations and formatted.
    /// </remarks>
    public static JsonDocument Build(this IDataBuilder builder)
    {
        string jsonString = JsonSerializer.Serialize(builder.GetProperties(), new JsonSerializerOptions
        {
            WriteIndented = false
        });

        JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

        return jsonDocument;
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
    public static IDataBuilder LoadFrom(this IDataBuilder builder, JsonDocument json)
    {
        if (json is null)
        {
            return builder;
        }

        return json.RootElement
            .EnumerateObject()
            .Aggregate(builder,
                (currentBuilder, jsonProperty) =>
                    ConvertToIDataBuilder(currentBuilder, jsonProperty.Name, jsonProperty.Value));
    }

    /// <summary>
    /// Loads data into an IDataBuilder object from a JsonElement.
    /// </summary>
    /// <param name="builder">The IDataBuilder object to load data into.</param>
    /// <param name="json">The JsonElement containing the data to load into the IDataBuilder object.</param>
    /// <returns>The IDataBuilder object with data loaded from the JsonElement.</returns>
    /// <remarks>
    /// This method loads data into the IDataBuilder object from the provided JsonElement.
    /// It iterates over the properties of the JsonElement and converts each property into IDataBuilder properties, updating the builder.
    /// </remarks>
    public static IDataBuilder LoadFrom(this IDataBuilder builder, JsonElement json)
    {
        builder = json
            .EnumerateObject()
            .Aggregate(builder,
                (currentBuilder, jsonProperty) =>
                    ConvertToIDataBuilder(currentBuilder, jsonProperty.Name, jsonProperty.Value));

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

        JsonDocument jsonDocument = JsonDocument.Parse(json);

        builder = jsonDocument.RootElement
            .EnumerateObject()
            .Aggregate(builder,
                (currentBuilder, jsonProperty) =>
                    ConvertToIDataBuilder(currentBuilder, jsonProperty.Name, jsonProperty.Value));

        return builder;
    }

    private static IDataBuilder ConvertToIDataBuilder(IDataBuilder builder, string key, JsonElement jsonElement)
    {
        builder.Add(key, GetJsonNode(jsonElement));

        return builder;
    }

    private static object? GetJsonNode(JsonElement jsonElement)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Object:
                return new DataBuilder().LoadFrom(jsonElement).GetProperties();
            case JsonValueKind.Array:
                return jsonElement.EnumerateArray()
                    .Select(GetJsonNode)
                    .ToArray();
            case JsonValueKind.String:
                return jsonElement.GetString();
            case JsonValueKind.Number:
                return jsonElement.GetInt32();
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
            {
                object? value = null;
                return value;
            }
            default:
                throw new ArgumentOutOfRangeException($"unknown data type: {jsonElement.ValueKind}");
        }
    }
}