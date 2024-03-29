﻿using System;
using System.CodeDom;
using System.Runtime.InteropServices;

namespace Linguist.VisualStudio
{
    [ Guid ( "D2C9F9EB-4F27-4B56-8C83-F18BE5A2C991" ) ]
    public sealed class InternalLinguistCodeGenerator : BaseLinguistCodeGenerator
    {
        public const string Name        = nameof ( InternalLinguistCodeGenerator );
        public const string Description = "Generates an internal strongly-typed resource class for localization.";

        protected override MemberAttributes AccessModifier => MemberAttributes.Assembly | MemberAttributes.Static;
    }
}