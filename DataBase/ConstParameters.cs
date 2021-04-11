using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class ConstParameters
    {
        // public const string strCon = "Data Source = .;Initial Catalog = NBSJ;User Id = sa;Password = Yjs88291280;Integrated Security=false;";
        //
        //数据库链接语句
        //public const string strCon = "Data Source = 10.5.130.28;Initial Catalog = LGSJ;User Id = sa;Password = Yjs88291280;";
        public const string strCon = "Data Source = .;Initial Catalog = LGSJ;User Id = sa;Password = Yjs88291280;";

        //凌钢现场
      //  public const string strCon = "Data Source = 192.168.100.2;Initial Catalog = LGSJ;User Id = sa;Password = Yjs88291280;";

        //  public const string strCon = "Data Source = 10.9.11.1\\NGSJ;Initial Catalog = NBSJ;User Id = sa;Password = Yjs88291280;Integrated Security=false;";
        //下发程序ID地址
        //public const string strCon_ID = "127.0.0.1";
        public const string strCon_ID = "192.168.100.3";

        // public const string strCon_ID = "10.202.11.1";
        //端口号
        // public const int PORT = 20201;
        public const int PORT = 50006;

        //下发个数
        // public const int _COUNT = 59;
        public const int _COUNT = 1;

        //下发频率
        //  public const int _COUNT_1 = 21;//
        public const int _COUNT_1 = 20;//屏蔽设定下料量
    }
}