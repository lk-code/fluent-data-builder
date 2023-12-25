using System.Xml;
using FluentAssertions;
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
        builder.Add("name", "John Doe");
        builder.Add("editor", new DataBuilder()
            .Add("typevalue", "a object")
            .Add("numbervalue", 55865)
            .Add("booleanvalue", true));

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("<Root><id>7c27a562-d405-4b22-9ade-37503bed6014</id><name>John Doe</name><editor><typevalue>a object</typevalue><numbervalue>55865</numbervalue><booleanvalue>True</booleanvalue></editor></Root>");
    }

    [TestMethod]
    public void Build_WithSimpleObject_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("number", 123);

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("<Root><name>this is a test</name><number>123</number></Root>");
    }

    [TestMethod]
    public void Build_WithStringList_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" });

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("<Root><name>this is a test</name><array><Item>this</Item><Item>is</Item><Item>a</Item><Item>test</Item></array></Root>");
    }

    [TestMethod]
    public void Build_WithStringArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<string> { "this", "is", "a", "test" }.ToArray());

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("<Root><name>this is a test</name><array><Item>this</Item><Item>is</Item><Item>a</Item><Item>test</Item></array></Root>");
    }

    [TestMethod]
    public void Build_WithMixedArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<object> { "this", 123, true, 456.78 }.ToArray());

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("<Root><name>this is a test</name><array><Item>this</Item><Item>123</Item><Item>True</Item><Item>456.78</Item></array></Root>");
    }

    [TestMethod]
    public void Build_WithNumericIntArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<int> { 12, 34, 56, 78 }.ToArray());

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("<Root><name>this is a test</name><array><Item>12</Item><Item>34</Item><Item>56</Item><Item>78</Item></array></Root>");
    }

    [TestMethod]
    public void Build_WithNumericDoubleArray_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("name", "this is a test");
        builder.Add("array", new List<double> { 12.34, 34, 56.78, 78.901 }.ToArray());

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("<Root><name>this is a test</name><array><Item>12.34</Item><Item>34</Item><Item>56.78</Item><Item>78.901</Item></array></Root>");
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

        XmlDocument xmlDocument = builder.Build();

        string result = xmlDocument.OuterXml;

        result.Should().NotBeNullOrEmpty();
        result.Should().Be("<Root><name>this is a test</name><array><Item>15</Item><Item>15.123412341234</Item><Item>16.123413</Item><Item>False</Item><Item>9.123412341234</Item><Item>test</Item><Item>x</Item><Item>9876</Item><Item>9876</Item></array></Root>");
    }

    [TestMethod]
    public void LoadFrom_WithJsonDocumentNull_Returns()
    {
        XmlDocument? data = null;
        IDataBuilder builder = new DataBuilder().LoadFrom(data!);

        builder.Should().NotBeNull();
        builder.GetProperties().Count.Should().Be(0);
    }

    [TestMethod]
    public void LoadFrom_WithSimpleJson_Returns()
    {
        string dataValue = """
                           <data>
                             <name>this is a test</name>
                             <number>123</number>
                             <decimal>123.45</decimal>
                             <boolean>true</boolean>
                             <null />
                             <array>
                               <item>this</item>
                               <item>is</item>
                               <item>a</item>
                               <item>test</item>
                             </array>
                             <object>
                               <name>this is a test</name>
                               <number>123</number>
                               <decimal>123.45</decimal>
                               <boolean>true</boolean>
                               <null />
                               <array>
                                 <item>this</item>
                                 <item>is</item>
                                 <item>a</item>
                                 <item>test</item>
                               </array>
                             </object>
                           </data>
                           """;
        
        var data = new XmlDocument();
        data.LoadXml(dataValue);

        IDataBuilder builder = new DataBuilder().LoadFrom(data);

        builder.Should().NotBeNull();
        var properties = builder.GetProperties();
        properties.Count.Should().Be(7);
        properties["name"].Should().Be("this is a test");
        properties["number"].Should().Be(123);
        properties["decimal"].Should().Be(123.45);
        properties["boolean"].Should().Be(true);
        properties["null"].Should().BeNull();
        properties["array"].Should().BeOfType<object[]>();
        properties["array"].Should().BeEquivalentTo(new List<string> { "this", "is", "a", "test" });
        properties["object"].Should().BeOfType<Dictionary<string, object>>();
        properties["object"].Should().BeEquivalentTo(new Dictionary<string, object>
        {
            { "name", "this is a test" },
            { "number", 123 },
            { "decimal", 123.45 },
            { "boolean", true },
            { "null", null! },
            { "array", new List<string> { "this", "is", "a", "test" } }
        });
    }
}