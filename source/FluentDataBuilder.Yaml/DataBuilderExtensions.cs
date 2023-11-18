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
}