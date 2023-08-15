# Living Documentation Guide

Welcome to the LivingDocumentation guide.
This guide will walk you through the process of analyzing your projects or solutions using LivingDocumentation.

## Analyzing a Project or Solution

### Installation

To get started, you'll need to install the LivingDocumentation tool, which is available as a dotnet tool. Install it using the following command:

```shell
dotnet tool install --global LivingDocumentation.Analyzer
```

### Analysis

To analyze a project or solution, use the following commands:

For a project:

```shell
livingdoc-analyze --project your-project.csproj --output analysis.json
```

For a solution:

```shell
livingdoc-analyze --solution your-solution.sln --output analysis.json
```

Available switches:

| Switch        | Description                                           |
| ------------- | ----------------------------------------------------- |
| --project     | Specifies the path to the project file for analysis.  |
| --solution    | Specifies the path to the solution file for analysis. |
| --output      | Specifies the path for the output file.               |
| -v, --verbose | Enables warnings during compilation (default: false). |
| -p, --pretty  | Pretty-prints the JSON output (default: false).       |
| -q, --quiet   | Suppresses informational messages (default: false).   |
| --help        | Displays the help screen.                             |
| --version     | Displays version information.                         |

## Using the Analysis Results

### Reading Serialized Data

1. Add references to the `LivingDocumentation.Json` and `LivingDocumentation.RenderExtensions` NuGet packages in your project.
2. Read the serialized data from the output file.
3. Deserialize the content using the provided `JsonDefaults.DeserializerSettings()`.

```csharp
var fileContents = File.ReadAllText("analysis.json");
var types = JsonConvert.DeserializeObject<List<TypeDescription>>(fileContents, JsonDefaults.DeserializerSettings())!.ToList();
```

### Fetching Specific Type Information

Retrieve specific type details using the `First` or `FirstOrDefault` methods:

```csharp
var type = types.FirstOrDefault("Pitstop.TimeService.Events.DayHasPassed");
```

### Fix inheritance

To optimize the JSON file, base types and inheritance aren't directly reflected on the members of a type. To address this:

```csharp
Types.PopulateInheritedBaseTypes();
Types.PopulateInheritedMembers();
```

### Exploring Properties of a Type

Various properties of a type are available, such as:

| Property     | Description                               |
| ------------ | ----------------------------------------- |
| Type         | The type category (e.g. Class, Interface) |
| FullName     | The full name, including Namespace.       |
| Namespace    | The namespace.                            |
| Name         | The name.                                 |
| BaseTypes    | The base types, this includes Interfaces. |
| Attributes   | The declared attributes.                  |
| Constructors | The constructors.                         |
| Properties   | The properties.                           |
| Methods      | The methods.                              |
| Events       | The events.                               |
| Fields       | The fields.                               |
| EnumMembers  | The enum members.                         |

**Helpful methods:**

| Method                   | Description                                                                                      |
| ------------------------ | ------------------------------------------------------------------------------------------------ |
| IsClass                  | Returns true if the type is a class.                                                             |
| IsInterface              | Returns true if the type is an interface.                                                        |
| IsEnum                   | Returns true if the type is an enum.                                                             |
| IsStruct                 | Returns true if the type is a struct.                                                            |
| HasProperty              | Returns true if the type has a property with the given name.                                     |
| HasMethod                | Returns true if the type has a method with the given name.                                       |
| HasEvent                 | Returns true if the type has an event with the given name.                                       |
| HasField                 | Returns true if the type has a field with the given name.                                        |
| HasEnumMember            | Returns true if the type has an enum member with the given name.                                 |
| ImplementsType           | Returns true if the type implements the given type.                                              |
| ImplementsTypeStartsWith | Returns true if the type implements a type that starts with the given name. Useful for generics. |

For every description with modifiers (like class, method, property), several methods are available:

| Method      | Description                        |
| ----------- | ---------------------------------- |
| IsStatic    | Checks if the type is static.      |
| IsAbstract  | Checks if the type is abstract.    |
| IsSealed    | Checks if the type is sealed.      |
| IsOverride  | Checks if the type is an override. |
| IsVirtual   | Checks if the type is virtual.     |
| IsReadOnly  | Checks if the type is read only.   |
| IsUnsafe    | Checks if the type is unsafe.      |
| IsPublic    | Checks if the type is public.      |
| IsInternal  | Checks if the type is internal.    |
| IsProtected | Checks if the type is protected.   |
| IsPrivate   | Checks if the type is private.     |
| IsAsync     | Checks if the type is async.       |
| IsConst     | Checks if the type is a const.     |
| IsPartial   | Checks if the type is partial.     |
| IsNew       | Checks if the type is new.         |

## Common Issues and Solutions

**Issue**: Challenges when analyzing LivingDocumentation-based applications.

**Description**: Analyzing a project that references LivingDocumentation with the tool can lead to issues.

**Solutions**:

* Remove the reference to the project containing LivingDocumentation dependencies during analysis.
* Consider creating a separate solution file that excludes the problematic projects and analyze that solution.

## Dive Deeper

For more detailed examples and advanced use cases, refer to the second chapter of the [LivingDocumentation.Workshop](https://github.com/eNeRGy164/LivingDocumentation.Workshop/).
