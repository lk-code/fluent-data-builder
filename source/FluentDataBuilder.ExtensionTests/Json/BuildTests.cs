using System.Text.Json;
using FluentAssertions;
using FluentDataBuilder.Json;

namespace FluentDataBuilder.ExtensionTests.Json;

[TestClass]
public class BuildTests
{
    // [TestMethod]
    // public void SystemTextJsonMethod1()
    // {
    //     IDataBuilder builder = new DataBuilder();
    //
    //     builder.Add("id", "7c27a562-d405-4b22-9ade-37503bed6014");
    //     var jsonProperty = builder.Add("name", "John Doe");
    //
            // var jsonArray = builder.Add("articles")
            //     .AsArray()
            //     .Add("string")
            //     .Add(123);
            // jsonArray.Add(true);
    //
    //     var jsonObject = builder.Add("editor", new DataBuilder()
    //         .Add("typevalue", "a object")
    //         .Add("numbervalue", 55865)
    //         .Add("booleanvalue", true));
    //
    //     JsonDocument jsonResult = builder.Build();
    //
    //     string result = jsonResult.RootElement.GetRawText();
    //
    //     result.Should().NotBeNullOrEmpty();
    //     result.Should().Be("{\"id\":\"7c27a562-d405-4b22-9ade-37503bed6014\",\"name\":\"John Doe\",\"editor\":{\"typevalue\":\"a object\",\"numbervalue\":55865,\"booleanvalue\":true}}");
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

        JsonDocument jsonResult = builder.Build();

        string result = jsonResult.RootElement.GetRawText();

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"id\":\"7c27a562-d405-4b22-9ade-37503bed6014\",\"name\":\"John Doe\",\"editor\":{\"typevalue\":\"a object\",\"numbervalue\":55865,\"booleanvalue\":true}}");
    }

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

    [TestMethod]
    public void Build_WithStringList_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" });

        JsonDocument jsonResult = builder.Build();

        string result = jsonResult.RootElement.GetRawText();

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"name\":\"this is a test\",\"array\":[\"this\",\"is\",\"a\",\"test\"]}");
    }

    [TestMethod]
    public void Build_WithStringArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" }.ToArray());

        JsonDocument jsonResult = builder.Build();

        string result = jsonResult.RootElement.GetRawText();

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"name\":\"this is a test\",\"array\":[\"this\",\"is\",\"a\",\"test\"]}");
    }

    [TestMethod]
    public void Build_WithMixedArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<object> { "this", 123, true, 456.78 }.ToArray());

        JsonDocument jsonResult = builder.Build();

        string result = jsonResult.RootElement.GetRawText();

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"name\":\"this is a test\",\"array\":[\"this\",123,true,456.78]}");
    }

    [TestMethod]
    public void Build_WithNumericIntArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<int> { 12, 34, 56, 78 }.ToArray());

        JsonDocument jsonResult = builder.Build();

        string result = jsonResult.RootElement.GetRawText();

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"name\":\"this is a test\",\"array\":[12,34,56,78]}");
    }

    [TestMethod]
    public void Build_WithNumericDoubleArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<double> { 12.34, 34, 56.78, 78.901 }.ToArray());

        JsonDocument jsonResult = builder.Build();

        string result = jsonResult.RootElement.GetRawText();

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"name\":\"this is a test\",\"array\":[12.34,34,56.78,78.901]}");
    }
}