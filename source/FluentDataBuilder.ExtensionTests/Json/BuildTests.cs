using System.Text.Json;
using FluentAssertions;
using FluentDataBuilder.Json;

namespace FluentDataBuilder.ExtensionTests.Json;

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

    [TestMethod]
    public void Build_WithArrayWithAllDataTypes_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<object> { (int)15.123412341234, (double)15.123412341234, (float)16.123412341234, false, (decimal)9.123412341234, "test", 'x', (long)9876.123412341234, (ulong)9876.123412341234 }.ToArray());

        JsonDocument jsonResult = builder.Build();

        string result = jsonResult.RootElement.GetRawText();

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("{\"name\":\"this is a test\",\"array\":[15,15.123412341234,16.123413,false,9.123412341234,\"test\",\"x\",9876,9876]}");
    }

    [TestMethod]
    public void LoadFrom_WithJsonDocumentNull_Returns()
    {
        JsonDocument? json = null;
        IDataBuilder builder = new DataBuilder().LoadFrom(json);
        
        builder.Should().NotBeNull();
        builder.GetProperties().Count.Should().Be(0);
    }

    [TestMethod]
    public void LoadFrom_WithSimpleJson_Returns()
    {
        JsonDocument json = JsonDocument.Parse("{\"name\":\"this is a test\",\"number\":123,\"boolean\":true,\"null\":null,\"array\":[\"this\",\"is\",\"a\",\"test\"],\"object\":{\"name\":\"this is a test\",\"number\":123,\"boolean\":true,\"null\":null,\"array\":[\"this\",\"is\",\"a\",\"test\"]}}");
        
        IDataBuilder builder = new DataBuilder().LoadFrom(json);
        
        builder.Should().NotBeNull();
        var properties = builder.GetProperties();
        properties.Count.Should().Be(6);
        properties["name"].Should().Be("this is a test");
        properties["number"].Should().Be(123);
        properties["boolean"].Should().Be(true);
        properties["null"].Should().BeNull();
        properties["array"].Should().BeOfType<object[]>();
        properties["array"].Should().BeEquivalentTo(new List<string> { "this", "is", "a", "test" });
        properties["object"].Should().BeOfType<Dictionary<string, object>>();
        properties["object"].Should().BeEquivalentTo(new Dictionary<string, object>
        {
            { "name", "this is a test" },
            { "number", 123 },
            { "boolean", true },
            { "null", null },
            { "array", new List<string> { "this", "is", "a", "test" } }
        });
    }

    [TestMethod]
    public void LoadFrom_WithStringNull_Returns()
    {
        string? json = null;
        IDataBuilder builder = new DataBuilder().LoadFrom(json);
        
        builder.Should().NotBeNull();
        builder.GetProperties().Count.Should().Be(0);
    }

    [TestMethod]
    public void LoadFrom_WithSimpleJsonString_Returns()
    {        
        IDataBuilder builder = new DataBuilder()
            .LoadFrom("{\"name\":\"this is a test\",\"number\":123,\"boolean\":true,\"null\":null,\"array\":[\"this\",\"is\",\"a\",\"test\"],\"object\":{\"name\":\"this is a test\",\"number\":123,\"boolean\":true,\"null\":null,\"array\":[\"this\",\"is\",\"a\",\"test\"]}}");
        
        builder.Should().NotBeNull();
        var properties = builder.GetProperties();
        properties.Count.Should().Be(6);
        properties["name"].Should().Be("this is a test");
        properties["number"].Should().Be(123);
        properties["boolean"].Should().Be(true);
        properties["null"].Should().BeNull();
        properties["array"].Should().BeOfType<object[]>();
        properties["array"].Should().BeEquivalentTo(new List<string> { "this", "is", "a", "test" });
        properties["object"].Should().BeOfType<Dictionary<string, object>>();
        properties["object"].Should().BeEquivalentTo(new Dictionary<string, object>
        {
            { "name", "this is a test" },
            { "number", 123 },
            { "boolean", true },
            { "null", null },
            { "array", new List<string> { "this", "is", "a", "test" } }
        });
    }
}