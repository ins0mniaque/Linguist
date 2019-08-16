using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;

namespace Linguist.CodeDom.Fluent
{
    /// <summary>
    /// Fluent comment builder to deal with CodeDom's verbosity
    /// </summary>
    public static class Comment
    {
        public static int SingleLineThreshold { get; set; } = 80;
        public static int MaxLineLength       { get; set; } = 100;

        public static T AddComment < T > ( this T codeTypeMember, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = args == null || args.Length > 0 ? string.Format ( format, args ) : format;
            foreach ( var line in comment.WordWrap ( MaxLineLength ) )
                codeTypeMember.Comments.Add ( new CodeCommentStatement ( line ) );

            return codeTypeMember;
        }

        public static T AddSummary < T > ( this T codeTypeMember, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = Format ( format, args );

            return codeTypeMember.AddTagComments ( "summary", comment, codeTypeMember is CodeMemberField && comment.Length < SingleLineThreshold );
        }

        public static T AddRemarks < T > ( this T codeTypeMember, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = Format ( format, args );

            return codeTypeMember.AddTagComments ( "remarks", comment, comment.Length < SingleLineThreshold );
        }

        public static T AddParameterComment < T > ( this T codeTypeMember, string parameter, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = Format ( format, args );

            return codeTypeMember.AddTagComments ( $"param name=\"{ parameter }\"", comment, comment.Length < SingleLineThreshold );
        }

        public static T AddReturnComment < T > ( this T codeTypeMember, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = Format ( format, args );

            return codeTypeMember.AddTagComments ( "returns", comment, comment.Length < SingleLineThreshold );
        }

        private static T AddTagComments < T > ( this T codeTypeMember, string tag, string comment, bool singleLine ) where T : CodeTypeMember
        {
            var endTag = tag.Split ( ' ' ) [ 0 ];

            if ( ! singleLine )
            {
                codeTypeMember.Comments.Add ( new CodeCommentStatement ( $"<{ tag }>", true ) );

                foreach ( var line in comment.WordWrap ( MaxLineLength ) )
                    codeTypeMember.Comments.Add ( new CodeCommentStatement ( line, true ) );

                codeTypeMember.Comments.Add ( new CodeCommentStatement ( $"</{ endTag }>", true ) );
            }
            else
                codeTypeMember.Comments.Add ( new CodeCommentStatement ( $"<{ tag }>{ comment }</{ endTag }>", true ) );

            return codeTypeMember;
        }

        private static readonly char [ ] lineBreaks = new [ ] { '\n', '\r' };
        private static readonly char [ ] wordBreaks = new [ ] { ' ', ',', '.', '?', '!', ':', ';', '-', '\n', '\r', '\t' };

        private static string Format ( string format, params object [ ] args )
        {
            return args == null || args.Length > 0 ? string.Format ( CultureInfo.InvariantCulture, format, args ) : format;
        }

        private static IEnumerable < string > WordWrap ( this string text, int maxLineLength )
        {
            var lineBreak     = 0;
            var lastLineBreak = 0;

            do
            {
                var cut = lastLineBreak + maxLineLength;

                lineBreak = text.IndexOfAny ( lineBreaks, lastLineBreak ) + 1;

                if ( lineBreak == 0 || lineBreak > cut )
                {
                    if ( cut < text.Length )
                    {
                        lineBreak = text.LastIndexOfAny ( wordBreaks, cut ) + 1;
                        if ( lineBreak <= lastLineBreak )
                        {
                            lineBreak = text.IndexOfAny ( wordBreaks, cut ) + 1;
                            if ( lineBreak == 0 )
                                lineBreak = text.Length;
                        }
                    }
                    else
                        lineBreak = text.Length;
                }

                yield return text.Substring ( lastLineBreak, lineBreak - lastLineBreak ).Trim ( );

                lastLineBreak = lineBreak;
            }
            while ( lineBreak < text.Length );
        }
    }
}