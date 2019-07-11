using System;
using System.Collections.Generic;

namespace Localizer
{
    /// <summary>Represents a parsed format string.</summary>
    public sealed class FormatString
    {
        /// <summary>Initializes a new preparsed instance of the <see cref="T:Localizer.FormatString" /> class with the argument holes specified as a <see cref="T:Localizer.FormatString.ArgumentHole" /> array.</summary>
        /// <param name="format">The preparsed format string.</param>
        /// <param name="address">The argument holes array value of the preparsed format string.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="format" /> or <paramref name="argumentHoles" /> is <see langword="null" />.</exception>
        public FormatString ( string format, ArgumentHole [ ] argumentHoles )
        {
            Format        = format        ?? throw new ArgumentNullException ( nameof ( format        ) );
            ArgumentHoles = argumentHoles ?? throw new ArgumentNullException ( nameof ( argumentHoles ) );
        }

        /// <summary>Gets the format string.</summary>
        /// <returns>The format string.</returns>
        public string Format { get; }

        /// <summary>Gets the parsed argument holes for the current <see cref="T:Localizer.FormatString" /> object.</summary>
        /// <returns>The parsed argument holes.</returns>
        public ArgumentHole [ ] ArgumentHoles { get; }

        /// <summary>Returns the format string.</summary>
        /// <returns>The format string.</returns>
        public override string ToString ( ) => Format;

        /// <summary>Represents a parsed format string argument hole.</summary>
        public sealed class ArgumentHole
        {
            // NOTE: Undocumented exclusive limits on the range for Argument Hole Index and Argument Hole Alignment.
            internal const int IndexLimit     = 1000000; // NOTE:               0 <= Index     < IndexLimit
            internal const int AlignmentLimit = 1000000; // NOTE: -AlignmentLimit <  Alignment < AlignmentLimit

            /// <summary>
            /// Initializes a new instance of the <see cref="T:Localizer.FormatString.ArgumentHole" /> class
            /// with the specified index, alignment, formatting, start and end offsets.
            /// </summary>
            /// <param name="index">The argument hole index.</param>
            /// <param name="alignment">The optional alignment parameter.</param>
            /// <param name="format">The optional formatting parameter.</param>
            /// <param name="startOffset">The start offset in the format string.</param>
            /// <param name="endOffset">The end offset in the format string.</param>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            /// <paramref name="index" />, <paramref name="startOffset" />, or <paramref name="endOffset" /> is less than zero.
            /// </exception>
            public ArgumentHole ( int index, int alignment, string format, int startOffset, int endOffset )
            {
                if ( index       < 0 ) throw new ArgumentOutOfRangeException ( nameof ( index       ), index,       "Index must not be less than zero"        );
                if ( startOffset < 0 ) throw new ArgumentOutOfRangeException ( nameof ( startOffset ), startOffset, "Start offset must not be less than zero" );
                if ( endOffset   < 0 ) throw new ArgumentOutOfRangeException ( nameof ( endOffset   ), endOffset,   "End offset must not be less than zero"   );

                Index     = index;
                Alignment = alignment;
                Format    = format;
                Start     = startOffset;
                End       = endOffset;
            }

            /// <summary>Gets the argument hole index for the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object.</summary>
            /// <returns>The argument hole index.</returns>
            public int Index { get; }

            /// <summary>Gets the optional alignment parameter for the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object.</summary>
            /// <returns>The optional alignment parameter.</returns>
            public int Alignment { get; }

            /// <summary>Gets the optional formatting parameter for the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object.</summary>
            /// <returns>The optional formatting parameter.</returns>
            public string Format { get; }

            /// <summary>Gets the start offset in the format string for the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object.</summary>
            /// <returns>The start offset in the format string.</returns>
            public int Start { get; }

            /// <summary>Gets the end offset in the format string for the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object.</summary>
            /// <returns>The end offset in the format string.</returns>
            public int End { get; }

            /// <summary>Gets a value indicating whether the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object has a non-zero alignment.</summary>
            /// <returns><see langword="true" /> if the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object has a non-zero alignment; <see langword="false" /> otherwise.</returns>
            public bool HasAlignment => Alignment != 0;

            /// <summary>Gets a value indicating whether the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object has a format.</summary>
            /// <returns><see langword="true" /> if the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object has a format; <see langword="false" /> otherwise.</returns>
            public bool HasFormat => ! string.IsNullOrEmpty ( Format );

            /// <summary>Converts the value of the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object to its equivalent format string representation.</summary>
            /// <returns>The equivalent format string representation.</returns>
            public string ToFormatString ( ) => $"{{{ Index }{ ( HasAlignment ? "," + Alignment : null ) }{ ( HasFormat ? ":" + Format : null ) }}}";

