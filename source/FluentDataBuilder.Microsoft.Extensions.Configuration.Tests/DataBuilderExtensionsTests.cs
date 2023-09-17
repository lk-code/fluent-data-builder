using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentDataBuilder.Microsoft.Extensions.Configuration.Tests;

[TestClass]
public class DataBuilderExtensionsTests
{
    [TestMethod]
    public void Build_WithChildObject_Returns()
    {
        IDataBuilder builder = new DataBuilder();

        builder.Add("Tests", new DataBuilder()
            .Add("Id", "7c27a562-d405-4b22-9ade-37503bed6014"));
        
        IConfiguration configuration = builder.ToConfiguration();

        string value = configuration.GetSection("Tests:Id").Get<string>()!;

        value.Should().NotBeNullOrEmpty();
        value.Should().Be("7c27a562-d405-4b22-9ade-37503bed6014");
    }
}