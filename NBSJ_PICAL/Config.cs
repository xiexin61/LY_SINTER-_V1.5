using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSJ_PICAL
{
   public  class Config
    {
        /// <summary>
        /// Account have permission to create database
        /// 用有建库权限的数据库账号
        /// </summary>
        
public static string ConnectionString = "server=127.0.0.1;uid=sa;pwd=Yjs88291280;database=LGSJ";
   // public static string ConnectionString = "server=10.202.11.1\\NGSJ;uid=sa;pwd=Yjs88291280;database=NBSJ";
    
     //    public static string ConnectionString = "  Data Source = .; Initial Catalog = NBsj; User Id = sa; Password = Yjs88291280;Integrated Security = false";

        /// <summary>
        /// Account have permission to create database
        /// 用有建库权限的数据库账号
        /// </summary>
        public static string ConnectionString2 = "server=.;uid=sa;pwd=haosql;database=SQLSUGAR4XTEST2";
        /// <summary>
        /// Account have permission to create database
        /// 用有建库权限的数据库账号
        /// </summary>
        public static string ConnectionString3 = "server=.;uid=sa;pwd=haosql;database=SQLSUGAR4XTEST3";
    }
}
