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
            WriteIndented = false // Option, um den JSON-String formatiert und leserlich zu machen
        });

        // Den JSON-String in ein JsonDocument umwandeln
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
        
        builder = json.RootElement.EnumerateObject().Aggregate(builder, (currentBuilder, jsonProperty) => ConvertToIDataBuilder(currentBuilder, jsonProperty));
        
        return builder;
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
        builder = json.EnumerateObject().Aggregate(builder, (currentBuilder, jsonProperty) => ConvertToIDataBuilder(currentBuilder, jsonProperty));

        return builder;
    }

    private static IDataBuilder ConvertToIDataBuilder(IDataBuilder builder, JsonProperty jsonProperty)
    {
        switch (jsonProperty.Value.ValueKind)
        {
            case JsonValueKind.Object:
                builder.Add(jsonProperty.Name, new DataBuilder().LoadFrom(jsonProperty.Value));
                break;
            case JsonValueKind.Array:
                builder.Add(jsonProperty.Name, jsonProperty.Value.EnumerateArray().Select(x => x.ToString()).ToList());
                break;
            case JsonValueKind.String:
                builder.Add(jsonProperty.Name, jsonProperty.Value.GetString());
                break;
            case JsonValueKind.Number:
                builder.Add(jsonProperty.Name, jsonProperty.Value.GetInt32());
                break;
            case JsonValueKind.True:
                builder.Add(jsonProperty.Name, true);
                break;
            case JsonValueKind.False:
                builder.Add(jsonProperty.Name, false);
                break;
            case JsonValueKind.Null:
                object? value = null;
                builder.Add(jsonProperty.Name, value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return builder;
    }
}