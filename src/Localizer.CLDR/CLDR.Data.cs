using System.Globalization;

using Localizer.Pluralization;

namespace Localizer.CLDR
{
    internal static class CardinalRuleSet
    {
        public static PluralFormSelector For ( CultureInfo culture )
        {
            switch ( culture.Name )
            {
                case "pt-PT"  : return CardinalRuleSetOneD.Instance;
            }

            switch ( culture.TwoLetterISOLanguageName )
            {
                case "am"  : return CardinalRuleSetOneA.Instance;
                case "as"  : return CardinalRuleSetOneA.Instance;
                case "bn"  : return CardinalRuleSetOneA.Instance;
                case "fa"  : return CardinalRuleSetOneA.Instance;
                case "gu"  : return CardinalRuleSetOneA.Instance;
                case "hi"  : return CardinalRuleSetOneA.Instance;
                case "kn"  : return CardinalRuleSetOneA.Instance;
                case "zu"  : return CardinalRuleSetOneA.Instance;
                case "ff"  : return CardinalRuleSetOneB.Instance;
                case "fr"  : return CardinalRuleSetOneB.Instance;
                case "hy"  : return CardinalRuleSetOneB.Instance;
                case "kab" : return CardinalRuleSetOneB.Instance;
                case "pt"  : return CardinalRuleSetOneC.Instance;
                case "ast" : return CardinalRuleSetOneD.Instance;
                case "ca"  : return CardinalRuleSetOneD.Instance;
                case "de"  : return CardinalRuleSetOneD.Instance;
                case "en"  : return CardinalRuleSetOneD.Instance;
                case "et"  : return CardinalRuleSetOneD.Instance;
                case "fi"  : return CardinalRuleSetOneD.Instance;
                case "fy"  : return CardinalRuleSetOneD.Instance;
                case "gl"  : return CardinalRuleSetOneD.Instance;
                case "ia"  : return CardinalRuleSetOneD.Instance;
                case "io"  : return CardinalRuleSetOneD.Instance;
                case "it"  : return CardinalRuleSetOneD.Instance;
                case "ji"  : return CardinalRuleSetOneD.Instance;
                case "nl"  : return CardinalRuleSetOneD.Instance;
                case "sc"  : return CardinalRuleSetOneD.Instance;
                case "scn" : return CardinalRuleSetOneD.Instance;
                case "sv"  : return CardinalRuleSetOneD.Instance;
                case "sw"  : return CardinalRuleSetOneD.Instance;
                case "ur"  : return CardinalRuleSetOneD.Instance;
                case "yi"  : return CardinalRuleSetOneD.Instance;
                case "si"  : return CardinalRuleSetOneE.Instance;
                case "ak"  : return CardinalRuleSetOneF.Instance;
                case "bh"  : return CardinalRuleSetOneF.Instance;
                case "guw" : return CardinalRuleSetOneF.Instance;
                case "ln"  : return CardinalRuleSetOneF.Instance;
                case "mg"  : return CardinalRuleSetOneF.Instance;
                case "nso" : return CardinalRuleSetOneF.Instance;
                case "pa"  : return CardinalRuleSetOneF.Instance;
                case "ti"  : return CardinalRuleSetOneF.Instance;
                case "wa"  : return CardinalRuleSetOneF.Instance;
                case "tzm" : return CardinalRuleSetOneG.Instance;
                case "af"  : return CardinalRuleSetOneH.Instance;
                case "asa" : return CardinalRuleSetOneH.Instance;
                case "az"  : return CardinalRuleSetOneH.Instance;
                case "bem" : return CardinalRuleSetOneH.Instance;
                case "bez" : return CardinalRuleSetOneH.Instance;
                case "bg"  : return CardinalRuleSetOneH.Instance;
                case "brx" : return CardinalRuleSetOneH.Instance;
                case "ce"  : return CardinalRuleSetOneH.Instance;
                case "cgg" : return CardinalRuleSetOneH.Instance;
                case "chr" : return CardinalRuleSetOneH.Instance;
                case "ckb" : return CardinalRuleSetOneH.Instance;
                case "dv"  : return CardinalRuleSetOneH.Instance;
                case "ee"  : return CardinalRuleSetOneH.Instance;
                case "el"  : return CardinalRuleSetOneH.Instance;
                case "eo"  : return CardinalRuleSetOneH.Instance;
                case "es"  : return CardinalRuleSetOneH.Instance;
                case "eu"  : return CardinalRuleSetOneH.Instance;
                case "fo"  : return CardinalRuleSetOneH.Instance;
                case "fur" : return CardinalRuleSetOneH.Instance;
                case "gsw" : return CardinalRuleSetOneH.Instance;
                case "ha"  : return CardinalRuleSetOneH.Instance;
                case "haw" : return CardinalRuleSetOneH.Instance;
                case "hu"  : return CardinalRuleSetOneH.Instance;
                case "jgo" : return CardinalRuleSetOneH.Instance;
                case "jmc" : return CardinalRuleSetOneH.Instance;
                case "ka"  : return CardinalRuleSetOneH.Instance;
                case "kaj" : return CardinalRuleSetOneH.Instance;
                case "kcg" : return CardinalRuleSetOneH.Instance;
                case "kk"  : return CardinalRuleSetOneH.Instance;
                case "kkj" : return CardinalRuleSetOneH.Instance;
                case "kl"  : return CardinalRuleSetOneH.Instance;
                case "ks"  : return CardinalRuleSetOneH.Instance;
                case "ksb" : return CardinalRuleSetOneH.Instance;
                case "ku"  : return CardinalRuleSetOneH.Instance;
                case "ky"  : return CardinalRuleSetOneH.Instance;
                case "lb"  : return CardinalRuleSetOneH.Instance;
                case "lg"  : return CardinalRuleSetOneH.Instance;
                case "mas" : return CardinalRuleSetOneH.Instance;
                case "mgo" : return CardinalRuleSetOneH.Instance;
                case "ml"  : return CardinalRuleSetOneH.Instance;
                case "mn"  : return CardinalRuleSetOneH.Instance;
                case "mr"  : return CardinalRuleSetOneH.Instance;
                case "nah" : return CardinalRuleSetOneH.Instance;
                case "nb"  : return CardinalRuleSetOneH.Instance;
                case "nd"  : return CardinalRuleSetOneH.Instance;
                case "ne"  : return CardinalRuleSetOneH.Instance;
                case "nn"  : return CardinalRuleSetOneH.Instance;
                case "nnh" : return CardinalRuleSetOneH.Instance;
                case "no"  : return CardinalRuleSetOneH.Instance;
                case "nr"  : return CardinalRuleSetOneH.Instance;
                case "ny"  : return CardinalRuleSetOneH.Instance;
                case "nyn" : return CardinalRuleSetOneH.Instance;
                case "om"  : return CardinalRuleSetOneH.Instance;
                case "or"  : return CardinalRuleSetOneH.Instance;
                case "os"  : return CardinalRuleSetOneH.Instance;
                case "pap" : return CardinalRuleSetOneH.Instance;
                case "ps"  : return CardinalRuleSetOneH.Instance;
                case "rm"  : return CardinalRuleSetOneH.Instance;
                case "rof" : return CardinalRuleSetOneH.Instance;
                case "rwk" : return CardinalRuleSetOneH.Instance;
                case "saq" : return CardinalRuleSetOneH.Instance;
                case "sd"  : return CardinalRuleSetOneH.Instance;
                case "sdh" : return CardinalRuleSetOneH.Instance;
                case "seh" : return CardinalRuleSetOneH.Instance;
                case "sn"  : return CardinalRuleSetOneH.Instance;
                case "so"  : return CardinalRuleSetOneH.Instance;
                case "sq"  : return CardinalRuleSetOneH.Instance;
                case "ss"  : return CardinalRuleSetOneH.Instance;
                case "ssy" : return CardinalRuleSetOneH.Instance;
                case "st"  : return CardinalRuleSetOneH.Instance;
                case "syr" : return CardinalRuleSetOneH.Instance;
                case "ta"  : return CardinalRuleSetOneH.Instance;
                case "te"  : return CardinalRuleSetOneH.Instance;
                case "teo" : return CardinalRuleSetOneH.Instance;
                case "tig" : return CardinalRuleSetOneH.Instance;
                case "tk"  : return CardinalRuleSetOneH.Instance;
                case "tn"  : return CardinalRuleSetOneH.Instance;
                case "tr"  : return CardinalRuleSetOneH.Instance;
                case "ts"  : return CardinalRuleSetOneH.Instance;
                case "ug"  : return CardinalRuleSetOneH.Instance;
                case "uz"  : return CardinalRuleSetOneH.Instance;
                case "ve"  : return CardinalRuleSetOneH.Instance;
                case "vo"  : return CardinalRuleSetOneH.Instance;
                case "vun" : return CardinalRuleSetOneH.Instance;
                case "wae" : return CardinalRuleSetOneH.Instance;
                case "xh"  : return CardinalRuleSetOneH.Instance;
                case "xog" : return CardinalRuleSetOneH.Instance;
                case "da"  : return CardinalRuleSetOneI.Instance;
                case "is"  : return CardinalRuleSetOneJ.Instance;
                case "mk"  : return CardinalRuleSetOneK.Instance;
                case "ceb" : return CardinalRuleSetOneL.Instance;
                case "fil" : return CardinalRuleSetOneL.Instance;
                case "tl"  : return CardinalRuleSetOneL.Instance;
                case "lv"  : return CardinalRuleSetZeroOneA.Instance;
                case "prg" : return CardinalRuleSetZeroOneA.Instance;
                case "lag" : return CardinalRuleSetZeroOneB.Instance;
                case "ksh" : return CardinalRuleSetZeroOneC.Instance;
                case "iu"  : return CardinalRuleSetOneTwoA.Instance;
                case "naq" : return CardinalRuleSetOneTwoA.Instance;
                case "se"  : return CardinalRuleSetOneTwoA.Instance;
                case "sma" : return CardinalRuleSetOneTwoA.Instance;
                case "smi" : return CardinalRuleSetOneTwoA.Instance;
                case "smj" : return CardinalRuleSetOneTwoA.Instance;
                case "smn" : return CardinalRuleSetOneTwoA.Instance;
                case "sms" : return CardinalRuleSetOneTwoA.Instance;
                case "shi" : return CardinalRuleSetOneFewA.Instance;
                case "mo"  : return CardinalRuleSetOneFewB.Instance;
                case "ro"  : return CardinalRuleSetOneFewB.Instance;
                case "bs"  : return CardinalRuleSetOneFewC.Instance;
                case "hr"  : return CardinalRuleSetOneFewC.Instance;
                case "sh"  : return CardinalRuleSetOneFewC.Instance;
                case "sr"  : return CardinalRuleSetOneFewC.Instance;
                case "gd"  : return CardinalRuleSetOneTwoFewA.Instance;
                case "sl"  : return CardinalRuleSetOneTwoFewB.Instance;
                case "dsb" : return CardinalRuleSetOneTwoFewC.Instance;
                case "hsb" : return CardinalRuleSetOneTwoFewC.Instance;
                case "he"  : return CardinalRuleSetOneTwoManyA.Instance;
                case "iw"  : return CardinalRuleSetOneTwoManyA.Instance;
                case "cs"  : return CardinalRuleSetOneFewManyA.Instance;
                case "sk"  : return CardinalRuleSetOneFewManyA.Instance;
                case "pl"  : return CardinalRuleSetOneFewManyB.Instance;
                case "be"  : return CardinalRuleSetOneFewManyC.Instance;
                case "lt"  : return CardinalRuleSetOneFewManyD.Instance;
                case "mt"  : return CardinalRuleSetOneFewManyE.Instance;
                case "ru"  : return CardinalRuleSetOneFewManyF.Instance;
                case "uk"  : return CardinalRuleSetOneFewManyF.Instance;
                case "br"  : return CardinalRuleSetOneTwoFewManyA.Instance;
                case "ga"  : return CardinalRuleSetOneTwoFewManyB.Instance;
                case "gv"  : return CardinalRuleSetOneTwoFewManyC.Instance;
                case "ar"  : return CardinalRuleSetZeroOneTwoFewManyA.Instance;
                case "ars" : return CardinalRuleSetZeroOneTwoFewManyA.Instance;
                case "cy"  : return CardinalRuleSetZeroOneTwoFewManyB.Instance;
                case "kw"  : return CardinalRuleSetZeroOneTwoFewManyC.Instance;
                default    : return DefaultCardinalRuleSet.Instance;
            }
        }
    }

