# Living Documentation

![Azure DevOps builds](https://img.shields.io/azure-devops/build/hompus/dccc1034-d776-48ea-8a70-8822a02987f9/6?style=plastic) ![Azure DevOps tests](https://img.shields.io/azure-devops/tests/hompus/LivingDocumentation/6?style=plastic)

Living Documentation allows you to analyze your dotnet source code and generate comprehensive documentation for your stakeholders.
It's a powerful tool that bridges the gap between code and documentation, ensuring that your documentation is always up-to-date with your source code.

## Features

* **Analyzer**: A tool to analyze dotnet projects or solutions.
* **Libraries**: Assists in generating applications that can create plain text files such as MarkDown, AsciiDoc, PlantUML, Mermaid, and more.

## Packages

| Package       | Type     | Status                                            |
| ------------- | -------- | ------------------------------------------------- |
| Analyzer Tool | Released | [![Nuget][NUGET_BADGE]][NUGET_FEED]               |
| Analyzer Tool | Preview  | [![Azure Artifacts][PREVIEW_BADGE]][PREVIEW_FEED] |

## Presentation

Watch the session given at NDC London 2023 that covers examples using this tool:

[![Use your source code to document your application - Michaël Hompus - NDC London 2023](https://img.youtube.com/vi/hf8hzGb2C6E/0.jpg)](https://www.youtube.com/watch?v=hf8hzGb2C6E)

## Getting Started

### Prerequisites

* Dotnet 6.0 SDK or newer.

## Installation

Install the analyzer as a dotnet global tool:

```shell
dotnet tool install --global LivingDocumentation.Analyzer
```

## Generating Documentation

Using LivingDocumentation to generate documentation involves a three-step process:

1. **Analyze Source Code**: Run the LivingDocumentation.Analyzer with your Visual Studio solution file as input.
   This will generate an intermediate JSON file containing detailed information about your source code.
2. **Develop Renderers**: Create a custom "render application" to interpret the JSON file and generate various views on your source code, such as class diagrams or sequence diagrams.
3. **Output Documentation**: Export your findings in text-based formats like Markdown, AsciiDoc, PlantUML, Mermaid, etc.

Both during local development, as during your CI&CD pipeline, can follow the same flow.

### Local Development

The analysis of a solution might take some time.
Therefore, an intermediate JSON file is created to speed up the documentation generation process.
This ensures a fast feedback loop when developing your renderers.

## Develop Your Own Renderers

A renderer application can be as simple as a command line tool that takes in the generated JSON files, makes conclusions based on the type information and writes this to a plain text file format.

To get started quickly, you should make a dependency on 2 NuGet packages in your project:

* **LivingDocumentation.RenderExtensions**: Contains extension methods and dependencies for serialized analysis.
* **LivingDocumentation.Json**: Contains JSON serializers and contract resolvers.

## More details

More details can be found in the [Guide](docs/guide.md).

## Dive Deeper

For more detailed examples and advanced use cases, refer to the second chapter of the [LivingDocumentation.Workshop](https://github.com/eNeRGy164/LivingDocumentation.Workshop/).

## Contributing

You are welcome to contribute! Feel free to create [issues](https://github.com/eNeRGy164/LivingDocumentation/issues) or [pull requests](https://github.com/eNeRGy164/LivingDocumentation/pulls).

## License

This project is licensed under the [MIT License](LICENSE).

[NUGET_BADGE]: https://img.shields.io/nuget/v/LivingDocumentation.Analyzer.svg?style=plastic
[NUGET_FEED]: https://www.nuget.org/packages/LivingDocumentation.Analyzer/
[PREVIEW_BADGE]: https://feeds.dev.azure.com/hompus/dccc1034-d776-48ea-8a70-8822a02987f9/_apis/public/Packaging/Feeds/030d64ca-8fad-4972-b7b7-8b1679c95e25/Packages/f3b0fbae-213f-412b-a98c-4d339e7a09e7/Badge
[PREVIEW_FEED]: https://dev.azure.com/hompus/LivingDocumentation/_packaging?_a=package&feed=030d64ca-8fad-4972-b7b7-8b1679c95e25&package=f3b0fbae-213f-412b-a98c-4d339e7a09e7&preferRelease=true
