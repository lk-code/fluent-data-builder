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
    IDataBuilder Add(string name, object? value);

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
    Dictionary<string, object?> GetProperties();

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the element to get or set.</param>
    /// <returns>The value associated with the specified key.</returns>
    object? this[string key] { get; set; }
}