    internal static class OrdinalRuleSet
    {
        public static PluralFormSelector For ( CultureInfo culture )
        {
            switch ( culture.TwoLetterISOLanguageName )
            {
                case "sv"  : return OrdinalRuleSetOneA.Instance;
                case "fil" : return OrdinalRuleSetOneB.Instance;
                case "fr"  : return OrdinalRuleSetOneB.Instance;
                case "ga"  : return OrdinalRuleSetOneB.Instance;
                case "hy"  : return OrdinalRuleSetOneB.Instance;
                case "lo"  : return OrdinalRuleSetOneB.Instance;
                case "mo"  : return OrdinalRuleSetOneB.Instance;
                case "ms"  : return OrdinalRuleSetOneB.Instance;
                case "ro"  : return OrdinalRuleSetOneB.Instance;
                case "tl"  : return OrdinalRuleSetOneB.Instance;
                case "vi"  : return OrdinalRuleSetOneB.Instance;
                case "hu"  : return OrdinalRuleSetOneC.Instance;
                case "ne"  : return OrdinalRuleSetOneD.Instance;
                case "be"  : return OrdinalRuleSetFewA.Instance;
                case "uk"  : return OrdinalRuleSetFewB.Instance;
                case "tk"  : return OrdinalRuleSetFewC.Instance;
                case "kk"  : return OrdinalRuleSetManyA.Instance;
                case "it"  : return OrdinalRuleSetManyB.Instance;
                case "sc"  : return OrdinalRuleSetManyB.Instance;
                case "scn" : return OrdinalRuleSetManyB.Instance;
                case "ka"  : return OrdinalRuleSetOneManyA.Instance;
                case "sq"  : return OrdinalRuleSetOneManyB.Instance;
                case "kw"  : return OrdinalRuleSetOneManyC.Instance;
                case "en"  : return OrdinalRuleSetOneTwoFewA.Instance;
                case "mr"  : return OrdinalRuleSetOneTwoFewB.Instance;
                case "gd"  : return OrdinalRuleSetOneTwoFewC.Instance;
                case "ca"  : return OrdinalRuleSetOneTwoFewD.Instance;
                case "mk"  : return OrdinalRuleSetOneTwoManyA.Instance;
                case "az"  : return OrdinalRuleSetOneFewManyA.Instance;
                case "gu"  : return OrdinalRuleSetOneTwoFewManyA.Instance;
                case "hi"  : return OrdinalRuleSetOneTwoFewManyA.Instance;
                case "as"  : return OrdinalRuleSetOneTwoFewManyB.Instance;
                case "bn"  : return OrdinalRuleSetOneTwoFewManyB.Instance;
                case "or"  : return OrdinalRuleSetOneTwoFewManyC.Instance;
                case "cy"  : return OrdinalRuleSetZeroOneTwoFewManyA.Instance;
                default    : return DefaultOrdinalRuleSet.Instance;
            }
        }
    }

    // Locales: bm, bo, dz, id, ig, ii, in, ja, jbo, jv, jw, kde, kea, km, ko, lkt, lo, ms, my, nqo, root, sah, ses, sg, th, to, vi, wo, yo, yue, zh
    internal class DefaultCardinalRuleSet : PluralFormSelector
    {
        public static readonly DefaultCardinalRuleSet Instance = new DefaultCardinalRuleSet ( );

        private DefaultCardinalRuleSet ( ) : base ( PluralForm.Other ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }
            // Explicit "1" rule "one" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.One ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.One;
            }

