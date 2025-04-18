using Shouldly;

namespace FluentDataBuilder.TestsShared.Extensions;

public static class DictionaryShouldyExtensions
{
    public static void ShouldContain<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? value, string? customMessage = null)
    {
        dictionary.ShouldContainKey(key, customMessage);
        if (dictionary.TryGetValue(key, out TValue? dictionaryValue))
        {
            dictionary[key].ShouldBe(value, customMessage);
        }
    }

    public static void ShouldBeEqualTo<T>(this object? obj, Action<T> action)
    {
        obj.ShouldBeOfType<T>();
        
        T castedObj = (T)obj!;
        
        action(castedObj);
    }
}