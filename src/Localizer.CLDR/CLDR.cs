using System.Globalization;

using Localizer.Plural;

namespace Localizer.CLDR
{
    internal static class RuleSets
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

    public class bm_bo_dz_id_ig_ii_in_ja_jbo_jv_jw_kde_kea_km_ko_lkt_lo_ms_my_nqo_root_sah_ses_sg_th_to_vi_wo_yo_yue_zh_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class am_as_bn_fa_gu_hi_kn_zu_one
    {
        public static bool Rule ( decimal number ) => false; // i = 0 or n = 1 
    }

    public class am_as_bn_fa_gu_hi_kn_zu_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class ff_fr_hy_kab_one
    {
        public static bool Rule ( decimal number ) => false; // i = 0,1 
    }

    public class ff_fr_hy_kab_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class pt_one
    {
        public static bool Rule ( decimal number ) => false; // i = 0..1 
    }

    public class pt_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class ast_ca_de_en_et_fi_fy_gl_ia_io_it_ji_nl_pt_PT_sc_scn_sv_sw_ur_yi_one
    {
        public static bool Rule ( decimal number ) => false; // i = 1 and v = 0 
    }

    public class ast_ca_de_en_et_fi_fy_gl_ia_io_it_ji_nl_pt_PT_sc_scn_sv_sw_ur_yi_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class si_one
    {
        public static bool Rule ( decimal number ) => false; // n = 0,1 or i = 0 and f = 1 
    }

    public class si_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class ak_bh_guw_ln_mg_nso_pa_ti_wa_one
    {
        public static bool Rule ( decimal number ) => false; // n = 0..1 
    }

    public class ak_bh_guw_ln_mg_nso_pa_ti_wa_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class tzm_one
    {
        public static bool Rule ( decimal number ) => false; // n = 0..1 or n = 11..99 
    }

