using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY_SINTER.Model
{
    class Parameter_Model
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        /// <summary>
        /// 返回烧结生产信息记录配置表
        /// flag =1 原因大类
        /// flag = 2 原因小类
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, DataTable> _GetTs(int flag)
        {
            try
            {
                if (flag == 1 )//原因大类
                {
                    var sql_1 = "  select FLAG_1 AS 'VALUES',DESCRIBE AS NAME from M_PRO_RECORDS_CONFIG_1 where FLAG = 1";
                    DataTable table_1 = dBSQL.GetCommand(sql_1);
                    if (table_1 != null && table_1.Rows.Count > 0 )
                    {
                        return new Tuple<bool, DataTable>(true, table_1);
                    }
                    else
                    {
                        return new Tuple<bool, DataTable>(false, null);
                    }
                }
                else//原因小类
                {
                    var sql_1 = "  select FLAG_1 AS 'VALUES',DESCRIBE AS NAME from M_PRO_RECORDS_CONFIG_1 where FLAG = 2";
                    DataTable table_1 = dBSQL.GetCommand(sql_1);
                    if (table_1 != null && table_1.Rows.Count > 0)
                    {
                        return new Tuple<bool, DataTable>(true, table_1);
                    }
                    else
                    {
                        return new Tuple<bool, DataTable>(false, null);
                    }
                }
            }
            catch
            {
                return new Tuple<bool, DataTable>(false, null);
            }
        }
    }
}
