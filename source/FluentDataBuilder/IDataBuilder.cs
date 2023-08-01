namespace FluentDataBuilder;

public interface IDataBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    IDataBuilder Add(string name, object value);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Dictionary<string, object> GetProperties();
}