            return PluralForm.Other;
        }
    }

    // Locales: am, as, bn, fa, gu, hi, kn, zu
    internal class CardinalRuleSetOneA : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneA Instance = new CardinalRuleSetOneA ( );

        private CardinalRuleSetOneA ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": i = 0 or n = 1
            //   @integer: 0, 1
            //   @decimal: 0.0~1.0, 0.00~0.04
            if ( i == 0m || n == 1m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: ff, fr, hy, kab
    internal class CardinalRuleSetOneB : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneB Instance = new CardinalRuleSetOneB ( );

        private CardinalRuleSetOneB ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": i = 0,1
            //   @integer: 0, 1
            //   @decimal: 0.0~1.5
            if ( i.equals ( 0m, 1m ) )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: pt
    internal class CardinalRuleSetOneC : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneC Instance = new CardinalRuleSetOneC ( );

        private CardinalRuleSetOneC ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": i = 0..1
            //   @integer: 0, 1
            //   @decimal: 0.0~1.5
            if ( i.between ( 0m, 1m ) )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: ast, ca, de, en, et, fi, fy, gl, ia, io, it, ji, nl, pt-PT, sc, scn, sv, sw, ur, yi
    internal class CardinalRuleSetOneD : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneD Instance = new CardinalRuleSetOneD ( );

        private CardinalRuleSetOneD ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": i = 1 and v = 0
            //   @integer: 1
            if ( i == 1m && v == 0m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: si
    internal class CardinalRuleSetOneE : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneE Instance = new CardinalRuleSetOneE ( );

        private CardinalRuleSetOneE ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );
            var f = number.f ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n = 0,1 or i = 0 and f = 1
            //   @integer: 0, 1
            //   @decimal: 0.0, 0.1, 1.0, 0.00, 0.01, 1.00, 0.000, 0.001, 1.000, 0.0000, 0.0001, 1.0000
            if ( n.equals ( 0m, 1m ) || i == 0m && f == 1m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: ak, bh, guw, ln, mg, nso, pa, ti, wa
    internal class CardinalRuleSetOneF : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneF Instance = new CardinalRuleSetOneF ( );

        private CardinalRuleSetOneF ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n = 0..1
            //   @integer: 0, 1
            //   @decimal: 0.0, 1.0, 0.00, 1.00, 0.000, 1.000, 0.0000, 1.0000
            if ( n.between ( 0m, 1m ) )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: tzm
    internal class CardinalRuleSetOneG : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneG Instance = new CardinalRuleSetOneG ( );

        private CardinalRuleSetOneG ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n = 0..1 or n = 11..99
            //   @integer: 0, 1, 11~24
            //   @decimal: 0.0, 1.0, 11.0, 12.0, 13.0, 14.0, 15.0, 16.0, 17.0, 18.0, 19.0, 20.0, 21.0, 22.0, 23.0, 24.0
            if ( n.between ( 0m, 1m ) || n.between ( 11m, 99m ) )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: af, asa, az, bem, bez, bg, brx, ce, cgg, chr, ckb, dv, ee, el, eo, es, eu, fo, fur, gsw, ha, haw, hu, jgo, jmc, ka, kaj, kcg, kk, kkj, kl, ks, ksb, ku, ky, lb, lg, mas, mgo, ml, mn, mr, nah, nb, nd, ne, nn, nnh, no, nr, ny, nyn, om, or, os, pap, ps, rm, rof, rwk, saq, sd, sdh, seh, sn, so, sq, ss, ssy, st, syr, ta, te, teo, tig, tk, tn, tr, ts, ug, uz, ve, vo, vun, wae, xh, xog
    internal class CardinalRuleSetOneH : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneH Instance = new CardinalRuleSetOneH ( );

        private CardinalRuleSetOneH ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n = 1
            //   @integer: 1
            //   @decimal: 1.0, 1.00, 1.000, 1.0000
            if ( n == 1m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: da
    internal class CardinalRuleSetOneI : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneI Instance = new CardinalRuleSetOneI ( );

        private CardinalRuleSetOneI ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );
            var t = number.t ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n = 1 or t != 0 and i = 0,1
            //   @integer: 1
            //   @decimal: 0.1~1.6
            if ( n == 1m || t != 0m && i.equals ( 0m, 1m ) )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: is
    internal class CardinalRuleSetOneJ : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneJ Instance = new CardinalRuleSetOneJ ( );

        private CardinalRuleSetOneJ ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var t = number.t ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": t = 0 and i % 10 = 1 and i % 100 != 11 or t != 0
            //   @integer: 1, 21, 31, 41, 51, 61, 71, 81, 101, 1001, …
            //   @decimal: 0.1~1.6, 10.1, 100.1, 1000.1, …
            if ( t == 0m && i % 10m == 1m && i % 100m != 11m || t != 0m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: mk
    internal class CardinalRuleSetOneK : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneK Instance = new CardinalRuleSetOneK ( );

        private CardinalRuleSetOneK ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var f = number.f ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": v = 0 and i % 10 = 1 and i % 100 != 11 or f % 10 = 1 and f % 100 != 11
            //   @integer: 1, 21, 31, 41, 51, 61, 71, 81, 101, 1001, …
            //   @decimal: 0.1, 1.1, 2.1, 3.1, 4.1, 5.1, 6.1, 7.1, 10.1, 100.1, 1000.1, …
            if ( v == 0m && i % 10m == 1m && i % 100m != 11m || f % 10m == 1m && f % 100m != 11m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: ceb, fil, tl
    internal class CardinalRuleSetOneL : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneL Instance = new CardinalRuleSetOneL ( );

        private CardinalRuleSetOneL ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var f = number.f ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": v = 0 and i = 1,2,3 or v = 0 and i % 10 != 4,6,9 or v != 0 and f % 10 != 4,6,9
            //   @integer: 0~3, 5, 7, 8, 10~13, 15, 17, 18, 20, 21, 100, 1000, 10000, 100000, 1000000, …
            //   @decimal: 0.0~0.3, 0.5, 0.7, 0.8, 1.0~1.3, 1.5, 1.7, 1.8, 2.0, 2.1, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …
            if ( v == 0m && i.equals ( 1m, 2m, 3m ) || v == 0m && ! ( i % 10m ).equals ( 4m, 6m, 9m ) || v != 0m && ! ( f % 10m ).equals ( 4m, 6m, 9m ) )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: lv, prg
    internal class CardinalRuleSetZeroOneA : PluralFormSelector
    {
        public static readonly CardinalRuleSetZeroOneA Instance = new CardinalRuleSetZeroOneA ( );

        private CardinalRuleSetZeroOneA ( ) : base ( PluralForm.Zero | PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );
            var f = number.f ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "zero": n % 10 = 0 or n % 100 = 11..19 or v = 2 and f % 100 = 11..19
            //   @integer: 0, 10~20, 30, 40, 50, 60, 100, 1000, 10000, 100000, 1000000, …
            //   @decimal: 0.0, 10.0, 11.0, 12.0, 13.0, 14.0, 15.0, 16.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …
            if ( n % 10m == 0m || ( n % 100m ).between ( 11m, 19m ) || v == 2m && ( f % 100m ).between ( 11m, 19m ) )
                return PluralForm.Zero;

            // "one": n % 10 = 1 and n % 100 != 11 or v = 2 and f % 10 = 1 and f % 100 != 11 or v != 2 and f % 10 = 1
            //   @integer: 1, 21, 31, 41, 51, 61, 71, 81, 101, 1001, …
            //   @decimal: 0.1, 1.0, 1.1, 2.1, 3.1, 4.1, 5.1, 6.1, 7.1, 10.1, 100.1, 1000.1, …
            if ( n % 10m == 1m && n % 100m != 11m || v == 2m && f % 10m == 1m && f % 100m != 11m || v != 2m && f % 10m == 1m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: lag
    internal class CardinalRuleSetZeroOneB : PluralFormSelector
    {
        public static readonly CardinalRuleSetZeroOneB Instance = new CardinalRuleSetZeroOneB ( );

        private CardinalRuleSetZeroOneB ( ) : base ( PluralForm.Zero | PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "zero": n = 0
            //   @integer: 0
            //   @decimal: 0.0, 0.00, 0.000, 0.0000
            if ( n == 0m )
                return PluralForm.Zero;

            // "one": i = 0,1 and n != 0
            //   @integer: 1
            //   @decimal: 0.1~1.6
            if ( i.equals ( 0m, 1m ) && n != 0m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: ksh
    internal class CardinalRuleSetZeroOneC : PluralFormSelector
    {
        public static readonly CardinalRuleSetZeroOneC Instance = new CardinalRuleSetZeroOneC ( );

        private CardinalRuleSetZeroOneC ( ) : base ( PluralForm.Zero | PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "zero": n = 0
            //   @integer: 0
            //   @decimal: 0.0, 0.00, 0.000, 0.0000
            if ( n == 0m )
                return PluralForm.Zero;

            // "one": n = 1
            //   @integer: 1
            //   @decimal: 1.0, 1.00, 1.000, 1.0000
            if ( n == 1m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: iu, naq, se, sma, smi, smj, smn, sms
    internal class CardinalRuleSetOneTwoA : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneTwoA Instance = new CardinalRuleSetOneTwoA ( );

        private CardinalRuleSetOneTwoA ( ) : base ( PluralForm.One | PluralForm.Two ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n = 1
            //   @integer: 1
            //   @decimal: 1.0, 1.00, 1.000, 1.0000
            if ( n == 1m )
                return PluralForm.One;

            // "two": n = 2
            //   @integer: 2
            //   @decimal: 2.0, 2.00, 2.000, 2.0000
            if ( n == 2m )
                return PluralForm.Two;

            return PluralForm.Other;
        }
    }

    // Locales: shi
    internal class CardinalRuleSetOneFewA : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneFewA Instance = new CardinalRuleSetOneFewA ( );

        private CardinalRuleSetOneFewA ( ) : base ( PluralForm.One | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": i = 0 or n = 1
            //   @integer: 0, 1
            //   @decimal: 0.0~1.0, 0.00~0.04
            if ( i == 0m || n == 1m )
                return PluralForm.One;

            // "few": n = 2..10
            //   @integer: 2~10
            //   @decimal: 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 2.00, 3.00, 4.00, 5.00, 6.00, 7.00, 8.00
            if ( n.between ( 2m, 10m ) )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: mo, ro
    internal class CardinalRuleSetOneFewB : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneFewB Instance = new CardinalRuleSetOneFewB ( );

        private CardinalRuleSetOneFewB ( ) : base ( PluralForm.One | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": i = 1 and v = 0
            //   @integer: 1
            if ( i == 1m && v == 0m )
                return PluralForm.One;

            // "few": v != 0 or n = 0 or n % 100 = 2..19
            //   @integer: 0, 2~16, 102, 1002, …
            //   @decimal: 0.0~1.5, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …
            if ( v != 0m || n == 0m || ( n % 100m ).between ( 2m, 19m ) )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: bs, hr, sh, sr
    internal class CardinalRuleSetOneFewC : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneFewC Instance = new CardinalRuleSetOneFewC ( );

        private CardinalRuleSetOneFewC ( ) : base ( PluralForm.One | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var f = number.f ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": v = 0 and i % 10 = 1 and i % 100 != 11 or f % 10 = 1 and f % 100 != 11
            //   @integer: 1, 21, 31, 41, 51, 61, 71, 81, 101, 1001, …
            //   @decimal: 0.1, 1.1, 2.1, 3.1, 4.1, 5.1, 6.1, 7.1, 10.1, 100.1, 1000.1, …
            if ( v == 0m && i % 10m == 1m && i % 100m != 11m || f % 10m == 1m && f % 100m != 11m )
                return PluralForm.One;

            // "few": v = 0 and i % 10 = 2..4 and i % 100 != 12..14 or f % 10 = 2..4 and f % 100 != 12..14
            //   @integer: 2~4, 22~24, 32~34, 42~44, 52~54, 62, 102, 1002, …
            //   @decimal: 0.2~0.4, 1.2~1.4, 2.2~2.4, 3.2~3.4, 4.2~4.4, 5.2, 10.2, 100.2, 1000.2, …
            if ( v == 0m && ( i % 10m ).between ( 2m, 4m ) && ! ( i % 100m ).between ( 12m, 14m ) || ( f % 10m ).between ( 2m, 4m ) && ! ( f % 100m ).between ( 12m, 14m ) )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: gd
    internal class CardinalRuleSetOneTwoFewA : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneTwoFewA Instance = new CardinalRuleSetOneTwoFewA ( );

        private CardinalRuleSetOneTwoFewA ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n = 1,11
            //   @integer: 1, 11
            //   @decimal: 1.0, 11.0, 1.00, 11.00, 1.000, 11.000, 1.0000
            if ( n.equals ( 1m, 11m ) )
                return PluralForm.One;

            // "two": n = 2,12
            //   @integer: 2, 12
            //   @decimal: 2.0, 12.0, 2.00, 12.00, 2.000, 12.000, 2.0000
            if ( n.equals ( 2m, 12m ) )
                return PluralForm.Two;

            // "few": n = 3..10,13..19
            //   @integer: 3~10, 13~19
            //   @decimal: 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 13.0, 14.0, 15.0, 16.0, 17.0, 18.0, 19.0, 3.00
            if ( n.between ( 3m, 10m, 13m, 19m ) )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: sl
    internal class CardinalRuleSetOneTwoFewB : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneTwoFewB Instance = new CardinalRuleSetOneTwoFewB ( );

        private CardinalRuleSetOneTwoFewB ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": v = 0 and i % 100 = 1
            //   @integer: 1, 101, 201, 301, 401, 501, 601, 701, 1001, …
            if ( v == 0m && i % 100m == 1m )
                return PluralForm.One;

            // "two": v = 0 and i % 100 = 2
            //   @integer: 2, 102, 202, 302, 402, 502, 602, 702, 1002, …
            if ( v == 0m && i % 100m == 2m )
                return PluralForm.Two;

            // "few": v = 0 and i % 100 = 3..4 or v != 0
            //   @integer: 3, 4, 103, 104, 203, 204, 303, 304, 403, 404, 503, 504, 603, 604, 703, 704, 1003, …
            //   @decimal: 0.0~1.5, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …
            if ( v == 0m && ( i % 100m ).between ( 3m, 4m ) || v != 0m )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: dsb, hsb
    internal class CardinalRuleSetOneTwoFewC : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneTwoFewC Instance = new CardinalRuleSetOneTwoFewC ( );

        private CardinalRuleSetOneTwoFewC ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var f = number.f ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": v = 0 and i % 100 = 1 or f % 100 = 1
            //   @integer: 1, 101, 201, 301, 401, 501, 601, 701, 1001, …
            //   @decimal: 0.1, 1.1, 2.1, 3.1, 4.1, 5.1, 6.1, 7.1, 10.1, 100.1, 1000.1, …
            if ( v == 0m && i % 100m == 1m || f % 100m == 1m )
                return PluralForm.One;

            // "two": v = 0 and i % 100 = 2 or f % 100 = 2
            //   @integer: 2, 102, 202, 302, 402, 502, 602, 702, 1002, …
            //   @decimal: 0.2, 1.2, 2.2, 3.2, 4.2, 5.2, 6.2, 7.2, 10.2, 100.2, 1000.2, …
            if ( v == 0m && i % 100m == 2m || f % 100m == 2m )
                return PluralForm.Two;

            // "few": v = 0 and i % 100 = 3..4 or f % 100 = 3..4
            //   @integer: 3, 4, 103, 104, 203, 204, 303, 304, 403, 404, 503, 504, 603, 604, 703, 704, 1003, …
            //   @decimal: 0.3, 0.4, 1.3, 1.4, 2.3, 2.4, 3.3, 3.4, 4.3, 4.4, 5.3, 5.4, 6.3, 6.4, 7.3, 7.4, 10.3, 100.3, 1000.3, …
            if ( v == 0m && ( i % 100m ).between ( 3m, 4m ) || ( f % 100m ).between ( 3m, 4m ) )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: he, iw
    internal class CardinalRuleSetOneTwoManyA : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneTwoManyA Instance = new CardinalRuleSetOneTwoManyA ( );

        private CardinalRuleSetOneTwoManyA ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": i = 1 and v = 0
            //   @integer: 1
            if ( i == 1m && v == 0m )
                return PluralForm.One;

            // "two": i = 2 and v = 0
            //   @integer: 2
            if ( i == 2m && v == 0m )
                return PluralForm.Two;

            // "many": v = 0 and n != 0..10 and n % 10 = 0
            //   @integer: 20, 30, 40, 50, 60, 70, 80, 90, 100, 1000, 10000, 100000, 1000000, …
            if ( v == 0m && ! n.between ( 0m, 10m ) && n % 10m == 0m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: cs, sk
    internal class CardinalRuleSetOneFewManyA : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneFewManyA Instance = new CardinalRuleSetOneFewManyA ( );

        private CardinalRuleSetOneFewManyA ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": i = 1 and v = 0
            //   @integer: 1
            if ( i == 1m && v == 0m )
                return PluralForm.One;

            // "few": i = 2..4 and v = 0
            //   @integer: 2~4
            if ( i.between ( 2m, 4m ) && v == 0m )
                return PluralForm.Few;

            // "many": v != 0
            if ( v != 0m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: pl
    internal class CardinalRuleSetOneFewManyB : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneFewManyB Instance = new CardinalRuleSetOneFewManyB ( );

        private CardinalRuleSetOneFewManyB ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": i = 1 and v = 0
            //   @integer: 1
            if ( i == 1m && v == 0m )
                return PluralForm.One;

            // "few": v = 0 and i % 10 = 2..4 and i % 100 != 12..14
            //   @integer: 2~4, 22~24, 32~34, 42~44, 52~54, 62, 102, 1002, …
            if ( v == 0m && ( i % 10m ).between ( 2m, 4m ) && ! ( i % 100m ).between ( 12m, 14m ) )
                return PluralForm.Few;

            // "many": v = 0 and i != 1 and i % 10 = 0..1 or v = 0 and i % 10 = 5..9 or v = 0 and i % 100 = 12..14
            //   @integer: 0, 5~19, 100, 1000, 10000, 100000, 1000000, …
            if ( v == 0m && i != 1m && ( i % 10m ).between ( 0m, 1m ) || v == 0m && ( i % 10m ).between ( 5m, 9m ) || v == 0m && ( i % 100m ).between ( 12m, 14m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: be
    internal class CardinalRuleSetOneFewManyC : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneFewManyC Instance = new CardinalRuleSetOneFewManyC ( );

        private CardinalRuleSetOneFewManyC ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n % 10 = 1 and n % 100 != 11
            //   @integer: 1, 21, 31, 41, 51, 61, 71, 81, 101, 1001, …
            //   @decimal: 1.0, 21.0, 31.0, 41.0, 51.0, 61.0, 71.0, 81.0, 101.0, 1001.0, …
            if ( n % 10m == 1m && n % 100m != 11m )
                return PluralForm.One;

            // "few": n % 10 = 2..4 and n % 100 != 12..14
            //   @integer: 2~4, 22~24, 32~34, 42~44, 52~54, 62, 102, 1002, …
            //   @decimal: 2.0, 3.0, 4.0, 22.0, 23.0, 24.0, 32.0, 33.0, 102.0, 1002.0, …
            if ( ( n % 10m ).between ( 2m, 4m ) && ! ( n % 100m ).between ( 12m, 14m ) )
                return PluralForm.Few;

            // "many": n % 10 = 0 or n % 10 = 5..9 or n % 100 = 11..14
            //   @integer: 0, 5~19, 100, 1000, 10000, 100000, 1000000, …
            //   @decimal: 0.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …
            if ( n % 10m == 0m || ( n % 10m ).between ( 5m, 9m ) || ( n % 100m ).between ( 11m, 14m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: lt
    internal class CardinalRuleSetOneFewManyD : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneFewManyD Instance = new CardinalRuleSetOneFewManyD ( );

        private CardinalRuleSetOneFewManyD ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );
            var f = number.f ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n % 10 = 1 and n % 100 != 11..19
            //   @integer: 1, 21, 31, 41, 51, 61, 71, 81, 101, 1001, …
            //   @decimal: 1.0, 21.0, 31.0, 41.0, 51.0, 61.0, 71.0, 81.0, 101.0, 1001.0, …
            if ( n % 10m == 1m && ! ( n % 100m ).between ( 11m, 19m ) )
                return PluralForm.One;

            // "few": n % 10 = 2..9 and n % 100 != 11..19
            //   @integer: 2~9, 22~29, 102, 1002, …
            //   @decimal: 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 22.0, 102.0, 1002.0, …
            if ( ( n % 10m ).between ( 2m, 9m ) && ! ( n % 100m ).between ( 11m, 19m ) )
                return PluralForm.Few;

            // "many": f != 0
            if ( f != 0m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: mt
    internal class CardinalRuleSetOneFewManyE : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneFewManyE Instance = new CardinalRuleSetOneFewManyE ( );

        private CardinalRuleSetOneFewManyE ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n = 1
            //   @integer: 1
            //   @decimal: 1.0, 1.00, 1.000, 1.0000
            if ( n == 1m )
                return PluralForm.One;

            // "few": n = 0 or n % 100 = 2..10
            //   @integer: 0, 2~10, 102~107, 1002, …
            //   @decimal: 0.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 10.0, 102.0, 1002.0, …
            if ( n == 0m || ( n % 100m ).between ( 2m, 10m ) )
                return PluralForm.Few;

            // "many": n % 100 = 11..19
            //   @integer: 11~19, 111~117, 1011, …
            //   @decimal: 11.0, 12.0, 13.0, 14.0, 15.0, 16.0, 17.0, 18.0, 111.0, 1011.0, …
            if ( ( n % 100m ).between ( 11m, 19m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: ru, uk
    internal class CardinalRuleSetOneFewManyF : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneFewManyF Instance = new CardinalRuleSetOneFewManyF ( );

        private CardinalRuleSetOneFewManyF ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": v = 0 and i % 10 = 1 and i % 100 != 11
            //   @integer: 1, 21, 31, 41, 51, 61, 71, 81, 101, 1001, …
            if ( v == 0m && i % 10m == 1m && i % 100m != 11m )
                return PluralForm.One;

            // "few": v = 0 and i % 10 = 2..4 and i % 100 != 12..14
            //   @integer: 2~4, 22~24, 32~34, 42~44, 52~54, 62, 102, 1002, …
            if ( v == 0m && ( i % 10m ).between ( 2m, 4m ) && ! ( i % 100m ).between ( 12m, 14m ) )
                return PluralForm.Few;

            // "many": v = 0 and i % 10 = 0 or v = 0 and i % 10 = 5..9 or v = 0 and i % 100 = 11..14
            //   @integer: 0, 5~19, 100, 1000, 10000, 100000, 1000000, …
            if ( v == 0m && i % 10m == 0m || v == 0m && ( i % 10m ).between ( 5m, 9m ) || v == 0m && ( i % 100m ).between ( 11m, 14m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: br
    internal class CardinalRuleSetOneTwoFewManyA : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneTwoFewManyA Instance = new CardinalRuleSetOneTwoFewManyA ( );

        private CardinalRuleSetOneTwoFewManyA ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n % 10 = 1 and n % 100 != 11,71,91
            //   @integer: 1, 21, 31, 41, 51, 61, 81, 101, 1001, …
            //   @decimal: 1.0, 21.0, 31.0, 41.0, 51.0, 61.0, 81.0, 101.0, 1001.0, …
            if ( n % 10m == 1m && ! ( n % 100m ).equals ( 11m, 71m, 91m ) )
                return PluralForm.One;

            // "two": n % 10 = 2 and n % 100 != 12,72,92
            //   @integer: 2, 22, 32, 42, 52, 62, 82, 102, 1002, …
            //   @decimal: 2.0, 22.0, 32.0, 42.0, 52.0, 62.0, 82.0, 102.0, 1002.0, …
            if ( n % 10m == 2m && ! ( n % 100m ).equals ( 12m, 72m, 92m ) )
                return PluralForm.Two;

            // "few": n % 10 = 3..4,9 and n % 100 != 10..19,70..79,90..99
            //   @integer: 3, 4, 9, 23, 24, 29, 33, 34, 39, 43, 44, 49, 103, 1003, …
            //   @decimal: 3.0, 4.0, 9.0, 23.0, 24.0, 29.0, 33.0, 34.0, 103.0, 1003.0, …
            if ( ( ( n % 10m ).between ( 3m, 4m ) || ( n % 10m ).equals ( 9m ) ) && ! ( n % 100m ).between ( 10m, 19m, 70m, 79m, 90m, 99m ) )
                return PluralForm.Few;

            // "many": n != 0 and n % 1000000 = 0
            //   @integer: 1000000, …
            //   @decimal: 1000000.0, 1000000.00, 1000000.000, …
            if ( n != 0m && n % 1000000m == 0m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: ga
    internal class CardinalRuleSetOneTwoFewManyB : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneTwoFewManyB Instance = new CardinalRuleSetOneTwoFewManyB ( );

        private CardinalRuleSetOneTwoFewManyB ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": n = 1
            //   @integer: 1
            //   @decimal: 1.0, 1.00, 1.000, 1.0000
            if ( n == 1m )
                return PluralForm.One;

            // "two": n = 2
            //   @integer: 2
            //   @decimal: 2.0, 2.00, 2.000, 2.0000
            if ( n == 2m )
                return PluralForm.Two;

            // "few": n = 3..6
            //   @integer: 3~6
            //   @decimal: 3.0, 4.0, 5.0, 6.0, 3.00, 4.00, 5.00, 6.00, 3.000, 4.000, 5.000, 6.000, 3.0000, 4.0000, 5.0000, 6.0000
            if ( n.between ( 3m, 6m ) )
                return PluralForm.Few;

            // "many": n = 7..10
            //   @integer: 7~10
            //   @decimal: 7.0, 8.0, 9.0, 10.0, 7.00, 8.00, 9.00, 10.00, 7.000, 8.000, 9.000, 10.000, 7.0000, 8.0000, 9.0000, 10.0000
            if ( n.between ( 7m, 10m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: gv
    internal class CardinalRuleSetOneTwoFewManyC : PluralFormSelector
    {
        public static readonly CardinalRuleSetOneTwoFewManyC Instance = new CardinalRuleSetOneTwoFewManyC ( );

        private CardinalRuleSetOneTwoFewManyC ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }
            // Explicit "0" rule "zero" fallback
            else if ( availablePluralForms.HasBitMask ( PluralForm.Zero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.Zero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "one": v = 0 and i % 10 = 1
            //   @integer: 1, 11, 21, 31, 41, 51, 61, 71, 101, 1001, …
            if ( v == 0m && i % 10m == 1m )
                return PluralForm.One;

            // "two": v = 0 and i % 10 = 2
            //   @integer: 2, 12, 22, 32, 42, 52, 62, 72, 102, 1002, …
            if ( v == 0m && i % 10m == 2m )
                return PluralForm.Two;

            // "few": v = 0 and i % 100 = 0,20,40,60,80
            //   @integer: 0, 20, 40, 60, 80, 100, 120, 140, 1000, 10000, 100000, 1000000, …
            if ( v == 0m && ( i % 100m ).equals ( 0m, 20m, 40m, 60m, 80m ) )
                return PluralForm.Few;

            // "many": v != 0
            if ( v != 0m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: ar, ars
    internal class CardinalRuleSetZeroOneTwoFewManyA : PluralFormSelector
    {
        public static readonly CardinalRuleSetZeroOneTwoFewManyA Instance = new CardinalRuleSetZeroOneTwoFewManyA ( );

        private CardinalRuleSetZeroOneTwoFewManyA ( ) : base ( PluralForm.Zero | PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "zero": n = 0
            //   @integer: 0
            //   @decimal: 0.0, 0.00, 0.000, 0.0000
            if ( n == 0m )
                return PluralForm.Zero;

            // "one": n = 1
            //   @integer: 1
            //   @decimal: 1.0, 1.00, 1.000, 1.0000
            if ( n == 1m )
                return PluralForm.One;

            // "two": n = 2
            //   @integer: 2
            //   @decimal: 2.0, 2.00, 2.000, 2.0000
            if ( n == 2m )
                return PluralForm.Two;

            // "few": n % 100 = 3..10
            //   @integer: 3~10, 103~110, 1003, …
            //   @decimal: 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 103.0, 1003.0, …
            if ( ( n % 100m ).between ( 3m, 10m ) )
                return PluralForm.Few;

            // "many": n % 100 = 11..99
            //   @integer: 11~26, 111, 1011, …
            //   @decimal: 11.0, 12.0, 13.0, 14.0, 15.0, 16.0, 17.0, 18.0, 111.0, 1011.0, …
            if ( ( n % 100m ).between ( 11m, 99m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: cy
    internal class CardinalRuleSetZeroOneTwoFewManyB : PluralFormSelector
    {
        public static readonly CardinalRuleSetZeroOneTwoFewManyB Instance = new CardinalRuleSetZeroOneTwoFewManyB ( );

        private CardinalRuleSetZeroOneTwoFewManyB ( ) : base ( PluralForm.Zero | PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "zero": n = 0
            //   @integer: 0
            //   @decimal: 0.0, 0.00, 0.000, 0.0000
            if ( n == 0m )
                return PluralForm.Zero;

            // "one": n = 1
            //   @integer: 1
            //   @decimal: 1.0, 1.00, 1.000, 1.0000
            if ( n == 1m )
                return PluralForm.One;

            // "two": n = 2
            //   @integer: 2
            //   @decimal: 2.0, 2.00, 2.000, 2.0000
            if ( n == 2m )
                return PluralForm.Two;

            // "few": n = 3
            //   @integer: 3
            //   @decimal: 3.0, 3.00, 3.000, 3.0000
            if ( n == 3m )
                return PluralForm.Few;

            // "many": n = 6
            //   @integer: 6
            //   @decimal: 6.0, 6.00, 6.000, 6.0000
            if ( n == 6m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: kw
    internal class CardinalRuleSetZeroOneTwoFewManyC : PluralFormSelector
    {
        public static readonly CardinalRuleSetZeroOneTwoFewManyC Instance = new CardinalRuleSetZeroOneTwoFewManyC ( );

        private CardinalRuleSetZeroOneTwoFewManyC ( ) : base ( PluralForm.Zero | PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );
            var v = number.v ( );
            var n = number.n ( );

            // Explicit "0" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitZero ) )
            {
                if ( ExplicitRule.Zero ( i, v ) )
                    return PluralForm.ExplicitZero;
            }

            // Explicit "1" rule
            if ( availablePluralForms.HasBitMask ( PluralForm.ExplicitOne ) )
            {
                if ( ExplicitRule.One ( i, v ) )
                    return PluralForm.ExplicitOne;
            }

            // "zero": n = 0
            //   @integer: 0
            //   @decimal: 0.0, 0.00, 0.000, 0.0000
            if ( n == 0m )
                return PluralForm.Zero;

            // "one": n = 1
            //   @integer: 1
            //   @decimal: 1.0, 1.00, 1.000, 1.0000
            if ( n == 1m )
                return PluralForm.One;

            // "two": n % 100 = 2,22,42,62,82 or n%1000 = 0 and n%100000=1000..20000,40000,60000,80000 or n!=0 and n%1000000=100000
            //   @integer: 2, 22, 42, 62, 82, 102, 122, 142, 1002, …
            //   @decimal: 2.0, 22.0, 42.0, 62.0, 82.0, 102.0, 122.0, 142.0, 1002.0, …
            if ( ( n % 100m ).equals ( 2m, 22m, 42m, 62m, 82m ) || n % 1000m == 0m && ( ( n % 100000m ).between ( 1000m, 20000m ) || ( n % 100000m ).equals ( 40000m, 60000m, 80000m ) ) || n != 0m && n % 1000000m == 100000m )
                return PluralForm.Two;

            // "few": n % 100 = 3,23,43,63,83
            //   @integer: 3, 23, 43, 63, 83, 103, 123, 143, 1003, …
            //   @decimal: 3.0, 23.0, 43.0, 63.0, 83.0, 103.0, 123.0, 143.0, 1003.0, …
            if ( ( n % 100m ).equals ( 3m, 23m, 43m, 63m, 83m ) )
                return PluralForm.Few;

            // "many": n != 1 and n % 100 = 1,21,41,61,81
            //   @integer: 21, 41, 61, 81, 101, 121, 141, 161, 1001, …
            //   @decimal: 21.0, 41.0, 61.0, 81.0, 101.0, 121.0, 141.0, 161.0, 1001.0, …
            if ( n != 1m && ( n % 100m ).equals ( 1m, 21m, 41m, 61m, 81m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: af, am, ar, bg, bs, ce, cs, da, de, dsb, el, es, et, eu, fa, fi, fy, gl, gsw, he, hr, hsb, ia, id, in, is, iw, ja, km, kn, ko, ky, lt, lv, ml, mn, my, nb, nl, pa, pl, prg, ps, pt, root, ru, sd, sh, si, sk, sl, sr, sw, ta, te, th, tr, ur, uz, yue, zh, zu
    internal class DefaultOrdinalRuleSet : PluralFormSelector
    {
        public static readonly DefaultOrdinalRuleSet Instance = new DefaultOrdinalRuleSet ( );

        private DefaultOrdinalRuleSet ( ) : base ( PluralForm.Other ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {

            return PluralForm.Other;
        }
    }

    // Locales: sv
    internal class OrdinalRuleSetOneA : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneA Instance = new OrdinalRuleSetOneA ( );

        private OrdinalRuleSetOneA ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n % 10 = 1,2 and n % 100 != 11,12
            //   @integer: 1, 2, 21, 22, 31, 32, 41, 42, 51, 52, 61, 62, 71, 72, 81, 82, 101, 1001, …
            if ( ( n % 10m ).equals ( 1m, 2m ) && ! ( n % 100m ).equals ( 11m, 12m ) )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: fil, fr, ga, hy, lo, mo, ms, ro, tl, vi
    internal class OrdinalRuleSetOneB : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneB Instance = new OrdinalRuleSetOneB ( );

        private OrdinalRuleSetOneB ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1
            //   @integer: 1
            if ( n == 1m )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: hu
    internal class OrdinalRuleSetOneC : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneC Instance = new OrdinalRuleSetOneC ( );

        private OrdinalRuleSetOneC ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1,5
            //   @integer: 1, 5
            if ( n.equals ( 1m, 5m ) )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: ne
    internal class OrdinalRuleSetOneD : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneD Instance = new OrdinalRuleSetOneD ( );

        private OrdinalRuleSetOneD ( ) : base ( PluralForm.One ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1..4
            //   @integer: 1~4
            if ( n.between ( 1m, 4m ) )
                return PluralForm.One;

            return PluralForm.Other;
        }
    }

    // Locales: be
    internal class OrdinalRuleSetFewA : PluralFormSelector
    {
        public static readonly OrdinalRuleSetFewA Instance = new OrdinalRuleSetFewA ( );

        private OrdinalRuleSetFewA ( ) : base ( PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "few": n % 10 = 2,3 and n % 100 != 12,13
            //   @integer: 2, 3, 22, 23, 32, 33, 42, 43, 52, 53, 62, 63, 72, 73, 82, 83, 102, 1002, …
            if ( ( n % 10m ).equals ( 2m, 3m ) && ! ( n % 100m ).equals ( 12m, 13m ) )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: uk
    internal class OrdinalRuleSetFewB : PluralFormSelector
    {
        public static readonly OrdinalRuleSetFewB Instance = new OrdinalRuleSetFewB ( );

        private OrdinalRuleSetFewB ( ) : base ( PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "few": n % 10 = 3 and n % 100 != 13
            //   @integer: 3, 23, 33, 43, 53, 63, 73, 83, 103, 1003, …
            if ( n % 10m == 3m && n % 100m != 13m )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: tk
    internal class OrdinalRuleSetFewC : PluralFormSelector
    {
        public static readonly OrdinalRuleSetFewC Instance = new OrdinalRuleSetFewC ( );

        private OrdinalRuleSetFewC ( ) : base ( PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "few": n % 10 = 6,9 or n = 10
            //   @integer: 6, 9, 10, 16, 19, 26, 29, 36, 39, 106, 1006, …
            if ( ( n % 10m ).equals ( 6m, 9m ) || n == 10m )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: kk
    internal class OrdinalRuleSetManyA : PluralFormSelector
    {
        public static readonly OrdinalRuleSetManyA Instance = new OrdinalRuleSetManyA ( );

        private OrdinalRuleSetManyA ( ) : base ( PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "many": n % 10 = 6 or n % 10 = 9 or n % 10 = 0 and n != 0
            //   @integer: 6, 9, 10, 16, 19, 20, 26, 29, 30, 36, 39, 40, 100, 1000, 10000, 100000, 1000000, …
            if ( n % 10m == 6m || n % 10m == 9m || n % 10m == 0m && n != 0m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: it, sc, scn
    internal class OrdinalRuleSetManyB : PluralFormSelector
    {
        public static readonly OrdinalRuleSetManyB Instance = new OrdinalRuleSetManyB ( );

        private OrdinalRuleSetManyB ( ) : base ( PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "many": n = 11,8,80,800
            //   @integer: 8, 11, 80, 800
            if ( n.equals ( 11m, 8m, 80m, 800m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: ka
    internal class OrdinalRuleSetOneManyA : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneManyA Instance = new OrdinalRuleSetOneManyA ( );

        private OrdinalRuleSetOneManyA ( ) : base ( PluralForm.One | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );

            // "one": i = 1
            //   @integer: 1
            if ( i == 1m )
                return PluralForm.One;

            // "many": i = 0 or i % 100 = 2..20,40,60,80
            //   @integer: 0, 2~16, 102, 1002, …
            if ( i == 0m || ( ( i % 100m ).between ( 2m, 20m ) || ( i % 100m ).equals ( 40m, 60m, 80m ) ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: sq
    internal class OrdinalRuleSetOneManyB : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneManyB Instance = new OrdinalRuleSetOneManyB ( );

        private OrdinalRuleSetOneManyB ( ) : base ( PluralForm.One | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1
            //   @integer: 1
            if ( n == 1m )
                return PluralForm.One;

            // "many": n % 10 = 4 and n % 100 != 14
            //   @integer: 4, 24, 34, 44, 54, 64, 74, 84, 104, 1004, …
            if ( n % 10m == 4m && n % 100m != 14m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: kw
    internal class OrdinalRuleSetOneManyC : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneManyC Instance = new OrdinalRuleSetOneManyC ( );

        private OrdinalRuleSetOneManyC ( ) : base ( PluralForm.One | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1..4 or n % 100 = 1..4,21..24,41..44,61..64,81..84
            //   @integer: 1~4, 21~24, 41~44, 61~64, 101, 1001, …
            if ( n.between ( 1m, 4m ) || ( n % 100m ).between ( 1m, 4m, 21m, 24m, 41m, 44m, 61m, 64m, 81m, 84m ) )
                return PluralForm.One;

            // "many": n = 5 or n % 100 = 5
            //   @integer: 5, 105, 205, 305, 405, 505, 605, 705, 1005, …
            if ( n == 5m || n % 100m == 5m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: en
    internal class OrdinalRuleSetOneTwoFewA : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneTwoFewA Instance = new OrdinalRuleSetOneTwoFewA ( );

        private OrdinalRuleSetOneTwoFewA ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n % 10 = 1 and n % 100 != 11
            //   @integer: 1, 21, 31, 41, 51, 61, 71, 81, 101, 1001, …
            if ( n % 10m == 1m && n % 100m != 11m )
                return PluralForm.One;

            // "two": n % 10 = 2 and n % 100 != 12
            //   @integer: 2, 22, 32, 42, 52, 62, 72, 82, 102, 1002, …
            if ( n % 10m == 2m && n % 100m != 12m )
                return PluralForm.Two;

            // "few": n % 10 = 3 and n % 100 != 13
            //   @integer: 3, 23, 33, 43, 53, 63, 73, 83, 103, 1003, …
            if ( n % 10m == 3m && n % 100m != 13m )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: mr
    internal class OrdinalRuleSetOneTwoFewB : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneTwoFewB Instance = new OrdinalRuleSetOneTwoFewB ( );

        private OrdinalRuleSetOneTwoFewB ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1
            //   @integer: 1
            if ( n == 1m )
                return PluralForm.One;

            // "two": n = 2,3
            //   @integer: 2, 3
            if ( n.equals ( 2m, 3m ) )
                return PluralForm.Two;

            // "few": n = 4
            //   @integer: 4
            if ( n == 4m )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: gd
    internal class OrdinalRuleSetOneTwoFewC : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneTwoFewC Instance = new OrdinalRuleSetOneTwoFewC ( );

        private OrdinalRuleSetOneTwoFewC ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1,11
            //   @integer: 1, 11
            if ( n.equals ( 1m, 11m ) )
                return PluralForm.One;

            // "two": n = 2,12
            //   @integer: 2, 12
            if ( n.equals ( 2m, 12m ) )
                return PluralForm.Two;

            // "few": n = 3,13
            //   @integer: 3, 13
            if ( n.equals ( 3m, 13m ) )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: ca
    internal class OrdinalRuleSetOneTwoFewD : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneTwoFewD Instance = new OrdinalRuleSetOneTwoFewD ( );

        private OrdinalRuleSetOneTwoFewD ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1,3
            //   @integer: 1, 3
            if ( n.equals ( 1m, 3m ) )
                return PluralForm.One;

            // "two": n = 2
            //   @integer: 2
            if ( n == 2m )
                return PluralForm.Two;

            // "few": n = 4
            //   @integer: 4
            if ( n == 4m )
                return PluralForm.Few;

            return PluralForm.Other;
        }
    }

    // Locales: mk
    internal class OrdinalRuleSetOneTwoManyA : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneTwoManyA Instance = new OrdinalRuleSetOneTwoManyA ( );

        private OrdinalRuleSetOneTwoManyA ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );

            // "one": i % 10 = 1 and i % 100 != 11
            //   @integer: 1, 21, 31, 41, 51, 61, 71, 81, 101, 1001, …
            if ( i % 10m == 1m && i % 100m != 11m )
                return PluralForm.One;

            // "two": i % 10 = 2 and i % 100 != 12
            //   @integer: 2, 22, 32, 42, 52, 62, 72, 82, 102, 1002, …
            if ( i % 10m == 2m && i % 100m != 12m )
                return PluralForm.Two;

            // "many": i % 10 = 7,8 and i % 100 != 17,18
            //   @integer: 7, 8, 27, 28, 37, 38, 47, 48, 57, 58, 67, 68, 77, 78, 87, 88, 107, 1007, …
            if ( ( i % 10m ).equals ( 7m, 8m ) && ! ( i % 100m ).equals ( 17m, 18m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: az
    internal class OrdinalRuleSetOneFewManyA : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneFewManyA Instance = new OrdinalRuleSetOneFewManyA ( );

        private OrdinalRuleSetOneFewManyA ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var i = number.i ( );

            // "one": i % 10 = 1,2,5,7,8 or i % 100 = 20,50,70,80
            //   @integer: 1, 2, 5, 7, 8, 11, 12, 15, 17, 18, 20~22, 25, 101, 1001, …
            if ( ( i % 10m ).equals ( 1m, 2m, 5m, 7m, 8m ) || ( i % 100m ).equals ( 20m, 50m, 70m, 80m ) )
                return PluralForm.One;

            // "few": i % 10 = 3,4 or i % 1000 = 100,200,300,400,500,600,700,800,900
            //   @integer: 3, 4, 13, 14, 23, 24, 33, 34, 43, 44, 53, 54, 63, 64, 73, 74, 100, 1003, …
            if ( ( i % 10m ).equals ( 3m, 4m ) || ( i % 1000m ).equals ( 100m, 200m, 300m, 400m, 500m, 600m, 700m, 800m, 900m ) )
                return PluralForm.Few;

            // "many": i = 0 or i % 10 = 6 or i % 100 = 40,60,90
            //   @integer: 0, 6, 16, 26, 36, 40, 46, 56, 106, 1006, …
            if ( i == 0m || i % 10m == 6m || ( i % 100m ).equals ( 40m, 60m, 90m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: gu, hi
    internal class OrdinalRuleSetOneTwoFewManyA : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneTwoFewManyA Instance = new OrdinalRuleSetOneTwoFewManyA ( );

        private OrdinalRuleSetOneTwoFewManyA ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1
            //   @integer: 1
            if ( n == 1m )
                return PluralForm.One;

            // "two": n = 2,3
            //   @integer: 2, 3
            if ( n.equals ( 2m, 3m ) )
                return PluralForm.Two;

            // "few": n = 4
            //   @integer: 4
            if ( n == 4m )
                return PluralForm.Few;

            // "many": n = 6
            //   @integer: 6
            if ( n == 6m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: as, bn
    internal class OrdinalRuleSetOneTwoFewManyB : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneTwoFewManyB Instance = new OrdinalRuleSetOneTwoFewManyB ( );

        private OrdinalRuleSetOneTwoFewManyB ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1,5,7,8,9,10
            //   @integer: 1, 5, 7~10
            if ( n.equals ( 1m, 5m, 7m, 8m, 9m, 10m ) )
                return PluralForm.One;

            // "two": n = 2,3
            //   @integer: 2, 3
            if ( n.equals ( 2m, 3m ) )
                return PluralForm.Two;

            // "few": n = 4
            //   @integer: 4
            if ( n == 4m )
                return PluralForm.Few;

            // "many": n = 6
            //   @integer: 6
            if ( n == 6m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: or
    internal class OrdinalRuleSetOneTwoFewManyC : PluralFormSelector
    {
        public static readonly OrdinalRuleSetOneTwoFewManyC Instance = new OrdinalRuleSetOneTwoFewManyC ( );

        private OrdinalRuleSetOneTwoFewManyC ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "one": n = 1,5,7..9
            //   @integer: 1, 5, 7~9
            if ( ( n.between ( 7m, 9m ) || n.equals ( 1m, 5m ) ) )
                return PluralForm.One;

            // "two": n = 2,3
            //   @integer: 2, 3
            if ( n.equals ( 2m, 3m ) )
                return PluralForm.Two;

            // "few": n = 4
            //   @integer: 4
            if ( n == 4m )
                return PluralForm.Few;

            // "many": n = 6
            //   @integer: 6
            if ( n == 6m )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }

    // Locales: cy
    internal class OrdinalRuleSetZeroOneTwoFewManyA : PluralFormSelector
    {
        public static readonly OrdinalRuleSetZeroOneTwoFewManyA Instance = new OrdinalRuleSetZeroOneTwoFewManyA ( );

        private OrdinalRuleSetZeroOneTwoFewManyA ( ) : base ( PluralForm.Zero | PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

        public override PluralForm SelectPluralForm ( decimal number, PluralForm availablePluralForms )
        {
            var n = number.n ( );

            // "zero": n = 0,7,8,9
            //   @integer: 0, 7~9
            if ( n.equals ( 0m, 7m, 8m, 9m ) )
                return PluralForm.Zero;

            // "one": n = 1
            //   @integer: 1
            if ( n == 1m )
                return PluralForm.One;

            // "two": n = 2
            //   @integer: 2
            if ( n == 2m )
                return PluralForm.Two;

            // "few": n = 3,4
            //   @integer: 3, 4
            if ( n.equals ( 3m, 4m ) )
                return PluralForm.Few;

            // "many": n = 5,6
            //   @integer: 5, 6
            if ( n.equals ( 5m, 6m ) )
                return PluralForm.Many;

            return PluralForm.Other;
        }
    }
}