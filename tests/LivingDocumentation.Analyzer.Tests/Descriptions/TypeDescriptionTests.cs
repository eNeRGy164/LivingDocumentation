using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class TypeDescriptionTests
    {
        [TestMethod]
        public void TypeDescription_GetHashCode_Should_BeTheSame()
        {
            var descriptionX = new TypeDescription(0, "TestNamespace.TestClass").GetHashCode();
            var descriptionY = new TypeDescription(0, "TestNamespace.TestClass").GetHashCode();

            descriptionX.Should().Be(descriptionY);
        }

        [TestMethod]
        public void TypeDescription_GetHashCode_Should_ShouldBeTheSameForDifferentTypeTypes()
        {
            var descriptionX = new TypeDescription(TypeType.Class, "TestNamespace.TestClass").GetHashCode();
            var descriptionY = new TypeDescription(TypeType.Interface, "TestNamespace.TestClass").GetHashCode();

            descriptionX.Should().Be(descriptionY);
        }

        [TestMethod]
        public void TypeDescription_GetHashCode_Should_DifferentNamesShouldDiffer()
        {
            var descriptionX = new TypeDescription(0, "TestNamespace.TestClass1").GetHashCode();
            var descriptionY = new TypeDescription(0, "TestNamespace.TestClass2").GetHashCode();

            descriptionX.Should().NotBe(descriptionY);
        }

        [TestMethod]
        public void TypeDescription_Equals_Should_BeTrueForSameTypeDescriptions()
        {
            var descriptionX = new TypeDescription(0, "TestNamespace.TestClass");
            var descriptionY = new TypeDescription(0, "TestNamespace.TestClass");

            descriptionX.Equals(descriptionY).Should().BeTrue();
        }

        [TestMethod]
        public void TypeDescription_Equals_Should_BeFalseForDifferentCasing()
        {
            var descriptionX = new TypeDescription(0, "TestNamespace.TestClass");
            var descriptionY = new TypeDescription(0, "testnamespace.testclass");

            descriptionX.Equals(descriptionY).Should().BeFalse();
        }

        [TestMethod]
        public void TypeDescription_Equals_Should_BeFalseForDifferentTypeDescriptions()
        {
            var descriptionX = new TypeDescription(0, "TestNamespace.TestClass1");
            var descriptionY = new TypeDescription(0, "TestNamespace.TestClass2");

            descriptionX.Equals(descriptionY).Should().BeFalse();
        }

        [TestMethod]
        public void TypeDescription_Equals_Should_BeFalseForDifferentObjects()
        {
            var descriptionX = new TypeDescription(0, "TestNamespace.TestClass");
            var descriptionY = new object();

            descriptionX.Equals(descriptionY).Should().BeFalse();
        }

        [TestMethod]
        public void TypeDescription_Name_Should_BeFullNameIfThereAreNoDots()
        {
            var description = new TypeDescription(0, "TestClass");

            description.Name.Should().Be("TestClass");
        }

        [TestMethod]
        public void TypeDescription_Name_Should_BeParsedFromFullName()
        {
            var description = new TypeDescription(0, "TestNamespace.TestClass");

            description.Name.Should().Be("TestClass");
        }

        [TestMethod]
        public void TypeDescription_Namespace_Should_BeEmptyIfThereAreNoDots()
        {
            var description = new TypeDescription(0, "TestClass");

            description.Namespace.Should().BeEmpty();
        }

        [TestMethod]
        public void TypeDescription_Namespace_Should_BeParsedFromFullName()
        {
            var description = new TypeDescription(0, "TestNamespace.TestClass");

            description.Namespace.Should().Be("TestNamespace");
        }

        [TestMethod]
        public void TypeDescription_Constructor_Should_SetFullName()
        {
            var description = new TypeDescription(0, "TestNamespace.TestClass");

            description.FullName.Should().Be("TestNamespace.TestClass");
        }

        [TestMethod]
        public void TypeDescription_Constructor_Should_SetFullNameEmptyIfNull()
        {
            var description = new TypeDescription(TypeType.Class, null);

            description.FullName.Should().BeEmpty();
        }

        [TestMethod]
        public void TypeDescription_Constructor_Should_SetType()
        {
            var description = new TypeDescription(TypeType.Struct, null);

            description.Type.Should().Be(TypeType.Struct);
        }

        [TestMethod]
        public void TypeDescription_AddMember_Should_ThrowEdescriptionception()
        {
            var description = new TypeDescription(0, null);

            Action action = () => { description.AddMember(new UnsupportedMemberDescription(null)); };

            action.Should().Throw<NotSupportedException>();
        }

        [TestMethod]
        public void TypeDescription_BaseTypes_Should_BeEmpty()
        {
            var description = new TypeDescription(0, null);

            description.BaseTypes.Should().BeEmpty();
        }

        [TestMethod]
        public void TypeDescription_Attributes_Should_BeEmpty()
        {
            var description = new TypeDescription(0, null);

            description.Attributes.Should().BeEmpty();
        }

        private class UnsupportedMemberDescription : MemberDescription
        {
            public UnsupportedMemberDescription(string name) : base(name)
            {
            }

            public override MemberType MemberType => throw new NotImplementedException();
        }
    }
}
