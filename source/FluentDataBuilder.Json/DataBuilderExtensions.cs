using System.Text.Json;

namespace FluentDataBuilder.Json;

public static class DataBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
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
}