namespace FluentDataBuilder;

/// <summary>
/// The DataBuilder interface.
/// </summary>
public interface IDataBuilder
{
    /// <summary>
    /// Adds a single value.
    /// </summary>
    /// <param name="name">The property name for the entry.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>The IDataBuilder object.</returns>
    IDataBuilder Add(string name, object value);
    
    /// <summary>
    /// Adds a complex value.
    /// </summary>
    /// <param name="name">The property name for the entry.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>The IDataBuilder object.</returns>
    IDataBuilder Add(string name, IDataBuilder value);
    
    /// <summary>
    /// Adds a list of values.
    /// </summary>
    /// <param name="name">The property name for the entry.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>The IDataBuilder object.</returns>
    IDataBuilder Add(string name, IEnumerable<object> values);
    
    /// <summary>
    /// Retrieves all added properties as a dictionary.
    /// </summary>
    /// <returns>A dictionary containing the properties.</returns>
    Dictionary<string, object> GetProperties();
}
