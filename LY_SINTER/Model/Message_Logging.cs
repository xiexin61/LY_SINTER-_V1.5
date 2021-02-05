using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLog;

namespace LY_SINTER.Model
{
    class Message_Logging
    {
        public vLog _vLog { get; set; }
        public Message_Logging()
        {
           
        }
        /// <summary>
        /// 消息推送接口
        /// 具体操作：_incident；
        /// 页面名称：_name；
        /// 模型名称： modelname；
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_incident"></param>
        /// <param name="_modelname"></param>
        /// <param name="_model"></param>
        public void Operation_Log( string _incident, string _name, string _modelname)
        { 
            try
            {
                DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
                string sql_Log = "INSERT INTO LogTable (timestamp,modelname,funcname,info ,FLAG,FLAG_alarm) " +
                    "VALUES (GETDATE(),'" + _modelname + "','" + _name + "','" + _incident + "','1','0');";
                _dBSQL.CommandExecuteNonQuery(sql_Log);

            }
            catch (Exception ee)
            {
                _vLog = new vLog(".\\log_Page\\Frame\\Loger_Frame\\");
                var MISTAKE = "operation_log()方法消息添加失败" + ee.ToString();
            }
        }
        /// <summary>
        /// 传入数据判断有效性
        /// </summary>
        /// <param name="vMax">最大值</param>
        /// <param name="vMin">最小值</param>
        /// <param name="vVal">输入的当前值</param>
        /// <returns></returns>
        public bool JudgeOk(double vMax, double vMin, string vVal)
        {
            try
            {
                if (vVal != "")
                {
                    double _VAL = double.Parse(vVal.ToString());
                    if (_VAL >= vMin && _VAL <= vMax)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ee)
            {
                _vLog = new vLog(".\\log_Page\\Frame\\Loger_Frame\\");
                string mistake = "JudgeOk()方法判断用户输入上下限失败，上限" + vMax.ToString() + "下限:" + vMin.ToString() + "判断值 ：" + vVal.ToString() + ee.ToString();
                _vLog.writelog(mistake, -1);
                return true;
            }
        }

        
    }
}
