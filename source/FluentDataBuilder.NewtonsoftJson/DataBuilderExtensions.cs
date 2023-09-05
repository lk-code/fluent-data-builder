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

}