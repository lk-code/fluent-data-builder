using FluentAssertions;
using FluentDataBuilder.Yaml;

namespace FluentDataBuilder.ExtensionTests.Yaml;

[TestClass]
public class BuildTests
{
    [TestMethod]
    public void Build_WithChildObject_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("id", "7c27a562-d405-4b22-9ade-37503bed6014");
        var jsonProperty = builder.Add("name", "John Doe");

        var jsonObject = builder.Add("editor", new DataBuilder()
            .Add("typevalue", "a object")
            .Add("numbervalue", 55865)
            .Add("booleanvalue", true));

        string result = builder.Build();
        string[] lines = result.Split(Environment.NewLine);
        
        result.Should().NotBeNullOrEmpty();
        lines[0].Should().Be("id: 7c27a562-d405-4b22-9ade-37503bed6014");
        lines[1].Should().Be("name: John Doe");
        lines[2].Should().Be("editor:");
        lines[3].Should().Be("  typevalue: a object");
        lines[4].Should().Be("  numbervalue: 55865");
        lines[5].Should().Be("  booleanvalue: true");
    }
}