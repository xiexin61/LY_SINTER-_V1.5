using DataBase;
using MIXHMICAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLog;

namespace LY_SINTER.Model
{
    /// <summary>
    /// 配料模型页面调用 @LT
    /// </summary>
    internal class MIX_PAGE_MODEL
    {
        public vLog _vLog { get; set; }
        private DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);

        public MIX_PAGE_MODEL()
        {
            _vLog = new vLog(".\\Mix_Page_Model\\");
            _vLog.connstring = DataBase.ConstParameters.strCon;
        }

        /// <summary>
        /// 判断上下限
        /// </summary>
        public Tuple<bool, List<float>> Judge_Val()
        {
            var SQL_MC_MIXCAL_PAR = "SELECT PAR_BILL_FUEL_MAX,PAR_BILL_FUEL_MIN,PAR_BILL_LIMESTONE_MAX,PAR_BILL_LIMESTONE_MIN,PAR_BILL_DOLOMITE_MAX,PAR_BILL_DOLOMITE_MIN,PAR_R_A_MAX,PAR_R_A_MIN,PAR_C_A_MAX,PAR_C_A_MIN,PAR_MG_A_MAX,PAR_MG_A_MIN,PAR_R_ADJ_MAX,PAR_R_ADJ_MIN,PAR_C_ADJ_MAX,PAR_C_ADJ_MIN,PAR_MG_ADJ_MAX,PAR_MG_ADJ_MIN,PAR_SUM_MIX_MAX,PAR_SUM_MIX_MIN  FROM MC_MIXCAL_PAR WHERE TIMESTAMP = (SELECT max(TIMESTAMP) from MC_MIXCAL_PAR)";
            DataTable data_MC_MIXCAL_PAR = _dBSQL.GetCommand(SQL_MC_MIXCAL_PAR);
            if (data_MC_MIXCAL_PAR.Rows.Count > 0 && data_MC_MIXCAL_PAR != null)
            {
                List<float> _list = new List<float>();
                //R调整
                float R_TZ_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_R_ADJ_MIN"].ToString());
                _list.Add(R_TZ_MIN);
                float R_TZ_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_R_ADJ_MAX"].ToString());
                _list.Add(R_TZ_MAX);
                //目标R
                float R_MB_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_R_A_MIN"].ToString());
                _list.Add(R_MB_MIN);
                float R_MB_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_R_A_MAX"].ToString());
                _list.Add(R_MB_MAX);
                //C调整
                float C_TZ_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_C_ADJ_MIN"].ToString());
                _list.Add(C_TZ_MIN);
                float C_TZ_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_C_ADJ_MAX"].ToString());
                _list.Add(C_TZ_MAX);
                //目标C
                float C_MB_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_C_A_MIN"].ToString());
                _list.Add(C_MB_MIN);
                float C_MB_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_C_A_MAX"].ToString());
                _list.Add(C_MB_MAX);
                //MG调整
                float MG_TZ_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_MG_ADJ_MIN"].ToString());
                _list.Add(MG_TZ_MIN);
                float MG_TZ_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_MG_ADJ_MAX"].ToString());
                _list.Add(MG_TZ_MAX);
                //目标MG
                float MG_MB_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_MG_A_MIN"].ToString());
                _list.Add(MG_MB_MIN);
                float MG_MB_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_MG_A_MAX"].ToString());
                _list.Add(MG_MB_MAX);
                //总料量sp
                float ZLL_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_SUM_MIX_MIN"].ToString());
                _list.Add(ZLL_MIN);
                float ZLL_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_SUM_MIX_MAX"].ToString());
                _list.Add(ZLL_MAX);
                //燃料
                float RL_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_BILL_FUEL_MIN"].ToString());
                _list.Add(RL_MIN);
                float RL_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_BILL_FUEL_MAX"].ToString());
                _list.Add(RL_MAX);

                //溶剂
                float RJ_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_BILL_LIMESTONE_MIN"].ToString());
                _list.Add(RJ_MIN);
                float RJ_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_BILL_LIMESTONE_MAX"].ToString());
                _list.Add(RJ_MAX);

                //白云石
                float BYS_MIN = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_BILL_DOLOMITE_MIN"].ToString());
                _list.Add(BYS_MIN);
                float BYS_MAX = float.Parse(data_MC_MIXCAL_PAR.Rows[0]["PAR_BILL_DOLOMITE_MAX"].ToString());
                _list.Add(BYS_MAX);

                Tuple<bool, List<float>> _tuple = new Tuple<bool, List<float>>(true, _list);
                return _tuple;
            }
            else
            {
                Tuple<bool, List<float>> _tuple = new Tuple<bool, List<float>>(false, new List<float>());
                return _tuple;
            }
        }

