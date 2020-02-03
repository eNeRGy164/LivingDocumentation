using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class NoteTests
    {
        [TestMethod]
        public void StringBuilderExtensions_Note_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.Note(NotePosition.Left, "Note");

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_Note_WithPosition_Should_ContainNoteLineWithPosition()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Note(NotePosition.Left, "Note");

            // Assert
            stringBuilder.ToString().Should().Be("note left : Note\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Note_WithNote_Should_ContainNoteLineWithNote()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Note(NotePosition.Left, "Note");

            // Assert
            stringBuilder.ToString().Should().Be("note left : Note\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Note_WithMultiLineNote_Should_ContainNoteLineWithMultilineNotes()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Note(NotePosition.Left, "Line1\nLine2");

            // Assert
            stringBuilder.ToString().Should().Be("note left : Line1\\nLine2\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Note_WithHexagonalStyle_Should_ContainNoteLineWithHexagonalStyle()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Note(NotePosition.Left, "Note", style: NoteStyle.Hexagonal);

            // Assert
            stringBuilder.ToString().Should().Be("hnote left : Note\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Note_WithBoxStyle_Should_ContainNoteLineWithBoxStyle()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Note(NotePosition.Left, "Note", style: NoteStyle.Box);

            // Assert
            stringBuilder.ToString().Should().Be("rnote left : Note\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Note_WithPositionRelatedToParticipant_Should_ContainNoteLineWithParticipant()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Note(NotePosition.Left, "Note", participant: "actorA");

            // Assert
            stringBuilder.ToString().Should().Be("note left of actorA : Note\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Note_PositionOverParticipant_Should_ContainNoteLineWithOver()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Note("actorA", "Note");

            // Assert
            stringBuilder.ToString().Should().Be("note over actorA : Note\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Note_AcrossParticipant_Should_ContainNoteLineWithBothParticipants()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Note("actorA", "actorB", "Note");

            // Assert
            stringBuilder.ToString().Should().Be("note over actorA,actorB : Note\n");
        }
    }
}
