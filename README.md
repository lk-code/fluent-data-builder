# FluentDataBuilder

![fluent-data-builder](https://raw.githubusercontent.com/lk-code/fluent-data-builder/main/icon_128.png)

[![License](https://img.shields.io/github/license/lk-code/fluent-data-builder.svg?style=flat-square)](https://github.com/lk-code/fluent-data-builder/blob/master/LICENSE)
[![Downloads](https://img.shields.io/nuget/dt/FluentDataBuilder.svg?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder/)

[![.NET Version](https://img.shields.io/badge/dotnet%20version-net8.0-blue?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder/)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=lk-code_fluent-data-builder&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=lk-code_fluent-data-builder)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=lk-code_fluent-data-builder&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=lk-code_fluent-data-builder)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=lk-code_fluent-data-builder&metric=coverage)](https://sonarcloud.io/summary/new_code?id=lk-code_fluent-data-builder)

[![buy me a coffe](https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png)](https://www.buymeacoffee.com/lk.code)

A fluent data builder for json and xml

## DataBuilder Output Libraries

FluentDataBuilder has some NuGet-Libraries to generate output:

| Package                                              | Target                             | NuGet                                                                                                                                                                                                       |
|------------------------------------------------------|------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| FluentDataBuilder.Json                               | System.Text.Json                   | [![NuGet](https://img.shields.io/nuget/v/FluentDataBuilder.Json.svg?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder.Json/)                                                             |
| FluentDataBuilder.NewtonsoftJson                     | Newtonsoft.Json                    | [![NuGet](https://img.shields.io/nuget/v/FluentDataBuilder.NewtonsoftJson.svg?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder.NewtonsoftJson/)                                         |
| FluentDataBuilder.Microsoft.Extensions.Configuration | Microsoft.Extensions.Configuration | [![NuGet](https://img.shields.io/nuget/v/FluentDataBuilder.Microsoft.Extensions.Configuration.svg?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder.Microsoft.Extensions.Configuration/) |
| FluentDataBuilder.Xml                                | System.Xml                         | [![NuGet](https://img.shields.io/nuget/v/FluentDataBuilder.Xml.svg?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder.Xml/)                                                               |
| FluentDataBuilder.Yaml                               | YamlDotNet                         | [![NuGet](https://img.shields.io/nuget/v/FluentDataBuilder.Yaml.svg?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder.Yaml/)                                                              |

## Examples

### FluentDataBuilder.Json (System.Text.Json)

install the nuget `FluentDataBuilder.Json` and follow the general DataBuilder steps. the following code is a sample for the initialization with System.Text.Json.

```
IDataBuilder builder = new DataBuilder();
...
JsonDocument jsonResult = builder.Build();
```

### FluentDataBuilder.NewtonsoftJson (Newtonsoft.Json)

install the nuget `FluentDataBuilder.NewtonsoftJson` and follow the general DataBuilder steps. the following code is a sample for the initialization with Newtonsoft.Json.

```
IDataBuilder builder = new DataBuilder();
...
JObject jsonResult = builder.Build();
```

### FluentDataBuilder.Microsoft.Extensions.Configuration (Microsoft.Extensions.Configuration)

With this package you can store the DataBuilder instance directly as an IConfiguration instance.

install the nuget `FluentDataBuilder.Microsoft.Extensions.Configuration` and follow the general DataBuilder steps. the following code is a sample for the initialization with Newtonsoft.Json.

```
IDataBuilder builder = new DataBuilder();
...
IConfiguration configuration = builder.ToConfiguration();
```

### FluentDataBuilder.Xml (System.Xml)

install the nuget `FluentDataBuilder.Xml` and follow the general DataBuilder steps. the following code is a sample for the initialization with System.Xml.

```
IDataBuilder builder = new DataBuilder();
...
XmlDocument xmlDocument = builder.Build();
```

### FluentDataBuilder.Yaml (YamlDotNet)

install the nuget `FluentDataBuilder.Yaml` and follow the general DataBuilder steps..

```
IDataBuilder builder = new DataBuilder();
...
string yamlContent = builder.Build();
```

## How to create Data Objects

First you need to create an instance:

```
IDataBuilder builder = new DataBuilder();
```

Use the Add-Method to add data:

### add simple properties

```
builder.Add("StringProperty", "a value");
builder.Add("NumericProperty", 12345);
builder.Add("BooleanProperty", true);
```

**result (in json):**

```
{
    "StringProperty": "a value",
    "NumericProperty": 12345,
    "BooleanProperty": true
}
```

### add arrays

```
builder.Add("ListProperty", new List<string> { "this", "is", "a", "test" });
builder.Add("ArrayProperty", new string[] { "this", "is", "a", "test" });
builder.Add("MixedListProperty", new List<object> { "value", 123, true, 456.78 });
```

**result (in json):**

```
{
    "ListProperty":
    [
        "this",
        "is",
        "a",
        "test"
    ],
    "ArrayProperty":
    [
        "this",
        "is",
        "a",
        "test"
    ],
    "MixedListProperty":
    [
        "value",
        123,
        true,
        456.78
    ]
}
```

### add new object

```
builder.Add("ObjectProperty", new DataBuilder()
    .Add("StringProperty", "another value")
    .Add("NumericProperty", 67890)
    .Add("BooleanProperty", false));
```

**result (in json):**

```
{
    "ObjectProperty":
    {
        "StringProperty": "another value",
        "NumericProperty": 67890,
        "BooleanProperty": false
    }
}
```
