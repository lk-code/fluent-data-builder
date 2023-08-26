using System.Text;
using FluentDataBuilder.Json;
using Microsoft.Extensions.Configuration;

namespace FluentDataBuilder.Microsoft.Extensions.Configuration;

/// <summary>
/// Provides extension methods for converting an IDataBuilder to an IConfiguration.
/// </summary>
public static class DataBuilderExtensions
{
    /// <summary>
    /// Converts the provided IDataBuilder instance to an IConfiguration instance.
    /// </summary>
    /// <param name="dataBuilder">The IDataBuilder instance.</param>
    /// <returns>An IConfiguration instance representing the data in the IDataBuilder.</returns>
    public static IConfiguration ToConfiguration(this IDataBuilder dataBuilder)
    {
        string configJsonString = dataBuilder.Build()
            .RootElement
            .GetRawText();
        using MemoryStream configJsonStream = new MemoryStream(Encoding.UTF8.GetBytes(configJsonString));
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonStream(configJsonStream)
            .Build();
        
        return configuration;
    }
}
