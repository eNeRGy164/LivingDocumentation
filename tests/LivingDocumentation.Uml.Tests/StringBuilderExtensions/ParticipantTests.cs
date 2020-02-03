using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class ParticipantTests
    {
        [TestMethod]
        public void StringBuilderExtensions_Participant_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.Participant("actorA");

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_Participant_NullName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Participant(null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_Participant_EmptyName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Participant(string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_Participant_WhitespaceName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Participant(" ");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_Participant_Should_ContainParticipantLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Participant("actorA");

            // Assert
            stringBuilder.ToString().Should().Be("participant actorA\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Participant_WithDisplayName_Should_ContainParticipantLineWithDisplayName()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Participant("actorA", displayName: "Actor A");

            // Assert
            stringBuilder.ToString().Should().Be("participant \"Actor A\" as actorA\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Participant_WithShape_Should_ContainParticipantLineWithShape()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Participant("actorA", type: ParticipantType.Actor);

            // Assert
            stringBuilder.ToString().Should().Be("actor actorA\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Participant_WithColor_Should_ContainParticipantLineWithColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Participant("actorA", color: "AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("participant actorA #AliceBlue\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Participant_WithColorWithHashtag_Should_ContainParticipantLineWithColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Participant("actorA", color: "#AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("participant actorA #AliceBlue\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Participant_WithOrder_Should_ContainParticipantLineWithOrder()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Participant("actorA", order: 10);

            // Assert
            stringBuilder.ToString().Should().Be("participant actorA order 10\n");
        }
    }
}
