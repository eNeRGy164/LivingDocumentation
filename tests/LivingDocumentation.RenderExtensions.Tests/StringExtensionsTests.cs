namespace LivingDocumentation.RenderExtensions.Tests;

[TestClass]
public class StringExtensionsTests
{
    [DataRow("GenericTypes", DisplayName = "`GenericTypes()` should guard against a null reference exception")]
    [DataRow("IsEnumerable", DisplayName = "`IsEnumerable()` should guard against a null reference exception")]
    [DataRow("IsGeneric", DisplayName = "`IsGeneric()` should guard against a null reference exception")]
    [DataRow("ForDiagram", DisplayName = "`ForDiagram()` should guard against a null reference exception")]
    [TestMethod]
    public void ExtensionMethodShouldGuardAgainstNRE(string methodName)
    {
        // Assign
        string type = default;

        var method = typeof(IEnumerableIAttributeDescriptionExtensions).Assembly.GetType("LivingDocumentation.StringExtensions").GetMethod(methodName);
        var parameters = new object[] { type }.ToArray();

        // Act
        Action action = () => method.Invoke(null, parameters);

        // Assert
        action.Should().ThrowExactly<System.Reflection.TargetInvocationException>()
            .WithInnerExceptionExactly<ArgumentNullException>()
            .And.ParamName.Should().Be("type");
    }

    [DynamicData(nameof(GetGenericTypes), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(GetGenericTypesDisplayName))]
    [TestMethod]
    public void GenericTypesShouldBeCorrectlyExtracted(string type, string[] expected)
    {
        // Act
        var result = type.GenericTypes();

        // Assert
        result.Should().Equal(expected);
    }

    private static IEnumerable<object[]> GetGenericTypes()
    {
        yield return new object[] { "System.Object", new string[0] };
        yield return new object[] { "System.Collections.Generic.List<System.String>", new[] { "System.String" } };
        yield return new object[] { " System.Collections.Generic.List< System.String > ", new[] { "System.String" } };
        yield return new object[] { "System.Collections.Generic.Dictionary<System.String,System.String>", new[] { "System.String", "System.String" } };
        yield return new object[] { " System.Collections.Generic.Dictionary< System.String , System.String > ", new[] { "System.String", "System.String" } };
        yield return new object[] { "System.Collections.Generic.List<System.Nullable<System.Int32>>", new[] { "System.Nullable<System.Int32>" } };
        yield return new object[] { " System.Collections.Generic.List< System.Nullable < System.Int32 > > ", new[] { "System.Nullable < System.Int32 >" } };
        yield return new object[] { "System.Nullable<System.Collections.Generic.Dictionary<System.Int32,System.Object>>", new[] { "System.Collections.Generic.Dictionary<System.Int32,System.Object>" } };
    }

    public static string GetGenericTypesDisplayName(System.Reflection.MethodInfo _, object[] data)
    {
        return $"Value `{data[0]}` should return `[{string.Join(", ", (string[])data[1])}]`";
    }

    [DataRow("System.Collections.Generic.LinkedList<System.String>", true, DisplayName = "A generic collection should return `true`")]
    [DataRow("System.Collections.Generic.IList<System.String>", true, DisplayName = "A generic interface collection should return `true`")]
    [DataRow("System.Collections.Generic.Dictionary<System.String,System.String>.Enumerator", false, DisplayName = "A enumerator of a generic collection should return `false`")]
    [DataRow("System.Collections.Generic.Comparer<System.String,System.String>", false, DisplayName = "A generic comparer should return `false`")]
    [DataRow("System.Collections.Generic.KeyNotFoundException", false, DisplayName = "An exception type in the generic namespace should return `false`")]
    [DataRow("System.Collections.Concurrent.ConcurrentStack<System.String>", true, DisplayName = "A concurrent collection should return `true`")]
    [DataRow("System.Collections.Concurrent.Partitioner", false, DisplayName = "A partioner in the concurrent namespace should return `false`")]
    [DataRow("System.Collections.Concurrent.OrderablePartitioner<System.String>", false, DisplayName = "A generic partioner in the concurrent namespace should return `false`")]
    [DataRow("System.Collections.ArrayList", true, DisplayName = "A non-generic enumerable should return `true`")]
    [DataRow("System.Collections.CaseInsensitiveComparer", false, DisplayName = "A non-generic comparer should return `false`")]
    [DataRow("System.Collections.CaseInsensitiveHashCodeProvider", false, DisplayName = "A non-generic hash code provider in the collections namespace should return `false`")]
    [DataRow("System.Collections.Comparer", false, DisplayName = "A non-generic comparer in the collections namespace should return `false`")]
    [DataRow("System.Collections.StructuralComparisons", false, DisplayName = "A non-generic comparisons type in the collections namespace should return `false`")]
    [DataRow("System.Object", false, DisplayName = "A simple type should return `false`")]
    [TestMethod]
    public void TypesShouldBeCorrectlyDetectedAsEnumerable(string type, bool expectation)
    {
        // Act
        var result = type.IsEnumerable();

        // Assert
        result.Should().Be(expectation);
    }

    [DataRow("System.Object", false, DisplayName = "A non-generic type should return `false`")]
    [DataRow("System.Collections.Generic.Dictionary<System.String,System.String>", true, DisplayName = "A generic type should return `true`")]
    [DataRow("System.Collections.Generic.Dictionary<System.String,System.String>.Enumerator", false, DisplayName = "A non-generic embedded type should return `false`")]
    [TestMethod]
    public void DetectGenericTypesCorrectly(string type, bool expectation)
    {
        // Act
        var result = type.IsGeneric();

        // Assert
        result.Should().Be(expectation);
    }

    [DataRow(null, null, DisplayName = "A `null` string should return the same")]
    [DataRow("", "", DisplayName = "An empty string should return the same")]
    [DataRow("Object", "Object", DisplayName = "A Pascal cased single word should return the same")]
    [DataRow("InvocationsAnalyzer", "Invocations Analyzer", DisplayName = "Multiple Pascal cased words should be seperated by a space")]
    [DataRow("InvocationsAnalyzerAPIService", "Invocations Analyzer API Service", DisplayName = "An abbrevation should be kept together")]
    [DataRow("InvocationsAnalyzer164Class", "Invocations Analyzer 164 Class", DisplayName = "Numbers should be kept together")]
    [TestMethod]
    public void ToSentenceCaseShouldReturnTheCorrectString(string input, string expectation)
    {
        // Act
        var result = input.ToSentenceCase();

        // Assert
        result.Should().Be(expectation);
    }

    [DataRow("Object", "Object", DisplayName = "A class without namespace should return the same")]
    [DataRow("System.Object", "Object", DisplayName = "A class in a namespace should only return the class name")]
    [DataRow("System.Collections.Generic.IList<System.String>", "IList<String>", DisplayName = "A generic class in a namespace should only return the class name with stripped type parameters")]
    [DataRow("System.Collections.Generic.Dictionary<System.String,System.String>", "Dictionary<String, String>", DisplayName = "A generic class in a namespace with multiple type parameters should only return the class name with stripped type parameters")]
    [TestMethod]
    public void ForDiagramStripNamespacesFromTypes(string input, string expectation)
    {
        // Act
        var result = input.ForDiagram();

        // Assert
        result.Should().Be(expectation);
    }
}
