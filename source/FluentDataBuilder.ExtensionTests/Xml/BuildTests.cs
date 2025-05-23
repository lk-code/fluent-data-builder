using System.Xml;
using Shouldly;
using FluentDataBuilder.Xml;

namespace FluentDataBuilder.ExtensionTests.Xml;

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

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("<Root><id>7c27a562-d405-4b22-9ade-37503bed6014</id><name>John Doe</name><editor><typevalue>a object</typevalue><numbervalue>55865</numbervalue><booleanvalue>True</booleanvalue></editor></Root>");
    }

    [TestMethod]
    public void Build_WithSimpleObject_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("number", 123);

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("<Root><name>this is a test</name><number>123</number></Root>");
    }

    [TestMethod]
    public void Build_WithStringList_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" });

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("<Root><name>this is a test</name><array><Item>this</Item><Item>is</Item><Item>a</Item><Item>test</Item></array></Root>");
    }

    [TestMethod]
    public void Build_WithStringArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" }.ToArray());

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("<Root><name>this is a test</name><array><Item>this</Item><Item>is</Item><Item>a</Item><Item>test</Item></array></Root>");
    }

    [TestMethod]
    public void Build_WithMixedArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<object> { "this", 123, true, 456.78 }.ToArray());

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("<Root><name>this is a test</name><array><Item>this</Item><Item>123</Item><Item>True</Item><Item>456.78</Item></array></Root>");
    }

    [TestMethod]
    public void Build_WithNumericIntArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<int> { 12, 34, 56, 78 }.ToArray());

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("<Root><name>this is a test</name><array><Item>12</Item><Item>34</Item><Item>56</Item><Item>78</Item></array></Root>");
    }

    [TestMethod]
    public void Build_WithNumericDoubleArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<double> { 12.34, 34, 56.78, 78.901 }.ToArray());

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("<Root><name>this is a test</name><array><Item>12.34</Item><Item>34</Item><Item>56.78</Item><Item>78.901</Item></array></Root>");
    }

    [TestMethod]
    public void Build_WithArrayWithAllDataTypes_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<object> { (int)15.123412341234, (double)15.123412341234, (float)16.123412341234, false, (decimal)9.123412341234, "test", 'x', (long)9876.123412341234, (ulong)9876.123412341234 }.ToArray());

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("<Root><name>this is a test</name><array><Item>15</Item><Item>15.123412341234</Item><Item>16.123413</Item><Item>False</Item><Item>9.123412341234</Item><Item>test</Item><Item>x</Item><Item>9876</Item><Item>9876</Item></array></Root>");
    }
}