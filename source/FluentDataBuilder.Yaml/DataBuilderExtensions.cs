using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FluentDataBuilder.Yaml;


public static class DataBuilderExtensions
{
    /// <summary>
    /// Builds a YAML Serializer from the properties of an IDataBuilder object.
    /// </summary>
    /// <param name="builder">The IDataBuilder object whose properties are used to create the yaml content.</param>
    /// <returns>A yaml string containing the properties of the IDataBuilder object.</returns>
    public static string Build(this IDataBuilder builder)
    {
        ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        string yaml = serializer.Serialize(builder.GetProperties());

        return yaml;
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
    public static IDataBuilder LoadFrom(this IDataBuilder builder,
        string yaml)
    {
        if (string.IsNullOrEmpty(yaml))
        {
            return builder;
        }

        ar deserializer = new DeserializerBuilder().Build();
        var result = deserializer.Deserialize<Dictionary<string, object>>(yamlString);


        return builder;
    }
}