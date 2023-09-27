using FluentAssertions;
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
            {"id", "7c27a562-d405-4b22-9ade-37503bed6014"}
        };
        Dictionary<string, object?> right = new()
        {
            {"name", "John Doe"}
        };
        
        var mergedDictionary = left.MergeDictionaries(right);
        mergedDictionary.Should().NotBeNull();
        mergedDictionary.Should().HaveCount(2);
        mergedDictionary.Should().ContainKey("id");
        mergedDictionary.Should().ContainKey("name");
        mergedDictionary["id"].Should().Be("7c27a562-d405-4b22-9ade-37503bed6014");
        mergedDictionary["name"].Should().Be("John Doe");
    }
    
    [TestMethod]
    public void MergeDictionaries_WithOverlappingEntries_Returns()
    {
        Dictionary<string, object?> left = new()
        {
            {"id", "7c27a562-d405-4b22-9ade-37503bed6014"},
            {"username", "uname"}
        };
        Dictionary<string, object?> right = new()
        {
            {"name", "John Doe"},
            {"username", "jdoe"}
        };
        
        var mergedDictionary = left.MergeDictionaries(right);
        mergedDictionary.Should().NotBeNull();
        mergedDictionary.Should().HaveCount(3);
        mergedDictionary.Should().ContainKey("id");
        mergedDictionary.Should().ContainKey("name");
        mergedDictionary.Should().ContainKey("username");
        mergedDictionary["id"].Should().Be("7c27a562-d405-4b22-9ade-37503bed6014");
        mergedDictionary["name"].Should().Be("John Doe");
        mergedDictionary["username"].Should().Be("jdoe");
    }
}