    public class tzm_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class af_asa_az_bem_bez_bg_brx_ce_cgg_chr_ckb_dv_ee_el_eo_es_eu_fo_fur_gsw_ha_haw_hu_jgo_jmc_ka_kaj_kcg_kk_kkj_kl_ks_ksb_ku_ky_lb_lg_mas_mgo_ml_mn_mr_nah_nb_nd_ne_nn_nnh_no_nr_ny_nyn_om_or_os_pap_ps_rm_rof_rwk_saq_sd_sdh_seh_sn_so_sq_ss_ssy_st_syr_ta_te_teo_tig_tk_tn_tr_ts_ug_uz_ve_vo_vun_wae_xh_xog_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1 
    }

    public class af_asa_az_bem_bez_bg_brx_ce_cgg_chr_ckb_dv_ee_el_eo_es_eu_fo_fur_gsw_ha_haw_hu_jgo_jmc_ka_kaj_kcg_kk_kkj_kl_ks_ksb_ku_ky_lb_lg_mas_mgo_ml_mn_mr_nah_nb_nd_ne_nn_nnh_no_nr_ny_nyn_om_or_os_pap_ps_rm_rof_rwk_saq_sd_sdh_seh_sn_so_sq_ss_ssy_st_syr_ta_te_teo_tig_tk_tn_tr_ts_ug_uz_ve_vo_vun_wae_xh_xog_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class da_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1 or t != 0 and i = 0,1 
    }

    public class da_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class is_one
    {
        public static bool Rule ( decimal number ) => false; // t = 0 and i % 10 = 1 and i % 100 != 11 or t != 0 
    }

    public class is_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class mk_one
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 10 = 1 and i % 100 != 11 or f % 10 = 1 and f % 100 != 11 
    }

    public class mk_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class ceb_fil_tl_one
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i = 1,2,3 or v = 0 and i % 10 != 4,6,9 or v != 0 and f % 10 != 4,6,9 
    }

    public class ceb_fil_tl_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class lv_prg_zero
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 0 or n % 100 = 11..19 or v = 2 and f % 100 = 11..19 
    }

    public class lv_prg_one
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 1 and n % 100 != 11 or v = 2 and f % 10 = 1 and f % 100 != 11 or v != 2 and f % 10 = 1 
    }

    public class lv_prg_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class lag_zero
    {
        public static bool Rule ( decimal number ) => false; // n = 0 
    }

    public class lag_one
    {
        public static bool Rule ( decimal number ) => false; // i = 0,1 and n != 0 
    }

    public class lag_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class ksh_zero
    {
        public static bool Rule ( decimal number ) => false; // n = 0 
    }

    public class ksh_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1 
    }

    public class ksh_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class iu_naq_se_sma_smi_smj_smn_sms_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1 
    }

    public class iu_naq_se_sma_smi_smj_smn_sms_two
    {
        public static bool Rule ( decimal number ) => false; // n = 2 
    }

    public class iu_naq_se_sma_smi_smj_smn_sms_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class shi_one
    {
        public static bool Rule ( decimal number ) => false; // i = 0 or n = 1 
    }

    public class shi_few
    {
        public static bool Rule ( decimal number ) => false; // n = 2..10 
    }

    public class shi_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class mo_ro_one
    {
        public static bool Rule ( decimal number ) => false; // i = 1 and v = 0 
    }

    public class mo_ro_few
    {
        public static bool Rule ( decimal number ) => false; // v != 0 or n = 0 or n % 100 = 2..19 
    }

    public class mo_ro_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class bs_hr_sh_sr_one
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 10 = 1 and i % 100 != 11 or f % 10 = 1 and f % 100 != 11 
    }

    public class bs_hr_sh_sr_few
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 10 = 2..4 and i % 100 != 12..14 or f % 10 = 2..4 and f % 100 != 12..14 
    }

    public class bs_hr_sh_sr_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class gd_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1,11 
    }

    public class gd_two
    {
        public static bool Rule ( decimal number ) => false; // n = 2,12 
    }

    public class gd_few
    {
        public static bool Rule ( decimal number ) => false; // n = 3..10,13..19 
    }

    public class gd_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class sl_one
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 100 = 1 
    }

    public class sl_two
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 100 = 2 
    }

    public class sl_few
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 100 = 3..4 or v != 0 
    }

    public class sl_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class dsb_hsb_one
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 100 = 1 or f % 100 = 1 
    }

    public class dsb_hsb_two
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 100 = 2 or f % 100 = 2 
    }

    public class dsb_hsb_few
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 100 = 3..4 or f % 100 = 3..4 
    }

    public class dsb_hsb_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class he_iw_one
    {
        public static bool Rule ( decimal number ) => false; // i = 1 and v = 0 
    }

    public class he_iw_two
    {
        public static bool Rule ( decimal number ) => false; // i = 2 and v = 0 
    }

    public class he_iw_many
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and n != 0..10 and n % 10 = 0 
    }

    public class he_iw_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class cs_sk_one
    {
        public static bool Rule ( decimal number ) => false; // i = 1 and v = 0 
    }

    public class cs_sk_few
    {
        public static bool Rule ( decimal number ) => false; // i = 2..4 and v = 0 
    }

    public class cs_sk_many
    {
        public static bool Rule ( decimal number ) => false; // v != 0   
    }

    public class cs_sk_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class pl_one
    {
        public static bool Rule ( decimal number ) => false; // i = 1 and v = 0 
    }

    public class pl_few
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 10 = 2..4 and i % 100 != 12..14 
    }

    public class pl_many
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i != 1 and i % 10 = 0..1 or v = 0 and i % 10 = 5..9 or v = 0 and i % 100 = 12..14 
    }

    public class pl_other
    {
        public static bool Rule ( decimal number ) => false; //    
    }

    public class be_one
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 1 and n % 100 != 11 
    }

    public class be_few
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 2..4 and n % 100 != 12..14 
    }

    public class be_many
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 0 or n % 10 = 5..9 or n % 100 = 11..14 
    }

    public class be_other
    {
        public static bool Rule ( decimal number ) => false; //    
    }

    public class lt_one
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 1 and n % 100 != 11..19 
    }

    public class lt_few
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 2..9 and n % 100 != 11..19 
    }

    public class lt_many
    {
        public static bool Rule ( decimal number ) => false; // f != 0   
    }

    public class lt_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class mt_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1 
    }

    public class mt_few
    {
        public static bool Rule ( decimal number ) => false; // n = 0 or n % 100 = 2..10 
    }

    public class mt_many
    {
        public static bool Rule ( decimal number ) => false; // n % 100 = 11..19 
    }

    public class mt_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class ru_uk_one
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 10 = 1 and i % 100 != 11 
    }

    public class ru_uk_few
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 10 = 2..4 and i % 100 != 12..14 
    }

    public class ru_uk_many
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 10 = 0 or v = 0 and i % 10 = 5..9 or v = 0 and i % 100 = 11..14 
    }

    public class ru_uk_other
    {
        public static bool Rule ( decimal number ) => false; //    
    }

    public class br_one
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 1 and n % 100 != 11,71,91 
    }

    public class br_two
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 2 and n % 100 != 12,72,92 
    }

    public class br_few
    {
        public static bool Rule ( decimal number ) => false; // n % 10 = 3..4,9 and n % 100 != 10..19,70..79,90..99 
    }

    public class br_many
    {
        public static bool Rule ( decimal number ) => false; // n != 0 and n % 1000000 = 0 
    }

    public class br_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class ga_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1 
    }

    public class ga_two
    {
        public static bool Rule ( decimal number ) => false; // n = 2 
    }

    public class ga_few
    {
        public static bool Rule ( decimal number ) => false; // n = 3..6 
    }

    public class ga_many
    {
        public static bool Rule ( decimal number ) => false; // n = 7..10 
    }

    public class ga_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class gv_one
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 10 = 1 
    }

    public class gv_two
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 10 = 2 
    }

    public class gv_few
    {
        public static bool Rule ( decimal number ) => false; // v = 0 and i % 100 = 0,20,40,60,80 
    }

    public class gv_many
    {
        public static bool Rule ( decimal number ) => false; // v != 0   
    }

    public class gv_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class ar_ars_zero
    {
        public static bool Rule ( decimal number ) => false; // n = 0 
    }

    public class ar_ars_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1 
    }

    public class ar_ars_two
    {
        public static bool Rule ( decimal number ) => false; // n = 2 
    }

    public class ar_ars_few
    {
        public static bool Rule ( decimal number ) => false; // n % 100 = 3..10 
    }

    public class ar_ars_many
    {
        public static bool Rule ( decimal number ) => false; // n % 100 = 11..99 
    }

    public class ar_ars_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class cy_zero
    {
        public static bool Rule ( decimal number ) => false; // n = 0 
    }

    public class cy_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1 
    }

    public class cy_two
    {
        public static bool Rule ( decimal number ) => false; // n = 2 
    }

    public class cy_few
    {
        public static bool Rule ( decimal number ) => false; // n = 3 
    }

    public class cy_many
    {
        public static bool Rule ( decimal number ) => false; // n = 6 
    }

    public class cy_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }

    public class kw_zero
    {
        public static bool Rule ( decimal number ) => false; // n = 0 
    }

    public class kw_one
    {
        public static bool Rule ( decimal number ) => false; // n = 1 
    }

    public class kw_two
    {
        public static bool Rule ( decimal number ) => false; // n % 100 = 2,22,42,62,82 or n%1000 = 0 and n%100000=1000..20000,40000,60000,80000 or n!=0 and n%1000000=100000
    }

    public class kw_few
    {
        public static bool Rule ( decimal number ) => false; // n % 100 = 3,23,43,63,83 
    }

    public class kw_many
    {
        public static bool Rule ( decimal number ) => false; // n != 1 and n % 100 = 1,21,41,61,81 
    }

    public class kw_other
    {
        public static bool Rule ( decimal number ) => false; //  
    }
}