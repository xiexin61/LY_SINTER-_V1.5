using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MIXHMICAL
{
    class Program
    {
        public static int DelayTime = 60000;

        //public static HMICAL vHmiCal = new HMICAL();
        public static LGSinter vHmiCal = new LGSinter();
        static void Main(string[] args)
        {
            //配料主程
            DBSQL _vdb = new DBSQL(vHmiCal._connstring);
            //传入四个参数  C目标值 碱度目标值 C调整值 碱度调整值(这四个调整值由界面传入) 计算配比计算（两个配比）
            //
            //Tuple<int, float, float> pb = vHmiCal.CptSolfuels(3,1.5f,0,-0.5f); //OK
            //int a = vHmiCal.CalPB(1); //计算设定百分比  OK
            //int b = vHmiCal.CalPB(2); //计算当前配比和当前百分比  OK
            //分仓系数不用计算，此处跳过测试
            //计算干下料比例 
            //int c =vHmiCal.FeedBLCompute(15, 13.6227f);   //计算干下料比例 %FeedBLCompute,传入配比ID,当前配比% 计算干下料比例 OK
            //FeedLLCompute 计算设定下料量
            //int d = vHmiCal.FeedLLCompute(900);
            //下料偏差计算
            //vHmiCal.diffNormal();
            //湿配比计算
            //  vHmiCal.WetPB();
            while (true)
            {


                //1、SP
                var _Sp = vHmiCal.CalculateSinterBySP();

                if (_Sp.Item1 != 0)
                {
                    vHmiCal.mixlog.writelog("设定成分计算错误", -1);
                }

                //2、PV
                var _Pv = vHmiCal.CalculateSinterByPV();
                if (_Pv.Item1 != 0)
                {
                    vHmiCal.mixlog.writelog("实际成分计算错误", -1);
                }
                //3、新增插入数据  20200322

                var _NewAddData = vHmiCal.ModifyData();

                if (!_NewAddData.Item1)
                {
                    vHmiCal.mixlog.writelog("新增模型数据获取失败,表M_MACAL_INTERFACE_RESULT", -1);
                }
                //20200911 李涛更新
                //插入数据库
                if (_Sp.Item1 == 0 && _Pv.Item1 == 0 && _NewAddData.Item1)
                {
                    string sql_insert_spv = "insert into MC_MIXCAL_RESULT_1MIN(TIMESTAMP," +
                    "SINCAL_SIN_SP_TFE,SINCAL_SIN_SP_FEO,SINCAL_SIN_SP_CAO,SINCAL_SIN_SP_SIO2," +
                    "SINCAL_SIN_SP_AL2O3,SINCAL_SIN_SP_MGO,SINCAL_SIN_SP_S,SINCAL_SIN_SP_P,SINCAL_SIN_SP_MN," +
                    "SINCAL_SIN_SP_R,SINCAL_SIN_SP_TIO2,SINCAL_SIN_SP_K2O,SINCAL_SIN_SP_NA2O,SINCAL_SIN_SP_PBO," +
                    "SINCAL_SIN_SP_ZNO,SINCAL_SIN_SP_F,SINCAL_SIN_SP_AS,SINCAL_SIN_SP_CU,SINCAL_SIN_SP_PB,SINCAL_SIN_SP_ZN," +
                    "SINCAL_SIN_SP_K,SINCAL_SIN_SP_NA,SINCAL_SIN_SP_CR,SINCAL_SIN_SP_NI,SINCAL_SIN_SP_MNO,SINCAL_SIN_SP_SPARE1," +
                    "SINCAL_SIN_SP_SPARE2,SINCAL_SIN_SP_SPARE3,SINCAL_SIN_SP_SPARE4,SINCAL_SIN_SP_SPARE5,SINCAL_SIN_SP_SPARE6," +
                    "SINCAL_MIX_SP_LOT,SINCAL_MIX_SP_H2O_1,SINCAL_MIX_SP_H2O_2,SINCAL_MIX_SP_FeO,SINCAL_MIX_SP_C," +
                    "SINCAL_NON_FUEL_SP_C,SINCAL_FUEL_SP_C,SINCAL_NON_FE_SP_SIO2,SINCAL_DRY_MIX_SP,SINCAL_OUTPUT_SP," +
                    "SINCAL_FUEL_CON_SP,SINCAL_SIN_PV_TFE,SINCAL_SIN_PV_FEO,SINCAL_SIN_PV_CAO,SINCAL_SIN_PV_SIO2," +
                    "SINCAL_SIN_PV_AL2O3,SINCAL_SIN_PV_MGO,SINCAL_SIN_PV_S,SINCAL_SIN_PV_P,SINCAL_SIN_PV_MN,SINCAL_SIN_PV_R," +
                    "SINCAL_SIN_PV_TIO2,SINCAL_SIN_PV_K2O,SINCAL_SIN_PV_NA2O,SINCAL_SIN_PV_PBO,SINCAL_SIN_PV_ZNO,SINCAL_SIN_PV_F," +
                    "SINCAL_SIN_PV_AS,SINCAL_SIN_PV_CU,SINCAL_SIN_PV_PB,SINCAL_SIN_PV_ZN,SINCAL_SIN_PV_K,SINCAL_SIN_PV_NA," +
                    "SINCAL_SIN_PV_CR,SINCAL_SIN_PV_NI,SINCAL_SIN_PV_MNO,SINCAL_SIN_PV_SPARE1,SINCAL_SIN_PV_SPARE2," +
                    "SINCAL_SIN_PV_SPARE3,SINCAL_SIN_PV_SPARE4,SINCAL_SIN_PV_SPARE5,SINCAL_MIX_PV_LOT," +
                    "SINCAL_MIX_PV_H2O_1,SINCAL_MIX_PV_H2O_2,SINCAL_MIX_PV_FeO,SINCAL_MIX_PV_C,SINCAL_NON_FUEL_PV_C," +
                    "SINCAL_FUEL_PV_C,SINCAL_NON_FE_PV_SIO2,SINCAL_DRY_MIX_PV,SINCAL_OUTPUT_PV,SINCAL_FUEL_CON_PV," +
                    "SINCAL_BL_RATIO_PV," +
                    //从M_MACAL_INTERFACE_RESULT中获取
                    "SINCAL_L2_CODE_1,SINCAL_L2_CODE_2,SINCAL_L2_CODE_3,SINCAL_L2_CODE_4,SINCAL_L2_CODE_5," +
                    "SINCAL_L2_CODE_6,SINCAL_L2_CODE_7,SINCAL_L2_CODE_8,SINCAL_L2_CODE_9,SINCAL_L2_CODE_10," +
                    "SINCAL_L2_CODE_11,SINCAL_L2_CODE_12,SINCAL_L2_CODE_13,SINCAL_L2_CODE_14,SINCAL_L2_CODE_15," +
                    "SINCAL_L2_CODE_16,SINCAL_L2_CODE_17,SINCAL_L2_CODE_18,SINCAL_L2_CODE_19,SINCAL_L2_CODE_20," +
                    "SINCAL_BLEND_ORE_BILL_DRY,SINCAL_BFES_ORE_BILL_DRY,SINCAL_FLUX_STONE_BILL_DRY," +
                    "SINCAL_DOLOMATE_BILL_DRY,SINCAL_FLUX_BILL_DRY,SINCAL_FUEL_BILL_DRY,SINCAL_BRUN_DRY," +
                    "SINCAL_ASH_DUST_BILL_DRY,SINCAL_9_BILL_DRY," +
                    "SINCAL_10_BILL_DRY,SINCAL_11_BILL_DRY,SINCAL_12_BILL_DRY,SINCAL_13_BILL_DRY,SINCAL_14_BILL_DRY,SINCAL_15_16_BILL_DRY," +
                    "SINCAL_17_BILL_DRY,SINCAL_18_BILL_DRY,SINCAL_19_BILL_DRY,SINCAL_20_BILL_DRY," +
                     "SINCAL_C_A,SINCAL_R_A,SINCAL_MG_A,SINCAL_R_C,SINCAL_C_DC,SINCAL_MG_C,SINCAL_SUM_MIX_SP,SINCAL_SUM_MIX_PV," +
                    "SINCAL_BLEND_ORE_BILL_WET,SINCAL_BFES_ORE_BILL_WET,SINCAL_FLUX_STONE_BILL_WET," +
                    "SINCAL_DOLOMATE_BILL_WET,SINCAL_FLUX_BILL_WET,SINCAL_FUEL_BILL_WET," +
                    "SINCAL_BRUN_BILL_WET,SINCAL_ASH_DUST_BILL_WET,SINCAL_9_BILL_WET," +
                     "SINCAL_10_BILL_WET,SINCAL_11_BILL_WET,SINCAL_12_BILL_WET,SINCAL_13_BILL_WET,SINCAL_14_BILL_WET," +
                     "SINCAL_15_16_BILL_WET,SINCAL_17_BILL_WET,SINCAL_18_BILL_WET,SINCAL_19_BILL_WET,SINCAL_20_BILL_WET,SINCAL_MATCH_BFE_W_1," +
                    "SINCAL_MATCH_BFE_W_2,SINCAL_MATCH_RE_W_1,SINCAL_MATCH_RE_W_2" +
                    ")values(";
                    string sql_mid = "'" + DateTime.Now + "',";
                    string sql_end = ")";
                    List<float> _vRs = new List<float>();
                    _vRs.AddRange(_Sp.Item2);
                    _vRs.AddRange(_Pv.Item2);
                    _vRs.AddRange(_NewAddData.Item2);
                    int len = _vRs.Count();
                    for (int i = 0; i < len; i++)
                    {
                        if (i != len - 1)
                            sql_mid += Math.Round(_vRs[i], 4) + ",";
                        else
                            sql_mid += Math.Round(_vRs[i], 4);
                    }
                    string sql_insert = sql_insert_spv + sql_mid + sql_end;
                    int _rs = _vdb.CommandExecuteNonQuery(sql_insert);
                    if (_rs <= 0)
                    {
                        //Console.WriteLine("SP-PV插入数据库表MC_MIXCAL_RESULT_1MIN失败");
                        vHmiCal.mixlog.writelog("SP-PV插入数据库表MC_MIXCAL_RESULT_1MIN失败(" + sql_insert + ")", -1);
                    }
                    else
                    {
                        //Console.WriteLine("SP-PV插入数据库表MC_MIXCAL_RESULT_1MIN成功");
                        vHmiCal.mixlog.writelog("SP-PV插入数据库表MC_MIXCAL_RESULT_1MIN成功", 0);
                    }

                }
                else
                {
                    if (_Sp.Item1 < 0)
                    {
                        vHmiCal.mixlog.writelog("总残存sumRemnant_SP<=0,错误！，需要检查现场设定配比是否正确输入", -1);
                    }
                    if (_Pv.Item1 < 0)
                    {
                        vHmiCal.mixlog.writelog("sumMix_Wet_PV总下料量为0错误; 请查看Read_MC_MEASURE_5MIN 是否5分钟表内没有数据", -1);

                    }
                }

                vHmiCal.initParam();
                //3、R 调整
                try
                {
                    vHmiCal.R_Modify();
                }
                catch (Exception e)
                {
                    vHmiCal.mixlog.writelog(e.Message, -1);
                }
                finally
                {

                }
                //4、C 调整
                try
                {
                    vHmiCal.C_Modify();
                }
                catch (Exception e)
                {
                    vHmiCal.mixlog.writelog(e.Message, -1);
                }
                finally
                {

                }

                //5、Mg 调整
                try
                {
                    vHmiCal.Mg_Modify();
                }
                catch (Exception e)
                {
                    vHmiCal.mixlog.writelog(e.Message, -1);
                }
                finally
                {

                }

                Thread.Sleep(DelayTime);
            }


        }
    }
}
