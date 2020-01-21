using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LivingDocumentation.RenderExtensions.Tests
{
    public partial class TypeDescriptionListExtensions
    {
        [TestMethod]
        public void PopulateInheritedMembers_NullTypes_Should_Throw()
        {
            // Assign
            var types = (List<TypeDescription>)null;

            // Act
            Action action = () => types.PopulateInheritedMembers();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("types");
        }

        [TestMethod]
        public void PopulateInheritedMembers_NoBaseTypes_Should_NotThrow()
        {
            // Assign
            var types = new[] { 
                new TypeDescription(TypeType.Class, "Test")
            };

            // Act
            Action action = () => types.PopulateInheritedMembers();

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void PopulateInheritedMembers_UnknownBaseTypes_Should_NotThrow()
        {
            // Assign
            var types = new[] {
                new TypeDescription(TypeType.Class, "Test")
                {
                    BaseTypes =
                    {
                        "XXX"
                    }
                },
            };

            // Act
            Action action = () => types.PopulateInheritedMembers();

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void PopulateInheritedMembers_BaseTypeMembers_Should_BeCopiedToImplementingType()
        {
            // Assign
            var baseType = new TypeDescription(TypeType.Class, "BaseTest");
            baseType.AddMember(new FieldDescription("int", "number"));

            var types = new[] {
                new TypeDescription(TypeType.Class, "Test")
                {
                    BaseTypes =
                    {
                        "BaseTest"
                    }
                },
                baseType
            };

            // Act
            types.PopulateInheritedMembers();

            // Assert
            types[0].Fields.Should().HaveCount(1);
            types[0].Fields[0].Should().NotBeNull();
            types[0].Fields[0].Name.Should().Be("number");
        }

        [TestMethod]
        public void PopulateInheritedMembers_PrivateFields_Should_NotBeCopiedToImplementingType()
        {
            // Assign
            var baseType = new TypeDescription(TypeType.Class, "BaseTest");
            baseType.AddMember(new FieldDescription("int", "number"));
            baseType.AddMember(new FieldDescription("int", "number2") { Modifiers = Modifier.Private });

            var types = new[] {
                new TypeDescription(TypeType.Class, "Test")
                {
                    BaseTypes =
                    {
                        "BaseTest"
                    }
                },
                baseType
            };

            // Act
            types.PopulateInheritedMembers();

            // Assert
            types[0].Fields.Should().HaveCount(1);
            types[0].Fields[0].Name.Should().Be("number");
        }
    }
}
