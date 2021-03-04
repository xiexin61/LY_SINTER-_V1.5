using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;

namespace SCZZJH_lib
{
    public class Class1
    {
        public void function()
        {
            //每5分钟调用一次，每次调用该函数把12个时间段的值存入list中，
            //12个时间段的查询时间开始时间和结束时间不同，查询条件都相同
            //如果当前时间是8点-24点，获取当日时间查询8-24，明天的0-8
            //如果当前时间是0点-8点，获取前一天的8-24，当天的0-8
            for(int i = 0; i <= 12; i++)
            {

            }
        }
    }

}
