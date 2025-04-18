using Shouldly;
using FluentDataBuilder.NewtonsoftJson;
using FluentDataBuilder.TestsShared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentDataBuilder.ExtensionTests.NewtonsoftJson;

[TestClass]
public class BuildTests
{
    [TestMethod]
    public void Build_WithChildObject_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("id", "7c27a562-d405-4b22-9ade-37503bed6014");
        builder.Add("name", "John Doe");
        builder.Add("editor", new DataBuilder()
            .Add("typevalue", "a object")
            .Add("numbervalue", 55865)
            .Add("booleanvalue", true));

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("{\"id\":\"7c27a562-d405-4b22-9ade-37503bed6014\",\"name\":\"John Doe\",\"editor\":{\"typevalue\":\"a object\",\"numbervalue\":55865,\"booleanvalue\":true}}");
    }

    [TestMethod]
    public void Build_WithSimpleObject_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("number", 123);

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("{\"name\":\"this is a test\",\"number\":123}");
    }

    [TestMethod]
    public void Build_WithStringList_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" });

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("{\"name\":\"this is a test\",\"array\":[\"this\",\"is\",\"a\",\"test\"]}");
    }

    [TestMethod]
    public void Build_WithStringArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" }.ToArray());

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("{\"name\":\"this is a test\",\"array\":[\"this\",\"is\",\"a\",\"test\"]}");
    }

    [TestMethod]
    public void Build_WithMixedArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<object> { "this", 123, true, 456.78 }.ToArray());

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("{\"name\":\"this is a test\",\"array\":[\"this\",123,true,456.78]}");
    }

    [TestMethod]
    public void Build_WithNumericIntArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<int> { 12, 34, 56, 78 }.ToArray());

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("{\"name\":\"this is a test\",\"array\":[12,34,56,78]}");
    }

    [TestMethod]
    public void Build_WithNumericDoubleArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<double> { 12.34, 34, 56.78, 78.901 }.ToArray());

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("{\"name\":\"this is a test\",\"array\":[12.34,34.0,56.78,78.901]}");
    }

    [TestMethod]
    public void Build_WithArrayWithAllDataTypes_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array",
            new List<object>
            {
                (int)15.123412341234,
                (double)15.123412341234,
                (float)16.123412341234,
                false,
                (decimal)9.123412341234,
                "test",
                'x',
                (long)9876.123412341234,
                (ulong)9876.123412341234
            }.ToArray());

        JObject jsonResult = builder.Build();

        string result = jsonResult.ToString(Formatting.None);

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("{\"name\":\"this is a test\",\"array\":[15,15.123412341234,16.123413,false,9.123412341234,\"test\",\"x\",9876,9876]}");
    }

    [TestMethod]
    public void LoadFrom_WithJsonDocumentNull_Returns()
    {
        JObject? json = null;
        IDataBuilder builder = new DataBuilder().LoadFrom(json!);

        builder.ShouldNotBeNull();
        builder.GetProperties().Count.ShouldBe(0);
    }

    [TestMethod]
    public void LoadFrom_WithSimpleJson_Returns()
    {
        string jsonValue = """
                           {
                             "name": "this is a test",
                             "number": 123,
                             "decimal": 123.45,
                             "boolean": true,
                             "null": null,
                             "array": [
                               "this",
                               "is",
                               "a",
                               "test"
                             ],
                             "object": {
                               "name": "this is a test",
                               "number": 123,
                               "decimal": 123.45,
                               "boolean": true,
                               "null": null,
                               "array": [
                                 "this",
                                 "is",
                                 "a",
                                 "test"
                               ]
                             }
                           }
                           """;
        JObject json = JObject.Parse(jsonValue);

        IDataBuilder builder = new DataBuilder().LoadFrom(json);

        builder.ShouldNotBeNull();
        var properties = builder.GetProperties();
        properties.Count.ShouldBe(7);
        properties["name"].ShouldBe("this is a test");
        properties["number"].ShouldBe(123);
        properties["decimal"].ShouldBe((float)123.45);
        properties["boolean"].ShouldBe(true);
        properties["null"].ShouldBeNull();
        properties["array"].ShouldBeOfType<object[]>();
        properties["array"].ShouldBeEquivalentTo(new object[] { "this", "is", "a", "test" });
        properties["object"].ShouldBeEqualTo<Dictionary<string, object>>(dict =>
        {
            dict.Count.ShouldBe(6);
            dict.ShouldContain("name", "this is a test");
            dict.ShouldContain("number", 123);
            dict.ShouldContain("decimal", 123.45f);
            dict.ShouldContain("boolean", true);
            dict.ShouldContain("null", null!);
            dict.ShouldContain("array", new object[] { "this", "is", "a", "test" });
        });
    }

    [TestMethod]
    public void LoadFrom_WithStringNull_Returns()
    {
        string? json = null;
        IDataBuilder builder = new DataBuilder().LoadFrom(json!);

        builder.ShouldNotBeNull();
        builder.GetProperties().Count.ShouldBe(0);
    }

    [TestMethod]
    public void LoadFrom_WithSimpleJsonString_Returns()
    {
        IDataBuilder builder = new DataBuilder()
            .LoadFrom(
                """
                {
                  "name": "this is a test",
                  "number": 123,
                  "decimal": 123.45,
                  "boolean": true,
                  "null": null,
                  "array": [
                    "this",
                    "is",
                    "a",
                    "test"
                  ],
                  "object": {
                    "name": "this is a test",
                    "number": 123,
                    "decimal": 123.45,
                    "boolean": true,
                    "null": null,
                    "array": [
                      "this",
                      "is",
                      "a",
                      "test"
                    ]
                  }
                }
                """);

        builder.ShouldNotBeNull();
        var properties = builder.GetProperties();
        properties.Count.ShouldBe(7);
        properties["name"].ShouldBe("this is a test");
        properties["number"].ShouldBe(123);
        properties["decimal"].ShouldBe(123.45F);
        properties["boolean"].ShouldBe(true);
        properties["null"].ShouldBeNull();
        properties["array"].ShouldBeOfType<object[]>();
        properties["array"].ShouldBeEquivalentTo(new object[] { "this", "is", "a", "test" });
        properties["object"].ShouldBeEqualTo<Dictionary<string, object>>(dict =>
        {
            dict.Count.ShouldBe(6);
            dict.ShouldContain("name", "this is a test");
            dict.ShouldContain("number", 123);
            dict.ShouldContain("decimal", 123.45f);
            dict.ShouldContain("boolean", true);
            dict.ShouldContain("null", null!);
            dict.ShouldContain("array", new object[] { "this", "is", "a", "test" });
        });
    }

    [TestMethod]
    public void LoadFrom_WithSpecificDataObjectFromString_Returns()
    {
        IDataBuilder builder = new DataBuilder()
            .LoadFrom("""
                      {
                        "Title": "Section Page",
                        "Website": {
                          "Author": "htmlc"
                        },
                        "Names": [
                          {
                            "Name": "Max"
                          },
                          {
                            "Name": "Lisa"
                          },
                          {
                            "Name": "Fred"
                          }
                        ]
                      }
                      """);

        builder.ShouldNotBeNull();
        var properties = builder.GetProperties();
        properties.Count.ShouldBe(3);
        properties["Title"].ShouldBe("Section Page");
        properties["Website"].ShouldBeOfType<Dictionary<string, object>>();
        properties["Website"].ShouldBeEquivalentTo(new Dictionary<string, object>
        {
            { "Author", "htmlc" }
        });
        properties["Names"].ShouldBeOfType<object[]>();
        object[] objArray = (properties["Names"] as object[])!;
        objArray[0].ShouldBeOfType<Dictionary<string, object>>();
        objArray[0].ShouldBeEquivalentTo(new Dictionary<string, object>
        {
            { "Name", "Max" }
        });
        objArray[1].ShouldBeOfType<Dictionary<string, object>>();
        objArray[1].ShouldBeEquivalentTo(new Dictionary<string, object>
        {
            { "Name", "Lisa" }
        });
        objArray[2].ShouldBeOfType<Dictionary<string, object>>();
        objArray[2].ShouldBeEquivalentTo(new Dictionary<string, object>
        {
            { "Name", "Fred" }
        });
    }
}