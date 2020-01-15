using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class DeserializationTests
    {
        [TestMethod]
        public void NoTypes_Should_GiveEmptyArray()
        {
            // Assign
            var json = @"[]";

            // Act
            var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

            // Assert
            types.Should().BeEmpty();
        }

        [TestMethod]
        public void OnlyFullName_Should_GiveInternalClass()
        {
            // Assign
            var json = @"[{""FullName"":""Test""}]";

            // Act
            var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

            // Assert
            types.Should().HaveCount(1);
            types[0].Should().NotBeNull();
            types[0].Type.Should().Be(TypeType.Class);
            types[0].FullName.Should().Be("Test");
            types[0].Modifiers.Should().Be(Modifier.Internal);
        }

        [TestMethod]
        public void Collections_Should_NotBeNull()
        {
            // Assign
            var json = @"[{""FullName"":""Test""}]";

            // Act
            var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

            // Assert
            types[0].Fields.Should().BeEmpty();
            types[0].Constructors.Should().BeEmpty();
            types[0].Properties.Should().BeEmpty();
            types[0].Methods.Should().BeEmpty();
            types[0].EnumMembers.Should().BeEmpty();
            types[0].Events.Should().BeEmpty();
        }

        [TestMethod]
        public void Modifiers2_Should_GivePublicClass()
        {
            // Assign
            var json = @"[{""Modifiers"":2,""FullName"":""Test""}]";

            // Act
            var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

            // Assert
            types[0].Modifiers.Should().Be(Modifier.Public);
        }

        [TestMethod]
        public void MethodsCollection_Should_GivePrivateMethod()
        {
            // Assign
            var json = @"[{""FullName"":""Test"",""Methods"":[{""Name"":""Method""}]}]";

            // Act
            var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

            // Assert
            types[0].Methods.Should().HaveCount(1);
            types[0].Methods[0].Should().NotBeNull();
            types[0].Methods[0].Name.Should().Be("Method");
            types[0].Methods[0].Modifiers.Should().Be(Modifier.Private);
        }

        [TestMethod]
        public void AttributeCollection_Should_GiveAttributeWithNameAndType()
        {
            // Assign
            var json = @"[{""FullName"":""Test"",""Attributes"":[{""Type"":""System.ObsoleteAttribute"",""Name"":""System.Obsolete""}]}]";

            // Act
            var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

            // Assert
            types[0].Attributes.Should().HaveCount(1);
            types[0].Attributes[0].Should().NotBeNull();
            types[0].Attributes[0].Type.Should().Be("System.ObsoleteAttribute");
            types[0].Attributes[0].Name.Should().Be("System.Obsolete");
        }

        [TestMethod]
        public void AttributeArgumentCollection_Should_GiveAttributeArgumentWithName_TypeAndValue()
        {
            // Assign
            var json = @"[{""FullName"":""Test"",""Attributes"":[{""Type"":""System.ObsoleteAttribute"",""Name"":""System.Obsolete"",""Arguments"":[{""Name"":""\""Reason\"""",""Type"":""string"",""Value"":""Reason""}]}]}]";

            // Act
            var types = JsonConvert.DeserializeObject<List<TypeDescription>>(json, JsonDefaults.DeserializerSettings());

            // Assert
            types[0].Attributes[0].Arguments.Should().HaveCount(1);
            types[0].Attributes[0].Arguments[0].Should().NotBeNull();
            types[0].Attributes[0].Arguments[0].Type.Should().Be("string");
            types[0].Attributes[0].Arguments[0].Name.Should().Be(@"""Reason""");
            types[0].Attributes[0].Arguments[0].Value.Should().Be(@"Reason");
        }
    }
}
