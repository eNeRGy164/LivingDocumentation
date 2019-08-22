# Living Documentation

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