            /// <summary>Converts the value of the current <see cref="T:Localizer.FormatString.ArgumentHole" /> object to its equivalent format string representation with the offset range appended, formatted as "@[start..end]".</summary>
            /// <returns>
            /// The equivalent format string representation with the offset range appended, formatted as "@[start..end]".
            /// e.g. "{0}@[0..2]", "{3,-34:N6}@[10..19]".
            /// </returns>
            public override string ToString ( ) => $"{ ToFormatString ( ) }@[{ Start }..{ End }]";
        }

        /// <summary>Determines whether a string is a valid format string.</summary>
        /// <param name="format">The string to validate.</param>
        /// <param name="formatString">The equivalent <see cref="T:Localizer.FormatString" /> instance of the format string.</param>
        /// <returns><see langword="true" /> if <paramref name="format" /> is a valid format string; otherwise, <see langword="false" />.</returns>
        public static bool TryParse ( string format, out FormatString formatString )
        {
            var argumentHoles = Parse ( format, false );

            if ( argumentHoles != null )
            {
                formatString = new FormatString ( format, argumentHoles );
                return true;
            }

            formatString = null;
            return false;
        }

        /// <summary>Converts a format string to an equivalent <see cref="T:Localizer.FormatString" /> instance.</summary>
        /// <param name="format">A format string to convert.</param>
        /// <returns>An equivalent <see cref="T:Localizer.FormatString" /> instance.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="format" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.FormatException"><paramref name="format" /> is not a valid format string.</exception>
        public static FormatString Parse ( string format )
        {
            return new FormatString ( format, Parse ( format, true ) );
        }

        private static ArgumentHole [ ] Parse ( string format, bool throwOnError )
        {
            if ( format == null )
                return throwOnError ? ArgumentNull ( nameof ( format ) ) : null;

            var arguments = new List < ArgumentHole > ( );
            var cursor    = 0;
            var length    = format.Length;
            var character = '\x0';

            while ( true )
            {
                while ( cursor < length )
                {
                    character = format [ cursor++ ];

                    // Is it a closing brace?
                    if ( character == '}' )
                    {
                        // Check next character (if there is one) to see if it is escaped. eg }}
                        if ( cursor < length && format [ cursor ] == '}' )
                        {
                            cursor++;
                        }
                        else
                        {
                            // Otherwise treat it as an error (Mismatched closing brace)
                            return throwOnError ? MismatchedClosingBrace ( cursor - 1 ) : null;
                        }
                    }

                    // Is it a opening brace?
                    if ( character == '{' )
                    {
                        // Check next character (if there is one) to see if it is escaped. e.g. {{
                        if ( cursor < length && format [ cursor ] == '{' )
                        {
                            cursor++;
                        }
                        else
                        {
                            // Otherwise treat it as the opening brace of an Argument Hole.
                            cursor--;
                            break;
                        }
                    }

                    // If it neither then the character is just text.
                }

                if ( cursor == length )
                    break;

                // Parsing argument hole
                //
                // Argument Hole ::= { Index (, WS* Alignment WS*)? (: Formatting)? }

                var argument   = cursor;
                var index      = 0;
                var alignment  = 0;
                var formatting = (string) null;

                #region Parsing index parameter => Index ::= ('0'-'9')+ WS*

                cursor++;

                // If reached end of text then error (Unexpected end of text)
                if ( cursor == length )
                    return throwOnError ? MissingClosingBrace ( argument ) : null;

                // or character is not a digit then error (Unexpected Character)
                if ( ( character = format [ cursor ] ) < '0' || character > '9' )
                    return throwOnError ? UnexpectedCharacter ( cursor, character ) : null;

                do
                {
                    index = index * 10 + character - '0';
                    cursor++;

                    // If reached end of text then error (Unexpected end of text)
                    if ( cursor == length )
                        return throwOnError ? MissingClosingBrace ( argument ) : null;

                    character = format [ cursor ];
                    // so long as character is digit and value of the index is less than 1000000 ( index limit )
                }
                while ( character >= '0' && character <= '9' && index < ArgumentHole.IndexLimit );

                // If value is not less than 1000000 ( index limit ) then error
                if ( index >= ArgumentHole.IndexLimit )
                    return throwOnError ? InvalidArgumentIndex ( argument ) : null;

                // Consume optional whitespace.
                while ( cursor < length && ( character = format [ cursor ] ) == ' ' ) cursor++;

                #endregion End parsing index parameter

                #region Parsing optional alignment => Alignment ::= comma WS* minus? ('0'-'9')+ WS*

                // Is the character a comma, which indicates the start of alignment parameter.
                if ( character == ',' )
                {
                    cursor++;

                    // Consume Optional whitespace
                    while ( cursor < length && format [ cursor ] == ' ' ) cursor++;

                    // If reached the end of the text then error (Unexpected end of text)
                    if ( cursor == length )
                        return throwOnError ? MissingClosingBrace ( argument ) : null;

                    var start = cursor;
                    var left  = false;

                    // Is there a minus sign?
                    character = format [ cursor ];
                    if ( character == '-' )
                    {
                        // Alignment is left justified.
                        left = true;

                        cursor++;

                        // If reached end of text then error (Unexpected end of text)
                        if ( cursor == length )
                            return throwOnError ? MissingClosingBrace ( argument ) : null;

                        character = format [ cursor ];
                    }

                    // If current character is not a digit then error (Unexpected character)
                    if ( character < '0' || character > '9' )
                        return throwOnError ? UnexpectedCharacter ( cursor, character ) : null;

                    // Parse alignment digits.
                    do
                    {
                        alignment = alignment * 10 + character - '0';
                        cursor++;

                        // If reached end of text then error. (Unexpected end of text)
                        if ( cursor == length )
                            return throwOnError ? MissingClosingBrace ( argument ) : null;

                        character = format [ cursor ];

                        // So long a current character is a digit and the value of alignment is less than 1000000 ( alignment limit )
                    }
                    while ( character >= '0' && character <= '9' && alignment < ArgumentHole.AlignmentLimit );

                    // If value is not less than 1000000 ( alignment limit ) then error
                    if ( alignment >= ArgumentHole.AlignmentLimit )
                        return throwOnError ? InvalidAlignmentWidth ( start ) : null;

                    if ( left )
                        alignment = -alignment;
                }

                // Consume optional whitespace
                while ( cursor < length && ( character = format [ cursor ] ) == ' ' ) cursor++;

                #endregion End parsing optional alignment

                #region Parsing optional formatting parameter => Formatting ::= colon *

                // Is current character a colon? which indicates start of formatting parameter.
                if ( character == ':' )
                {
                    cursor++;

                    var start = cursor;

                    while ( true )
                    {
                        // If reached end of text then error. (Unexpected end of text)
                        if ( cursor == length )
                            return throwOnError ? MissingClosingBrace ( argument ) : null;

                        character = format [ cursor ];

                        if ( character == '}' )
                        {
                            // Argument hole closed
                            break;
                        }
                        else if ( character == '{' )
                        {
                            // Braces inside the argument hole are not supported
                            return throwOnError ? BraceInsideArgument ( cursor ) : null;
                        }

                        cursor++;
                    }

                    if ( cursor > start )
                        formatting = format.Substring ( start, cursor - start );
                }
                else if ( character != '}' )
                {
                    // Unexpected character
                    return throwOnError ? UnexpectedCharacter ( cursor, character ) : null;
                }

                #endregion End parsing optional formatting parameter

                arguments.Add ( new ArgumentHole ( index, alignment, formatting, argument, cursor ) );

                cursor++;

                // Continue to parse other characters.
            }

            return arguments.ToArray ( );
        }

