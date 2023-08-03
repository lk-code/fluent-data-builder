using FluentAssertions;

namespace FluentDataBuilder.Tests;

[TestClass]
public class DataBuilderTests
{
    [TestMethod]
    public void Add_WithDefault_Return()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("id", "7c27a562-d405-4b22-9ade-37503bed6014");
        var jsonProperty = builder.Add("name", "John Doe");

        var jsonObject = builder.Add("editor", new DataBuilder()
            .Add("typevalue", "a object")
            .Add("numbervalue", 55865)
            .Add("booleanvalue", true));

        var properties = builder.GetProperties();

        properties.Should().NotBeNull();
    }

    [TestMethod]
    public void Build_WithStringArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();
        
        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> {"this", "is", "a", "test"}.ToArray());
        builder.Add("list", new List<string> {"this", "is", "a", "test"});

        var properties = builder.GetProperties();

        properties.Should().NotBeNull();
    }
}