        /// <summary>
        /// 计算溶剂和燃料的配比
        /// matching_signal:调整方式
        /// RJ_MAX：碱度上限
        /// RJ_MIN:碱度下限
        /// RL_MAX：燃料上限
        /// RL_MIN：燃料下限
        /// BYS_MAX：白云石上限
        /// BYS_MIN：白云石下限
        /// </summary>
        public bool CptSolfuel_1(int matching_signal, float RJ_MAX, float RJ_MIN, float RL_MAX, float RL_MIN, float BYS_MAX, float BYS_MIN)
        {
            try
            {
                string sql_cf = "select  top (1) C_Aim,C_Md,R_Aim,R_Md,MG_Aim,MG_Md from  CFG_MAT_L2_MACAL_IDT  order by TIMESTAMP desc";
                DataTable dataTable_cf = _dBSQL.GetCommand(sql_cf);
                if (dataTable_cf.Rows.Count > 0 && dataTable_cf != null)
                {
                    float MBHT = float.Parse(dataTable_cf.Rows[0]["C_Aim"].ToString());//目标含碳
                    float TTZZ = float.Parse(dataTable_cf.Rows[0]["C_Md"].ToString());//碳调整值
                    float MBJD = float.Parse(dataTable_cf.Rows[0]["R_Aim"].ToString());//目标碱度
                    float RTZZ = float.Parse(dataTable_cf.Rows[0]["R_Md"].ToString());//碱度调整值
                    float MBMG = float.Parse(dataTable_cf.Rows[0]["MG_Aim"].ToString());//目标MG
                    float MGZZ = float.Parse(dataTable_cf.Rows[0]["MG_Md"].ToString());//MG调整值
                    LGSinter HMICAL = new LGSinter();
                    // 判断matching_signal字段的计算方式（1：调整熔剂、燃料配比；2：调整熔剂、燃料、白云石配比）
                    if (matching_signal == 1)
                    {
                        string messbox = "配比调整特殊配比为调整熔剂、燃料配比方式";
                        //****计算溶剂和燃料   0非溶剂和燃料、1溶剂、2燃料*****
                        var result = HMICAL.CptSolfuels(MBHT, MBJD, RTZZ, TTZZ);
                        if (result.Item1 >= 0)
                        {
                            if (result.Item2 > RJ_MAX && result.Item2 < RJ_MIN)
                            {
                                messbox += ",溶剂设定配比超限:" + result.Item2.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            if (result.Item3 > RL_MAX && result.Item3 < RL_MIN)
                            {
                                messbox += ",燃料设定配比超限:" + result.Item3.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            //***查询溶剂对应的下料口 （0：非熔剂、非燃料、非烧返、非白云石配比；1：熔剂配比；2：燃料配比；3：烧返配比；4：白云石配比）
                            string sql_solvent = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 1 and a.canghao = b.MAT_L2_CH";
                            //***查询燃料对应的下料口
                            string sql_fuel = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 2 and a.canghao = b.MAT_L2_CH";
                            DataTable dataTable_solvent = _dBSQL.GetCommand(sql_solvent);
                            DataTable dataTable_fuel = _dBSQL.GetCommand(sql_fuel);
                            for (int i = 0; i < dataTable_solvent.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_solvent.Rows[i][0].ToString());
                                string sql_1 = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SDPB = " + result.Item2 + " where MAT_L2_CH = " + CH + "";
                                string sql_3 = "update CFG_MAT_L2_PBSD_INTERFACE set peibizhi = " + result.Item2 + " where canghao =" + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_1);
                                if (count > 0)
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString();
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_1.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_3);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_3.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            for (int i = 0; i < dataTable_fuel.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_fuel.Rows[i][0].ToString());
                                string sql_2 = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SDPB = " + result.Item3 + " where MAT_L2_CH = " + CH + "";
                                string sql_4 = "update CFG_MAT_L2_PBSD_INTERFACE set peibizhi = " + result.Item3 + " where canghao = " + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_2);

                                if (count > 0)
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString();
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_2.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_4);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_4.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            _vLog.writelog(messbox, 0);
                            return true;
                        }
                        else
                        {
                            string mistake = "混合料中CaO已经过多 石灰石调节要求减少石灰石配比";
                            _vLog.writelog(mistake, -1);
                            return false;
                        }
                    }
                    else if (matching_signal == 2)
                    {
                        string messbox = "配比调整特殊配比为调整熔剂、燃料、白云石配比方式";
                        var result = HMICAL.CptSolfuel(MBHT, MBJD, RTZZ, TTZZ, MBMG, MGZZ);

                        if (result != null && result.Item1 != -9014)
                        {
                            if (result.Item2 > RJ_MAX && result.Item2 < RJ_MIN)
                            {
                                messbox += ",溶剂设定配比超限:" + result.Item2.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            if (result.Item3 > RL_MAX && result.Item3 < RL_MIN)
                            {
                                messbox += ",燃料设定配比超限:" + result.Item3.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            if (result.Item4 > BYS_MAX && result.Item4 < BYS_MIN)
                            {
                                messbox += ",白云石设定配比超限:" + result.Item4.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            //***查询溶剂对应的下料口
                            string sql_solvent = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 1 and a.canghao = b.MAT_L2_CH";
                            //***查询燃料对应的下料口
                            string sql_fuel = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 2 and a.canghao = b.MAT_L2_CH";
                            //***查询白云石对应的下料口
                            string sql_BYS = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 4 and a.canghao = b.MAT_L2_CH";

                            DataTable dataTable_solvent = _dBSQL.GetCommand(sql_solvent);
                            DataTable dataTable_fuel = _dBSQL.GetCommand(sql_fuel);
                            DataTable dataTable_BYS = _dBSQL.GetCommand(sql_BYS);
                            for (int i = 0; i < dataTable_solvent.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_solvent.Rows[i][0].ToString());
                                string sql_1 = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SDPB = " + result.Item2 + " where MAT_L2_CH = " + CH + "";
                                string sql_3 = "update CFG_MAT_L2_PBSD_INTERFACE set peibizhi = " + result.Item2 + " where canghao =" + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_1);
                                if (count > 0)
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString();
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_1.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_3);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_3.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            for (int i = 0; i < dataTable_fuel.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_fuel.Rows[i][0].ToString());
                                string sql_2 = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SDPB = " + result.Item3 + " where MAT_L2_CH = " + CH + "";
                                string sql_4 = "update CFG_MAT_L2_PBSD_INTERFACE set peibizhi = " + result.Item3 + " where canghao = " + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_2);
                                if (count > 0)
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString();
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_2.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_4);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_4.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            for (int i = 0; i < dataTable_BYS.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_BYS.Rows[i][0].ToString());
                                string sql_2 = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SDPB = " + result.Item4 + " where MAT_L2_CH = " + CH + "";
                                string sql_4 = "update CFG_MAT_L2_PBSD_INTERFACE set peibizhi = " + result.Item4 + " where canghao = " + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_2);
                                if (count > 0)
                                {
                                    messbox += "，仓号:" + CH.ToString() + "白云石设定配比：" + result.Item4.ToString();
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "白云石设定配比：" + result.Item4.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_2.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_4);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "白云石设定配比：" + result.Item4.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_4.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            _vLog.writelog(messbox, 0);
                            return true;
                        }
                        else
                        {
                            string mistake = " CptSolfuel_1方法配比输入错误或者原料成分不合适";
                            _vLog.writelog(mistake, -1);
                            return false;
                        }
                    }
                    else
                    {
                        string mistake = " CptSolfuel_1方法MC_MIXCAL_PAR 表PAR_S_BILL_STATE字段特殊配比调整方式有误";
                        _vLog.writelog(mistake, -1);
                        return false;
                    }
                }
                else
                {
                    string mistake = "CptSolfuel_1方法CFG_MAT_L2_MACAL_IDT表查询失败";
                    _vLog.writelog(mistake, -1);

                    return false;
                }
            }
            catch (Exception ee)
            {
                string mistake = "CptSolfuel_1方法计算熔剂白云石燃料失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return false;
            }
        }

        /// <summary>
        /// 计算溶剂和燃料的配比(演算模式)
        /// matching_signal:调整方式
        /// RJ_MAX：碱度上限
        /// RJ_MIN:碱度下限
        /// RL_MAX：燃料上限
        /// RL_MIN：燃料下限
        /// BYS_MAX：白云石上限
        /// BYS_MIN：白云石下限
        /// </summary>
        public bool CptSolfuel_2(int matching_signal, float RJ_MAX, float RJ_MIN, float RL_MAX, float RL_MIN, float BYS_MAX, float BYS_MIN)
        {
            try
            {
                string sql_cf = "select  top (1) C_Aim,C_Md,R_Aim,R_Md,MG_Aim,MG_Md from  CFG_MAT_L2_MACAL_IDT_CalCulus  order by TIMESTAMP desc";
                DataTable dataTable_cf = _dBSQL.GetCommand(sql_cf);
                if (dataTable_cf.Rows.Count > 0 && dataTable_cf != null)
                {
                    float MBHT = float.Parse(dataTable_cf.Rows[0]["C_Aim"].ToString());//目标含碳
                    float TTZZ = float.Parse(dataTable_cf.Rows[0]["C_Md"].ToString());//碳调整值
                    float MBJD = float.Parse(dataTable_cf.Rows[0]["R_Aim"].ToString());//目标碱度
                    float RTZZ = float.Parse(dataTable_cf.Rows[0]["R_Md"].ToString());//碱度调整值
                    float MBMG = float.Parse(dataTable_cf.Rows[0]["MG_Aim"].ToString());//目标MG
                    float MGZZ = float.Parse(dataTable_cf.Rows[0]["MG_Md"].ToString());//MG调整值
                    LGSinter HMICAL = new LGSinter();
                    // 判断matching_signal字段的计算方式（1：调整熔剂、燃料配比；2：调整熔剂、燃料、白云石配比）
                    if (matching_signal == 1)
                    {
                        string messbox = "配比调整特殊配比为调整熔剂、燃料配比方式";
                        //****计算溶剂和燃料   0非溶剂和燃料、1溶剂、2燃料*****
                        var result = HMICAL.CptSolfuels_1(MBHT, MBJD, RTZZ, TTZZ);
                        if (result.Item1 >= 0)
                        {
                            if (result.Item2 > RJ_MAX && result.Item2 < RJ_MIN)
                            {
                                messbox += ",溶剂设定配比超限:" + result.Item2.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            if (result.Item3 > RL_MAX && result.Item3 < RL_MIN)
                            {
                                messbox += ",燃料设定配比超限:" + result.Item3.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            //***查询溶剂对应的下料口 （0：非熔剂、非燃料、非烧返、非白云石配比；1：熔剂配比；2：燃料配比；3：烧返配比；4：白云石配比）
                            string sql_solvent = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 1 and a.canghao = b.MAT_L2_CH";
                            //***查询燃料对应的下料口
                            string sql_fuel = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 2 and a.canghao = b.MAT_L2_CH";
                            DataTable dataTable_solvent = _dBSQL.GetCommand(sql_solvent);
                            DataTable dataTable_fuel = _dBSQL.GetCommand(sql_fuel);
                            for (int i = 0; i < dataTable_solvent.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_solvent.Rows[i][0].ToString());
                                string sql_1 = "update CFG_MAT_L2_SJPB_INTERFACE_CalCulus set MAT_L2_SDPB = " + result.Item2 + " where MAT_L2_CH = " + CH + "";
                                string sql_3 = "update CFG_MAT_L2_PBSD_INTERFACE_CalCulus set peibizhi = " + result.Item2 + " where canghao =" + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_1);
                                if (count > 0)
                                {
                                    // messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString();
                                }
                                else
                                {
                                    // messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_1.ToString();
                                    // _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_3);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_3.ToString();
                                    //_vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            for (int i = 0; i < dataTable_fuel.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_fuel.Rows[i][0].ToString());
                                string sql_2 = "update CFG_MAT_L2_SJPB_INTERFACE_CalCulus set MAT_L2_SDPB = " + result.Item3 + " where MAT_L2_CH = " + CH + "";
                                string sql_4 = "update CFG_MAT_L2_PBSD_INTERFACE_CalCulus set peibizhi = " + result.Item3 + " where canghao = " + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_2);

                                if (count > 0)
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString();
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_2.ToString();
                                    // _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_4);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_4.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            _vLog.writelog(messbox, 0);
                            return true;
                        }
                        else
                        {
                            string mistake = "混合料中CaO已经过多 石灰石调节要求减少石灰石配比";
                            _vLog.writelog(mistake, -1);
                            return false;
                        }
                    }
                    else if (matching_signal == 2)
                    {
                        string messbox = "配比调整特殊配比为调整熔剂、燃料、白云石配比方式";
                        var result = HMICAL.CptSolfuel_1(MBHT, MBJD, RTZZ, TTZZ, MBMG, MGZZ);

                        if (result != null && result.Item1 != -9014)
                        {
                            if (result.Item2 > RJ_MAX && result.Item2 < RJ_MIN)
                            {
                                messbox += ",溶剂设定配比超限:" + result.Item2.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            if (result.Item3 > RL_MAX && result.Item3 < RL_MIN)
                            {
                                messbox += ",燃料设定配比超限:" + result.Item3.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            if (result.Item4 > BYS_MAX && result.Item4 < BYS_MIN)
                            {
                                messbox += ",白云石设定配比超限:" + result.Item4.ToString();
                                _vLog.writelog(messbox, -1);
                                return false;
                            }
                            //***查询溶剂对应的下料口
                            string sql_solvent = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 1 and a.canghao = b.MAT_L2_CH";
                            //***查询燃料对应的下料口
                            string sql_fuel = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 2 and a.canghao = b.MAT_L2_CH";
                            //***查询白云石对应的下料口
                            string sql_BYS = "  select b.MAT_L2_CH from CFG_MAT_L2_PBSD_INTERFACE a ,CFG_MAT_L2_SJPB_INTERFACE b where a.category = 4 and a.canghao = b.MAT_L2_CH";

                            DataTable dataTable_solvent = _dBSQL.GetCommand(sql_solvent);
                            DataTable dataTable_fuel = _dBSQL.GetCommand(sql_fuel);
                            DataTable dataTable_BYS = _dBSQL.GetCommand(sql_BYS);
                            for (int i = 0; i < dataTable_solvent.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_solvent.Rows[i][0].ToString());
                                string sql_1 = "update CFG_MAT_L2_SJPB_INTERFACE_CalCulus set MAT_L2_SDPB = " + result.Item2 + " where MAT_L2_CH = " + CH + "";
                                string sql_3 = "update CFG_MAT_L2_PBSD_INTERFACE_CalCulus set peibizhi = " + result.Item2 + " where canghao =" + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_1);
                                if (count > 0)
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString();
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_1.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_3);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "溶剂设定配比：" + result.Item2.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_3.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            for (int i = 0; i < dataTable_fuel.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_fuel.Rows[i][0].ToString());
                                string sql_2 = "update CFG_MAT_L2_SJPB_INTERFACE_CalCulus set MAT_L2_SDPB = " + result.Item3 + " where MAT_L2_CH = " + CH + "";
                                string sql_4 = "update CFG_MAT_L2_PBSD_INTERFACE_CalCulus set peibizhi = " + result.Item3 + " where canghao = " + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_2);
                                if (count > 0)
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString();
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_2.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_4);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "燃料设定配比：" + result.Item3.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_4.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            for (int i = 0; i < dataTable_BYS.Rows.Count; i++)
                            {
                                int CH = int.Parse(dataTable_BYS.Rows[i][0].ToString());
                                string sql_2 = "update CFG_MAT_L2_SJPB_INTERFACE_CalCulus set MAT_L2_SDPB = " + result.Item4 + " where MAT_L2_CH = " + CH + "";
                                string sql_4 = "update CFG_MAT_L2_PBSD_INTERFACE_CalCulus set peibizhi = " + result.Item4 + " where canghao = " + CH + "";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_2);
                                if (count > 0)
                                {
                                    messbox += "，仓号:" + CH.ToString() + "白云石设定配比：" + result.Item4.ToString();
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "白云石设定配比：" + result.Item4.ToString() + "更新CFG_MAT_L2_SJPB_INTERFACE失败" + sql_2.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                                int count_1 = _dBSQL.CommandExecuteNonQuery(sql_4);
                                if (count_1 > 0)
                                {
                                }
                                else
                                {
                                    messbox += "，仓号:" + CH.ToString() + "白云石设定配比：" + result.Item4.ToString() + "更新CFG_MAT_L2_PBSD_INTERFACE失败" + sql_4.ToString();
                                    _vLog.writelog(messbox, -1);
                                    return false;
                                }
                            }
                            _vLog.writelog(messbox, 0);
                            return true;
                        }
                        else
                        {
                            string mistake = " CptSolfuel_1方法配比输入错误或者原料成分不合适";
                            _vLog.writelog(mistake, -1);
                            return false;
                        }
                    }
                    else
                    {
                        string mistake = " CptSolfuel_1方法MC_MIXCAL_PAR 表PAR_S_BILL_STATE字段特殊配比调整方式有误";
                        _vLog.writelog(mistake, -1);
                        return false;
                    }
                }
                else
                {
                    string mistake = "CptSolfuel_1方法CFG_MAT_L2_MACAL_IDT表查询失败";
                    _vLog.writelog(mistake, -1);

                    return false;
                }
            }
            catch (Exception ee)
            {
                string mistake = "CptSolfuel_1方法计算熔剂白云石燃料失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return false;
            }
        }

        /// <summary>
        /// 调整值变化参数
        /// item1：R
        /// item2：C
        /// item3：MG
        /// item4: SP
        /// </summary>
        /// <returns></returns>
        public Tuple<float, float, float, float> TEXT_CHANGE()
        {
            try
            {
                var _SQL = "SELECT PAR_C_ADJ_ADD , PAR_R_ADJ_ADD , PAR_MG_ADJ_ADD , PAR_SP_ADJ_ADD  FROM MC_MIXCAL_PAR";
                DataTable _data = _dBSQL.GetCommand(_SQL);
                if (_data == null && _data.Rows.Count <= 0)
                    return new Tuple<float, float, float, float>(0, 0, 0, 0);
                float A1 = float.Parse(_data.Rows[0]["PAR_R_ADJ_ADD"].ToString());
                float A2 = float.Parse(_data.Rows[0]["PAR_C_ADJ_ADD"].ToString());
                float A3 = float.Parse(_data.Rows[0]["PAR_MG_ADJ_ADD"].ToString());
                float A4 = float.Parse(_data.Rows[0]["PAR_SP_ADJ_ADD"].ToString());
                return new Tuple<float, float, float, float>(A1, A2, A3, A4);
            }
            catch (Exception EE)
            {
                return new Tuple<float, float, float, float>(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// 查询参数数据，
        /// _A:字段名
        /// _B:参数表名
        /// </summary>
        /// <param name="_A"></param>
        /// <param name="_B"></param>
        /// <returns></returns>
        public List<float> Get_MIX_PAR(string[] _A, string _B)
        {
            try
            {
                string _Field = "";
                for (int x = 0; x < _A.Count(); x++)
                {
                    if (x == _A.Count() - 1)
                    {
                        _Field += _A[x];
                    }
                    else
                    {
                        _Field += _A[x] + ",";
                    }
                }
                var _sql = "select " + _Field + " from " + _B;
                DataTable _table = _dBSQL.GetCommand(_sql);
                if (_table != null && _table.Rows.Count > 0)
                {
                    List<float> _C = new List<float>();
                    for (int x = 0; x < _table.Columns.Count; x++)
                    {
                        _C.Add(float.Parse(_table.Rows[0][x].ToString()));
                    }
                    return _C;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ee)
            {
                var mistake = "Get_MIX_PAR方法调用失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return null;
            }
        }

        /// <summary>
        /// 小数位数
        ///Digit_1	设定配比
        ///Digit_2  设定配比%
        ///Digit_3  烧返分仓系数
        ///Digit_4  仓位
        ///Digit_5  实际下料量
        ///Digit_6  偏差
        ///Digit_7  设备转速
        ///Digit_8  湿配比
        ///Digit_9  累计值
        /// </summary>
        /// <returns></returns>
        public List<int> MIX_Digit()
        {
            try
            {
                string _sql = "SELECT Digit_1,Digit_2,Digit_3,Digit_4,Digit_5,Digit_6,Digit_7,Digit_8,Digit_9 FROM MC_MIX_Digit";
                DataTable _data = _dBSQL.GetCommand(_sql);
                if (_data.Rows.Count > 0 && _data != null)
                {
                    List<int> _list = new List<int>();
                    for (int x = 0; x < _data.Columns.Count; x++)
                    {
                        _list.Add(int.Parse(_data.Rows[0][x].ToString()));
                    }
                    return _list;
                }
                else
                {
                    var mistake = "MIX_Digit方法失败,sql :" + _sql;
                    _vLog.writelog(mistake, -1);
                    return null;
                }
            }
            catch (Exception ee)
            {
                var mistake = "MIX_Digit方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return null;
            }
        }

        /// <summary>
        /// 获取开关名称
        /// </summary>
        /// <returns></returns>
        public List<string> Switch_name()
        {
            try
            {
                string _SQL = "SELECT SWITCH_1_OPEN,SWITCH_1_CLOSE,SWITCH_2_OPEN,SWITCH_2_CLOSE,SWITCH_3_OPEN,SWITCH_3_CLOSE,SWITCH_4_OPEN,SWITCH_4_CLOSE FROM MC_CONFIG_MIX";
                DataTable _data = _dBSQL.GetCommand(_SQL);
                if (_data.Rows.Count > 0 && _data != null)
                {
                    List<string> _list = new List<string>();
                    for (int x = 0; x < _data.Columns.Count; x++)
                    {
                        _list.Add(_data.Rows[0][x].ToString());
                    }
                    return _list;
                }
                else
                {
                    var mistake = "MIX_Digit方法失败,sql :" + _SQL;
                    _vLog.writelog(mistake, -1);
                    return null;
                }
            }
            catch (Exception ee)
            {
                var mistake = "Switch_name方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return null;
            }
        }

        /// <summary>
        /// 整体存库
        /// _FLAG ：计算设定下料量情况说明
        /// _MODEL:特殊配比计算方式
        /// </summary>
        /// <param name="_FLAG"></param>
        public void OVER_Storage(int _FLAG, int _MODEL)
        {
            try
            {
                List<int> _L2_CODE = new List<int>();//二级编码数据组
                List<float> _Value_1 = new List<float>();//下料比例数据组(干)
                List<float> _Value_2 = new List<float>();//设定下料量数据组（湿）
                float _sp = 0;//总料量sp
                string _SQL = " UPDATE M_MACAL_INTERFACE_RESULT SET ";//拼接sql语句

                #region 物料编码

                //物料编码数据库字段
                string[] NAME_1_Class = {"MAT_L2_CODE_1",
                                    "MAT_L2_CODE_2",
                                    "MAT_L2_CODE_3",
                                    "MAT_L2_CODE_4",
                                    "MAT_L2_CODE_5",
                                    "MAT_L2_CODE_6",
                                    "MAT_L2_CODE_7",
                                    "MAT_L2_CODE_8",
                                    "MAT_L2_CODE_9",
                                    "MAT_L2_CODE_10",
                                    "MAT_L2_CODE_11",
                                    "MAT_L2_CODE_12",
                                    "MAT_L2_CODE_13",
                                    "MAT_L2_CODE_14",
                                    "MAT_L2_CODE_15",
                                    "MAT_L2_CODE_16",
                                    "MAT_L2_CODE_17",
                                    "MAT_L2_CODE_18",
                                    "MAT_L2_CODE_19",
                                    "MAT_L2_CODE_20"  };

                string sql_1 = "select L2_CODE from M_MATERIAL_BINS order by BIN_NUM asc";
                DataTable data_1 = _dBSQL.GetCommand(sql_1);
                if (data_1.Rows.Count > 0 && data_1 != null)
                {
                    for (int x = 0; x < data_1.Rows.Count; x++)
                    {
                        _SQL += NAME_1_Class[x] + " = '" + data_1.Rows[x][0].ToString() + "',";
                        _L2_CODE.Add(int.Parse(data_1.Rows[x][0].ToString()));
                    }
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误，物料编码查询失败，sql:" + sql_1;
                    _vLog.writelog(mistake, -1);
                    return;
                }

                #endregion 物料编码

                #region 启停信号

                //数据库字段
                string[] NAME_2_Class = {"MAT_L2_STATE_1",
                                    "MAT_L2_STATE_2",
                                    "MAT_L2_STATE_3",
                                    "MAT_L2_STATE_4",
                                    "MAT_L2_STATE_5",
                                    "MAT_L2_STATE_6",
                                    "MAT_L2_STATE_7",
                                    "MAT_L2_STATE_8",
                                    "MAT_L2_STATE_9",
                                    "MAT_L2_STATE_10",
                                    "MAT_L2_STATE_11",
                                    "MAT_L2_STATE_12",
                                    "MAT_L2_STATE_13",
                                    "MAT_L2_STATE_14",
                                    "MAT_L2_STATE_15",
                                    "MAT_L2_STATE_16",
                                    "MAT_L2_STATE_17",
                                    "MAT_L2_STATE_18",
                                    "MAT_L2_STATE_19",
                                    "MAT_L2_STATE_20"  };

                string sql_2 = "select MAT_L2_STATE_1,MAT_L2_STATE_2,MAT_L2_STATE_3,MAT_L2_STATE_4,MAT_L2_STATE_5,MAT_L2_STATE_6,MAT_L2_STATE_7,MAT_L2_STATE_8,MAT_L2_STATE_9,MAT_L2_STATE_10,MAT_L2_STATE_11,MAT_L2_STATE_12,MAT_L2_STATE_13,MAT_L2_STATE_14,MAT_L2_STATE_15,MAT_L2_STATE_16,MAT_L2_STATE_17,MAT_L2_STATE_18,MAT_L2_STATE_19,MAT_L2_STATE_20 from MC_SINCAL_PLC_PAR where mat_l2_flag = 1";
                DataTable data_2 = _dBSQL.GetCommand(sql_2);
                if (data_2.Rows.Count > 0 && data_2 != null)
                {
                    for (int x = 0; x < data_2.Columns.Count; x++)
                    {
                        _SQL += NAME_2_Class[x] + " = '" + data_2.Rows[0][x].ToString() + "',";
                    }
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误，启停信号查询失败，sql:" + sql_2;
                    _vLog.writelog(mistake, -1);
                    return;
                }

                #endregion 启停信号

                #region 当前配比（干）&当前配比（干）%&湿配比百分比

                //数据库字段
                // 当前配比（干）
                string[] NAME_3_Class = {
                                    "MAT_L2_DQPB_1",
                                    "MAT_L2_DQPB_2",
                                    "MAT_L2_DQPB_3",
                                    "MAT_L2_DQPB_4",
                                    "MAT_L2_DQPB_5",
                                    "MAT_L2_DQPB_6",
                                    "MAT_L2_DQPB_7",
                                    "MAT_L2_DQPB_8",
                                    "MAT_L2_DQPB_9",
                                    "MAT_L2_DQPB_10",
                                    "MAT_L2_DQPB_11",
                                    "MAT_L2_DQPB_12",
                                    "MAT_L2_DQPB_13",
                                    "MAT_L2_DQPB_14",
                                    "MAT_L2_DQPB_15",
                                    "MAT_L2_DQPB_16",
                                    "MAT_L2_DQPB_17",
                                    "MAT_L2_DQPB_18",
                                    "MAT_L2_DQPB_19" };
                // 当前配比（干）%
                string[] NAME_4_Class = {
                                    "MAT_L2_DQBFB_1",
                                    "MAT_L2_DQBFB_2",
                                    "MAT_L2_DQBFB_3",
                                    "MAT_L2_DQBFB_4",
                                    "MAT_L2_DQBFB_5",
                                    "MAT_L2_DQBFB_6",
                                    "MAT_L2_DQBFB_7",
                                    "MAT_L2_DQBFB_8",
                                    "MAT_L2_DQBFB_9",
                                    "MAT_L2_DQBFB_10",
                                    "MAT_L2_DQBFB_11",
                                    "MAT_L2_DQBFB_12",
                                    "MAT_L2_DQBFB_13",
                                    "MAT_L2_DQBFB_14",
                                    "MAT_L2_DQBFB_15",
                                    "MAT_L2_DQBFB_16",
                                    "MAT_L2_DQBFB_17",
                                    "MAT_L2_DQBFB_18",
                                    "MAT_L2_DQBFB_19"};
                //湿配比百分比
                string[] NAME_9_Class = {
                                    "MAT_L2_PBBFB_1",
                                    "MAT_L2_PBBFB_2",
                                    "MAT_L2_PBBFB_3",
                                    "MAT_L2_PBBFB_4",
                                    "MAT_L2_PBBFB_5",
                                    "MAT_L2_PBBFB_6",
                                    "MAT_L2_PBBFB_7",
                                    "MAT_L2_PBBFB_8",
                                    "MAT_L2_PBBFB_9",
                                    "MAT_L2_PBBFB_10",
                                    "MAT_L2_PBBFB_11",
                                    "MAT_L2_PBBFB_12",
                                    "MAT_L2_PBBFB_13",
                                    "MAT_L2_PBBFB_14",
                                    "MAT_L2_PBBFB_15",
                                    "MAT_L2_PBBFB_16",
                                    "MAT_L2_PBBFB_17",
                                    "MAT_L2_PBBFB_18",
                                    "MAT_L2_PBBFB_19"};

                string sql_3 = "select MAT_L2_DQPB,MAT_L2_DQBFB,MAT_L2_SPB from CFG_MAT_L2_SJPB_INTERFACE where MAT_L2_CH != 16 order by MAT_L2_CH asc";//15仓及16仓查询需要一个即可
                DataTable data_3 = _dBSQL.GetCommand(sql_3);
                if (data_3.Rows.Count > 0 && data_3 != null)
                {
                    for (int x = 0; x < data_3.Rows.Count; x++)
                    {
                        _SQL += NAME_3_Class[x] + " = '" + data_3.Rows[x][0].ToString() + "',";//当前配比（干）
                        _SQL += NAME_4_Class[x] + " = '" + data_3.Rows[x][1].ToString() + "',";// 当前配比（干）%
                        _SQL += NAME_9_Class[x] + " = '" + data_3.Rows[x][2].ToString() + "',";// 湿配比百分比
                    }
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误， 当前配比（干）&百分比&湿配比百分比查询失败，sql:" + sql_3;
                    _vLog.writelog(mistake, -1);
                    return;
                }

                #endregion 当前配比（干）&当前配比（干）%&湿配比百分比

                #region 分仓系数 & 干下料比例&设定下料

                //数据库字段
                //分仓系数
                string[] NAME_5_Class = {
                                    "MAT_L2_FCXX_1",
                                    "MAT_L2_FCXX_2",
                                    "MAT_L2_FCXX_3",
                                    "MAT_L2_FCXX_4",
                                    "MAT_L2_FCXX_5",
                                    "MAT_L2_FCXX_6",
                                    "MAT_L2_FCXX_7",
                                    "MAT_L2_FCXX_8",
                                    "MAT_L2_FCXX_9",
                                    "MAT_L2_FCXX_10",
                                    "MAT_L2_FCXX_11",
                                    "MAT_L2_FCXX_12",
                                    "MAT_L2_FCXX_13",
                                    "MAT_L2_FCXX_14",
                                    "MAT_L2_FCXX_15",
                                    "MAT_L2_FCXX_16",
                                    "MAT_L2_FCXX_17",
                                    "MAT_L2_FCXX_18",
                                    "MAT_L2_FCXX_19",
                                    "MAT_L2_FCXX_20" };
                //下料比例
                string[] NAME_6_Class = {
                                    "MAT_L2_XLBL_1",
                                    "MAT_L2_XLBL_2",
                                    "MAT_L2_XLBL_3",
                                    "MAT_L2_XLBL_4",
                                    "MAT_L2_XLBL_5",
                                    "MAT_L2_XLBL_6",
                                    "MAT_L2_XLBL_7",
                                    "MAT_L2_XLBL_8",
                                    "MAT_L2_XLBL_9",
                                    "MAT_L2_XLBL_10",
                                    "MAT_L2_XLBL_11",
                                    "MAT_L2_XLBL_12",
                                    "MAT_L2_XLBL_13",
                                    "MAT_L2_XLBL_14",
                                    "MAT_L2_XLBL_15",
                                    "MAT_L2_XLBL_16",
                                    "MAT_L2_XLBL_17",
                                    "MAT_L2_XLBL_18",
                                    "MAT_L2_XLBL_19",
                                    "MAT_L2_XLBL_20" };

                //设定下料量
                string[] NAME_7_Class = {
                                    "MAT_L2_SDXL_1",
                                    "MAT_L2_SDXL_2",
                                    "MAT_L2_SDXL_3",
                                    "MAT_L2_SDXL_4",
                                    "MAT_L2_SDXL_5",
                                    "MAT_L2_SDXL_6",
                                    "MAT_L2_SDXL_7",
                                    "MAT_L2_SDXL_8",
                                    "MAT_L2_SDXL_9",
                                    "MAT_L2_SDXL_10",
                                    "MAT_L2_SDXL_11",
                                    "MAT_L2_SDXL_12",
                                    "MAT_L2_SDXL_13",
                                    "MAT_L2_SDXL_14",
                                    "MAT_L2_SDXL_15",
                                    "MAT_L2_SDXL_16",
                                    "MAT_L2_SDXL_17",
                                    "MAT_L2_SDXL_18",
                                    "MAT_L2_SDXL_19",
                                    "MAT_L2_SDXL_20" };

                string sql_4 = "select MAT_L2_FCXS,MAT_L2_GXLBL,MAT_L2_SDXL from CFG_MAT_L2_XLK_INTERFACE  order by MAT_L2_CH asc";
                DataTable data_4 = _dBSQL.GetCommand(sql_4);
                if (data_4.Rows.Count > 0 && data_4 != null)
                {
                    for (int x = 0; x < data_4.Rows.Count; x++)
                    {
                        _SQL += NAME_5_Class[x] + " = '" + data_4.Rows[x][0].ToString() + "',";//分仓系数
                        _SQL += NAME_6_Class[x] + " = '" + data_4.Rows[x][1].ToString() + "',";//干下料配比
                        _SQL += NAME_7_Class[x] + " = '" + data_4.Rows[x][2].ToString() + "',";//设定下料量
                        _Value_1.Add(float.Parse(data_4.Rows[x][1].ToString()));//干下料配比
                        _Value_2.Add(float.Parse(data_4.Rows[x][2].ToString()));//设定下料量
                    }
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误， 分仓系数 &干下料比例&设定下料查询失败，sql:" + sql_4;
                    _vLog.writelog(mistake, -1);
                    return;
                }

                #endregion 分仓系数 & 干下料比例&设定下料

                #region 当前水分

                //水分当前
                string[] NAME_8_Class = {
                                    "MAT_L2_DQSF_1",
                                    "MAT_L2_DQSF_2",
                                    "MAT_L2_DQSF_3",
                                    "MAT_L2_DQSF_4",
                                    "MAT_L2_DQSF_5",
                                    "MAT_L2_DQSF_6",
                                    "MAT_L2_DQSF_7",
                                    "MAT_L2_DQSF_8",
                                    "MAT_L2_DQSF_9",
                                    "MAT_L2_DQSF_10",
                                    "MAT_L2_DQSF_11",
                                    "MAT_L2_DQSF_12",
                                    "MAT_L2_DQSF_13",
                                    "MAT_L2_DQSF_14",
                                    "MAT_L2_DQSF_15",
                                    "MAT_L2_DQSF_16",
                                    "MAT_L2_DQSF_17",
                                    "MAT_L2_DQSF_18",
                                    "MAT_L2_DQSF_19",
                                    "MAT_L2_DQSF_20" };
                string sql_5 = "select MAT_L2_SFDQ from CFG_MAT_L2_SJPB_INTERFACE  order by MAT_L2_CH asc";
                DataTable data_5 = _dBSQL.GetCommand(sql_5);
                if (data_5.Rows.Count > 0 && data_5 != null)
                {
                    for (int x = 0; x < data_5.Rows.Count; x++)
                    {
                        _SQL += NAME_8_Class[x] + " = '" + data_5.Rows[x][0].ToString() + "',";//当前水分
                    }
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误，当前水分查询失败，sql:" + sql_5;
                    _vLog.writelog(mistake, -1);
                    return;
                }

                #endregion 当前水分

                #region 调整值及目标值

                //水分当前
                string[] NAME_10_Class = {
                                    "MAT_L2_MB_C",
                                    "MAT_L2_TZ_C",
                                    "MAT_L2_MB_R",
                                    "MAT_L2_TZ_R",
                                    "MAT_L2_MB_MG",
                                    "MAT_L2_TZ_MG" };
                var _sql = "";
                if (_MODEL == 1)//调整熔剂、燃料配比
                {
                    _sql = "select top(1) C_Aim,C_Md,R_Aim,R_Md from CFG_MAT_L2_MACAL_IDT order by TIMESTAMP desc";
                }
                else if (_MODEL == 2)         //调整熔剂、燃料、白云石配比
                {
                    _sql = "select top(1) C_Aim,C_Md,R_Aim,R_Md,MG_Aim,MG_Md from CFG_MAT_L2_MACAL_IDT order by TIMESTAMP desc";
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误，调整值及目标值计算中特殊配比计算模式不包含已有规则";
                    _vLog.writelog(mistake, -1);
                    return;
                }
                DataTable data_6 = _dBSQL.GetCommand(_sql);
                if (data_6.Rows.Count > 0 && data_6 != null)
                {
                    for (int x = 0; x < data_6.Columns.Count; x++)
                    {
                        _SQL += NAME_10_Class[x] + " = '" + data_6.Rows[0][x].ToString() + "',";//调整值及目标值
                    }
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误，调整值及目标值查询失败，sql:" + _sql;
                    _vLog.writelog(mistake, -1);
                    return;
                }

                #endregion 调整值及目标值

                #region 料量

                string[] NAME_11_Class = {
                                    "MAT_L2_ZGLL",//总干料量
                                    "MAT_L2_ZLL_PV",//总料量PV
                                    "MAT_L2_ZLL_SP",//总料量SP
                                    "MAT_L2_LLCL"//理论产量
                                      };
                var sql_6 = "select top(1) MAT_L2_ZGLL,MAT_PLC_PV,MAT_L2_SP,MAT_L2_LLCL from CFG_MAT_L2_PLZL_INTERFACE order by TIMESTAMP desc";

                DataTable data_7 = _dBSQL.GetCommand(sql_6);
                if (data_7.Rows.Count > 0 && data_7 != null)
                {
                    _sp = float.Parse(data_7.Rows[0]["MAT_L2_SP"].ToString());
                    for (int x = 0; x < data_7.Columns.Count; x++)
                    {
                        _SQL += NAME_11_Class[x] + " = '" + data_7.Rows[0][x].ToString() + "',";//
                    }
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误，料量查询失败，sql:" + sql_6;
                    _vLog.writelog(mistake, -1);
                    return;
                }

                #endregion 料量

                #region 开关状态

                string[] NAME_12_Class = {
                                    " MAT_L2_SIGNAL_R",
                                    "MAT_L2_SIGNAL_C",
                                    "MAT_L2_SIGNAL_FK",
                                    "MAT_L2_SIGNAL_MG"
                                       };
                var sql_7 = "";
                if (_MODEL == 1)//调整熔剂、燃料配比
                {
                    sql_7 = "select top(1) MAT_L2_but_r,MAT_L2_but_c,MAT_L2_but_fk from CFG_MAT_L2_Butsig_INTERFACE order by TIMESTAMP desc";
                }
                else if (_MODEL == 2)         //调整熔剂、燃料、白云石配比
                {
                    sql_7 = "select top(1) MAT_L2_but_r,MAT_L2_but_c,MAT_L2_but_fk,MAT_L2_but_mg from CFG_MAT_L2_Butsig_INTERFACE order by TIMESTAMP desc";
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误，开关状态中特殊配比计算模式不包含已有规则";
                    _vLog.writelog(mistake, -1);
                    return;
                }
                DataTable data_8 = _dBSQL.GetCommand(sql_7);
                if (data_8.Rows.Count > 0 && data_8 != null)
                {
                    for (int x = 0; x < data_8.Columns.Count; x++)
                    {
                        _SQL += NAME_12_Class[x] + " = '" + data_8.Rows[0][x].ToString() + "',";//
                    }
                }
                else
                {
                    var mistake = "OVER_Storage方法调用错误，料量查询失败，sql:" + sql_6;
                    _vLog.writelog(mistake, -1);
                    return;
                }

                #endregion 开关状态

                #region 特殊配比百分比

                #region 高返配比百分比

                Tuple<bool, float> tuple1_1 = _Get_Matching(_L2_CODE, _Value_1, 602, 602, 1, _sp);//干配比
                if (tuple1_1.Item1)
                {
                    _SQL += "MAT_L2_DQ_DRY_1 =  '" + tuple1_1.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_DQ_DRY_1 =  '0', ";
                }
                Tuple<bool, float> tuple1_2 = _Get_Matching(_L2_CODE, _Value_2, 602, 602, 2, _sp);//湿配比
                if (tuple1_1.Item1)
                {
                    _SQL += "MAT_L2_PB_WET_1 =  '" + tuple1_1.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_PB_WET_1 =  '0', ";
                }

                #endregion 高返配比百分比

                #region 铁料配比百分比

                Tuple<bool, float> tuple2_1 = _Get_Matching(_L2_CODE, _Value_1, 101, 299, 1, _sp);//干配比
                if (tuple2_1.Item1)
                {
                    _SQL += "MAT_L2_DQ_DRY_2 =  '" + tuple2_1.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_DQ_DRY_2 =  '0', ";
                }
                Tuple<bool, float> tuple2_2 = _Get_Matching(_L2_CODE, _Value_2, 101, 299, 2, _sp);//湿配比
                if (tuple2_2.Item1)
                {
                    _SQL += "MAT_L2_PB_WET_2 =  '" + tuple2_2.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_PB_WET_2 =  '0', ";
                }

                #endregion 铁料配比百分比

                #region 石灰石配比百分比

                Tuple<bool, float> tuple3_1 = _Get_Matching(_L2_CODE, _Value_1, 403, 406, 1, _sp);//干配比
                if (tuple3_1.Item1)
                {
                    _SQL += "MAT_L2_DQ_DRY_3 =  '" + tuple3_1.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_DQ_DRY_3 =  '0', ";
                }
                Tuple<bool, float> tuple3_2 = _Get_Matching(_L2_CODE, _Value_2, 403, 406, 2, _sp);//湿配比
                if (tuple3_2.Item1)
                {
                    _SQL += "MAT_L2_PB_WET_3 =  '" + tuple3_2.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_PB_WET_3 =  '0', ";
                }

                #endregion 石灰石配比百分比

                #region 白云石配比百分比

                Tuple<bool, float> tuple4_1 = _Get_Matching(_L2_CODE, _Value_1, 401, 402, 1, _sp);//干配比
                if (tuple4_1.Item1)
                {
                    _SQL += "MAT_L2_DQ_DRY_4 =  '" + tuple4_1.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_DQ_DRY_4 =  '0', ";
                }
                Tuple<bool, float> tuple4_2 = _Get_Matching(_L2_CODE, _Value_2, 401, 402, 2, _sp);//湿配比
                if (tuple4_2.Item1)
                {
                    _SQL += "MAT_L2_PB_WET_4 =  '" + tuple4_2.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_PB_WET_4 =  '0', ";
                }

                #endregion 白云石配比百分比

                #region 生石灰配比百分比

                Tuple<bool, float> tuple5_1 = _Get_Matching(_L2_CODE, _Value_1, 408, 409, 1, _sp);//干配比
                if (tuple5_1.Item1)
                {
                    _SQL += "MAT_L2_DQ_DRY_5 =  '" + tuple5_1.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_DQ_DRY_5 =  '0', ";
                }
                Tuple<bool, float> tuple5_2 = _Get_Matching(_L2_CODE, _Value_2, 408, 409, 2, _sp);//湿配比
                if (tuple5_2.Item1)
                {
                    _SQL += "MAT_L2_PB_WET_5 =  '" + tuple5_2.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_PB_WET_5 =  '0', ";
                }

                #endregion 生石灰配比百分比

                #region 燃料配比百分比

                Tuple<bool, float> tuple6_1 = _Get_Matching(_L2_CODE, _Value_1, 301, 399, 1, _sp);//干配比
                if (tuple6_1.Item1)
                {
                    _SQL += "MAT_L2_DQ_DRY_6 =  '" + tuple6_1.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_DQ_DRY_6 =  '0', ";
                }
                Tuple<bool, float> tuple6_2 = _Get_Matching(_L2_CODE, _Value_2, 301, 399, 2, _sp);//湿配比
                if (tuple6_2.Item1)
                {
                    _SQL += "MAT_L2_PB_WET_6 =  '" + tuple6_2.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_PB_WET_6 =  '0', ";
                }

                #endregion 燃料配比百分比

                #region 烧返配比百分比

                Tuple<bool, float> tuple7_1 = _Get_Matching(_L2_CODE, _Value_1, 601, 601, 1, _sp);//干配比
                if (tuple7_1.Item1)
                {
                    _SQL += "MAT_L2_DQ_DRY_7 =  '" + tuple7_1.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_DQ_DRY_7 =  '0', ";
                }
                Tuple<bool, float> tuple7_2 = _Get_Matching(_L2_CODE, _Value_2, 601, 601, 2, _sp);//湿配比
                if (tuple7_2.Item1)
                {
                    _SQL += "MAT_L2_PB_WET_7 =  '" + tuple7_2.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_PB_WET_7 =  '0', ";
                }

                #endregion 烧返配比百分比

                #region 除尘灰配比百分比

                Tuple<bool, float> tuple8_1 = _Get_Matching(_L2_CODE, _Value_1, 501, 550, 1, _sp);//干配比
                if (tuple8_1.Item1)
                {
                    _SQL += "MAT_L2_DQ_DRY_8 =  '" + tuple8_1.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_DQ_DRY_8 =  '0', ";
                }
                Tuple<bool, float> tuple8_2 = _Get_Matching(_L2_CODE, _Value_2, 501, 550, 2, _sp);//湿配比
                if (tuple8_2.Item1)
                {
                    _SQL += "MAT_L2_PB_WET_8 =  '" + tuple8_2.Item2 + "', ";
                }
                else
                {
                    _SQL += "MAT_L2_PB_WET_8 =  '0', ";
                }

                #endregion 除尘灰配比百分比

                #endregion 特殊配比百分比

                _SQL += " TIMESTAMP = GETDATE(),MAT_L2_STATE_FLAG = " + _FLAG + "  where MAT_L2_FLAG = 1";
                int _COUNT = _dBSQL.CommandExecuteNonQuery(_SQL);
                if (_COUNT <= 0)
                {
                    var mistake = "OVER_Storage方法调用错误，数据库操作失败，sql:" + _SQL;
                    _vLog.writelog(mistake, -1);
                    return;
                }
            }
            catch (Exception ee)
            {
                var mistake = "OVER_Storage方法调用错误" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        public Dictionary<int, float> Calculate_SPB()
        {
            try
            {
                //实际下料量和
                float practical_laying_add = 0;

                //获取页面上配比id对应的实际下料量
                Dictionary<int, float> dictionary_SJXLL = new Dictionary<int, float>();
                //每个配比id对应的湿配比%
                Dictionary<int, float> practical = new Dictionary<int, float>();
                //获取页面上配比id对应的消耗累加
                Dictionary<int, float> dictionary_LJ = new Dictionary<int, float>();
                //获取页面消耗累计显示值
                Dictionary<int, float> consume_accumulative = new Dictionary<int, float>();
                var sql1 = "select MAT_L2_SDXL,MAT_PB_ID from CFG_MAT_L2_XLK_INTERFACE order by MAT_PB_ID asc";
                DataTable data_1 = _dBSQL.GetCommand(sql1);
                if (data_1.Rows.Count <= 0 && data_1 == null)
                    return null;

                for (int x = 0; x < data_1.Rows.Count; x++)
                {
                    float COUNT = 0;
                    int PB = 0;
                    COUNT = float.Parse(data_1.Rows[x]["MAT_L2_SDXL"].ToString());
                    practical_laying_add += COUNT;

                    //配比id
                    PB = int.Parse(data_1.Rows[x]["MAT_PB_ID"].ToString());
                    //19个仓每个对应的配比id的实际下料量

                    //根据配比id算出每个配比id对应的实际下料量和
                    //若字典中存在相同的配比则进行累加，不存在则添加
                    if (dictionary_SJXLL.ContainsKey(PB))
                    {
                        dictionary_SJXLL[PB] += COUNT;
                    }
                    else
                    {
                        dictionary_SJXLL.Add(PB, COUNT);
                    }
                }
                //通过dictionary_SJXLL字典的数据计算每个配比id对应的湿配比%
                foreach (var a in dictionary_SJXLL)
                {
                    //（每个配比id对应的设定下料量之和）/（整体的设定下料量）*100
                    //配比id
                    int SPB_ID = a.Key;
                    //配比id对应的实际下料量和
                    float SPB_XLL = a.Value;
                    //添加逻辑判断分母是否为0，
                    if (SPB_XLL == 0)
                    {
                        practical.Add(SPB_ID, 0);
                    }
                    else
                    {
                        float SPB = SPB_XLL / practical_laying_add * 100;
                        practical.Add(SPB_ID, SPB);
                    }
                }
                return practical;
            }
            catch (Exception EE)
            {
                var MISTAKE = "Calculate_SPB方法计算湿配比失败" + EE.ToString();
                _vLog.writelog(MISTAKE, -1);
                return null;
            }
        }

        /// <summary>
        /// 物料种类及二级编码对应最大值最小值（数据库规则）
        /// key:物料描述
        /// values:item1:二级编码最小值、item2:二级编码最大值;、item3:物料归属标志位
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Tuple<int, int, int>> _DIC_L2_CODE_CONFIG()
        {
            try
            {
                Dictionary<string, Tuple<int, int, int>> _dictionary = new Dictionary<string, Tuple<int, int, int>>();
                var _sql = "SELECT M_TYPE,M_DESC,CODE_MIN,CODE_MAX from M_MATERIAL_COOD_CONFIG order by M_TYPE asc";
                DataTable _data = _dBSQL.GetCommand(_sql);
                if (_data.Rows.Count > 0 && _data != null)
                {
                    for (int x = 0; x < _data.Rows.Count; x++)
                    {
                        //物料归属名称
                        var _name = _data.Rows[x]["M_DESC"].ToString();
                        //物料归属名称对应标志位
                        var _flag = int.Parse(_data.Rows[x]["M_TYPE"].ToString());
                        //物料归属二级编码最小值
                        var _L2_CODE_MIN = int.Parse(_data.Rows[x]["CODE_MIN"].ToString());
                        //物料归属二级编码最大值
                        var _L2_CODE_MAX = int.Parse(_data.Rows[x]["CODE_MAX"].ToString());
                        if (!_dictionary.ContainsKey(_name))
                            _dictionary.Add(_name, new Tuple<int, int, int>(_L2_CODE_MIN, _L2_CODE_MAX, _flag));
                    }
                    return _dictionary;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("_DIC_L2_CODE_CONFIG方法失败" + ee.ToString(), -1);
                return null;
            }
        }

        /// <summary>
        /// 物料种类及二级编码对应最大值最小值（自定义规则）
        /// key:物料描述
        /// values:item1:二级编码最小值、item2:二级编码最大值;、item3:物料归属标志位
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Tuple<int, int, int>>> _DIC_L2_CODE_CONFIG_1()
        {
            try
            {
                //混合料 101-119
                //矿粉 120-299
                //燃料 301-399
                //熔剂 401-499
                //除尘灰 501-550
                //返矿 601-650

                //燃料   301 - 399
                //熔剂  401 - 499
                //铁料  101 - 119,501 - 599,601 - 699，
                //矿粉  120 - 299
                //全部  101 - 699
                Dictionary<string, List<Tuple<int, int, int>>> _DIC = new Dictionary<string, List<Tuple<int, int, int>>>();
                List<Tuple<int, int, int>> list_1 = new List<Tuple<int, int, int>>();
                list_1.Add(new Tuple<int, int, int>(101, 119, 1));
                _DIC.Add("混合料", list_1);
                List<Tuple<int, int, int>> list_2 = new List<Tuple<int, int, int>>();
                list_2.Add(new Tuple<int, int, int>(120, 299, 2));
                _DIC.Add("矿粉", list_2);
                List<Tuple<int, int, int>> list_3 = new List<Tuple<int, int, int>>();
                list_3.Add(new Tuple<int, int, int>(301, 399, 3));
                _DIC.Add("燃料", list_3);
                List<Tuple<int, int, int>> list_4 = new List<Tuple<int, int, int>>();
                list_4.Add(new Tuple<int, int, int>(401, 499, 4));
                _DIC.Add("熔剂", list_4);
                List<Tuple<int, int, int>> list_5 = new List<Tuple<int, int, int>>();
                list_5.Add(new Tuple<int, int, int>(501, 550, 5));
                _DIC.Add("除尘灰", list_5);
                List<Tuple<int, int, int>> list_6 = new List<Tuple<int, int, int>>();
                list_6.Add(new Tuple<int, int, int>(601, 650, 7));
                _DIC.Add("返矿", list_6);

                return _DIC;
            }
            catch (Exception ee)
            {
                _vLog.writelog("_DIC_L2_CODE_CONFIG方法失败" + ee.ToString(), -1);
                return null;
            }
        }

        /// <summary>
        /// 查询三级检化验数据是否满足重新计算设定下料量
        /// _GetTuple判断是否满足调用配比调整及确认条件
        /// return item1：是否调用;item2:   0无发生 1成分变化  2水分变化
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, int> Get_INIT_WATER()
        {
            try
            {
                var _sql = "select sum(ELEMENT_FLAG) as flag_1,sum(Water_Flag1) as flag_2 from M_MATERIAL_BINS";
                DataTable _data = _dBSQL.GetCommand(_sql);
                if (_data.Rows.Count > 0 && _data != null)
                {
                    if (_data.Rows[0]["flag_1"].ToString() == "0")//原料成分无变化
                    {
                        if (_data.Rows[0]["flag_2"].ToString() == "0")
                        {
                            return new Tuple<bool, int>(false, 0);
                        }
                        else
                        {
                            return new Tuple<bool, int>(false, 2);
                        }
                    }
                    else
                    {
                        return new Tuple<bool, int>(false, 1);
                    }
                }
                else
                {
                    return new Tuple<bool, int>(false, 0);
                }
            }
            catch
            {
                return new Tuple<bool, int>(false, 0);
            }
        }

        /// <summary>
        /// 是否发生了启停信号变化
        /// retuen item1：是否发生了启停信号变化，item2 1禁用变为启用  -1启用变为禁用
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, int> _Signat_flag()
        {
            try
            {
                DateTime time_2 = DateTime.Now;
                DateTime time_1 = time_2.AddHours(-1);
                var sql = "select * from TurnFlag where flag = 1 and InsertTime >= '" + time_1 + "' and InsertTime<= '" + time_2 + "'";
                DataTable _data = _dBSQL.GetCommand(sql);
                if (_data != null && _data.Rows.Count > 0)
                {
                    var sql_Update = "UPDATE TurnFlag SET flag = 2,ConfirmTime = GETDATE() WHERE InsertTime >= '" + time_1 + "' and InsertTime<= '" + time_2 + "'";
                    int _count = _dBSQL.CommandExecuteNonQuery(sql_Update);
                    if (_count > 0)
                    {
                        return new Tuple<bool, int>(true, 1);
                    }
                    else
                    {
                        _vLog.writelog("_Signat_flag方法更新标志位失败" + sql_Update, 0);
                        return new Tuple<bool, int>(false, 0);
                    }
                }
                else
                {
                    return new Tuple<bool, int>(false, 0);
                }
            }
            catch
            {
                return new Tuple<bool, int>(false, 0);
            }
        }

        /// <summary>
        /// 配料页面调整方式 1 计算熔剂燃料，2计算熔剂燃料白云石
        /// </summary>
        /// <returns></returns>
        public int Initialize_CAL_MODE()
        {
            try
            {
                //判断MC_MIXCAL_PAR表PAR_S_BILL_STATE字段标志位 1 计算熔剂燃料，2计算熔剂燃料白云石
                string _sql = "select top (1) isnull(PAR_S_BILL_STATE,2) as PAR_S_BILL_STATE FROM MC_MIXCAL_PAR order by TIMESTAMP desc";
                DataTable _dataTable = _dBSQL.GetCommand(_sql);
                if (_dataTable != null && _dataTable.Rows.Count == 1)
                {
                    return int.Parse(_dataTable.Rows[0]["PAR_S_BILL_STATE"].ToString());
                }
                else
                {
                    var mistake = "Initialize_CAL_MODE()方法判断调整方式失败,查询行数为0";
                    _vLog.writelog(mistake, -1);
                    return 1;
                }
            }
            catch (Exception ee)
            {
                var mistake = "Initialize_CAL_MODE()方法判断调整方式失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return 1;
            }
        }

        /// <summary>
        ///  初始化计算累计t\天
        /// </summary>
        public Tuple<bool, Dictionary<int, double>> Accumulative_account()
        {
            try
            {
                //key: 仓号 value :消耗
                Dictionary<int, double> dictionary_LJ = new Dictionary<int, double>();
                DateTime now = DateTime.Now;
                //8点、20点作为开始时间
                int _hour = now.Hour;
                DateTime today2 = new DateTime();
                if (_hour >= 8 && _hour < 20)
                {
                    today2 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
                    //当天
                }
                else if (_hour >= 18 && _hour < 24)
                {
                    today2 = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);
                    //当天
                }
                else
                {
                    today2 = new DateTime(now.Year, now.Month, now.Day - 1, 20, 0, 0);
                    //跨天
                }
                //20201021 end
                today2 = new DateTime(now.Year, now.Month, now.Day);
                // int xx = D DATEDIFF(mi, DtOpTime, DtEnd)
                TimeSpan d3 = now - today2;
                double xx = double.Parse(d3.TotalMinutes.ToString());
                int count = (int)xx;
                if (count != 0)
                {
                    //查询当前时间到当天凌晨有多少条数据,根据配比id进行计算
                    string sql = "select " +
                        "sum(isnull(t.MAT_PLC_PV_W_1,0))/60 as MAT_PLC_PV_W_1," +
                        "sum(isnull(t.MAT_PLC_PV_W_2,0))/60 as MAT_PLC_PV_W_2," +
                        "sum(isnull(t.MAT_PLC_PV_W_3,0))/60 as MAT_PLC_PV_W_3," +
                        "SUM(isnull(t.MAT_PLC_PV_W_4,0))/60 as MAT_PLC_PV_W_4," +
                        "SUM(isnull(t.MAT_PLC_PV_W_5,0))/60 as MAT_PLC_PV_W_5," +
                        "SUM(isnull(t.MAT_PLC_PV_W_6,0))/60 as MAT_PLC_PV_W_6," +
                        "SUM(isnull(t.MAT_PLC_PV_W_7,0))/60 as MAT_PLC_PV_W_7," +
                        "SUM(isnull(t.MAT_PLC_PV_W_8,0))/60 as MAT_PLC_PV_W_8," +
                        "SUM(isnull(t.MAT_PLC_PV_W_9,0))/60 as MAT_PLC_PV_W_9," +
                        "SUM(isnull(t.MAT_PLC_PV_W_10,0))/60 as MAT_PLC_PV_W_10," +
                        "SUM(isnull(t.MAT_PLC_PV_W_11,0))/60 as MAT_PLC_PV_W_11," +
                        "SUM(isnull(t.MAT_PLC_PV_W_12,0))/60 as MAT_PLC_PV_W_12," +
                        "SUM(isnull(t.MAT_PLC_PV_W_13,0))/60 as MAT_PLC_PV_W_13," +
                        "SUM(isnull(t.MAT_PLC_PV_W_14,0))/60 as MAT_PLC_PV_W_14," +
                        "SUM(isnull(t.MAT_PLC_PV_W_15,0))/60 as MAT_PLC_PV_W_15," +
                        "SUM(isnull(t.MAT_PLC_PV_W_16,0))/60 as MAT_PLC_PV_W_16," +
                        "SUM(isnull(t.MAT_PLC_PV_W_17,0))/60 as MAT_PLC_PV_W_17," +
                        "SUM(isnull(t.MAT_PLC_PV_W_18,0))/60 as MAT_PLC_PV_W_18," +
                        "SUM(isnull(t.MAT_PLC_PV_W_19,0))/60 as MAT_PLC_PV_W_19, " +
                        "SUM(isnull(t.MAT_PLC_PV_W_20,0))/60 as MAT_PLC_PV_W_20 " +
                        "from(select top " + count + "  MAT_PLC_PV_W_1,MAT_PLC_PV_W_2,MAT_PLC_PV_W_3,MAT_PLC_PV_W_4,MAT_PLC_PV_W_5,MAT_PLC_PV_W_6,MAT_PLC_PV_W_7,MAT_PLC_PV_W_8,MAT_PLC_PV_W_9,MAT_PLC_PV_W_10,MAT_PLC_PV_W_11,MAT_PLC_PV_W_12,MAT_PLC_PV_W_13,MAT_PLC_PV_W_14,MAT_PLC_PV_W_15,MAT_PLC_PV_W_16,MAT_PLC_PV_W_17,MAT_PLC_PV_W_18,MAT_PLC_PV_W_19,MAT_PLC_PV_W_20 from C_MAT_PLC_1MIN order by TIMESTAMP desc) t";
                    DataTable dataTable = _dBSQL.GetCommand(sql);
                    if (dataTable.Rows.Count > 0)
                    {
                        for (int x = 0; x < dataTable.Columns.Count; x++)
                        {
                            dictionary_LJ.Add(x + 1, Math.Round(double.Parse(dataTable.Rows[0][x].ToString()), 2));
                        }
                        return new Tuple<bool, Dictionary<int, double>>(true, dictionary_LJ);
                    }
                    else
                    {
                        return new Tuple<bool, Dictionary<int, double>>(false, null);
                    }
                }
                else
                {
                    return new Tuple<bool, Dictionary<int, double>>(false, null);
                }
            }
            catch (Exception ee)
            {
                string mistake = "Accumulative_account方法计算累计天数有误" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return new Tuple<bool, Dictionary<int, double>>(false, null);
            }
        }

        /// <summary>
        /// 设定配比最小值最大值
        /// key 种类计算出的燃料的配比最大值 ，item1：最小值 item2：最大
        ///计算出的燃料的配比最小值
        ///计算出的灰石的配比最大值
        ///计算出的灰石的配比最小值
        ///计算出的白云石的配比最大值
        ///计算出的白云石的配比最小值
        ///</summary>
        /// <returns></returns>
        public Dictionary<int, Tuple<float, float>> _Getrule()
        {
            try
            {
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 返回下发mid地址
        /// </summary>
        /// <returns></returns>
        public List<int> _Get_Mid()
        {
            List<int> vs = new List<int>();
            //设定下料量
            for (int x = 3; x < 23; x++)
            {
                vs.Add(x);
            }
            //总料量sp
            // vs.Add(1);
            return vs;
        }

        /// <summary>
        /// 返回下发数据
        /// item1:是否正常
        /// item2：数据值(1-20仓+sp)
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, List<float>> _Get_Mid_Date()
        {
            try
            {
                List<float> vs = new List<float>();
                string sql = "select   MAT_L2_SDXL   from CFG_MAT_L2_XLK_INTERFACE order by MAT_L2_CH asc";
                DataTable data = _dBSQL.GetCommand(sql);
                if (data != null && data.Rows.Count > 0)
                {
                    for (int x = 0; x < data.Rows.Count; x++)
                    {
                        vs.Add(float.Parse(data.Rows[x]["MAT_L2_SDXL"].ToString()));
                    }
                    //总料量sp
                    var _sql = " SELECT TOP (1) MAT_L2_SP   FROM CFG_MAT_L2_PLZL_INTERFACE ORDER BY TIMESTAMP DESC ";
                    DataTable _table = _dBSQL.GetCommand(_sql);
                    if (_table != null & _table.Rows.Count > 0)
                    {
                        // vs.Add(float.Parse(_table.Rows[0][0].ToString()));
                        return new Tuple<bool, List<float>>(true, vs);
                    }
                    else
                    {
                        return new Tuple<bool, List<float>>(false, null);
                    }
                }
                else
                {
                    return new Tuple<bool, List<float>>(false, null);
                }
            }
            catch
            {
                return new Tuple<bool, List<float>>(false, null);
            }
        }

        /// <summary>
        /// _Get_Matching获取特殊配比
        ///  _L2_CODE:二级编码数据组
        ///   _Value：数据数据组
        /// L2_code_MIN：二级编码最小值
        /// L2_code_MAX：二级编码最大值
        /// _flag：1特殊干配比 _flag：2特殊湿配比
        /// _SP:总料量SP
        /// </summary>
        /// <param name="L2_code"></param>
        /// <param name="_flag"></param>
        /// <returns></returns>
        public Tuple<bool, float> _Get_Matching(List<int> _L2_CODE, List<float> _Value, int L2_code_MIN, int L2_code_MAX, int _flag, float _SP)
        {
            try
            {
                if (_flag == 1)
                {
                    bool flag = false;
                    float _Sum = 0;
                    for (int x = 0; x < _L2_CODE.Count; x++)
                    {
                        if (L2_code_MIN <= _L2_CODE[x] && L2_code_MAX >= _L2_CODE[x])
                        {
                            flag = true;
                            _Sum += _Value[x];
                        }
                    }
                    if (flag)
                    {
                        return new Tuple<bool, float>(true, _Sum);
                    }
                    else
                    {
                        return new Tuple<bool, float>(false, 0);
                    }
                }
                else
                {
                    bool flag = false;
                    float _Sum = 0;
                    for (int x = 0; x < _L2_CODE.Count; x++)
                    {
                        if (L2_code_MIN <= _L2_CODE[x] && L2_code_MAX >= _L2_CODE[x])
                        {
                            flag = true;
                            _Sum += _Value[x];
                        }
                    }
                    if (flag)
                    {
                        _Sum = _Sum / _SP * 100;
                        return new Tuple<bool, float>(true, _Sum);
                    }
                    else
                    {
                        return new Tuple<bool, float>(false, 0);
                    }
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("_Get_Matching错误" + ee.ToString(), -1);
                return new Tuple<bool, float>(false, 0);
            }
        }

        /// <summary>
        /// 获取指定中控客户端设定IP
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, string> _Get_IpAdress()
        {
            try
            {
                var sql = "select Adress_Ip from  MC_MIX_Digit";
                DataTable _data = _dBSQL.GetCommand(sql);
                if (_data.Rows.Count > 0 && _data != null)
                {
                    return new Tuple<bool, string>(true, _data.Rows[0][0].ToString());
                }
                else
                {
                    return new Tuple<bool, string>(false, "");
                }
            }
            catch
            {
                return new Tuple<bool, string>(false, "");
            }
        }

        /// <summary>
        /// 获取下发权限
        /// item1:现场最高权限
        /// item2：非现场演算权限
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, bool> _GetIp_Jurisdiction()
        {
            GetIpAddress.GetApi getApi = new GetIpAddress.GetApi();
            var IP_Local = getApi.GetIp_Power();//获取本机IP
            Tuple<bool, string> Ip_Setting = _Get_IpAdress();//获取设定Ip
            string[] _A1 = { "User_ID" };
            List<float> _B = Get_MIX_PAR(_A1, "MC_MIX_Digit");
            bool _A = User_Boss(int.Parse(_B[0].ToString()));
            if (Ip_Setting.Item1)
            {
                if (IP_Local.Equals(Ip_Setting.Item2))
                {
                    return new Tuple<bool, bool>(true, _A);
                }
                else
                {
                    return new Tuple<bool, bool>(false, _A);
                }
            }
            else
            {
                return new Tuple<bool, bool>(false, _A);
            }
        }

        /// <summary>
        /// insert 小数位数
        /// 返回现场实际设定下料量
        /// item1:是否正常
        /// item2：数据（1-20仓+sp）
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, List<float>> _Get_Values(int Digit)
        {
            try
            {
                var sql = "select MAT_L2_SDXL_1,MAT_L2_SDXL_2,MAT_L2_SDXL_3,MAT_L2_SDXL_4,MAT_L2_SDXL_5,MAT_L2_SDXL_6,MAT_L2_SDXL_7,MAT_L2_SDXL_8,MAT_L2_SDXL_9,MAT_L2_SDXL_10,MAT_L2_SDXL_11,MAT_L2_SDXL_12,MAT_L2_SDXL_13,MAT_L2_SDXL_14,MAT_L2_SDXL_15,MAT_L2_SDXL_16,MAT_L2_SDXL_17,MAT_L2_SDXL_18,MAT_L2_SDXL_19,MAT_L2_SDXL_20,MAT_L2_SDXL_SP from MC_MIXCAL_Baiting where TIMESTAMP = (select max(TIMESTAMP) from MC_MIXCAL_Baiting)";
                DataTable _table = _dBSQL.GetCommand(sql);
                if (_table != null && _table.Rows.Count > 0)
                {
                    List<float> _a = new List<float>();
                    for (int x = 0; x < _table.Columns.Count; x++)
                    {
                        if (_table.Rows[0][x].ToString() != "")
                        {
                            _a.Add(float.Parse(_table.Rows[0][x].ToString()));
                        }
                        else
                        {
                            return new Tuple<bool, List<float>>(false, null);
                        }
                    }
                    return new Tuple<bool, List<float>>(true, _a);
                }
                else
                {
                    return new Tuple<bool, List<float>>(false, null);
                }
            }
            catch
            {
                return new Tuple<bool, List<float>>(false, null);
            }
        }

        /// <summary>
        /// 判断用户是否支持配比调整演算功能
        /// _D:判断用户等级是否可以进行配比演算
        /// </summary>
        /// <returns></returns>
        public bool User_Boss(int _D)
        {
            try
            {
                var sql = "select * from USER_AUTHORITY where USER_NAME = '" + User_Level.User_name + "' ";
                DataTable _data = _dBSQL.GetCommand(sql);
                if (_data != null && _data.Rows.Count > 0)
                {
                    if (int.Parse(_data.Rows[0]["AUTHORITY"].ToString()) <= _D)
                        return true;
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}