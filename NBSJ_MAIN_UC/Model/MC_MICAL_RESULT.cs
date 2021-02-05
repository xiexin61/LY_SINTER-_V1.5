using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSJ_MAIN_UC.Model
{
    public class MC_MICAL_RESULT
    {

        public DateTime TIMESTAMP { get; set; }///时间
        public double DATANUM{get;set;}///记录编码
    public DateTime MICAL_MATCH_TIME { get; set; }///配料室记录时间
    public double MICAL_MATCH_BLO_RATIO_DRY {get;set;}///混匀矿配比（干）
    public double MICAL_MATCH_DRF_RATIO_1_DRY {get;set;}///高返配比（干）
    public double MICAL_MATCH_LIMS_RATIO_DRY {get;set;}///石灰石配比（干）
    public double MICAL_MATCH_SLIMS_RATIO_DRY {get;set;}///白云石配比（干）
    public double MICAL_MATCH_QLIME_RATIO_DRY { get;set;}///生石灰配比（干）
    public double MICAL_MATCH_COKE_RATIO_DRY{get;set;}///燃料配比（干）
public double MICAL_MATCH_RE_RATIO_DRY{get;set;}///烧返配比（干）
public double MICAL_MATCH_DUST_RATIO_DRY{get;set;}///除尘灰配比（干）
public double MICAL_MATCH_BLO_RATIO_WET{get;set;}///混匀矿下料百分比（湿）
public double MICAL_MATCH_DRF_RATIO_1_WET{get;set;}///高返下料百分比（湿）
public double MICAL_MATCH_LIMS_RATIO_WET{get;set;}///石灰石下料百分比（湿）
public double MICAL_MATCH_SLIMS_RATIO_WET{get;set;}///白云石下料百分比（湿）
public double MICAL_MATCH_QLIME_RATIO_WET{get;set;}///生石灰下料百分比（湿）
public double MICAL_MATCH_COKE_RATIO_WET{get;set;}///燃料下料百分比（湿）
public double MICAL_MATCH_RE_RATIO_WET{get;set;}///烧返下料百分比（湿）
public double MICAL_MATCH_DUST_RATIO_WET{get;set;}///除尘灰下料百分比（湿）
public double MICAL_MATCH_TFE { get;set;}///配料混合料TFe
public double MICAL_MATCH_CAO { get;set;}///配料混合料CaO
public double MICAL_MATCH_FEO { get;set;}///配料混合料FeO
public double MICAL_MATCH_SIO2  { get;set;}///配料混合料SiO2
public double MICAL_MATCH_MGO { get;set;}///配料混合料MgO
public double MICAL_MATCH_AL2O3 { get;set;}///配料混合料Al2O3
public double MICAL_MATCH_R { get;set;}///配料混合料碱度
public double MICAL_MATCH_C  { get;set;}///配料混合料含碳
public double MICAL_MATCH_TOTAL_SP { get;set;}///总料量SP
public double MICAL_MATCH_TOTAL_PV{get;set;}///总料量PV
public double MICAL_MATCH_TOTAL_DRY{get;set;}///总干料量设定
public DateTime MICAL_BLEND_1M_TIME { get; set; }///一混记录时间
public double MICAL_BLEND_BELT_W_1H1{get;set;}///一混前转运皮带秤称重值
public double MICAL_BLEND_1M_RATE{get;set;}///一混转速
public double MICAL_BLEND_1M_TARGET_WATER{get;set;}///一混目标水分
public double MICAL_BLEND_1M_FT_SP{get;set;}///一混设定加水量
public double MICAL_BLEND_1M_FT_PV{get;set;}///一混实际加水量
public double MICAL_BLEND_1M_NEX_WAT_AVG{get;set;}///一混后检测水分
public double MICAL_1M_MAT_TIME{get;set;}///一混到配料倒推时间耗时（1）
public DateTime MICAL_BLEND_2M_TIME { get; set; }///二混记录时间
public double MICAL_BLEND_BELT_W_2H1{get;set;}///一混后转运皮带秤称重值
public double MICAL_BLEND_2M_RATE{get;set;}///二混转速
public double MICAL_BLEND_2M_TARGET_WATER{get;set;}///二混目标水分
public double MICAL_BLEND_2M_FLOW_SP{get;set;}///二混设定加水量
public double MICAL_BLEND_2M_FLOW_PV{get;set;}///二混实际加水量
public double MICAL_BLEND_2M_NEX_WAT_AVG{get;set;}///二混后检测水分
public double MICAL_2M_1M_TIME{get;set;}///二混到一混倒推时间耗时（2）
public DateTime MICAL_CLOTH_DST_TIME { get; set; }///布料记录时间
public double MICAL_CLOTH_STICK_PV_RATE{get;set;}///圆辊转速
public double MICAL_CLOTH_THICK_AVG{get;set;}///烧结料厚
public double MICAL_CLOTH_BL_TAR_LEVEL{get;set;}///混合料槽料位
public double MICAL_CLOTH_2M_W_Z2_1{get;set;}///Z2-1皮带秤值
public double MICAL_BL_BUNK_TIME{get;set;}///目标料批在混料槽下料耗时
public double MICAL_CLO_2M_TIME{get;set;}///布料到二混倒推时间耗时（从1号台车开始位置到二混）（3）
public DateTime MICAL_BU_C_SIN_TIME { get; set; }///烧结记录时间
public double MICAL_BU_C_IG_AIM_TE{get;set;}///点火温度
public double MICAL_BU_C_IG_GAS_PV{get;set;}///煤气流量
public double MICAL_BU_C_IG_AIR_PV{get;set;}///空气流量
public double MICAL_BU_C_AIM_AFR{get;set;}///空燃比
public double MICAL_BU_C_SIN_MS_SP{get;set;}///烧结机设定机速
public double MICAL_BU_C_SIN_MS_PV{get;set;}///烧结机实际机速
public double MICAL_MA_SB_FLUE_TE{get;set;}///主抽大烟道温度 （两个平均值）
public double MICAL_MA_SB_FLUE_PT{get;set;}///主抽大烟道压力（两个平均值）
public double MICAL_MA_SB_FLUE_FT{get;set;}///主抽大烟道流量（两个之和）
public double MICAL_BREATH{get;set;}///透气性指数
public double MICAL_BU_C_LOCAT_BRP{get;set;}///BRP位置
public double MICAL_BU_C_BRP_TE{get;set;}///BRP温度
public double MICAL_BU_C_LOCAT_BTP{get;set;}///BTP位置
public double MICAL_BU_C_BTP_TE{get;set;}///BTP温度
public double MICAL_SIN_U_TIME{get;set;}///烧结工序耗时
public DateTime MICAL_BU_C_RC_TIME { get; set; }///环冷记录时间
public double MICAL_BU_C_RC_B_IN_TE{get;set;}///入口温度
public double MICAL_BU_C_RC_B_OUT_TE{get;set;}///出口温度
public double MICAL_BU_C_RC_SPEED_SP{get;set;}///环冷机设定机速
public double MICAL_BU_C_RC_SPEED_PV{get;set;}///环冷机实际机速
public double MICAL_RC_U_TIME{get;set;}///环冷工序耗时
public DateTime MICAL_BU_C_SCREEN_TIME { get; set; }///筛分记录时间
public double MICAL_BU_C_BED_MATE_AO_FT{get;set;}///铺底料皮带秤值
public double MICAL_BU_C_COLD_AO_FT{get;set;}///返矿皮带秤值
public double MICAL_BU_C_FP_BELT_FT_AVG{get;set;}///成品皮带秤
public double MICAL_SCR_RC_TIME{get;set;}///筛分到环冷倒推时间耗时
public DateTime MICAL_BU_C_SAMP_TIME {get;set;}///取样记录时间
public double MICAL_BU_C_FP_RATE { get; set; }///成品率计算
public double MICAL_SAM_SCR_TIME{get;set;}///取样到筛分倒推时间耗时
public double MICAL_SAM_MAT_TIME{get;set;}///流程总耗时
public double MICAL_M_STATE_FLAG{get;set;}///模型计算状态（1：正常计算；2：异常停机计算；3：程序第一次运行计算；4：模型计算停止）
public double MICAL_RC_STATE_FLAG{get;set;}///环冷机状态（1：环冷机运转时间计算准确；0：环冷机运转时间大概计算）
public double MICAL_SIN_STATE_FLAG{get;set;}///烧结机状态（1：烧结机运转时间计算准确；0：烧结机运转时间大概计算）

        public  DateTime SYSTIME { get; set; }


    }
}