        private const string Format_UnexpectedCharacter    = "Unexpected character '{1}' encountered at position '{0}'.";
        private const string Format_MismatchedClosingBrace = "Mismatched closing brace at position '{0}'.";
        private const string Format_MissingClosingBrace    = "Missing closing brace at position '{0}'.";
        private const string Format_BraceInsideArgument    = "Invalid opening brace '{{' inside argument at position '{0}'.";
        private const string Format_InvalidArgumentIndex   = "Invalid argument index at position '{0}'.";
        private const string Format_InvalidAlignmentWidth  = "Invalid alignment width at position '{0}'.";

        private static ArgumentHole [ ] ArgumentNull           ( string name )                  => throw new ArgumentNullException ( name );
        private static ArgumentHole [ ] UnexpectedCharacter    ( int position, char character ) => throw new FormatException ( string.Format ( Format_UnexpectedCharacter,    position, character ) );
        private static ArgumentHole [ ] MismatchedClosingBrace ( int position )                 => throw new FormatException ( string.Format ( Format_MismatchedClosingBrace, position ) );
        private static ArgumentHole [ ] MissingClosingBrace    ( int position )                 => throw new FormatException ( string.Format ( Format_MissingClosingBrace,    position ) );
        private static ArgumentHole [ ] BraceInsideArgument    ( int position )                 => throw new FormatException ( string.Format ( Format_BraceInsideArgument,    position ) );
        private static ArgumentHole [ ] InvalidArgumentIndex   ( int position )                 => throw new FormatException ( string.Format ( Format_InvalidArgumentIndex,   position ) );
        private static ArgumentHole [ ] InvalidAlignmentWidth  ( int position )                 => throw new FormatException ( string.Format ( Format_InvalidAlignmentWidth,  position ) );
    }
}