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
    //数据处理接口
    class Data_Treating
    {
        public vLog _vLog { get; set; }
       // DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        public Data_Treating()
        {
            _vLog = new vLog(".\\Data_Treating\\");
            _vLog.connstring = DataBase.ConstParameters.strCon;
        }
        /// <summary>
        /// 数据临时存储
        /// _db:连接对象
        /// Col_Key：操作表检索关键字段
        /// Table：操作表
        /// Date_key：操作表检索关键字
        /// Table_CaChe：临时缓存表
        /// CaChe_Count：临时缓存表存储量
        /// Table_Cache_ColKEy：临时缓存表排序关键字
        /// FALG:1：通过触发器清空临时表，2：通过方法清空临时表
        /// </summary>
        /// <param name="Col_Key">操作表检索关键字段</param>
        /// <param name="Table"></param>
        /// <param name="Date_key"></param>
        /// <param name="Table_CaChe"></param>
        /// <returns></returns>
        public int Data_Caching_Mechanism(DBSQL _db, string Col_Key,string Table,string Date_key,string Table_CaChe,int CaChe_Count = 10,string Table_Cache_ColKEY ="",int FALG = 1)
        {
            try
            {
                var _sql = " insert into "+ Table_CaChe + "  select TOP(1) * from "+ Table + " where "+ Col_Key + " = '"+ Date_key + "'";
                int count = _db.CommandExecuteNonQuery(_sql);
                if (count == 1)
                {
                    if (FALG == 2)
                    {
                        var _sql1 = "select "+ Table_Cache_ColKEY + " from " + Table_CaChe + " order by "+ Table_Cache_ColKEY + " desc";
                        DataTable _A = _db.GetCommand(_sql1);
                        if (_A.Rows.Count > CaChe_Count && _A != null)
                        {
                            for (int x = CaChe_Count;x< _A.Rows.Count;x++)
                            {
                                var _del = "delete from "+ Table_CaChe + " where "+ Table_Cache_ColKEY + " = '"+ _A.Rows[x][0].ToString() + "'";
                                int count_1 = _db.CommandExecuteNonQuery(_del);
                                if (count_1!= 1)
                                {
                                    _vLog.writelog("执行sql语句"+_del+"错误",-1);
                                    return -1;
                                }
                            }
                            return 1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 1;
                    }
                    //var sql
                    
                }
                else
                {
                    var mistake = _vLog.writelog("执行sql语句" + _sql + "错误",-1);
                    return 1;
                }
               
            }
            catch(Exception ee)
            {
                _vLog.writelog(ee.ToString(),-1);
                return -1;
            }
            
        }

    }
}
