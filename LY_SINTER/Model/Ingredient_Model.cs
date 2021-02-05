using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY_SINTER.Model
{
   
    /// <summary>
    /// 原料维护弹出框仓号传值
    /// </summary>
    class Ingredient_Model
    {
        private static int data = 0;
        public static int Data
        {
            get { return data; }
            set { data = value; }
        }
    }
    /// <summary>
    /// 原料新成分弹出窗
    /// </summary>
    class Raw_Popup
    {
        /// <summary>
        /// 未处理新原料 补录 三级编码
        /// </summary>
        private static string untreated_bl = "";
        public static string UNTREATED_BL
        {
            get { return untreated_bl; }
            set { untreated_bl = value; }
        }
        /// <summary>
        /// 检测到新原料 补录 三级编码
        /// </summary>
        private static string L3_code_bl = "";
        public static string L3_CODE_BL
        {
            get { return L3_code_bl; }
            set { L3_code_bl = value; }
        }
    }
    class L2_CODE_CALSS
    {
        DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        /// <summary>
        /// 通过二级编码判断上下限判断条件
        /// retuen 0：错误 ；1：混匀矿； 2：矿粉；3：燃料；4：溶剂；5：除尘灰；6：返矿；7：烧结矿；8：高炉炉渣
        /// </summary>
        /// 
        /// <param name="_L2_CODE"></param>
        public int L2_code_Judeg(int _L2_CODE)
        {
            string sql_l2code = "select M_TYPE from M_MATERIAL_COOD_CONFIG where CODE_MIN<=" + _L2_CODE + " and CODE_MAX >= " + _L2_CODE + "";
            DataTable data = _dBSQL.GetCommand(sql_l2code);
            if (data.Rows.Count > 0)
            {
                int flag = int.Parse(data.Rows[0][0].ToString());
                return flag;
            }
            else
            {
                return 0;
            }

        }
        /// <summary>
        /// 成分上下限
        /// _flag:物料归属
        /// _L2_code：二级编码
        /// </summary>
        /// <param name="_flag"></param>
        /// <returns></returns>
        public List<double> _GetList(int _flag,int _L2_code)
        {
            try
            {
                var sql_M_MATERIAL_COOD = "";
                if(_flag == 1)
                {
                    sql_M_MATERIAL_COOD = "select " +
                    "C_TFE_UP,C_TFE_LOWER," +
                    "C_CAO_UP,C_CAO_LOWER," +
                    "C_SIO2_UP,C_SIO2_LOWER," +
                    "C_MGO_UP,C_MGO_LOWER," +
                    "C_AL2O3_UP,C_AL2O3_LOWER ," +
                    "C_P_UP,C_P_LOWER," +
                    "C_S_UP,C_S_LOWER," +
                    "C_MNO_UP,C_MNO_LOWER  " +
                    "from M_MATERIAL_COOD where  L2_CODE = " + _L2_code + " ";
                }
                else if(_flag == 3)
                {
                    sql_M_MATERIAL_COOD = "select " +
                        "C_S_UP,C_S_LOWER," +
                        "C_C_UP,C_C_LOWER," +
                        "C_ASH_UP,C_ASH_LOWER," +
                        "C_VOLATILES_UP,C_VOLATILES_LOWER," +
                        //特殊成分上下限
                        "C_CAO_UP,C_CAO_LOWER," +
                        "C_SIO2_UP,C_SIO2_LOWER," +
                        "C_MGO_UP,C_MGO_LOWER," +
                        "C_AL2O3_UP,C_AL2O3_LOWER ," +
                        "C_P_UP,C_P_LOWER," +
                        "C_K2O_UP,C_K2O_LOWER,  " +
                        "C_NA2O_UP,C_NA2O_LOWER  " +
                        "from M_MATERIAL_COOD where  L2_CODE = " + _L2_code + " ";
                }
                else if (_flag == 4)
                {
                    sql_M_MATERIAL_COOD = "select " +
                        "C_CAO_UP,C_CAO_LOWER," +
                        "C_SIO2_UP,C_SIO2_LOWER," +
                        "C_MGO_UP,C_MGO_LOWER," +
                        "C_AL2O3_UP,C_AL2O3_LOWER ," +
                        "C_P_UP,C_P_LOWER," +
                        "C_S_UP,C_S_LOWER  " +
                        "from M_MATERIAL_COOD where  L2_CODE = " + _L2_code + " ";
                }
                else if (_flag == 5)
                {
                    sql_M_MATERIAL_COOD = "select  " +
                        "C_TFE_UP,C_TFE_LOWER," +
                        "C_CAO_UP,C_CAO_LOWER," +
                        "C_SIO2_UP,C_SIO2_LOWER," +
                        "C_MGO_UP,C_MGO_LOWER," +
                        "C_AL2O3_UP,C_AL2O3_LOWER ," +
                        "C_P_UP,C_P_LOWER," +
                        "C_S_UP,C_S_LOWER," +
                        "C_MNO_UP,C_MNO_LOWER  " +
                        "from M_MATERIAL_COOD where  L2_CODE = " + _L2_code + " ";
                }
                else if (_flag == 6)
                {
                    //获取上下限
                     sql_M_MATERIAL_COOD = "select  " +
                        "C_TFE_UP,C_TFE_LOWER," +
                        "C_CAO_UP,C_CAO_LOWER," +
                        "C_SIO2_UP,C_SIO2_LOWER," +
                        "C_MGO_UP,C_MGO_LOWER," +
                        "C_AL2O3_UP,C_AL2O3_LOWER ," +
                        "C_P_UP,C_P_LOWER," +
                        "C_S_UP,C_S_LOWER," +
                        "C_MNO_UP,C_MNO_LOWER  " +
                        "from M_MATERIAL_COOD where  L2_CODE = " + _L2_code + " ";
                }
                    
                DataTable data = _dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                if (data.Rows.Count > 0)
                {
                    List<double> _list = new List<double>();
                    //获取指定的上下限
                    for (int x = 0; x < data.Columns.Count; x++)
                    {
                        _list.Add(double.Parse(data.Rows[0][x].ToString()));
                    }
                    return _list;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            
        }

    }
}
