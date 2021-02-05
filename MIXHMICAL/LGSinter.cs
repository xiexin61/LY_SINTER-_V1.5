using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLog;

namespace MIXHMICAL
{
    public class LGSinter
    {
        //test
        //   public string _connstring = @"Data Source = 127.0.0.1;Initial Catalog = LGSJ;User Id = QM;Password = admin;Integrated Security=false;";

        //   public string _connstring = @"Data Source =127.0.0.1;Initial Catalog = LGSJ;User Id = sa;Password = Yjs88291280;Integrated Security=false;";
        public string _connstring = DataBase.ConstParameters.strCon;
        private int[] L2Code = { 101, 102, 501, 601 };
        public vLog mixlog { get; set; }
        public LGSinter()
        {
            mixlog = new vLog(".\\log\\MixModel\\");
            mixlog.connstring = _connstring;
        }
        //检测设定配比值是否合法
        public bool CheckSetPB(int canghao)
        {
            //连接数据库
            DBSQL _dbs = new DBSQL(_connstring);
            //通过仓号查询配比ID,并获取配比ID对应的所有仓号
            string sql = "select canghao from dbo.CFG_MAT_L2_PBSD_INTERFACE where peinimingcheng=(select p.peinimingcheng from dbo.CFG_MAT_L2_PBSD_INTERFACE p where p.canghao=" + canghao + ")";
            DataTable _dt = _dbs.GetCommand(sql);
            if (_dt == null)
            {
                Console.WriteLine("CheckSetPB:数据库查询失败，请检查数据库连接是否正常");
                return false;
            }
            else
            {
                var selrs = _dt.AsEnumerable();
                string sql_0 = "select COUNT(*) from dbo.CFG_MAT_L2_XLK_INTERFACE where MAT_L2_XLKZT=1 and ";
                string sql_1 = "";
                int len = selrs.Count();
                for (int i = 0; i < len; i++)
                {
                    DataRow _row = selrs.ElementAt(i);
                    if (i == len - 1)
                    {
                        sql_1 += "MAT_L2_CH=" + (int)_row["MAT_L2_CH"];
                    }
                    else
                    {
                        sql_1 += "MAT_L2_CH=" + (int)_row["MAT_L2_CH"] + " or ";
                    }

                }
                sql = sql_0 + sql_1;
                //执行检测语句
                var _countrs = _dbs.GetCommand(sql).AsEnumerable();
                DataRow _crow = _countrs.ElementAt(0);
                int counts = (int)_crow[0];
                if (counts > 0)
                {
                    return true;//该配比对应的下料口至少有一个是启用状态
                }
                else
                {
                    return false;//该配比对应的下料口全部是禁用状态
                }
            }
        }


        //更新设定配比
        public bool UpdateSetPB(int canghao, float pbVal)
        {
            //连接数据库
            DBSQL _dbs = new DBSQL(_connstring);
            //通过仓号查询配比ID,并更新设定配比值
            string sql = "update dbo.CFG_MAT_L2_PBSD_INTERFACE set peibizhi=" + pbVal + " where peinimingcheng=(select p.peinimingcheng from dbo.CFG_MAT_L2_PBSD_INTERFACE p where p.canghao=" + canghao + " );";
            //执行操作
            int _rs = _dbs.CommandExecuteNonQuery(sql);
            if (_rs > 0)
            {
                Console.WriteLine("设定配比更新成功");
            }
            else
            {
                Console.WriteLine("设定配比更新失败");
            }
            return true;
        }



        //配比%计算

        /// <summary>
        /// 
        /// 当前配比% curpb
        /// mode 配比类型
        /// 1：设定配比,计算设定%；2：当前配比，计算当前%
        /// </summary>
        public int CalPB(int mode)
        {
            DBSQL _mdb = new DBSQL(_connstring);
            //配比计算
            string sql = "select distinct peinimingcheng,peibizhi from dbo.CFG_MAT_L2_PBSD_INTERFACE";
            // 执行sql语句
            DataTable _dt = _mdb.GetCommand(sql);
            if (mode == 1)
            {
                //设定配比计算
                Dictionary<int, Tuple<int, float>> _Dic = new Dictionary<int, Tuple<int, float>>();
                float PbSum = 0;
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    var _r = _dt.Rows[i];
                    int rr = int.Parse(_r[0].ToString());
                    float fr = float.Parse(_r[1].ToString());
                    _Dic.Add(i, new Tuple<int, float>(rr, fr));//配比ID，配比值
                    PbSum += fr;
                }
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    Tuple<int, float> _temp = new Tuple<int, float>(_Dic[i].Item1, _Dic[i].Item2 / PbSum * 100);
                    _Dic.Remove(i);
                    _Dic.Add(i, _temp);

                }

                //更新数据库表
                string sql_0 = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SDBFB=";

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    Tuple<int, float> _temp = new Tuple<int, float>(_Dic[i].Item1, _Dic[i].Item2);

                    string usql = "";
                    usql = sql_0 + _temp.Item2 + " where MAT_PB_ID=" + _temp.Item1;

                    int rs = _mdb.CommandExecuteNonQuery(usql);
                    if (rs > 0)
                    {
                        //插入日志表
                        Console.WriteLine("更新数据库表CFG_MAT_L2_SJPB_INTERFACE成功({0})", rs);
                    }
                    else
                    {
                        //插入日志表
                        Console.WriteLine("更新数据库表CFG_MAT_L2_SJPB_INTERFACE失败({0})", usql);
                        return -1;
                    }
                }

            }
            else if (mode == 2)
            {
                //当前配比计算
                Dictionary<int, Tuple<int, float, float>> _Dic = new Dictionary<int, Tuple<int, float, float>>();
                float PbSum = 0;
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    var _r = _dt.Rows[i];
                    int rr = int.Parse(_r[0].ToString());
                    float fr = float.Parse(_r[1].ToString());
                    _Dic.Add(i, new Tuple<int, float, float>(rr, fr, fr));//配比ID，配比值
                    PbSum += fr;
                }
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    Tuple<int, float, float> _temp = new Tuple<int, float, float>(_Dic[i].Item1, _Dic[i].Item2 / PbSum * 100, _Dic[i].Item3);
                    _Dic.Remove(i);
                    _Dic.Add(i, _temp);
                }

                //更新数据库表
                string sql_0 = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_DQPB=";

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    Tuple<int, float, float> _temp = new Tuple<int, float, float>(_Dic[i].Item1, _Dic[i].Item2, _Dic[i].Item3);

                    string sql_1 = sql_0 + _temp.Item3 + " ,MAT_L2_DQBFB= " + _temp.Item2 + " where MAT_PB_ID=" + _temp.Item1;

                    int rs = _mdb.CommandExecuteNonQuery(sql_1);
                    if (rs > 0)
                    {
                        //插入日志表
                        Console.WriteLine("更新数据库表CFG_MAT_L2_SJPB_INTERFACE成功({0})", rs);
                    }
                    else
                    {
                        //插入日志表
                        Console.WriteLine("更新数据库表CFG_MAT_L2_SJPB_INTERFACE失败({0})", sql_0);
                        return -2;
                    }

                }



            }
            else
            {
                //插入日志表
                Console.WriteLine("配比计算模式错误({0})", mode);
                return -3;
            }



            return 1;
        }
        #region
        ////
        ////分仓系数计算
        ////
        ///// <summary>
        ///// 
        ///// 当前配比% curpb
        ///// 配比ID sid
        ///// 计算模式 style 1:其他；2：启停信号调整
        ///// </summary>
        //public int BHouse(int sid,float curpb,int style)
        //{
        //    //
        //    DBSQL _mdb = new DBSQL(_connstring);

        //    ////构造数据库查询语句,查询配比


        //    //根据配比ID，查询对应的下料口
        //    string sql = "select MAT_L2_XLKZT,MAT_L2_SIGN,MAT_L2_GXLBL,MAT_L2_CH,MAT_L2_XLK from dbo.CFG_MAT_L2_XLK_INTERFACE where MAT_PB_ID=" + sid;
        //    //每次调用方法，从三秒表C_PLC_3S更新最新的启停信号状态
        //    string sql_plc_sign = "select top 1 T_SL_1_3S,T_SL_2_3S,T_SL_3_3S,T_SL_4_3S,T_SL_5_3S,T_SL_6_3S,T_SL_7_3S,T_SL_8_3S,T_SL_9_3S,T_SL_10_3S,T_SL_11_3S,T_SL_12_3S,T_SL_13_3S,T_SL_14_3S,T_SL_15_3S,T_SL_16_3S,T_SL_17_3S,T_SL_18_3S,T_SL_19_3S from dbo.C_PLC_3S order by TIMESTAMP desc ";

        //    //执行sql语句
        //    DataTable _dt = _mdb.GetCommand(sql);
        //    //**** 执行sql语句 从plc三秒表获取启停信号插入到CFG_MAT_L2_XLK_INTERFACE中对应的下料口启停状态
        //    DataTable _dt_plc_sign = _mdb.GetCommand(sql_plc_sign);
        //    var _vdt_plc_sign = _dt_plc_sign.AsEnumerable();
        //   // int count = int.Parse(_vdt_plc_sign.LongCount().ToString());
        //    int[] sign = new int[19] ; 

        //    for (int i = 0; i < 19; i++)
        //    {
        //        sign[i] = int.Parse(_vdt_plc_sign.ElementAt(0)[i].ToString());
        //        int CH = i + 1;
        //        string sql_sign = "update [CFG_MAT_L2_XLK_INTERFACE] set MAT_L2_XLKZT =  "+ sign[i] + " where MAT_L2_XLK = "+ CH + "";
        //        int _rs = _mdb.CommandExecuteNonQuery(sql_sign);
        //        if (_rs >= 0)
        //        {
        //            //请写入日志表
        //            string logstr = " plc下料口状态数据更新成功(sql =" + sql_sign + ")";

        //            Console.WriteLine("plc下料口状态数据更新成功(sql={0})", sql_sign);
        //        }
        //        else
        //        {
        //            //请写入日志表
        //            Console.WriteLine("plc下料口状态数据更新失败(sql={0})", sql_sign);
        //            return -1;
        //        }


        //    }



        //    var _vdt = _dt.AsEnumerable();
        //    float Sum = 0;//每个非活动料口的下发比例之和
        //                  //分仓系数计算
        //    float rsFC = 0;//每个非活动料口的分仓系数
        //    float rsFCLive = 0f;//活动料口的分仓系数
        //    //位置，仓号，下料口，下料比例,是否活动
        //    Dictionary<int, Tuple<int, int, float, int>> FCk = new Dictionary<int, Tuple<int, int, float, int>>();

        //    if (style==1)
        //    {
        //        // int _fcIndex = 0;
        //        int p0 = 0, p1 = 0, p3 = 0, p4 = 0;
        //        float p2 = 0;
        //        for (int i = 0; i < _vdt.Count(); i++)
        //        {
        //            p0 = int.Parse(_vdt.ElementAt(i)[0].ToString());
        //            p1 = int.Parse(_vdt.ElementAt(i)[1].ToString());
        //            p2 = float.Parse(_vdt.ElementAt(i)[2].ToString());
        //            p3 = int.Parse(_vdt.ElementAt(i)[3].ToString());
        //            p4 = int.Parse(_vdt.ElementAt(i)[4].ToString());
        //            rsFC = p0 * p2 / curpb;
        //            FCk.Add(i, new Tuple<int, int, float, int>(p3, p4, rsFC, p1));
        //            if (p0 == 1 && p1 == 0)//启用状态，非活动料口求和
        //                Sum += p0 * p2;
        //        }
        //        //计算活动料口
        //        rsFCLive = 1 - Sum / curpb;
        //        for (int i = 0; i < _vdt.Count(); i++)
        //        {
        //            if (FCk[i].Item4 == 1)
        //            {
        //                Tuple<int, int, float, int> _temp = FCk[i];
        //                FCk.Remove(i);
        //                FCk.Add(i, new Tuple<int, int, float, int>(_temp.Item1, _temp.Item2, rsFCLive, _temp.Item4));
        //                break;
        //            }
        //        }
        //    }
        //    else if(style==2)
        //    {
        //        int p0 = 0, p1 = 0, p3 = 0, p4 = 0;
        //        float p2 = 0;
        //        for (int i = 0; i < _vdt.Count(); i++)
        //        {
        //            p0 = int.Parse(_vdt.ElementAt(i)[0].ToString());
        //            p1 = int.Parse(_vdt.ElementAt(i)[1].ToString());
        //            p2 = float.Parse(_vdt.ElementAt(i)[2].ToString());
        //            p3 = int.Parse(_vdt.ElementAt(i)[3].ToString());
        //            p4 = int.Parse(_vdt.ElementAt(i)[4].ToString());
        //            if (p0 == 1)//启用状态
        //            {
        //                rsFC = curpb / _vdt.Count();
        //                FCk.Add(i, new Tuple<int, int, float, int>(p3, p4, rsFC, p1));
        //            }
        //            else
        //            {
        //                rsFC = 0;
        //                FCk.Add(i, new Tuple<int, int, float, int>(p3, p4, rsFC, p1));
        //            }  

        //        }
        //    }



        //    //更新数据库


        //    for (int k = 0; k<_vdt.Count();k++)
        //    {
        //        //更新分仓系数
        //        string usql = "update dbo.CFG_MAT_L2_XLK_INTERFACE set MAT_L2_FCXS="+FCk[k].Item3+ " where MAT_L2_CH="+FCk[k].Item1+" and MAT_L2_XLK="+FCk[k].Item2;//

        //        //
        //        //执行sql语句
        //        int _rs = _mdb.CommandExecuteNonQuery(usql);
        //        if (_rs >= 0)
        //        {
        //            //请写入日志表
        //            string logstr = " 数据更新成功(sql =" + usql + ")";

        //            Console.WriteLine("数据更新成功(sql={0})", usql);
        //        }
        //        else
        //        {
        //            //请写入日志表
        //            Console.WriteLine("数据更新失败(sql={0})", usql);
        //            return -1;
        //        }
        //        //
        //    }


        //    return 0;

        //}
        #endregion

        //
        //分仓系数计算
        //
        /// <summary>
        /// 
        /// 当前配比% curpb
        /// 配比ID sid
        /// 计算模式 style 1:其他；2：启停信号调整;3:按照下料量计算分仓系数
        /// </summary>
        public int BHouse(int sid, float curpb, int style)

        {
            //
            DBSQL _mdb = new DBSQL(_connstring);

            ////构造数据库查询语句,查询配比


            //根据配比ID，查询对应的下料口
            string sql = "select MAT_L2_XLKZT,MAT_L2_SIGN,MAT_L2_GXLBL,MAT_L2_CH,MAT_L2_XLK from dbo.CFG_MAT_L2_XLK_INTERFACE where MAT_PB_ID=" + sid;
            //执行sql语句
            DataTable _dt = _mdb.GetCommand(sql);
            var _vdt = _dt.AsEnumerable();
            float Sum = 0;//每个非活动料口的下发比例之和
                          //分仓系数计算
            float rsFC = 0;//每个非活动料口的分仓系数
            float rsFCLive = 0f;//活动料口的分仓系数
            //位置，仓号，下料口，下料比例,是否活动
            Dictionary<int, Tuple<int, int, float, int>> FCk = new Dictionary<int, Tuple<int, int, float, int>>();

            if (style == 1)
            {
                // int _fcIndex = 0;
                int p0 = 0, p1 = 0, p3 = 0, p4 = 0;
                float p2 = 0;
                for (int i = 0; i < _vdt.Count(); i++)
                {
                    p0 = int.Parse(_vdt.ElementAt(i)[0].ToString());
                    p1 = int.Parse(_vdt.ElementAt(i)[1].ToString());
                    p2 = float.Parse(_vdt.ElementAt(i)[2].ToString());
                    p3 = int.Parse(_vdt.ElementAt(i)[3].ToString());
                    p4 = int.Parse(_vdt.ElementAt(i)[4].ToString());
                    rsFC = p0 * p2 / curpb;
                    FCk.Add(i, new Tuple<int, int, float, int>(p3, p4, rsFC, p1));
                    if (p0 == 1 && p1 == 0)//启用状态，非活动料口求和
                        Sum += p0 * p2;
                }
                //计算活动料口
                rsFCLive = 1 - Sum / curpb;
                for (int i = 0; i < _vdt.Count(); i++)
                {
                    if (FCk[i].Item4 == 1)
                    {
                        Tuple<int, int, float, int> _temp = FCk[i];
                        FCk.Remove(i);
                        FCk.Add(i, new Tuple<int, int, float, int>(_temp.Item1, _temp.Item2, rsFCLive, _temp.Item4));
                        break;
                    }
                }
            }
            else if (style == 2)
            {
                int p0 = 0, p1 = 0, p3 = 0, p4 = 0;
                float p2 = 0;
                for (int i = 0; i < _vdt.Count(); i++)
                {
                    p0 = int.Parse(_vdt.ElementAt(i)[0].ToString());
                    p1 = int.Parse(_vdt.ElementAt(i)[1].ToString());
                    p2 = float.Parse(_vdt.ElementAt(i)[2].ToString());
                    p3 = int.Parse(_vdt.ElementAt(i)[3].ToString());
                    p4 = int.Parse(_vdt.ElementAt(i)[4].ToString());
                    if (p0 == 1)//启用状态
                    {
                        // rsFC = curpb / _vdt.Count();
                        rsFC = 1.0f / _vdt.Count();
                        FCk.Add(i, new Tuple<int, int, float, int>(p3, p4, rsFC, p1));
                    }
                    else
                    {
                        rsFC = 0;
                        FCk.Add(i, new Tuple<int, int, float, int>(p3, p4, rsFC, p1));
                    }

                }
            }
            else if (style == 3)//20200203
            {
                //求解新旧差值
                // val:配比，新旧差值绝对值,新旧差值
                var _A = BMethodFinal();
                //非活动旧下料口 下料量
                //string sql_1 = "select p.MAT_PB_ID,SUM(p.MAT_L2_SDXL) as sumxl from dbo.CFG_MAT_L2_XLK_INTERFACE p  where p.MAT_L2_SIGN=0  and p.MAT_L2_XLKZT=1  group by p.MAT_PB_ID  order by p.MAT_PB_ID";
                string sql_1 = "select p.MAT_PB_ID,p.MAT_L2_SDXL,p.MAT_L2_XLK,p.MAT_L2_SIGN from dbo.CFG_MAT_L2_XLK_INTERFACE p  where p.MAT_L2_XLKZT=1  order by p.MAT_L2_XLK";
                DataTable _dtt = _mdb.GetCommand(sql_1);
                var _vdtt = _dtt.AsEnumerable();
                float newSum = 0;
                //下料口，下料量
                Dictionary<int, float> _New = new Dictionary<int, float>();
                foreach (var x in _vdtt)
                {
                    int _p0 = int.Parse(x[0].ToString());//PBID
                    float _p1 = float.Parse(x[1].ToString());//下料量
                    int _p2 = int.Parse(x[2].ToString());//下料口
                    int _p3 = int.Parse(x[3].ToString());//是否为活动料口

                    if (_New.ContainsKey(_p2))
                    {
                        continue;
                    }
                    else
                    {
                        if (_p3 == 0)//非活动料口
                        {
                            _New.Add(_p2, _p1);
                            newSum += _p1;
                        }
                        else if (_p3 == 1)//活动料口
                        {
                            float detaSum = 0;
                            for (int i = 0; i < _A.Count; i++)
                            {
                                if (_A[i].Item1 == _p0)
                                {
                                    detaSum = _A[i].Item3;
                                    break;
                                }
                            }
                            float _temp = _p1 + detaSum;
                            newSum += _temp;
                            _New.Add(_p2, _temp);
                        }
                        else
                        {
                            Console.WriteLine("活动料口状态异常!");
                            return -1;
                        }

                    }
                }
                //每个下料口的下料量湿百分比（LCS_j）求解
                //LCS_j = W_NEW_j / SUM_W_NEW * 100
                //每个下料口的干下料比例（%）（DRY_BILL_j）
                //索引，配比ID，下料口，下料量湿百分比，干下料比例（%）
                Dictionary<int, Tuple<int, int, float, float>> _midVal = new Dictionary<int, Tuple<int, int, float, float>>();
                for (int j = 0; j < _New.Count; j++)
                {
                    if (_midVal.ContainsKey(j))
                    {
                        continue;
                    }
                    else
                    {
                        int pbid = int.Parse(_vdtt.ElementAt(j)[0].ToString());
                        int xlk = int.Parse(_vdtt.ElementAt(j)[2].ToString());
                        float wetpb = _New[xlk] / newSum * 100;
                        float drypb = 0;
                        Tuple<int, int, float, float> _temp = new Tuple<int, int, float, float>(pbid, xlk, wetpb, drypb);
                        _midVal.Add(j, _temp);
                    }
                }

                //当前水分
                string sql_11 = "select t1.MAT_L2_CH,t1.MAT_L2_XLK,t1.MAT_L2_GXLBL,t2.MAT_L2_SFDQ,t1.MAT_PB_ID,t1.MAT_L2_XLKZT,t1.MAT_L2_SIGN from dbo.CFG_MAT_L2_XLK_INTERFACE t1 inner join(select MAT_L2_CH, MAT_L2_SFDQ from dbo.CFG_MAT_L2_SJPB_INTERFACE) t2 on t1.MAT_L2_CH = t2.MAT_L2_CH where t1.MAT_L2_XLKZT=1";
                //执行sql
                DataTable _dt11 = _mdb.GetCommand(sql_11);
                var _vdt11 = _dt11.AsEnumerable();
                //下料口，val
                List<Tuple<int, float>> _list = new List<Tuple<int, float>>();
                float h2oSum = 0;

                foreach (var b in _vdt11)
                {
                    int _p1 = int.Parse(b[1].ToString());
                    float _wetpb = _New[_p1] / newSum * 100;
                    float _drtbp = _wetpb * (100 - float.Parse(b[3].ToString())) / 100;
                    _list.Add(new Tuple<int, float>(_p1, _drtbp));
                    h2oSum += _drtbp;
                }
                //计算干下料比例（%）
                for (int i = 0; i < _list.Count; i++)
                {
                    Tuple<int, float> _temp = new Tuple<int, float>(_list[0].Item1, _list[0].Item2 / h2oSum * 100);
                    _list.RemoveAt(0);
                    _list.Add(_temp);
                }

                //计算分仓系数

                List<int> xlks = new List<int>();
                foreach (var c in _vdt)
                {
                    int _xlk = int.Parse(c[4].ToString());
                    xlks.Add(_xlk);
                }
                float drySumPb = 0;
                foreach (var c in _list)
                {
                    if (xlks.Contains(c.Item1))
                    {
                        drySumPb += c.Item2;
                    }
                }

                //位置，仓号，下料口，下料比例,是否活动
                //Dictionary<int, Tuple<int, int, float, int>> FCk = new Dictionary<int, Tuple<int, int, float, int>>();
                int p0 = 0, p1 = 0, p3 = 0, p4 = 0;
                float p2 = 0;
                for (int i = 0; i < _vdt.Count(); i++)
                {
                    p0 = int.Parse(_vdt.ElementAt(i)[0].ToString());
                    p1 = int.Parse(_vdt.ElementAt(i)[1].ToString());
                    p2 = float.Parse(_vdt.ElementAt(i)[2].ToString());
                    p3 = int.Parse(_vdt.ElementAt(i)[3].ToString());
                    p4 = int.Parse(_vdt.ElementAt(i)[4].ToString());
                    if (p0 == 1)//启用状态
                    {

                        if (xlks.Contains(p4))
                        {
                            rsFC = 0;
                            float _drypb = _list.Find(w => w.Item1.Equals(p4)).Item2;
                            rsFC = _drypb / drySumPb;
                        }
                        FCk.Add(i, new Tuple<int, int, float, int>(p3, p4, rsFC, p1));
                    }
                    else
                    {
                        rsFC = 0;
                        FCk.Add(i, new Tuple<int, int, float, int>(p3, p4, rsFC, p1));
                    }
                }
            }
            //更新数据库


            for (int k = 0; k < _vdt.Count(); k++)
            {
                //更新分仓系数
                string usql = "update dbo.CFG_MAT_L2_XLK_INTERFACE set MAT_L2_FCXS=" + FCk[k].Item3 + " where MAT_L2_CH=" + FCk[k].Item1 + " and MAT_L2_XLK=" + FCk[k].Item2;//

                //
                //执行sql语句
                int _rs = _mdb.CommandExecuteNonQuery(usql);
                if (_rs > 0)
                {
                    //请写入日志表
                    string logstr = " 数据更新成功(sql =" + usql + ")";

                    Console.WriteLine("数据更新成功(sql={0})", usql);
                }
                else
                {
                    //请写入日志表
                    Console.WriteLine("数据更新失败(sql={0})", usql);
                    return -1;
                }
                //
            }


            return 0;

        }

        /// <summary>
        /// 分仓系数 设定下料聊调整
        /// </summary>
        /// <param name="sid">配比ID</param>
        /// <param name="curpb">当前配比</param>
        /// <returns>
        /// item1: 是否满足调整活动料口
        /// item2: 满足条件的，活动料口的调整量
        /// </returns>
        public Tuple<bool, int, float> BMethod(int sid, float curpb)
        {

            //
            DBSQL _mdb = new DBSQL(_connstring);
            /*
             * ①该配比对应的加权平均水计算（SUM_H2O_i）：
                SUM_H2O_i+=第j个料口启停信号*第j个料口分仓系数/(SUM(料口启停信号*料口分仓系数)*该料仓的水分当前值
             */

            string sql_0 = "select * from ((select t.MAT_L2_FCXS, t.MAT_L2_XLKZT, t.MAT_L2_CH from dbo.CFG_MAT_L2_XLK_INTERFACE t where t.MAT_PB_ID = " + sid + ") w join (select t.MAT_L2_SFDQ, t.MAT_L2_CH from dbo.CFG_MAT_L2_SJPB_INTERFACE t where t.MAT_PB_ID = " + sid + ") m on w.MAT_L2_CH = m.MAT_L2_CH)cross join (select SUM(t.MAT_L2_FCXS* t.MAT_L2_XLKZT) as total from dbo.CFG_MAT_L2_XLK_INTERFACE t where t.MAT_PB_ID = " + sid + ") c ";

            DataTable _dt = _mdb.GetCommand(sql_0);

            var _vdt = _dt.AsEnumerable();

            float sum_h2o = 0f;
            //仓号，和(料口启停信号*料口分仓系数与配比对应)，加权平均水计算
            Dictionary<int, Tuple<float, float>> FcPlus = new Dictionary<int, Tuple<float, float>>();
            foreach (var x in _vdt)
            {
                int p0 = int.Parse(x[2].ToString());
                float p1 = float.Parse(x[5].ToString());
                float p2 = float.Parse(x[0].ToString());
                float p3 = float.Parse(x[1].ToString());
                float p4 = float.Parse(x[3].ToString());
                if (p1 == 0)
                {
                    return new Tuple<bool, int, float>(true, 0, 0);//该配比对应所有下料口全部禁用
                }
                if (FcPlus.ContainsKey(p0))
                {
                    float _temp = p2 * p3 / p1 * p4;
                    float _add = FcPlus[p0].Item2 + _temp;
                    FcPlus.Remove(p0);
                    FcPlus.Add(p0, new Tuple<float, float>(p1, _add));
                    sum_h2o += _temp;
                }
                else
                {
                    float _temp = p2 * p3 / p1 * p4;
                    FcPlus.Add(p0, new Tuple<float, float>(p1, _temp));
                    sum_h2o += _temp;
                }
            }

            /*②该配比对应的湿配比
                BILL_WET_i = 当前配比 / (100 - SUM_H2O_i) * 100
            */
            float BILL_WET_i = 0;
            BILL_WET_i = curpb / (100 - sum_h2o) * 100;

            return new Tuple<bool, int, float>(true, sid, BILL_WET_i);

        }
        /// <summary>
        /// 计算所有湿配比%
        /// </summary>
        /// /// <param name="mode">1:new 2:old</param>
        /// <returns>
        /// key:配比ID
        /// val:湿配比%
        /// </returns>
        public Dictionary<int, Tuple<int, float>> BMethods(int mode)
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            if (mode == 1)
            {
                string sql_0 = "select distinct p.MAT_PB_ID,p.MAT_L2_DQPB from dbo.CFG_MAT_L2_SJPB_INTERFACE p order by p.MAT_PB_ID asc";
                Dictionary<int, Tuple<int, float>> rs = new Dictionary<int, Tuple<int, float>>();
                //
                DataTable _dt = _mdb.GetCommand(sql_0);
                int p0 = 0;
                float p1 = 0;
                float sum = 0;
                var _vdt = _dt.AsEnumerable();
                List<Tuple<int, float>> singleWetPB = new List<Tuple<int, float>>();
                foreach (var x in _vdt)//求和
                {
                    p0 = int.Parse(x[0].ToString());
                    p1 = float.Parse(x[1].ToString());
                    var y = BMethod(p0, p1);
                    singleWetPB.Add(new Tuple<int, float>(p0, y.Item3));
                    sum += y.Item3;
                }
                for (int i = 0; i < singleWetPB.Count; i++)
                {
                    float _temp = singleWetPB[i].Item2;
                    int key = singleWetPB[i].Item1;
                    _temp = _temp / sum;// * 100;
                    if (rs.ContainsKey(key))
                    {
                        Console.WriteLine("配比键值重复，请检查配比设定表！");
                        return null;
                    }
                    else
                    {
                        rs.Add(i, new Tuple<int, float>(key, _temp));
                    }
                }

                return rs;
            }
            else if (mode == 2)
            {
                string sql_0 = "select t.MAT_PB_ID,SUM(t.MAT_L2_SDXL) from dbo.CFG_MAT_L2_XLK_INTERFACE t group by t.MAT_PB_ID order by t.MAT_PB_ID";
                Dictionary<int, Tuple<int, float>> rs = new Dictionary<int, Tuple<int, float>>();
                DataTable _dt = _mdb.GetCommand(sql_0);
                var _vdt = _dt.AsEnumerable();
                int i = 0;
                foreach (var x in _vdt)
                {
                    int p0 = int.Parse(x[0].ToString());
                    float p1 = float.Parse(x[1].ToString());
                    rs.Add(i, new Tuple<int, float>(p0, p1));
                    i++;
                }
                return rs;
            }
            else
            {
                Console.WriteLine("输入计算模式异常");
                return null;
            }
        }

        /// <summary>
        /// 终极湿配比计算
        /// </summary>
        /// <returns>
        /// key:索引
        /// val:配比，新旧差值绝对值,新旧差值
        /// </returns>
        public Dictionary<int, Tuple<int, float, float>> BMethodFinal()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //
            string sql_0 = "select top 1 p.MAT_L2_SP from dbo.CFG_MAT_L2_PLZL_INTERFACE p order by TIMESTAMP desc";

            DataTable _dt = _mdb.GetCommand(sql_0);

            var _vdt = _dt.AsEnumerable();

            float sp = 0;

            foreach (var x in _vdt)
            {
                sp = float.Parse(x[0].ToString());
                break;
            }

            Dictionary<int, Tuple<int, float, float>> rs = new Dictionary<int, Tuple<int, float, float>>();
            var Bfnew = BMethods(1);
            for (int i = 0; i < Bfnew.Count; i++)
            {
                int p0 = Bfnew[i].Item1;
                float p1 = Bfnew[i].Item2 * sp;
                rs.Add(i, new Tuple<int, float, float>(p0, p1, p1));
            }
            var Bfold = BMethods(2);
            for (int i = 0; i < Bfold.Count; i++)
            {
                int p0 = Bfold[i].Item1;
                float p1 = Bfold[i].Item2;
                float diff = Math.Abs(rs[i].Item2 - p1);
                float _diff = rs[i].Item2 - p1;
                if (rs.ContainsKey(i))
                {
                    rs.Remove(i);
                    rs.Add(i, new Tuple<int, float, float>(p0, diff, _diff));
                }
                else
                {
                    rs.Add(i, new Tuple<int, float, float>(p0, diff, _diff));
                }

            }
            return rs;

        }



        /// <summary>
        /// 碱度调整条件
        /// </summary>
        /// <returns>
        /// item1:  true:满足    false:不满足
        /// item2:  检测值
        /// item3:批次号
        /// item4:Cao
        /// item5:SiO2
        /// </returns>
        public Tuple<bool, float, string, float, float, DateTime> R_Condition()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //M_SINTER_ANALYSIS 标志位  代表插入、更新
            //是否更新或者插入  //20200914
            //M_SINTER_ANALYSIS表插入或更新一条新记录，且C_R && C_CAO && C_SIO2字段不为0或空值 20200915 与闫龙飞确认过
            string sql = "select top(1) isnull(s.C_R,0) C_R,isnull(s.flag,0),isnull(s.BATCH_NUM,''),isnull(C_CAO,0),isnull(C_SIO2,0),timestamp from dbo.M_SINTER_ANALYSIS s where s.flag>0   order by s.TIMESTAMP desc";

            DataTable _dt = _mdb.GetCommand(sql);
            if (_dt == null)
            {
                mixlog.writelog("M_SINTER_ANALYSIS表异常", -1);
                return new Tuple<bool, float, string, float, float, DateTime>(false, 0, "", 0, 0, DateTime.Now);
            }
            var _vdt = _dt.AsEnumerable();
            foreach (var x in _vdt)//只取最近一条
            {
                int c = x.ItemArray.Count();
                int vflag = int.Parse(x[1].ToString());
                if (vflag <= 0)
                {
                    mixlog.writelog("R:flag<=0", -1);
                    return new Tuple<bool, float, string, float, float, DateTime>(false, 0, "", 0, 0, DateTime.Now);
                }
                float vCR = float.Parse(x[0].ToString()) * float.Parse(x[3].ToString()) * float.Parse(x[4].ToString());//20200915 
                if (vCR == 0)
                {
                    mixlog.writelog("R:C_R=NULL", -1);
                    return new Tuple<bool, float, string, float, float, DateTime>(false, vCR, "", 0, 0, DateTime.Now);
                }
                else
                {
                    vCR = float.Parse(x[0].ToString());
                    string batchNum = x[2].ToString();

                    if (batchNum == "" || batchNum.Length < 3)
                    {
                        mixlog.writelog("R:批次号长度不正确(" + batchNum + ")", -1);
                        return new Tuple<bool, float, string, float, float, DateTime>(false, vCR, "", 0, 0, DateTime.Now);
                    }
                    else
                    {
                        string sql1 = "select * from MC_SINCAL_R_result r where r.SINCAL_R_SAMPLE_CODE='" + batchNum + "'";
                        DataTable _cdt = _mdb.GetCommand(sql1);
                        int _rows = _cdt.Rows.Count;
                        if (_rows == 0)
                        {
                            float _CaO = float.Parse(x[3].ToString() == "" ? "0" : x[3].ToString());//20200914
                            float _SiO2 = float.Parse(x[4].ToString() == "" ? "0" : x[4].ToString());//20200914
                            DateTime _Dtime = DateTime.Parse(x[5].ToString());
                            //20200914
                            return new Tuple<bool, float, string, float, float, DateTime>(true, vCR, batchNum, _CaO, _SiO2, _Dtime);
                        }
                        else
                        {
                            return new Tuple<bool, float, string, float, float, DateTime>(false, vCR, "", 0, 0, DateTime.Now);
                        }
                    }
                    //else
                    //{
                    //    mixlog.writelog("R:批次号不正确(" + batchNum + ")", -1);
                    //    return new Tuple<bool, float, string, float, float, DateTime>(false, vCR, "", 0, 0, DateTime.Now);
                    //}
                    //20200528 修改

                }
                //break;
            }
            return new Tuple<bool, float, string, float, float, DateTime>(false, 0, "", 0, 0, DateTime.Now);

        }
        /// <summary>
        /// 初始化全局参数
        /// </summary>
        /// <returns>
        /// 倒推MIN
        /// 倒推MAX
        /// 成功与否标志
        /// </returns>
        float T_PAR_MIN_TIME = 0;
        float T_PAR_MAX_TIME = 0;
        public Tuple<float, float, bool> initParam()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);

            string sql = "select p.T_PAR_RET_TIME_MIN,p.T_PAR_RET_TIME_MAX from dbo.MC_SINCAL_C_R_PAR p";

            DataTable _dt1 = _mdb.GetCommand(sql);
            if (_dt1 == null)
            {
                mixlog.writelog("参数表 MC_SINCAL_C_R_PAR 连接异常", -1);
                return new Tuple<float, float, bool>(0, 0, false);
            }
            var vdt1 = _dt1.AsEnumerable();
            float[] vals = { 0, 0 };
            foreach (var x in vdt1)
            {
                for (int i = 0; i < x.ItemArray.Count(); i++)
                {
                    vals[i] = float.Parse(x[i].ToString() == "" ? "0" : x[i].ToString());
                }
            }
            T_PAR_MIN_TIME = vals[0];
            T_PAR_MAX_TIME = vals[1];
            return new Tuple<float, float, bool>(vals[0], vals[1], true);
        }

        /// <summary>
        /// 获取碱度调整计算需要的参数值
        /// </summary>
        /// <returns>
        /// R_W,K_OUTSIDE,K_INSIDE,ADJ_MIN,ADJ_MAX
        /// </returns>
        public Tuple<float, float, float, float, float, int, List<float>, Tuple<bool>> R_Params()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            /*
             * R_W：烧结矿碱度正常波动范围，参数值：MC_SINCAL_C_R_PAR表，R_PAR_R_W字段。
            K_OUTSIDE：检测碱度在目标碱度正常波动范围外的修正系数，参数表：MC_SINCAL_C_R_PAR表，R_PAR_ K_OUTSIDE字段；
            K_INSIDE：检测碱度在目标碱度正常波动范围内的修正系数，参数表：MC_SINCAL_C_R_PAR表，R_PAR_ K_INSIDE字段；
            ADJ_MIN：每次计算碱度调整量下限值；参数表：MC_SINCAL_C_R_PAR表，R_PAR_ADJ_MIN字段；
            ADJ_MAX：每次计算碱度调整量上限值；参数表：MC_SINCAL_C_R_PAR表，R_PAR_ADJ_MAX字段；*
            */

            //20200915 与闫龙飞确认过
            /*
             T_PAR_RET_TIME_MIN：有效倒推时间最小值；参数值：MC_MIXCAL_PAR 表，T_PAR_RET_TIME_MIN字段；
            T_PAR_RET_TIME_MAX：有效倒推时间最大值；参数值：MC_MIXCAL_PAR 表，T_PAR_RET_TIME_MAX字段；
             */
            /*
            //20200915 与闫龙飞确认
             n：烧结矿取平均批数；参数值：MC_SINCAL_C_R_PAR表，PAR_AVG_NUM
            PAR_CAO_RANGE：烧结矿CAO正常波动范围值；参数值：MC_SINCAL_C_R_PAR表，R_PAR_CAO_STD字段
            PAR_SIO2_RANGE1：烧结矿SIO2正常波动范围值；参数值：MC_SINCAL_C_R_PAR表，R_PAR_SIO2_STD_U
            PAR_SIO2_RANGE2：烧结矿SIO2正常波动范围值；参数值：MC_SINCAL_C_R_PAR表，R_PAR_SIO2_STD_D
           
            K_CAO：CaO偏低调整系数；参数值：MC_SINCAL_C_R_PAR表，R_PAR_K_CAO;
            K_SIO2：SIO2偏低调整系数；参数值：MC_SINCAL_C_R_PAR表，R_PAR_K_SIO2;
            K_SIO2_CAO：SIO2偏低调整系数；参数值：MC_SINCAL_C_R_PAR表，R_PAR_K_SIO2_CAO;
            //20201201新增 是否启用加样判断 来源参数表 PAR_S_ADD_FLAG
             */
            string sql = "select p.R_PAR_R_W,p.R_PAR_K_OUTSIDE,p.R_PAR_K_INSIDE,p.R_PAR_ADJ_MIN,p.R_PAR_ADJ_MAX,PAR_AVG_NUM,R_PAR_CAO_STD,R_PAR_SIO2_STD_U,R_PAR_SIO2_STD_D,R_PAR_K_CAO,R_PAR_K_SIO2,R_PAR_K_SIO2_CAO,PAR_S_ADD_FLAG from dbo.MC_SINCAL_C_R_PAR p";

            DataTable _dt = _mdb.GetCommand(sql);
            if (_dt == null)
            {
                mixlog.writelog("参数表MC_SINCAL_C_R_PAR连接异常", -1);
                return new Tuple<float, float, float, float, float, int, List<float>, Tuple<bool>>(0, 0, 0, 0, 0, 0, null, new Tuple<bool>(false));
            }
            var vdt = _dt.AsEnumerable();
            float[] vals = { 0, 0, 0, 0, 0, 0 };
            int n = 0;
            List<float> Lval = new List<float>();
            foreach (var x in vdt)
            {
                for (int i = 0; i < x.ItemArray.Count(); i++)
                {
                    if (i == 5)
                    {
                        n = int.Parse(x[i].ToString() == "" ? "0" : x[i].ToString());
                    }
                    else if (i < 5)
                        vals[i] = float.Parse(x[i].ToString() == "" ? "0" : x[i].ToString());
                    else
                    {
                        Lval.Add(float.Parse(x[i].ToString() == "" ? "0" : x[i].ToString()));
                    }
                }
            }



            return new Tuple<float, float, float, float, float, int, List<float>, Tuple<bool>>(vals[0], vals[1], vals[2], vals[3], vals[4], n, Lval, new Tuple<bool>(true));
        }

        /// <summary>
        /// 碱度调整采集值
        /// </summary>
        /// <returns></returns>
        public Tuple<float, float, bool> R_CollectVal()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //R_AIM：目标碱度（中线值）；采集值：存储位置：MC_SINCAL_R_result 表，SINCAL_R_AIM字段；（采集位置：MC_MIXCAL_RESULT_1MIN表，SINCAL_R_A字段）
            string sql = "select SINCAL_R_A,SINCAL_R_C from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";//20200604 
            DataTable _dt = _mdb.GetCommand(sql);
            if (_dt == null)
            {
                return new Tuple<float, float, bool>(0, 0, false);
            }
            var _vdt = _dt.AsEnumerable();
            foreach (var x in _vdt)
            {
                float midval = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
                float cval = float.Parse(x[1].ToString() == "" ? "0" : x[1].ToString());
                return new Tuple<float, float, bool>(midval, cval, true);
            }
            return new Tuple<float, float, bool>(0, 0, false);//数据表异常
        }

        /// <summary>
        /// 碱度调整计算值
        /// </summary>
        /// 状态，倒推时间内预测烧结矿平均碱度，当前配料室下料预测碱度,倒推时间
        //        public Tuple<bool, float, float, int> R_ComputeVal()
        //        {
        //            //初始化数据库
        //            DBSQL _mdb = new DBSQL(_connstring);
        //            /*
        //             R_RET：倒推时间内预测烧结矿平均碱度；计算值：存储位置：MC_SINCAL_R_result 表，SINCAL_R_RE字段；
        //（取倒推时间R_RETRODICT_TIME前PAR_T分钟内，MC_SINCAL_result_1min表，SINCAL_SIN_PV_R字段的平均值
        //    R_RETRODICT_TIME：MC_MICAL_RESULT表中最新一条，DATAMUN = 14 的记录的MICAL_SAM_MAT_TIME字段；
        //存储位置：MC_SINCAL_C_result表，SINCAL_R_RETRODICT_TIME字段
        //PAR_T：MC_SINCAL_C_R_PAR表，PAR_T字段;)
        //             */
        //            //获取PAR_T时间字段
        //            string sql_0 = "select PAR_T from MC_SINCAL_C_R_PAR";

        //            DataTable _dt = _mdb.GetCommand(sql_0);

        //            if (_dt == null)
        //            {
        //                return new Tuple<bool, float, float, int>(false, 0, 0, 0);
        //            }
        //            //读取PAR_T的值
        //            var _vdt = _dt.AsEnumerable();
        //            int ipart = -1;
        //            foreach (var x in _vdt)
        //            {
        //                ipart = int.Parse(x[0].ToString());
        //            }
        //            //获取R_RETRODICT_TIME
        //            string sql_1 = "select Top(1) MICAL_SAM_MAT_TIME from MC_MICAL_RESULT where DATAMUN = 14 order by TIMESTAMP desc";
        //            DataTable _dt1 = _mdb.GetCommand(sql_1);

        //            if (_dt1 == null)
        //            {
        //                return new Tuple<bool, float, float, int>(false, 0, 0, 0);
        //            }
        //            var _vdt1 = _dt1.AsEnumerable();
        //            int strRT = 0;
        //            foreach (var x in _vdt1)
        //            {

        //                strRT = int.Parse(x[0].ToString());
        //            }
        //            DateTime curDT1 = DateTime.Now;
        //            DateTime curDT2 = DateTime.Now;
        //            string sql_2 = "select AVG(SINCAL_SIN_PV_R) from MC_SINCAL_result_1min where MICAL_SAM_MAT_TIME>" + curDT1.AddMinutes(-1 * strRT).AddMinutes(-1 * ipart) + " and MICAL_SAM_MAT_TIME<=" + curDT2.AddMinutes(-1 * strRT);
        //            DataTable _dt2 = _mdb.GetCommand(sql_2);

        //            if (_dt2 == null)
        //            {
        //                return new Tuple<bool, float, float, int>(false, 0, 0, 0);
        //            }
        //            var _vdt2 = _dt2.AsEnumerable();
        //            float avg_PV_R = -1;
        //            foreach (var x in _vdt2)
        //            {
        //                avg_PV_R = float.Parse(x[0].ToString());
        //            }

        //            //计算当前
        //            DateTime curDT3 = DateTime.Now;
        //            DateTime curDT4 = DateTime.Now;
        //            string sql_3 = "select AVG(SINCAL_SIN_PV_R) from MC_SINCAL_result_1min where MICAL_SAM_MAT_TIME>" + curDT3.AddMinutes(-1 * ipart) + " and MICAL_SAM_MAT_TIME<=" + curDT4.AddMinutes(-1 * ipart);
        //            DataTable _dt3 = _mdb.GetCommand(sql_3);

        //            if (_dt3 == null)
        //            {
        //                return new Tuple<bool, float, float, int>(false, 0, 0, 0);
        //            }
        //            var _vdt3 = _dt3.AsEnumerable();
        //            float cur_PV_R = -1;
        //            foreach (var x in _vdt3)
        //            {
        //                cur_PV_R = float.Parse(x[0].ToString());
        //            }


        //            return new Tuple<bool, float, float, int>(true, avg_PV_R, cur_PV_R, strRT);

        //        }
        /// <summary>
        /// 碱度调整计算值
        /// </summary>
        /// 状态，倒推时间内预测烧结矿平均碱度，当前配料室下料预测碱度,倒推时间,倒推时间获取是否成功：1 yes 0 no
        public Tuple<bool, float, float, float, int> R_ComputeVal()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            /*
             R_RET：倒推时间内预测烧结矿平均碱度；计算值：存储位置：MC_SINCAL_R_result 表，SINCAL_R_RE字段；
（取倒推时间R_RETRODICT_TIME前PAR_T分钟内，MC_SINCAL_result_1min表，SINCAL_SIN_PV_R字段的平均值
    R_RETRODICT_TIME：MC_MICAL_RESULT表中最新一条，DATAMUN = 14 的记录的MICAL_SAM_MAT_TIME字段；
存储位置：MC_SINCAL_C_result表，SINCAL_R_RETRODICT_TIME字段
PAR_T：MC_SINCAL_C_R_PAR表，PAR_T字段;)（已废弃）
             */
            //20200915 修改逻辑 与闫龙飞确认过
            /*
             R_RET：倒推时间内预测烧结矿平均碱度；计算值：存储位置：MC_SINCAL_R_result 表，SINCAL_R_RE字段；

            （读取M_SINTER_ANALYSIS表该条烧结矿记录的“取样时间”（即M_SINTER_ANALYSIS表，SAMPLETIME字段），取数时间段修改为
            [SAMPLETIME - R_RETRODICT_TIME -15Min - PAR_T ,SAMPLETIME - R_RETRODICT_TIME - 15min],内MC_MIXCAL_RESULT_1MIN表，SINCAL_SIN_PV_R字段的平均值。此处进行了修改：

            R_RETRODICT_TIME：倒推时间段，单位min;计算MC_MICAL_RESULT表，SAMPLETIME时间前2小时内的DATAMUN = 14 的所有记录的MICAL_SAM_MAT_TIME字段平均值（0或空不参与平均值计算）。
            如果求得的平均倒推时间，不满足T_PAR_RET_TIME_MIN < R_RETRODICT_TIME < T_PAR_RET_TIME_MAX，则令R_RETRODICT_TIME = 0；存储位置：MC_SINCAL_R_result表，SINCAL_R_RETRODICT_TIME字段

            如果R_RETRODICT_TIME = 0，则令SINCAL_R_RE_TIME_FLAG =0；否则，则令SINCAL_R_RE_TIME_FLAG =1；
            */
            //20200915 
            //获取PAR_T时间字段
            string sql_0 = "select PAR_T from MC_SINCAL_C_R_PAR";

            DataTable _dt = _mdb.GetCommand(sql_0);

            if (_dt == null)
            {
                return new Tuple<bool, float, float, float, int>(false, 0, 0, 0, 0);
            }
            //读取PAR_T的值
            var _vdt = _dt.AsEnumerable();
            int ipart = -1;
            foreach (var x in _vdt)
            {
                ipart = int.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }
            mixlog.writelog("R:PAR_T的值=" + ipart, 0);
            //20200604 修改

            DateTime vSamleTime = DateTime.Now;

            //（取倒推时间R_RETRODICT_TIME前PAR_T分钟内，读取M_SINTER_ANALYSIS表该条烧结矿记录的“取样时间”（即M_SINTER_ANALYSIS表，SAMPLETIME字段）
            var _lrrt = LGetLastTime("M_SINTER_ANALYSIS");
            if (_lrrt == null)
            {

            }
            else
            {
                vSamleTime = DateTime.Parse(_lrrt[1].ToString() == "" ? DateTime.Now + "" : _lrrt[1].ToString());
                mixlog.writelog("烧结矿取样时间=" + vSamleTime, 0);
            }
            //R_RETRODICT_TIME：倒推时间段，单位min;计算MC_MICAL_RESULT表，
            //SAMPLETIME时间前2小时内的DATAMUN = 14 的所有记录的MICAL_SAM_MAT_TIME字段平均值。
            //如果求得的平均倒退时间,则令R_RETRODICT_TIME = 0；
            //如果R_RETRODICT_TIME = 0，则令SINCAL_R_RE_TIME_FLAG =0；如果R_RETRODICT_TIME > 0，则令SINCAL_R_RE_TIME_FLAG =1；

            string sql_1 = "select Avg(MICAL_SAM_MAT_TIME) from MC_MICAL_RESULT where TIMESTAMP>'" + vSamleTime.AddHours(-2) + "' and TIMESTAMP<'" + vSamleTime + "' and DATANUM = 14 and MICAL_SAM_MAT_TIME !=0 and MICAL_SAM_MAT_TIME is not null";

            //获取R_RETRODICT_TIME
            //（已废弃）string sql_1 = "select Top(1) MICAL_SAM_MAT_TIME from MC_MICAL_RESULT where DATANUM = 14 order by TIMESTAMP desc";
            DataTable _dt1 = _mdb.GetCommand(sql_1);

            if (_dt1 == null)
            {
                return new Tuple<bool, float, float, float, int>(false, 0, 0, 0, 0);
            }
            var _vdt1 = _dt1.AsEnumerable();
            float strRT = 0;//单位分钟
            int vSINCAL_R_RE_TIME_FLAG = 0;//倒推时间成功标志
            foreach (var x in _vdt1)
            {
                strRT = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }
            mixlog.writelog("R:计算RT=" + strRT, 0);
            //if(strRT<60)//20200915 修改 与闫龙飞确认过
            if ((strRT > T_PAR_MIN_TIME && strRT < T_PAR_MAX_TIME) == false)//20200916
            {
                strRT = 0;
                vSINCAL_R_RE_TIME_FLAG = 0;//失败
            }
            else
            {
                vSINCAL_R_RE_TIME_FLAG = 1;//成功
            }
            mixlog.writelog("R:经判断后RT=" + strRT, 0);
            DateTime curDT1 = vSamleTime;
            DateTime curDT2 = vSamleTime;
            int _OffsetTime = 15;//20200916 增加 与闫龙飞确认过
            //20200604 修改逻辑

            string sql_2 = "select AVG(SINCAL_SIN_PV_R) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP>'" + curDT1.AddMinutes(-1 * strRT).AddMinutes(-1 * ipart).AddMinutes(-1 * _OffsetTime) + "' and TIMESTAMP<='" + curDT2.AddMinutes(-1 * strRT).AddMinutes(-1 * _OffsetTime) + "' and SINCAL_SIN_PV_R!=0 and SINCAL_SIN_PV_R is not null";
            DataTable _dt2 = _mdb.GetCommand(sql_2);

            if (_dt2 == null)
            {
                return new Tuple<bool, float, float, float, int>(false, 0, 0, 0, 0);
            }
            var _vdt2 = _dt2.AsEnumerable();
            float avg_PV_R = -1;
            foreach (var x in _vdt2)
            {
                avg_PV_R = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }
            mixlog.writelog("R:碱度平均值=" + avg_PV_R, 0);
            //计算当前
            DateTime curDT3 = DateTime.Now;
            DateTime curDT4 = DateTime.Now;
            //20200528 修改 逻辑     20200727 修改  与闫龙飞 20200925
            string sql_3 = "select AVG(SINCAL_SIN_PV_R) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP>'" + curDT3.AddMinutes(-1 * ipart * 0.2) + "' and TIMESTAMP<='" + curDT4 + "' and SINCAL_SIN_PV_R!=0 and SINCAL_SIN_PV_R is not null";//20200916 与闫龙飞确认过
            //string sql_3 = "select SINCAL_SIN_PV_R from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";


            DataTable _dt3 = _mdb.GetCommand(sql_3);

            if (_dt3 == null)
            {
                return new Tuple<bool, float, float, float, int>(false, 0, 0, 0, 0);
            }
            var _vdt3 = _dt3.AsEnumerable();
            float cur_PV_R = -1;
            foreach (var x in _vdt3)
            {
                cur_PV_R = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }
            mixlog.writelog("R:碱度当前值=" + cur_PV_R, 0);

            return new Tuple<bool, float, float, float, int>(true, avg_PV_R, cur_PV_R, strRT, vSINCAL_R_RE_TIME_FLAG);

        }



        /// <summary>
        /// 使用于判断最近两条记录中某字段是否发生变化
        /// </summary>
        /// <param name="vmdb">数据库对象</param>
        /// <param name="segMent">字段名</param>
        /// <param name="tableName">表名</param>
        /// <param name="typeNum">字段数据类型,1:int,2:float</param>
        /// <returns></returns>
        private bool Top2Turn(DBSQL vmdb, string segMent, string tableName, int typeNum, float vEqual = 0.00001f)
        {
            string sql_1 = "select Top(2) " + segMent + " from " + tableName + " m order by m.TIMESTAMP desc";
            DataTable _dt = vmdb.GetCommand(sql_1);
            if (_dt == null)
            {
                Console.WriteLine("语句{0}执行失败", sql_1);
                return false;
            }
            var _vdt = _dt.AsEnumerable();
            int _count = _vdt.Count();
            if (_count < 2)
            {
                return false;
            }
            else
            {
                if (typeNum == 2)
                {
                    float[] _temp = { 0, 0 };

                    for (int i = 0; i < _count; i++)
                    {
                        _temp[i] = float.Parse(_vdt.ElementAt(i)[0].ToString());
                    }

                    if (Math.Abs(_temp[0] - _temp[1]) < vEqual)//相等无变化
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (typeNum == 1)
                {
                    int[] _temp = { 0, 0 };

                    for (int i = 0; i < _count; i++)
                    {
                        _temp[i] = int.Parse(_vdt.ElementAt(i)[0].ToString());
                    }

                    if (_temp[0] - _temp[1] == 0)//相等无变化
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
                    return false;
                }

            }
        }

        /// <summary>
        /// 使用于判断周期T中某字段平均值是否发生变化
        /// </summary>
        /// <param name="vmdb">数据库对象</param>
        /// <param name="segMent">字段名</param>
        /// <param name="tableName">表名</param>
        /// <param name="T">周期</param>
        /// <returns>0:无变化，1：增加，-1减少；-88：异常</returns>
        private int Avg2Turn(DBSQL vmdb, string segMent, string tableName, float T, float vEqual = 0.00001f)
        {
            DateTime curDT1 = DateTime.Now;
            DateTime curDT2 = DateTime.Now;
            string sql_1 = "select AVG(" + segMent + ") from " + tableName + " where TIMESTAMP>'" + curDT1.AddMinutes(-1 * T) + "' and TIMESTAMP<='" + curDT2 + "'";
            string sql_2 = "select AVG(" + segMent + ") from " + tableName + " where TIMESTAMP>'" + curDT1.AddMinutes(-2 * T) + "' and TIMESTAMP<='" + curDT2.AddMinutes(-1 * T) + "'";
            DataTable _dt1 = vmdb.GetCommand(sql_1);
            DataTable _dt2 = vmdb.GetCommand(sql_2);
            var _vdt1 = _dt1.AsEnumerable();
            var _vdt2 = _dt2.AsEnumerable();
            float[] _temp = { 0, 0 };
            if (_vdt1.Count() <= 0 || _vdt2.Count() <= 0)
            {
                return -88;
            }
            _temp[0] = float.Parse(_vdt1.ElementAt(0)[0].ToString() == "" ? "0" : _vdt1.ElementAt(0)[0].ToString());
            _temp[1] = float.Parse(_vdt2.ElementAt(0)[0].ToString() == "" ? "0" : _vdt2.ElementAt(0)[0].ToString());
            if (Math.Abs(_temp[0] - _temp[1]) < vEqual)
            {
                return 0;
            }
            else if (_temp[0] - _temp[1] > vEqual)
            {
                return 1;
            }
            else if (_temp[0] - _temp[1] < -1 * vEqual)
            {
                return -1;
            }

            return -88;
        }
        private bool isInsertedNew(DBSQL vmdb)
        {
            // string sql = "select count(*) from MC_SINCAL_C_result where SINCAL_C_FLAG=3";
            string sql = "select SINCAL_C_FLAG from MC_SINCAL_C_result order by timestamp desc";
            DataTable _dt = vmdb.GetCommand(sql);
            if (_dt == null) return false;
            var _vdt = _dt.AsEnumerable();
            if (_vdt.Count() <= 0)
            {
                return false;
            }
            else
            {
                int _vflag = int.Parse(_vdt.ElementAt(0)[0].ToString() == "" ? "0" : _vdt.ElementAt(0)[0].ToString());
                if (_vflag == 3)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 烧结矿FeO变化调整
        /// </summary>
        /// <param name="vmdb">数据库对象</param>
        /// <returns>
        /// item1:计算结果是否有效，true：有效
        /// item2:调整值
        /// 
        /// </returns>
        private Tuple<bool, float> FeO_Cpt(DBSQL vmdb, string vbatchnum)
        {
            //数据准备
            float vFeO_TEST = 0;
            float vC_RET = 0;
            float vC_CUR = 0;
            //20200604 新增修改
            string vFeObtn = "";
            bool isCpt = false;
            //20200604
            //string sql = "select isnull(s.C_FEO,0) C_FEO from dbo.M_SINTER_ANALYSIS s where s.flag>0 order by s.TIMESTAMP desc";
            //string sql = "select isnull(s.C_FEO,0) C_FEO,s.flag,isnull(s.BATCH_NUM,'') from dbo.M_SINTER_ANALYSIS s where s.flag>0 order by s.TIMESTAMP desc";
            string sql = "select isnull(s.C_FEO,0) C_FEO,isnull(s.flag,0),isnull(s.BATCH_NUM,'') from dbo.M_SINTER_ANALYSIS s where s.flag>0  order by s.TIMESTAMP desc";//"select isnull(s.C_FEO,0) C_FEO,isnull(s.flag,0),isnull(s.BATCH_NUM,'') from dbo.M_SINTER_ANALYSIS s where s.flag>0 order by s.TIMESTAMP desc";

            DataTable _dt = vmdb.GetCommand(sql);
            if (_dt == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt = _dt.AsEnumerable();
            if (_vdt.Count() > 0)
            {
                vFeO_TEST = float.Parse(_vdt.ElementAt(0)[0].ToString() == "" ? "0" : _vdt.ElementAt(0)[0].ToString());
                if (vFeO_TEST == 0)
                {
                    return new Tuple<bool, float>(false, 0);
                }

                //float vCR = 0;// float.Parse(_vdt.ElementAt(0)[2].ToString() == "" ? "0" : _vdt.ElementAt(0)[2].ToString());

                vFeObtn = _vdt.ElementAt(0)[2].ToString();


            }
            else
            {
                return new Tuple<bool, float>(false, 0);
            }
            //获取PAR_T时间字段
            string sql_1 = "select PAR_T from MC_SINCAL_C_R_PAR";

            DataTable _dtt = vmdb.GetCommand(sql_1);

            if (_dtt == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            //读取PAR_T的值
            var _vdtt = _dtt.AsEnumerable();
            int ipart = -1;
            foreach (var x in _vdtt)
            {
                ipart = int.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }


            DateTime vSampleTime = DateTime.Now;
            var _vlr = LGetLastTime("M_SINTER_ANALYSIS");
            if (_vlr == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                vSampleTime = DateTime.Parse(_vlr[1].ToString() == "" ? vSampleTime + "" : _vlr[1].ToString());
            }
            //获取R_RETRODICT_TIME
            //(已废弃)string sql_2 = "select Top(1) MICAL_SAM_MAT_TIME from MC_MICAL_RESULT where DATAMUN = 14 order by TIMESTAMP desc";
            string sql_2 = "select AVG(MICAL_SAM_MAT_TIME) from MC_MICAL_RESULT where TIMESTAMP>'" + vSampleTime.AddHours(-2) + "' and TIMESTAMP<'" + vSampleTime + "' and DATANUM = 14 and MICAL_SAM_MAT_TIME!=0 and MICAL_SAM_MAT_TIME is not null";
            DataTable _dt2 = vmdb.GetCommand(sql_2);

            if (_dt2 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt2 = _dt2.AsEnumerable();
            float strRT = 0;
            int SINCAL_C_RE_TIME_FLAG = 0;
            foreach (var x in _vdt2)
            {

                strRT = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }
            //if(strRT<60)
            if ((strRT > T_PAR_MIN_TIME && strRT < T_PAR_MAX_TIME) == false)//20200916 修改
            {
                strRT = 0;
                SINCAL_C_RE_TIME_FLAG = 0;
            }
            else
            {
                SINCAL_C_RE_TIME_FLAG = 1;
            }
            DateTime curDT1 = vSampleTime;
            DateTime curDT2 = vSampleTime;
            int _FeOTime = 15;//20200916 修改
            string sql_3 = "select AVG(SINCAL_MIX_PV_C) from MC_MIXCAL_RESULT_1MIN  where TIMESTAMP > '" + curDT1.AddMinutes(-1 * strRT).AddMinutes(-1 * ipart).AddMinutes(-1 * _FeOTime) + "' and TIMESTAMP<='" + curDT2.AddMinutes(-1 * strRT).AddMinutes(-1 * _FeOTime) + "' and SINCAL_MIX_PV_C!=0 and SINCAL_MIX_PV_C is not null";
            DataTable _dt3 = vmdb.GetCommand(sql_3);

            if (_dt3 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt3 = _dt3.AsEnumerable();
            //float avg_PV_C = -1;
            foreach (var x in _vdt3)
            {
                vC_RET = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }

            //计算当前
            DateTime curDT3 = DateTime.Now;
            DateTime curDT4 = DateTime.Now;
            string sql_4 = "select AVG(SINCAL_MIX_PV_C) from MC_MIXCAL_RESULT_1MIN " + "where TIMESTAMP > '" + curDT3.AddMinutes(-1 * strRT * 0.2) + "' and TIMESTAMP<'" + curDT4 + "' and SINCAL_MIX_PV_C!=0 and SINCAL_MIX_PV_C is not null";
            DataTable _dt4 = vmdb.GetCommand(sql_4);

            if (_dt4 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt4 = _dt4.AsEnumerable();
            //float cur_PV_C = -1;
            foreach (var x in _vdt4)
            {
                vC_CUR = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
                break;
            }
            ////20200604 新增修改
            //string sql_55 = "select p.SINCAL_C_A,p.SINCAL_C_DC from MC_MIXCAL_RESULT_1MIN p order by timestamp desc";
            //float C_a = 0;
            //float C_dc = 0;
            //DataTable _dt55 = vmdb.GetCommand(sql_55);

            //if (_dt55 == null)
            //{
            //    return new Tuple<bool, float>(false, 0);
            //}
            //else
            //{
            //    var _vdt55 = _dt55.AsEnumerable();
            //    foreach(var x in _vdt55)
            //    {
            //        C_a = float.Parse(x[0].ToString()==""?"0": x[0].ToString());
            //        C_dc = float.Parse(x[1].ToString() == "" ? "0" : x[1].ToString());
            //        break;
            //    }

            //}
            string FeO_btnum = "";
            FeO_btnum = vbatchnum;//20200727

            //参数值
            float vFeO_AIM = 0;//MC_MIXCAL_PAR 表，PAR_AIM_FEO字段；
            float vFeO_W = 0;//参数值：MC_SINCAL_C_R_PAR表，C_PAR_FeO_W字段；
            float vFeO_SIN_ADJ_MIN = 0;//参数表：MC_SINCAL_C_R_PAR表，C_PAR_FeO_SIN_ADJ_MIN字段；
            float vFeO_SIN_ADJ_MAX = 0;//参数表：MC_SINCAL_C_R_PAR表，C_PAR_FeO_SIN_ADJ_MAX字段；
            float vK_OUTSIDE = 0;
            float vK_INSIDE = 0;
            float vC_DEV = 0;// C_DEV: 烧结矿化验FeO偏差对应含碳偏差
            string sql_5 = "select PAR_AIM_FEO from MC_MIXCAL_PAR";
            string sql_6 = "select C_PAR_FeO_W,C_PAR_FeO_SIN_ADJ_MIN,C_PAR_FeO_SIN_ADJ_MAX,C_PAR_K_OUTSIDE,C_PAR_K_INSIDE from MC_SINCAL_C_R_PAR";

            DataTable _dt5 = vmdb.GetCommand(sql_5);
            if (_dt5 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt5 = _dt5.AsEnumerable();
            if (_vdt5.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            vFeO_AIM = float.Parse(_vdt5.ElementAt(0)[0].ToString() == "" ? "0" : _vdt5.ElementAt(0)[0].ToString());

            DataTable _dt6 = vmdb.GetCommand(sql_6);
            if (_dt6 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt6 = _dt6.AsEnumerable();
            if (_vdt6.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            vFeO_W = float.Parse(_vdt6.ElementAt(0)[0].ToString() == "" ? "0" : _vdt6.ElementAt(0)[0].ToString());
            vFeO_SIN_ADJ_MIN = float.Parse(_vdt6.ElementAt(0)[1].ToString() == "" ? "0" : _vdt6.ElementAt(0)[1].ToString());
            vFeO_SIN_ADJ_MAX = float.Parse(_vdt6.ElementAt(0)[2].ToString() == "" ? "0" : _vdt6.ElementAt(0)[2].ToString());
            vK_OUTSIDE = float.Parse(_vdt6.ElementAt(0)[3].ToString() == "" ? "0" : _vdt6.ElementAt(0)[3].ToString());
            vK_INSIDE = float.Parse(_vdt6.ElementAt(0)[4].ToString() == "" ? "0" : _vdt6.ElementAt(0)[4].ToString());
            //20200916
            //20201201新增
            int ADD_State = R_ADDState() == true ? 1 : 0;//加样是否激活
            var _vC_FeO = SamplePlus(vbatchnum, 1);
            int SINCAL_SPE_ADD_FLAG = 2;
            if (ADD_State == 1 && _vC_FeO.Item1 == 1)//加样判断激活且加样
            {
                SINCAL_SPE_ADD_FLAG = 1;
                mixlog.writelog("碳调整 FeO加样,本次不调整", 0);
                //20200916 新增逻辑
                int SINcal_SPE_ADD_NOTES = 0;
                mixlog.writelog("该批次(" + vbatchnum + ")加样，碳调整不执行", -1);
                //加样逻辑
                float curFeOVal = _vC_FeO.Item2;
                float lastFeOVal = _vC_FeO.Item3;
                if (vFeO_AIM - vFeO_W <= curFeOVal && curFeOVal <= vFeO_AIM + vFeO_W)
                {
                    SINcal_SPE_ADD_NOTES = 5;
                    InsertLogTable("配料模型", "碳调整", "加样FeO正常，上次调整量=" + lastFeOVal + ",请人工关注上批次碳度调整值变化");
                }
                else
                {
                    if (curFeOVal > vFeO_AIM + vFeO_W && lastFeOVal > vFeO_AIM + vFeO_W)//都偏高
                    {
                        SINcal_SPE_ADD_NOTES = 1;
                        InsertLogTable("配料模型", "碳调整", "加样FeO偏高=" + curFeOVal + "，上次检测FeO偏高=" + lastFeOVal + ",本次碳不调整");
                    }
                    else if (curFeOVal < vFeO_AIM - vFeO_W && lastFeOVal < vFeO_AIM - vFeO_W)//都偏低
                    {
                        SINcal_SPE_ADD_NOTES = 2;
                        InsertLogTable("配料模型", "碳调整", "加样FeO偏低=" + curFeOVal + "，上次检测FeO偏低=" + lastFeOVal + ",本次碳不调整");
                    }
                    else if (curFeOVal > vFeO_AIM + vFeO_W && lastFeOVal < vFeO_AIM - vFeO_W)//加样碳度偏高，上次偏低
                    {
                        SINcal_SPE_ADD_NOTES = 3;
                        InsertLogTable("配料模型", "碳调整", "加样FeO偏高=" + curFeOVal + "，上次检测FeO偏低=" + lastFeOVal + ",请岗位人工确认");
                    }
                    else if (curFeOVal < vFeO_AIM - vFeO_W && lastFeOVal > vFeO_AIM + vFeO_W)//加样碳度偏低，上次偏高
                    {
                        SINcal_SPE_ADD_NOTES = 4;
                        InsertLogTable("配料模型", "碳调整", "加样FeO偏低=" + curFeOVal + "，上次检测FeO偏高=" + lastFeOVal + ",请岗位人工确认");
                    }


                }
                //插入数据库

                bool _xiflag = isInsertedNew(vmdb);
                string xsql_update = "update MC_SINCAL_C_result set SINCAL_C_FeO_TEST=" + vFeO_TEST + ",SINCAL_SPE_ADD_NOTES=" + SINcal_SPE_ADD_NOTES + ",SINCAL_C_SAMPLE_CODE='" + FeO_btnum + "'" + ",SINCAL_FEO_AIM=" + vFeO_AIM + " where SINCAL_C_FLAG=3";
                string xsql_ins = "insert into MC_SINCAL_C_result(timestamp,SINCAL_C_FeO_TEST,SINCAL_C_FLAG,SINCAL_C_SAMPLE_CODE,SINCAL_SPE_ADD_NOTES,SINCAL_FEO_AIM,SINCAL_SPE_ADD_FLAG) values('" + DateTime.Now + "'," + vFeO_TEST + "," + 3 + ",'" + vFeObtn + "'," + SINcal_SPE_ADD_NOTES + "," + vFeO_AIM + "," + SINCAL_SPE_ADD_FLAG + ")";
                string xsql_insert = _xiflag == true ? xsql_update : xsql_ins;

                int _xrs = vmdb.CommandExecuteNonQuery(xsql_insert);
                if (_xrs > 0)
                {
                    //Console.WriteLine("插入数据成功");//需要插入数据库
                    mixlog.writelog("烧结矿FeO 插入数据成功(加样)(" + xsql_insert + ")", 0);

                }
                else
                {
                    //Console.WriteLine("插入数据失败");//需要插入数据库
                    mixlog.writelog("烧结矿FeO 插入数据失败(加样)(" + xsql_insert + ")", -1);

                }
                return new Tuple<bool, float>(false, 0);
                //20200916
            }

            //20200916
            //不加样
            //20200916
            //计算值
            float vFeO_TEST_DEV = 0;// FeO_TEST_DEV：烧结矿化验FeO偏差

            //start
            if (vFeO_TEST > vFeO_AIM - vFeO_W && vFeO_TEST < vFeO_AIM + vFeO_W)
            {
                vFeO_TEST_DEV = vFeO_AIM - vFeO_TEST;
                vC_DEV = vK_INSIDE * vFeO_TEST_DEV;
            }
            else
            {
                //vFeO_TEST_DEV = vFeO_TEST - vFeO_AIM;
                vFeO_TEST_DEV = vFeO_AIM - vFeO_TEST;
                vC_DEV = vK_OUTSIDE * vFeO_TEST_DEV;
            }
            float vC_FeO_ADJ = 0;//C_FeO_ADJ：计算配碳调整量
            float vC_FeO_RE_AD = 0;// 经过有效性判断下发的配碳调整量
                                   //20200603
            if (vC_RET <= 0 || vC_CUR <= 0)
            {
                vC_FeO_ADJ = vC_DEV;

            }
            else
            {
                vC_FeO_ADJ = vC_RET - vC_CUR + vC_DEV;
            }
            float vvC_FeO_ADJ = Math.Abs(vC_FeO_ADJ);
            if (vvC_FeO_ADJ <= vFeO_SIN_ADJ_MIN)
            {
                vC_FeO_RE_AD = 0;
            }
            else
            {
                if (vC_FeO_ADJ >= vFeO_SIN_ADJ_MAX)
                {
                    vC_FeO_RE_AD = vFeO_SIN_ADJ_MAX;
                }
                else if (vC_FeO_ADJ <= -1 * vFeO_SIN_ADJ_MAX)
                {
                    vC_FeO_RE_AD = -1 * vFeO_SIN_ADJ_MAX;
                }
                else if (vvC_FeO_ADJ > vFeO_SIN_ADJ_MIN && vvC_FeO_ADJ < vFeO_SIN_ADJ_MAX)
                {
                    vC_FeO_RE_AD = vC_FeO_ADJ;
                }
            }
            //if (vC_FeO_ADJ > vFeO_SIN_ADJ_MAX)
            //{
            //    vC_FeO_RE_AD = vFeO_SIN_ADJ_MAX;
            //}
            //else if (vC_FeO_ADJ < vFeO_SIN_ADJ_MIN)
            //{
            //    vC_FeO_RE_AD = vFeO_SIN_ADJ_MIN;
            //}
            //else
            //{
            //    vC_FeO_RE_AD = vC_FeO_ADJ;
            //}
            //20200604 计算修改

            //float SINCAL_C_BEFORE_Modify = 0;
            //float SINCAL_C_AFTER_Modify = 0;
            //float SINCAL_C_SV_R_BE = 0;
            //float SINCAL_C_SV_R = 0;
            //SINCAL_C_BEFORE_Modify = C_dc;
            //SINCAL_C_AFTER_Modify = vC_FeO_RE_AD + SINCAL_C_BEFORE_Modify;
            //SINCAL_C_SV_R_BE= C_a + SINCAL_C_BEFORE_Modify;
            //SINCAL_C_SV_R = C_a + SINCAL_C_AFTER_Modify;
            //20200604 修改
            //存储数据库
            bool _iflag = isInsertedNew(vmdb);
            string sql_update = "update MC_SINCAL_C_result set SINCAL_C_FeO_TEST=" + vFeO_TEST + "," + "SINCAL_C_RETRODICT_TIME=" + strRT + ",SINCAL_FeO_TEST_DEV=" + vFeO_TEST_DEV + ",SINCAL_C_DEV=" + vC_DEV + ",SINCAL_C_CUR=" + vC_CUR + ",SINCAL_C_FeO_ADJ=" + vC_FeO_ADJ + ",SINCAL_C_FeO_RE_ADJ=" + vC_FeO_RE_AD + ",SINCAL_C_SAMPLE_CODE='" + FeO_btnum + "', SINCAL_C_RE_TIME_FLAG=" + SINCAL_C_RE_TIME_FLAG + ",SINCAL_FEO_AIM=" + vFeO_AIM + ",SINCAL_C_RET=" + vC_RET + " where SINCAL_C_FLAG=3";
            string sql_ins = "insert into MC_SINCAL_C_result(timestamp,SINCAL_C_FeO_TEST,SINCAL_C_RETRODICT_TIME,SINCAL_FeO_TEST_DEV,SINCAL_C_DEV,SINCAL_C_CUR,SINCAL_C_FeO_ADJ,SINCAL_C_FeO_RE_ADJ,SINCAL_C_FLAG,SINCAL_C_SAMPLE_CODE,SINCAL_C_RE_TIME_FLAG,SINCAL_FEO_AIM,SINCAL_C_RET,SINCAL_SPE_ADD_FLAG) values('" + DateTime.Now + "'," + vFeO_TEST + "," + strRT + "," + vFeO_TEST_DEV + "," + vC_DEV + "," + vC_CUR + "," + vC_FeO_ADJ + "," + vC_FeO_RE_AD + "," + 3 + ",'" + vFeObtn + "'," + SINCAL_C_RE_TIME_FLAG + "," + vFeO_AIM + "," + vC_RET + "," + SINCAL_SPE_ADD_FLAG + ")";
            string sql_insert = _iflag == true ? sql_update : sql_ins;

            int _rs = vmdb.CommandExecuteNonQuery(sql_insert);
            if (_rs > 0)
            {
                //Console.WriteLine("插入数据成功");//需要插入数据库
                mixlog.writelog("烧结矿FeO 插入数据成功", 0);
                return new Tuple<bool, float>(true, vC_FeO_RE_AD);
            }
            else
            {
                //Console.WriteLine("插入数据失败");//需要插入数据库
                mixlog.writelog("烧结矿FeO 插入数据失败(" + sql_insert + ")", 0);
                return new Tuple<bool, float>(false, 0);
            }

            //}
            //else
            //{
            //    return new Tuple<bool, float>(false, 0);
            //}

        }

        /// <summary>
        /// 烧返配比变化调整
        /// </summary>
        /// <param name="vmdb">数据库对象</param>
        /// <returns>
        /// item1:计算结果是否有效，true：有效
        /// item2:调整值
        /// </returns>
        private Tuple<bool, float> HotRet_Cpt(DBSQL vmdb)
        {
            //数据准备
            //采集值
            float vBRUN_DRY_NEW = 0;//BRUN_DRY_NEW：变化后烧返配比
            float vBRUN_DRY_OLD = 0;// BRUN_DRY_OLD：变化前烧返配比
            //参数值
            float vBILL_SIN_RM_TH = 0;// BILL_SIN_RM_TH：烧返配比变化调整含碳门槛值
            float vPAR_BILL_SIN_RM_ADJ = 0;//PAR_BILL_SIN_RM_ADJ ：烧返配比变化1%，配碳调整量
            float vBILL_SIN_ADJ_MIN = 0;//BILL_SIN_ADJ_MIN：每次计算配碳调整量下限值
            float vBILL_SIN_ADJ_MAX = 0;//BILL_SIN_ADJ_MAX：每次计算配碳调整量上限值
            string sql_3 = "select C_PAR_BILL_SIN_RM_TH,C_PAR_BILL_SIN_RM_ADJ,C_PAR_BILL_SIN_ADJ_MIN,C_PAR_BILL_SIN_ADJ_MAX from MC_SINCAL_C_R_PAR";
            DataTable _dt3 = vmdb.GetCommand(sql_3);
            if (_dt3 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt3 = _dt3.AsEnumerable();
            if (_vdt3.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                vBILL_SIN_RM_TH = float.Parse(_vdt3.ElementAt(0)[0].ToString() == "" ? "0" : _vdt3.ElementAt(0)[0].ToString());
                vPAR_BILL_SIN_RM_ADJ = float.Parse(_vdt3.ElementAt(0)[1].ToString() == "" ? "0" : _vdt3.ElementAt(0)[1].ToString());
                vBILL_SIN_ADJ_MIN = float.Parse(_vdt3.ElementAt(0)[2].ToString() == "" ? "0" : _vdt3.ElementAt(0)[2].ToString());
                vBILL_SIN_ADJ_MAX = float.Parse(_vdt3.ElementAt(0)[3].ToString() == "" ? "0" : _vdt3.ElementAt(0)[3].ToString());
            }

            //计算值
            float vWHOLE_BILL_NEW = 0;//WHOLE_BILL_NEW：变化后总设定配比
            float vWHOLE_BILL_OLD = 0;// WHOLE_BILL_OLD：变化前总设定配比
            float vBILL_SIN_RM_CHANGE = 0;//BILL_SIN_RM_CHANGE：烧返配比变化百分比
            float vC_BILL_SIN_RM_ADJ = 0;//C_BILL_SIN_RM_ADJ：烧结返矿配比变化配碳量调整量（未经过上下限限制）
            float vC_BILL_SIN_RM_RE_ADJ = 0;// C_BILL_SIN_RM_RE_ADJ：烧结返矿配比变化配碳量调整量（经过上下限限制）
            string sql_4 = "select Top(2) m.SINCAL_BLEND_ORE_BILL_DRY,m.SINCAL_BFES_ORE_BILL_DRY,m.SINCAL_FLUX_STONE_BILL_DRY,m.SINCAL_DOLOMATE_BILL_DRY,m.SINCAL_FLUX_BILL_DRY,m.SINCAL_FUEL_BILL_DRY,m.SINCAL_BRUN_DRY,m.SINCAL_ASH_DUST_BILL_DRY,m.SINCAL_BRUN_DRY  from MC_MIXCAL_RESULT_1MIN  m order by m.TIMESTAMP desc";
            DataTable _dt4 = vmdb.GetCommand(sql_4);
            if (_dt4 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt4 = _dt4.AsEnumerable();
            if (_vdt4.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                for (int i = 0; i < _vdt4.Count(); i++)
                {
                    int vcol = _vdt4.ElementAt(i).ItemArray.Count();
                    if (i == 0)
                    {
                        vBRUN_DRY_NEW = float.Parse(_vdt4.ElementAt(i)[vcol - 1].ToString() == "" ? "0" : _vdt4.ElementAt(i)[vcol - 1].ToString());
                        for (int j = 0; j < vcol - 1; j++)
                        {
                            vWHOLE_BILL_NEW += float.Parse(_vdt4.ElementAt(i)[j].ToString() == "" ? "0" : _vdt4.ElementAt(i)[j].ToString());
                        }
                    }
                    else
                    {
                        vBRUN_DRY_OLD = float.Parse(_vdt4.ElementAt(i)[vcol - 1].ToString());
                        for (int j = 0; j < vcol - 1; j++)
                        {
                            vWHOLE_BILL_OLD += float.Parse(_vdt4.ElementAt(i)[j].ToString() == "" ? "0" : _vdt4.ElementAt(i)[j].ToString());
                        }
                    }
                }
                //
                vBILL_SIN_RM_CHANGE = (vBRUN_DRY_NEW / vWHOLE_BILL_NEW - vBRUN_DRY_OLD / vWHOLE_BILL_OLD) * 100;

                if (Math.Abs(vBILL_SIN_RM_CHANGE) >= vBILL_SIN_RM_TH)
                {
                    vC_BILL_SIN_RM_ADJ = vBILL_SIN_RM_CHANGE * vPAR_BILL_SIN_RM_ADJ;
                    if (vC_BILL_SIN_RM_ADJ > vBILL_SIN_ADJ_MAX)
                    {
                        vC_BILL_SIN_RM_RE_ADJ = vBILL_SIN_ADJ_MAX;
                    }
                    else if (vC_BILL_SIN_RM_ADJ < vBILL_SIN_ADJ_MIN)
                    {
                        vC_BILL_SIN_RM_RE_ADJ = vBILL_SIN_ADJ_MIN;
                    }
                    else if (vBILL_SIN_ADJ_MIN <= vC_BILL_SIN_RM_ADJ && vC_BILL_SIN_RM_ADJ <= vBILL_SIN_ADJ_MAX)
                    {
                        vC_BILL_SIN_RM_RE_ADJ = vC_BILL_SIN_RM_ADJ;
                    }
                }
                else
                {
                    vC_BILL_SIN_RM_ADJ = 0;
                    vC_BILL_SIN_RM_RE_ADJ = 0;
                }
                //存储数据库
                bool _iflag = isInsertedNew(vmdb);
                string sql_update = "update MC_SINCAL_C_result set SINCAL_C_BILL_RM_NEW=" + vBRUN_DRY_NEW + "," + "SINCAL_C_BILL_RM_OLD=" + vBRUN_DRY_OLD + ",SINCAL_C_RM_CHANGE=" + vBILL_SIN_RM_CHANGE + ",SINCAL_C_BILL_SIN_RM_ADJ=" + vC_BILL_SIN_RM_ADJ + ",SINCAL_C_BILL_SIN_RM_RE_ADJ=" + vC_BILL_SIN_RM_RE_ADJ + " where SINCAL_C_FLAG=3";
                string sql_ins = "insert into MC_SINCAL_C_result(timestamp,SINCAL_C_BILL_RM_NEW,SINCAL_C_BILL_RM_OLD,SINCAL_C_RM_CHANGE,SINCAL_C_BILL_SIN_RM_ADJ,SINCAL_C_BILL_SIN_RM_RE_ADJ,SINCAL_C_FLAG) values('" + DateTime.Now + "'," + vBRUN_DRY_NEW + "," + vBRUN_DRY_OLD + "," + vBILL_SIN_RM_CHANGE + "," + vC_BILL_SIN_RM_ADJ + "," + vC_BILL_SIN_RM_RE_ADJ + "," + 3 + ")";
                string sql_insert = _iflag == true ? sql_update : sql_ins;
                int _rs = vmdb.CommandExecuteNonQuery(sql_insert);
                if (_rs > 0)
                {
                    //Console.WriteLine("插入数据成功");//需要插入数据库
                    mixlog.writelog("烧返 插入数据成功", 0);
                    return new Tuple<bool, float>(true, vC_BILL_SIN_RM_RE_ADJ);
                }
                else
                {
                    // Console.WriteLine("插入数据失败");//需要插入数据库
                    mixlog.writelog("烧返 插入数据失败", -1);
                    return new Tuple<bool, float>(false, 0);
                }
            }
        }
        /// <summary>
        /// 高返配比变化调整
        /// </summary>
        /// <param name="vmdb">数据库对象</param>
        /// <returns>
        /// item1:计算结果是否有效，true：有效
        /// item2:调整值
        /// </returns>
        private Tuple<bool, float> HighRet_Cpt(DBSQL vmdb)
        {
            //数据准备
            //采集值
            float vBFES_ORE_BILL_DRY_NEW = 0;//BFES_ORE_BILL_DRY_NEW：变化后高返配比
            float vBFES_ORE_BILL_DRY_OLD = 0;// BFES_ORE_BILL_DRY_OLD：变化前高返配比
            //参数值
            float vBILL_BFES_ORE_TH = 0;// BILL_BFES_ORE_TH：高返配比变化调整含碳门槛值
            float vPAR_BILL_BFES_ORE_ADJ = 0;//PAR_BILL_BFES_ORE_ADJ ：高返配比变化1%，配碳调整量
            float vBILL_BFES_ADJ_MIN = 0;//BILL_BFES_ADJ_MIN：每次计算配碳调整量下限值
            float vBILL_BFES_ADJ_MAX = 0;//BILL_BFES_ADJ_MAX：每次计算配碳调整量上限值

            string sql_3 = "select C_PAR_BILL_BFES_ORE_TH,C_PAR_BILL_BFES_ORE_ADJ,C_PAR_BILL_BFES_ADJ_MIN,C_PAR_BILL_BFES_ADJ_MAX from MC_SINCAL_C_R_PAR";
            DataTable _dt3 = vmdb.GetCommand(sql_3);
            if (_dt3 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt3 = _dt3.AsEnumerable();
            if (_vdt3.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                vBILL_BFES_ORE_TH = float.Parse(_vdt3.ElementAt(0)[0].ToString() == "" ? "0" : _vdt3.ElementAt(0)[0].ToString());
                vPAR_BILL_BFES_ORE_ADJ = float.Parse(_vdt3.ElementAt(0)[1].ToString() == "" ? "0" : _vdt3.ElementAt(0)[1].ToString());
                vBILL_BFES_ADJ_MIN = float.Parse(_vdt3.ElementAt(0)[2].ToString() == "" ? "0" : _vdt3.ElementAt(0)[2].ToString());
                vBILL_BFES_ADJ_MAX = float.Parse(_vdt3.ElementAt(0)[3].ToString() == "" ? "0" : _vdt3.ElementAt(0)[3].ToString());
            }

            //计算值
            float vWHOLE_BILL_NEW = 0;//WHOLE_BILL_NEW：变化后总设定配比
            float vWHOLE_BILL_OLD = 0;// WHOLE_BILL_OLD：变化前总设定配比
            float vBILL_BFES_ORE_CHANGE = 0;//BILL_BFES_ORE_CHANGE：高返配比变化百分比
            float vC_BILL_BFES_ORE_ADJ = 0;//C_BILL_BFES_ORE_ADJ：高返配比变化配碳量调整量（未经过上下限限制）
            float vC_BILL_BFES_ORE_RE_ADJ = 0;// C_BILL_BFES_ORE_RE_ADJ：高返配比变化配碳量调整量（经过上下限限制）
            string sql_4 = "select Top(2) m.SINCAL_BLEND_ORE_BILL_DRY,m.SINCAL_BFES_ORE_BILL_DRY,m.SINCAL_FLUX_STONE_BILL_DRY,m.SINCAL_DOLOMATE_BILL_DRY,m.SINCAL_FLUX_BILL_DRY,m.SINCAL_FUEL_BILL_DRY,m.SINCAL_BRUN_DRY,m.SINCAL_ASH_DUST_BILL_DRY,m.SINCAL_BFES_ORE_BILL_DRY  from MC_MIXCAL_RESULT_1MIN  m order by m.TIMESTAMP desc";
            DataTable _dt4 = vmdb.GetCommand(sql_4);
            if (_dt4 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt4 = _dt4.AsEnumerable();
            if (_vdt4.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                for (int i = 0; i < _vdt4.Count(); i++)
                {
                    int vcol = _vdt4.ElementAt(i).ItemArray.Count();
                    if (i == 0)
                    {
                        vBFES_ORE_BILL_DRY_NEW = float.Parse(_vdt4.ElementAt(i)[vcol - 1].ToString());
                        for (int j = 0; j < vcol - 1; j++)
                        {
                            vWHOLE_BILL_NEW += float.Parse(_vdt4.ElementAt(i)[j].ToString() == "" ? "0" : _vdt4.ElementAt(i)[j].ToString());
                        }
                    }
                    else
                    {
                        vBFES_ORE_BILL_DRY_OLD = float.Parse(_vdt4.ElementAt(i)[vcol - 1].ToString() == "" ? "0" : _vdt4.ElementAt(i)[vcol - 1].ToString());
                        for (int j = 0; j < vcol - 1; j++)
                        {
                            vWHOLE_BILL_OLD += float.Parse(_vdt4.ElementAt(i)[j].ToString() == "" ? "0" : _vdt4.ElementAt(i)[j].ToString());
                        }
                    }
                }
                //
                vBILL_BFES_ORE_CHANGE = (vBFES_ORE_BILL_DRY_NEW / vWHOLE_BILL_NEW - vBFES_ORE_BILL_DRY_OLD / vWHOLE_BILL_OLD) * 100;

                if (Math.Abs(vBILL_BFES_ORE_CHANGE) >= vBILL_BFES_ORE_TH)
                {
                    vC_BILL_BFES_ORE_ADJ = vBILL_BFES_ORE_CHANGE * vPAR_BILL_BFES_ORE_ADJ;
                    if (vC_BILL_BFES_ORE_ADJ > vBILL_BFES_ADJ_MAX)
                    {
                        vC_BILL_BFES_ORE_RE_ADJ = vBILL_BFES_ADJ_MAX;
                    }
                    else if (vC_BILL_BFES_ORE_ADJ < vBILL_BFES_ADJ_MIN)
                    {
                        vC_BILL_BFES_ORE_RE_ADJ = vBILL_BFES_ADJ_MIN;
                    }
                    else if (vBILL_BFES_ADJ_MIN <= vC_BILL_BFES_ORE_ADJ && vC_BILL_BFES_ORE_ADJ <= vBILL_BFES_ADJ_MAX)
                    {
                        vC_BILL_BFES_ORE_RE_ADJ = vC_BILL_BFES_ORE_ADJ;
                    }
                }
                else
                {
                    vC_BILL_BFES_ORE_ADJ = 0;
                    vC_BILL_BFES_ORE_RE_ADJ = 0;
                }
                //存储数据库

                string sql_ins = "insert into MC_SINCAL_C_result(timestamp,SINCAL_C_BILL_BFES_ORE_NEW,SINCAL_C_BILL_BFES_ORE_OLD,SINCAL_C_BFES_ORE_CHANGE,SINCAL_C_BILL_BFES_ORE_ADJ,SINCAL_C_BILL_BFES_ORE_RE_ADJ,SINCAL_C_FLAG) values('" + DateTime.Now + "'," + vBFES_ORE_BILL_DRY_NEW + "," + vBFES_ORE_BILL_DRY_OLD + "," + vBILL_BFES_ORE_CHANGE + "," + vC_BILL_BFES_ORE_ADJ + "," + vC_BILL_BFES_ORE_RE_ADJ + "," + 3 + ")";
                bool _iflag = isInsertedNew(vmdb);
                string sql_update = "update MC_SINCAL_C_result set SINCAL_C_BILL_BFES_ORE_NEW=" + vBFES_ORE_BILL_DRY_NEW + "," + "SINCAL_C_BILL_BFES_ORE_OLD=" + vBFES_ORE_BILL_DRY_OLD + ",SINCAL_C_BFES_ORE_CHANGE=" + vBILL_BFES_ORE_CHANGE + ",SINCAL_C_BILL_BFES_ORE_ADJ=" + vC_BILL_BFES_ORE_ADJ + ",SINCAL_C_BILL_BFES_ORE_RE_ADJ=" + vC_BILL_BFES_ORE_RE_ADJ + " where SINCAL_C_FLAG=3";
                string sql_insert = _iflag == true ? sql_update : sql_ins;
                int _rs = vmdb.CommandExecuteNonQuery(sql_insert);
                if (_rs > 0)
                {
                    //Console.WriteLine("插入数据成功");//需要插入数据库
                    mixlog.writelog("高返 插入数据成功", 0);
                    return new Tuple<bool, float>(true, vC_BILL_BFES_ORE_RE_ADJ);
                }
                else
                {
                    //Console.WriteLine("插入数据失败");//需要插入数据库
                    mixlog.writelog("高返 插入数据失败(" + sql_insert + ")", -1);
                    return new Tuple<bool, float>(false, 0);
                }
            }
        }
        /// <summary>
        /// 混合料综合烧损变化调整
        /// </summary>
        /// <param name="vmdb">数据库对象</param>
        /// <returns>
        /// item1:计算结果是否有效，true：有效
        /// item2:调整值
        /// </returns>
        private Tuple<bool, float> MixMatRet_CPt(DBSQL vmdb)
        {
            //采集值
            float vMIX_SP_LOT_NEW = 0;//MIX_SP_LOT_NEW：变化后混合料综合烧损
            float vMIX_SP_LOT_OLD = 0;//MIX_SP_LOT_OLD：变化前混合料综合烧损
            string sql_1 = "select Top(2) SINCAL_MIX_SP_LOT from MC_MIXCAL_RESULT_1MIN m order by m.TIMESTAMP desc";
            DataTable _dt1 = vmdb.GetCommand(sql_1);
            if (_dt1 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt1 = _dt1.AsEnumerable();
            vMIX_SP_LOT_NEW = float.Parse(_vdt1.ElementAt(0)[0].ToString() == "" ? "0" : _vdt1.ElementAt(0)[0].ToString());
            vMIX_SP_LOT_OLD = float.Parse(_vdt1.ElementAt(1)[0].ToString() == "" ? "0" : _vdt1.ElementAt(1)[0].ToString());
            //参数值
            float vPAR_LOT_TH = 0;//PAR_LOT_TH：综合烧损变化，调整配碳门槛值
            float vPAR_LOT_ADJ = 0;// PAR_LOT_ADJ：综合烧损变化1 %，配碳调整量
            float vLOT_ADJ_MIN = 0;//LOT_ADJ_MIN：每次计算配碳调整量下限值
            float vLOT_ADJ_MAX = 0;//LOT_ADJ_MAX：每次计算配碳调整量上限值
            string sql_2 = "select C_PAR_LOT_TH,C_PAR_LOT_ADJ,C_PAR_LOT_ADJ_MIN,C_PAR_LOT_ADJ_MAX from MC_SINCAL_C_R_PAR m";
            DataTable _dt2 = vmdb.GetCommand(sql_2);
            if (_dt2 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt2 = _dt2.AsEnumerable();
            if (_vdt2.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                vPAR_LOT_TH = float.Parse(_vdt2.ElementAt(0)[0].ToString() == "" ? "0" : _vdt2.ElementAt(0)[0].ToString());
                vPAR_LOT_ADJ = float.Parse(_vdt2.ElementAt(0)[1].ToString() == "" ? "0" : _vdt2.ElementAt(0)[1].ToString());
                vLOT_ADJ_MIN = float.Parse(_vdt2.ElementAt(0)[2].ToString() == "" ? "0" : _vdt2.ElementAt(0)[2].ToString());
                vLOT_ADJ_MAX = float.Parse(_vdt2.ElementAt(0)[3].ToString() == "" ? "0" : _vdt2.ElementAt(0)[3].ToString());
            }

            //计算值
            float vC_LOT_ADJ = 0;//C_LOT_ADJ：综合烧损变化，配碳量调整量（未经过上下限限制）
            float vC_LOT_RE_ADJ = 0;// C_LOT_RE_ADJ：综合烧损变化，配碳量调整量（经过上下限限制）；

            if (Math.Abs(vMIX_SP_LOT_NEW - vMIX_SP_LOT_OLD) >= vPAR_LOT_TH)
            {
                vC_LOT_ADJ = (vMIX_SP_LOT_NEW - vMIX_SP_LOT_OLD) * vPAR_LOT_ADJ;
                if (vC_LOT_ADJ > vLOT_ADJ_MAX)
                {
                    vC_LOT_RE_ADJ = vLOT_ADJ_MAX;
                }
                else if (vC_LOT_ADJ < vLOT_ADJ_MIN)
                {
                    vC_LOT_RE_ADJ = vLOT_ADJ_MIN;
                }
                else if (vLOT_ADJ_MIN <= vC_LOT_ADJ && vC_LOT_ADJ <= vLOT_ADJ_MAX)
                {
                    vC_LOT_RE_ADJ = vC_LOT_ADJ;
                }
            }
            else
            {
                vC_LOT_ADJ = 0;
                vC_LOT_RE_ADJ = 0;
            }
            //存储数据库
            string sql_ins = "insert into MC_SINCAL_C_result(timestamp,SINCAL_C_MIX_SP_LOT_NEW,SINCAL_C_MIX_SP_LOT_OLD,SINCAL_C_LOT_ADJ,SINCAL_C_LOT_RE_ADJ,SINCAL_C_FLAG) values('" + DateTime.Now + "'," + vMIX_SP_LOT_NEW + "," + vMIX_SP_LOT_OLD + "," + vC_LOT_ADJ + "," + vC_LOT_RE_ADJ + "," + 3 + ")";
            bool _iflag = isInsertedNew(vmdb);
            string sql_update = "update MC_SINCAL_C_result set SINCAL_C_MIX_SP_LOT_NEW=" + vMIX_SP_LOT_NEW + "," + "SINCAL_C_MIX_SP_LOT_OLD=" + vMIX_SP_LOT_OLD + ",SINCAL_C_LOT_ADJ=" + vC_LOT_ADJ + ",SINCAL_C_LOT_RE_ADJ=" + vC_LOT_RE_ADJ + " where SINCAL_C_FLAG=3";
            string sql_insert = _iflag == true ? sql_update : sql_ins;
            int _rs = vmdb.CommandExecuteNonQuery(sql_insert);
            if (_rs > 0)
            {
                //Console.WriteLine("插入数据成功");//需要插入数据库
                mixlog.writelog("MixMatRet 插入数据成功", 0);
                return new Tuple<bool, float>(true, vC_LOT_RE_ADJ);
            }
            else
            {
                //Console.WriteLine("插入数据失败");//需要插入数据库
                mixlog.writelog("MixMatRet 插入数据失败(" + sql_insert + ")", 0);
                return new Tuple<bool, float>(false, 0);
            }
        }

        /// <summary>
        /// 非燃料含碳变化调整
        /// </summary>
        /// <param name="vmdb">数据库对象</param>
        /// <returns>
        /// item1:计算结果是否有效，true：有效
        /// item2:调整值
        /// </returns>
        private Tuple<bool, float> NoFuelC_Cpt(DBSQL vmdb)
        {

            //参数值
            float vPAR_NONFUEL_TH = 0;//PAR_NONFUEL_TH：非燃料含碳变化，调整配碳门槛值
            float vPAR_NONFUEL_ADJ = 0;// PAR_NONFUEL_ADJ：非燃料含碳变化1%，配碳调整量
            float vNONFUEL_ADJ_MIN = 0;// NONFUEL_ADJ_MIN：每次计算配碳调整量下限值；
            float vNONFUEL_ADJ_MAX = 0;//NONFUEL_ADJ_MAX：每次计算配碳调整量上限值

            string sql_2 = "select C_PAR_NONFUEL_TH,C_PAR_NONFUEL_ADJ,C_PAR_NONFUEL_ADJ_MIN,C_PAR_NONFUEL_ADJ_MAX from MC_SINCAL_C_R_PAR";
            DataTable _dt2 = vmdb.GetCommand(sql_2);
            if (_dt2 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt2 = _dt2.AsEnumerable();
            if (_vdt2.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                vPAR_NONFUEL_TH = float.Parse(_vdt2.ElementAt(0)[0].ToString() == "" ? "0" : _vdt2.ElementAt(0)[0].ToString());
                vPAR_NONFUEL_ADJ = float.Parse(_vdt2.ElementAt(0)[1].ToString() == "" ? "0" : _vdt2.ElementAt(0)[1].ToString());
                vNONFUEL_ADJ_MIN = float.Parse(_vdt2.ElementAt(0)[2].ToString() == "" ? "0" : _vdt2.ElementAt(0)[2].ToString());
                vNONFUEL_ADJ_MAX = float.Parse(_vdt2.ElementAt(0)[3].ToString() == "" ? "0" : _vdt2.ElementAt(0)[3].ToString());
            }
            //计算值
            float vNON_FUEL_SP_C_NEW = 0;//NON_FUEL_SP_C_NEW：变化后非燃料含碳
            float vNON_FUEL_SP_C_OLD = 0;//NON_FUEL_SP_C_OLD：变化前非燃料含碳
            float vC_NONFUEL_ADJ = 0;//C_NONFUEL_ADJ：非燃料含碳变化，配碳量调整量（未经过上下限限制）
            float vC_NONFUEL_RE_ADJ = 0;//C_NONFUEL_RE_ADJ：非燃料含碳变化，配碳量调整量（经过上下限限制）
            string sql_3 = "select Top(2) SINCAL_NON_FUEL_SP_C from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";
            DataTable _dt3 = vmdb.GetCommand(sql_3);
            if (_dt3 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt3 = _dt3.AsEnumerable();
            if (_vdt3.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                vNON_FUEL_SP_C_NEW = float.Parse(_vdt3.ElementAt(0)[0].ToString() == "" ? "0" : _vdt3.ElementAt(0)[0].ToString());
                vNON_FUEL_SP_C_OLD = float.Parse(_vdt3.ElementAt(1)[0].ToString() == "" ? "0" : _vdt3.ElementAt(1)[0].ToString());
            }

            //start
            if ((vNON_FUEL_SP_C_NEW - vNON_FUEL_SP_C_OLD) >= vPAR_NONFUEL_TH)
            {
                vC_NONFUEL_ADJ = -(vNON_FUEL_SP_C_NEW - vNON_FUEL_SP_C_OLD) * vPAR_NONFUEL_ADJ;
            }
            else if ((vNON_FUEL_SP_C_NEW - vNON_FUEL_SP_C_OLD) <= -1 * vPAR_NONFUEL_TH)
            {
                vC_NONFUEL_ADJ = (vNON_FUEL_SP_C_NEW - vNON_FUEL_SP_C_OLD) * vPAR_NONFUEL_ADJ;
            }
            else
            {
                vC_NONFUEL_ADJ = 0;
            }

            if (vC_NONFUEL_ADJ > vNONFUEL_ADJ_MAX)
            {
                vC_NONFUEL_RE_ADJ = vNONFUEL_ADJ_MAX;
            }
            if (vC_NONFUEL_ADJ < vNONFUEL_ADJ_MIN)
            {
                vC_NONFUEL_RE_ADJ = vNONFUEL_ADJ_MIN;
            }
            if (vNONFUEL_ADJ_MIN <= vC_NONFUEL_ADJ && vC_NONFUEL_ADJ <= vNONFUEL_ADJ_MAX)
            {
                vC_NONFUEL_RE_ADJ = vC_NONFUEL_ADJ;
            }
            //插入数据库

            string sql_ins = "insert into MC_SINCAL_C_result(timestamp,SINCAL_C_NON_FUEL_SP_C_NEW,SINCAL_C_NON_FUEL_SP_C_OLD,SINCAL_C_NONFUEL_ADJ,SINCAL_C_NONFUEL_RE_ADJ,SINCAL_C_FLAG) values('" + DateTime.Now + "'," + vNON_FUEL_SP_C_NEW + "," + vNON_FUEL_SP_C_OLD + "," + vC_NONFUEL_ADJ + "," + vC_NONFUEL_RE_ADJ + "," + 3 + ")";
            bool _iflag = isInsertedNew(vmdb);
            string sql_update = "update MC_SINCAL_C_result set SINCAL_C_NON_FUEL_SP_C_NEW=" + vNON_FUEL_SP_C_NEW + "," + "SINCAL_C_NON_FUEL_SP_C_OLD=" + vNON_FUEL_SP_C_OLD + ",SINCAL_C_NONFUEL_ADJ=" + vC_NONFUEL_ADJ + ",SINCAL_C_NONFUEL_RE_ADJ=" + vC_NONFUEL_RE_ADJ + " where SINCAL_C_FLAG=3";
            string sql_insert = _iflag == true ? sql_update : sql_ins;
            int _rs = vmdb.CommandExecuteNonQuery(sql_insert);
            if (_rs > 0)
            {
                //Console.WriteLine("插入数据成功");//需要插入数据库
                mixlog.writelog("非燃料 插入数据成功", 0);
                return new Tuple<bool, float>(true, vC_NONFUEL_RE_ADJ);
            }
            else
            {
                //Console.WriteLine("插入数据失败");//需要插入数据库
                mixlog.writelog("非燃料 插入数据失败(" + sql_insert + ")", -1);
                return new Tuple<bool, float>(false, 0);
            }

        }

        /// <summary>
        /// 原料FeO变化调整
        /// </summary>
        /// <param name="vmdb">数据库对象</param>
        /// <returns>
        /// item1:计算结果是否有效，true：有效
        /// item2:调整值
        private Tuple<bool, float> yFeoTurn_Cpt(DBSQL vmdb)
        {
            //采集值
            float vMIX_SP_FeO_NEW = 0;//MIX_SP_FeO_NEW：变化后混合料FeO含量
            float vMIX_SP_FeO_OLD = 0;//MIX_SP_FeO_OLD：变化前混合料FeO含量

            string sql_1 = "select Top(2) SINCAL_MIX_SP_FeO from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";
            DataTable _dt1 = vmdb.GetCommand(sql_1);
            if (_dt1 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt1 = _dt1.AsEnumerable();
            if (_vdt1.Count() <= 1)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                vMIX_SP_FeO_NEW = float.Parse(_vdt1.ElementAt(0)[0].ToString() == "" ? "0" : _vdt1.ElementAt(0)[0].ToString());
                vMIX_SP_FeO_OLD = float.Parse(_vdt1.ElementAt(1)[0].ToString() == "" ? "0" : _vdt1.ElementAt(1)[0].ToString());
            }
            //参数值
            float vFeO_MA_TH = 0;//FeO_MA_TH：原料FeO变化，调整含碳门槛值
            float vPAR_FeO_MA_ADJ = 0;//PAR_FeO_MA_ADJ：原料FeO变化1%，配碳调整量；
            float vFeO_MA_ADJ_MIN = 0;//FeO_MA_ADJ_MIN：每次计算配碳调整量下限值；
            float vFeO_MA_ADJ_MAX = 0;//FeO_MA_ADJ_MAX：每次计算配碳调整量上限值

            string sql_2 = "select C_PAR_FeO_MA_TH,C_PAR_FeO_MA_ADJ,C_PAR_FeO_MA_ADJ_MIN,C_PAR_FeO_MA_ADJ_MAX from MC_SINCAL_C_R_PAR";
            DataTable _dt2 = vmdb.GetCommand(sql_2);
            if (_dt2 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt2 = _dt2.AsEnumerable();
            if (_vdt2.Count() < 1)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                vFeO_MA_TH = float.Parse(_vdt2.ElementAt(0)[0].ToString() == "" ? "0" : _vdt2.ElementAt(0)[0].ToString());
                vPAR_FeO_MA_ADJ = float.Parse(_vdt2.ElementAt(0)[1].ToString() == "" ? "0" : _vdt2.ElementAt(0)[1].ToString());
                vFeO_MA_ADJ_MIN = float.Parse(_vdt2.ElementAt(0)[2].ToString() == "" ? "0" : _vdt2.ElementAt(0)[2].ToString());
                vFeO_MA_ADJ_MAX = float.Parse(_vdt2.ElementAt(0)[3].ToString() == "" ? "0" : _vdt2.ElementAt(0)[3].ToString());
            }
            //计算值
            float vC_FeO_MA_ADJ = 0;//C_FeO_MA_ADJ：原料FeO变化，配碳量调整量（未经过上下限限制）
            float vC_FeO_MA_RE_ADJ = 0;//C_FeO_MA_RE_ADJ：原料FeO变化，配碳量调整量（经过上下限限制）

            //start
            if ((vMIX_SP_FeO_NEW - vMIX_SP_FeO_OLD) >= vFeO_MA_TH)
            {
                vC_FeO_MA_ADJ = -(vMIX_SP_FeO_NEW - vMIX_SP_FeO_OLD) * vPAR_FeO_MA_ADJ;
            }
            else if ((vMIX_SP_FeO_NEW - vMIX_SP_FeO_OLD) <= -1 * vFeO_MA_TH)
            {
                vC_FeO_MA_ADJ = (vMIX_SP_FeO_NEW - vMIX_SP_FeO_OLD) * vPAR_FeO_MA_ADJ;
            }
            else
            {
                vC_FeO_MA_ADJ = 0;
            }

            if (vC_FeO_MA_ADJ > vFeO_MA_ADJ_MAX)
            {
                vC_FeO_MA_RE_ADJ = vFeO_MA_ADJ_MAX;
            }
            if (vC_FeO_MA_ADJ < vFeO_MA_ADJ_MIN)
            {
                vC_FeO_MA_RE_ADJ = vFeO_MA_ADJ_MIN;
            }
            if (vFeO_MA_ADJ_MIN <= vC_FeO_MA_ADJ && vC_FeO_MA_ADJ <= vFeO_MA_ADJ_MAX)
            {
                vC_FeO_MA_RE_ADJ = vC_FeO_MA_ADJ;
            }
            //插入到数据库
            string sql_ins = "insert into MC_SINCAL_C_result(timestamp,SINCAL_C_MIX_SP_FeO_NEW,SINCAL_C_MIX_SP_FeO_OLD,SINCAL_C_FeO_MA_ADJ,SINCAL_C_FeO_MA_RE_ADJ,SINCAL_C_FLAG) values('" + DateTime.Now + "'," + vMIX_SP_FeO_NEW + "," + vMIX_SP_FeO_OLD + "," + vC_FeO_MA_ADJ + "," + vC_FeO_MA_RE_ADJ + "," + 3 + ")";
            bool _iflag = isInsertedNew(vmdb);
            string sql_update = "update MC_SINCAL_C_result set SINCAL_C_MIX_SP_FeO_NEW=" + vMIX_SP_FeO_NEW + "," + "SINCAL_C_MIX_SP_FeO_OLD=" + vMIX_SP_FeO_OLD + ",SINCAL_C_FeO_MA_ADJ=" + vC_FeO_MA_ADJ + ",SINCAL_C_FeO_MA_RE_ADJ=" + vC_FeO_MA_RE_ADJ + " where SINCAL_C_FLAG=3";
            string sql_insert = _iflag == true ? sql_update : sql_ins;
            int _rs = vmdb.CommandExecuteNonQuery(sql_insert);
            if (_rs > 0)
            {
                //Console.WriteLine("插入数据成功");//需要插入数据库
                mixlog.writelog("原料FeO 插入数据成功", 0);
                return new Tuple<bool, float>(true, vC_FeO_MA_RE_ADJ);
            }
            else
            {
                //Console.WriteLine("插入数据失败");//需要插入数据库
                mixlog.writelog("原料FeO 插入数据失败(" + sql_insert + ")", -1);
                return new Tuple<bool, float>(false, 0);
            }
        }
        /// <summary>
        /// 主机参数变化调整
        /// </summary>
        /// <param name="vmdb">数据库对象</param>
        /// <param name="mode">1：增加配碳量逻辑；2：减少配碳量逻辑</param>
        /// <returns></returns>
        private Tuple<bool, float> MainArg_Cpt(DBSQL vmdb, int mode)
        {
            //参数值
            float vSB_FLUE_TE_ADJ = 0;//SB_FLUE_TE_ADJ：主抽烟道温度持续偏低或偏高，配碳调整量

            string sql_1 = "select C_PAR_SB_FLUE_TE_ADJ from MC_SINCAL_C_R_PAR";
            DataTable _dt1 = vmdb.GetCommand(sql_1);
            if (_dt1 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            var _vdt1 = _dt1.AsEnumerable();
            if (_vdt1.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                vSB_FLUE_TE_ADJ = float.Parse(_vdt1.ElementAt(0)[0].ToString() == "" ? "0" : _vdt1.ElementAt(0)[0].ToString());
            }
            //计算值
            float vHOST_ADJ = 0;//HOST_ADJ：主机参数变化，配碳量调整量
            if (mode == 7)
                vHOST_ADJ = vSB_FLUE_TE_ADJ;
            else if (mode == 8)
                vHOST_ADJ = -1 * vSB_FLUE_TE_ADJ;
            else
                return new Tuple<bool, float>(false, 0);

            //插入数据库
            bool _iflag = isInsertedNew(vmdb);
            string sql_ins = "insert into MC_SINCAL_C_result(timestamp,SINCAL_C_HOST_ADJ,SINCAL_C_FLAG) values('" + DateTime.Now + "'," + vHOST_ADJ + "," + 3 + ")";
            string sql_update = "update MC_SINCAL_C_result set SINCAL_C_HOST_ADJ=" + vHOST_ADJ + " where SINCAL_C_FLAG=3";
            string sql_insert = _iflag == true ? sql_update : sql_ins;
            int _rs = vmdb.CommandExecuteNonQuery(sql_insert);
            if (_rs > 0)
            {
                //Console.WriteLine("插入数据成功");//需要插入数据库
                mixlog.writelog("主机参数变化 插入数据成功", 0);
                return new Tuple<bool, float>(true, vHOST_ADJ);
            }
            else
            {
                //Console.WriteLine("插入数据失败");//需要插入数据库
                mixlog.writelog("主机参数变化 插入数据失败", -1);
                return new Tuple<bool, float>(false, 0);
            }

        }

        /// <summary>
        /// 主机参数变化条件判断
        /// </summary>
        /// <param name="vmdb"></param>
        /// <param name="style">1：没有增大，2：没有减小</param>
        /// <returns>int: 0:无变化，7：增加配碳量逻辑，8：减小配碳量逻辑</returns>
        private int MainArg_Conds(DBSQL vmdb, int style)
        {
            int MainArsTurn = 0;
            float vPAR_MA_TE3 = 0;
            float vPAR_MA_TE2 = 0;
            //（1）主抽温度参数
            bool mainTempArg = false;
            string sql = "select PAR_MA_TE3,PAR_MA_TE2 from MC_BTPCAL_PAR";
            DataTable _dt = vmdb.GetCommand(sql);
            if (_dt == null)
            {
                mainTempArg = false;
                Console.WriteLine("MC_BTPCAL_PAR参数表异常");
                return MainArsTurn;
            }
            var _vdt = _dt.AsEnumerable();
            if (_vdt.Count() <= 0)
            {
                mainTempArg = false;
            }
            else
            {
                vPAR_MA_TE3 = float.Parse(_vdt.ElementAt(0)[0].ToString() == "" ? "0" : _vdt.ElementAt(0)[0].ToString());
                vPAR_MA_TE2 = float.Parse(_vdt.ElementAt(0)[1].ToString() == "" ? "0" : _vdt.ElementAt(0)[1].ToString());
            }
            string sql_0 = "select C_PAR_C_HOST_T from MC_SINCAL_C_R_PAR";
            DataTable _dt0 = vmdb.GetCommand(sql_0);
            float vC_PAR_C_HOST_T = 0;
            var _vdt0 = _dt0.AsEnumerable();
            if (_vdt0.Count() <= 0)
            {
                mainTempArg = false;
            }
            else
            {
                vC_PAR_C_HOST_T = float.Parse(_vdt0.ElementAt(0)[0].ToString() == "" ? "0" : _vdt0.ElementAt(0)[0].ToString());
                DateTime dt0 = DateTime.Now;
                DateTime dt1 = DateTime.Now;
                //C_SIN_PLC_1MIN 表，(SIN_PLC_MA_SB_1_FLUE_TE + SIN_PLC_MA_SB_2_FLUE_TE)/2） 
                string comp = style == 1 ? " < " : " > ";
                float vPAR_MA_TE = style == 1 ? vPAR_MA_TE3 : vPAR_MA_TE2;
                string sql_1 = "select * from " +
"(select COUNT(*) as wc from dbo.C_SIN_PLC_1MIN where TIMESTAMP > '" + dt0.AddMinutes(-1 * vC_PAR_C_HOST_T) + "' and TIMESTAMP < '" + dt1 + "' and((SIN_PLC_MA_SB_1_FLUE_TE + SIN_PLC_MA_SB_2_FLUE_TE) / 2) " + comp + " " + vPAR_MA_TE + ") w" +
",(select COUNT(*) as qc from dbo.C_SIN_PLC_1MIN where TIMESTAMP > '" + dt0.AddMinutes(-1 * vC_PAR_C_HOST_T) + "' and TIMESTAMP < '" + dt1 + "') q where  w.wc = q.qc";
                DataTable _dt1 = vmdb.GetCommand(sql_1);
                var _vdt1 = _dt1.AsEnumerable();
                if (_vdt1.Count() <= 0)
                {
                    mainTempArg = false;
                }
                else
                {
                    mainTempArg = true;//20200512
                    //20200604
                    //vPAR_MA_TE3 = float.Parse(_vdt0.ElementAt(0)[0].ToString()==""?"0": _vdt0.ElementAt(0)[0].ToString());//20200512
                }
            }
            if (mainTempArg == true)
            {
                //（2）最近一批烧结矿报出FeO(M_SINTER_ANALYSIS表，最新一条记录的C_FEO字段)值小于FeO中线值（MC_MIXCAL_PAR 表，PAR_AIM_FEO字段）
                bool hotFeoflag = false;
                string comp = style == 1 ? " < " : " > ";
                string sql_2 = "select * from (select Top(1) C_FEO from M_SINTER_ANALYSIS order by timestamp desc) w,(select PAR_AIM_FEO from MC_MIXCAL_PAR) p where w.C_FEO" + comp + "p.PAR_AIM_FEO";
                DataTable _dt2 = vmdb.GetCommand(sql_2);
                if (_dt2 == null)
                {
                    return 0;
                }
                var _vdt2 = _dt2.AsEnumerable();
                if (_vdt2.Count() <= 0)
                {
                    hotFeoflag = false;
                }
                else
                {
                    hotFeoflag = true;
                }
                if (hotFeoflag == true)
                {
                    //（3）在一段时间（MC_BTPCAL_PAR表 ，C_PAR_C_HOST_T）内，没有进行增加配碳量（即MC_SINCAL_result_1min表，SINCAL_C_DC字段平均值，与上一周期时间段内平均值相比没有增加）
                    //修改内容：MC_MIXCAL_RESULT_1MIN    上表名已废弃
                    bool CAddflag = false;
                    int _rs = Avg2Turn(vmdb, "SINCAL_C_DC", "MC_MIXCAL_RESULT_1MIN", vC_PAR_C_HOST_T);
                    if (style == 1)
                    {
                        if (_rs == 1)
                        {
                            CAddflag = false;
                        }
                        else if (_rs != -88)
                        {
                            CAddflag = true;
                        }
                        else
                        {
                            CAddflag = false;
                        }
                    }
                    else if (style == 2)
                    {
                        if (_rs == -1)
                        {
                            CAddflag = false;
                        }
                        else if (_rs != -88)
                        {
                            CAddflag = true;
                        }
                        else
                        {
                            CAddflag = false;
                        }
                    }
                    if (CAddflag == true)
                    {
                        //（4）在一段时间（MC_BTPCAL_PAR表 ，C_PAR_C_HOST_T）内，没有增加主抽风机频率（即一段时间内T_MA_PGD_PLC_1MIN 表，(PS_MA_FAN_1_SP_FRE + PS_MA_FAN_2_SP_FRE)/2的平均值，与上一周期时间段内平均值相比没有增加）
                        //20200529 修改表 C_SIN_PLC_1MIN ，上述T_MA_PGD_PLC_1MIN表 已废弃;(SIN_PLC_MA_FAN_1_SP + SIN_PLC_MA_FAN_2_SP)/2的平均值
                        bool WindAddflag = false;
                        _rs = Avg2Turn(vmdb, "(SIN_PLC_MA_FAN_1_SP + SIN_PLC_MA_FAN_2_SP)/2", "C_SIN_PLC_1MIN", vC_PAR_C_HOST_T);
                        if (style == 1)
                        {
                            if (_rs == 1)
                            {
                                WindAddflag = false;
                            }
                            else if (_rs != -88)
                            {
                                WindAddflag = true;
                                MainArsTurn = 7;
                            }
                            else
                            {
                                WindAddflag = false;
                            }
                        }
                        else if (style == 2)
                        {
                            if (_rs == -1)
                            {
                                WindAddflag = false;
                            }
                            else if (_rs != -88)
                            {
                                WindAddflag = true;
                                MainArsTurn = 8;
                            }
                            else
                            {
                                WindAddflag = false;
                            }
                        }
                    }
                    else
                    {
                        MainArsTurn = 0;
                    }
                }
                else
                {
                    MainArsTurn = 0;
                }
            }
            else
            {
                MainArsTurn = 0;
            }

            return MainArsTurn;
        }
        /// <summary>
        /// C 调整判断条件
        /// </summary>
        /// <returns>
        /// -88:异常
        /// 0：正常，但不满足触发条件
        /// 1：烧结矿FeO变化
        /// 2：烧返配比变化
        /// 3：高返配比变化
        /// 4：混合料综合烧损变化
        /// 5：非燃料含碳变化
        /// 6：原料FeO变化
        /// 7,8：主机参数变化    7:没有增大   8：没有减小
        /// item1:满足条件的个数
        /// item2:满足条件的序列，例如：0，2，3 或者 1，4，6 等
        /// item3:满足烧结矿FeO变化  批次号
        /// </returns>
        public Tuple<int, List<int>, string> C_Conditions()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //返回值

            int _Count = 0;
            List<int> _conds = new List<int>();
            string _batchnum = "";//20200727
            mixlog.writelog("C_Conditions==Start", 0);
            //烧结矿FeO变化
            {
                int FeO_Turn = -1;
                //M_SINTER_ANALYSIS 标志位  代表插入、更新
                //是否更新或者插入
                string sql = "select isnull(s.C_FEO,0) C_FEO,isnull(s.flag,0),isnull(s.BATCH_NUM,'') from dbo.M_SINTER_ANALYSIS s where s.flag>0  order by s.TIMESTAMP desc";//"select isnull(s.C_FEO,0) C_FEO,isnull(s.flag,0),isnull(s.BATCH_NUM,'') from dbo.M_SINTER_ANALYSIS s where s.flag>0 order by s.TIMESTAMP desc";

                DataTable _dt = _mdb.GetCommand(sql);
                if (_dt == null)
                {
                    FeO_Turn = 0;
                }
                else
                {

                    var _vdt = _dt.AsEnumerable();
                    foreach (var x in _vdt)//只取最近一条
                    {
                        int c = x.ItemArray.Count();
                        int vflag = int.Parse(x[1].ToString());
                        if (vflag <= 0)
                        {
                            FeO_Turn = 0;
                        }
                        float vCFEO = float.Parse(x[0].ToString());
                        if (vCFEO == 0)
                        {
                            FeO_Turn = 0;
                        }
                        else
                        {
                            string batchNum = x[2].ToString();
                            _batchnum = batchNum;//20200727
                            //20200528 修改
                            if (batchNum.Length < 3)
                            {
                                mixlog.writelog("批次号长度不正确" + batchNum, -1);
                                FeO_Turn = 0;
                            }
                            else
                            {
                                string sql1 = "select * from MC_SINCAL_C_result r where r.SINCAL_C_SAMPLE_CODE='" + batchNum + "'";
                                DataTable _cdt = _mdb.GetCommand(sql1);
                                int _rows = _cdt.Rows.Count;
                                if (_rows == 0)
                                {
                                    FeO_Turn = 1;
                                    _Count++;
                                    mixlog.writelog("烧结矿FeO变化满足", 0);
                                }
                                else
                                {
                                    FeO_Turn = 0;
                                }
                            }

                            //20200529 修改
                        }
                        break;
                    }
                }

                _conds.Add(FeO_Turn);

            }
            //烧返配比变化
            {
                int hotRet = -1;
                //触发条件：烧返配比变化调用（即MC_MIXCAL_RESULT_1MIN表，SINCAL_BRUN_DRY字段，前后两条记录发生变化触发）
                bool _rs = Top2Turn(_mdb, "SINCAL_BRUN_DRY", "MC_MIXCAL_RESULT_1MIN", 2);
                if (!_rs)//相等无变化
                {
                    hotRet = 0;
                }
                else
                {
                    hotRet = 2;
                    _Count++;
                    mixlog.writelog("烧返配比变化满足", 0);
                }
                _conds.Add(hotRet);

            }
            //高返配比变化 
            // 凌源钢铁此段屏蔽
            {
                //触发条件：高返配比变化调用（即MC_MIXCAL_RESULT_1MIN表，SINCAL_BFES_ORE_BILL_DRY字段，前后两条记录发生变化触发）
                int HighRet = -1;
                bool _rs = Top2Turn(_mdb, "SINCAL_BFES_ORE_BILL_DRY", "MC_MIXCAL_RESULT_1MIN", 2);
                if (!_rs)//相等无变化
                {
                    HighRet = 0;
                }
                else
                {
                    HighRet = 3;
                    _Count++;
                    mixlog.writelog("高返配比变化满足", 0);
                }
                HighRet = 0;//20201201修改
                _conds.Add(HighRet);

            }
            //混合料综合烧损变化
            {
                //触发条件：配料计算模型计算混合料综合烧损变化调用（即MC_SINCAL_result_1min表，SINCAL_MIX_SP_LOT字段，前后两条记录发生变化触发）
                //MC_MIXCAL_RESULT_1MIN
                int MixMatRet = -1;
                bool _rs = Top2Turn(_mdb, "SINCAL_MIX_SP_LOT", "MC_MIXCAL_RESULT_1MIN", 2);
                if (!_rs)//相等无变化
                {
                    MixMatRet = 0;
                }
                else
                {
                    MixMatRet = 4;
                    _Count++;
                    mixlog.writelog("混合料综合烧损变化满足", 0);
                }
                _conds.Add(MixMatRet);

            }
            //非燃料含碳变化
            {
                //触发条件：配料计算模型计算出的非燃料含碳变化调用（即MC_SINCAL_result_1min表，SINCAL_NON_FUEL_SP_C字段，前后两条记录发生变化触发）
                int noFuelC = -1;
                bool _rs = Top2Turn(_mdb, "SINCAL_NON_FUEL_SP_C", "MC_MIXCAL_RESULT_1MIN", 2);
                if (!_rs)//相等无变化
                {
                    noFuelC = 0;
                }
                else
                {
                    noFuelC = 5;
                    _Count++;
                    mixlog.writelog("非燃料含碳变化满足", 0);
                }
                _conds.Add(noFuelC);

            }
            //原料FeO变化
            {
                //触发条件：配料计算模型计算混合料FeO含量变化调用（即MC_SINCAL_result_1min表，SINCAL_MIX_SP_FeO字段，前后两条记录发生变化触发）
                int yFeoTurn = -1;
                bool _rs = Top2Turn(_mdb, "SINCAL_MIX_SP_FeO", "MC_MIXCAL_RESULT_1MIN", 2);
                if (!_rs)//相等无变化
                {
                    yFeoTurn = 0;
                }
                else
                {
                    yFeoTurn = 6;
                    _Count++;
                    mixlog.writelog("原料FeO变化满足", 0);
                }
                _conds.Add(yFeoTurn);

            }
            //主机参数变化
            {
                int _rsTurnAdd = MainArg_Conds(_mdb, 1);
                int _rsTurnMinuse = MainArg_Conds(_mdb, 2);
                int _MainArg = _rsTurnAdd == 0 ? _rsTurnMinuse == 0 ? 0 : _rsTurnMinuse : _rsTurnAdd;
                _conds.Add(_MainArg);
                if (_MainArg > 0)
                {
                    _Count++;
                    mixlog.writelog("主机参数变化满足" + _MainArg, 0);
                }

            }
            mixlog.writelog("C_Conditions==END", 0);
            return new Tuple<int, List<int>, string>(_Count, _conds, _batchnum);
        }
        /// <summary>
        /// 碳自动投入状态
        /// </summary>
        /// <returns>
        /// 
        /// true:投入自动    false:为投入自动
        /// </returns>
        public bool C_AutoState()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //20200529 数据库字段名MAT_L2_butC1改为MAT_L2_but_C
            string sql_0 = "select MAT_L2_but_C from CFG_MAT_L2_Butsig_INTERFACE order by timestamp desc";
            DataTable _dt = _mdb.GetCommand(sql_0);
            if (_dt == null)
            {
                return false;
            }
            var _vdt = _dt.AsEnumerable();
            int autoFlag = 0;
            foreach (var x in _vdt)
            {
                autoFlag = int.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
                break;
            }

            return autoFlag == 1 ? true : false;
        }
        /// <summary>
        /// 碳 调整
        /// </summary>
        /// <returns>
        /// item1:true:结果正常，false:结果不正常
        /// item2: 碳调整值
        /// </returns>
        DateTime vDt_Main = new DateTime();//20200727
        bool v_first = true;//20200727
        int v_turnfirst = 0;//20200727
        public Tuple<bool, float> C_Modify()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //参数值
            //K1：烧结矿FeO变化含碳量调整投入 / 退出键选；参数值：MC_SINCAL_C_R_PAR表，C_PAR_FEO_SIN_KEY字段；
            //K2：烧返配比变化含碳量调整投入 / 退出键选；参数值：MC_SINCAL_C_R_PAR表，C_PAR_BILL_SIN_RM_KEY字段；
            //K3：高返配比变化含碳量调整投入 / 退出键选；参数值：MC_SINCAL_C_R_PAR表，C_PAR_BILL_BFES_KEY字段；
            //K4：综合烧损变化含碳量调整投入 / 退出键选；参数值：MC_SINCAL_C_R_PAR表，C_PAR_LOT_KEY字段；
            //K5：非燃料含碳变化含碳量调整投入 / 退出键选；参数值：MC_SINCAL_C_R_PAR表，C_PAR_NONFUEL_KEY字段；
            //K6：原料FeO变化含碳量调整投入 / 退出键选；参数值：MC_SINCAL_C_R_PAR表，C_PAR_FEO_MA_KEY字段；
            //K7：主机参数变化含碳量调整投入 / 退出键选；参数值：MC_SINCAL_C_R_PAR表，C_PAR_C_HOST_KEY字段；
            float vADJ_MIN = 0;//ADJ_MIN:  ;参数值：MC_SINCAL_C_R_PAR表，C_PAR_ADJ_MIN字段；
            float vADJ_MAX = 0;//ADJ_MAX:  ;参数值：MC_SINCAL_C_R_PAR表，C_PAR_ADJ_MAX字段；
            int vC_PAR_C_HOST_T = 0;
            float[] vK = new float[7];
            string sql_0 = "select C_PAR_FEO_SIN_KEY,C_PAR_BILL_SIN_RM_KEY,C_PAR_BILL_BFES_KEY,C_PAR_LOT_KEY,C_PAR_NONFUEL_KEY,C_PAR_FEO_MA_KEY,C_PAR_C_HOST_KEY,C_PAR_ADJ_MIN,C_PAR_ADJ_MAX,C_PAR_C_HOST_T from MC_SINCAL_C_R_PAR";
            DataTable _dt = _mdb.GetCommand(sql_0);

            var _vdt = _dt.AsEnumerable();
            if (_vdt.Count() <= 0)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                int Cols = _vdt.ElementAt(0).ItemArray.Count();
                for (int c = 0; c < Cols - 3; c++)
                {
                    vK[c] = float.Parse(_vdt.ElementAt(0)[c].ToString());
                }
                vADJ_MIN = float.Parse(_vdt.ElementAt(0)[Cols - 3].ToString());
                vADJ_MAX = float.Parse(_vdt.ElementAt(0)[Cols - 2].ToString());
                vC_PAR_C_HOST_T = int.Parse(_vdt.ElementAt(0)[Cols - 1].ToString());
            }

            //计算值
            var conds = C_Conditions();

            int[] trueLove = new int[] { 0, 0, 0, 0, 0, 0, 0 };
            float C_FeO_RE_ADJ = 0;
            float C_BILL_SIN_RM_RE_ADJ = 0;
            float C_BILL_BFES_ORE_RE_ADJ = 0;
            float C_LOT_RE_ADJ = 0;
            float C_NONFUEL_RE_ADJ = 0;
            float C_FeO_MA_RE_ADJ = 0;
            float HOST_ADJ = 0;
            if (conds.Item2[0] == 1)
            {

                trueLove[0] = 1;
                var rs = FeO_Cpt(_mdb, conds.Item3);//20200727
                C_FeO_RE_ADJ = rs.Item1 == true ? rs.Item2 : 0;

            }
            if (conds.Item2[1] == 2)
            {
                trueLove[1] = 1;
                var rs = HotRet_Cpt(_mdb);
                C_BILL_SIN_RM_RE_ADJ = rs.Item1 == true ? rs.Item2 : 0;
            }
            if (conds.Item2[2] == 3)
            {
                trueLove[2] = 1;
                var rs = HighRet_Cpt(_mdb);
                C_BILL_BFES_ORE_RE_ADJ = rs.Item1 == true ? rs.Item2 : 0;
            }
            if (conds.Item2[3] == 4)
            {
                trueLove[3] = 1;
                var rs = MixMatRet_CPt(_mdb);
                C_LOT_RE_ADJ = rs.Item1 == true ? rs.Item2 : 0;
            }
            if (conds.Item2[4] == 5)
            {
                trueLove[4] = 1;
                var rs = NoFuelC_Cpt(_mdb);
                C_NONFUEL_RE_ADJ = rs.Item1 == true ? rs.Item2 : 0;
            }
            if (conds.Item2[5] == 6)
            {
                trueLove[5] = 1;
                var rs = yFeoTurn_Cpt(_mdb);
                C_FeO_MA_RE_ADJ = rs.Item1 == true ? rs.Item2 : 0;
            }
            if (conds.Item2[6] == 7 || conds.Item2[6] == 8)
            {
                //20200727 修改   满足条件插入频率    相邻两次变化异向，直接存库； 相邻两次变化同向，时间差大于vC_PAR_C_HOST_T分钟，存库
                if (v_first)
                {
                    mixlog.writelog("第一次主机参数变化:" + (conds.Item2[6] == 7 ? "增加" : "减少"), 0);
                    vDt_Main = DateTime.Now;
                    v_first = false;
                    v_turnfirst = conds.Item2[6];
                    trueLove[6] = 1;
                    var rs = MainArg_Cpt(_mdb, conds.Item2[6]);
                    HOST_ADJ = rs.Item1 == true ? rs.Item2 : 0;
                }
                else if (conds.Item1 == 1 && v_turnfirst != conds.Item2[6])//变化方向不同
                {
                    mixlog.writelog("主机参数变化方向不同:由" + (v_turnfirst == 7 ? "增加" : "减少") + ";变为" + (conds.Item2[6] == 7 ? "增加" : "减少"), 0);
                    v_turnfirst = conds.Item2[6];
                    trueLove[6] = 1;
                    var rs = MainArg_Cpt(_mdb, conds.Item2[6]);
                    HOST_ADJ = rs.Item1 == true ? rs.Item2 : 0;
                }
                else if (conds.Item1 == 1 && (DateTime.Now - vDt_Main).TotalMinutes >= vC_PAR_C_HOST_T)
                {
                    vDt_Main = DateTime.Now;
                    trueLove[6] = 1;
                    var rs = MainArg_Cpt(_mdb, conds.Item2[6]);
                    HOST_ADJ = rs.Item1 == true ? rs.Item2 : 0;
                }
                else if (conds.Item1 == 1)
                {
                    trueLove[6] = 0;
                }
                else
                {
                    trueLove[6] = 0;//20200729 修改  闫龙飞
                    //var rs = MainArg_Cpt(_mdb, conds.Item2[6]);
                    //HOST_ADJ = rs.Item1 == true ? rs.Item2 : 0;
                }
            }

            float vC_COM_ADJ = 0;//C_COM_ADJ：综合配碳量调整量（未经过上下限限制）；计算值：存储位置：MC_SINCAL_C_result表，SINCAL_C_COM_ADJ字段；
            float vC_COM_RE_ADJ = 0;//C_COM_RE_ADJ：综合配碳量调整量（经过上下限限制）；计算值：存储位置：MC_SINCAL_C_result表，SINCAL_C_COM_RE_ADJ字段；
            vC_COM_ADJ = vK[1 - 1] * C_FeO_RE_ADJ + vK[2 - 1] * C_BILL_SIN_RM_RE_ADJ + vK[3 - 1] * C_BILL_BFES_ORE_RE_ADJ + vK[4 - 1] * C_LOT_RE_ADJ + vK[5 - 1] * C_NONFUEL_RE_ADJ + vK[6 - 1] * C_FeO_MA_RE_ADJ + vK[7 - 1] * HOST_ADJ;
            //202000605 修改
            float vvC_COM_ADJ = Math.Abs(vC_COM_ADJ);
            if (vvC_COM_ADJ <= vADJ_MIN)
            {
                vC_COM_RE_ADJ = 0;
            }
            else
            {
                if (vC_COM_ADJ >= vADJ_MAX)
                {
                    vC_COM_RE_ADJ = vADJ_MAX;
                }
                else if (vC_COM_ADJ <= -1 * vADJ_MAX)
                {
                    vC_COM_RE_ADJ = -1 * vADJ_MAX;
                }
                else if (vADJ_MIN < vvC_COM_ADJ && vvC_COM_ADJ <= vADJ_MAX)
                {
                    vC_COM_RE_ADJ = vC_COM_ADJ;//20200729 修改   与闫龙飞
                }
            }

            //20200605 修改
            //20200604 新增修改
            string sql_55 = "select p.SINCAL_C_A,p.SINCAL_C_DC from MC_MIXCAL_RESULT_1MIN p order by timestamp desc";
            float C_a = 0;
            float C_dc = 0;
            DataTable _dt55 = _mdb.GetCommand(sql_55);

            if (_dt55 == null)
            {
                return new Tuple<bool, float>(false, 0);
            }
            else
            {
                var _vdt55 = _dt55.AsEnumerable();
                foreach (var x in _vdt55)
                {
                    C_a = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
                    C_dc = float.Parse(x[1].ToString() == "" ? "0" : x[1].ToString());
                    break;
                }

            }
            float SINCAL_C_BEFORE_Modify = 0;
            float SINCAL_C_AFTER_Modify = 0;
            float SINCAL_C_SV_R_BE = 0;
            float SINCAL_C_SV_R = 0;
            SINCAL_C_BEFORE_Modify = C_dc;
            SINCAL_C_AFTER_Modify = vC_COM_RE_ADJ + SINCAL_C_BEFORE_Modify;
            SINCAL_C_SV_R_BE = C_a + SINCAL_C_BEFORE_Modify;
            SINCAL_C_SV_R = C_a + SINCAL_C_AFTER_Modify;

            //20200605
            //存储到数据库
            if (conds.Item1 <= 0)
            {
                mixlog.writelog("C调整条件不满足", -1);
                return new Tuple<bool, float>(false, 0);
            }
            else if (conds.Item1 == 1 && (conds.Item2[6] == 7 || conds.Item2[6] == 8) && trueLove[6] == 0)
            {
                mixlog.writelog("C调整主机参数条件满足但时间太近不插库", 1);
                return new Tuple<bool, float>(false, 0);
            }

            int cs = C_AutoState() == true ? 1 : 0;//20200605
            bool _iflag = isInsertedNew(_mdb);
            int _rsflag = vC_COM_RE_ADJ == 0 ? 5 : 1;//202020729 修改  与闫龙飞沟通
            string sql_ins = "insert into MC_SINCAL_C_result(TIMESTAMP,SINCAL_C_COM_ADJ,SINCAL_C_COM_RE_ADJ,SINCAL_C_FLAG,SINCAL_C_MODEL_FLAG,SINCAL_C_BEFORE_Modify,SINCAL_C_AFTER_Modify,SINCAL_C_BEFORE_SV_C,SINCAL_C_SV_C,SINCAL_C_FEO_FLAG,SINCAL_C_BILL_FLAG,SINCAL_C_BILL_BFES_ORE_FLAG,SINCAL_C_LOT_FLAG,SINCAL_C_NONFUEL_FLAG,SINCAL_C_FEO_MA_FLAG,SINCAL_C_HOST_FLAG,SINCAL_C_A)" +
            " values('" + DateTime.Now + "'," + vC_COM_ADJ + "," + vC_COM_RE_ADJ + "," + _rsflag + "," + cs + "," + SINCAL_C_BEFORE_Modify + "," + SINCAL_C_AFTER_Modify + "," + SINCAL_C_SV_R_BE + "," + SINCAL_C_SV_R + "," + trueLove[0] + "," + trueLove[1] + "," + trueLove[2] + "," + trueLove[3] + "," + trueLove[4] + "," + trueLove[5] + "," + trueLove[6] + "," + C_a + ")";
            string sql_update = "update MC_SINCAL_C_result set TIMESTAMP='" + DateTime.Now + "',SINCAL_C_COM_ADJ=" + vC_COM_ADJ + ",SINCAL_C_COM_RE_ADJ=" + vC_COM_RE_ADJ + ",SINCAL_C_FLAG=" + 1 + ",SINCAL_C_MODEL_FLAG=" + cs
                + ",SINCAL_C_BEFORE_Modify=" + SINCAL_C_BEFORE_Modify + ",SINCAL_C_AFTER_Modify=" + SINCAL_C_AFTER_Modify + ",SINCAL_C_BEFORE_SV_C=" + SINCAL_C_SV_R_BE + ",SINCAL_C_SV_C=" + SINCAL_C_SV_R + ", SINCAL_C_FEO_FLAG=" + trueLove[0] + ", SINCAL_C_BILL_FLAG=" + trueLove[1] + ", SINCAL_C_BILL_BFES_ORE_FLAG=" + trueLove[2] + ", SINCAL_C_LOT_FLAG=" + trueLove[3] + ", SINCAL_C_NONFUEL_FLAG=" + trueLove[4] + ", SINCAL_C_FEO_MA_FLAG=" + trueLove[5] + ", SINCAL_C_HOST_FLAG=" + trueLove[6] + ", SINCAL_C_A=" + C_a
                + " where SINCAL_C_FLAG=3";
            string sql_insert = _iflag == true ? sql_update : sql_ins;
            int _rs = _mdb.CommandExecuteNonQuery(sql_insert);
            if (_rs > 0)
            {
                mixlog.writelog("C调整插入或者更新MC_SINCAL_C_result数据成功", 0);//需要插入数据库
                return new Tuple<bool, float>(true, vC_COM_RE_ADJ);
            }
            else
            {
                mixlog.writelog("C调整插入或者更新MC_SINCAL_C_result数据失败(" + sql_insert + ")", -1);//需要插入数据库
                return new Tuple<bool, float>(false, 0);
            }
        }

        /// <summary>
        /// 碱度自动投入状态
        /// </summary>
        /// <returns>
        /// 
        /// true:投入自动    false:为投入自动
        /// </returns>
        public bool R_AutoState()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);

            //20200529 数据库字段名MAT_L2_butR1改为MAT_L2_but_R
            string sql_0 = "select MAT_L2_but_R from CFG_MAT_L2_Butsig_INTERFACE order by timestamp desc";
            DataTable _dt = _mdb.GetCommand(sql_0);
            var _vdt = _dt.AsEnumerable();
            int autoFlag = 0;
            foreach (var x in _vdt)
            {
                autoFlag = int.Parse(x[0].ToString());
                break;
            }

            return autoFlag == 1 ? true : false;
        }
        //加样激活
        public bool R_ADDState()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);

            //20200529 数据库字段名MAT_L2_butR1改为MAT_L2_but_R
            string sql_0 = "select PAR_S_ADD_FLAG from MC_SINCAL_C_R_PAR order by timestamp desc";
            DataTable _dt = _mdb.GetCommand(sql_0);
            var _vdt = _dt.AsEnumerable();
            int ADDFlag = 0;
            foreach (var x in _vdt)
            {
                ADDFlag = int.Parse(x[0].ToString());
                break;
            }

            return ADDFlag == 1 ? true : false;
        }
        /// <summary>
        /// 碱度调整
        /// </summary>
        ///  <returns>
        /// 碱度调整值
        /// </returns>
        public float R_Modify()
        {
            //满足碱度调整条件
            var rc = R_Condition();
            if (rc.Item1 == true)
            {

                int R_State = R_AutoState() == true ? 1 : 0;//碱度自动是否投入 1：投入  0：退出
                int ADD_State = R_ADDState() == true ? 1 : 0;//加样是否激活
                mixlog.writelog("碱度调整条件满足,自动是否投入:" + R_State, 0);
                /// R_W,K_OUTSIDE,K_INSIDE,ADJ_MIN,ADJ_MAX
                var R_par = R_Params();

                if (R_par.Rest.Item1 == false)
                {
                    mixlog.writelog("碱度调整参数异常，碱度调整不执行", -1);
                    return -1;
                }
                var R_cpt = R_ComputeVal();
                if (R_cpt.Item1 == false)
                {
                    mixlog.writelog("碱度调整计算值异常(倒推时间等)，碱度调整不执行", -1);
                    return -1;
                }
                var R_collect = R_CollectVal();
                if (R_collect.Item3 == false)
                {
                    mixlog.writelog("碱度调整采集值异常(SINCAL_R_A、SINCAL_R_C)，碱度调整不执行", -1);
                    return -1;
                }
                //烧结矿化验碱度值
                float R_TEST = rc.Item2;
                float CaO_TEST = rc.Item4;//20200914
                float SiO2_TEST = rc.Item5;//20200914
                //R_RET：倒推时间内预测烧结矿平均碱度
                float R_RET = R_cpt.Item2;
                //R_CUR：当前配料室下料预测碱度
                float R_CUR = R_cpt.Item3;
                //倒推时间
                float R_RETRODICT_TIME = R_cpt.Item4;
                //R_AIM：目标碱度（中线值）
                float R_AIM = R_collect.Item1;
                //R_DC: 前  碱度调整值   20200604 新增
                float R_DC = R_collect.Item2;
                //SINCAL_R_RE_TIME_FLAG:获取倒推时间成功标志（1：成功；0：失败主界面倒推程序异常）
                int vSINCAL_R_RE_TIME_FLAG = R_cpt.Item5;

                //R_W：烧结矿碱度正常波动范围
                float R_W = R_par.Item1;
                //K_OUTSIDE：检测碱度在目标碱度正常波动范围外的修正系数
                float K_OUTSIDE = R_par.Item2;
                //K_INSIDE：检测碱度在目标碱度正常波动范围外的修正系数
                float K_INSIDE = R_par.Item3;
                //ADJ_MIN：每次计算碱度调整量下限值
                float ADJ_MIN = R_par.Item4;
                //ADJ_MAX：每次计算碱度调整量上限值
                float ADJ_MAX = R_par.Item5;
                //开始计算
                float R_PF = 0;
                //20200916 新增逻辑
                int SINcal_SPE_ADD_NOTES = 0;
                var rssp = SamplePlus(rc.Item3, 2);//加样判断
                int SINCAL_SPE_ADD_FLAG = 2;
                //判断该批次是否加样 20201201新增逻辑是否启用加样判断
                if (ADD_State == 1 && rssp.Item1 == 1)//加样，不调整
                {
                    SINCAL_SPE_ADD_FLAG = 1;
                    mixlog.writelog("该批次(" + rc.Item3 + ")加样，碱度调整不执行", -1);
                    //加样逻辑
                    float curRVal = rssp.Item2;
                    float lastRVal = rssp.Item3;
                    if (R_AIM - R_W <= curRVal && curRVal <= R_AIM + R_W)
                    {
                        SINcal_SPE_ADD_NOTES = 5;
                        InsertLogTable("配料模型", "碱度调整", "加样碱度正常，上次调整量=" + lastRVal + ",请人工关注上批次碱度调整值变化");
                    }
                    else
                    {
                        if (curRVal > R_AIM + R_W && lastRVal > R_AIM + R_W)//都偏高
                        {
                            SINcal_SPE_ADD_NOTES = 1;
                            InsertLogTable("配料模型", "碱度调整", "加样碱度偏高=" + curRVal + "，上次检测碱度偏高=" + lastRVal + ",本次碱度不调整");
                        }
                        else if (curRVal < R_AIM - R_W && lastRVal < R_AIM - R_W)//都偏低
                        {
                            SINcal_SPE_ADD_NOTES = 2;
                            InsertLogTable("配料模型", "碱度调整", "加样碱度偏低=" + curRVal + "，上次检测碱度偏低=" + lastRVal + ",本次碱度不调整");
                        }
                        else if (curRVal > R_AIM + R_W && lastRVal < R_AIM - R_W)//加样碱度偏高，上次偏低
                        {
                            SINcal_SPE_ADD_NOTES = 3;
                            InsertLogTable("配料模型", "碱度调整", "加样碱度偏高=" + curRVal + "，上次检测碱度偏低=" + lastRVal + ",请岗位人工确认");
                        }
                        else if (curRVal < R_AIM - R_W && lastRVal > R_AIM + R_W)//加样碱度偏低，上次偏高
                        {
                            SINcal_SPE_ADD_NOTES = 4;
                            InsertLogTable("配料模型", "碱度调整", "加样碱度偏低=" + curRVal + "，上次检测碱度偏高=" + lastRVal + ",请岗位人工确认");
                        }


                    }
                    //插入数据库

                    string xsql_0 = "insert into MC_SINCAL_R_result(TIMESTAMP,SINCAL_R_TEST,SINCAL_R_MODEL_FLAG,SINCAL_R_FLAG,SINCAL_R_SAMPLE_CODE,SINCAL_SPE_ADD_NOTES,SINCAL_SPE_ADD_FLAG)" + "" +
                        "values('" + DateTime.Now + "'," + R_TEST + "," + R_State + "," + R_State + ",'" + rc.Item3 + "'," + SINcal_SPE_ADD_NOTES + "," + SINCAL_SPE_ADD_FLAG + ")";

                    DBSQL _mdbs = new DBSQL(_connstring);
                    int xrs = _mdbs.CommandExecuteNonQuery(xsql_0);
                    if (xrs <= 0)
                    {
                        mixlog.writelog("插入数据库表MC_SINCAL_R_result失败(" + xsql_0 + ")", -1);
                    }
                    else
                    {
                        mixlog.writelog("插入数据库表MC_SINCAL_R_result成功(" + xsql_0 + ")", 0);
                    }
                    //加样逻辑
                    return -1;
                }
                //不加样
                //20200916
                if (R_AIM - R_W <= R_TEST && R_TEST <= R_AIM + R_W)
                {
                    R_PF = K_INSIDE * (R_AIM - R_TEST);
                }
                else
                {
                    R_PF = K_OUTSIDE * (R_AIM - R_TEST);
                }
                //20200603 修改 碱度调整计算
                float R_ADJ = 0;
                if (R_RET <= 0 || R_CUR <= 0)
                {
                    R_ADJ = R_PF;

                }
                else
                {
                    //R_ADJ：计算碱度调整量：计算碱度调整量
                    R_ADJ = R_RET - R_CUR + R_PF;
                }
                //20200915 与闫龙飞确认过
                //20201201 凌源钢铁屏蔽此段程序
                //if (R_TEST < R_AIM - R_W)
                //{
                //    //20200915 与闫龙飞 确认过
                //    /*
                //     * 1）“CAO_TEST正常”判断条件：判断CAO_TEST与前n批CAO平均值（CAO_AV）进行比较，
                //    如果|CAO_TEST - CAO_AV|< PAR_CAO_RANGE，则认为该批次烧结矿CAO正常；
                //    2)“SIO2_TEST正常”判断条件：判断SIO2_TEST与前n批SIO2平均值（SIO2_AV）进行比较，
                //    如果|SIO2_TEST - SIO2_AV|< PAR_SIO2_RANGE1，则认为该批次烧结矿SIO2正常；
                //    3）“CAO_TEST偏低”判断条件：判断CAO_TEST与前n批CAO平均值（CAO_AV）进行比较，
                //    如果CAO_TEST - CAO_AV < -PAR_CAO_RANGE，则认为该批次烧结矿CAO偏低；
                //    4）“SIO2_TEST偏低”判断条件：判断SIO2_TEST与前n批SIO2平均值（SIO2_AV）进行比较，
                //    如果SIO2_TEST - SIO2_AV < -PAR_SIO2_RANGE1，则认为该批次烧结矿SIO2偏低；
                //    5）“SIO2_TEST偏高”判断条件：判断SIO2_TEST与前n批SIO2平均值（SIO2_AV）进行比较，
                //    如果SIO2_TEST - SIO2_AV > PAR_SIO2_RANGE2，则认为该批次烧结矿SIO2偏高；（注：以上均取非0非空平均）
                //     */
                //    int n = R_par.Item6;
                //    float vPAR_CAO_RANGE = R_par.Item7[0];
                //    float vPAR_SIO2_RANGE1 = R_par.Item7[1];
                //    float vPAR_SIO2_RANGE2 = R_par.Item7[2];
                //    float vPAR_K_CAO = R_par.Item7[3];
                //    float vPAR_K_SIO2 = R_par.Item7[4];
                //    float vPAR_K_SIO2_CAO = R_par.Item7[5];
                //    DateTime __dt = rc.Item6;

                //    var rowR = LGetTimesAvg("M_SINTER_ANALYSIS", "C_CAO,C_SIO2", n, " where timestamp!= '" + __dt + "' and C_CAO!=0 and C_CAO is not null and C_SIO2!=0 and C_SIO2 is not null");

                //    if (rowR != null)
                //    {
                //        float avgCaO = float.Parse(rowR[0].ToString());
                //        float avgSiO2 = float.Parse(rowR[1].ToString());
                //        mixlog.writelog("avgCaO=" + avgCaO + ";avgSiO2=" + avgSiO2, 0);
                //        List<int> _judge = new List<int>();
                //        bool CaOOk = Math.Abs(CaO_TEST - avgCaO) < vPAR_CAO_RANGE;
                //        bool CaOFlag = (CaO_TEST - avgCaO) < -1 * vPAR_CAO_RANGE;//CaO偏低
                //        bool SiO2FlagD = (SiO2_TEST - avgSiO2) < -1 * vPAR_SIO2_RANGE1;//SiO2偏低
                //        bool SiO2FlagG = (SiO2_TEST - avgSiO2) > vPAR_SIO2_RANGE2;//SiO2偏高
                //        bool SiO2Ok = Math.Abs(SiO2_TEST - avgSiO2) < vPAR_SIO2_RANGE1;
                //        //20200915  1728
                //        if (CaOFlag && SiO2FlagD)
                //        {
                //            mixlog.writelog("CaO和SiO2都偏低", 0);
                //            R_ADJ = vPAR_K_SIO2_CAO * R_ADJ;
                //        }
                //        else if (SiO2FlagG && CaOOk)
                //        {
                //            mixlog.writelog("CaO正常和SiO2偏高", 0);
                //            R_ADJ = vPAR_K_SIO2 * R_ADJ;
                //        }
                //        else if (SiO2Ok && CaOFlag)
                //        {
                //            mixlog.writelog("CaO偏低和SiO2正常", 0);
                //            R_ADJ = vPAR_K_CAO * R_ADJ;
                //        }
                //        else if (SiO2Ok && CaOOk)
                //        {
                //            mixlog.writelog("CaO正常和SiO2正常", 0);

                //        }
                //    }
                //    else
                //    {
                //        mixlog.writelog("M_SINTER_ANALYSIS表数据异常", -1);
                //    }

                //}

                //20200915

                //R_RE_ADJ：经过有效性判断下发的碱度调整量
                float R_RE_ADJ = 0;
                float vR_ADJ = Math.Abs(R_ADJ);
                if (vR_ADJ <= ADJ_MIN)
                {
                    R_RE_ADJ = 0;
                }
                else
                {
                    if (R_ADJ >= ADJ_MAX)
                    {
                        R_RE_ADJ = ADJ_MAX;
                    }
                    else if (R_ADJ <= -1 * ADJ_MAX)
                    {
                        R_RE_ADJ = -1 * ADJ_MAX;
                    }
                    else if (vR_ADJ > ADJ_MIN && vR_ADJ < ADJ_MAX)
                    {
                        R_RE_ADJ = R_ADJ;
                    }
                }
                //20200603
                //if (R_ADJ > ADJ_MAX)
                //{
                //    R_RE_ADJ = ADJ_MAX;
                //}
                //else if (R_ADJ < ADJ_MIN)
                //{
                //    R_RE_ADJ = ADJ_MIN;
                //}
                //else
                //{
                //    R_RE_ADJ = R_ADJ;
                //}
                //20200604 新增
                //调整后混合料碱度调整值（SINCAL_R_BEFORE_Modify+SINCAL_R_RE_ADJ）
                //调整前混合料碱度标准值(SINCAL_R_AIM + SINCAL_R_BEFORE_Modify)
                //调整后混合料碱度标准值(SINCAL_R_AIM + SINCAL_R_AFTER_Modify)

                float SINCAL_R_AFTER_Modify = 0;
                float SINCAL_R_SV_R_BE = 0;
                float SINCAL_R_SV_R = 0;
                SINCAL_R_AFTER_Modify = R_DC + R_RE_ADJ;
                SINCAL_R_SV_R_BE = R_AIM + R_DC;
                SINCAL_R_SV_R = R_AIM + SINCAL_R_AFTER_Modify;
                mixlog.writelog("碱度调整值=" + R_RE_ADJ, 0);
                //插入数据库
                string sql_0 = "insert into MC_SINCAL_R_result(TIMESTAMP,SINCAL_R_TEST,SINCAL_R_RE,SINCAL_R_RETRODICT_TIME,SINCAL_R_AIM,SINCAL_R_PF,SINCAL_R_PRE_AVG,SINCAL_R_ADJ,SINCAL_R_RE_ADJ,SINCAL_R_MODEL_FLAG,SINCAL_R_FLAG,SINCAL_R_SAMPLE_CODE,SINCAL_R_BEFORE_Modify,SINCAL_R_AFTER_Modify,SINCAL_R_SV_R_BE,SINCAL_R_SV_R,SINCAL_R_RE_TIME_FLAG,SINCAL_SPE_ADD_FLAG)" + "" +
                    "values('" + DateTime.Now + "'," + R_TEST + "," + R_RET + "," + R_RETRODICT_TIME + "," + R_AIM + "," + R_PF + "," + R_CUR + "," + R_ADJ + "," + R_RE_ADJ + "," + R_State + "," + R_State + ",'" + rc.Item3 + "'," + R_DC + "," + SINCAL_R_AFTER_Modify + "," + SINCAL_R_SV_R_BE + "," + SINCAL_R_SV_R + "," + vSINCAL_R_RE_TIME_FLAG + "," + SINCAL_SPE_ADD_FLAG + ")";
                //20200604 修改
                //初始化数据库
                DBSQL _mdb = new DBSQL(_connstring);
                int rs = _mdb.CommandExecuteNonQuery(sql_0);
                if (rs <= 0)
                {
                    mixlog.writelog("插入数据库表MC_SINCAL_R_result失败(" + sql_0 + ")", -1);
                }
                else
                {
                    mixlog.writelog("插入数据库表MC_SINCAL_R_result成功", 0);
                }
                return R_RE_ADJ;

            }
            else
            {
                //碱度调整不满足
                mixlog.writelog("碱度调整条件不满足", 1);
                return -1;
            }

        }


        /// <summary>
        /// Mg自动投入状态
        /// </summary>
        /// <returns>
        /// 
        /// true:投入自动    false:为投入自动
        /// </returns>
        public bool Mg_AutoState()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //20200529 数据库字段名MAT_L2_butMg1改为MAT_L2_but_Mg
            string sql_0 = "select MAT_L2_but_Mg from CFG_MAT_L2_Butsig_INTERFACE order by timestamp desc";
            DataTable _dt = _mdb.GetCommand(sql_0);
            var _vdt = _dt.AsEnumerable();
            int autoFlag = 0;
            foreach (var x in _vdt)
            {
                autoFlag = int.Parse(x[0].ToString());
                break;//只判断第一条  20200529
            }

            return autoFlag == 1 ? true : false;
        }

        /// <summary>
        /// Mg调整条件
        /// </summary>
        /// <returns>true:满足    false:不满足</returns>
        public Tuple<bool, float, string> Mg_Condition()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //M_SINTER_ANALYSIS 标志位  代表插入、更新
            //是否更新或者插入
            string sql = "select isnull(s.C_MGO,0) C_MGO,isnull(s.flag,0),isnull(s.BATCH_NUM,'') from dbo.M_SINTER_ANALYSIS s where s.flag>0  order by s.TIMESTAMP desc";//"select isnull(s.C_MGO,0) C_MGO,s.flag,isnull(s.BATCH_NUM,'') from dbo.M_SINTER_ANALYSIS s where s.flag>0 order by s.TIMESTAMP desc";

            DataTable _dt = _mdb.GetCommand(sql);

            var _vdt = _dt.AsEnumerable();
            foreach (var x in _vdt)//只取最近一条
            {
                int c = x.ItemArray.Count();
                int vflag = int.Parse(x[1].ToString());
                if (vflag <= 0)
                {
                    return new Tuple<bool, float, string>(false, 0, "");
                }
                float vCR = float.Parse(x[0].ToString());
                if (vCR == 0)
                {
                    return new Tuple<bool, float, string>(false, vCR, "");
                }
                else
                {
                    string batchNum = x[2].ToString();
                    //20200529 修改
                    if (batchNum.Length < 3)
                    {
                        Console.WriteLine("批次号长度不正确");
                        return new Tuple<bool, float, string>(false, vCR, "");
                    }
                    else
                    {
                        string sql1 = "select * from MC_SINCAL_MG_result r where r.SINCAL_MG" +
                       "_SAMPLE_CODE='" + batchNum + "'";
                        DataTable _cdt = _mdb.GetCommand(sql1);
                        int _rows = _cdt.Rows.Count;
                        if (_rows == 0)
                        {
                            return new Tuple<bool, float, string>(true, vCR, batchNum);
                        }
                        else
                        {
                            return new Tuple<bool, float, string>(false, vCR, "");
                        }
                        //20200529 修改
                    }
                    //else
                    //{
                    //    Console.WriteLine("批次号格式不正确");
                    //    return new Tuple<bool, float, string>(false, vCR, "");
                    //}

                }
                //break;
            }
            return new Tuple<bool, float, string>(false, 0, "");

        }


        /// <summary>
        /// 获取Mg调整计算需要的参数值
        /// </summary>
        /// <returns>
        /// MG_W,K_OUTSIDE,K_INSIDE,ADJ_MIN,ADJ_MAX
        /// </returns>
        public Tuple<float, float, float, float, float, bool> Mg_Params()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            /*
             * MG_W：烧结矿碱度正常波动范围，参数值：MC_SINCAL_C_R_PAR表，MG_PAR_MG_W字段。
            K_OUTSIDE：检测碱度在目标碱度正常波动范围外的修正系数，参数表：MC_SINCAL_C_R_PAR表，MG_PAR_ K_OUTSIDE字段；
            K_INSIDE：检测碱度在目标碱度正常波动范围内的修正系数，参数表：MC_SINCAL_C_R_PAR表，MG_PAR_ K_INSIDE字段；
            ADJ_MIN：每次计算碱度调整量下限值；参数表：MC_SINCAL_C_R_PAR表，MG_PAR_ADJ_MIN字段；
            ADJ_MAX：每次计算碱度调整量上限值；参数表：MC_SINCAL_C_R_PAR表，MG_PAR_ADJ_MAX字段；*
            */
            string sql = "select p.MG_PAR_MG_W,p.MG_PAR_K_OUTSIDE,p.MG_PAR_K_INSIDE,p.MG_PAR_ADJ_MIN,p.MG_PAR_ADJ_MAX from dbo.MC_SINCAL_C_R_PAR p";

            DataTable _dt = _mdb.GetCommand(sql);
            if (_dt == null)
            {
                mixlog.writelog("MC_SINCAL_C_R_PAR连接存在问题", -1);
                return new Tuple<float, float, float, float, float, bool>(0, 0, 0, 0, 0, false);
            }
            var vdt = _dt.AsEnumerable();
            if (vdt.Count() <= 0)
            {
                mixlog.writelog("MC_SINCAL_C_R_PAR没有数据", 1);
                return new Tuple<float, float, float, float, float, bool>(0, 0, 0, 0, 0, false);
            }

            float[] vals = { 0, 0, 0, 0, 0 };
            foreach (var x in vdt)
            {
                for (int i = 0; i < x.ItemArray.Count(); i++)
                {
                    vals[i] = float.Parse(x[i].ToString());
                }
            }
            return new Tuple<float, float, float, float, float, bool>(vals[0], vals[1], vals[2], vals[3], vals[4], true);
        }


        /// <summary>
        /// Mg调整采集值
        /// </summary>
        /// <returns>
        /// 目标MgO
        /// 调整前混合料Mg调整值
        /// </returns>
        public Tuple<float, float, bool> Mg_CollectVal()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //MG_AIM：目标MgO（中线值）；采集值：存储位置：MC_SINCAL_MG_result 表，SINCAL_MG_AIM字段；（采集位置：MC_MIXCAL_RESULT_1MIN表，SINCAL_MG_A字段）
            //20200604 修改 新增SINCAL_MG_C
            string sql = "select SINCAL_MG_A,SINCAL_MG_C from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";
            DataTable _dt = _mdb.GetCommand(sql);
            if (_dt == null)
            {
                return new Tuple<float, float, bool>(0, 0, false);
            }
            var _vdt = _dt.AsEnumerable();
            foreach (var x in _vdt)
            {
                float midval = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
                float cval = float.Parse(x[1].ToString() == "" ? "0" : x[1].ToString());
                return new Tuple<float, float, bool>(midval, cval, true);
            }
            return new Tuple<float, float, bool>(0, 0, false);//数据表异常
        }



        /// <summary>
        /// Mg调整计算值
        /// </summary>
        /// 状态，倒推时间内预测烧结矿平均Mg，当前配料室下料预测Mg,倒推时间,获取倒推时间成功标志:1 yes 0 no
        public Tuple<bool, float, float, float, int> Mg_ComputeVal()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            /*
             R_RET：倒推时间内预测烧结矿平均MgO；计算值：存储位置：MC_SINCAL_MG_result 表，SINCAL_MG_RE字段；
（取倒推时间R_RETRODICT_TIME前PAR_T分钟内，MC_SINCAL_result_1min表，SINCAL_SIN_PV_MG
字段的平均值
    R_RETRODICT_TIME：MC_MICAL_RESULT表中最新一条，DATAMUN = 14 的记录的MICAL_SAM_MAT_TIME字段；
存储位置：MC_SINCAL_C_result表，SINCAL_R_RETRODICT_TIME字段
PAR_T：MC_SINCAL_C_R_PAR表，PAR_T字段;)
             */
            //获取PAR_T时间字段
            string sql_0 = "select PAR_T from MC_SINCAL_C_R_PAR";

            DataTable _dt = _mdb.GetCommand(sql_0);

            if (_dt == null)
            {
                return new Tuple<bool, float, float, float, int>(false, 0, 0, 0, 0);
            }
            //读取PAR_T的值
            var _vdt = _dt.AsEnumerable();
            int ipart = -1;
            foreach (var x in _vdt)
            {
                ipart = int.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }
            mixlog.writelog("Mg:PAR_T=" + ipart, 0);
            //20200604 新增修改
            //20200320
            var lastrow = LGetLastTime("M_SINTER_ANALYSIS");
            DateTime curDT1 = new DateTime();
            DateTime curDT2 = new DateTime();
            if (lastrow != null)
            {
                curDT1 = DateTime.Parse(lastrow["SAMPLETIME"].ToString());
                curDT2 = DateTime.Parse(lastrow["SAMPLETIME"].ToString());
            }
            else
            {
                curDT1 = DateTime.Now;
                curDT2 = DateTime.Now;
            }
            //获取R_RETRODICT_TIME
            //（已废弃）string sql_1 = "select Top(1) MICAL_SAM_MAT_TIME from MC_MICAL_RESULT where DATAMUN = 14 order by TIMESTAMP desc";
            string sql_1 = "select  AVG(MICAL_SAM_MAT_TIME) from MC_MICAL_RESULT where TIMESTAMP>'" + curDT1.AddHours(-2) + "' and TIMESTAMP<'" + curDT1 + "' and DATANUM = 14 and MICAL_SAM_MAT_TIME!=0 and MICAL_SAM_MAT_TIME is not null";
            DataTable _dt1 = _mdb.GetCommand(sql_1);
            int vSINCAL_Mg_RE_TIME_FLAG = 0;
            if (_dt1 == null)
            {
                return new Tuple<bool, float, float, float, int>(false, 0, 0, 0, 0);
            }
            var _vdt1 = _dt1.AsEnumerable();
            float strRT = 0;//倒推时间
            foreach (var x in _vdt1)
            {
                //strRT = int.Parse(x[0].ToString()==""?"0": x[0].ToString());
                strRT = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }
            mixlog.writelog("倒推时间:" + strRT, 0);
            //if(strRT<60)
            if ((strRT > T_PAR_MIN_TIME && strRT < T_PAR_MAX_TIME) == false)
            {
                strRT = 0;
                vSINCAL_Mg_RE_TIME_FLAG = 0;
            }
            else
            {

                vSINCAL_Mg_RE_TIME_FLAG = 1;
            }
            int _MgOoffsetTime = 15;
            string sql_2 = "select AVG(SINCAL_SIN_PV_MGO) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP>'" + curDT1.AddMinutes(-1 * strRT).AddMinutes(-1 * ipart).AddMinutes(-1 * _MgOoffsetTime) + "' and TIMESTAMP<='" + curDT2.AddMinutes(-1 * strRT).AddMinutes(-1 * _MgOoffsetTime) + "' and SINCAL_SIN_PV_MGO!=0 and SINCAL_SIN_PV_MGO is not null";
            DataTable _dt2 = _mdb.GetCommand(sql_2);

            if (_dt2 == null)
            {
                return new Tuple<bool, float, float, float, int>(false, 0, 0, 0, 0);
            }
            var _vdt2 = _dt2.AsEnumerable();
            float avg_PV_Mg = -1;
            foreach (var x in _vdt2)
            {
                avg_PV_Mg = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
            }

            //计算当前
            DateTime curDT3 = DateTime.Now;
            DateTime curDT4 = DateTime.Now;
            string sql_3 = "select AVG(SINCAL_SIN_PV_MGO) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP >'" + curDT3.AddMinutes(-1 * ipart * 0.2) + "' and TIMESTAMP <'" + curDT4 + "' and SINCAL_SIN_PV_MGO!=0 and SINCAL_SIN_PV_MGO is not null";
            DataTable _dt3 = _mdb.GetCommand(sql_3);

            if (_dt3 == null)
            {
                return new Tuple<bool, float, float, float, int>(false, 0, 0, 0, 0);
            }
            var _vdt3 = _dt3.AsEnumerable();
            float cur_PV_Mg = -1;
            foreach (var x in _vdt3)
            {
                cur_PV_Mg = float.Parse(x[0].ToString() == "" ? "0" : x[0].ToString());
                break;
            }
            mixlog.writelog("Mg实际平均值=" + avg_PV_Mg, 0);
            mixlog.writelog("Mg实际当前值=" + cur_PV_Mg, 0);
            return new Tuple<bool, float, float, float, int>(true, avg_PV_Mg, cur_PV_Mg, strRT, vSINCAL_Mg_RE_TIME_FLAG);
            //20200604 修改
        }

        /// <summary>
        /// Mg调整
        /// </summary>
        ///  <returns>
        /// Mg调整值
        /// </returns>
        public float Mg_Modify()
        {
            var rc = Mg_Condition();
            if (rc.Item1 == true)
            {
                /// R_W,K_OUTSIDE,K_INSIDE,ADJ_MIN,ADJ_MAX
                var Mg_par = Mg_Params();
                if (Mg_par.Item6 == false)
                {
                    mixlog.writelog("Mg 参数异常", -1);
                }
                var Mg_cpt = Mg_ComputeVal();
                if (Mg_cpt.Item1 == false)
                {
                    mixlog.writelog("Mg 计算值异常", -1);
                }
                var Mg_collect = Mg_CollectVal();//20200604 修改
                if (Mg_collect.Item3 == false)
                {
                    mixlog.writelog("Mg 采集值异常", -1);
                }
                //烧结矿化验碱度值
                float Mg_TEST = rc.Item2;
                //Mg_RET：倒推时间内预测烧结矿平均碱度
                float Mg_RET = Mg_cpt.Item2;
                //Mg_CUR：当前配料室下料预测碱度
                float Mg_CUR = Mg_cpt.Item3;
                //倒推时间
                float R_RETRODICT_TIME = Mg_cpt.Item4;
                //Mg_AIM：目标碱度（中线值）
                float Mg_AIM = Mg_collect.Item1;
                //SINCAL_Mg_BEFORE_Modify:前 Mg调整值

                float SINCAL_Mg_BEFORE_Modify = 0;
                SINCAL_Mg_BEFORE_Modify = Mg_collect.Item2;//20200604 修改
                float SINCAL_Mg_AFTER_Modify = 0;
                float SINCAL_Mg_SV_R_BE = 0;
                float SINCAL_Mg_SV_R = 0;
                float SINCAL_Mg_RE_TIME_FLAG = 0;
                SINCAL_Mg_RE_TIME_FLAG = Mg_cpt.Item5;
                string btnum = rc.Item3;
                //Mg_W：烧结矿碱度正常波动范围
                float Mg_W = Mg_par.Item1;
                //K_OUTSIDE：检测碱度在目标碱度正常波动范围外的修正系数
                float K_OUTSIDE = Mg_par.Item2;
                //K_INSIDE：检测碱度在目标碱度正常波动范围外的修正系数
                float K_INSIDE = Mg_par.Item3;
                //ADJ_MIN：每次计算碱度调整量下限值
                float ADJ_MIN = Mg_par.Item4;
                //ADJ_MAX：每次计算碱度调整量上限值
                float ADJ_MAX = Mg_par.Item5;
                //开始计算
                float Mg_PF = 0;

                //20200916 新增逻辑
                int SINcal_SPE_ADD_NOTES = 0;
                int ADD_State = R_ADDState() == true ? 1 : 0;//加样是否激活
                int SINCAL_SPE_ADD_FLAG = 2;
                var rssp = SamplePlus(rc.Item3, 3);
                if (ADD_State == 1 && rssp.Item1 == 1)//加样，不调整
                {
                    SINCAL_SPE_ADD_FLAG = 1;
                    mixlog.writelog("该批次(" + rc.Item3 + ")加样，MgO调整不执行", -1);
                    //加样逻辑
                    float curMgVal = rssp.Item2;
                    float lastMgVal = rssp.Item3;
                    if (Mg_AIM - Mg_W <= curMgVal && curMgVal <= Mg_AIM + Mg_W)
                    {
                        SINcal_SPE_ADD_NOTES = 5;
                        InsertLogTable("配料模型", "MgO调整", "加样MgO正常，上次调整量=" + lastMgVal + ",请人工关注上批次MgO调整值变化");
                    }
                    else
                    {
                        if (curMgVal > Mg_AIM + Mg_W && lastMgVal > Mg_AIM + Mg_W)//都偏高
                        {
                            SINcal_SPE_ADD_NOTES = 1;
                            InsertLogTable("配料模型", "MgO调整", "加样MgO偏高=" + curMgVal + "，上次检测MgO偏高=" + lastMgVal + ",本次MgO不调整");
                        }
                        else if (curMgVal < Mg_AIM - Mg_W && lastMgVal < Mg_AIM - Mg_W)//都偏低
                        {
                            SINcal_SPE_ADD_NOTES = 2;
                            InsertLogTable("配料模型", "MgO调整", "加样MgO偏低=" + curMgVal + "，上次检测MgO偏低=" + lastMgVal + ",本次MgO不调整");
                        }
                        else if (curMgVal > Mg_AIM + Mg_W && lastMgVal < Mg_AIM - Mg_W)//加样MgO偏高，上次偏低
                        {
                            SINcal_SPE_ADD_NOTES = 3;
                            InsertLogTable("配料模型", "MgO调整", "加样MgO偏高=" + curMgVal + "，上次检测MgO偏低=" + lastMgVal + ",请岗位人工确认");
                        }
                        else if (curMgVal < Mg_AIM - Mg_W && lastMgVal > Mg_AIM + Mg_W)//加样MgO偏低，上次偏高
                        {
                            SINcal_SPE_ADD_NOTES = 4;
                            InsertLogTable("配料模型", "MgO调整", "加样MgO偏低=" + curMgVal + "，上次检测MgO偏高=" + lastMgVal + ",请岗位人工确认");
                        }


                    }
                    //插入数据库

                    string xsql_0 = "insert into MC_SINCAL_MG_result(TIMESTAMP,SINCAL_MG_TEST,SINCAL_MG_SAMPLE_CODE,SINCAL_SPE_ADD_NOTES,SINCAL_SPE_ADD_FLAG)" + "" +
                        "values('" + DateTime.Now + "'," + Mg_TEST + ",'" + rc.Item3 + "'," + SINcal_SPE_ADD_NOTES + "," + SINCAL_SPE_ADD_FLAG + ")";

                    DBSQL _mdbs = new DBSQL(_connstring);
                    int xrs = _mdbs.CommandExecuteNonQuery(xsql_0);
                    if (xrs <= 0)
                    {
                        mixlog.writelog("插入数据库表 MC_SINCAL_MG_result 失败(" + xsql_0 + ")", -1);
                    }
                    else
                    {
                        mixlog.writelog("插入数据库表 MC_SINCAL_MG_result 成功(" + xsql_0 + ")", 0);
                    }
                    //加样逻辑
                    return -1;
                }
                //不加样
                //20200916

                if (Mg_AIM - Mg_W < Mg_TEST && Mg_TEST < Mg_AIM + Mg_W)
                {
                    Mg_PF = K_INSIDE * (Mg_AIM - Mg_TEST);
                }
                else
                {
                    Mg_PF = K_OUTSIDE * (Mg_AIM - Mg_TEST);
                }

                //20200603 修改 
                //R_ADJ：计算碱度调整量：计算碱度调整量
                float Mg_ADJ = 0;

                if (Mg_RET <= 0 || Mg_CUR <= 0)
                {
                    Mg_ADJ = Mg_PF;
                }
                else
                {
                    //R_ADJ：计算碱度调整量：计算碱度调整量
                    Mg_ADJ = Mg_RET - Mg_CUR + Mg_PF;
                }
                //20200603 
                //R_RE_ADJ：经过有效性判断下发的碱度调整量
                float Mg_RE_ADJ = 0;//20200320 修改
                if (Math.Abs(Mg_ADJ) <= ADJ_MIN)
                {
                    Mg_RE_ADJ = 0;
                }
                else
                {
                    if (Mg_ADJ >= ADJ_MAX)
                    {
                        Mg_RE_ADJ = ADJ_MAX;
                    }
                    else if (Mg_ADJ <= -1 * ADJ_MAX)
                    {
                        Mg_RE_ADJ = -1 * ADJ_MAX;
                    }
                    else if (Math.Abs(Mg_ADJ) < ADJ_MAX && Math.Abs(Mg_ADJ) > ADJ_MIN)
                    {
                        Mg_RE_ADJ = Mg_ADJ;
                    }
                }
                //20200604 修改
                SINCAL_Mg_AFTER_Modify = SINCAL_Mg_BEFORE_Modify + Mg_RE_ADJ;
                SINCAL_Mg_SV_R_BE = SINCAL_Mg_BEFORE_Modify + Mg_AIM;
                SINCAL_Mg_SV_R = SINCAL_Mg_AFTER_Modify + Mg_AIM;

                //插入数据库
                int Mg_flag = Mg_AutoState() == true ? 1 : 0;
                //插入数据库 是否时间字段插上？
                //SINCAL_R_BEFORE_Modify,SINCAL_R_AFTER_Modify,SINCAL_R_SV_R_BE,SINCAL_R_SV_R,SINCAL_R_RE_TIME_FLAG

                string sql_0 = "insert into MC_SINCAL_MG_result(TIMESTAMP,SINCAL_MG_TEST,SINCAL_MG_RE,SINCAL_MG_RETRODICT_TIME,SINCAL_MG_AIM,SINCAL_MG_PF,SINCAL_MG_PRE_AVG,SINCAL_MG_ADJ,SINCAL_MG_RE_ADJ,SINCAL_MG_FLAG,SINCAL_MG_MODEL_FLAG,SINCAL_MG_SAMPLE_CODE,SINCAL_MG_BEFORE_Modify,SINCAL_MG_AFTER_Modify,SINCAL_MG_SV_R_BE,SINCAL_MG_SV_R,SINCAL_MG_RE_TIME_FLAG,SINCAL_SPE_ADD_FLAG)" + "" +
                    "values('" + DateTime.Now + "'," + Mg_TEST + "," + Mg_RET + "," + R_RETRODICT_TIME + "," + Mg_AIM + "," + Mg_PF + "," + Mg_CUR + "," + Mg_ADJ + "," + Mg_RE_ADJ + "," + 1 + "," + Mg_flag + ",'" + btnum + "'," + SINCAL_Mg_BEFORE_Modify + "," + SINCAL_Mg_AFTER_Modify + "," + SINCAL_Mg_SV_R_BE + "," + SINCAL_Mg_SV_R + "," + SINCAL_Mg_RE_TIME_FLAG + "," + SINCAL_SPE_ADD_FLAG + ")";
                //初始化数据库
                DBSQL _mdb = new DBSQL(_connstring);
                int rs = _mdb.CommandExecuteNonQuery(sql_0);
                if (rs <= 0)
                {
                    mixlog.writelog("插入数据库表MC_SINCAL_MG_result失败(" + sql_0 + ")", -1);
                }
                else
                {
                    mixlog.writelog("插入数据库表MC_SINCAL_MG_result成功", 0);
                }
                return Mg_RE_ADJ;

            }
            else
            {
                //MgO调整不满足
                mixlog.writelog("MgO调整条件不满足", -1);
                return -1;
            }

        }
        /// <summary>
        /// 计算烧结矿设定成分
        /// </summary>
        /// <returns>
        /// 
        /// item1:计算结果 0：正常 其他：异常
        /// item2:预测设定成分
        /// </returns>
        //        public Tuple<int, List<float>> CalculateSinterBySP()//该函数是计算烧结矿设定成分

        //        {
        //            //初始化数据库
        //            DBSQL _mdb = new DBSQL(_connstring);
        //            //  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19,  20, 21, 22, 23, 24, 25, 26, 27, 28,  29
        //            // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO,  ZnO, F, As, Cu, Pb, Zn, K,  Na, Cr, Ni, MnO  //宁钢检化验成分排序
        //            //	                                             烧损         灰分 挥发分                            

        //            float sumTFe = 0;
        //            float sumFeO = 0;
        //            float sumCaO = 0;
        //            float sumSiO2 = 0;
        //            float sumAl2O3 = 0;
        //            float sumMgO = 0;
        //            float sumS = 0;
        //            float sumP = 0;
        //            float sumC = 0;
        //            float sumMn = 0;
        //            float sumLOT = 0;//总烧损
        //            float sumR = 0;
        //            float sumH2O_1 = 0;//采用检化验检测水分计算的混合料初始含水量
        //            float sumH2O_2 = 0;//采用人工设定水分计算的混合料初始含水量
        //            float sumAsH = 0;
        //            float sumVOL = 0;
        //            float sumTiO2 = 0;
        //            float sumK2O = 0;
        //            float sumNa2O = 0;
        //            float sumPbO = 0;
        //            float sumZnO = 0;
        //            float sumF = 0;
        //            float sumAs = 0;
        //            float sumCu = 0;
        //            float sumPb = 0;
        //            float sumZn = 0;
        //            float sumK = 0;
        //            float sumNa = 0;
        //            float sumCr = 0;
        //            float sumNi = 0;
        //            float sumMnO = 0;

        //            float sumMix_Wet_SP = 0;     //设定总湿料量
        //            float sumMix_Dry_SP = 0;     //设定总干料量
        //            float sumRemnant_SP = 0;     //设定总残存量
        //            List<float> sinterAly = new List<float> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//TFe,FeO,CaO,SiO2,Al2O3,MgO,S,P,Mn,R,TiO2,K2O,Na2O,PbO,ZnO,F,As,Cu,Pb,Zn,K,Na,Cr,Ni,备用,备用,备用,备用,备用,备用
        //            float sumOther_SiO2_SP = 0;  //除混匀矿和直供料外，其他原料带入的SiO2量
        //            float sumOther_C_SP = 0;     //非燃料总含碳量
        //            float sumFuel_C_SP = 0;      //燃料带入的总固定碳量	
        //            float sumOutput_SP = 0;      //设定下料量计算的理论产量(单位：t/h)
        //            float sumFuel_SP = 0;        //燃料总设定干下料量
        //            float FuelCon_SP = 0;        //设定下料量计算的理论燃耗

        //            float m_SP = 2.1f;            //C与FeO对应比例系数，需要现场数据分析进行修改
        //            //数据准备  (0.非溶剂或燃料 1.溶剂  2.燃料)
        //            //key:0 非燃料   1 燃料
        //            //val:仓号，下料口，设定下料量，当前水分，启停状态，物料成分，二级物料编码
        //            //START

        //            string sql = "select tt.category,p.MAT_L2_CH,p.MAT_L2_XLK,q.MAT_L2_SFDQ,b.L2_CODE,isnull(b.C_TFE,0) cf1,isnull(b.C_FEO,0) cf2,isnull(b.C_CAO,0) cf3,isnull(b.C_SIO2,0) cf4,isnull(b.C_AL2O3, 0) cf5,isnull(b.C_MGO, 0) cf6,isnull(b.C_S, 0) cf7,isnull(b.C_P, 0) cf8,isnull(b.C_C, 0) cf9,"
        //+ "isnull(b.C_MN, 0) cf10,isnull(b.C_LOT, 0) cf11,isnull(b.C_R, 0) cf12,isnull(b.C_H2O, 0) cf13,isnull(b.C_ASH, 0) cf14,isnull(b.C_VOLATILES, 0) cf15,isnull(b.C_TIO2, 0) cf16,isnull(b.C_K2O, 0) cf17,isnull(b.C_NA2O, 0) cf18,"
        //+ "isnull(b.C_PBO, 0) cf19,isnull(b.C_ZNO, 0) cf20,isnull(b.C_F, 0) cf21,isnull(b.C_As, 0) cf22,isnull(b.C_Cu, 0) cf23,"
        //+ "isnull(b.C_Pb, 0) cf24,isnull(b.C_Zn, 0) cf25,isnull(b.C_K, 0) cf26,isnull(b.C_Na, 0) cf27,isnull(b.C_Cr, 0) cf28,"
        //+ "isnull(b.C_Ni, 0) cf29,isnull(b.C_MnO, 0) cf30 from dbo.CFG_MAT_L2_XLK_INTERFACE p left join "
        //+ " dbo.CFG_MAT_L2_PBSD_INTERFACE tt on tt.canghao = p.MAT_L2_CH  left join "
        //+ " dbo.CFG_MAT_L2_SJPB_INTERFACE q on tt.canghao = q.MAT_L2_CH left join"
        //+ " dbo.M_MATERIAL_BINS b on b.BIN_NUM_SHOW = q.MAT_L2_CH order by p.MAT_L2_XLK";//类别，仓号，下料口，当前水分，成分
        //            DataTable _dt = _mdb.GetCommand(sql);

        //            string sql_1 = "select isnull(MAT_PLC_SP_W_1,0),isnull(MAT_PLC_SP_W_2, 0),isnull(MAT_PLC_SP_W_3, 0),"
        //+ "isnull(MAT_PLC_SP_W_4, 0),isnull(MAT_PLC_SP_W_5, 0),isnull(MAT_PLC_SP_W_6, 0),isnull(MAT_PLC_SP_W_7, 0),"
        //+ "isnull(MAT_PLC_SP_W_8, 0),isnull(MAT_PLC_SP_W_9, 0),isnull(MAT_PLC_SP_W_10, 0),isnull(MAT_PLC_SP_W_11, 0),"
        //+ "isnull(MAT_PLC_SP_W_12, 0),isnull(MAT_PLC_SP_W_13, 0),isnull(MAT_PLC_SP_W_14, 0),isnull(MAT_PLC_SP_W_15, 0),"
        //+ "isnull(MAT_PLC_SP_W_16, 0),isnull(MAT_PLC_SP_W_17, 0),isnull(MAT_PLC_SP_W_18, 0),isnull(MAT_PLC_SP_W_19, 0),"
        //+ "isnull(MAT_PLC_SS_SIGNAL_1, 0),isnull(MAT_PLC_SS_SIGNAL_2, 0),isnull(MAT_PLC_SS_SIGNAL_3, 0),isnull(MAT_PLC_SS_SIGNAL_4, 0),"
        //+ "isnull(MAT_PLC_SS_SIGNAL_5, 0),isnull(MAT_PLC_SS_SIGNAL_6, 0),isnull(MAT_PLC_SS_SIGNAL_7, 0),isnull(MAT_PLC_SS_SIGNAL_8, 0),"
        //+ "isnull(MAT_PLC_SS_SIGNAL_9, 0),isnull(MAT_PLC_SS_SIGNAL_10, 0),isnull(MAT_PLC_SS_SIGNAL_11, 0),isnull(MAT_PLC_SS_SIGNAL_12, 0),"
        //+ "isnull(MAT_PLC_SS_SIGNAL_13, 0),isnull(MAT_PLC_SS_SIGNAL_14, 0),isnull(MAT_PLC_SS_SIGNAL_15, 0),"
        //+ "isnull(MAT_PLC_SS_SIGNAL_16, 0),isnull(MAT_PLC_SS_SIGNAL_17, 0),isnull(MAT_PLC_SS_SIGNAL_18, 0),isnull(MAT_PLC_SS_SIGNAL_19, 0)"
        //+ "from dbo.C_MAT_PLC_1MIN p order by p.TIMESTAMP desc";
        //            //设定下料量，启停信号
        //            DataTable _dts = _mdb.GetCommand(sql_1);
        //            #region 20200806 LT更换设定下料量数据来源 ，因为下发程序未投入，现场的设定下料量和页面的设定下料量不一致
        //            // var _vdts = _dts.AsEnumerable();
        //            //if (_vdts.Count() <= 0)
        //            //{
        //            //    Console.WriteLine("C_MAT_PLC_1MIN中没有数据");
        //            //    return new Tuple<int, List<float>>(-1, null);//20200416

        //            //}
        //            //List<Tuple<float, int>> spSS = new List<Tuple<float, int>>();//index:下料口号 item1:设定下料量，item2：下料口启停信号
        //            //foreach (var x in _vdts)
        //            //{
        //            //    for (int i = 0; i < x.ItemArray.Count() / 2; i++)
        //            //    {
        //            //        float sp = float.Parse(x[i].ToString());
        //            //        int ss = int.Parse(x[i + 19].ToString());
        //            //        Tuple<float, int> _temp = new Tuple<float, int>(sp, ss);
        //            //        spSS.Add(_temp);
        //            //    }
        //            //    break;
        //            //}
        //            #endregion
        //            ///*******20200806*****设定下料量更换数据源，测试期间未下发数据导致设定下料量有误
        //            var sql_CFG_MAT_L2_XLK_INTERFACE = "SELECT   MAT_L2_SDXL FROM [NBSJ].[dbo].[CFG_MAT_L2_XLK_INTERFACE] order by MAT_L2_XLK asc";
        //            DataTable data_CFG_MAT_L2_XLK_INTERFACE = _mdb.GetCommand(sql_CFG_MAT_L2_XLK_INTERFACE);
        //            var _vdts = _dts.AsEnumerable();
        //            if (_vdts.Count() <= 0)
        //            {
        //                Console.WriteLine("C_MAT_PLC_1MIN中没有数据");
        //                return new Tuple<int, List<float>>(-1, null);//20200416

        //            }
        //            List<Tuple<float, int>> spSS = new List<Tuple<float, int>>();//index:下料口号 item1:设定下料量，item2：下料口启停信号
        //            foreach (var x in _vdts)
        //            {
        //                for (int i = 0; i < x.ItemArray.Count() / 2; i++)
        //                {
        //                    ////20200806
        //                    // float sp = float.Parse(x[i].ToString());
        //                    float sp = float.Parse(data_CFG_MAT_L2_XLK_INTERFACE.Rows[i][0].ToString());
        //                    int ss = int.Parse(x[i + 19].ToString());
        //                    Tuple<float, int> _temp = new Tuple<float, int>(sp, ss);
        //                    spSS.Add(_temp);
        //                }
        //                break;
        //            }

        //            var _vdt = _dt.AsEnumerable();
        //            if (_vdt.Count() <= 0)
        //            {
        //                Console.WriteLine("CFG_MAT_L2_XLK_INTERFACE中数据异常");
        //                return new Tuple<int, List<float>>(-1, null);//20200416

        //            }
        //            Dictionary<int, List<Tuple<int, int, float, float, int, List<float>, int>>> rsl = new Dictionary<int, List<Tuple<int, int, float, float, int, List<float>, int>>>();

        //            foreach (var x in _vdt)
        //            {
        //                int _p0 = int.Parse(x[0].ToString()) == 2 ? 1 : 0; //类别

        //                bool iscontain = rsl.ContainsKey(_p0);
        //                if (iscontain)
        //                {
        //                    int _p1 = int.Parse(x[1].ToString());//仓号
        //                    int _p2 = int.Parse(x[2].ToString());//下料口
        //                    float _p3 = float.Parse(x[3].ToString());//当前水分
        //                    int _p4 = int.Parse(x[4].ToString());//二级物料编码
        //                    List<float> _p5 = new List<float>();
        //                    for (int i = 5; i < x.ItemArray.Count(); i++)
        //                    {
        //                        _p5.Add(float.Parse(x[i].ToString()));//成分
        //                    }

        //                    Tuple<int, int, float, float, int, List<float>, int> _tup = new Tuple<int, int, float, float, int, List<float>, int>(_p1, _p2, spSS[_p2 - 1].Item1, _p3, spSS[_p2 - 1].Item2, _p5, _p4);

        //                    rsl[_p0].Add(_tup);

        //                }
        //                else
        //                {
        //                    int _p1 = int.Parse(x[1].ToString());//仓号
        //                    int _p2 = int.Parse(x[2].ToString());//下料口
        //                    float _p3 = float.Parse(x[3].ToString());//当前水分
        //                    int _p4 = int.Parse(x[4].ToString());//二级物料编码
        //                    List<float> _p5 = new List<float>();
        //                    for (int i = 5; i < x.ItemArray.Count(); i++)
        //                    {
        //                        _p5.Add(float.Parse(x[i].ToString()));//成分
        //                    }

        //                    Tuple<int, int, float, float, int, List<float>, int> _tup = new Tuple<int, int, float, float, int, List<float>, int>(_p1, _p2, spSS[_p2 - 1].Item1, _p3, spSS[_p2 - 1].Item2, _p5, _p4);

        //                    List<Tuple<int, int, float, float, int, List<float>, int>> _ll = new List<Tuple<int, int, float, float, int, List<float>, int>>();
        //                    _ll.Add(_tup);
        //                    rsl.Add(_p0, _ll);
        //                }
        //            }


        //            //END
        //            //仓号，设定下料量，水分，启停状态
        //            List<Tuple<int, float, float, int>> _sf = new List<Tuple<int, float, float, int>>();
        //            for (int i = 0; i < 2; i++)
        //            {
        //                if (i == 0)//非燃料下料口
        //                {
        //                    /*    
        //                    输入项：

        //                           Silo[i].flow_sp      第i下料口的设定下料量：C_MAT_PLC_1MIN表，MAT_PLC_SP_W_1 - MAT_PLC_SP_W_19字段，对应的第i个下料口的设定下料量；
        //			       x.Item4      第i下料口的当前水分：CFG_MAT_L2_SJPB_INTERFACE表，MAT_L2_SFDQ字段，对应的第i下料口的当前水分值
        //			       x.Item5          第i下料口的启停状态：C_MAT_PLC_1MIN表，MAT_PLC_SS_SIGNAL_1 - MAT_PLC_SS_SIGNAL_19字段，对应第i下料口的启停状态；
        //				   Silo[i].aly[0]       第i下料口的物料a成分的含量： 根据CFG_MAT_L2_XLK_INTERFACE表，确定该下料口与仓号的对应关系，然后联合查询M_MATERIAL_BINS表中对应仓的物料a成分信息与给每个下料口进行匹配。
        //				   成分对应关系
        //                           //  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19,  20, 21, 22, 23, 24, 25, 26, 27, 28,  29
        //                           // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO,  ZnO, F, As, Cu, Pb, Zn, K,  Na, Cr, Ni, MnO  //宁钢检化验成分排序
        //                          //	                                            烧损         灰分 挥发分      

        //                    输出项：
        //                           sinterReport[i]      预测设定成分，保存到表MC_MIXCAL_RESULT_1MIN，对应字段(字段描述与数据库一致)
        //                    */
        //                    foreach (var x in rsl[i])
        //                    {
        //                        int canghao = x.Item2;
        //                        if (canghao == 17 || canghao == 18 || canghao == 19)
        //                        {
        //                            Tuple<int, float, float, int> _sftup = new Tuple<int, float, float, int>(canghao, x.Item3, x.Item4, x.Item5);
        //                            _sf.Add(_sftup);
        //                        }

        //                        sumTFe += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[0] / 100;
        //                        sumFeO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[1] / 100;
        //                        sumCaO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[2] / 100;
        //                        sumSiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[3] / 100;
        //                        sumAl2O3 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[4] / 100;
        //                        sumMgO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[5] / 100;
        //                        sumS += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[6] / 100;
        //                        sumP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[7] / 100;
        //                        sumC += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;   //
        //                        sumMn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[9] / 100;
        //                        sumLOT += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[10] / 100;   //
        //                        sumAsH += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[13] / 100;   //
        //                        sumVOL += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[14] / 100;   //
        //                        sumTiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[15] / 100;   //
        //                        sumK2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[16] / 100;   //
        //                        sumNa2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[17] / 100;   //
        //                        sumPbO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[18] / 100;   //
        //                        sumZnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[19] / 100;   //
        //                        sumF += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[20] / 100;   //
        //                        sumAs += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[21] / 100;   //
        //                        sumCu += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[22] / 100;   //
        //                        sumPb += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[23] / 100;   //
        //                        sumZn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[24] / 100;   //
        //                        sumK += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[25] / 100;   //
        //                        sumNa += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[26] / 100;   //
        //                        sumCr += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[27] / 100;   //
        //                        sumNi += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[28] / 100;   //
        //                        sumMnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[29] / 100;   //

        //                        sumH2O_1 += x.Item3 * x.Item5 * x.Item6[12] / 100;
        //                        sumH2O_2 += x.Item3 * x.Item5 * x.Item4 / 100;

        //                        sumOther_C_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;
        //                        sumRemnant_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * (1 - x.Item6[10] / 100);//总残存量

        //                        //
        //                        sumMix_Dry_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5;//设定总干料量
        //                        sumMix_Wet_SP += x.Item3 * x.Item5;
        //                        //sumDryBill +=Silo[i].bill_use * x.Item5;

        //                        if (x.Item7 != L2Code[0])//确保不是混匀矿
        //                        {
        //                            if (i == 1)//为燃料仓
        //                            {
        //                                sumOther_SiO2_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item6[3] / 100 * x.Item6[13] / 100;
        //                            }
        //                            else
        //                            {
        //                                sumOther_SiO2_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item6[3] / 100;
        //                            }
        //                        }
        //                    }



        //                }
        //                else //燃料下料口
        //                {

        //                    //val:仓号，下料口，设定下料量，当前水分，启停状态，物料成分，二级物料编码
        //                    foreach (var x in rsl[1])
        //                    {
        //                        int canghao = x.Item2;
        //                        if (canghao == 17 || canghao == 18 || canghao == 19)
        //                        {
        //                            Tuple<int, float, float, int> _sftup = new Tuple<int, float, float, int>(canghao, x.Item3, x.Item4, x.Item5);
        //                            _sf.Add(_sftup);
        //                        }

        //                        sumTFe += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[0] / 100 * x.Item6[13] / 100;
        //                        sumFeO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[1] / 100 * x.Item6[13] / 100;
        //                        sumCaO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[2] / 100 * x.Item6[13] / 100;
        //                        sumSiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[3] / 100 * x.Item6[13] / 100;
        //                        sumAl2O3 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[4] / 100 * x.Item6[13] / 100;
        //                        sumMgO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[5] / 100 * x.Item6[13] / 100;
        //                        sumS += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[6] / 100 * x.Item6[13] / 100;
        //                        sumP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[7] / 100 * x.Item6[13] / 100;
        //                        sumMn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[9] / 100 * x.Item6[13] / 100;
        //                        sumTiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[15] / 100 * x.Item6[13] / 100;
        //                        sumK2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[16] / 100 * x.Item6[13] / 100;
        //                        sumNa2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[17] / 100 * x.Item6[13] / 100;
        //                        sumPbO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[18] / 100 * x.Item6[13] / 100;
        //                        sumZnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[19] / 100 * x.Item6[13] / 100;
        //                        sumF += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[20] / 100 * x.Item6[13] / 100;
        //                        sumAs += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[21] / 100 * x.Item6[13] / 100;
        //                        sumCu += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[22] / 100 * x.Item6[13] / 100;
        //                        sumPb += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[23] / 100 * x.Item6[13] / 100;
        //                        sumZn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[24] / 100 * x.Item6[13] / 100;
        //                        sumK += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[25] / 100 * x.Item6[13] / 100;
        //                        sumNa += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[26] / 100 * x.Item6[13] / 100;
        //                        sumCr += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[27] / 100 * x.Item6[13] / 100;
        //                        sumNi += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[28] / 100 * x.Item6[13] / 100;
        //                        sumMnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[29] / 100 * x.Item6[13] / 100;

        //                        sumH2O_1 += x.Item3 * x.Item5 / (1 - x.Item6[12] / 100) * x.Item6[12] / 100;
        //                        sumH2O_2 += x.Item3 * x.Item5 / (1 - x.Item4 / 100) * x.Item4 / 100;

        //                        sumC += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;
        //                        sumLOT += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * (1 - x.Item6[13] / 100);
        //                        sumAsH += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[13] / 100;
        //                        sumVOL += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[14] / 100;

        //                        sumFuel_C_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;//燃料带入的C量
        //                        sumRemnant_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[13] / 100;//如果煤粉检化验有“灰分”，总烧损计算用该公式
        //                                                                                                     //sumRemnant_SP += x.Item3 * (1-x.Item4/100) * x.Item5 * (1 - x.Item6[10]/100);//如果煤粉检化验有“烧损”，总烧损计算用该公式
        //                        sumFuel_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5;

        //                        sumMix_Dry_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5;//设定总干料量
        //                        sumMix_Wet_SP += x.Item3 * x.Item5;
        //                        //sumDryBill +=Silo[i].bill_use * x.Item5;

        //                        if (x.Item7 != L2Code[0])//确保不是混匀矿
        //                        {
        //                            if (i == 1)//为燃料仓
        //                            {
        //                                sumOther_SiO2_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item6[3] / 100 * x.Item6[13] / 100;
        //                            }
        //                            else
        //                            {
        //                                sumOther_SiO2_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item6[3] / 100;
        //                            }
        //                        }
        //                    }

        //                }

        //            }

        //            float sf_cch = 0f;//烧返和除尘灰的干料量
        //            //仓号，设定下料量，水分，启停状态
        //            foreach (var y in _sf)
        //            {
        //                sf_cch += y.Item2 * (1 - y.Item3 / 100) * y.Item4;
        //            }
        //            sumOutput_SP = sumRemnant_SP - sf_cch;//总残存 - 烧返和除尘灰的干料量
        //                                                  //sumOutput_SP = sumRemnant_SP - Silo[7].flow_sp * (1 - Silo[7].h2o_use/100) * Silo[7].use - Silo[8].flow_sp * (1 - Silo[8].h2o_use/100) * Silo[8].use - Silo[13].flow_sp * (1 - Silo[13].h2o_use/100) * Silo[13].use - Silo[14].flow_sp * (1 - Silo[14].h2o_use/100) * Silo[14].use;
        //            FuelCon_SP = sumFuel_SP / sumOutput_SP * 1000;

        //            if (sumRemnant_SP > 0)
        //            {
        //                sinterAly[0] = sumTFe / sumRemnant_SP * 100;                        //烧结矿--SP_TFE
        //                sinterAly[1] = (m_SP * sumC / sumMix_Dry_SP * 100) / sumRemnant_SP * 100;//烧结矿--SP_FeO
        //                sinterAly[2] = sumCaO / sumRemnant_SP * 100;                        //烧结矿--SP_CaO
        //                sinterAly[3] = sumSiO2 / sumRemnant_SP * 100;                       //烧结矿--SP_SiO2
        //                sinterAly[4] = sumAl2O3 / sumRemnant_SP * 100;                      //烧结矿--SP_Al2O3
        //                sinterAly[5] = sumMgO / sumRemnant_SP * 100;                        //烧结矿--SP_MgO
        //                sinterAly[6] = sumS / sumRemnant_SP * 100;                          //烧结矿--SP_S
        //                sinterAly[7] = sumP / sumRemnant_SP * 100;                          //烧结矿--SP_P
        //                sinterAly[8] = sumMn / sumRemnant_SP * 100;                         //烧结矿--SP_Mn

        //                if (sinterAly[3] > 0)
        //                {
        //                    sinterAly[9] = sinterAly[2] / sinterAly[3];              //烧结矿--SP_R
        //                }

        //                sinterAly[10] = sumTiO2 / sumRemnant_SP * 100;                      //烧结矿--SP_TiO2
        //                sinterAly[11] = sumK2O / sumRemnant_SP * 100;                      //烧结矿--SP_K2O
        //                sinterAly[12] = sumNa2O / sumRemnant_SP * 100;                     //烧结矿--SP_Na2O
        //                sinterAly[13] = sumPbO / sumRemnant_SP * 100;                      //烧结矿--SP_PbO
        //                sinterAly[14] = sumZnO / sumRemnant_SP * 100;                      //烧结矿--SP_ZnO
        //                sinterAly[15] = sumF / sumRemnant_SP * 100;                        //烧结矿--SP_F
        //                sinterAly[16] = sumAs / sumRemnant_SP * 100;                       //烧结矿--SP_As
        //                sinterAly[17] = sumCu / sumRemnant_SP * 100;                       //烧结矿--SP_Cu
        //                sinterAly[18] = sumPb / sumRemnant_SP * 100;                       //烧结矿--SP_Pb
        //                sinterAly[19] = sumZn / sumRemnant_SP * 100;                       //烧结矿--SP_Zn
        //                sinterAly[20] = sumK / sumRemnant_SP * 100;                        //烧结矿--SP_K
        //                sinterAly[21] = sumNa / sumRemnant_SP * 100;                       //烧结矿--SP_Na
        //                sinterAly[22] = sumCr / sumRemnant_SP * 100;                       //烧结矿--SP_Cr
        //                sinterAly[23] = sumNi / sumRemnant_SP * 100;                       //烧结矿--SP_Ni
        //                sinterAly[24] = sumMnO / sumRemnant_SP * 100;                       //烧结矿--SP_MNO

        //                Tuple<int, List<float>> _ret = new Tuple<int, List<float>>(0, new List<float>());

        //                for (int i = 0; i < 30; i++)
        //                {
        //                    _ret.Item2.Add(sinterAly[i]);//用于插入配料1Min结果表
        //                }

        //                _ret.Item2.Add(sumLOT / sumMix_Dry_SP * 100);           //混合料--设定下料量计算混合料综合烧损
        //                _ret.Item2.Add(sumH2O_1 / sumMix_Wet_SP);               //混合料--采集水分计算的混合料原始水分含量
        //                _ret.Item2.Add(sumH2O_2 / sumMix_Wet_SP);               //混合料--设定水分计算的混合料原始水分含量
        //                _ret.Item2.Add(sumFeO / sumRemnant_SP * 100);           //混合料--设定下料量计算混合料FeO含量
        //                _ret.Item2.Add(sumC / sumMix_Dry_SP * 100);             //混合料--设定下料量计算混合料固定碳
        //                _ret.Item2.Add(sumOther_C_SP / sumMix_Dry_SP * 100);    //混合料--设定下料量计算混合料中非燃料带入的固定碳
        //                _ret.Item2.Add(sumFuel_C_SP / sumMix_Dry_SP * 100);    //混合料--设定下料量计算混合料中燃料带入的固定碳
        //                _ret.Item2.Add(sumOther_SiO2_SP / sumMix_Dry_SP * 100); //混合料--设定下料量计算混合料中非铁料带入的SIO2含量
        //                _ret.Item2.Add(sumMix_Dry_SP);                          //设定下料量计算总干料量
        //                _ret.Item2.Add(sumOutput_SP / 60);                      //设定下料量计算的每分钟理论产量(单位：t/min)
        //                _ret.Item2.Add(FuelCon_SP);                             //设定下料量计算的理论燃耗(干基)（单位：kg/t）

        //                return _ret;
        //            }
        //            else
        //            {
        //                Console.WriteLine("总残存sumRemnant_SP<=0,错误！，需要检查现场设定配比是否正确输入");
        //            }
        //            Tuple<int, List<float>> _rets = new Tuple<int, List<float>>(-8014, new List<float>());
        //            return _rets; //基本数据错误,烧结矿配比产量 <= 0
        //        }
        public Tuple<int, List<float>> CalculateSinterBySP()//该函数是计算烧结矿设定成分 20200911 李涛更新
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19,  20, 21, 22, 23, 24, 25, 26, 27, 28,  29
            // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO,  ZnO, F, As, Cu, Pb, Zn, K,  Na, Cr, Ni, MnO  //宁钢检化验成分排序
            //	                                             烧损         灰分 挥发分                            


            ////20200809  lt    m_SP参数换成数据库数据源[MC_MIXCAL_PAR]表PAR_K_FeO字段

            var sql_MC_MIXCAL_PAR = "SELECT PAR_K_FeO  FROM MC_MIXCAL_PAR where  TIMESTAMP = (SELECT MAX(TIMESTAMP) from [MC_MIXCAL_PAR] )";
            DataTable data = _mdb.GetCommand(sql_MC_MIXCAL_PAR);


            var _vdts_1 = data.AsEnumerable();
            if (_vdts_1.Count() <= 0)
            {
                Console.WriteLine("MC_MIXCAL_PAR中没有数据");
                return new Tuple<int, List<float>>(-1, null);//20200416

            }

            float sumTFe = 0;
            float sumFeO = 0;
            float sumCaO = 0;
            float sumSiO2 = 0;
            float sumAl2O3 = 0;
            float sumMgO = 0;
            float sumS = 0;
            float sumP = 0;
            float sumC = 0;
            float sumMn = 0;
            float sumLOT = 0;//总烧损
            float sumR = 0;
            float sumH2O_1 = 0;//采用检化验检测水分计算的混合料初始含水量
            float sumH2O_2 = 0;//采用人工设定水分计算的混合料初始含水量
            float sumAsH = 0;
            float sumVOL = 0;
            float sumTiO2 = 0;
            float sumK2O = 0;
            float sumNa2O = 0;
            float sumPbO = 0;
            float sumZnO = 0;
            float sumF = 0;
            float sumAs = 0;
            float sumCu = 0;
            float sumPb = 0;
            float sumZn = 0;
            float sumK = 0;
            float sumNa = 0;
            float sumCr = 0;
            float sumNi = 0;
            float sumMnO = 0;

            float sumMix_Wet_SP = 0;     //设定总湿料量
            float sumMix_Dry_SP = 0;     //设定总干料量
            float sumRemnant_SP = 0;     //设定总残存量
            List<float> sinterAly = new List<float> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//TFe,FeO,CaO,SiO2,Al2O3,MgO,S,P,Mn,R,TiO2,K2O,Na2O,PbO,ZnO,F,As,Cu,Pb,Zn,K,Na,Cr,Ni,备用,备用,备用,备用,备用,备用
            float sumOther_SiO2_SP = 0;  //除混匀矿和直供料外，其他原料带入的SiO2量
            float sumOther_C_SP = 0;     //非燃料总含碳量
            float sumFuel_C_SP = 0;      //燃料带入的总固定碳量	
            float sumOutput_SP = 0;      //设定下料量计算的理论产量(单位：t/h)
            float sumFuel_SP = 0;        //燃料总设定干下料量
            float FuelCon_SP = 0;        //设定下料量计算的理论燃耗
                                         //20200808
                                         //20200808 
                                         //  float m_SP = 3.02f;            //C与FeO对应比例系数，需要现场数据分析进行修改
                                         //20200809  换为数据库配料参数表字段
            float m_SP = float.Parse(data.Rows[0][0].ToString());            //C与FeO对应比例系数，需要现场数据分析进行修改
                                                                             //    float m_SP = 2.1f;            //C与FeO对应比例系数，需要现场数据分析进行修改
                                                                             //数据准备  (0.非溶剂或燃料 1.溶剂  2.燃料)
                                                                             //key:0 非燃料   1 燃料
                                                                             //val:仓号，下料口，设定下料量，当前水分，启停状态，物料成分，二级物料编码
                                                                             //START

            string sql = "select tt.category,p.MAT_L2_CH,p.MAT_L2_XLK,q.MAT_L2_SFDQ,b.L2_CODE,isnull(b.C_TFE,0) cf1,isnull(b.C_FEO,0) cf2,isnull(b.C_CAO,0) cf3,isnull(b.C_SIO2,0) cf4,isnull(b.C_AL2O3, 0) cf5,isnull(b.C_MGO, 0) cf6,isnull(b.C_S, 0) cf7,isnull(b.C_P, 0) cf8,isnull(b.C_C, 0) cf9,"
+ "isnull(b.C_MN, 0) cf10,isnull(b.C_LOT, 0) cf11,isnull(b.C_R, 0) cf12,isnull(b.C_H2O, 0) cf13,isnull(b.C_ASH, 0) cf14,isnull(b.C_VOLATILES, 0) cf15,isnull(b.C_TIO2, 0) cf16,isnull(b.C_K2O, 0) cf17,isnull(b.C_NA2O, 0) cf18,"
+ "isnull(b.C_PBO, 0) cf19,isnull(b.C_ZNO, 0) cf20,isnull(b.C_F, 0) cf21,isnull(b.C_As, 0) cf22,isnull(b.C_Cu, 0) cf23,"
+ "isnull(b.C_Pb, 0) cf24,isnull(b.C_Zn, 0) cf25,isnull(b.C_K, 0) cf26,isnull(b.C_Na, 0) cf27,isnull(b.C_Cr, 0) cf28,"
+ "isnull(b.C_Ni, 0) cf29,isnull(b.C_MnO, 0) cf30 from dbo.CFG_MAT_L2_XLK_INTERFACE p left join "
+ " dbo.CFG_MAT_L2_PBSD_INTERFACE tt on tt.canghao = p.MAT_L2_CH  left join "
+ " dbo.CFG_MAT_L2_SJPB_INTERFACE q on tt.canghao = q.MAT_L2_CH left join"
+ " dbo.M_MATERIAL_BINS b on b.BIN_NUM_SHOW = q.MAT_L2_CH order by p.MAT_L2_XLK";//类别，仓号，下料口，当前水分，成分
            DataTable _dt = _mdb.GetCommand(sql);

            string sql_1 = "select isnull(MAT_PLC_SP_W_1,0),isnull(MAT_PLC_SP_W_2, 0),isnull(MAT_PLC_SP_W_3, 0),"
+ "isnull(MAT_PLC_SP_W_4, 0),isnull(MAT_PLC_SP_W_5, 0),isnull(MAT_PLC_SP_W_6, 0),isnull(MAT_PLC_SP_W_7, 0),"
+ "isnull(MAT_PLC_SP_W_8, 0),isnull(MAT_PLC_SP_W_9, 0),isnull(MAT_PLC_SP_W_10, 0),isnull(MAT_PLC_SP_W_11, 0),"
+ "isnull(MAT_PLC_SP_W_12, 0),isnull(MAT_PLC_SP_W_13, 0),isnull(MAT_PLC_SP_W_14, 0),isnull(MAT_PLC_SP_W_15, 0),"
+ "isnull(MAT_PLC_SP_W_16, 0),isnull(MAT_PLC_SP_W_17, 0),isnull(MAT_PLC_SP_W_18, 0),isnull(MAT_PLC_SP_W_19, 0),isnull(MAT_PLC_SP_W_20, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_1, 0),isnull(MAT_PLC_SS_SIGNAL_2, 0),isnull(MAT_PLC_SS_SIGNAL_3, 0),isnull(MAT_PLC_SS_SIGNAL_4, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_5, 0),isnull(MAT_PLC_SS_SIGNAL_6, 0),isnull(MAT_PLC_SS_SIGNAL_7, 0),isnull(MAT_PLC_SS_SIGNAL_8, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_9, 0),isnull(MAT_PLC_SS_SIGNAL_10, 0),isnull(MAT_PLC_SS_SIGNAL_11, 0),isnull(MAT_PLC_SS_SIGNAL_12, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_13, 0),isnull(MAT_PLC_SS_SIGNAL_14, 0),isnull(MAT_PLC_SS_SIGNAL_15, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_16, 0),isnull(MAT_PLC_SS_SIGNAL_17, 0),isnull(MAT_PLC_SS_SIGNAL_18, 0),isnull(MAT_PLC_SS_SIGNAL_19, 0),isnull(MAT_PLC_SS_SIGNAL_20, 0)"
+ "from dbo.C_MAT_PLC_1MIN p order by p.TIMESTAMP desc";
            //设定下料量，启停信号
            DataTable _dts = _mdb.GetCommand(sql_1);
            #region 20200806 LT更换设定下料量数据来源 ，因为下发程序未投入，现场的设定下料量和页面的设定下料量不一致
            // var _vdts = _dts.AsEnumerable();
            //if (_vdts.Count() <= 0)
            //{
            //    Console.WriteLine("C_MAT_PLC_1MIN中没有数据");
            //    return new Tuple<int, List<float>>(-1, null);//20200416

            //}
            //List<Tuple<float, int>> spSS = new List<Tuple<float, int>>();//index:下料口号 item1:设定下料量，item2：下料口启停信号
            //foreach (var x in _vdts)
            //{
            //    for (int i = 0; i < x.ItemArray.Count() / 2; i++)
            //    {
            //        float sp = float.Parse(x[i].ToString());
            //        int ss = int.Parse(x[i + 19].ToString());
            //        Tuple<float, int> _temp = new Tuple<float, int>(sp, ss);
            //        spSS.Add(_temp);
            //    }
            //    break;
            //}
            #endregion
            ///*******20200806*****设定下料量更换数据源，测试期间未下发数据导致设定下料量有误
            var sql_CFG_MAT_L2_XLK_INTERFACE = "SELECT   MAT_L2_SDXL,MAT_L2_XLKZT FROM CFG_MAT_L2_XLK_INTERFACE order by MAT_L2_XLK asc";
            DataTable data_CFG_MAT_L2_XLK_INTERFACE = _mdb.GetCommand(sql_CFG_MAT_L2_XLK_INTERFACE);
            var _vdts = _dts.AsEnumerable();
            if (_vdts.Count() <= 0)
            {
                Console.WriteLine("C_MAT_PLC_1MIN中没有数据");
                return new Tuple<int, List<float>>(-1, null);//20200416

            }
            List<Tuple<float, int>> spSS = new List<Tuple<float, int>>();//index:下料口号 item1:设定下料量，item2：下料口启停信号
            foreach (var x in _vdts)
            {
                for (int i = 0; i < x.ItemArray.Count() / 2; i++)
                {
                    ////20200806
                    // float sp = float.Parse(x[i].ToString());
                    float sp = float.Parse(data_CFG_MAT_L2_XLK_INTERFACE.Rows[i][0].ToString());
                    //20200808  lt 修改起停信号数据来源
                    int ss = int.Parse(data_CFG_MAT_L2_XLK_INTERFACE.Rows[i][1].ToString());
                    // int ss = int.Parse(x[i + 19].ToString());
                    Tuple<float, int> _temp = new Tuple<float, int>(sp, ss);
                    spSS.Add(_temp);
                }
                break;
            }

            var _vdt = _dt.AsEnumerable();
            if (_vdt.Count() <= 0)
            {
                Console.WriteLine("CFG_MAT_L2_XLK_INTERFACE中数据异常");
                return new Tuple<int, List<float>>(-1, null);//20200416

            }

            Dictionary<int, List<Tuple<int, int, float, float, int, List<float>, int>>> rsl = new Dictionary<int, List<Tuple<int, int, float, float, int, List<float>, int>>>();
            //修改20201209 按照成分区分燃料非燃料
            foreach (var x in _vdt)
            {
                // int _p0 = int.Parse(x[0].ToString()) == 2 ? 1 : 0; //类别
                int _p0 = 0;
                if (float.Parse(x[13].ToString()) > 30 && float.Parse(x[18].ToString()) > 5)
                {
                    _p0 = 1;
                }
                bool iscontain = rsl.ContainsKey(_p0);
                if (iscontain)
                {
                    int _p1 = int.Parse(x[1].ToString());//仓号
                    int _p2 = int.Parse(x[2].ToString());//下料口
                    float _p3 = float.Parse(x[3].ToString());//当前水分
                    int _p4 = int.Parse(x[4].ToString());//二级物料编码
                    List<float> _p5 = new List<float>();
                    for (int i = 5; i < x.ItemArray.Count(); i++)
                    {
                        _p5.Add(float.Parse(x[i].ToString()));//成分
                    }

                    Tuple<int, int, float, float, int, List<float>, int> _tup = new Tuple<int, int, float, float, int, List<float>, int>(_p1, _p2, spSS[_p2 - 1].Item1, _p3, spSS[_p2 - 1].Item2, _p5, _p4);

                    rsl[_p0].Add(_tup);

                }
                else
                {
                    int _p1 = int.Parse(x[1].ToString());//仓号
                    int _p2 = int.Parse(x[2].ToString());//下料口
                    float _p3 = float.Parse(x[3].ToString());//当前水分
                    int _p4 = int.Parse(x[4].ToString());//二级物料编码
                    List<float> _p5 = new List<float>();
                    for (int i = 5; i < x.ItemArray.Count(); i++)
                    {
                        _p5.Add(float.Parse(x[i].ToString()));//成分
                    }

                    Tuple<int, int, float, float, int, List<float>, int> _tup = new Tuple<int, int, float, float, int, List<float>, int>(_p1, _p2, spSS[_p2 - 1].Item1, _p3, spSS[_p2 - 1].Item2, _p5, _p4);

                    List<Tuple<int, int, float, float, int, List<float>, int>> _ll = new List<Tuple<int, int, float, float, int, List<float>, int>>();
                    _ll.Add(_tup);
                    rsl.Add(_p0, _ll);
                }
            }


            //END
            //仓号，设定下料量，水分，启停状态
            List<Tuple<int, float, float, int>> _sf = new List<Tuple<int, float, float, int>>();
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)//非燃料下料口
                {
                    /*    
                    输入项：

                           Silo[i].flow_sp      第i下料口的设定下料量：C_MAT_PLC_1MIN表，MAT_PLC_SP_W_1 - MAT_PLC_SP_W_19字段，对应的第i个下料口的设定下料量；
			       x.Item4      第i下料口的当前水分：CFG_MAT_L2_SJPB_INTERFACE表，MAT_L2_SFDQ字段，对应的第i下料口的当前水分值
			       x.Item5          第i下料口的启停状态：C_MAT_PLC_1MIN表，MAT_PLC_SS_SIGNAL_1 - MAT_PLC_SS_SIGNAL_19字段，对应第i下料口的启停状态；
				   Silo[i].aly[0]       第i下料口的物料a成分的含量： 根据CFG_MAT_L2_XLK_INTERFACE表，确定该下料口与仓号的对应关系，然后联合查询M_MATERIAL_BINS表中对应仓的物料a成分信息与给每个下料口进行匹配。
				   成分对应关系
                           //  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19,  20, 21, 22, 23, 24, 25, 26, 27, 28,  29
                           // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO,  ZnO, F, As, Cu, Pb, Zn, K,  Na, Cr, Ni, MnO  //宁钢检化验成分排序
                          //	                                            烧损         灰分 挥发分      

                    输出项：
                           sinterReport[i]      预测设定成分，保存到表MC_MIXCAL_RESULT_1MIN，对应字段(字段描述与数据库一致)
                    */
                    foreach (var x in rsl[i])
                    {
                        int canghao = x.Item2;
                        //if (canghao == 17 || canghao == 18 || canghao == 19)
                        if (x.Item7 == L2Code[2] || x.Item7 == L2Code[3])
                        {
                            Tuple<int, float, float, int> _sftup = new Tuple<int, float, float, int>(canghao, x.Item3, x.Item4, x.Item5);
                            _sf.Add(_sftup);
                        }

                        sumTFe += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[0] / 100;
                        sumFeO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[1] / 100;
                        sumCaO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[2] / 100;
                        sumSiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[3] / 100;
                        sumAl2O3 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[4] / 100;
                        sumMgO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[5] / 100;
                        sumS += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[6] / 100;
                        sumP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[7] / 100;
                        sumC += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;   //
                        sumMn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[9] / 100;
                        sumLOT += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[10] / 100;   //
                        sumAsH += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[13] / 100;   //
                        sumVOL += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[14] / 100;   //
                        sumTiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[15] / 100;   //
                        sumK2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[16] / 100;   //
                        sumNa2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[17] / 100;   //
                        sumPbO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[18] / 100;   //
                        sumZnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[19] / 100;   //
                        sumF += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[20] / 100;   //
                        sumAs += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[21] / 100;   //
                        sumCu += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[22] / 100;   //
                        sumPb += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[23] / 100;   //
                        sumZn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[24] / 100;   //
                        sumK += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[25] / 100;   //
                        sumNa += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[26] / 100;   //
                        sumCr += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[27] / 100;   //
                        sumNi += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[28] / 100;   //
                        sumMnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[29] / 100;   //

                        sumH2O_1 += x.Item3 * x.Item5 * x.Item6[12] / 100;
                        sumH2O_2 += x.Item3 * x.Item5 * x.Item4 / 100;

                        sumOther_C_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;
                        sumRemnant_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * (1 - x.Item6[10] / 100);//总残存量

                        //
                        sumMix_Dry_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5;//设定总干料量
                        sumMix_Wet_SP += x.Item3 * x.Item5;
                        //sumDryBill +=Silo[i].bill_use * x.Item5;

                        if (x.Item7 > 299)
                        {
                            if (i == 1)//为燃料仓
                            {
                                sumOther_SiO2_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item6[3] / 100 * x.Item6[13] / 100;
                            }
                            else
                            {
                                sumOther_SiO2_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item6[3] / 100;
                            }
                        }
                    }



                }
                else//燃料下料口
                {

                    //val:仓号，下料口，设定下料量，当前水分，启停状态，物料成分，二级物料编码
                    foreach (var x in rsl[1])
                    {
                        int canghao = x.Item2;
                        //if (canghao == 17 || canghao == 18 || canghao == 19)
                        if (x.Item7 == L2Code[2] || x.Item7 == L2Code[3])
                        {
                            Tuple<int, float, float, int> _sftup = new Tuple<int, float, float, int>(canghao, x.Item3, x.Item4, x.Item5);
                            _sf.Add(_sftup);
                        }
                        //sumTFe += 下料量 * (1 -水分 / 100) *是否激活 *成分 / 100 *灰分 / 100;
                        sumTFe += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[0] / 100 * x.Item6[13] / 100;
                        sumFeO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[1] / 100 * x.Item6[13] / 100;
                        sumCaO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[2] / 100 * x.Item6[13] / 100;
                        sumSiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[3] / 100 * x.Item6[13] / 100;
                        sumAl2O3 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[4] / 100 * x.Item6[13] / 100;
                        sumMgO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[5] / 100 * x.Item6[13] / 100;
                        sumS += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[6] / 100 * x.Item6[13] / 100;
                        sumP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[7] / 100 * x.Item6[13] / 100;
                        sumMn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[9] / 100 * x.Item6[13] / 100;
                        sumTiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[15] / 100 * x.Item6[13] / 100;
                        sumK2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[16] / 100 * x.Item6[13] / 100;
                        sumNa2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[17] / 100 * x.Item6[13] / 100;
                        sumPbO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[18] / 100 * x.Item6[13] / 100;
                        sumZnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[19] / 100 * x.Item6[13] / 100;
                        sumF += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[20] / 100 * x.Item6[13] / 100;
                        sumAs += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[21] / 100 * x.Item6[13] / 100;
                        sumCu += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[22] / 100 * x.Item6[13] / 100;
                        sumPb += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[23] / 100 * x.Item6[13] / 100;
                        sumZn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[24] / 100 * x.Item6[13] / 100;
                        sumK += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[25] / 100 * x.Item6[13] / 100;
                        sumNa += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[26] / 100 * x.Item6[13] / 100;
                        sumCr += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[27] / 100 * x.Item6[13] / 100;
                        sumNi += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[28] / 100 * x.Item6[13] / 100;
                        sumMnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[29] / 100 * x.Item6[13] / 100;

                        sumH2O_1 += x.Item3 * x.Item5 / (1 - x.Item6[12] / 100) * x.Item6[12] / 100;
                        sumH2O_2 += x.Item3 * x.Item5 / (1 - x.Item4 / 100) * x.Item4 / 100;

                        sumC += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;
                        sumLOT += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * (1 - x.Item6[13] / 100);
                        sumAsH += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[13] / 100;
                        sumVOL += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[14] / 100;

                        sumFuel_C_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;//燃料带入的C量
                        sumRemnant_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[13] / 100;//如果煤粉检化验有“灰分”，总烧损计算用该公式
                                                                                                     //sumRemnant_SP += x.Item3 * (1-x.Item4/100) * x.Item5 * (1 - x.Item6[10]/100);//如果煤粉检化验有“烧损”，总烧损计算用该公式
                        sumFuel_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5;

                        sumMix_Dry_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item5;//设定总干料量
                        sumMix_Wet_SP += x.Item3 * x.Item5;
                        //sumDryBill +=Silo[i].bill_use * x.Item5;

                        if (x.Item7 > 299)
                        {
                            if (i == 1)//为燃料仓
                            {
                                sumOther_SiO2_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item6[3] / 100 * x.Item6[13] / 100;
                            }
                            else
                            {
                                sumOther_SiO2_SP += x.Item3 * (1 - x.Item4 / 100) * x.Item6[3] / 100;
                            }
                        }
                    }

                }

            }

            float sf_cch = 0f;//烧返和除尘灰的干料量
            //仓号，设定下料量，水分，启停状态
            foreach (var y in _sf)//501或者601的
            {
                sf_cch += y.Item2 * (1 - y.Item3 / 100) * y.Item4;
            }
            sumOutput_SP = sumRemnant_SP - sf_cch;//总残存 - 烧返和除尘灰的干料量
                                                  //sumOutput_SP = sumRemnant_SP - Silo[7].flow_sp * (1 - Silo[7].h2o_use/100) * Silo[7].use - Silo[8].flow_sp * (1 - Silo[8].h2o_use/100) * Silo[8].use - Silo[13].flow_sp * (1 - Silo[13].h2o_use/100) * Silo[13].use - Silo[14].flow_sp * (1 - Silo[14].h2o_use/100) * Silo[14].use;
            FuelCon_SP = sumFuel_SP / sumOutput_SP * 1000;

            if (sumRemnant_SP > 0)
            {
                sinterAly[0] = sumTFe / sumRemnant_SP * 100;                        //烧结矿--SP_TFE
                //20200808 lt 公式问题
                //  sinterAly[1] = (m_SP * sumC / sumMix_Dry_SP * 100) / sumRemnant_SP * 100;//烧结矿--SP_FeO
                sinterAly[1] = (m_SP * sumC / sumMix_Dry_SP * 100);//烧结矿--SP_FeO
                sinterAly[2] = sumCaO / sumRemnant_SP * 100;                        //烧结矿--SP_CaO
                sinterAly[3] = sumSiO2 / sumRemnant_SP * 100;                       //烧结矿--SP_SiO2
                sinterAly[4] = sumAl2O3 / sumRemnant_SP * 100;                      //烧结矿--SP_Al2O3
                sinterAly[5] = sumMgO / sumRemnant_SP * 100;                        //烧结矿--SP_MgO
                sinterAly[6] = sumS / sumRemnant_SP * 100;                          //烧结矿--SP_S
                sinterAly[7] = sumP / sumRemnant_SP * 100;                          //烧结矿--SP_P
                sinterAly[8] = sumMn / sumRemnant_SP * 100;                         //烧结矿--SP_Mn

                if (sinterAly[3] > 0)
                {
                    sinterAly[9] = sinterAly[2] / sinterAly[3];              //烧结矿--SP_R
                }

                sinterAly[10] = sumTiO2 / sumRemnant_SP * 100;                      //烧结矿--SP_TiO2
                sinterAly[11] = sumK2O / sumRemnant_SP * 100;                      //烧结矿--SP_K2O
                sinterAly[12] = sumNa2O / sumRemnant_SP * 100;                     //烧结矿--SP_Na2O
                sinterAly[13] = sumPbO / sumRemnant_SP * 100;                      //烧结矿--SP_PbO
                sinterAly[14] = sumZnO / sumRemnant_SP * 100;                      //烧结矿--SP_ZnO
                sinterAly[15] = sumF / sumRemnant_SP * 100;                        //烧结矿--SP_F
                sinterAly[16] = sumAs / sumRemnant_SP * 100;                       //烧结矿--SP_As
                sinterAly[17] = sumCu / sumRemnant_SP * 100;                       //烧结矿--SP_Cu
                sinterAly[18] = sumPb / sumRemnant_SP * 100;                       //烧结矿--SP_Pb
                sinterAly[19] = sumZn / sumRemnant_SP * 100;                       //烧结矿--SP_Zn
                sinterAly[20] = sumK / sumRemnant_SP * 100;                        //烧结矿--SP_K
                sinterAly[21] = sumNa / sumRemnant_SP * 100;                       //烧结矿--SP_Na
                sinterAly[22] = sumCr / sumRemnant_SP * 100;                       //烧结矿--SP_Cr
                sinterAly[23] = sumNi / sumRemnant_SP * 100;                       //烧结矿--SP_Ni
                sinterAly[24] = sumMnO / sumRemnant_SP * 100;                       //烧结矿--SP_MNO

                Tuple<int, List<float>> _ret = new Tuple<int, List<float>>(0, new List<float>());

                for (int i = 0; i <= 30; i++)
                {
                    _ret.Item2.Add(sinterAly[i]);//用于插入配料1Min结果表
                }

                _ret.Item2.Add(sumLOT / sumMix_Dry_SP * 100);           //混合料--设定下料量计算混合料综合烧损
                _ret.Item2.Add(sumH2O_1 / sumMix_Wet_SP);               //混合料--采集水分计算的混合料原始水分含量
                _ret.Item2.Add(sumH2O_2 / sumMix_Wet_SP);               //混合料--设定水分计算的混合料原始水分含量
                _ret.Item2.Add(sumFeO / sumRemnant_SP * 100);           //混合料--设定下料量计算混合料FeO含量
                _ret.Item2.Add(sumC / sumMix_Dry_SP * 100);             //混合料--设定下料量计算混合料固定碳
                _ret.Item2.Add(sumOther_C_SP / sumMix_Dry_SP * 100);    //混合料--设定下料量计算混合料中非燃料带入的固定碳
                _ret.Item2.Add(sumFuel_C_SP / sumMix_Dry_SP * 100);    //混合料--设定下料量计算混合料中燃料带入的固定碳
                _ret.Item2.Add(sumOther_SiO2_SP / sumMix_Dry_SP * 100); //混合料--设定下料量计算混合料中非铁料带入的SIO2含量
                _ret.Item2.Add(sumMix_Dry_SP);                          //设定下料量计算总干料量
                _ret.Item2.Add(sumOutput_SP / 60);                      //设定下料量计算的每分钟理论产量(单位：t/min)
                _ret.Item2.Add(FuelCon_SP);                             //设定下料量计算的理论燃耗(干基)（单位：kg/t）

                return _ret;
            }
            else
            {
                Console.WriteLine("总残存sumRemnant_SP<=0,错误！，需要检查现场设定配比是否正确输入");
            }
            Tuple<int, List<float>> _rets = new Tuple<int, List<float>>(-8014, new List<float>());
            return _rets; //基本数据错误,烧结矿配比产量 <= 0
        }
        /// <summary>
        /// 计算烧结矿实际成分
        /// </summary>

        /// <returns>
        /// item1:计算结果 0：正常 其他：异常
        /// item2:预测设定成分
        /// </returns>
        public Tuple<int, List<float>> CalculateSinterByPV()//该函数是计算烧结矿实际成分 20200911 李涛更新

        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);

            ////20200809  lt    m_PV参数换成数据库数据源[MC_MIXCAL_PAR]表PAR_K_FeO字段

            var sql_MC_MIXCAL_PAR = "SELECT PAR_K_FeO  FROM MC_MIXCAL_PAR where  TIMESTAMP = (SELECT MAX(TIMESTAMP) from [MC_MIXCAL_PAR] )";
            DataTable data = _mdb.GetCommand(sql_MC_MIXCAL_PAR);


            var _vdts_1 = data.AsEnumerable();
            if (_vdts_1.Count() <= 0)
            {
                Console.WriteLine("MC_MIXCAL_PAR中没有数据");
                return new Tuple<int, List<float>>(-1, null);//20200416

            }

            //long i;
            //按公式计算各种成分
            //  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19,  20, 21, 22, 23, 24, 25, 26, 27, 28,  29
            // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO, ZnO, F,  As, Cu, Pb, Zn, K,  Na, Cr, Ni, MnO  //宁钢检化验成分排序
            //	                                             烧损         灰分 挥发分      
            float sumTFe = 0;
            float sumFeO = 0;
            float sumCaO = 0;
            float sumSiO2 = 0;
            float sumAl2O3 = 0;
            float sumMgO = 0;
            float sumS = 0;
            float sumP = 0;
            float sumC = 0;//混合料实际含碳
            float sumMn = 0;
            float sumLOT = 0;//总烧损
            float sumR = 0;
            float sumH2O_1 = 0;//采用检化验检测水分计算的混合料初始含水量
            float sumH2O_2 = 0;//采用人工设定水分计算的混合料初始含水量
            float sumAsH = 0;
            float sumVOL = 0;
            float sumTiO2 = 0;
            float sumK2O = 0;
            float sumNa2O = 0;
            float sumPbO = 0;
            float sumZnO = 0;
            float sumF = 0;
            float sumAs = 0;
            float sumCu = 0;
            float sumPb = 0;
            float sumZn = 0;
            float sumK = 0;
            float sumNa = 0;
            float sumCr = 0;
            float sumNi = 0;
            float sumMnO = 0;

            float sumMix_Wet_PV = 0;      //实际总料量（湿）	
            float sumMix_Dry_PV = 0;      //实际总料量（干）
            float sumRemnant_PV = 0;       //实际总残存量  
            List<float> sinterAly = new List<float> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//TFe,FeO,CaO,SiO2,Al2O3,MgO,S,P,Mn,R,TiO2,K2O,Na2O,PbO,ZnO,F,As,Cu,Pb,Zn,K,Na,Cr,Ni,MnO,备用,备用,备用,备用,备用
            float sumOther_SiO2_PV = 0;   //除混匀矿和直供料外，其他原料带入的SiO2量
            float sumOther_C_PV = 0;      //非燃料总含碳量
            float sumFuel_C_PV = 0;       //燃料带入的实际固定碳量
            float sumOutput_PV = 0;       //实际下料量计算理论产量(单位：t/h)
            float sumFuel_PV = 0;         //燃料总实际干下料量
            float FuelCon_PV = 0;         //实际下料量计算的理论燃耗
                                          //20200808 修改
                                          //  float m_PV = 2.1f;             //C与FeO对应比例系数，需要现场数据分析进行修改
                                          //20200809 pv数据源换成数据库参数
                                          //   float m_PV = 3.02f;
            float m_PV = float.Parse(data.Rows[0][0].ToString());
            float sumDryBill = 0;         //当前干配比和
            float FeO_sp = 8.5f;           //FeO设定值用于理论产量计算
            float Burn_Back_Ratio_PV = 0; //实际下料计算烧返矿百分比（干） 
            float BurnLoss_Ratio_PV = 0;  //实际下料计算综合烧损(%)

            //数据准备
            //数据准备  (0.非溶剂或燃料 1.溶剂  2.燃料)
            //key:0 非燃料   1 燃料
            //val:仓号，下料口，实际下料量，当前水分，启停状态，物料成分，二级物料编码
            //START

            string sql = "select tt.category,p.MAT_L2_CH,p.MAT_L2_XLK,q.MAT_L2_SFDQ,b.L2_CODE,isnull(b.C_TFE,0) cf1,isnull(b.C_FEO,0) cf2,isnull(b.C_CAO,0) cf3,isnull(b.C_SIO2,0) cf4,isnull(b.C_AL2O3, 0) cf5,isnull(b.C_MGO, 0) cf6,isnull(b.C_S, 0) cf7,isnull(b.C_P, 0) cf8,isnull(b.C_C, 0) cf9,"
+ "isnull(b.C_MN, 0) cf10,isnull(b.C_LOT, 0) cf11,isnull(b.C_R, 0) cf12,isnull(b.C_H2O, 0) cf13,isnull(b.C_ASH, 0) cf14,isnull(b.C_VOLATILES, 0) cf15,isnull(b.C_TIO2, 0) cf16,isnull(b.C_K2O, 0) cf17,isnull(b.C_NA2O, 0) cf18,"
+ "isnull(b.C_PBO, 0) cf19,isnull(b.C_ZNO, 0) cf20,isnull(C_F, 0) cf21,isnull(C_As, 0) cf22,isnull(C_Cu, 0) cf23,"
+ "isnull(C_Pb, 0) cf24,isnull(C_Zn, 0) cf25,isnull(C_K, 0) cf26,isnull(C_Na, 0) cf27,isnull(C_Cr, 0) cf28,"
+ "isnull(C_Ni, 0) cf29,isnull(C_MnO, 0) cf30 from dbo.CFG_MAT_L2_XLK_INTERFACE p left join "
+ "dbo.CFG_MAT_L2_PBSD_INTERFACE tt on tt.canghao = p.MAT_L2_CH  left join "
+ "dbo.CFG_MAT_L2_SJPB_INTERFACE q on tt.canghao = q.MAT_L2_CH left join "
+ "dbo.M_MATERIAL_BINS b on b.BIN_NUM_SHOW = q.MAT_L2_CH order by p.MAT_L2_XLK";//类别，仓号，下料口，当前水分，成分
            DataTable _dt = _mdb.GetCommand(sql);

            string sql_1 = "select isnull(MAT_PLC_PV_W_1,0),isnull(MAT_PLC_PV_W_2, 0),isnull(MAT_PLC_PV_W_3, 0),"
+ "isnull(MAT_PLC_PV_W_4, 0),isnull(MAT_PLC_PV_W_5, 0),isnull(MAT_PLC_PV_W_6, 0),isnull(MAT_PLC_PV_W_7, 0),"
+ "isnull(MAT_PLC_PV_W_8, 0),isnull(MAT_PLC_PV_W_9, 0),isnull(MAT_PLC_PV_W_10, 0),isnull(MAT_PLC_PV_W_11, 0),"
+ "isnull(MAT_PLC_PV_W_12, 0),isnull(MAT_PLC_PV_W_13, 0),isnull(MAT_PLC_PV_W_14, 0),isnull(MAT_PLC_PV_W_15, 0),"
+ "isnull(MAT_PLC_PV_W_16, 0),isnull(MAT_PLC_PV_W_17, 0),isnull(MAT_PLC_PV_W_18, 0),isnull(MAT_PLC_PV_W_19, 0),isnull(MAT_PLC_PV_W_20, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_1, 0),isnull(MAT_PLC_SS_SIGNAL_2, 0),isnull(MAT_PLC_SS_SIGNAL_3, 0),isnull(MAT_PLC_SS_SIGNAL_4, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_5, 0),isnull(MAT_PLC_SS_SIGNAL_6, 0),isnull(MAT_PLC_SS_SIGNAL_7, 0),isnull(MAT_PLC_SS_SIGNAL_8, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_9, 0),isnull(MAT_PLC_SS_SIGNAL_10, 0),isnull(MAT_PLC_SS_SIGNAL_11, 0),isnull(MAT_PLC_SS_SIGNAL_12, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_13, 0),isnull(MAT_PLC_SS_SIGNAL_14, 0),isnull(MAT_PLC_SS_SIGNAL_15, 0),"
+ "isnull(MAT_PLC_SS_SIGNAL_16, 0),isnull(MAT_PLC_SS_SIGNAL_17, 0),isnull(MAT_PLC_SS_SIGNAL_18, 0),isnull(MAT_PLC_SS_SIGNAL_19, 0),isnull(MAT_PLC_SS_SIGNAL_20, 0)"
+ "from dbo.C_MAT_PLC_1MIN p order by p.TIMESTAMP desc";//实际下料量，启停信号
            DataTable _dts = _mdb.GetCommand(sql_1);
            var _vdts = _dts.AsEnumerable();
            if (_vdts.Count() <= 0)
            {
                Console.WriteLine("C_MAT_PLC_1MIN中没有数据");
                return new Tuple<int, List<float>>(-1, null);//20200416

            }
            List<Tuple<float, int>> spSS = new List<Tuple<float, int>>();//index:下料口号 item1:实际下料量，item2：下料口启停信号
            foreach (var x in _vdts)
            {
                for (int i = 0; i < x.ItemArray.Count() / 2; i++)
                {
                    float sp = float.Parse(x[i].ToString());
                    int ss = int.Parse(x[i + x.ItemArray.Count() / 2].ToString());
                    Tuple<float, int> _temp = new Tuple<float, int>(sp, ss);
                    spSS.Add(_temp);
                }
                break;
            }

            var _vdt = _dt.AsEnumerable();
            if (_vdt.Count() <= 0)
            {
                Console.WriteLine("CFG_MAT_L2_XLK_INTERFACE中数据异常");
                return new Tuple<int, List<float>>(-1, null);//20200416

            }
            Dictionary<int, List<Tuple<int, int, float, float, int, List<float>, int>>> rsl = new Dictionary<int, List<Tuple<int, int, float, float, int, List<float>, int>>>();

            foreach (var x in _vdt)
            {
                //int _p0 = int.Parse(x[0].ToString()) == 2 ? 1 : 0; //类别
                int _p0 = 0;
                if (float.Parse(x[13].ToString()) > 30 && float.Parse(x[18].ToString()) > 5)
                {
                    _p0 = 1;
                }
                bool iscontain = rsl.ContainsKey(_p0);
                if (iscontain)
                {
                    int _p1 = int.Parse(x[1].ToString());//仓号
                    int _p2 = int.Parse(x[2].ToString());//下料口
                    float _p3 = float.Parse(x[3].ToString());//当前水分
                    int _p4 = int.Parse(x[4].ToString());//二级物料编码
                    List<float> _p5 = new List<float>();
                    for (int i = 5; i < x.ItemArray.Count(); i++)
                    {
                        _p5.Add(float.Parse(x[i].ToString()));//成分
                    }

                    Tuple<int, int, float, float, int, List<float>, int> _tup = new Tuple<int, int, float, float, int, List<float>, int>(_p1, _p2, spSS[_p2 - 1].Item1, _p3, spSS[_p2 - 1].Item2, _p5, _p4);

                    rsl[_p0].Add(_tup);

                }
                else
                {
                    int _p1 = int.Parse(x[1].ToString());//仓号
                    int _p2 = int.Parse(x[2].ToString());//下料口
                    float _p3 = float.Parse(x[3].ToString());//当前水分
                    int _p4 = int.Parse(x[4].ToString());//二级物料编码
                    List<float> _p5 = new List<float>();
                    for (int i = 5; i < x.ItemArray.Count(); i++)
                    {
                        _p5.Add(float.Parse(x[i].ToString()));//成分
                    }

                    Tuple<int, int, float, float, int, List<float>, int> _tup = new Tuple<int, int, float, float, int, List<float>, int>(_p1, _p2, spSS[_p2 - 1].Item1, _p3, spSS[_p2 - 1].Item2, _p5, _p4);

                    List<Tuple<int, int, float, float, int, List<float>, int>> _ll = new List<Tuple<int, int, float, float, int, List<float>, int>>();
                    _ll.Add(_tup);
                    rsl.Add(_p0, _ll);
                }
            }


            //END
            //仓号，设定下料量，水分，启停状态
            List<Tuple<int, float, float, int>> _sf = new List<Tuple<int, float, float, int>>();
            List<Tuple<int, float, float, int>> _sf_sf = new List<Tuple<int, float, float, int>>();
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)//非燃料下料口
                {

                    /*    
                    输入项：

                           Silo[i].flow_avg     第i个下料口的实际下料量：C_MAT_PLC_1MIN表，MAT_L2_SJXLMAT_PLC_PV_W_1 - MAT_PLC_PV_W_19字段，对应的第i个下料口的实际下料量；
                           x.Item4      第i下料口的当前水分：CFG_MAT_L2_SJPB_INTERFACE表，MAT_L2_SFDQ字段，对应的第i下料口的当前水分值
                           x.Item5          第i下料口的启停状态：C_MAT_PLC_1MIN表，MAT_PLC_SS_SIGNAL_1 - MAT_PLC_SS_SIGNAL_19字段，对应第i下料口的启停状态；
                           Silo[i].aly[0]       第i下料口的物料a成分的含量： 根据CFG_MAT_L2_XLK_INTERFACE表，确定该下料口与仓号的对应关系，然后联合查询M_MATERIAL_BINS表中对应仓的物料a成分信息与给每个下料口进行匹配。
                           成分对应关系
                           //  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19,  20, 21, 22, 23, 24, 25, 26, 27, 28,  29
                           // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO,  ZnO, F, As, Cu, Pb, Zn, K,  Na, Cr, Ni, MnO  //宁钢检化验成分排序
                          //	                                            烧损         灰分 挥发分      

                    输出项：
                           sinterReport[i]      预测实际成分，保存到表MC_MIXCAL_RESULT_1MIN，对应字段(字段描述与数据库一致)
                    */
                    //val:仓号，下料口，实际下料量，当前水分，启停状态，物料成分，二级物料编码
                    foreach (var x in rsl[i])
                    {
                        int canghao = x.Item2;
                        //if (canghao == 17 || canghao == 18 || canghao == 19)
                        if (x.Item7 == L2Code[2] || x.Item7 == L2Code[3])
                        {
                            Tuple<int, float, float, int> _sftup = new Tuple<int, float, float, int>(canghao, x.Item3, x.Item4, x.Item5);
                            _sf.Add(_sftup);
                            if (x.Item7 == L2Code[3])
                            {
                                _sf_sf.Add(_sftup);
                            }

                        }
                        sumTFe += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[0] / 100;
                        sumFeO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[1] / 100;
                        sumCaO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[2] / 100;
                        sumSiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[3] / 100;
                        sumAl2O3 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[4] / 100;
                        sumMgO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[5] / 100;
                        sumS += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[6] / 100;
                        sumP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[7] / 100;
                        sumC += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;
                        sumMn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[9] / 100;
                        sumLOT += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[10] / 100;

                        sumAsH += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[13] / 100;
                        sumVOL += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[14] / 100;
                        sumTiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[15] / 100;
                        sumK2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[16] / 100;
                        sumNa2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[17] / 100;
                        sumPbO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[18] / 100;
                        sumZnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[19] / 100;
                        sumF += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[20] / 100;
                        sumAs += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[21] / 100;
                        sumCu += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[22] / 100;
                        sumPb += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[23] / 100;
                        sumZn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[24] / 100;
                        sumK += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[25] / 100;
                        sumNa += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[26] / 100;
                        sumCr += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[27] / 100;
                        sumNi += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[28] / 100;
                        sumMnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[29] / 100;

                        sumH2O_1 += x.Item3 * x.Item5 * x.Item6[12] / 100;
                        sumH2O_2 += x.Item3 * x.Item5 * x.Item4 / 100;

                        sumOther_C_PV += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;
                        sumRemnant_PV += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * (1 - x.Item6[10] / 100);

                        //
                        sumMix_Dry_PV += x.Item3 * (1 - x.Item4 / 100) * x.Item5;
                        sumMix_Wet_PV += x.Item3 * x.Item5;
                        //去掉
                        //sumDryBill += Silo[i].bill_use * x.Item5;//?????
                    }

                }
                else//燃料下料口
                {
                    foreach (var x in rsl[i])
                    {
                        int canghao = x.Item2;
                        if (x.Item7 == L2Code[2] || x.Item7 == L2Code[3])
                        {
                            Tuple<int, float, float, int> _sftup = new Tuple<int, float, float, int>(canghao, x.Item3, x.Item4, x.Item5);
                            _sf.Add(_sftup);
                            if (x.Item7 == L2Code[3])
                            {
                                _sf_sf.Add(_sftup);
                            }

                        }

                        sumTFe += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[0] / 100 * x.Item6[13] / 100;
                        sumFeO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[1] / 100 * x.Item6[13] / 100;
                        sumCaO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[2] / 100 * x.Item6[13] / 100;
                        sumSiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[3] / 100 * x.Item6[13] / 100;
                        sumAl2O3 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[4] / 100 * x.Item6[13] / 100;
                        sumMgO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[5] / 100 * x.Item6[13] / 100;
                        sumS += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[6] / 100 * x.Item6[13] / 100;
                        sumP += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[7] / 100 * x.Item6[13] / 100;
                        sumMn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[9] / 100 * x.Item6[13] / 100;
                        sumTiO2 += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[15] / 100 * x.Item6[13] / 100;
                        sumK2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[16] / 100 * x.Item6[13] / 100;
                        sumNa2O += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[17] / 100 * x.Item6[13] / 100;
                        sumPbO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[18] / 100 * x.Item6[13] / 100;
                        sumZnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[19] / 100 * x.Item6[13] / 100;
                        sumF += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[20] / 100 * x.Item6[13] / 100;
                        sumAs += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[21] / 100 * x.Item6[13] / 100;
                        sumCu += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[22] / 100 * x.Item6[13] / 100;
                        sumPb += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[23] / 100 * x.Item6[13] / 100;
                        sumZn += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[24] / 100 * x.Item6[13] / 100;
                        sumK += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[25] / 100 * x.Item6[13] / 100;
                        sumNa += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[26] / 100 * x.Item6[13] / 100;
                        sumCr += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[27] / 100 * x.Item6[13] / 100;
                        sumNi += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[28] / 100 * x.Item6[13] / 100;
                        sumMnO += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[29] / 100 * x.Item6[13] / 100;

                        sumC += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;
                        sumLOT += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * (1 - x.Item6[13] / 100);//燃料只有灰分检测，没有烧损检测
                        sumH2O_1 += x.Item3 * x.Item5 * x.Item6[12] / 100;
                        sumH2O_2 += x.Item3 * x.Item5 * x.Item4 / 100;
                        sumAsH += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[13] / 100;
                        sumVOL += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[14] / 100;

                        sumRemnant_PV += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[13] / 100;//默认燃料中有灰分检测，无烧损检测
                        sumFuel_C_PV += x.Item3 * (1 - x.Item4 / 100) * x.Item5 * x.Item6[8] / 100;
                        sumFuel_PV += x.Item3 * (1 - x.Item4 / 100) * x.Item5;
                        //
                        sumMix_Dry_PV += x.Item3 * (1 - x.Item4 / 100) * x.Item5;
                        sumMix_Wet_PV += x.Item3 * x.Item5;
                        //去掉
                        //sumDryBill += Silo[i].bill_use * x.Item5;//？？？？？？
                    }

                }



            }
            //
            float sf_cch = 0f;//烧返和除尘灰的干料量
            float sf_sf = 0f;//烧返的干料量
            //仓号，实际下料量，水分，启停状态
            foreach (var y in _sf)
            {
                sf_cch += y.Item2 * (1 - y.Item3 / 100) * y.Item4;

            }
            foreach (var y in _sf_sf)
            {
                sf_sf += y.Item2 * (1 - y.Item3 / 100) * y.Item4;

            }
            sumOutput_PV = sumRemnant_PV - sf_cch;//总残存 - 烧返和除尘灰的干料量
                                                  //sumOutput_PV = sumRemnant_PV - Silo[7].flow_avg * (1 - Silo[7].h2o_use/100) * Silo[7].use - Silo[8].flow_avg * (1 - Silo[8].h2o_use/100) * Silo[8].use - Silo[13].flow_avg * (1 - Silo[13].h2o_use/100) * Silo[13].use - Silo[14].flow_avg * (1 - Silo[14].h2o_use/100) * Silo[14].use;
            FuelCon_PV = sumFuel_PV / sumOutput_PV * 1000;

            //if (sumMix_Dry_PV > 0 && sumDryBill > 0)
            if (sumMix_Dry_PV > 0)
            {
                ////Burn_Back_Ratio_PV = 601编码的料仓的干料量和 / sumMix_Dry_PV * 100;--qxg20201125--凌钢实际返矿下料百分比
                // Burn_Back_Ratio_PV = sf_sf / sumMix_Dry_PV * 100;
                BurnLoss_Ratio_PV = sumLOT / sumMix_Dry_PV * 100;
            }
            else
            {
                //Burn_Back_Ratio_PV = 0;
                BurnLoss_Ratio_PV = 0;
            }

            if (sumMix_Wet_PV > 0)
            {

                sinterAly[0] = sumTFe / sumRemnant_PV * 100;                        //烧结矿--PV_TFE
                //20200808 lt 公式问题
                //  sinterAly[1] = (m_PV * sumC / sumMix_Dry_PV * 100) / sumRemnant_PV * 100;//烧结矿--PV_FeO
                sinterAly[1] = (m_PV * sumC / sumMix_Dry_PV * 100);//烧结矿--PV_FeO
                sinterAly[2] = sumCaO / sumRemnant_PV * 100;                        //烧结矿--PV_CaO
                sinterAly[3] = sumSiO2 / sumRemnant_PV * 100;                       //烧结矿--PV_SiO2
                sinterAly[4] = sumAl2O3 / sumRemnant_PV * 100;                      //烧结矿--PV_Al2O3
                sinterAly[5] = sumMgO / sumRemnant_PV * 100;                        //烧结矿--PV_MgO
                sinterAly[6] = sumS / sumRemnant_PV * 100;                          //烧结矿--PV_S
                sinterAly[7] = sumP / sumRemnant_PV * 100;                          //烧结矿--PV_P
                sinterAly[8] = sumMn / sumRemnant_PV * 100;                         //烧结矿--PV_Mn

                if (sinterAly[3] > 0)
                {
                    sinterAly[9] = sinterAly[2] / sinterAly[3];                //烧结矿--PV_R
                }

                sinterAly[10] = sumTiO2 / sumRemnant_PV * 100;                     //烧结矿--PV_TiO2
                sinterAly[11] = sumK2O / sumRemnant_PV * 100;                      //烧结矿--PV_K2O
                sinterAly[12] = sumNa2O / sumRemnant_PV * 100;                     //烧结矿--PV_Na2O
                sinterAly[13] = sumPbO / sumRemnant_PV * 100;                      //烧结矿--PV_PbO
                sinterAly[14] = sumZnO / sumRemnant_PV * 100;                      //烧结矿--PV_ZnO
                sinterAly[15] = sumF / sumRemnant_PV * 100;                        //烧结矿--PV_F
                sinterAly[16] = sumAs / sumRemnant_PV * 100;                       //烧结矿--PV_As
                sinterAly[17] = sumCu / sumRemnant_PV * 100;                       //烧结矿--PV_Cu
                sinterAly[18] = sumPb / sumRemnant_PV * 100;                       //烧结矿--PV_Pb
                sinterAly[19] = sumZn / sumRemnant_PV * 100;                       //烧结矿--PV_Zn
                sinterAly[20] = sumK / sumRemnant_PV * 100;                        //烧结矿--PV_K
                sinterAly[21] = sumNa / sumRemnant_PV * 100;                       //烧结矿--PV_Na
                sinterAly[22] = sumCr / sumRemnant_PV * 100;                       //烧结矿--PV_Cr
                sinterAly[23] = sumNi / sumRemnant_PV * 100;                       //烧结矿--PV_Ni
                sinterAly[24] = sumMnO / sumRemnant_PV * 100;                       //烧结矿--PV_MnO

                //
                Tuple<int, List<float>> _ret = new Tuple<int, List<float>>(0, new List<float>());

                for (int i = 0; i < 30; i++)
                {
                    _ret.Item2.Add(sinterAly[i]);//用于插入配料1Min结果表
                }



                _ret.Item2.Add(sumLOT / sumMix_Dry_PV * 100);            //混合料--实际下料量计算混合料综合烧损
                _ret.Item2.Add(sumH2O_1 / sumMix_Wet_PV);                //混合料--采集水分计算的混合料原始水分含量
                _ret.Item2.Add(sumH2O_2 / sumMix_Wet_PV);                //混合料--实际水分计算的混合料原始水分含量
                _ret.Item2.Add(sumFeO / sumRemnant_PV * 100);            //混合料--实际下料量计算混合料FeO含量
                _ret.Item2.Add(sumC / sumMix_Dry_PV * 100);              //混合料--实际下料量计算混合料固定碳
                _ret.Item2.Add(sumOther_C_PV / sumMix_Dry_PV * 100);     //混合料--实际下料量计算混合料中非燃料带入的固定碳
                _ret.Item2.Add(sumFuel_C_PV / sumMix_Dry_PV * 100);      //混合料--实际下料量计算混合料中燃料带入的固定碳
                _ret.Item2.Add(sumOther_SiO2_PV / sumMix_Dry_PV * 100);  //混合料--实际下料量计算混合料中非铁料带入的SIO2含量
                _ret.Item2.Add(sumMix_Dry_PV);                           //实际下料量计算总干料量
                _ret.Item2.Add(sumOutput_PV / 60);                       //实际下料量计算的每分钟理论产量(单位：t/min)
                _ret.Item2.Add(FuelCon_PV);                              //实际下料量计算的理论燃耗(干基)（单位：kg/t）
                                                                         // _ret.Item2.Add(Burn_Back_Ratio_PV);                     //实际下料计算烧返矿百分比（干） 
                _ret.Item2.Add(BurnLoss_Ratio_PV);                      //实际下料计算综合烧损(%)

                return _ret;
            }
            else
            {
                Console.WriteLine("sumMix_Wet_PV总下料量为0错误; 查看Read_MC_MEASURE_5MIN 是否5分钟表内没有数据");
                Console.WriteLine("查看Read_MC_MEASURE_5MIN 是否5分钟表内没有数据");
            }
            Tuple<int, List<float>> _rets = new Tuple<int, List<float>>(-8017, new List<float>());
            return _rets; //基本数据错误,烧结矿PV产量 <= 0
        }
        /// <summary>
        /// 
        /// 当前配比% curpb
        /// 配比ID sid
        /// 
        /// </summary>
        public int FeedBLCompute(int sid, float curpb)
        {

            DBSQL _mdb = new DBSQL(_connstring);
            //根据配比ID，查询对应的下料口 
            //下料口状态 仓号 下料口号 分仓系数 按照配比ID获取
            string sql = "select MAT_L2_XLKZT,MAT_L2_CH,MAT_L2_XLK,MAT_L2_FCXS from dbo.CFG_MAT_L2_XLK_INTERFACE where MAT_PB_ID=" + sid;
            //执行sql语句
            DataTable _dt = _mdb.GetCommand(sql);

            //位置，仓号，下料口，料口比例
            Dictionary<int, Tuple<int, int, float>> FeedBL = new Dictionary<int, Tuple<int, int, float>>();

            var _vdt = _dt.AsEnumerable();
            float FcSum = 0;
            int p0 = 0, p1 = 0, p2 = 0;
            float p3 = 0;
            //求和
            foreach (var x in _vdt)
            {
                p0 = int.Parse(x[0].ToString());//下料口状态
                p3 = float.Parse(x[3].ToString());//分仓系数
                if (1 == p0)//启用状态
                {
                    FcSum += p3;
                }
                else
                {
                    FcSum += 0;
                }
            }
            //计算
            for (int h = 0; h < _vdt.Count(); h++)
            {
                var x = _vdt.ElementAt(h);
                float BL = 0;
                p0 = int.Parse(x[0].ToString());//下料口状态
                p1 = int.Parse(x[1].ToString());//仓号
                p2 = int.Parse(x[2].ToString());//下料口号
                p3 = float.Parse(x[3].ToString());//分仓系数
                if (1 == p0)//启用状态
                {
                    BL = p3 / FcSum * curpb;
                }
                else
                {
                    BL = 0;
                }
                FeedBL.Add(h, new Tuple<int, int, float>(p1, p2, BL));
            }
            //更新数据库

            //更新料口比例
            for (int i = 0; i < _vdt.Count(); i++)
            {
                //按照仓号更新干下料比例和下料口号
                string usql = "update dbo.CFG_MAT_L2_XLK_INTERFACE set MAT_L2_GXLBL=" + FeedBL[i].Item3 + " where MAT_L2_CH=" + FeedBL[i].Item1 + " and MAT_L2_XLK=" + FeedBL[i].Item2;
                //
                //执行sql语句
                int _rs = _mdb.CommandExecuteNonQuery(usql);
                if (_rs > 0)
                {
                    //请写入日志表
                    string logstr = " 料口比例数据更新成功(sql =" + usql + ")";

                    Console.WriteLine("料口比例数据更新成功(sql={0})", usql);
                }
                else
                {
                    //请写入日志表
                    Console.WriteLine("料口比例数据更新失败(sql={0})", usql);
                }
                //
            }


            return 0;
        }

        //
        // 湿设定下料量计算
        //
        /// <summary>
        /// 
        /// 当前配比% curpb
        /// 配比ID sid
        /// 总量SP
        /// spVal 总料量 
        /// </summary>
        public int FeedLLCompute(float spVal)
        {
            DBSQL _mdb = new DBSQL(_connstring);
            //查询所有仓的仓号、下料口、料口比例、设定水分
            //****查询的数据为水分设定，修改为水分当前 2020-1-20 lt
            // string sql = "select t1.MAT_L2_CH,t1.MAT_L2_XLK,t1.MAT_L2_GXLBL,t2.MAT_L2_SFSD from dbo.CFG_MAT_L2_XLK_INTERFACE t1 inner join(select MAT_L2_CH, MAT_L2_SFSD from dbo.CFG_MAT_L2_SJPB_INTERFACE) t2 on t1.MAT_L2_CH = t2.MAT_L2_CH";
            //string sql = "select t1.MAT_L2_CH,t1.MAT_L2_XLK,t1.MAT_L2_GXLBL,t2.MAT_L2_SFDQ,t1.MAT_PB_ID,t1.MAT_L2_XLKZT,t1.MAT_L2_SIGN from dbo.CFG_MAT_L2_XLK_INTERFACE t1 inner join(select MAT_L2_CH, MAT_L2_SFDQ from dbo.CFG_MAT_L2_SJPB_INTERFACE) t2 on t1.MAT_L2_CH = t2.MAT_L2_CH where t1.MAT_L2_XLKZT=1";
            string sql = "select t1.MAT_L2_CH,t1.MAT_L2_XLK,t1.MAT_L2_GXLBL,t2.MAT_L2_SFDQ,t1.MAT_PB_ID,t1.MAT_L2_XLKZT,t1.MAT_L2_SIGN from dbo.CFG_MAT_L2_XLK_INTERFACE t1 inner join(select MAT_L2_CH, MAT_L2_SFDQ from dbo.CFG_MAT_L2_SJPB_INTERFACE) t2 on t1.MAT_L2_CH = t2.MAT_L2_CH ";
            //执行sql

            DataTable _dt = _mdb.GetCommand(sql);

            var _vdt = _dt.AsEnumerable();

            float Sum = 0;
            int p0 = 0;
            int p1 = 0;
            float p2 = 0;
            float p3 = 0;
            int p4 = 0;
            int p5 = 0;
            //求和
            foreach (var x in _vdt)
            {
                p2 = float.Parse(x[2].ToString());//干下料比例
                p3 = float.Parse(x[3].ToString());//水分当前百分比
                p4 = int.Parse(x[5].ToString());//下料口信号
                Sum += (p2 / (100 - p3) * 100) * p4;
            }
            //计算下料量
            float totalFeed = spVal;
            //仓号，下料口，下料量
            Dictionary<int, Tuple<int, int, float>> FeedXL = new Dictionary<int, Tuple<int, int, float>>();
            Dictionary<int, float> _New = new Dictionary<int, float>();
            //配比ID  设定下料量和 
            string sql_1 = "select p.MAT_PB_ID,SUM(p.MAT_L2_SDXL) as sumxl from dbo.CFG_MAT_L2_XLK_INTERFACE p  where p.MAT_L2_XLKZT=1  group by p.MAT_PB_ID  order by p.MAT_PB_ID";
            // string sql_1 = "select p.MAT_PB_ID,SUM(p.MAT_L2_SDXL) as sumxl from dbo.CFG_MAT_L2_XLK_INTERFACE p  where p.MAT_L2_SIGN=0  and p.MAT_L2_XLKZT=1  group by p.MAT_PB_ID  order by p.MAT_PB_ID";
            DataTable _dtt = _mdb.GetCommand(sql_1);
            var _vdtt = _dtt.AsEnumerable();
            Dictionary<int, float> _Old = new Dictionary<int, float>();

            foreach (var x in _vdtt)
            {
                int _p0 = int.Parse(x[0].ToString());
                float _p1 = float.Parse(x[1].ToString());
                if (_Old.ContainsKey(_p0))
                {
                    float _temp = _Old[_p0];
                    _Old.Remove(_p0);
                    _Old.Add(_p0, _temp);
                }
                else
                {
                    _Old.Add(_p0, _p1);
                }
            }

            //
            //string sql_x = "select y.MAT_L2_SDXL from  dbo.CFG_MAT_L2_XLK_INTERFACE y where y.MAT_L2_SIGN=0 and y.MAT_L2_XLKZT=1 order by y.MAT_L2_XLK";
            //DataTable _dtts = _mdb.GetCommand(sql_x);
            //var _vdtts = _dtts.AsEnumerable();
            for (int i = 0; i < _vdt.Count(); i++)
            {
                p0 = int.Parse(_vdt.ElementAt(i)[0].ToString());//仓号
                p1 = int.Parse(_vdt.ElementAt(i)[1].ToString());//下料口号
                p2 = float.Parse(_vdt.ElementAt(i)[2].ToString());//干下料比例
                p3 = float.Parse(_vdt.ElementAt(i)[3].ToString());//水分百分比
                p4 = int.Parse(_vdt.ElementAt(i)[4].ToString());//配比ID
                p5 = int.Parse(_vdt.ElementAt(i)[5].ToString());//下料口状态

                float xll = totalFeed * (p2 / (100 - p3) * 100) / Sum * p5;

                FeedXL.Add(i, new Tuple<int, int, float>(p0, p1, xll));

                //2020-120
                if (_New.ContainsKey(p4))
                {
                    float _temp = _New[p4];
                    _New.Remove(p4);
                    _New.Add(p4, _temp + xll);
                }
                else
                {
                    _New.Add(p4, xll);
                }
            }
            //Dictionary<int, float> _deta = new Dictionary<int, float>();
            //foreach (var x in _Old)
            //{
            //    float deta = _New[x.Key] - x.Value;
            //    if (_deta.ContainsKey(x.Key))
            //    {
            //        Console.WriteLine("配比配置错误，请查看配比设定以及下料口表");
            //        return -1;
            //    }
            //    else
            //    {
            //        _deta.Add(x.Key, deta);
            //    }
            //}

            //string sql_2 = "select p.MAT_PB_ID,p.MAT_L2_XLK,p.MAT_L2_CH from dbo.CFG_MAT_L2_XLK_INTERFACE p where p.MAT_L2_XLKZT=1 and p.MAT_L2_SIGN=1";
            //DataTable _dttt = _mdb.GetCommand(sql_2);
            //var _vdttt = _dttt.AsEnumerable();
            //Dictionary<int, Tuple<int, int, float>> _Live = new Dictionary<int, Tuple<int, int, float>>();

            //foreach (var x in _vdttt)
            //{
            //    int _p0 = int.Parse(x[0].ToString());//配比
            //    int _p1 = int.Parse(x[1].ToString());//下料口号
            //    int _p2 = int.Parse(x[2].ToString());//仓号
            //    if (_Live.ContainsKey(_p0))
            //    {
            //        return -1;
            //    }
            //    else
            //    {
            //        _Live.Add(_p0, new Tuple<int, int, float>(_p2, _p1, _deta[_p0]));
            //    }
            //}

            //foreach (var x in _Live)
            //{
            //    int ch = x.Value.Item1;
            //    int xlk = x.Value.Item2;
            //    float detax = x.Value.Item3;
            //    for (int i = 0; i < _vdt.Count(); i++)
            //    {
            //        if (FeedXL[i].Item1 == ch && FeedXL[i].Item2 == xlk)
            //        {
            //            float _temp = detax;
            //            FeedXL.Remove(i);
            //            FeedXL.Add(i, new Tuple<int, int, float>(ch, xlk, _temp));
            //        }
            //    }
            //}


            //更新数据库
            for (int i = 0; i < _vdt.Count(); i++)
            {
                //string  usql = "update dbo.CFG_MAT_L2_XLK_INTERFACE set MAT_L2_SDXL=" + FeedXL[i].Item3+ " " +"where MAT_L2_CH="+ FeedXL[i].Item1 + " and MAT_L2_XLK="+ FeedXL[i].Item2+ " and MAT_L2_SIGN=1";
                string usql = "update dbo.CFG_MAT_L2_XLK_INTERFACE set MAT_L2_SDXL=" + FeedXL[i].Item3 + " " + "where MAT_L2_CH=" + FeedXL[i].Item1 + " and MAT_L2_XLK=" + FeedXL[i].Item2 + "";

                int _rs = _mdb.CommandExecuteNonQuery(usql);
                if (_rs > 0)
                {
                    //请写入日志表
                    string logstr = " 下料量数据更新成功(sql =" + usql + ")";

                    Console.WriteLine("下料量数据更新成功(sql={0})", usql);


                }
                else
                {
                    //请写入日志表
                    Console.WriteLine("下料量数据更新失败(sql={0})", usql);
                    return -1;
                }
                //
            }
            //02-10 lt
            //string sql_10 = "select MAT_L2_CH from dbo.CFG_MAT_L2_XLK_INTERFACE p  where  p.MAT_L2_XLKZT=0  order by p.MAT_PB_ID";
            //DataTable dataTable = _mdb.GetCommand(sql_10);
            return 0;
        }

        /// <summary>
        /// 下料偏差计算
        /// </summary>
        /// <returns></returns>

        public void diffNormal()
        {
            DBSQL _mdb = new DBSQL(_connstring);
            //查询计算差值 仓号 下料口 实际下料量 设定下料量
            string sql = "select MAT_L2_CH,MAT_L2_XLK,MAT_L2_SJXL-MAT_L2_SDXL from dbo.CFG_MAT_L2_XLK_INTERFACE";

            //执行sql

            DataTable _dt = _mdb.GetCommand(sql);

            var _vdt = _dt.AsEnumerable();

            for (int i = 0; i < _vdt.Count(); i++)
            {
                //更新数据库
                string usql = "update dbo.CFG_MAT_L2_XLK_INTERFACE set MAT_L2_PC=" + (float)_vdt.ElementAt(i)[2] + " where MAT_L2_CH=" + (int)_vdt.ElementAt(i)[0] + " and MAT_L2_XLK=" + (int)_vdt.ElementAt(i)[1];
                int _rs = _mdb.CommandExecuteNonQuery(usql);
                if (_rs > 0)
                {
                    //请写入日志表
                    string logstr = " 下料量偏差数据更新成功(sql =" + usql + ")";

                    Console.WriteLine("下料量偏差数据更新成功(sql={0})", usql);
                }
                else
                {
                    //请写入日志表
                    Console.WriteLine("下料量偏差数据更新失败(sql={0})", usql);
                    return;
                }
                //
            }


        }

        /// <summary>
        /// 湿配比计算
        /// </summary>
        /// <returns></returns>
        public int WetPB()
        {
            //湿设定下料量和为SUM,料仓实际下料量LCj,料仓湿配比为LCSj(%)(其中j为仓号, j = 1, 2,...)
            //LCj += Qji; (其中i为下料口, i = 1, 2,...)
            //LCSj = SUM == 0 ? 0 : LCj / SUM * 100;

            DBSQL _mdb = new DBSQL(_connstring);
            //每一个仓的设定下料量 和 实际下料量
            string sql = "select t2.MAT_L2_CH,SUM(t2.MAT_L2_SDXL),SUM(t2.MAT_L2_SJXL) from  dbo.CFG_MAT_L2_XLK_INTERFACE t2 group by t2.MAT_L2_CH";

            //执行sql

            DataTable _dt = _mdb.GetCommand(sql);

            var _vdt = _dt.AsEnumerable();

            float SUM = 0;
            int p0 = 0;
            float p1 = 0;
            //求和
            foreach (var x in _vdt)
            {
                SUM += float.Parse(x[1].ToString());
            }
            //湿配比计算
            //仓号，湿配比值
            Dictionary<int, Tuple<int, float>> WetPBs = new Dictionary<int, Tuple<int, float>>();
            for (int i = 0; i < _vdt.Count(); i++)
            {
                p0 = int.Parse(_vdt.ElementAt(i)[0].ToString());
                p1 = float.Parse(_vdt.ElementAt(i)[1].ToString());
                float wpbval = SUM == 0 ? 0 : p1 / SUM * 100;
                WetPBs.Add(i, new Tuple<int, float>(p0, wpbval));
            }
            //更新数据库
            for (int i = 0; i < _vdt.Count(); i++)
            {
                //更新数据库
                string usql = "update dbo.CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SPB=" + WetPBs[i].Item2 + " where MAT_L2_CH=" + WetPBs[i].Item1;
                int _rs = _mdb.CommandExecuteNonQuery(usql);
                if (_rs > 0)
                {
                    //请写入日志表
                    string logstr = " 湿配比计算数据更新成功(sql =" + usql + ")";

                    Console.WriteLine("湿配比计算数据更新成功(sql={0})", usql);
                }
                else
                {
                    //请写入日志表
                    Console.WriteLine("湿配比计算数据更新失败(sql={0})", usql);
                    return -1;
                }
                //
            }

            return 0;
        }

        /// <summary>
        /// 料仓累计值
        /// </summary>
        /// <param name="matName">物料名称</param>
        /// <returns></returns>
        public int FeedAdd(string matName)
        {

            return 0;
        }

        /// <summary>
        /// 综合计算
        /// </summary>
        /// <returns></returns>
        public int Compute360()
        {
            DBSQL _mdb = new DBSQL(_connstring);
            //查询计算差值
            string sql = "select t2.MAT_L2_CH,SUM(t2.MAT_L2_SDXL),SUM(t2.MAT_L2_SJXL) from  dbo.CFG_MAT_L2_XLK_INTERFACE t2 group by t2.MAT_L2_CH";

            //执行sql

            DataTable _dt = _mdb.GetCommand(sql);

            var _vdt = _dt.AsEnumerable();
            return 0;
        }

        /// <summary>
        /// 数据准备
        /// </summary>
        /// <returns>
        /// item_1
        /// key:0、非燃料或溶剂  1、溶剂  2、燃料
        /// value:仓号，类别，物料二级编码，下料口，下料口启用标识，分仓系数和，成分
        /// item_2
        /// key:仓号(非燃料或溶剂 且 为启用状态)
        /// value:配比值
        /// </returns>
        private Tuple<Dictionary<int, List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>>, Dictionary<int, float>> houseData()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //返回结果

            Dictionary<int, List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>> _rs = new Dictionary<int, List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>>();
            Dictionary<int, float> noFuel = new Dictionary<int, float>();
            //string sql_0 = "select p.canghao,p.category,b.L2_CODE,c.MAT_L2_XLK,c.MAT_L2_XLKZT,c.MAT_L2_FCXS,isnull(b.C_TFE,0) cf1,isnull(b.C_FEO,0) cf2,isnull(b.C_CAO,0) cf3,isnull(b.C_SIO2,0) cf4,isnull(b.C_AL2O3, 0) cf5,isnull(b.C_MGO, 0) cf6,isnull(b.C_S, 0) cf7,isnull(b.C_P, 0) cf8,isnull(b.C_C, 0) cf9,"
            //                + "isnull(b.C_MN, 0) cf10,isnull(b.C_LOT,0) cf11,isnull(b.C_R, 0) cf12,isnull(b.C_H2O, 0) cf13,isnull(b.C_ASH, 0) cf14,isnull(b.C_VOLATILES, 0) cf15,isnull(b.C_TIO2, 0) cf16,isnull(b.C_K2O, 0) cf17,isnull(b.C_NA2O, 0) cf18,"
            //                + "isnull(b.C_PBO, 0) cf19,isnull(b.C_ZNO, 0) cf20,p.peinimingcheng,p.peibizhi from dbo.CFG_MAT_L2_PBSD_INTERFACE p, dbo.M_MATERIAL_BINS b, dbo.CFG_MAT_L2_XLK_INTERFACE c where p.canghao = b.BIN_NUM_SHOW and p.canghao = c.MAT_L2_CH";// "select p.canghao,p.peinimingcheng,p.category,b.L2_CODE from dbo.CFG_MAT_L2_PBSD_INTERFACE p,dbo.M_MATERIAL_BINS b where p.canghao=b.BIN_NUM_SHOW";
            //依次获取仓号，所属类别，二级编码，下料口号，下料口状态，分仓系数，30个成分，配比名称，配比值等数据
            string sql_0 = "select p.canghao,p.category,b.L2_CODE,c.MAT_L2_XLK,c.MAT_L2_XLKZT,c.MAT_L2_FCXS,isnull(b.C_TFE,0) cf1,"
           + "isnull(b.C_FEO, 0) cf2,isnull(b.C_CAO, 0) cf3,isnull(b.C_SIO2, 0) cf4,isnull(b.C_AL2O3, 0) cf5,isnull(b.C_MGO, 0) cf6,"
           + "isnull(b.C_S, 0) cf7,isnull(b.C_P, 0) cf8,isnull(b.C_C, 0) cf9,isnull(b.C_MN, 0) cf10,isnull(b.C_LOT, 0) cf11,isnull(b.C_R, 0) cf12,"
           + "isnull(b.C_H2O, 0) cf13,isnull(b.C_ASH, 0) cf14,isnull(b.C_VOLATILES, 0) cf15,isnull(b.C_TIO2, 0) cf16,isnull(b.C_K2O, 0) cf17,"
           + "isnull(b.C_NA2O, 0) cf18,isnull(b.C_PBO, 0) cf19,isnull(b.C_ZNO, 0) cf20,isnull(b.C_F, 0) cf21,isnull(b.C_AS, 0) cf22,isnull(b.C_CU, 0) cf23,"
           + "isnull(b.C_PB, 0) cf24,isnull(b.C_ZN, 0) cf25,isnull(b.C_K, 0) cf26,isnull(b.C_NA, 0) cf27,isnull(b.C_CR, 0) cf28,isnull(b.C_NI, 0) cf29,"
           + "isnull(b.C_MNO, 0) cf30,p.peinimingcheng,p.peibizhi from dbo.CFG_MAT_L2_PBSD_INTERFACE p, dbo.M_MATERIAL_BINS b, dbo.CFG_MAT_L2_XLK_INTERFACE c where p.canghao = b.BIN_NUM_SHOW and p.canghao = c.MAT_L2_CH order by p.category asc";
            DataTable _dt = _mdb.GetCommand(sql_0);
            if (_dt == null)
            {
                return null;
            }
            else
            {
                var _vdt = _dt.AsEnumerable();
                int p0 = 0, p1 = 0, p2 = 0, p3 = 0, p4 = 0, p26 = 0;
                float p5 = 0, p27 = 0;

                foreach (var x in _vdt)
                {
                    List<float> p7s = new List<float>();
                    p0 = int.Parse(x[0].ToString());//仓号
                    p1 = int.Parse(x[1].ToString());//类别:0：非熔剂、非燃料、非烧返、非白云石配比；1：熔剂配比；2：燃料配比；3：烧返配比；4：白云石配比
                    p2 = int.Parse(x[2].ToString());//二级编码
                    p3 = int.Parse(x[3].ToString());//下料口
                    p4 = int.Parse(x[4].ToString());//下料口状态
                    p5 = float.Parse(x[5].ToString());//分仓系数
                    p26 = int.Parse(x[36].ToString());//配比ID
                    p27 = float.Parse(x[37].ToString());//配比值
                    for (int p = 0; p < 30; p++)
                    {
                        p7s.Add(float.Parse(x[p + 6].ToString()));
                    }
                    Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>> trs = new Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>(p0, p1, p2, p3, p4, p5, p7s, new Tuple<int, float>(p26, p27));
                    if (_rs.ContainsKey(p1))
                    {
                        _rs[p1].Add(trs);
                    }
                    else
                    {
                        List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>> ltrs = new List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>();

                        _rs.Add(p1, ltrs);

                        _rs[p1].Add(trs);
                    }
                }
            }


            //查询启动状态的仓、分仓系数以及配比值
            //string sql_1 = "select w.MAT_L2_CH,w.SumCol,w.SumCol*t.peibizhi as PbVal from (select k.MAT_L2_CH,SUM(k.MAT_L2_FCXS) as SumCol from dbo.CFG_MAT_L2_XLK_INTERFACE k " +
            //                "where k.MAT_L2_XLKZT = 1 group by k.MAT_L2_CH) w inner join dbo.CFG_MAT_L2_PBSD_INTERFACE t  on w.MAT_L2_CH = t.canghao where t.category=0";


            //查询仓号，分仓系数/分仓系数和  ,配比ID, where 下料口为启动状态=1 连接 配置值 配比名称 仓号 所属分类
            string sql_1 = "select *  from (((select k.MAT_L2_CH, k.MAT_L2_FCXS / w.SumCol as BL from dbo.CFG_MAT_L2_XLK_INTERFACE k "
+ " inner join (select kk.MAT_PB_ID, SUM(kk.MAT_L2_FCXS) as SumCol from dbo.CFG_MAT_L2_XLK_INTERFACE kk where kk.MAT_L2_XLKZT = 1 group by kk.MAT_PB_ID) w on w.MAT_PB_ID = k.MAT_PB_ID)) g "
+ " join (select y.peibizhi, y.peinimingcheng, y.canghao,y.category from dbo.CFG_MAT_L2_PBSD_INTERFACE y) f on f.canghao = g.MAT_L2_CH) where category=0";

            DataTable _dts = _mdb.GetCommand(sql_1);
            if (_dts == null)
            {
                return null;
            }
            else
            {
                ///非燃料或者溶剂
                ///仓号，配比值
                ///
                var _vdts = _dts.AsEnumerable();
                //Dictionary<int, float> noFuel = new Dictionary<int, float>();
                int p0 = 0;
                float p1 = 0, p2 = 0;
                for (int i = 0; i < _vdts.Count(); i++)
                {
                    p0 = int.Parse(_vdts.ElementAt(i)[0].ToString());//仓号
                    p1 = float.Parse(_vdts.ElementAt(i)[1].ToString());// 分仓系数/分仓系数和
                    p2 = float.Parse(_vdts.ElementAt(i)[2].ToString()); //配比ID
                    p1 *= p2;

                    if (noFuel.ContainsKey(p0))//一仓多口
                    {
                        //累加
                        float _temp = noFuel[p0] + p1;
                        noFuel.Remove(p0);
                        noFuel.Add(p0, _temp);

                    }
                    else
                    {
                        noFuel.Add(p0, p1);
                    }

                }
            }



            return new Tuple<Dictionary<int, List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>>, Dictionary<int, float>>(_rs, noFuel);
        }
        private Tuple<Dictionary<int, List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>>, Dictionary<int, float>> houseDataCRM()
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //返回结果

            Dictionary<int, List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>> _rs = new Dictionary<int, List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>>();
            Dictionary<int, float> noFuel = new Dictionary<int, float>();
            //string sql_0 = "select p.canghao,p.category,b.L2_CODE,c.MAT_L2_XLK,c.MAT_L2_XLKZT,c.MAT_L2_FCXS,isnull(b.C_TFE,0) cf1,isnull(b.C_FEO,0) cf2,isnull(b.C_CAO,0) cf3,isnull(b.C_SIO2,0) cf4,isnull(b.C_AL2O3, 0) cf5,isnull(b.C_MGO, 0) cf6,isnull(b.C_S, 0) cf7,isnull(b.C_P, 0) cf8,isnull(b.C_C, 0) cf9,"
            //                + "isnull(b.C_MN, 0) cf10,isnull(b.C_LOT,0) cf11,isnull(b.C_R, 0) cf12,isnull(b.C_H2O, 0) cf13,isnull(b.C_ASH, 0) cf14,isnull(b.C_VOLATILES, 0) cf15,isnull(b.C_TIO2, 0) cf16,isnull(b.C_K2O, 0) cf17,isnull(b.C_NA2O, 0) cf18,"
            //                + "isnull(b.C_PBO, 0) cf19,isnull(b.C_ZNO, 0) cf20,p.peinimingcheng,p.peibizhi from dbo.CFG_MAT_L2_PBSD_INTERFACE p, dbo.M_MATERIAL_BINS b, dbo.CFG_MAT_L2_XLK_INTERFACE c where p.canghao = b.BIN_NUM_SHOW and p.canghao = c.MAT_L2_CH order by p.category asc";// "select p.canghao,p.peinimingcheng,p.category,b.L2_CODE from dbo.CFG_MAT_L2_PBSD_INTERFACE p,dbo.M_MATERIAL_BINS b where p.canghao=b.BIN_NUM_SHOW";

            string sql_0 = "select p.canghao,p.category,b.L2_CODE,c.MAT_L2_XLK,c.MAT_L2_XLKZT,c.MAT_L2_FCXS,isnull(b.C_TFE,0) cf1,"
            + "isnull(b.C_FEO, 0) cf2,isnull(b.C_CAO, 0) cf3,isnull(b.C_SIO2, 0) cf4,isnull(b.C_AL2O3, 0) cf5,isnull(b.C_MGO, 0) cf6,"
            + "isnull(b.C_S, 0) cf7,isnull(b.C_P, 0) cf8,isnull(b.C_C, 0) cf9,isnull(b.C_MN, 0) cf10,isnull(b.C_LOT, 0) cf11,isnull(b.C_R, 0) cf12,"
            + "isnull(b.C_H2O, 0) cf13,isnull(b.C_ASH, 0) cf14,isnull(b.C_VOLATILES, 0) cf15,isnull(b.C_TIO2, 0) cf16,isnull(b.C_K2O, 0) cf17,"
            + "isnull(b.C_NA2O, 0) cf18,isnull(b.C_PBO, 0) cf19,isnull(b.C_ZNO, 0) cf20,isnull(b.C_F, 0) cf21,isnull(b.C_AS, 0) cf22,isnull(b.C_CU, 0) cf23,"
            + "isnull(b.C_PB, 0) cf24,isnull(b.C_ZN, 0) cf25,isnull(b.C_K, 0) cf26,isnull(b.C_NA, 0) cf27,isnull(b.C_CR, 0) cf28,isnull(b.C_NI, 0) cf29,"
            + "isnull(b.C_MNO, 0) cf30,p.peinimingcheng   ,p.peibizhi *c.MAT_L2_XLKZT as peibizhi from dbo.CFG_MAT_L2_PBSD_INTERFACE p, dbo.M_MATERIAL_BINS b, dbo.CFG_MAT_L2_XLK_INTERFACE c where p.canghao = b.BIN_NUM_SHOW and p.canghao = c.MAT_L2_CH order by p.category asc";
            //

            DataTable _dt = _mdb.GetCommand(sql_0);
            if (_dt == null)
            {
                return null;
            }
            else
            {
                var _vdt = _dt.AsEnumerable();
                int p0 = 0, p1 = 0, p2 = 0, p3 = 0, p4 = 0, p26 = 0;
                float p5 = 0, p27 = 0;

                foreach (var x in _vdt)
                {
                    List<float> p7s = new List<float>();

                    p0 = int.Parse(x[0].ToString());//仓号
                    p1 = int.Parse(x[1].ToString());//类别:0：非熔剂、非燃料、非烧返、非白云石配比；1：熔剂配比；2：燃料配比；3：烧返配比；4：白云石配比
                    p2 = int.Parse(x[2].ToString());//二级编码
                    p3 = int.Parse(x[3].ToString());//下料口
                    p4 = int.Parse(x[4].ToString());//下料口状态
                    p5 = float.Parse(x[5].ToString());//分仓系数
                    p26 = int.Parse(x[36].ToString());//配比ID
                    p27 = float.Parse(x[37].ToString());//配比值
                    p27 = float.Parse(x[37].ToString());
                    for (int p = 0; p < 30; p++)
                    {
                        p7s.Add(float.Parse(x[p + 6].ToString()));
                    }
                    //20210204烧返配比配比值处理
                    //if (p1 == 3)
                    //    p27 = p27 / 2;

                    Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>> trs = new Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>(p0, p1, p2, p3, p4, p5, p7s, new Tuple<int, float>(p26, p27));
                    if (_rs.ContainsKey(p1))
                    {
                        _rs[p1].Add(trs);
                    }
                    else
                    {
                        List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>> ltrs = new List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>();

                        _rs.Add(p1, ltrs);

                        _rs[p1].Add(trs);
                    }
                }
            }

            //查询启动状态的仓、分仓系数以及配比值
            //       string sql_1 = "select *  from (((select k.MAT_L2_CH, k.MAT_L2_FCXS / w.SumCol as BL from dbo.CFG_MAT_L2_XLK_INTERFACE k "
            //+ " inner join (select kk.MAT_PB_ID, SUM(kk.MAT_L2_FCXS) as SumCol from dbo.CFG_MAT_L2_XLK_INTERFACE kk where kk.MAT_L2_XLKZT = 1 group by kk.MAT_PB_ID) w on w.MAT_PB_ID = k.MAT_PB_ID)) g "
            //+ " join (select y.peibizhi, y.peinimingcheng, y.canghao,y.category from dbo.CFG_MAT_L2_PBSD_INTERFACE y) f on f.canghao = g.MAT_L2_CH) where category=0 or category = 3";

            string sql_1 = "select *  from (((select k.MAT_L2_CH, k.MAT_L2_FCXS / w.SumCol * k.MAT_L2_XLKZT as BL from dbo.CFG_MAT_L2_XLK_INTERFACE k "
         + " inner join (select kk.MAT_PB_ID, SUM(kk.MAT_L2_FCXS) as SumCol from dbo.CFG_MAT_L2_XLK_INTERFACE kk where kk.MAT_L2_XLKZT = 1 group by kk.MAT_PB_ID) w on w.MAT_PB_ID = k.MAT_PB_ID)) g "
         + " join (select y.peibizhi, y.peinimingcheng, y.canghao,y.category from dbo.CFG_MAT_L2_PBSD_INTERFACE y) f on f.canghao = g.MAT_L2_CH) where category=0 or category = 3";

            //string sql_1 = "select *  from (((select k.MAT_L2_CH, k.MAT_L2_XLKZT / w.SumCol as BL from dbo.CFG_MAT_L2_XLK_INTERFACE k "
            //+ " inner join (select kk.MAT_PB_ID, SUM(kk.MAT_L2_FCXS) as SumCol from dbo.CFG_MAT_L2_XLK_INTERFACE kk where kk.MAT_L2_XLKZT = 1 group by kk.MAT_PB_ID) w on w.MAT_PB_ID = k.MAT_PB_ID)) g "
            //+ " join (select y.peibizhi, y.peinimingcheng, y.canghao,y.category from dbo.CFG_MAT_L2_PBSD_INTERFACE y) f on f.canghao = g.MAT_L2_CH) where category=0 or category = 3";

            DataTable _dts = _mdb.GetCommand(sql_1);
            if (_dts == null)
            {
                return null;
            }
            else
            {
                ///非燃料或者溶剂
                ///仓号，配比值
                ///
                var _vdts = _dts.AsEnumerable();
                //Dictionary<int, float> noFuel = new Dictionary<int, float>();
                int p0 = 0;
                float p1 = 0, p2 = 0;
                for (int i = 0; i < _vdts.Count(); i++)
                {
                    p0 = int.Parse(_vdts.ElementAt(i)[0].ToString());
                    p1 = float.Parse(_vdts.ElementAt(i)[1].ToString());
                    p2 = float.Parse(_vdts.ElementAt(i)[2].ToString());
                    p1 *= p2;

                    if (noFuel.ContainsKey(p0))//一仓多口
                    {
                        //累加
                        float _temp = noFuel[p0] + p1;
                        noFuel.Remove(p0);
                        noFuel.Add(p0, _temp);

                    }
                    else
                    {
                        noFuel.Add(p0, p1);
                    }

                }
            }


            return new Tuple<Dictionary<int, List<Tuple<int, int, int, int, int, float, List<float>, Tuple<int, float>>>>, Dictionary<int, float>>(_rs, noFuel);
        }

        ///// <summary>
        ///// 计算溶剂和燃料的配比
        ///// </summary>
        ///// <returns>异常代码(0-正常)，溶剂配比，燃料配比</returns>
        //public Tuple<int, float, float> CptSolfuel(float C_Aim, float R_Aim, float R_md, float C_md)
        //{
        //    //初始化数据库
        //    DBSQL _mdb = new DBSQL(_connstring);
        //    //返回结果
        //    Tuple<float, float> rs = new Tuple<float, float>(0, 0);

        //    float R_Aim_md = R_Aim + R_md;//目标碱度+R调整值
        //    float C_Aim_md = C_Aim + C_md;//目标含碳+C调整值


        //    float[] FuelAly = new float[20];          //燃料加权平均成分数组--包含20个成分
        //    float[] FluxAly = new float[20];          //熔剂加权平均成分数组--包含20个成分

        //    float sumCaO = 0;           //除熔剂、燃料调整仓以外原料带入烧结矿 CaO
        //    float sumSiO2 = 0;          //除熔剂、燃料调整仓以外原料带入烧结矿 SiO2
        //    float sumBill = 0;          //除熔剂、燃料调整仓以外原料总配比数
        //    float sumC = 0;             //除熔剂、燃料调整仓以外原料带入烧结矿 C
        //    float a1 = 0;               //燃料方程系数
        //    float b1 = 0;               //燃料方程系数
        //    float n1 = 0;               //燃料方程系数
        //    float a2 = 0;               //熔剂方程系数
        //    float b2 = 0;               //熔剂方程系数
        //    float n2 = 0;               //熔剂方程系数
        //    float Flux_Bill = 0;        //熔剂调整仓计算配比
        //    float Fuel_Bill = 0;        //燃料调整仓计算配比
        //    float F_CaO = 0;            //燃料调整仓燃料有效CaO
        //    float F_SiO2 = 0;           //燃料调整仓燃料有效SiO2
        //    float L_CaO = 0;            //熔剂调整仓熔剂有效CaO
        //    float L_SiO2 = 0;           //熔剂调整仓熔剂有效SiO2


        //    //  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19
        //    // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO, ZnO    //宁钢检化验成分排序
        //    //	                                             烧损         灰分 挥发分      


        //    /*20191224先根据配比和仓号的对应关系，对每个料仓是否为燃料和熔剂进行标记,根据标记情况选出熔剂和燃料仓号*/
        //    var hrs = houseData();


        //    for (int i = 0; i < 20; i++)//根据选择出的熔剂和燃料仓号，求出燃料和熔剂的加权平均成分
        //    {
        //        //燃料加权平均成分计算  //qxg Silo[i].use

        //        //FuelAly[i] = ((Silo[燃料1仓号 - 1].aly[i] * Silo[燃料1仓号 - 1].use * Silo[燃料1仓号 - 1].allot) + (Silo[燃料2仓号 - 1].aly[i] * Silo[燃料2仓号 - 1].use * Silo[燃料2仓号 - 1].allot)) / (Silo[燃料1仓号 - 1].use * Silo[燃料1仓号 - 1].allot + Silo[燃料2仓号 - 1].use * Silo[燃料2仓号 - 1].allot);//有几个燃料仓，加权平均计算时就用几个仓
        //        //FluxAly[i] = ((Silo[熔剂1仓号 - 1].aly[i] * Silo[熔剂1仓号 - 1].use * Silo[熔剂1仓号 - 1].allot) + (Silo[熔剂2仓号 - 1].aly[i] * Silo[熔剂2仓号 - 1].use * Silo[熔剂2仓号 - 1].allot)) / (Silo[熔剂1仓号 - 1].use * Silo[熔剂1仓号 - 1].allot + Silo[熔剂2仓号 - 1].use * Silo[熔剂2仓号 - 1].allot);//有几个熔剂仓，加权平均计算时就用几个仓
        //        float sumFuelChild = 0;
        //        float sumFuelMother = 0;
        //        float sumFluxChild = 0;
        //        float sumFluxMother = 0;
        //        for (int j = 0; j < hrs.Item1[2].Count; j++)
        //        {
        //            sumFuelChild += hrs.Item1[2][j].Item7[i] * hrs.Item1[2][j].Item5 * hrs.Item1[2][j].Item6;
        //            sumFuelMother += hrs.Item1[2][j].Item5 * hrs.Item1[2][j].Item6;
        //        }
        //        FuelAly[i] = sumFuelChild / sumFuelMother;
        //        for (int j = 0; j < hrs.Item1[1].Count; j++)
        //        {
        //            sumFluxChild += hrs.Item1[1][j].Item7[i] * hrs.Item1[1][j].Item5 * hrs.Item1[1][j].Item6;
        //            sumFluxMother += hrs.Item1[1][j].Item5 * hrs.Item1[1][j].Item6;
        //        }
        //        FluxAly[i] = sumFluxChild / sumFluxMother;
        //    }

        //    //
        //    foreach (var x in hrs.Item2)//仓，配比值
        //    {
        //        foreach (var y in hrs.Item1[0])//非溶剂或燃料仓
        //        {
        //            if (y.Item1.ToString() == x.Key.ToString())
        //            {
        //                if (y.Item7[8] > 30 && y.Item7[13] > 5)//单位？/////////////////////////////
        //                {
        //                    //Silo[i].bill_sp：为第i个仓的设定配比，需要将多个仓共用一个配比的情况，提前按照料仓进行拆分计算；
        //                    //Silo[i].use：为第i个仓的启用状态，需要对一个仓两个下料口的情况，进行单独判断；

        //                    sumCaO += x.Value * y.Item7[2] / 100 * y.Item7[13] / 100;
        //                    sumSiO2 += x.Value * y.Item7[3] / 100 * y.Item7[13] / 100;
        //                    sumBill += x.Value;//除 石灰石X1,燃料X2 以外原料总配比数
        //                    sumC += x.Value * y.Item7[8] / 100;
        //                }
        //                else
        //                {
        //                    sumCaO += x.Value * y.Item7[2] / 100;
        //                    sumSiO2 += x.Value * y.Item7[3] / 100;
        //                    sumBill += x.Value;
        //                    sumC += x.Value * y.Item7[8] / 100;
        //                }
        //            }
        //        }

        //    }

        //    ////碱度调整仓原料的含碳量,工艺上不可能大于等于目标含碳量

        //    if (FluxAly[8] / 100 - C_Aim_md / 100 > -0.01f) return new Tuple<int, float, float>(-8011, 0, 0); //碱度调整仓原料的含碳量,工艺上不可能大于等于目标含碳量；

        //    F_CaO = FuelAly[2] / 100 * FuelAly[13] / 100;
        //    F_SiO2 = FuelAly[3] / 100 * FuelAly[13] / 100;

        //    L_CaO = FluxAly[2] / 100;
        //    L_SiO2 = FluxAly[3] / 100;

        //    //燃料配比方程
        //    a1 = FluxAly[8] / 100 - C_Aim_md / 100;
        //    b1 = FuelAly[8] / 100 - C_Aim_md / 100;
        //    n1 = C_Aim_md / 100 * sumBill - sumC;

        //    //灰石配比方程
        //    a2 = L_CaO - L_SiO2 * R_Aim_md;
        //    b2 = F_CaO - F_SiO2 * R_Aim_md;
        //    n2 = sumSiO2 * R_Aim_md - sumCaO;

        //    if (a2 * b1 - a1 * b2 == 0) return new Tuple<int, float, float>(-8012, 0, 0); //确保分母不为零，保证方程有解

        //    Flux_Bill = (b1 * n2 - b2 * n1) / (a2 * b1 - a1 * b2);
        //    Fuel_Bill = (a2 * n1 - a1 * n2) / (a2 * b1 - a1 * b2);

        //    if ((Flux_Bill > 0) && (Fuel_Bill > 0))
        //    {
        //        return new Tuple<int, float, float>(0, Flux_Bill, Fuel_Bill);
        //    }
        //    else
        //    {
        //        //有小0的通常说明混合料中CaO已经过多 石灰石调节要求减少石灰石配比
        //        //或者原料成分不合适
        //        return new Tuple<int, float, float>(-8013, 0, 0);
        //    }
        //}

        /// <summary>
        /// 计算溶剂和燃料的配比
        /// 输入（目标值，调整值）
        /// </summary>
        /// <returns>异常代码(0-正常)，溶剂配比，燃料配比</returns>

        //界面调用计算配比 计算溶剂和燃料的配比
        public Tuple<int, float, float> CptSolfuels(float C_Aim, float R_Aim, float R_md, float C_md)
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //返回结果
            Tuple<float, float> rs = new Tuple<float, float>(0, 0);

            float R_Aim_md = R_Aim + R_md;//目标碱度+R调整值
            float C_Aim_md = C_Aim + C_md;//目标含碳+C调整值


            float[] FuelAly = new float[30];          //燃料加权平均成分数组--包含30个成分
            float[] FluxAly = new float[30];          //熔剂加权平均成分数组--包含30个成分

            float sumCaO = 0;           //除熔剂、燃料调整仓以外原料带入烧结矿 CaO
            float sumSiO2 = 0;          //除熔剂、燃料调整仓以外原料带入烧结矿 SiO2
            float sumBill = 0;          //除熔剂、燃料调整仓以外原料总配比数
            float sumC = 0;             //除熔剂、燃料调整仓以外原料带入烧结矿 C
            float a1 = 0;               //燃料方程系数
            float b1 = 0;               //燃料方程系数
            float n1 = 0;               //燃料方程系数
            float a2 = 0;               //熔剂方程系数
            float b2 = 0;               //熔剂方程系数
            float n2 = 0;               //熔剂方程系数
            float Flux_Bill = 0;        //熔剂调整仓计算配比
            float Fuel_Bill = 0;        //燃料调整仓计算配比
            float F_CaO = 0;            //燃料调整仓燃料有效CaO
            float F_SiO2 = 0;           //燃料调整仓燃料有效SiO2
            float L_CaO = 0;            //熔剂调整仓熔剂有效CaO
            float L_SiO2 = 0;           //熔剂调整仓熔剂有效SiO2


            //  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19
            // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO, ZnO    //宁钢检化验成分排序
            //	                                             烧损         灰分 挥发分      


            /*20191224先根据配比和仓号的对应关系，对每个料仓是否为燃料和熔剂进行标记,根据标记情况选出熔剂和燃料仓号*/
            var hrs = houseData();


            for (int i = 0; i < 30; i++)//根据选择出的熔剂和燃料仓号，求出燃料和熔剂的加权平均成分
            {
                //燃料加权平均成分计算  //qxg Silo[i].use

                //FuelAly[i] = ((Silo[燃料1仓号 - 1].aly[i] * Silo[燃料1仓号 - 1].use * Silo[燃料1仓号 - 1].allot) + (Silo[燃料2仓号 - 1].aly[i] * Silo[燃料2仓号 - 1].use * Silo[燃料2仓号 - 1].allot)) / (Silo[燃料1仓号 - 1].use * Silo[燃料1仓号 - 1].allot + Silo[燃料2仓号 - 1].use * Silo[燃料2仓号 - 1].allot);//有几个燃料仓，加权平均计算时就用几个仓
                //FluxAly[i] = ((Silo[熔剂1仓号 - 1].aly[i] * Silo[熔剂1仓号 - 1].use * Silo[熔剂1仓号 - 1].allot) + (Silo[熔剂2仓号 - 1].aly[i] * Silo[熔剂2仓号 - 1].use * Silo[熔剂2仓号 - 1].allot)) / (Silo[熔剂1仓号 - 1].use * Silo[熔剂1仓号 - 1].allot + Silo[熔剂2仓号 - 1].use * Silo[熔剂2仓号 - 1].allot);//有几个熔剂仓，加权平均计算时就用几个仓
                float sumFuelChild = 0;
                float sumFuelMother = 0;
                float sumFluxChild = 0;
                float sumFluxMother = 0;
                for (int j = 0; j < hrs.Item1[2].Count; j++)
                {
                    //燃料
                    sumFuelChild += hrs.Item1[2][j].Item7[i] * hrs.Item1[2][j].Item5 * hrs.Item1[2][j].Item6;//成分*下料口状态*分仓系数
                    sumFuelMother += hrs.Item1[2][j].Item5 * hrs.Item1[2][j].Item6;//下料口状态*分仓系数
                }
                FuelAly[i] = sumFuelChild / sumFuelMother;
                for (int j = 0; j < hrs.Item1[1].Count; j++)
                {
                    //溶剂
                    sumFluxChild += hrs.Item1[1][j].Item7[i] * hrs.Item1[1][j].Item5 * hrs.Item1[1][j].Item6;
                    sumFluxMother += hrs.Item1[1][j].Item5 * hrs.Item1[1][j].Item6;
                }
                FluxAly[i] = sumFluxChild / sumFluxMother;
            }

            //
            foreach (var x in hrs.Item2)//仓，配比值
            {
                foreach (var y in hrs.Item1[0])//非溶剂或燃料仓
                {
                    if (y.Item1.ToString() == x.Key.ToString())
                    {
                        //含碳量>30% 灰分>5% 则认为是燃料的一种，计算CaO、SiO2需要考虑灰分的量
                        if (y.Item7[8] > 30 && y.Item7[13] > 5)//单位？/////////////////////////////
                        {
                            //Silo[i].bill_sp：为第i个仓的设定配比，需要将多个仓共用一个配比的情况，提前按照料仓进行拆分计算；
                            //Silo[i].use：为第i个仓的启用状态，需要对一个仓两个下料口的情况，进行单独判断；

                            sumCaO += x.Value * y.Item7[2] / 100 * y.Item7[13] / 100;
                            sumSiO2 += x.Value * y.Item7[3] / 100 * y.Item7[13] / 100;
                            sumBill += x.Value;//除 石灰石X1,燃料X2 以外原料总配比数
                            sumC += x.Value * y.Item7[8] / 100;
                        }
                        else
                        {
                            sumCaO += x.Value * y.Item7[2] / 100;
                            sumSiO2 += x.Value * y.Item7[3] / 100;
                            sumBill += x.Value;
                            sumC += x.Value * y.Item7[8] / 100;
                        }
                        break;
                    }
                }

            }

            ////碱度调整仓原料的含碳量,工艺上不可能大于等于目标含碳量

            if (FluxAly[8] / 100 - C_Aim_md / 100 > -0.01f) return new Tuple<int, float, float>(-8011, 0, 0); //碱度调整仓原料的含碳量,工艺上不可能大于等于目标含碳量；

            F_CaO = FuelAly[2] / 100 * FuelAly[13] / 100;
            F_SiO2 = FuelAly[3] / 100 * FuelAly[13] / 100;

            L_CaO = FluxAly[2] / 100;
            L_SiO2 = FluxAly[3] / 100;

            //燃料配比方程
            a1 = FluxAly[8] / 100 - C_Aim_md / 100;
            b1 = FuelAly[8] / 100 - C_Aim_md / 100;
            n1 = C_Aim_md / 100 * sumBill - sumC;

            //灰石配比方程
            a2 = L_CaO - L_SiO2 * R_Aim_md;
            b2 = F_CaO - F_SiO2 * R_Aim_md;
            n2 = sumSiO2 * R_Aim_md - sumCaO;

            if (a2 * b1 - a1 * b2 == 0) return new Tuple<int, float, float>(-8012, 0, 0); //确保分母不为零，保证方程有解

            Flux_Bill = (b1 * n2 - b2 * n1) / (a2 * b1 - a1 * b2);
            Fuel_Bill = (a2 * n1 - a1 * n2) / (a2 * b1 - a1 * b2);

            if ((Flux_Bill > 0) && (Fuel_Bill > 0))
            {
                return new Tuple<int, float, float>(0, Flux_Bill, Fuel_Bill);
            }
            else
            {
                //有小0的通常说明混合料中CaO已经过多 石灰石调节要求减少石灰石配比
                //或者原料成分不合适
                return new Tuple<int, float, float>(-8013, 0, 0);
            }
        }
        /// <summary>
        /// 计算溶剂、燃料、白云石的配比
        /// </summary>
        /// <param name="C_Aim"></param>
        /// <param name="R_Aim"></param>
        /// <param name="R_md"></param>
        /// <param name="C_md"></param>
        /// <param name="Mg_Aim"></param>
        /// <param name="Mg_md"></param>
        /// <returns></returns>
        public Tuple<int, float, float, float> CptSolfuel(float C_Aim, float R_Aim, float R_md, float C_md, float Mg_Aim, float Mg_md)
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //返回结果
            Tuple<float, float> rs = new Tuple<float, float>(0, 0);

            float R_Aim_md = R_Aim + R_md;//目标碱度+R调整值
            float C_Aim_md = C_Aim + C_md;//目标含碳+C调整值
            float Mg_Aim_md = Mg_Aim + Mg_md;//目标含碳+Mg调整值


            float[] FuelAly = new float[30];          //燃料加权平均成分数组--包含30个成分
            float[] FluxAly = new float[30];          //熔剂加权平均成分数组--包含30个成分
            float[] DoloAly = new float[30];          //白云石加权平均成分数组--包含30个成分

            float sumCaO = 0;           //除熔剂、燃料调整仓以外原料带入烧结矿 CaO
            float sumSiO2 = 0;          //除熔剂、燃料调整仓以外原料带入烧结矿 SiO2
            float sumBill = 0;          //除熔剂、燃料调整仓以外原料总配比数
            float sumRemnant = 0;
            float sumC = 0;             //除熔剂、燃料调整仓以外原料带入烧结矿 C
            float sumMgO = 0;	        //除熔剂、燃料、白云石仓以外原料带入的 MgO
            float a1 = 0;               //燃料方程系数
            float b1 = 0;               //燃料方程系数
            float c1 = 0;		        //燃料方程系数
            float n1 = 0;               //燃料方程系数
            float a2 = 0;               //熔剂方程系数
            float b2 = 0;               //熔剂方程系数
            float c2 = 0;		        //燃料方程系数
            float n2 = 0;               //熔剂方程系数
            float a3 = 0;               //白云石方程系数
            float b3 = 0;               //白云石方程系数
            float c3 = 0;               //白云石方程系数
            float n3 = 0;		        //白云石方程系数
            float Flux_Bill = 0;        //熔剂调整仓计算配比
            float Fuel_Bill = 0;        //燃料调整仓计算配比
            float Dolo_Bill = 0;		//白云石调整仓计算配比

            float F_CaO = 0;            //燃料调整仓燃料有效CaO
            float F_SiO2 = 0;           //燃料调整仓燃料有效SiO2
            float F_MgO = 0;            //燃料调整仓有效MgO
            float L_CaO = 0;            //熔剂调整仓熔剂有效CaO
            float L_SiO2 = 0;           //熔剂调整仓熔剂有效SiO2
            float L_MgO = 0;            //熔剂调整仓有效MgO

            float D_CaO = 0;            //白云石调整仓有效CaO
            float D_SiO2 = 0;           //白云石调整仓有效SiO2
            float D_MgO = 0;            //白云石调整仓有效MgO

            float E = 0;                //中间量1
            float F = 0;                //中间量2
            float G = 0;                //中间量3
            float H = 0;                //中间量4

            //  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19
            // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO, ZnO    //宁钢检化验成分排序
            //	                                             烧损         灰分 挥发分      

            //20201127
            /*
             /  0,   1,   2,   3,     4,    5,  6, 7, 8, 9,  10,  11, 12,  13,  14,  15,   16,   17,  18,  19,  20, 21, 22, 23, 24, 25, 26, 27, 28,  29
            // TFe, FeO, CaO, SiO2, Al2O3, MgO, S, P, C, Mn, LOT, R, H2O, AsH, VOL, TiO2, K2O, Na2O, PbO,  ZnO, F, As, Cu, Pb, Zn, K,  Na, Cr, Ni, MnO  //检化验成分排序	
             */
            //20201127

            /*20191224先根据配比和仓号的对应关系，对每个料仓是否为燃料和熔剂进行标记,根据标记情况选出熔剂和燃料仓号*/
            /// <returns>
            /// item_1
            /// key:0、非燃料或溶剂  1、溶剂  2、燃料 3、烧返配比  4、白云石
            /// value:仓号，类别，物料二级编码，下料口，下料口启用标识，分仓系数和，成分
            /// item_2
            /// key:仓号(非燃料或溶剂 且 为启用状态)
            /// value:配比值
            /// </returns>
            var hrs = houseDataCRM();


            for (int i = 0; i < 30; i++)//根据选择出的熔剂和燃料仓号，求出燃料和熔剂的加权平均成分
            {
                //燃料加权平均成分计算  //qxg x.Item5

                //FuelAly[i] = ((Silo[燃料1仓号 - 1].aly[i] * Silo[燃料1仓号 - 1].use * Silo[燃料1仓号 - 1].allot) + (Silo[燃料2仓号 - 1].aly[i] * Silo[燃料2仓号 - 1].use * Silo[燃料2仓号 - 1].allot)) / (Silo[燃料1仓号 - 1].use * Silo[燃料1仓号 - 1].allot + Silo[燃料2仓号 - 1].use * Silo[燃料2仓号 - 1].allot);//有几个燃料仓，加权平均计算时就用几个仓
                //FluxAly[i] = ((Silo[熔剂1仓号 - 1].aly[i] * Silo[熔剂1仓号 - 1].use * Silo[熔剂1仓号 - 1].allot) + (Silo[熔剂2仓号 - 1].aly[i] * Silo[熔剂2仓号 - 1].use * Silo[熔剂2仓号 - 1].allot)) / (Silo[熔剂1仓号 - 1].use * Silo[熔剂1仓号 - 1].allot + Silo[熔剂2仓号 - 1].use * Silo[熔剂2仓号 - 1].allot);//有几个熔剂仓，加权平均计算时就用几个仓
                float sumFuelChild = 0;
                float sumFuelMother = 0;
                float sumFluxChild = 0;
                float sumFluxMother = 0;
                float sumDoloChild = 0;
                float sumDoloMother = 0;
                //20201127  不需要求加权平均  ，  只需要找出调整仓进行计算

                List<int> _class = new List<int>();
                var __rs = LGetLastTime("MC_MIXCAL_PAR");
                for (int w = 2; w <= 20; w++)
                {
                    _class.Add(int.Parse(__rs[w].ToString()));
                }
                int SolvTurn = 0;
                int FuelTurn = 0;
                int DolyTurn = 0;

                SolvTurn = _class.FindIndex(x => x == 2) + 1;
                FuelTurn = _class.FindIndex(x => x == 1) + 1;
                DolyTurn = _class.FindIndex(x => x == 4) + 1;



                for (int j = 0; j < hrs.Item1[2].Count; j++)
                {

                    if (hrs.Item1[2][j].Item1 == SolvTurn)
                    {
                        sumFuelChild += hrs.Item1[2][j].Item7[i] * hrs.Item1[2][j].Item5 * hrs.Item1[2][j].Item6;
                        //sumFuelMother += hrs.Item1[2][j].Item5 * hrs.Item1[2][j].Item6;
                        break;
                    }

                }
                FuelAly[i] = sumFuelChild;// / sumFuelMother;
                for (int j = 0; j < hrs.Item1[1].Count; j++)
                {
                    if (hrs.Item1[1][j].Item1 == FuelTurn)
                    {
                        sumFluxChild += hrs.Item1[1][j].Item7[i] * hrs.Item1[1][j].Item5 * hrs.Item1[1][j].Item6;
                        // sumFluxMother += hrs.Item1[1][j].Item5 * hrs.Item1[1][j].Item6;
                        break;
                    }

                }
                FluxAly[i] = sumFluxChild;// / sumFluxMother;

                //for (int j = 0; j < hrs.Item1[3].Count; j++)
                //{
                //    sumDoloChild += hrs.Item1[3][j].Item7[i] * hrs.Item1[3][j].Item5 * hrs.Item1[3][j].Item6;
                //    sumDoloMother += hrs.Item1[3][j].Item5 * hrs.Item1[3][j].Item6;
                //}
                //修改类别标志位 标志位3为4（白云石）
                for (int j = 0; j < hrs.Item1[4].Count; j++)
                {
                    if (hrs.Item1[4][j].Item1 == DolyTurn)
                    {
                        sumDoloChild += hrs.Item1[4][j].Item7[i] * hrs.Item1[4][j].Item5 * hrs.Item1[4][j].Item6;
                        // sumDoloMother += hrs.Item1[4][j].Item5 * hrs.Item1[4][j].Item6;
                        break;
                    }
                }

                DoloAly[i] = sumDoloChild;// / sumDoloMother;
            }

            //
            foreach (var x in hrs.Item2)//仓，配比值
            {
                foreach (var y in hrs.Item1[0])//非溶剂或燃料仓
                {
                    if (y.Item1.ToString() == x.Key.ToString())
                    {
                        if (y.Item7[8] > 30 && y.Item7[13] > 5)//单位？/////////////////////////////
                        {
                            //Silo[i].bill_sp：为第i个仓的设定配比，需要将多个仓共用一个配比的情况，提前按照料仓进行拆分计算；
                            //x.Item5：为第i个仓的启用状态，需要对一个仓两个下料口的情况，进行单独判断；

                            sumCaO += x.Value * y.Item7[2] / 100 * y.Item7[13] / 100;
                            sumSiO2 += x.Value * y.Item7[3] / 100 * y.Item7[13] / 100;
                            sumBill += x.Value;//除 石灰石X1,燃料X2 以外原料总配比数
                            sumRemnant += x.Value * y.Item7[13] / 100;
                            sumC += x.Value * y.Item7[8] / 100;
                            sumMgO += x.Value * y.Item7[5] / 100 * y.Item7[13] / 100;
                        }
                        else
                        {
                            sumCaO += x.Value * y.Item7[2] / 100;
                            sumSiO2 += x.Value * y.Item7[3] / 100;
                            sumBill += x.Value;
                            sumRemnant += x.Value * (100 - y.Item7[10]) / 100;
                            sumC += x.Value * y.Item7[8] / 100;
                            sumMgO += x.Value * y.Item7[5] / 100;

                        }
                        break;
                    }
                }

                //20200528 增加烧饭配比
                foreach (var y in hrs.Item1[3])//非溶剂或燃料仓
                {
                    if (y.Item1.ToString() == x.Key.ToString())
                    {
                        if (y.Item7[8] > 30 && y.Item7[13] > 5)//单位？/////////////////////////////
                        {
                            //Silo[i].bill_sp：为第i个仓的设定配比，需要将多个仓共用一个配比的情况，提前按照料仓进行拆分计算；
                            //x.Item5：为第i个仓的启用状态，需要对一个仓两个下料口的情况，进行单独判断；

                            sumCaO += x.Value * y.Item7[2] / 100 * y.Item7[13] / 100;
                            sumSiO2 += x.Value * y.Item7[3] / 100 * y.Item7[13] / 100;
                             sumBill += x.Value;//除 石灰石X1,燃料X2 以外原料总配比数

                            //20210204
                          // sumBill += x.Value / 2;//除 石灰石X1,燃料X2 以外原料总配比数
                            sumRemnant += x.Value * y.Item7[13] / 100;
                            sumC += x.Value * y.Item7[8] / 100;
                            sumMgO += x.Value * y.Item7[5] / 100 * y.Item7[13] / 100;
                        }
                        else
                        {
                            sumCaO += x.Value * y.Item7[2] / 100;
                            sumSiO2 += x.Value * y.Item7[3] / 100;
                            sumBill += x.Value ;
                            //20210204
                         // sumBill += x.Value / 2;
                            sumRemnant += x.Value * (100 - y.Item7[10]) / 100;
                            sumC += x.Value * y.Item7[8] / 100;
                            sumMgO += x.Value * y.Item7[5] / 100;

                        }
                        break;
                    }
                }

            }

            ////碱度调整仓原料的含碳量,工艺上不可能大于等于目标含碳量

            if (FluxAly[8] / 100 - C_Aim_md / 100 > -0.01f) return new Tuple<int, float, float, float>(-9011, 0, 0, 0); //碱度调整仓原料的含碳量,工艺上不可能大于等于目标含碳量；

            F_CaO = FuelAly[2] / 100 * FuelAly[13] / 100;
            F_SiO2 = FuelAly[3] / 100 * FuelAly[13] / 100;
            F_MgO = FuelAly[5] / 100 * FuelAly[13] / 100;

            L_CaO = FluxAly[2] / 100;
            L_SiO2 = FluxAly[3] / 100;
            L_MgO = FluxAly[5] / 100;

            D_CaO = DoloAly[2] / 100;
            D_SiO2 = DoloAly[3] / 100;
            D_MgO = DoloAly[5] / 100;
            //燃料配比方程
            a1 = FluxAly[8] / 100 - C_Aim_md / 100;
            b1 = FuelAly[8] / 100 - C_Aim_md / 100;
            n1 = C_Aim_md / 100 * sumBill - sumC;
            c1 = DoloAly[8] / 100 - C_Aim_md / 100;
            //灰石配比方程
            a2 = L_CaO - L_SiO2 * R_Aim_md;
            b2 = F_CaO - F_SiO2 * R_Aim_md;
            c2 = D_CaO - D_SiO2 * R_Aim_md;
            n2 = sumSiO2 * R_Aim_md - sumCaO;
            // 白云石配比方程

            a3 = L_MgO - Mg_Aim_md / 100 * ((100 - FluxAly[10]) / 100);
            b3 = F_MgO - Mg_Aim_md / 100 * (FuelAly[13] / 100);
            c3 = D_MgO - Mg_Aim_md / 100 * ((100 - DoloAly[10]) / 100);
            n3 = Mg_Aim_md / 100 * sumRemnant - sumMgO;

            if ((a2 * b1 - a1 * b2 == 0) || (b3 * a1 - b1 * a3 == 0)) return new Tuple<int, float, float, float>(-9012, 0, 0, 0); //确保分母不为零，保证方程有解

            E = (c2 * a1 - c1 * a2) / (b2 * a1 - b1 * a2);
            F = (n2 * a1 - n1 * a2) / (b2 * a1 - b1 * a2);
            G = (c3 * a1 - c1 * a3) / (b3 * a1 - b1 * a3);
            H = (n3 * a1 - n1 * a3) / (b3 * a1 - b1 * a3);
            if ((G - E) == 0) return new Tuple<int, float, float, float>(-9013, 0, 0, 0); //计算配比时，分母不能为0，保证方程有解

            Flux_Bill = (n1 * (G - E) - b1 * (G * F - H * E) - c1 * (H - F)) / (a1 * (G - E));
            Fuel_Bill = (G * F - H * E) / (G - E);
            Dolo_Bill = (H - F) / (G - E);


            if ((Flux_Bill > 0) && (Fuel_Bill > 0) && (Dolo_Bill > 0))
            {
                return new Tuple<int, float, float, float>(0, Flux_Bill, Fuel_Bill, Dolo_Bill);
            }
            else
            {
                //配比输入错误
                //或者原料成分不合适
                return new Tuple<int, float, float, float>(-9014, 0, 0, 0);
            }
        }


        /// <summary>
        /// 新增模型数据采集
        /// </summary>
        /// <returns>
        /// item1:方法执行是否成功
        /// item2:采集数据集
        /// </returns>
        //public Tuple<bool, List<float>> ModifyData()
        //{
        //    //初始化数据库
        //    DBSQL _mdb = new DBSQL(_connstring);
        //    //string sql = "select Top(1) MAT_L2_DQPB_1,MAT_L2_DQPB_2,MAT_L2_DQPB_3,MAT_L2_DQPB_4,MAT_L2_DQPB_5," +
        //    //            "MAT_L2_DQPB_6,MAT_L2_DQPB_7,MAT_L2_DQPB_8,MAT_L2_MB_C,MAT_L2_MB_R,MAT_L2_MB_MG," +
        //    //            "MAT_L2_TZ_R,MAT_L2_TZ_C,MAT_L2_TZ_MG,MAT_L2_ZLL_SP,MAT_L2_ZLL_PV from M_MACAL_INTERFACE_RESULT where MAT_L2_FLAG = 1";
        //    string sql = "select Top(1) MAT_L2_DQPB_2,MAT_L2_DQPB_1,MAT_L2_DQPB_3,MAT_L2_DQPB_4,MAT_L2_DQPB_5," +
        //             "MAT_L2_DQPB_6,MAT_L2_DQPB_7,MAT_L2_DQPB_8,MAT_L2_MB_C,MAT_L2_MB_R,MAT_L2_MB_MG," +
        //             "MAT_L2_TZ_R,MAT_L2_TZ_C,MAT_L2_TZ_MG,MAT_L2_ZLL_SP,MAT_L2_ZLL_PV from M_MACAL_INTERFACE_RESULT where MAT_L2_FLAG = 1";

        //    DataTable _dt = _mdb.GetCommand(sql);
        //    if (_dt != null)
        //    {
        //        List<float> rs = new List<float>();
        //        var _vdt = _dt.AsEnumerable();
        //        int cols = _vdt.ElementAt(0).ItemArray.Count();
        //        for (int i = 0; i < cols; i++)
        //        {
        //            rs.Add(float.Parse(_vdt.ElementAt(0)[i].ToString()==""?"0": _vdt.ElementAt(0)[i].ToString()));
        //        }
        //        return new Tuple<bool, List<float>>(true, rs);
        //    }
        //    else
        //    {
        //        return new Tuple<bool, List<float>>(false, null);
        //    }

        //}
        /// <summary>
        /// 新增模型数据采集
        /// </summary>
        /// <returns>
        /// item1:方法执行是否成功
        /// item2:采集数据集
        /// </returns>
        public Tuple<bool, List<float>> ModifyData()//20200911 李涛修改
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            //string sql = "select Top(1) MAT_L2_DQPB_1,MAT_L2_DQPB_2,MAT_L2_DQPB_3,MAT_L2_DQPB_4,MAT_L2_DQPB_5," +
            //            "MAT_L2_DQPB_6,MAT_L2_DQPB_7,MAT_L2_DQPB_8,MAT_L2_MB_C,MAT_L2_MB_R,MAT_L2_MB_MG," +
            //            "MAT_L2_TZ_R,MAT_L2_TZ_C,MAT_L2_TZ_MG,MAT_L2_ZLL_SP,MAT_L2_ZLL_PV from M_MACAL_INTERFACE_RESULT where MAT_L2_FLAG = 1";
            //20200905新增字段
            //string sql = "select Top(1) MAT_L2_DQPB_2,MAT_L2_DQPB_1,MAT_L2_DQPB_3,MAT_L2_DQPB_4,MAT_L2_DQPB_5," +
            //         "MAT_L2_DQPB_6,MAT_L2_DQPB_7,MAT_L2_DQPB_8,MAT_L2_MB_C,MAT_L2_MB_R,MAT_L2_MB_MG," +
            //         "MAT_L2_TZ_R,MAT_L2_TZ_C,MAT_L2_TZ_MG,MAT_L2_ZLL_SP,MAT_L2_ZLL_PV from M_MACAL_INTERFACE_RESULT where MAT_L2_FLAG = 1";
            string sql = @"select Top(1) MAT_L2_CODE_1,
                           MAT_L2_CODE_2,
                           MAT_L2_CODE_3,
                           MAT_L2_CODE_4,
                           MAT_L2_CODE_5,
                           MAT_L2_CODE_6,
                           MAT_L2_CODE_7,
                           MAT_L2_CODE_8,
                           MAT_L2_CODE_9,
                           MAT_L2_CODE_10,
                           MAT_L2_CODE_11,
                           MAT_L2_CODE_12,
                           MAT_L2_CODE_13,
                           MAT_L2_CODE_14,
                           MAT_L2_CODE_15,
                           MAT_L2_CODE_16,
                           MAT_L2_CODE_17,
                           MAT_L2_CODE_18,
                           MAT_L2_CODE_19,
                           MAT_L2_CODE_20,
                           MAT_L2_DQPB_1,
                           MAT_L2_DQPB_2,
                           MAT_L2_DQPB_3,
                           MAT_L2_DQPB_4,
                           MAT_L2_DQPB_5,
                           MAT_L2_DQPB_6,
                           MAT_L2_DQPB_7,
                           MAT_L2_DQPB_8,
                           MAT_L2_DQPB_9,
                           MAT_L2_DQPB_10,
                           MAT_L2_DQPB_11,
                           MAT_L2_DQPB_12,
                           MAT_L2_DQPB_13,
                           MAT_L2_DQPB_14,
                           MAT_L2_DQPB_15,
                           MAT_L2_DQPB_16,
                           MAT_L2_DQPB_17,
                           MAT_L2_DQPB_18,
                           MAT_L2_DQPB_19,
                           MAT_L2_MB_C,MAT_L2_MB_R,MAT_L2_MB_MG,
                           MAT_L2_TZ_R,MAT_L2_TZ_C,MAT_L2_TZ_MG,MAT_L2_ZLL_SP,MAT_L2_ZLL_PV,
                           MAT_L2_PBBFB_1,
                           MAT_L2_PBBFB_2,
                           MAT_L2_PBBFB_3,
                           MAT_L2_PBBFB_4,
                           MAT_L2_PBBFB_5,
                           MAT_L2_PBBFB_6,
                           MAT_L2_PBBFB_7,
                           MAT_L2_PBBFB_8,
                           MAT_L2_PBBFB_9,
                           MAT_L2_PBBFB_10,
                           MAT_L2_PBBFB_11,
                           MAT_L2_PBBFB_12,
                           MAT_L2_PBBFB_13,
                           MAT_L2_PBBFB_14,
                           MAT_L2_PBBFB_15,
                           MAT_L2_PBBFB_16,
                           MAT_L2_PBBFB_17,
                           MAT_L2_PBBFB_18,
                           MAT_L2_PBBFB_19
                           from M_MACAL_INTERFACE_RESULT where MAT_L2_FLAG = 1";
            //C_MAT_PLC_1MIn表
            string sql_1 = "select top (1) MAT_PLC_PV_W_1,MAT_PLC_PV_W_2,MAT_PLC_PV_W_17,MAT_PLC_PV_W_18 from C_MAT_PLC_1MIN order by TIMESTAMP desc";
            DataTable data = _mdb.GetCommand(sql_1);
            DataTable _dt = _mdb.GetCommand(sql);
            if (_dt != null)
            {
                if (data != null)
                {
                    List<float> rs = new List<float>();
                    var _vdt = _dt.AsEnumerable();
                    int cols = _vdt.ElementAt(0).ItemArray.Count();
                    for (int i = 0; i < cols; i++)
                    {
                        rs.Add(float.Parse(_vdt.ElementAt(0)[i].ToString() == "" ? "0" : _vdt.ElementAt(0)[i].ToString()));
                    }
                    for (int x = 0; x < 4; x++)
                    {
                        rs.Add(float.Parse(data.Rows[0][x].ToString() == "" ? "0" : data.Rows[0][x].ToString()));
                    }
                    return new Tuple<bool, List<float>>(true, rs);
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

        //20200914 修改逻辑
        /// <summary>
        /// 加样判断
        /// </summary>
        /// <param name="vStrBatch">批次号</param>
        /// <param name="mode">1:FeO 2:R 3:MgO</param>
        /// <returns>
        /// item1:1:加样 非1 不加样
        /// item2: 当前检测值
        /// item3: 上次检测值
        /// </returns>
        public Tuple<int, float, float> SamplePlus(string vStrBatch, int mode)
        {

            float Cur_Test = 0;//本次检测值
            float Last_Test = 0;//上次检测值
            if (vStrBatch == null || vStrBatch.Length < 10)
            {
                return new Tuple<int, float, float>(-1, 0, 0);

            }
            else
            {
                if (vStrBatch[9] % 2 != 0)
                {
                    //初始化数据库
                    DBSQL _mdb = new DBSQL(_connstring);
                    string segment = mode == 1 ? " isnull(C_FEO, 0) " :
                        mode == 3 ? " isnull(C_MGO, 0) " :
                        " isnull(C_R, 0) ";
                    string whereSeg = mode == 1 ? " C_FEO!=0 and C_FEO is not null " :
                     mode == 3 ? " C_MGO!=0 and C_MGO is not null " :
                    " C_R!=0 and C_R is not null ";
                    string _sql0 = "select Top(2) " + segment + " from M_SINTER_ANALYSIS where flag>0 and " + whereSeg + "  order by TIMESTAMP desc";
                    var _dt0 = _mdb.GetCommand(_sql0);
                    if (_dt0 == null)
                    {
                        return new Tuple<int, float, float>(-2, 0, 0);
                    }
                    else
                    {
                        var _vdt0 = _dt0.AsEnumerable();
                        if (_vdt0.Count() <= 0)
                        {
                            mixlog.writelog("M_SINTER_ANALYSIS 没有满足条件的数据", 1);
                            return new Tuple<int, float, float>(1, 0, 0);
                        }
                        else
                        {
                            if (_vdt0.Count() == 1)
                            {
                                Cur_Test = float.Parse(_vdt0.ElementAt(0)[0].ToString());
                                Last_Test = Cur_Test;
                            }
                            else if (_vdt0.Count() == 2)
                            {
                                Cur_Test = float.Parse(_vdt0.ElementAt(0)[0].ToString());
                                Last_Test = float.Parse(_vdt0.ElementAt(1)[0].ToString());
                            }
                            return new Tuple<int, float, float>(1, Cur_Test, Last_Test);
                        }

                    }

                }
                else
                {
                    return new Tuple<int, float, float>(0, 0, 0);
                }

            }
        }





        /// <summary>
        /// 获取X表最近一条记录，X表时间字段默认为timestamp
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="TimeSegment">时间字段名</param>
        /// <returns>
        /// 最近一条记录
        /// 异常：null
        /// </returns>
        public DataRow LGetLastTime(string TableName, string TimeSegment = "timestamp")
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            string str = "select Top(1) * from " + TableName + " order by " + TimeSegment + " desc";
            DataTable _dt = _mdb.GetCommand(str);
            var _vdt = _dt.AsEnumerable();
            if (_vdt.Count() <= 0)
            {
                return null;
            }
            else
            {
                return _vdt.ElementAt(0);
            }

        }
        /// <summary>
        /// 计算时间前n条数据的某些字段的平均值
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="SegMents">字段</param>
        /// <param name="vNum">条数</param>
        /// <returns></returns>
        public DataRow LGetTimesAvg(string TableName, string SegMents, int vNum, string vWhere = " ")
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);
            string[] strarr = SegMents.Split(',');
            string AvgSegMents = "";
            foreach (var x in strarr)
            {
                AvgSegMents += "AVG(" + x + "),";
            }
            AvgSegMents = AvgSegMents.Substring(0, AvgSegMents.Length - 1);
            string sql = "select " + AvgSegMents + " from (select Top(" + vNum + ") " + SegMents + " from " + TableName + vWhere + " order by timestamp desc) as t1";
            DataTable _dt = _mdb.GetCommand(sql);
            if (_dt != null)
            {
                var _vdt = _dt.AsEnumerable();
                return _vdt.ElementAt(0);
            }
            return null;
        }


        /// <summary>
        /// 界面滚动消息插入
        /// </summary>
        /// <param name="modename">模块名称</param>
        /// <param name="funcname">方法名称</param>
        /// <param name="info">插入内容</param>
        /// <param name="flag">1</param>
        /// <param name="flagAlarm">0</param>
        public void InsertLogTable(string modename, string funcname, string info, int flag = 1, int flagAlarm = 0)
        {
            //初始化数据库
            DBSQL _mdb = new DBSQL(_connstring);

            string _inssql = "insert into LogTable values('" + DateTime.Now + "','" + modename + "','" + funcname + "','" + info + "'," + 0 + "," + flag + "," + flagAlarm + ")";

            int rs = _mdb.CommandExecuteNonQuery(_inssql);
            if (rs > 0)
            {
                mixlog.writelog("插入LogTable成功(" + _inssql + ")", 0);
            }
            else
            {
                mixlog.writelog("插入LogTable失败(" + _inssql + ")", 0);
            }

        }

    }

}
