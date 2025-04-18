using Shouldly;

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

        properties.ShouldNotBeNull();
    }

    [TestMethod]
    public void Build_WithStringArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" }.ToArray());
        builder.Add("list", new List<string> { "this", "is", "a", "test" });

        var properties = builder.GetProperties();

        properties.ShouldNotBeNull();
    }

    [TestMethod]
    public void IndexerGet_WithSimpleData_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" }.ToArray());
        builder.Add("list", new List<string> { "this", "is", "a", "test" });

        var valObject = builder["name"];

        valObject.ShouldNotBeNull();
        valObject.ShouldBeOfType<string>();
        valObject.ShouldBe("this is a test");
    }

    [TestMethod]
    public void IndexerGet_WithMultiLevelData_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("object", new DataBuilder()
            .Add("value", "im a test")
            .Add("numeric", 125.77));
        builder.Add("array", new List<string> { "this", "is", "a", "test" }.ToArray());
        builder.Add("list", new List<string> { "this", "is", "a", "test" });

        var valObject = builder["object:numeric"];

        valObject.ShouldNotBeNull();
        valObject.ShouldBeOfType<double>();
        valObject.ShouldBe(125.77);
    }

    [TestMethod]
    public void IndexerGet_WithMultiLevelDataAndNotExistingEntry_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("object", new DataBuilder()
            .Add("value", "im a test")
            .Add("numeric", 125.77));
        builder.Add("array", new List<string> { "this", "is", "a", "test" }.ToArray());
        builder.Add("list", new List<string> { "this", "is", "a", "test" });

        var action = () => builder["object:notexisting"];

        // Assert
        action.ShouldThrow<KeyNotFoundException>();
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

        builder["String"].ShouldNotBeNull();
        builder["String"].ShouldBe("this is a string");

        builder["Number"].ShouldNotBeNull();
        builder["Number"].ShouldBe(125.86);

        builder["Dynamic"].ShouldNotBeNull();
        builder["Dynamic"].ShouldBe("new value");
    }

    [TestMethod]
    public void Merge_WithPartialOverlappingArrayDataBuilder_Returns()
    {
        IDataBuilder leftBuilder = new DataBuilder()
            .Add("first-value", "this is a string")
            .Add("list-values", new List<string> { "value1", "value2", "value3" }.ToArray());
        IDataBuilder rightBuilder = new DataBuilder()
            .Add("other-value", 125.86)
            .Add("list-values", new List<string> { "value2", "value3", "value4" }.ToArray());

        IDataBuilder builder = DataBuilder.Merge(leftBuilder, rightBuilder);

        builder["first-value"].ShouldNotBeNull();
        builder["first-value"].ShouldBe("this is a string");

        builder["other-value"].ShouldNotBeNull();
        builder["other-value"].ShouldBe(125.86);

        builder["list-values"].ShouldNotBeNull();
        builder["list-values"].ShouldBeOfType<object[]>();
        (builder["list-values"] as object[]).ShouldNotBeNull();
        (builder["list-values"] as object[]).Length.ShouldBe(4);
        (builder["list-values"] as object[])[0].ShouldBe("value1");
        (builder["list-values"] as object[])[1].ShouldBe("value2");
        (builder["list-values"] as object[])[2].ShouldBe("value3");
        (builder["list-values"] as object[])[3].ShouldBe("value4");
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

        builder["first-value"].ShouldNotBeNull();
        builder["first-value"].ShouldBe("this is a string");

        builder["other-value"].ShouldNotBeNull();
        builder["other-value"].ShouldBe(125.86);

        builder["object-values"].ShouldNotBeNull();
        builder["object-values"].ShouldBeOfType<Dictionary<string, object>>();
        var objectValues = builder["object-values"] as Dictionary<string, object>;
        objectValues.ShouldNotBeNull();
        objectValues["number"].ShouldBe(133.22);
        objectValues["bool"].ShouldBe(true);
        objectValues["string"].ShouldBe("a description");
    }
}