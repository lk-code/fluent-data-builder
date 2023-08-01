using System.Text.Json;
using FluentAssertions;
using FluentDataBuilder.Json;

namespace FluentDataBuilder.Tests.Json;

[TestClass]
public class BuildTests
{
    // [TestMethod]
    // public void SystemTextJsonMethod1()
    // {
    //     IDataBuilder builder = new DataBuilder();
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
    //     JsonDocument jsonResult = builder.Build();
    //
    //     string result = jsonResult.RootElement.GetRawText();
    // }
    
    [TestMethod]
    public void Build_WithSimpleObject_Returns()
    {
        IDataBuilder builder = new DataBuilder();
        
        builder.Add("name", "this is a test");
        builder.Add("number", 123);

        JsonDocument jsonResult = builder.Build();

        string result = jsonResult.RootElement.GetRawText();
        
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"name\":\"this is a test\",\"number\":123}");
    }
}