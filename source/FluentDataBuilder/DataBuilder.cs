namespace FluentDataBuilder;

public class DataBuilder : IDataBuilder
{
    private readonly Dictionary<string, object> _data = new();

    public DataBuilder()
    {
    }

    /// <inheritdoc />
    public IDataBuilder Add(string name, object value)
    {
        SetNestedValue(_data, name, value);

        return this;
    }

    /// <inheritdoc />
    public IDataBuilder Add(string name, IDataBuilder value)
    {
        SetNestedValue(_data, name, value.GetProperties());

        return this;
    }

    /// <inheritdoc />
    public IDataBuilder Add(string name, IEnumerable<object> values)
    {
        SetNestedValue(_data, name, values.ToArray());

        return this;
    }

    /// <inheritdoc />
    public Dictionary<string, object> GetProperties()
    {
        return _data;
    }

    /// <inheritdoc />
    public object this[string key]
    {
        get { return GetNestedValue(_data, key); }
        set { SetNestedValue(_data, key, value); }
    }

    private object GetNestedValue(Dictionary<string, object> data, string key)
    {
        var keys = key.Split(':');
        Dictionary<string, object> currentData = data;

        for (int i = 0; i < keys.Length; i++)
        {
            string subKey = keys[i];

            if (!currentData.ContainsKey(subKey))
            {
                throw new KeyNotFoundException($"Key '{subKey}' not found in the hierarchical dictionary.");
            }

            if (i == keys.Length - 1)
            {
                // Reached the final subkey, return its value
                return currentData[subKey];
            }
            else
            {
                // Move deeper into the hierarchy
                currentData = currentData[subKey] as Dictionary<string, object>;
            }
        }

        // This should not happen
        throw new InvalidOperationException("Unexpected error while accessing the hierarchical dictionary.");
    }

    private void SetNestedValue(Dictionary<string, object> data, string key, object value)
    {
        var keys = key.Split(':');
        Dictionary<string, object> currentData = data;

        for (int i = 0; i < keys.Length; i++)
        {
            string subKey = keys[i];

            if (i == keys.Length - 1)
            {
                // Reached the final subkey, set its value
                currentData[subKey] = value;
            }
            else
            {
                // Create a new dictionary if subkey doesn't exist
                if (!currentData.ContainsKey(subKey))
                {
                    currentData[subKey] = new Dictionary<string, object>();
                }

                // Move deeper into the hierarchy
                currentData = currentData[subKey] as Dictionary<string, object>;
            }
        }
    }
}