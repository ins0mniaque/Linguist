using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

using Linguist.CodeDom;
using Linguist.Resources;
using Linguist.Resources.Naming;

namespace Linguist.Generator
{
    using static String;
    using static ErrorMessages;
    using static MemberNames;

    public class ResourceMapper
    {
        private readonly Dictionary < IResource, CompilerError > errors = new Dictionary < IResource, CompilerError > ( );

        public ResourceMapper ( CodeDomProvider codeDomProvider, IResourceNamingStrategy resourceNamingStrategy = null )
        {
            CodeDomProvider        = codeDomProvider;
            ResourceNamingStrategy = resourceNamingStrategy;
        }

        public CodeDomProvider         CodeDomProvider        { get; }
        public IResourceNamingStrategy ResourceNamingStrategy { get; }

        public IList < ResourceMapping > Map ( CodeTypeDeclaration @class, IList < IResource > resourceSet )
        {
            var members = new HashSet < string > ( );
            foreach ( CodeTypeMember member in @class.Members )
                members.Add ( member.Name );

            var map = new SortedList < string, ResourceMapping > ( resourceSet.Count, StringComparer.InvariantCultureIgnoreCase );

            foreach ( var resource in resourceSet )
            {
                var name     = resource.Name;
                var property = name;

                if ( members.Contains ( name ) )
                {
                    AddError ( resource, Format ( PropertyAlreadyExists, name ) );
                    continue;
                }

                if ( typeof ( void ).FullName == resource.Type )
                {
                    AddError ( resource, Format ( InvalidPropertyType, resource.Type, name ) );
                    continue;
                }

                var isWinFormsLocalizableResource = name.Length > 0 && name [ 0 ] == '$' ||
                                                    name.Length > 1 && name [ 0 ] == '>' &&
                                                                       name [ 1 ] == '>';
                if ( isWinFormsLocalizableResource )
                {
                    AddWarning ( resource, Format ( SkippingWinFormsResource, name ) );
                    continue;
                }

                var isBaseName = ResourceNamingStrategy == null ||
                                 ResourceNamingStrategy.ParseArguments ( PluralRules.Invariant, name, out _ ) == name;
                if ( ! isBaseName )
                    continue;

                if ( ! CodeDomProvider.IsValidIdentifier ( property ) )
                {
                    property = CodeDomProvider.ValidateIdentifier ( property );

                    if ( property == null )
                    {
                        AddError ( resource, Format ( CannotCreateResourceProperty, name ) );
                        continue;
                    }
                }

                if ( map.TryGetValue ( property, out var duplicate ) )
                {
                    AddError ( duplicate.Resource, Format ( CannotCreateResourceProperty, duplicate.Resource.Name ) );
                    AddError ( resource,           Format ( CannotCreateResourceProperty, name ) );

                    map.Remove ( property );
                    continue;
                }

                map    .Add ( property, new ResourceMapping ( resource ) { Property = property } );
                members.Add ( property );
            }

            foreach ( var mapping in map.Values )
            {
                mapping.NumberOfArguments = GetNumberOfArguments ( mapping.Resource );
                if ( mapping.NumberOfArguments <= 0 )
                    continue;

                var methodName = mapping.Property + FormatMethodSuffix;
                var isUnique   = ! members.Contains ( methodName );
                if ( isUnique && CodeDomProvider.IsValidIdentifier ( methodName ) )
                {
                    mapping.FormatMethod = methodName;
                    members.Add ( methodName );
                }
                else
                    AddError ( mapping.Resource, Format ( CannotCreateFormatMethod, methodName, mapping.Resource.Name ) );
            }

            return map.Values.ToList ( );
        }

        public CompilerError [ ] GetErrors ( ) => errors.Values.ToArray ( );

        private int GetNumberOfArguments ( IResource resource )
        {
            if ( resource.Type != typeof ( string ).FullName )
                return 0;

            try
            {
                var formatString      = FormatString.Parse ( (string) resource.Value );
                var numberOfArguments = formatString.ArgumentHoles
                                                    .Select ( argumentHole => argumentHole.Index )
                                                    .DefaultIfEmpty ( -1 )
                                                    .Max ( ) + 1;
                return numberOfArguments;
            }
            catch ( FormatException exception )
            {
                AddError ( resource, Format ( ErrorInStringResourceFormat, exception.Message, resource.Name ) );
            }

            return 0;
        }

        private void AddError   ( IResource resource, string error   ) => GenerateError ( resource, null, error,   false );
        private void AddWarning ( IResource resource, string warning ) => GenerateError ( resource, null, warning, true  );

        private void GenerateError ( IResource resource, string number, string message, bool isWarning )
        {
            if ( errors.TryGetValue ( resource, out var error ) )
            {
                error.ErrorNumber = error.ErrorNumber ?? number;

                if ( ! error.ErrorText.Contains ( message ) )
                    error.ErrorText += "\n" + message;

                if ( ! isWarning )
                    error.IsWarning = false;
            }
            else
                errors.Add ( resource, new CompilerError ( ) { ErrorNumber = number,
                                                               ErrorText   = message,
                                                               IsWarning   = isWarning,
                                                               FileName    = resource.Source,
                                                               Line        = resource.Line   ?? 0,
                                                               Column      = resource.Column ?? 0 } );
        }
    }
}