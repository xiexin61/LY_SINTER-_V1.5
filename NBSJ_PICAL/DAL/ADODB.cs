using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSJ_PICAL
{
    public class ADODB
    {
        /// <summary>
        /// Account have permission to create database
        /// 用有建库权限的数据库账号
        /// </summary>
        //   public static string ConnectionString = "server=127.0.0.1;uid=sa;pwd=Yjs88291280;database=LGSJ";
        //   public static string ConnectionString = "server=192.168.100.2;uid=sa;pwd=Yjs88291280;database=LGSJ";
        public static string ConnectionString = "Data Source = 192.168.100.2;Initial Catalog = LGSJ;User Id = sa;Password = Yjs88291280";

        public const string strCon = "Data Source = 192.168.100.2;Initial Catalog = LGSJ;User Id = sa;Password = Yjs88291280;";
        //  public static string ConnectionString = "server=127.0.0.1;uid=QM;pwd=admin;database=LGSJ";
        // public static string ConnectionString = "server=10.202.11.1\\NGSJ;uid=sa;pwd=Yjs88291280;database=NBSJ";
    }
}