namespace LivingDocumentation.Extensions.Tests
{
    [TestClass]
    public partial class StringExtensionsTests
    {
        [DataRow(null, "", DisplayName = "A `null` value should return an empty string")]
        [DataRow("", "", DisplayName = "An empty string value should return an empty string")]
        [DataRow(".", "", DisplayName = "A value with only a dot should return an empty string")]
        [DataRow("Class", "Class", DisplayName = "A class name without a namespace should return the class name")]
        [DataRow("Namespace.Class", "Class", DisplayName = "A class in a namespace should only return the class name")]
        [DataRow(".Class", "Class", DisplayName = "A value starting with a dot should only return the class name")]
        [DataRow("Namespace.Class<String>", "Class<String>", DisplayName = "A generic class in a namespace should return the class name with the generic part")]
        [TestMethod]
        public void ReduceAFullTypeNameToOnlyTheClassPart(string fullname, string expectation)
        {
            // Act
            var result = fullname.ClassName();

            // Assert
            result.Should().Be(expectation);
        }

        [DataRow(null, "", DisplayName = "A `null` value should return an empty string")]
        [DataRow("", "", DisplayName = "An empty string value should return an empty string")]
        [DataRow(".", "", DisplayName = "A value with only a dot should return an empty string")]
        [DataRow("Class", "", DisplayName = "A class name without a namespace should return an empty string")]
        [DataRow("Namespace.Class", "Namespace", DisplayName = "A class in a namespace should only return the namespace name")]
        [DataRow(".Class", "", DisplayName = "A value starting with a dot should only return an empty string")]
        [DataRow(".Namespace.Class", "Namespace", DisplayName = "A value starting with a dot should only return the namespace name")]
        [DataRow("Namespace.Class<String>", "Namespace", DisplayName = "A generic class in a namespace should return the namespace name")]
        [DataRow("Namespace.Namespace.Namespace.Class", "Namespace.Namespace.Namespace", DisplayName = "A value with a hiearchy of namespaces should return all namespace parts")]
        [TestMethod]
        public void ReduceAFullTypeNameToOnlyTheNamespacePart(string fullname, string expectation)
        {
            // Act
            var result = fullname.Namespace();

            // Assert
            result.Should().Be(expectation);
        }

        [DataRow(null, 0, DisplayName = "A `null` value should return an empty list")]
        [DataRow("", 0, DisplayName = "An empty string value should return an empty list")]
        [DataRow(".", 0, DisplayName = "A value with only a dot should return an empty list")]
        [DataRow("Class", 1, DisplayName = "A class name without a namespace should return a list with a single item")]
        [DataRow("Namespace.Class", 2, DisplayName = "A class in a namespace should return a list with a two item")]
        [DataRow(".Class", 1, DisplayName = "A value starting with a dot should return a list with a single item")]
        [DataRow(".Namespace.Class", 2, DisplayName = "A value starting with a dot should return a list with a two item")]
        [DataRow("Namespace.Class<String>", 2, DisplayName = "A generic class in a namespace should return a list with a single item")]
        [DataRow("Namespace.Namespace.Namespace.Class", 4, DisplayName = "A value with a hiearchy of namespaces should return a list with four items")]
        [TestMethod]
        public void AllValidNamespacePartsShouldBeReturned(string fullname, int expectation)
        {
            // Act
            var result = fullname.NamespaceParts();

            // Assert
            result.Should().HaveCount(expectation);
        }
    }
}
