namespace FluentDataBuilder;

public class DataBuilder : IDataBuilder
{
    private readonly Dictionary<string, object> _data = new ();

    public DataBuilder()
    {
        
    }
    
    /// <inheritdoc />
    public IDataBuilder Add(string name, object value)
    {
        this._data.Add(name, value);

        return this;
    }

    /// <inheritdoc />
    public IDataBuilder Add(string name, IDataBuilder value)
    {
        this._data.Add(name, value.GetProperties());

        return this;
    }

    /// <inheritdoc />
    public IDataBuilder Add(string name, IEnumerable<object> values)
    {
        this._data.Add(name, values.ToArray());

        return this;
    }

    /// <inheritdoc />
    public Dictionary<string, object> GetProperties()
    {
        return this._data;
    }
}