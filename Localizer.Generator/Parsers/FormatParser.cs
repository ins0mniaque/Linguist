using System;

namespace Localizer.Generator
{
    public static class FormatParser
    {
        // Undocumented exclusive limits on the range for Argument Hole Index and Argument Hole Alignment.
        private const int IndexLimit = 1000000; // Note:            0 <= ArgIndex < IndexLimit
        private const int WidthLimit = 1000000; // Note:  -WidthLimit <  ArgAlign < WidthLimit

        public static int Parse ( string format )
        {
            if ( format == null )
                return 0;

            var cursor    = 0;
            var length    = format.Length;
            var character = '\x0';
            var maxIndex  = -1;

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
                            MismatchedClosingBrace ( cursor );
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

                var argument = cursor;

                #region Parsing index parameter => Index ::= ('0'-'9')+ WS*

                cursor++;

                // If reached end of text then error (Unexpected end of text)
                if ( cursor == length )
                    MissingClosingBrace ( argument );

                // or character is not a digit then error (Unexpected Character)
                if ( ( character = format [ cursor ] ) < '0' || character > '9' )
                    UnexpectedCharacter ( cursor, character );

                int index = 0;
                do
                {
                    index = index * 10 + character - '0';
                    cursor++;

                    // If reached end of text then error (Unexpected end of text)
                    if ( cursor == length )
                        MissingClosingBrace ( argument );

                    character = format [ cursor ];
                    // so long as character is digit and value of the index is less than 1000000 ( index limit )
                }
                while ( character >= '0' && character <= '9' && index < IndexLimit );

                // If value is not less than 1000000 ( index limit ) then error
                if ( index >= IndexLimit )
                    InvalidArgumentIndex ( argument );

                if ( index > maxIndex )
                    maxIndex = index;

                // Consume optional whitespace.
                while ( cursor < length && ( character = format [ cursor ] ) == ' ' ) cursor++;

                #endregion End parsing index parameter

                #region Parsing optional alignment => Alignment ::= comma WS* minus? ('0'-'9')+ WS*

                var width = 0;

                // Is the character a comma, which indicates the start of alignment parameter.
                if ( character == ',' )
                {
                    cursor++;

                    // Consume Optional whitespace
                    while ( cursor < length && format [ cursor ] == ' ' ) cursor++;

                    // If reached the end of the text then error (Unexpected end of text)
                    if ( cursor == length )
                        MissingClosingBrace ( argument );

                    // Is there a minus sign?
                    character = format [ cursor ];
                    if ( character == '-' )
                    {
                        // Alignment is left justified.

                        cursor++;

                        // If reached end of text then error (Unexpected end of text)
                        if ( cursor == length )
                            MissingClosingBrace ( argument );

                        character = format [ cursor ];
                    }

                    // If current character is not a digit then error (Unexpected character)
                    if ( character < '0' || character > '9' )
                        UnexpectedCharacter ( cursor, character );

                    // Parse alignment digits.
                    do
                    {
                        width = width * 10 + character - '0';
                        cursor++;

                        // If reached end of text then error. (Unexpected end of text)
                        if ( cursor == length )
                            MissingClosingBrace ( argument );

                        character = format [ cursor ];

                        // So long a current character is a digit and the value of width is less than 1000000 ( width limit )
                    }
                    while ( character >= '0' && character <= '9' && width < WidthLimit );
                }

                // Consume optional whitespace
                while ( cursor < length && ( character = format [ cursor ] ) == ' ' ) cursor++;

                #endregion End parsing optional alignment

                #region Parsing optional formatting parameter => Formatting ::= colon *

                // Is current character a colon? which indicates start of formatting parameter.
                if ( character == ':' )
                {
                    cursor++;

                    while ( true )
                    {
                        // If reached end of text then error. (Unexpected end of text)
                        if ( cursor == length )
                            MissingClosingBrace ( argument );

                        character = format [ cursor ];

                        if ( character == '}' )
                        {
                            // Argument hole closed
                            break;
                        }
                        else if ( character == '{' )
                        {
                            // Braces inside the argument hole are not supported
                            BraceInsideArgument ( cursor );
                        }

                        cursor++;
                    }
                }
                else if ( character != '}' )
                {
                    // Unexpected character
                    UnexpectedCharacter ( cursor, character );
                }

                cursor++;

                #endregion End parsing optional formatting parameter

                // Continue to parse other characters.
            }

            return maxIndex + 1;
        }

        private const string Format_UnexpectedCharacter    = "Unexpected character '{1}' encountered at position '{0}'.";
        private const string Format_MismatchedClosingBrace = "Mismatched closing brace at position '{0}'.";
        private const string Format_MissingClosingBrace    = "Missing closing brace at position '{0}'.";
        private const string Format_BraceInsideArgument    = "Invalid opening brace '{{' inside argument at position '{0}'.";
        private const string Format_InvalidArgumentIndex   = "Invalid argument index at position '{0}'.";

        private static void UnexpectedCharacter    ( int position, char character ) => throw new FormatException ( string.Format ( Format_UnexpectedCharacter,    position, character ) );
        private static void MismatchedClosingBrace ( int position )                 => throw new FormatException ( string.Format ( Format_MismatchedClosingBrace, position ) );
        private static void MissingClosingBrace    ( int position )                 => throw new FormatException ( string.Format ( Format_MissingClosingBrace,    position ) );
        private static void BraceInsideArgument    ( int position )                 => throw new FormatException ( string.Format ( Format_BraceInsideArgument,    position ) );
        private static void InvalidArgumentIndex   ( int position )                 => throw new FormatException ( string.Format ( Format_InvalidArgumentIndex,   position ) );
    }
}