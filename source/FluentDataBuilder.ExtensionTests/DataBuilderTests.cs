using FluentAssertions;

namespace FluentDataBuilder.ExtensionTests;

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
}