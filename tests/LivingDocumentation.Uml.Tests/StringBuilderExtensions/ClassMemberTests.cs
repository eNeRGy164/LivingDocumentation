using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class ClassMemberTests
    {
        [TestMethod]
        public void StringBuilderExtensions_ClassMember_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.ClassMember("member");

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_NullName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.ClassMember(null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_EmptyName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.ClassMember(string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_WhitespaceName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.ClassMember(" ");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_Should_ContainClassMemberLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassMember("member");

            // Assert
            stringBuilder.ToString().Should().Be("member\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_WithPublicVisibility_Should_ContainClassMemberLineWithPublicAnnotation()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassMember("member", visibility: VisibilityModifier.Public);

            // Assert
            stringBuilder.ToString().Should().Be("+member\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_WithPackagePrivateVisibility_Should_ContainClassMemberLineWithPackagePrivateAnnotation()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassMember("member", visibility: VisibilityModifier.PackagePrivate);

            // Assert
            stringBuilder.ToString().Should().Be("~member\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_WithProtectedVisibility_Should_ContainClassMemberLineWithProtectedAnnotation()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassMember("member", visibility: VisibilityModifier.Protected);

            // Assert
            stringBuilder.ToString().Should().Be("#member\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_WithPrivateVisibility_Should_ContainClassMemberLineWithPrivateAnnotation()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassMember("member", visibility: VisibilityModifier.Private);

            // Assert
            stringBuilder.ToString().Should().Be("-member\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_IsAbstract_Should_ContainClassMemberLineWithAbstractAnnotation()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassMember("member", isAbstract: true);

            // Assert
            stringBuilder.ToString().Should().Be("{abstract}member\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassMember_IsStatic_Should_ContainClassMemberLineWithStaticAnnotation()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassMember("member", isStatic: true);

            // Assert
            stringBuilder.ToString().Should().Be("{static}member\n");
        }
    }
}
