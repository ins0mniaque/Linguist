using System.Globalization;

using Localizer.Pluralization;

namespace Localizer.CLDR
{
    internal static class PluralRuleSets
    {
        public static PluralFormSelector GetCardinalRuleSet ( CultureInfo culture )
        {
            return null;
        }

        public static PluralFormSelector GetOrdinalRuleSet ( CultureInfo culture )
        {
            return null;
        }

        public static PluralFormRangeSelector GetRangeRuleSet ( CultureInfo culture )
        {
            return null;
        }
    }

    // Locales: bm, bo, dz, id, ig, ii, in, ja, jbo, jv, jw, kde, kea, km, ko, lkt, lo, ms, my, nqo, root, sah, ses, sg, th, to, vi, wo, yo, yue, zh
    public class DefaultCardinalRuleSet : PluralFormSelector
    {
        public DefaultCardinalRuleSet ( ) : base ( PluralForm.Other ) { }

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
    public class CardinalRuleSetOneA : PluralFormSelector
    {
        public CardinalRuleSetOneA ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneB : PluralFormSelector
    {
        public CardinalRuleSetOneB ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneC : PluralFormSelector
    {
        public CardinalRuleSetOneC ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneD : PluralFormSelector
    {
        public CardinalRuleSetOneD ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneE : PluralFormSelector
    {
        public CardinalRuleSetOneE ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneF : PluralFormSelector
    {
        public CardinalRuleSetOneF ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneG : PluralFormSelector
    {
        public CardinalRuleSetOneG ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneH : PluralFormSelector
    {
        public CardinalRuleSetOneH ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneI : PluralFormSelector
    {
        public CardinalRuleSetOneI ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneJ : PluralFormSelector
    {
        public CardinalRuleSetOneJ ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneK : PluralFormSelector
    {
        public CardinalRuleSetOneK ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetOneL : PluralFormSelector
    {
        public CardinalRuleSetOneL ( ) : base ( PluralForm.One ) { }

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
    public class CardinalRuleSetZeroOneA : PluralFormSelector
    {
        public CardinalRuleSetZeroOneA ( ) : base ( PluralForm.Zero | PluralForm.One ) { }

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
    public class CardinalRuleSetZeroOneB : PluralFormSelector
    {
        public CardinalRuleSetZeroOneB ( ) : base ( PluralForm.Zero | PluralForm.One ) { }

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
    public class CardinalRuleSetZeroOneC : PluralFormSelector
    {
        public CardinalRuleSetZeroOneC ( ) : base ( PluralForm.Zero | PluralForm.One ) { }

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
    public class CardinalRuleSetOneTwoA : PluralFormSelector
    {
        public CardinalRuleSetOneTwoA ( ) : base ( PluralForm.One | PluralForm.Two ) { }

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
    public class CardinalRuleSetOneFewA : PluralFormSelector
    {
        public CardinalRuleSetOneFewA ( ) : base ( PluralForm.One | PluralForm.Few ) { }

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
    public class CardinalRuleSetOneFewB : PluralFormSelector
    {
        public CardinalRuleSetOneFewB ( ) : base ( PluralForm.One | PluralForm.Few ) { }

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
    public class CardinalRuleSetOneFewC : PluralFormSelector
    {
        public CardinalRuleSetOneFewC ( ) : base ( PluralForm.One | PluralForm.Few ) { }

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
    public class CardinalRuleSetOneTwoFewA : PluralFormSelector
    {
        public CardinalRuleSetOneTwoFewA ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

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
    public class CardinalRuleSetOneTwoFewB : PluralFormSelector
    {
        public CardinalRuleSetOneTwoFewB ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

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
    public class CardinalRuleSetOneTwoFewC : PluralFormSelector
    {
        public CardinalRuleSetOneTwoFewC ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few ) { }

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
    public class CardinalRuleSetOneTwoManyA : PluralFormSelector
    {
        public CardinalRuleSetOneTwoManyA ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Many ) { }

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
    public class CardinalRuleSetOneFewManyA : PluralFormSelector
    {
        public CardinalRuleSetOneFewManyA ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetOneFewManyB : PluralFormSelector
    {
        public CardinalRuleSetOneFewManyB ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetOneFewManyC : PluralFormSelector
    {
        public CardinalRuleSetOneFewManyC ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetOneFewManyD : PluralFormSelector
    {
        public CardinalRuleSetOneFewManyD ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetOneFewManyE : PluralFormSelector
    {
        public CardinalRuleSetOneFewManyE ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetOneFewManyF : PluralFormSelector
    {
        public CardinalRuleSetOneFewManyF ( ) : base ( PluralForm.One | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetOneTwoFewManyA : PluralFormSelector
    {
        public CardinalRuleSetOneTwoFewManyA ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetOneTwoFewManyB : PluralFormSelector
    {
        public CardinalRuleSetOneTwoFewManyB ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetOneTwoFewManyC : PluralFormSelector
    {
        public CardinalRuleSetOneTwoFewManyC ( ) : base ( PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetZeroOneTwoFewManyA : PluralFormSelector
    {
        public CardinalRuleSetZeroOneTwoFewManyA ( ) : base ( PluralForm.Zero | PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetZeroOneTwoFewManyB : PluralFormSelector
    {
        public CardinalRuleSetZeroOneTwoFewManyB ( ) : base ( PluralForm.Zero | PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

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
    public class CardinalRuleSetZeroOneTwoFewManyC : PluralFormSelector
    {
        public CardinalRuleSetZeroOneTwoFewManyC ( ) : base ( PluralForm.Zero | PluralForm.One | PluralForm.Two | PluralForm.Few | PluralForm.Many ) { }

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
}