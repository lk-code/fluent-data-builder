namespace FluentDataBuilder.Helper;

public static class DictionaryExtensions
{
    public static Dictionary<string, object?> MergeDictionaries(this Dictionary<string, object?> left,
        Dictionary<string, object?> right)
    {
        var mergedDictionary = new Dictionary<string, object?>(left);

        foreach (var kvp in right)
        {
            if (mergedDictionary.ContainsKey(kvp.Key))
            {
                mergedDictionary[kvp.Key] = kvp.Value;
            }
            else
            {
                mergedDictionary.Add(kvp.Key, kvp.Value);
            }
        }

        return mergedDictionary;
    }
}