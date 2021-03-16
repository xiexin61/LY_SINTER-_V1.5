using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSJ_MAIN_UC.Model
{
    public class C_PLC_3S
    {

        /// <summary>
        /// Desc:时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime TIMESTAMP { get; set; }

        public float? T_TOTAL_SP_W_3S { get; set; }///总料量SP
        public float? T_TOTAL_PV_W_3S { get; set; }///总料量PV
        public float T_W_1_3S { get; set; }///1#配料仓仓位
        public float T_W_2_3S { get; set; }///2#配料仓仓位
        public float T_W_3_3S { get; set; }///3#配料仓仓位
        public float T_W_4_3S { get; set; }///4#配料仓仓位
        public float T_W_5_3S { get; set; }///5#配料仓仓位
        public float T_W_6_3S { get; set; }///6#配料仓仓位
        public float T_W_7_3S { get; set; }///7#配料仓仓位
        public float T_W_8_3S { get; set; }///8#配料仓仓位
        public float T_W_9_3S { get; set; }///9#配料仓仓位
        public float T_W_10_3S { get; set; }///10#配料仓仓位
        public float T_W_11_3S { get; set; }///11#配料仓仓位
        public float T_W_12_3S { get; set; }///12#配料仓仓位
        public float T_W_13_3S { get; set; }///13#配料仓仓位
        public float T_W_14_3S { get; set; }///14#配料仓仓位
        public float T_W_15_3S { get; set; }///15#配料仓仓位
        public float T_W_16_3S { get; set; }///16#配料仓仓位
        public float T_W_17_3S { get; set; }///17#配料仓仓位
        public float T_W_18_3S { get; set; }///18#配料仓仓位
        public float T_W_19_3S { get; set; }///19#配料仓仓位
        public float T_W_20_3S { get; set; }///20#配料仓仓位
        public float T_SL_1_3S { get; set; }///1#下料口启停信号（1-启用；0-禁用）
        public float T_SL_2_3S { get; set; }///2#下料口启停信号（1-启用；0-禁用）
        public float T_SL_3_3S { get; set; }///3#下料口启停信号（1-启用；0-禁用）
        public float T_SL_4_3S { get; set; }///4#下料口启停信号（1-启用；0-禁用）
        public float T_SL_5_3S { get; set; }///5#下料口启停信号（1-启用；0-禁用）
        public float T_SL_6_3S { get; set; }///6#下料口启停信号（1-启用；0-禁用）
        public float T_SL_7_3S { get; set; }///7#下料口启停信号（1-启用；0-禁用）
        public float T_SL_8_3S { get; set; }///8#下料口启停信号（1-启用；0-禁用）
        public float T_SL_9_3S { get; set; }///9#下料口启停信号（1-启用；0-禁用）
        public float T_SL_10_3S { get; set; }///10#下料口启停信号（1-启用；0-禁用）
        public float T_SL_11_3S { get; set; }///11#下料口启停信号（1-启用；0-禁用）
        public float T_SL_12_3S { get; set; }///12#下料口启停信号（1-启用；0-禁用）
        public float T_SL_13_3S { get; set; }///13#下料口启停信号（1-启用；0-禁用）
        public float T_SL_14_3S { get; set; }///14#下料口启停信号（1-启用；0-禁用）
        public float T_SL_15_3S { get; set; }///15#下料口启停信号（1-启用；0-禁用）
        public float T_SL_16_3S { get; set; }///16#下料口启停信号（1-启用；0-禁用）
        public float T_SL_17_3S { get; set; }///17#下料口启停信号（1-启用；0-禁用）
        public float T_SL_18_3S { get; set; }///18#下料口启停信号（1-启用；0-禁用）
        public float T_SL_19_3S { get; set; }///19#下料口启停信号（1-启用；0-禁用）
        public float T_SL_20_3S { get; set; }///20#下料口启停信号（1-启用；0-禁用）
        public float T_SP_W_1_3S { get; set; }///1#下料口设定下料量
        public float T_SP_W_2_3S { get; set; }///2#下料口设定下料量
        public float T_SP_W_3_3S { get; set; }///3#下料口设定下料量
        public float T_SP_W_4_3S { get; set; }///4#下料口设定下料量
        public float T_SP_W_5_3S { get; set; }///5#下料口设定下料量
        public float T_SP_W_6_3S { get; set; }///6#下料口设定下料量
        public float T_SP_W_7_3S { get; set; }///7#下料口设定下料量
        public float T_SP_W_8_3S { get; set; }///8#下料口设定下料量
        public float T_SP_W_9_3S { get; set; }///9#下料口设定下料量
        public float T_SP_W_10_3S { get; set; }///10#下料口设定下料量
        public float T_SP_W_11_3S { get; set; }///11#下料口设定下料量
        public float T_SP_W_12_3S { get; set; }///12#下料口设定下料量
        public float T_SP_W_13_3S { get; set; }///13#下料口设定下料量
        public float T_SP_W_14_3S { get; set; }///14#下料口设定下料量
        public float T_SP_W_15_3S { get; set; }///15#下料口设定下料量
        public float T_SP_W_16_3S { get; set; }///16#下料口设定下料量
        public float T_SP_W_17_3S { get; set; }///17#下料口设定下料量
        public float T_SP_W_18_3S { get; set; }///18#下料口设定下料量
        public float T_SP_W_19_3S { get; set; }///19#下料口设定下料量
        public float T_SP_W_20_3S { get; set; }///20#下料口设定下料量
        public float T_ACTUAL_W_1_3S { get; set; }///1#下料口实际下料量
        public float T_ACTUAL_W_2_3S { get; set; }///2#下料口实际下料量
        public float T_ACTUAL_W_3_3S { get; set; }///3#下料口实际下料量
        public float T_ACTUAL_W_4_3S { get; set; }///4#下料口实际下料量
        public float T_ACTUAL_W_5_3S { get; set; }///5#下料口实际下料量
        public float T_ACTUAL_W_6_3S { get; set; }///6#下料口实际下料量
        public float T_ACTUAL_W_7_3S { get; set; }///7#下料口实际下料量
        public float T_ACTUAL_W_8_3S { get; set; }///8#下料口实际下料量
        public float T_ACTUAL_W_9_3S { get; set; }///9#下料口实际下料量
        public float T_ACTUAL_W_10_3S { get; set; }///10#下料口实际下料量
        public float T_ACTUAL_W_11_3S { get; set; }///11#下料口实际下料量
        public float T_ACTUAL_W_12_3S { get; set; }///12#下料口实际下料量
        public float T_ACTUAL_W_13_3S { get; set; }///13#下料口实际下料量
        public float T_ACTUAL_W_14_3S { get; set; }///14#下料口实际下料量
        public float T_ACTUAL_W_15_3S { get; set; }///15#下料口实际下料量
        public float T_ACTUAL_W_16_3S { get; set; }///16#下料口实际下料量
        public float T_ACTUAL_W_17_3S { get; set; }///17#下料口实际下料量
        public float T_ACTUAL_W_18_3S { get; set; }///18#下料口实际下料量
        public float T_ACTUAL_W_19_3S { get; set; }///19#下料口实际下料量
        public float T_ACTUAL_W_20_3S { get; set; }///20#下料口实际下料量
        public float T_BELT_SL_P_7_3S { get; set; }///配料总皮带启停信号（1H1-1)
        public float? T_1M_PRE_BELT_B_E_3S { get; set; }///一混前皮带启停信号(皮带号：1H1-1)
        public float? T_1M_PRE_BELT_W_1H_1_3S { get; set; }///一混前转运皮带秤值(皮带号：1H1-1)
        public float? T_1M_SL_3S { get; set; }///一混设备启停信号（启-1；停-0）
        public float? T_1M_FT_SP_3S { get; set; }///一混加水流量设定值
        public float? T_1M_FT_PV_3S { get; set; }///一混加水流量反馈值
        public float? T_1M_MIXER_RATE_3S { get; set; }///一混混合机转速/频率
        public float? T_1M_NEX_WATER_AVG_3S { get; set; }///一混后混合料水分检测(皮带号：Z2-1)
        public float? T_1M_NEX_BELT_B_E_Z2_1_3S { get; set; }///一混后皮带启停信号(皮带号：Z2-1)
        public float? T_1M_NEX_BELT_W_1H2_1_3S { get; set; }///一混后皮带秤值(皮带号：1H2-1)
        public float? T_1M_NEX_BELT_B_E_1H2_1_3S { get; set; }///一混后皮带启停信号(1H2-1)
        public float? T_2M_SL_3S { get; set; }///二混设备启停信号
        public float? T_2M_FLOW_SP_3S { get; set; }///二混加水流量设定值
        public float? T_2M_FLOW_PV_3S { get; set; }///二混加水流量反馈值
        public float? T_2M_MIXER_RATE_3S { get; set; }///二混混合机转速/频率
        public float? T_2M_BELT_VALUE_3S { get; set; }///二混后转运皮带秤值
        public float? T_2M_NEX_BELT_1H2_2_S_3S { get; set; }///二混后皮带启停信号(皮带号：1H2-2)
        public float? T_2M_NEX_WATER_AVG_3S { get; set; }///二混后混合料水分检测(皮带号：1H2-2)
        public float? T_2M_NEX_BELT_Z3_1_S_3S { get; set; }///二混后皮带启停信号(Z3-1)
        public float? T_2M_NEX_BELT_Z15_1_S_3S { get; set; }///二混后皮带启停信号(Z15-1)
        public float? T_IN_SK_S_3S { get; set; }///进梭式布料器皮带启停信号(皮带号：1S-1)
        public float? T_SHUTTLE_S_S_3S { get; set; }///梭式布料器皮带启停信号((皮带号：1BL)
        public double T_BLEND_LEVEL_3S { get; set; }///混合料仓仓位
        public double T_BED_MAT_W_3S { get; set; }///铺底料仓称重
        public float? T_STICK_SL_3S { get; set; }///圆辊给料机启停信号
        public float? T_STICK_SP_3S { get; set; }///圆辊给料机设定转速
        public float? T_STICK_PV_3S { get; set; }///圆辊给料机反馈转速
        public float? T_N_STICK_A_SL_3S { get; set; }///九辊布料器启停信号
        public float? T_IG_01_TE_3S { get; set; }///点火段温度1
        public float? T_IG_02_TE_3S { get; set; }///点火段温度2
        public float? T_IG_03_TE_3S { get; set; }///点火段温度3
        public float? T_IG_GAS_SP_3S { get; set; }///点火段煤气流量设定值
        public float? T_IG_GAS_PV_3S { get; set; }///点火段煤气流量反馈值
        public float? T_IG_AIR_SP_3S { get; set; }///点火段空气流量设定值
        public float? T_IG_AIR_PV_3S { get; set; }///点火段空气流量反馈值
        public float? T_SIN_SL_3S { get; set; }///烧结机启停信号
        public float? T_SIN_MS_SP_3S { get; set; }///烧结机机速设定值
        public float? T_SIN_MS_PV_3S { get; set; }///烧结机机速反馈值
        public float? T_FAN_1_SL_3S { get; set; }///1#主抽风机启停信号
        public float? T_FAN_2_SL_3S { get; set; }///2#主抽风机启停信号
        public float? T_MA_SB_1_FLUE_FT_3S { get; set; }///1#主抽大烟道废气流量
        public float? T_MA_SB_2_FLUE_FT_3S { get; set; }///2#主抽大烟道废气流量
        public float? T_MA_SB_1_FLUE_PT_3S { get; set; }///1#主抽大烟道压力
        public float? T_MA_SB_2_FLUE_PT_3S { get; set; }///2#主抽大烟道压力
        public float? T_MA_SB_1_FLUE_TE_3S { get; set; }///1#主抽大烟道温度
        public float? T_MA_SB_2_FLUE_TE_3S { get; set; }///2#主抽大烟道温度
        public float? T_BM_SL_3S { get; set; }///单辊破碎机启停信号
        public float? T_RC_SL_3S { get; set; }///环冷机启停信号
        public float? T_RC_SPEED_SP_3S { get; set; }///环冷机机速设定值
        public float? T_RC_SPEED_PV_3S { get; set; }///环冷机机速反馈值
        public float? T_RC_IN_TE_3S { get; set; }///环冷机入口温度
        public float? T_RC_OUT_TE_3S { get; set; }///环冷机出口温度（或烧结矿冷却后温度）
        public float? T_RC_B_S_1_3S { get; set; }///环冷机1号鼓风机启停信号
        public float? T_RC_B_S_2_3S { get; set; }///环冷机2号鼓风机启停信号
        public float? T_RC_B_S_3_3S { get; set; }///环冷机3号鼓风机启停信号
        public float? T_RC_B_S_4_3S { get; set; }///环冷机4号鼓风机启停信号
        public float? T_RC_B_S_5_3S { get; set; }///环冷机5号鼓风机启停信号
        public float? T_PF_SL_3S { get; set; }///板式给矿机启停信号
        public float? T_PF_LEVEL_3S { get; set; }///板式给矿机仓位
        public float? T_MS_IN_SF_1_SL_3S { get; set; }///进料筛皮带启停信号(皮带号：Z4-1)
        public float? T_MS_IN_LS_1_SL_3S { get; set; }///进料筛皮带启停信号(皮带号：LS1-1)
        public float? T_SCREEN_SL_1_3S { get; set; }///筛一A启停信号
        public float? T_SCREEN_SL_2_3S { get; set; }///筛二A启停信号
        public float? T_SCREEN_SL_3_3S { get; set; }///筛三A启停信号
        public float? T_SCREEN_SL_1_B_3S { get; set; }///筛一B启停信号
        public float? T_SCREEN_SL_2_B_3S { get; set; }///筛二B启停信号
        public float? T_SCREEN_SL_3_B_3S { get; set; }///筛三B启停信号
        public float? T_FP_BELT1_SL_3S { get; set; }///1#成品皮带启停信号(皮带号：Z5-1)
        public float? T_FP_BELT2_SL_3S { get; set; }///2#成品皮带启停信号(皮带号：QY-2)
        public float? T_FP_BELT4_SL_3S { get; set; }///4#成品皮带启停信号(皮带号：QY-1)
        public float? T_COLD_AO_SF1_SL_3S { get; set; }///1#冷返矿皮带启停信号(皮带号：1Z10)
        public float? T_COLD_AO_SF2_SL_3S { get; set; }///2#冷返矿皮带启停信号(皮带号：1Z11)
        public float? T_COLD_AO_SF3_SL_3S { get; set; }///3#冷返矿皮带启停信号(皮带号：1P15)
        public float? T_BED_MAT_1_SL_3S { get; set; }///1#铺底料皮带启停信号(皮带号：Z6-1)
        public float? T_BED_MAT_2_SL_3S { get; set; }///2#铺底料皮带启停信号(皮带号：Z8-1)
        public float? T_BED_MAT_3_SL_3S { get; set; }///3#铺底料皮带启停信号(皮带号：1S-2)
        public float? T_PROD_DELT1_FQ_3S { get; set; }///成品皮带秤值（z5-1）
        public float? T_COLD_AO_FT_3S { get; set; }///冷返矿皮带秤值（1P15）
        public float? T_BED_MAT_AO_FT_3S { get; set; }///铺底料皮带秤值（1S-2）
        public float? T_PLC_1M_WATER_SP_3S { get; set; }//一混目标水分
        public float? C_THICK_SP_3S { get; set; }//布料厚度
        public float? C_THICK_PV_3S { get; set; }//布料厚度
        public float? T_PF_SPEED_SP_3S { get; set; }//板式给矿机设定转速/皮带(皮带号：***)
        public float? T_PF_SPEED_PV_3S { get; set; }//板式给矿机反馈转速/皮带(皮带号：***)
        public float? F_PLC_BLEND_TE { get; set; }//混合料温度
        public float? T_FP_BELT3_SL_3S { get; set; }//3#成品皮带启停信号（CP1（成-5））
        public float? F_PLC_BLEND_TE_3S { get; set; }//混合料温度检测值
    }
}