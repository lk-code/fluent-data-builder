namespace FluentDataBuilder.Helper;

public static class DictionaryExtensions
{
    public static Dictionary<string, object?> MergeDictionaries(this Dictionary<string, object?> left,
        Dictionary<string, object?> right)
    {
        Dictionary<string, object?> mergedDictionary = new(left);

        foreach ((string? key, object? value) in right)
        {
            if (!mergedDictionary.ContainsKey(key))
            {
                mergedDictionary.Add(key, value);
                continue;
            }

            if (value is object[] arrValue)
            {
                if (mergedDictionary[key] is object[] leftArrValue)
                {
                    mergedDictionary[key] = leftArrValue.Concat(arrValue)
                        .Distinct()
                        .ToArray();
                }
                else
                {
                    mergedDictionary[key] = value;
                }
            }
            else if (value is Dictionary<string, object> dictionaryValue)
            {
                if (mergedDictionary[key] is Dictionary<string, object?> leftDictionaryValue)
                {
                    mergedDictionary[key] = leftDictionaryValue.MergeDictionaries(dictionaryValue);
                }
                else
                {
                    mergedDictionary[key] = value;
                }
            }
            else if (value is object)
            {
                mergedDictionary[key] = value;
            }
            else
            {
                mergedDictionary[key] = value;
            }
        }

        return mergedDictionary;
    }
}