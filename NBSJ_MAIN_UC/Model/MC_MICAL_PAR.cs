using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSJ_MAIN_UC.Model
{
    public class MC_MICAL_PAR
    {
        public DateTime TIMESTAMP { get; set; }///插入时间
        public float PAR_MAT_T{get;set;}///配料总皮带运行耗时
        public float PAR_B_1H_T{get;set;}///进一混皮带运行耗时
        public float PAR_1H_T{get;set;}///混合料在一混内耗时
        public float PAR_1H_2H_T{get;set;}///出一混、进二混皮带运行耗时
        public float PAR_2H_T{get;set;}///混合料在二混内耗时
        public float PAR_A_2H_T{get;set;}///出二混、进梭式布料器运行耗时
        public float PAR_STICK_VEHICLE_1{get;set;}///混合料从圆辊到达1号台车运行耗时
        public float PAR_BREAK_T{get;set;}///烧结矿破碎过程耗时
        public float PAR_PF_T{get;set;}///烧结矿在板式给矿机内运行耗时
        public float PAR_PF_SCREEN_T{get;set;}///板式给矿机出口到达筛分运行耗时
        public float PAR_SCREEN_T{get;set;}///烧结矿筛分过程耗时
        public float PAR_SCREEN_SAMPLE_T{get;set;}///成品矿出筛分到达取样点运行耗时
        public float PAR_LENGTH_B01{get;set;}///1#风箱的宽度
        public float PAR_LENGTH_B02{get;set;}///2#风箱的宽度
        public float PAR_LENGTH_B03{get;set;}///3#风箱的宽度
        public float PAR_LENGTH_B04{get;set;}///4#风箱的宽度
        public float PAR_LENGTH_B05{get;set;}///5#风箱的宽度
        public float PAR_LENGTH_B06{get;set;}///6#风箱的宽度
        public float PAR_LENGTH_B07{get;set;}///7#风箱的宽度
        public float PAR_LENGTH_B08{get;set;}///8#风箱的宽度
        public float PAR_LENGTH_B09{get;set;}///9#风箱的宽度
        public float PAR_LENGTH_B10{get;set;}///10#风箱的宽度
        public float PAR_LENGTH_B11{get;set;}///11#风箱的宽度
        public float PAR_LENGTH_B12{get;set;}///12#风箱的宽度
        public float PAR_LENGTH_B13{get;set;}///13#风箱的宽度
        public float PAR_LENGTH_B14{get;set;}///14#风箱的宽度
        public float PAR_LENGTH_B15{get;set;}///15#风箱的宽度
        public float PAR_LENGTH_B16{get;set;}///16#风箱的宽度
        public float PAR_LENGTH_B17{get;set;}///17#风箱的宽度
        public float PAR_LENGTH_B18{get;set;}///18#风箱的宽度
        public float PAR_LENGTH_B19{get;set;}///19#风箱的宽度
        public float PAR_LENGTH_B20{get;set;}///20#风箱的宽度
        public float PAR_LENGTH_B21{get;set;}///21#风箱的宽度
        public float PAR_LENGTH_B22{get;set;}///22#风箱的宽度
        public float PAR_LENGTH_B23{get;set;}///23#风箱的宽度
        public float PAR_LENGTH_SIN{get;set;}///烧结机长度
        public float PAR_TROLLEY_L{get;set;}///烧结机台车宽度
        public float PAR_LENGTH_RC{get;set;}///环冷机周长
        public float PAR_BL_DENSITY{get;set;}///混合料密度
        /// <summary>
        /// Desc:铁料料仓上限 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public float PAR_IRON_BUNK_UP{get;set;}///铁料料仓上限
         /// <summary>
        /// Desc:溶剂料仓上限 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public float PAR_SOL_BUNK_UP{get;set;}///溶剂料仓上限
             /// <summary>
        /// Desc:灰类料仓上限 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public float PAR_DUST_BUNK_UP{get;set;}///灰类料仓上限
             /// <summary>
        /// Desc:燃料料仓上限 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public float PAR_FUEL_BUNK_UP{get;set;}///燃料料仓上限
        public float PAR_T1{get;set;}///料流追踪计算周期
        public float PAR_T2{get;set;}///指导采样校准周期
        public float PAR_AVG_T{get;set;}///采集值取平均时间段

    }
}

