using System;
using System.Globalization;

namespace Localizer.Resources
{
    public abstract class ResourceNamingStrategy : IResourceNamingStrategy
    {
        public static readonly IResourceNamingStrategy Default = new SuffixedResourceNamingStrategy ( );

        protected readonly StringComparison comparison;

        protected readonly string zero;
        protected readonly string one;
        protected readonly string two;
        protected readonly string few;
        protected readonly string many;
        protected readonly string other;
        protected readonly string explicitZero;
        protected readonly string explicitOne;
        protected readonly string discard;
        protected readonly string ordinalZero;
        protected readonly string ordinalOne;
        protected readonly string ordinalTwo;
        protected readonly string ordinalFew;
        protected readonly string ordinalMany;
        protected readonly string ordinalOther;
        protected readonly string range;

        protected ResourceNamingStrategy ( StringComparison comparisonType,
                                           string zero,
                                           string one,
                                           string two,
                                           string few,
                                           string many,
                                           string other,
                                           string explicitZero,
                                           string explicitOne,
                                           string discard,
                                           string ordinalZero,
                                           string ordinalOne,
                                           string ordinalTwo,
                                           string ordinalFew,
                                           string ordinalMany,
                                           string ordinalOther,
                                           string range )
        {
            comparison = comparisonType;

            this.zero         = zero;
            this.one          = one;
            this.two          = two;
            this.few          = few;
            this.many         = many;
            this.other        = other;
            this.explicitZero = explicitZero;
            this.explicitOne  = explicitOne;
            this.discard      = discard;
            this.ordinalZero  = ordinalZero;
            this.ordinalOne   = ordinalOne;
            this.ordinalTwo   = ordinalTwo;
            this.ordinalFew   = ordinalFew;
            this.ordinalMany  = ordinalMany;
            this.ordinalOther = ordinalOther;
            this.range        = range;
        }

        protected ResourceNamingStrategy ( string pluralFormFormat,
                                           string ordinalFormFormat,
                                           string rangeForm,
                                           string discardKeyword      = null,
                                           string ordinalOtherKeyword = null,
                                           StringComparison comparisonType = StringComparison.OrdinalIgnoreCase )
        {
            comparison = comparisonType;

            zero         = string.Format ( CultureInfo.InvariantCulture, pluralFormFormat, Keywords.Zero  );
            one          = string.Format ( CultureInfo.InvariantCulture, pluralFormFormat, Keywords.One   );
            two          = string.Format ( CultureInfo.InvariantCulture, pluralFormFormat, Keywords.Two   );
            few          = string.Format ( CultureInfo.InvariantCulture, pluralFormFormat, Keywords.Few   );
            many         = string.Format ( CultureInfo.InvariantCulture, pluralFormFormat, Keywords.Many  );
            other        = string.Format ( CultureInfo.InvariantCulture, pluralFormFormat, Keywords.Other );
            explicitZero = string.Format ( CultureInfo.InvariantCulture, pluralFormFormat, Keywords.ExplicitZero );
            explicitOne  = string.Format ( CultureInfo.InvariantCulture, pluralFormFormat, Keywords.ExplicitOne  );
            discard      = string.Format ( CultureInfo.InvariantCulture, pluralFormFormat, discardKeyword ?? Keywords.Discard );

            ordinalZero  = string.Format ( CultureInfo.InvariantCulture, ordinalFormFormat, Keywords.Zero  );
            ordinalOne   = string.Format ( CultureInfo.InvariantCulture, ordinalFormFormat, Keywords.One   );
            ordinalTwo   = string.Format ( CultureInfo.InvariantCulture, ordinalFormFormat, Keywords.Two   );
            ordinalFew   = string.Format ( CultureInfo.InvariantCulture, ordinalFormFormat, Keywords.Few   );
            ordinalMany  = string.Format ( CultureInfo.InvariantCulture, ordinalFormFormat, Keywords.Many  );
            ordinalOther = string.Format ( CultureInfo.InvariantCulture, ordinalFormFormat, ordinalOtherKeyword ?? Keywords.Ordinal );

            range = rangeForm;
        }

        public abstract string ParseArguments ( PluralRules pluralRules, string resourceName, out FormatString.Argument [ ] arguments );

        protected abstract bool   Match ( string name, string form, StringComparison comparisonType );
        protected abstract string Trim  ( string name, string match );

        protected bool TryExtractPluralForm ( ref string name, out PluralForm pluralForm, out NumberForm numberForm )
        {
            pluralForm = PluralForm.Other;
            numberForm = NumberForm.Cardinal;

            var match = (string) null;

            if      ( Match ( name, zero,         comparison ) ) { match = zero;    pluralForm = PluralForm.Zero;         }
            else if ( Match ( name, one,          comparison ) ) { match = one;     pluralForm = PluralForm.One;          }
            else if ( Match ( name, two,          comparison ) ) { match = two;     pluralForm = PluralForm.Two;          }
            else if ( Match ( name, few,          comparison ) ) { match = few;     pluralForm = PluralForm.Few;          }
            else if ( Match ( name, many,         comparison ) ) { match = many;    pluralForm = PluralForm.Many;         }
            else if ( Match ( name, other,        comparison ) ) { match = other;   pluralForm = PluralForm.Other;        }
            else if ( Match ( name, explicitZero, comparison ) ) { match = other;   pluralForm = PluralForm.ExplicitZero; }
            else if ( Match ( name, explicitOne,  comparison ) ) { match = other;   pluralForm = PluralForm.ExplicitOne;  }
            else if ( Match ( name, discard,      comparison ) ) { match = discard; pluralForm = PluralForm.Other;        }
            else
            {
                if      ( Match ( name, ordinalZero,  comparison ) ) { match = ordinalZero;  pluralForm = PluralForm.Zero;  }
                else if ( Match ( name, ordinalOne,   comparison ) ) { match = ordinalOne;   pluralForm = PluralForm.One;   }
                else if ( Match ( name, ordinalTwo,   comparison ) ) { match = ordinalTwo;   pluralForm = PluralForm.Two;   }
                else if ( Match ( name, ordinalFew,   comparison ) ) { match = ordinalFew;   pluralForm = PluralForm.Few;   }
                else if ( Match ( name, ordinalMany,  comparison ) ) { match = ordinalMany;  pluralForm = PluralForm.Many;  }
                else if ( Match ( name, ordinalOther, comparison ) ) { match = ordinalOther; pluralForm = PluralForm.Other; }

                if ( match != null )
                    numberForm = NumberForm.Ordinal;
            }

            if ( match == null )
                return false;

            name = Trim ( name, match );

            return true;
        }

        protected bool TryExtractPluralRangeForm ( ref string name )
        {
            var match = (string) null;

            if ( Match ( name, range, comparison ) ) match = range;

            if ( match == null )
                return false;

            name = Trim ( name, match );

            return true;
        }
    }
}