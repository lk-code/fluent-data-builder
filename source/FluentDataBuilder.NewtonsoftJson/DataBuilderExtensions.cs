using Newtonsoft.Json.Linq;

namespace FluentDataBuilder.NewtonsoftJson;

public static class DataBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static JObject Build(this IDataBuilder builder)
    {
        JObject jObject = JObject.FromObject(builder.GetProperties());

        return jObject;
    }
}