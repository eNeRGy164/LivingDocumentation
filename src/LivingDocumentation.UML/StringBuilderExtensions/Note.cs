using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a note.
        /// </summary>
        /// <param name="position">The position of the note.</param>
        /// <param name="note">The text of the note.</param>
        /// <param name="participant">Optional participant. The position is relative to this participant.</param>
        /// <param name="style">The style of note. Default <see cref="NoteStyle.Normal"/>.</param>
        public static void Note(this StringBuilder stringBuilder, NotePosition position, string note, string participant = null, NoteStyle style = NoteStyle.Normal)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (style == NoteStyle.Hexagonal)
            {
                stringBuilder.Append(Constant.NoteHexagon);
            }
            else if (style == NoteStyle.Box)
            {
                stringBuilder.Append(Constant.NoteBox);
            }

            stringBuilder.Append(Constant.Note);
            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(position.ToString().ToLowerInvariant());

            if (!string.IsNullOrWhiteSpace(participant))
            {
                if (position != NotePosition.Over)
                {
                    stringBuilder.Append(Constant.Space);
                    stringBuilder.Append(Constant.Of);
                }

                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(participant);
            }

            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(Constant.Colon);
            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(note.Replace("\n", "\\n"));
            stringBuilder.AppendNewLine();
        }

        /// <summary>
        /// Renders a note over a participant.
        /// </summary>
        /// <param name="participant">The participant the note is positioned over.</param>
        /// <param name="note">The text of the note.</param>
        /// <param name="style">The style of note. Default <see cref="NoteStyle.Normal"/>.</param>
        public static void Note(this StringBuilder stringBuilder, string participant, string note, NoteStyle style = NoteStyle.Normal)
        {
            stringBuilder.Note(NotePosition.Over, note, participant: participant, style: style);
        }

        /// <summary>
        /// Renders a note over multiple participants.
        /// </summary>
        /// <param name="participantA">The first participant.</param>
        /// <param name="participantB">The second participant.</param>
        /// <param name="note">The text of the note.</param>
        /// <param name="style">The style of note. Default <see cref="NoteStyle.Normal"/>.</param>
        public static void Note(this StringBuilder stringBuilder, string participantA, string participantB, string note, NoteStyle style = NoteStyle.Normal)
        {
            stringBuilder.Note(NotePosition.Over, note, participant: participantA + Constant.Comma + participantB, style: style);
        }
    }
}
