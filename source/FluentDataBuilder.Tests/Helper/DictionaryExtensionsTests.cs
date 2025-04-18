using Shouldly;
using FluentDataBuilder.Helper;

namespace FluentDataBuilder.Tests.Helper;

[TestClass]
public class DictionaryExtensionsTests
{
    [TestMethod]
    public void MergeDictionaries_WithDifferent_Returns()
    {
        Dictionary<string, object?> left = new()
        {
            { "id", "7c27a562-d405-4b22-9ade-37503bed6014" }
        };
        Dictionary<string, object?> right = new()
        {
            { "name", "John Doe" }
        };

        var mergedDictionary = left.MergeDictionaries(right);
        mergedDictionary.ShouldNotBeNull();
        mergedDictionary.Count.ShouldBe(2);
        mergedDictionary.ShouldContainKey("id");
        mergedDictionary.ShouldContainKey("name");
        mergedDictionary["id"].ShouldBe("7c27a562-d405-4b22-9ade-37503bed6014");
        mergedDictionary["name"].ShouldBe("John Doe");
    }

    [TestMethod]
    public void MergeDictionaries_WithOverlappingEntries_Returns()
    {
        Dictionary<string, object?> left = new()
        {
            { "id", "7c27a562-d405-4b22-9ade-37503bed6014" },
            { "username", "uname" }
        };
        Dictionary<string, object?> right = new()
        {
            { "name", "John Doe" },
            { "username", "jdoe" }
        };

        var mergedDictionary = left.MergeDictionaries(right);
        mergedDictionary.ShouldNotBeNull();
        mergedDictionary.Count.ShouldBe(3);
        mergedDictionary.ShouldContainKey("id");
        mergedDictionary.ShouldContainKey("name");
        mergedDictionary.ShouldContainKey("username");
        mergedDictionary["id"].ShouldBe("7c27a562-d405-4b22-9ade-37503bed6014");
        mergedDictionary["name"].ShouldBe("John Doe");
        mergedDictionary["username"].ShouldBe("jdoe");
    }
}