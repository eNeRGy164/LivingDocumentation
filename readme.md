# Living Documentation

![Azure DevOps builds](https://img.shields.io/azure-devops/build/hompus/dccc1034-d776-48ea-8a70-8822a02987f9/6?style=plastic) ![Azure DevOps tests](https://img.shields.io/azure-devops/tests/hompus/LivingDocumentation/6?style=plastic)

| Package | Type | Status
|-|-|-|
| Analyzer Tool | Released | [![Nuget](https://img.shields.io/nuget/v/LivingDocumentation.Analyzer?color=0071C8&label=NuGet&logo=NuGet)](https://www.nuget.org/packages/LivingDocumentation.Analyzer)
| Analyzer Tool | Preview | [![LivingDocumentation.Analyzer package in Preview feed in Azure Artifacts](https://feeds.dev.azure.com/hompus/dccc1034-d776-48ea-8a70-8822a02987f9/_apis/public/Packaging/Feeds/030d64ca-8fad-4972-b7b7-8b1679c95e25/Packages/f3b0fbae-213f-412b-a98c-4d339e7a09e7/Badge)](https://dev.azure.com/hompus/LivingDocumentation/_packaging?_a=package&feed=030d64ca-8fad-4972-b7b7-8b1679c95e25&package=f3b0fbae-213f-412b-a98c-4d339e7a09e7&preferRelease=false)

---

// TODO: Write explaination

## Running the sample

These steps expect the **eShopOnContainers** and **LivingDocumentation** repos to be subdirectories of the location where you execute these commands:

```plain
|
+-- eShopOnContainers
+-- LivingDocumentation
```

1. Install the analyzer as a global tool

   ```sh
   dotnet tool install --global LivingDocumentation.Analyzer
   ```

2. Make sure you've build the solution you want to document

   ```sh
   dotnet build eShopOnContainers/eShopOnContainers-ServicesAndWebApps.sln -c Release
   ```

3. Run the analyzer and store the intermediate output

   ```sh
   livingdoc-analyze --solution eShopOnContainers/eShopOnContainers-ServicesAndWebApps.sln --output analysis.json
   ```

4. Run you project specific renderer

   ```sh
   dotnet run --project LivingDocumentation\samples\LivingDocumentation.eShopOnContainers -c Release
   ```
