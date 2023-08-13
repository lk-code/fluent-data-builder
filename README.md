# FluentDataBuilder

![html-compiler-tool](https://raw.githubusercontent.com/lk-code/fluent-data-builder/main/icon_128.png)

[![.NET Version](https://img.shields.io/badge/dotnet%20version-net6.0-blue?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder/)
[![.NET Version](https://img.shields.io/badge/dotnet%20version-net7.0-blue?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder/)
[![License](https://img.shields.io/github/license/lk-code/fluent-data-builder.svg?style=flat-square)](https://github.com/lk-code/fluent-data-builder/blob/master/LICENSE)
[![Downloads](https://img.shields.io/nuget/dt/FluentDataBuilder.svg?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder/)
[![NuGet](https://img.shields.io/nuget/v/FluentDataBuilder.Json.svg?style=flat-square)](https://www.nuget.org/packages/FluentDataBuilder.Json/)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=lk-code_fluent-data-builder&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=lk-code_fluent-data-builder)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=lk-code_fluent-data-builder&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=lk-code_fluent-data-builder)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=lk-code_fluent-data-builder&metric=coverage)](https://sonarcloud.io/summary/new_code?id=lk-code_fluent-data-builder)

[![buy me a coffe](https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png)](https://www.buymeacoffee.com/lk.code)

A fluent data builder for json and xml

## DataBuilder Output Libraries

FluentDataBuilder has some NuGet-Libraries to generate output:

### System.Text.Json - FluentDataBuilder.Json

install the nuget `FluentDataBuilder.Json` and follow the general DataBuilder steps. the following code is a sample for the initialization with System.Text.Json.

`IDataBuilder builder = new DataBuilder();`<br />
`...`<br />
`JsonDocument jsonResult = builder.Build();`<br />

### Newtonsoft.Json - FluentDataBuilder.NewtonsoftJson

install the nuget `FluentDataBuilder.NewtonsoftJson` and follow the general DataBuilder steps. the following code is a sample for the initialization with Newtonsoft.Json.

`IDataBuilder builder = new DataBuilder();`<br />
`...`<br />
`JObject jsonResult = builder.Build();`<br />

### System.Xml - FluentDataBuilder.Xml

install the nuget `FluentDataBuilder.Xml` and follow the general DataBuilder steps. the following code is a sample for the initialization with System.Xml.

`IDataBuilder builder = new DataBuilder();`<br />
`...`<br />
`XmlDocument xmlDocument = builder.Build();`<br />

## How to create Data Objects

First you need to create an instance:
`IDataBuilder builder = new DataBuilder();`<br />

Use the Add-Method to add data:

### add simple properties

`builder.Add("StringProperty", "a value");`<br />
`builder.Add("NumericProperty", 12345);`<br />
`builder.Add("BooleanProperty", true);`<br />

**result (in json):**

`{`<br />
`    "StringProperty": "a value",`<br />
`    "NumericProperty": 12345,`<br />
`    "BooleanProperty": true`<br />
`}`<br />

### add arrays

`builder.Add("ListProperty", new List<string> { "this", "is", "a", "test" });`<br />
`builder.Add("ArrayProperty", new string[] { "this", "is", "a", "test" });`<br />
`builder.Add("MixedListProperty", new List<object> { "value", 123, true, 456.78 });`<br />

**result (in json):**

`{`<br />
`    "ListProperty":`<br />
`    [`<br />
`        "this",`<br />
`        "is",`<br />
`        "a",`<br />
`        "test"`<br />
`    ],`<br />
`    "ArrayProperty":`<br />
`    [`<br />
`        "this",`<br />
`        "is",`<br />
`        "a",`<br />
`        "test"`<br />
`    ],`<br />
`    "MixedListProperty":`<br />
`    [`<br />
`        "value",`<br />
`        123,`<br />
`        true,`<br />
`        456.78`<br />
`    ]`<br />
`}`<br />

### add new object

`builder.Add("ObjectProperty", new DataBuilder()`<br />
`    .Add("StringProperty", "another value")`<br />
`    .Add("NumericProperty", 67890)`<br />
`    .Add("BooleanProperty", false));`<br />

**result (in json):**

`{`<br />
`    "ObjectProperty":`<br />
`    {`<br />
`        "StringProperty": "another value",`<br />
`        "NumericProperty": 67890,`<br />
`        "BooleanProperty": false`<br />
`    }`<br />
`}`<br />
