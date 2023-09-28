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

    [TestMethod]
    public void IndexerGet_WithSimpleData_Returns()
    {
        IDataBuilder builder = new DataBuilder();
        
        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> {"this", "is", "a", "test"}.ToArray());
        builder.Add("list", new List<string> {"this", "is", "a", "test"});

        var valObject = builder["name"];
        
        valObject.Should().NotBeNull();
        valObject.Should().BeOfType<string>();
        valObject.Should().Be("this is a test");
    }

    [TestMethod]
    public void IndexerGet_WithMultiLevelData_Returns()
    {
        IDataBuilder builder = new DataBuilder();
        
        builder.Add("name", "this is a test");
        builder.Add("object", new DataBuilder()
            .Add("value", "im a test")
            .Add("numeric", 125.77));
        builder.Add("array", new List<string> {"this", "is", "a", "test"}.ToArray());
        builder.Add("list", new List<string> {"this", "is", "a", "test"});

        var valObject = builder["object:numeric"];
        
        valObject.Should().NotBeNull();
        valObject.Should().BeOfType<double>();
        valObject.Should().Be(125.77);
    }

    [TestMethod]
    public void IndexerGet_WithMultiLevelDataAndNotExistingEntry_Returns()
    {
        IDataBuilder builder = new DataBuilder();
        
        builder.Add("name", "this is a test");
        builder.Add("object", new DataBuilder()
            .Add("value", "im a test")
            .Add("numeric", 125.77));
        builder.Add("array", new List<string> {"this", "is", "a", "test"}.ToArray());
        builder.Add("list", new List<string> {"this", "is", "a", "test"});

        var action = () => builder["object:notexisting"];

        // Assert
        action.Should().Throw<KeyNotFoundException>();
    }

    [TestMethod]
    public void Merge_WithSimpleDataBuilder_Returns()
    {
        IDataBuilder leftBuilder = new DataBuilder()
            .Add("String", "this is a string")
            .Add("Dynamic", "value");
        IDataBuilder rightBuilder = new DataBuilder()
            .Add("Number", 125.86)
            .Add("Dynamic", "new value");
        
        IDataBuilder builder = DataBuilder.Merge(leftBuilder, rightBuilder);
        
        builder["String"].Should().NotBeNull();
        builder["String"].Should().Be("this is a string");
        
        builder["Number"].Should().NotBeNull();
        builder["Number"].Should().Be(125.86);
        
        builder["Dynamic"].Should().NotBeNull();
        builder["Dynamic"].Should().Be("new value");
    }

    [TestMethod]
    public void Merge_WithPartialOverlappingArrayDataBuilder_Returns()
    {
        IDataBuilder leftBuilder = new DataBuilder()
            .Add("first-value", "this is a string")
            .Add("list-values", new List<string> {"value1", "value2", "value3"}.ToArray());
        IDataBuilder rightBuilder = new DataBuilder()
            .Add("other-value", 125.86)
            .Add("list-values", new List<string> {"value2", "value3", "value4"}.ToArray());
        
        IDataBuilder builder = DataBuilder.Merge(leftBuilder, rightBuilder);
        
        builder["first-value"].Should().NotBeNull();
        builder["first-value"].Should().Be("this is a string");
        
        builder["other-value"].Should().NotBeNull();
        builder["other-value"].Should().Be(125.86);
        
        builder["list-values"].Should().NotBeNull();
        builder["list-values"].Should().BeOfType<object[]>();
        (builder["list-values"] as object[]).Should().HaveCount(4);
        (builder["list-values"] as object[])![0].Should().Be("value1");
        (builder["list-values"] as object[])![1].Should().Be("value2");
        (builder["list-values"] as object[])![2].Should().Be("value3");
        (builder["list-values"] as object[])![3].Should().Be("value4");
    }

    [TestMethod]
    public void Merge_WithPartialOverlappingObjectsDataBuilder_Returns()
    {
        IDataBuilder leftBuilder = new DataBuilder()
            .Add("first-value", "this is a string")
            .Add("object-values", new DataBuilder()
                .Add("string", "text")
                .Add("number", 133.22));
        IDataBuilder rightBuilder = new DataBuilder()
            .Add("other-value", 125.86)
            .Add("object-values", new DataBuilder()
                .Add("bool", true)
                .Add("string", "a description"));
        
        IDataBuilder builder = DataBuilder.Merge(leftBuilder, rightBuilder);
        
        builder["first-value"].Should().NotBeNull();
        builder["first-value"].Should().Be("this is a string");
        
        builder["other-value"].Should().NotBeNull();
        builder["other-value"].Should().Be(125.86);
        
        builder["object-values"].Should().NotBeNull();
        builder["object-values"].Should().BeOfType<Dictionary<string, object>>();
        (builder["object-values"] as Dictionary<string, object>)!["number"].Should().Be(133.22);
        (builder["object-values"] as Dictionary<string, object>)!["bool"].Should().Be(true);
        (builder["object-values"] as Dictionary<string, object>)!["string"].Should().Be("a description");
    }
}