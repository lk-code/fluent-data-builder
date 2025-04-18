using Shouldly;
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

        result.ShouldNotBeNullOrEmpty();
        lines[0].ShouldBe("id: 7c27a562-d405-4b22-9ade-37503bed6014");
        lines[1].ShouldBe("name: John Doe");
        lines[2].ShouldBe("editor:");
        lines[3].ShouldBe("  typevalue: a object");
        lines[4].ShouldBe("  numbervalue: 55865");
        lines[5].ShouldBe("  booleanvalue: true");
    }
}