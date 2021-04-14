using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLog;

namespace LY_SINTER.Model
{
    
    class dataTable_Point
    {
        public vLog _vLog { get; set; }
        public dataTable_Point()
        {
            _vLog = new vLog(".\\Data_Treating\\");
            _vLog.connstring = DataBase.ConstParameters.strCon;

        }
        /// <summary>
        /// 设置小数位数
        /// </summary>
        /// <param name="dataTable">表格</param>
        /// <param name="startColNum">开始的列号</param>
        /// <param name="endColNum">结束的列号</param>
        /// <returns></returns>
        public DataTable Format_Point(DataTable dataTable, int startColNum, int endColNum)
        {
            DataTable table = new DataTable();
            return table;
            /*dataTable.Columns[startColNum].DefaultValue;
            return DataTable ;*/
        }

    }
}
