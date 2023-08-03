using FluentAssertions;
using FluentDataBuilder.NewtonsoftJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentDataBuilder.ExtensionTests.NewtonsoftJson;

[TestClass]
public class BuildTests
{
    // [TestMethod]
    // public void NewtonsoftJsonMethod1()
    // {
    //     DataBuilder builder = new DataBuilder();
    //     
    //     builder.Add("id", Guid.NewGuid());
    //     var jsonProperty = builder.Add("name", "John Doe");
    //     
    //     var jsonArray = builder.Add("articles")
    //         .AsArray()
    //         .Add("string")
    //         .Add(123);
    //     jsonArray.Add(true);
    //
    //     var jsonObject = builder.Add("editor", new DataBuilder()
    //         .Add("typevalue", "a object")
    //         .Add("numbervalue", 55865)
    //         .Add("booleanvalue", true));
    //
    //     JObject jsonResult = builder.Build();
    // }

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

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"id\":\"7c27a562-d405-4b22-9ade-37503bed6014\",\"name\":\"John Doe\",\"editor\":{\"typevalue\":\"a object\",\"numbervalue\":55865,\"booleanvalue\":true}}");
    }
    
    [TestMethod]
    public void Build_WithSimpleObject_Returns()
    {
        IDataBuilder builder = new DataBuilder();
        
        builder.Add("name", "this is a test");
        builder.Add("number", 123);

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);
        
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"name\":\"this is a test\",\"number\":123}");
    }
}