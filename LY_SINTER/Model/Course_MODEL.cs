using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY_SINTER.Model
{
   
    public class Point3D
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }
    class Course_MODEL
    {
        DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        //烧结终点曲线
        /// <summary>
        /// 烧结终点3D***
        /// StartZloc,  EndZloc闸门边界
        /// StartFXlocation,EndFXlocation  开始及结束风箱位置
        /// XLocation //6个闸门位置
        /// YLocation //温度轴
        /// ZLocation //10个风箱热电偶坐标
        /// RDOSize //风箱热电偶坐标之间差值
        /// data_3d //温度datatable
        /// Step //步频自己设定
        /// initCount //初始曲线条数
        /// D_value //差值倍数
        /// </summary>
        public List<Point3D> Refresh_Data(double StartZloc, double EndZloc, double StartFXlocation, double EndFXlocation, double[] XLocation, double[] YLocation, double[] ZLocation, double[] RDOSize, DataTable data_3d, double Step, int initCount, int D_value)
        {

            int Zcount; //目标曲线条数
            List<Point3D> Data_3D_list = new List<Point3D>();
            /// <summary>
            /// z轴分割后的数组列表
            /// </summary>
            List<double> ZLocationArr = new List<double>();
            List<double> XLocationArr = new List<double>();
            int TargetCount = initCount * D_value;
            Decimal tempdistanceX = 0;
            double StepX = 0;
            if (XLocation.Length > 0)
            {

                StepX = (double)(XLocation[XLocation.Length - 1] - XLocation[0]) / (initCount - 1) / D_value;
            }

            int Xmaxcount = Convert.ToInt32(5 / StepX);
            XLocationArr.Add(StartZloc);//加上边界

            for (int i = 0; i < initCount-1; i++)
            {
                //开始职位
                double x = XLocation[i + 1] - XLocation[i];
                for (int y = 0; y < D_value ; y++)
                {
                    XLocationArr.Add((double)XLocation[i] + (x/ D_value) * y);
                }
                //结束位置
            }

            //XLocationArr.Add(0.4);
            //XLocationArr.Add(0.75);
            //XLocationArr.Add(1.1);
            //XLocationArr.Add(1.45);
            //XLocationArr.Add(1.8);
            //XLocationArr.Add(2.2);
            //XLocationArr.Add(2.6);
            //XLocationArr.Add(2.95);
            //XLocationArr.Add(3.3);
            //XLocationArr.Add(3.65);
            //XLocationArr.Add(4);
            //for (int i = 0; i < initCount; i++)
            //{
            //    //X分割
            //    if (i == (XLocation.Length - 1))
            //    {
            //        XLocationArr.Add(XLocation[i]);
            //    }
            //    else
            //    {
            //        for (int num = 0; num < Xmaxcount; num++)
            //        {
            //            tempdistanceX = (Decimal)(XLocation[i] + StepX * num);
            //            if (tempdistanceX >= (Decimal)XLocation[i + 1])
            //            {
            //                break;
            //            }
            //            XLocationArr.Add((double)tempdistanceX);
            //        }
            //    }
            //}
            XLocationArr.Add(XLocation[initCount - 1]);
            XLocationArr.Add(EndZloc);//加上边界
            
            
          
            //ZLocationArr.Clear();
            int Zmaxcount = Convert.ToInt32((EndFXlocation - StartFXlocation) / Step);
            double tempdistanceZ = 0;
            for (int i = 0; i < ZLocation.Length; i++)
            {
                if (i == (ZLocation.Length - 1))
                {
                    ZLocationArr.Add(ZLocation[i]);
                }
                else
                {
                    for (int num = 0; num < Zmaxcount; num++)
                    {
                        tempdistanceZ = ZLocation[i] + Step * num;
                        if (tempdistanceZ >= ZLocation[i + 1])
                        {
                            break;
                        }
                        ZLocationArr.Add(tempdistanceZ);
                    }
                }
            }

            Zcount = Convert.ToInt32((EndFXlocation - StartFXlocation) / Step);
            Zcount = ZLocationArr.Count;

            //计算两个热电偶之间显示数据的个数
            for (int i = 0; i < RDOSize.Length; i++)
            {
                RDOSize[i] = RDOSize[i] / Step;
            }
            try
            {
                Data_3D_list.Clear();
                double[] z = ZLocationArr.ToArray();
                double[,] TempDoubleArray = new double[initCount, Zcount];
                for (int num = 0; num < initCount; num++)
                {
                    int SecondLocation = 0;

                    for (int i = 0; i < RDOSize.Length; i++)
                    {
                        if (data_3d.Rows[num][i + 1] != DBNull.Value && data_3d.Rows[num][i] != DBNull.Value)
                        {
                            double tempValueCz = 0;
                            tempValueCz = (Convert.ToDouble(data_3d.Rows[num][i + 1]) - Convert.ToDouble(data_3d.Rows[num][i])) / RDOSize[i];
                            for (int q = 0; q < RDOSize[i]; q++)
                            {
                                TempDoubleArray[num, SecondLocation++] = Convert.ToDouble(data_3d.Rows[num][i]) + q * tempValueCz;
                            }
                        }
                    }
                    if (data_3d.Rows[num][RDOSize.Length] != DBNull.Value)
                    {
                        TempDoubleArray[num, SecondLocation] = Convert.ToDouble(data_3d.Rows[num][RDOSize.Length]);
                    }
                }

                //创建一个临时数组
                double[,] TargetDoubleArray = new double[TargetCount + 1, Zcount];
                //条数循环
                //for (int X_Index = (initCount - 1); X_Index >= 0; X_Index--)
                for (int X_Index = 0; X_Index < initCount; X_Index++)
                {

                    if (X_Index == 0)
                    {
                        //差值循环
                        for (int d = 0; d < D_value; d++)
                        {
                            //y轴循环
                            for (int Y_Index = 0; Y_Index < Zcount; Y_Index++)
                            {
                                TargetDoubleArray[X_Index + d, Y_Index] = TempDoubleArray[0, Y_Index];
                            }
                        }
                    }
                    else if (X_Index == initCount - 1)
                    {
                        for (int d = 0; d < D_value; d++)
                        {
                            //y轴循环
                            for (int Y_Index = 0; Y_Index < Zcount; Y_Index++)
                            {
                                TargetDoubleArray[X_Index * D_value + d, Y_Index] = TempDoubleArray[(initCount - 1), Y_Index];
                            }
                        }
                        for (int Y_Index = 0; Y_Index < Zcount; Y_Index++)
                        {
                            TargetDoubleArray[initCount * D_value, Y_Index] = TempDoubleArray[(initCount - 1), Y_Index];
                        }
                    }
                    else
                    {

                        //y轴循环
                        for (int Y_Index = 0; Y_Index < Zcount; Y_Index++)
                        {
                            //计算差值增量
                            double tempValue = (TempDoubleArray[X_Index + 1, Y_Index] - TempDoubleArray[X_Index, Y_Index]) / Convert.ToDouble(D_value);

                            for (int d = 0; d < D_value; d++)
                            {
                                TargetDoubleArray[X_Index * D_value + d, Y_Index] = TempDoubleArray[X_Index, Y_Index] + d * tempValue;
                            }
                        }
                    }
                }
                for (int i = 0; i < XLocationArr.Count; i++)
                {

                    for (int j = 0; j < Zcount; j++)
                    {

                        Point3D dd = new Point3D();
                        dd.x = XLocationArr[i];
                        //把Y打散
                        dd.y = z[j];
                        dd.z = Math.Round(TargetDoubleArray[i, j], 2);
                        Data_3D_list.Add(dd);
                    }
                }


            }
            catch (Exception ee)
            {
                throw;
            }
            return Data_3D_list;
        }

        /// <summary>
        /// 高次曲线数据准备(柱形图数据)
        /// </summary>
        /// <returns></returns>
        public List<double> Higher_Order_Date_1()
        {
            try
            {
                var sql_3 = "select top (1) " +
              "(SIN_PLC_B01_TE_L_1+SIN_PLC_B01_TE_L_2)/2 AS DOT_1," +
              "(SIN_PLC_B02_TE_L_1+SIN_PLC_B02_TE_L_2)/2 AS DOT_2," +
              "(SIN_PLC_B03_TE_L_1+SIN_PLC_B03_TE_L_2)/2 AS DOT_3," +
              "(SIN_PLC_B04_TE_L_1+SIN_PLC_B04_TE_L_2)/2 AS DOT_4," +
              "(SIN_PLC_B05_TE_L_1+SIN_PLC_B05_TE_L_2)/2 AS DOT_5," +
              "(SIN_PLC_B06_TE_L_1+SIN_PLC_B06_TE_L_2)/2 AS DOT_6," +
              "(SIN_PLC_B07_TE_L_1+SIN_PLC_B07_TE_L_2)/2 AS DOT_7," +
              "(SIN_PLC_B08_TE_L_1+SIN_PLC_B08_TE_L_2)/2 AS DOT_8," +
              "(SIN_PLC_B09_TE_L_1+SIN_PLC_B09_TE_L_2)/2 AS DOT_9," +
              "(SIN_PLC_B10_TE_L_1+SIN_PLC_B10_TE_L_2)/2  AS DOT_10," +
              "(SIN_PLC_B11_TE_L_1+SIN_PLC_B11_TE_L_2)/2  AS DOT_11," +
              "(SIN_PLC_B12_TE_L_1+SIN_PLC_B12_TE_L_2)/2 AS DOT_12," +
              "(SIN_PLC_B13_TE_L_1+SIN_PLC_B13_TE_L_2)/2 AS DOT_13," +
              "(SIN_PLC_B14_TE_L_1+SIN_PLC_B14_TE_L_2+SIN_PLC_B14_TE_L_3+SIN_PLC_B14_TE_L_4+SIN_PLC_B14_TE_L_5+SIN_PLC_B14_TE_L_6)/6 AS DOT_14," +
              "(SIN_PLC_B15_TE_L_1+SIN_PLC_B15_TE_L_2+SIN_PLC_B15_TE_L_3+SIN_PLC_B15_TE_L_4+SIN_PLC_B15_TE_L_5+SIN_PLC_B15_TE_L_6)/6 AS DOT_15," +
              "(SIN_PLC_B16_TE_L_1+SIN_PLC_B16_TE_L_2+SIN_PLC_B16_TE_L_3+SIN_PLC_B16_TE_L_4+SIN_PLC_B16_TE_L_5+SIN_PLC_B16_TE_L_6)/6 AS DOT_16," +
              "(SIN_PLC_B17_TE_L_1+SIN_PLC_B17_TE_L_2+SIN_PLC_B17_TE_L_3+SIN_PLC_B17_TE_L_4+SIN_PLC_B17_TE_L_5+SIN_PLC_B17_TE_L_6)/6 AS DOT_17," +
              "(SIN_PLC_B18_TE_L_1+SIN_PLC_B18_TE_L_2+SIN_PLC_B18_TE_L_3+SIN_PLC_B18_TE_L_4+SIN_PLC_B18_TE_L_5+SIN_PLC_B18_TE_L_6)/6 AS DOT_18," +
              "(SIN_PLC_B19_TE_L_1+SIN_PLC_B19_TE_L_2+SIN_PLC_B19_TE_L_3+SIN_PLC_B19_TE_L_4+SIN_PLC_B19_TE_L_5+SIN_PLC_B19_TE_L_6)/6 AS DOT_19," +
              "(SIN_PLC_B20_TE_L_1+SIN_PLC_B20_TE_L_2+SIN_PLC_B20_TE_L_3+SIN_PLC_B20_TE_L_4+SIN_PLC_B20_TE_L_5+SIN_PLC_B20_TE_L_6)/6 AS DOT_20," +
              "(SIN_PLC_B21_TE_L_1+SIN_PLC_B21_TE_L_2+SIN_PLC_B21_TE_L_3+SIN_PLC_B21_TE_L_4+SIN_PLC_B21_TE_L_5+SIN_PLC_B21_TE_L_6)/6 AS DOT_21," +
              "(SIN_PLC_B22_TE_L_1+SIN_PLC_B22_TE_L_2+SIN_PLC_B22_TE_L_3+SIN_PLC_B22_TE_L_4+SIN_PLC_B22_TE_L_5+SIN_PLC_B22_TE_L_6)/6 AS DOT_22  " +
              "from C_SIN_PLC_1MIN ORDER BY TIMESTAMP DESC";
                DataTable dataTable_3 = _dBSQL.GetCommand(sql_3);
                if (dataTable_3.Rows.Count > 0 && dataTable_3 != null)
                {
                    List<double> _G = new List<double>();
                    for (int x = 0; x < dataTable_3.Columns.Count;x++)
                    {
                        _G.Add(Math.Round(double.Parse(dataTable_3.Rows[0][x].ToString()),2));
                    }
                    return _G;
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

        /// <summary>
        /// 高次曲线数据准备(点状图)
        /// _Degree:拟合次数
        /// _DIG:小数位数
        /// </summary>
        /// <returns></returns>
        public List<double> Higher_Order_Date_2(int _Degree,int _DIG)
        {
            try
            {
                List<double> _G = new List<double>();//返回值
                #region 方程系数
                //平均风箱曲线-方程常数值
                float T_0 = 0;
                // 平均风箱曲线 - 方程1次系数
                float T_1 = 0;
                // 平均风箱曲线 - 方程2次系数
                float T_2 = 0;
                // 平均风箱曲线 - 方程3次系数
                float T_3 = 0;
                // 平均风箱曲线 - 方程4次系数
                float T_4 = 0;
                // 平均风箱曲线 - 方程5次系数
                float T_5 = 0;
                // 平均风箱曲线 - 方程6次系数
                float T_6 = 0;
                // 平均风箱曲线 - 方程7次系数
                float T_7 = 0;
                // 平均风箱曲线 - 方程8次系数
                float T_8 = 0;
                #endregion
                #region 热电偶系数
                //1#热电偶米数
                float DOT_1 = 0;
                //2#热电偶米数
                float DOT_2 = 0;
                //3#热电偶米数
                float DOT_3 = 0;
                //4#热电偶米数
                float DOT_4 = 0;
                //5#热电偶米数
                float DOT_5 = 0;
                //6#热电偶米数
                float DOT_6 = 0;
                //7#热电偶米数
                float DOT_7 = 0;
                //8#热电偶米数
                float DOT_8 = 0;
                //9#热电偶米数
                float DOT_9 = 0;
                //10#热电偶米数
                float DOT_10 = 0;
                //11#热电偶米数
                float DOT_11 = 0;
                //12#热电偶米数
                float DOT_12 = 0;
                //13#热电偶米数
                float DOT_13 = 0;
                //14#热电偶米数
                float DOT_14 = 0;
                //15#热电偶米数
                float DOT_15 = 0;
                //16#热电偶米数
                float DOT_16 = 0;
                //17#热电偶米数
                float DOT_17 = 0;
                //18#热电偶米数
                float DOT_18 = 0;
                //19#热电偶米数
                float DOT_19 = 0;
                //20#热电偶米数
                float DOT_20 = 0;
                //21#热电偶米数
                float DOT_21 = 0;
                //22#热电偶米数
                float DOT_22 = 0;
                #endregion
                //获取常数项的系数的数值
                string sql_2 = "select top (1) BTPCAL_LINEROOT0_9,BTPCAL_LINEROOT1_9,BTPCAL_LINEROOT2_9,BTPCAL_LINEROOT3_9,BTPCAL_LINEROOT4_9,BTPCAL_LINEROOT5_9,BTPCAL_LINEROOT6_9,BTPCAL_LINEROOT7_9,BTPCAL_LINEROOT8_9 from MC_BTPCAL_LINE_PAR_result order by TIMESTAMP desc";
                DataTable dataTable_2 = _dBSQL.GetCommand(sql_2);
                if (dataTable_2 != null && dataTable_2.Rows.Count > 0 )
                {
                    //平均风箱曲线-方程常数值
                    T_0 = float.Parse(dataTable_2.Rows[0]["BTPCAL_LINEROOT0_9"].ToString());
                    // 平均风箱曲线 - 方程1次系数
                    T_1 = float.Parse(dataTable_2.Rows[0]["BTPCAL_LINEROOT1_9"].ToString());
                    // 平均风箱曲线 - 方程2次系数
                    T_2 = float.Parse(dataTable_2.Rows[0]["BTPCAL_LINEROOT2_9"].ToString());
                    // 平均风箱曲线 - 方程3次系数
                    T_3 = float.Parse(dataTable_2.Rows[0]["BTPCAL_LINEROOT3_9"].ToString());
                    // 平均风箱曲线 - 方程4次系数
                    T_4 = float.Parse(dataTable_2.Rows[0]["BTPCAL_LINEROOT4_9"].ToString());
                    // 平均风箱曲线 - 方程5次系数
                    T_5 = float.Parse(dataTable_2.Rows[0]["BTPCAL_LINEROOT5_9"].ToString());
                    // 平均风箱曲线 - 方程6次系数
                    T_6 = float.Parse(dataTable_2.Rows[0]["BTPCAL_LINEROOT6_9"].ToString());
                    // 平均风箱曲线 - 方程7次系数
                    T_7 = float.Parse(dataTable_2.Rows[0]["BTPCAL_LINEROOT7_9"].ToString());
                    // 平均风箱曲线 - 方程8次系数
                    T_8 = float.Parse(dataTable_2.Rows[0]["BTPCAL_LINEROOT8_9"].ToString());

                    //获取热点偶米数系数
                    string sql_4 = "select top 1 " +
                                  "BTPCAL_BTC01M0101 as DOT_1," +
                                  "BTPCAL_BTC01M0201 as DOT_2," +
                                  "BTPCAL_BTC01M0301 as DOT_3," +
                                  "BTPCAL_BTC01M0401 as DOT_4," +
                                  "BTPCAL_BTC01M0501 as DOT_5," +
                                  "BTPCAL_BTC01M0601 as DOT_6," +
                                  "BTPCAL_BTC01M0701 as DOT_7," +
                                  "BTPCAL_BTC01M0801 as DOT_8," +
                                  "BTPCAL_BTC01M0901 as DOT_9," +
                                  "BTPCAL_BTC01M1001 as DOT_10," +
                                  "BTPCAL_BTC01M1101 as DOT_11," +
                                  "BTPCAL_BTC01M1201 as DOT_12," +
                                  "BTPCAL_BTC01M1301 as DOT_13," +
                                  "BTPCAL_BTC01M1402 as DOT_14," +
                                  "BTPCAL_BTC01M1502 as DOT_15," +
                                  "BTPCAL_BTC01M1602 as DOT_16," +
                                  "BTPCAL_BTC01M1702 as DOT_17," +
                                  "BTPCAL_BTC01M1802 as DOT_18," +
                                  "BTPCAL_BTC01M1902 as DOT_19," +
                                  "BTPCAL_BTC01M2002 as DOT_20," +
                                  "BTPCAL_BTC01M2102 as DOT_21," +
                                  "BTPCAL_BTC01M2202 as DOT_22 " +
                                  "from MC_BTPCAL_LINE_DISTANCE order by TIMESTAMP desc ";
                    DataTable dataTable_4 = _dBSQL.GetCommand(sql_4);
                    if (dataTable_4 != null && dataTable_4.Rows.Count > 0 )
                    {
                        //1#热电偶米数
                        DOT_1 = float.Parse(dataTable_4.Rows[0]["DOT_1"].ToString());
                        //2#热电偶米数
                        DOT_2 = float.Parse(dataTable_4.Rows[0]["DOT_2"].ToString());
                        //3#热电偶米数
                        DOT_3 = float.Parse(dataTable_4.Rows[0]["DOT_3"].ToString());
                        //4#热电偶米数
                        DOT_4 = float.Parse(dataTable_4.Rows[0]["DOT_4"].ToString());
                        //5#热电偶米数
                        DOT_5 = float.Parse(dataTable_4.Rows[0]["DOT_5"].ToString());
                        //6#热电偶米数
                        DOT_6 = float.Parse(dataTable_4.Rows[0]["DOT_6"].ToString());
                        //7#热电偶米数
                        DOT_7 = float.Parse(dataTable_4.Rows[0]["DOT_7"].ToString());
                        //8#热电偶米数
                        DOT_8 = float.Parse(dataTable_4.Rows[0]["DOT_8"].ToString());
                        //9#热电偶米数
                        DOT_9 = float.Parse(dataTable_4.Rows[0]["DOT_9"].ToString());
                        //10#热电偶米数
                        DOT_10 = float.Parse(dataTable_4.Rows[0]["DOT_10"].ToString());
                        //11#热电偶米数
                        DOT_11 = float.Parse(dataTable_4.Rows[0]["DOT_11"].ToString());
                        //12#热电偶米数
                        DOT_12 = float.Parse(dataTable_4.Rows[0]["DOT_12"].ToString());
                        //13#热电偶米数
                        DOT_13 = float.Parse(dataTable_4.Rows[0]["DOT_13"].ToString());
                        //14#热电偶米数
                        DOT_14 = float.Parse(dataTable_4.Rows[0]["DOT_14"].ToString());
                        //15#热电偶米数
                        DOT_15 = float.Parse(dataTable_4.Rows[0]["DOT_15"].ToString());
                        //16#热电偶米数
                        DOT_16 = float.Parse(dataTable_4.Rows[0]["DOT_16"].ToString());
                        //17#热电偶米数
                        DOT_17 = float.Parse(dataTable_4.Rows[0]["DOT_17"].ToString());
                        //18#热电偶米数
                        DOT_18 = float.Parse(dataTable_4.Rows[0]["DOT_18"].ToString());
                        //19#热电偶米数
                        DOT_19 = float.Parse(dataTable_4.Rows[0]["DOT_19"].ToString());
                        //20#热电偶米数
                        DOT_20 = float.Parse(dataTable_4.Rows[0]["DOT_20"].ToString());
                        //21#热电偶米数
                        DOT_21 = float.Parse(dataTable_4.Rows[0]["DOT_21"].ToString());
                        //22#热电偶米数
                        DOT_22 = float.Parse(dataTable_4.Rows[0]["DOT_22"].ToString());

                        if (_Degree == 0)
                        {
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));
                            _G.Add(Math.Round(T_0, _DIG));

                        }
                        else if (_Degree == 1)
                        {
                            _G.Add(Math.Round(T_0 + T_1 * DOT_1, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_2, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_3, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_4, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_5, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_6, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_7, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_8, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_9, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_10, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_11, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_12, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_13, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_14, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_15, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_16, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_17, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_18, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_19, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_20, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_21, _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_22, _DIG));


                        }
                        else if (_Degree == 2)
                        {
                            _G.Add(Math.Round(T_0 + T_1 * DOT_1 + T_2 * Math.Pow(DOT_1, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_2 + T_2 * Math.Pow(DOT_2, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_3 + T_2 * Math.Pow(DOT_3, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_4 + T_2 * Math.Pow(DOT_4, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_5 + T_2 * Math.Pow(DOT_5, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_6 + T_2 * Math.Pow(DOT_6, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_7 + T_2 * Math.Pow(DOT_7, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_8 + T_2 * Math.Pow(DOT_8, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_9 + T_2 * Math.Pow(DOT_9, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_10 + T_2 * Math.Pow(DOT_10, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_11 + T_2 * Math.Pow(DOT_11, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_12 + T_2 * Math.Pow(DOT_12, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_13 + T_2 * Math.Pow(DOT_13, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_14 + T_2 * Math.Pow(DOT_14, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_15 + T_2 * Math.Pow(DOT_15, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_16 + T_2 * Math.Pow(DOT_16, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_17 + T_2 * Math.Pow(DOT_17, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_18 + T_2 * Math.Pow(DOT_18, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_19 + T_2 * Math.Pow(DOT_19, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_20 + T_2 * Math.Pow(DOT_20, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_21 + T_2 * Math.Pow(DOT_21, 2), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_22 + T_2 * Math.Pow(DOT_22, 2), _DIG));
                        }
                        else if (_Degree == 3)
                        {
                            _G.Add(Math.Round((T_0 + T_1 * DOT_1 + T_2 * Math.Pow(DOT_1, 2) + T_3 * Math.Pow(DOT_1, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_2 + T_2 * Math.Pow(DOT_2, 2) + T_3 * Math.Pow(DOT_2, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_3 + T_2 * Math.Pow(DOT_3, 2) + T_3 * Math.Pow(DOT_3, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_4 + T_2 * Math.Pow(DOT_4, 2) + T_3 * Math.Pow(DOT_4, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_5 + T_2 * Math.Pow(DOT_5, 2) + T_3 * Math.Pow(DOT_5, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_6 + T_2 * Math.Pow(DOT_6, 2) + T_3 * Math.Pow(DOT_6, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_7 + T_2 * Math.Pow(DOT_7, 2) + T_3 * Math.Pow(DOT_7, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_8 + T_2 * Math.Pow(DOT_8, 2) + T_3 * Math.Pow(DOT_8, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_9 + T_2 * Math.Pow(DOT_9, 2) + T_3 * Math.Pow(DOT_9, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_10 + T_2 * Math.Pow(DOT_10, 2) + T_3 * Math.Pow(DOT_10, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_11 + T_2 * Math.Pow(DOT_11, 2) + T_3 * Math.Pow(DOT_11, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_12 + T_2 * Math.Pow(DOT_12, 2) + T_3 * Math.Pow(DOT_12, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_13 + T_2 * Math.Pow(DOT_13, 2) + T_3 * Math.Pow(DOT_13, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_14 + T_2 * Math.Pow(DOT_14, 2) + T_3 * Math.Pow(DOT_14, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_15 + T_2 * Math.Pow(DOT_15, 2) + T_3 * Math.Pow(DOT_15, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_16 + T_2 * Math.Pow(DOT_16, 2) + T_3 * Math.Pow(DOT_16, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_17 + T_2 * Math.Pow(DOT_17, 2) + T_3 * Math.Pow(DOT_17, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_18 + T_2 * Math.Pow(DOT_18, 2) + T_3 * Math.Pow(DOT_18, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_19 + T_2 * Math.Pow(DOT_19, 2) + T_3 * Math.Pow(DOT_19, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_20 + T_2 * Math.Pow(DOT_20, 2) + T_3 * Math.Pow(DOT_20, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_21 + T_2 * Math.Pow(DOT_21, 2) + T_3 * Math.Pow(DOT_21, 3)), _DIG));
                            _G.Add(Math.Round((T_0 + T_1 * DOT_22 + T_2 * Math.Pow(DOT_22, 2) + T_3 * Math.Pow(DOT_22, 3)), _DIG));
                         }
                        else if (_Degree == 4)
                        {
                            _G.Add(Math.Round(T_0 + T_1 * DOT_1 + T_2 * Math.Pow(DOT_1, 2) + T_3 * Math.Pow(DOT_1, 3) + T_4 * Math.Pow(DOT_1, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_2 + T_2 * Math.Pow(DOT_2, 2) + T_3 * Math.Pow(DOT_2, 3) + T_4 * Math.Pow(DOT_2, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_3 + T_2 * Math.Pow(DOT_3, 2) + T_3 * Math.Pow(DOT_3, 3) + T_4 * Math.Pow(DOT_3, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_4 + T_2 * Math.Pow(DOT_4, 2) + T_3 * Math.Pow(DOT_4, 3) + T_4 * Math.Pow(DOT_4, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_5 + T_2 * Math.Pow(DOT_5, 2) + T_3 * Math.Pow(DOT_5, 3) + T_4 * Math.Pow(DOT_5, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_6 + T_2 * Math.Pow(DOT_6, 2) + T_3 * Math.Pow(DOT_6, 3) + T_4 * Math.Pow(DOT_6, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_7 + T_2 * Math.Pow(DOT_7, 2) + T_3 * Math.Pow(DOT_7, 3) + T_4 * Math.Pow(DOT_7, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_8 + T_2 * Math.Pow(DOT_8, 2) + T_3 * Math.Pow(DOT_8, 3) + T_4 * Math.Pow(DOT_8, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_9 + T_2 * Math.Pow(DOT_9, 2) + T_3 * Math.Pow(DOT_9, 3) + T_4 * Math.Pow(DOT_9, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_10 + T_2 * Math.Pow(DOT_10, 2) + T_3 * Math.Pow(DOT_10, 3) + T_4 * Math.Pow(DOT_10, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_11 + T_2 * Math.Pow(DOT_11, 2) + T_3 * Math.Pow(DOT_11, 3) + T_4 * Math.Pow(DOT_11, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_12 + T_2 * Math.Pow(DOT_12, 2) + T_3 * Math.Pow(DOT_12, 3) + T_4 * Math.Pow(DOT_12, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_13 + T_2 * Math.Pow(DOT_13, 2) + T_3 * Math.Pow(DOT_13, 3) + T_4 * Math.Pow(DOT_13, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_14 + T_2 * Math.Pow(DOT_14, 2) + T_3 * Math.Pow(DOT_14, 3) + T_4 * Math.Pow(DOT_14, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_15 + T_2 * Math.Pow(DOT_15, 2) + T_3 * Math.Pow(DOT_15, 3) + T_4 * Math.Pow(DOT_15, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_16 + T_2 * Math.Pow(DOT_16, 2) + T_3 * Math.Pow(DOT_16, 3) + T_4 * Math.Pow(DOT_16, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_17 + T_2 * Math.Pow(DOT_17, 2) + T_3 * Math.Pow(DOT_17, 3) + T_4 * Math.Pow(DOT_17, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_18 + T_2 * Math.Pow(DOT_18, 2) + T_3 * Math.Pow(DOT_18, 3) + T_4 * Math.Pow(DOT_18, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_19 + T_2 * Math.Pow(DOT_19, 2) + T_3 * Math.Pow(DOT_19, 3) + T_4 * Math.Pow(DOT_19, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_20 + T_2 * Math.Pow(DOT_20, 2) + T_3 * Math.Pow(DOT_20, 3) + T_4 * Math.Pow(DOT_20, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_21 + T_2 * Math.Pow(DOT_21, 2) + T_3 * Math.Pow(DOT_21, 3) + T_4 * Math.Pow(DOT_21, 4), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_22 + T_2 * Math.Pow(DOT_22, 2) + T_3 * Math.Pow(DOT_22, 3) + T_4 * Math.Pow(DOT_22, 4), _DIG));
                       
                        }
                        else if (_Degree == 5)
                        {
                            _G.Add(Math.Round(T_0 + T_1 * DOT_1 + T_2 * Math.Pow(DOT_1, 2) + T_3 * Math.Pow(DOT_1, 3) + T_4 * Math.Pow(DOT_1, 4) + T_5 * Math.Pow(DOT_1, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_2 + T_2 * Math.Pow(DOT_2, 2) + T_3 * Math.Pow(DOT_2, 3) + T_4 * Math.Pow(DOT_2, 4) + T_5 * Math.Pow(DOT_2, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_3 + T_2 * Math.Pow(DOT_3, 2) + T_3 * Math.Pow(DOT_3, 3) + T_4 * Math.Pow(DOT_3, 4) + T_5 * Math.Pow(DOT_3, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_4 + T_2 * Math.Pow(DOT_4, 2) + T_3 * Math.Pow(DOT_4, 3) + T_4 * Math.Pow(DOT_4, 4) + T_5 * Math.Pow(DOT_4, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_5 + T_2 * Math.Pow(DOT_5, 2) + T_3 * Math.Pow(DOT_5, 3) + T_4 * Math.Pow(DOT_5, 4) + T_5 * Math.Pow(DOT_5, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_6 + T_2 * Math.Pow(DOT_6, 2) + T_3 * Math.Pow(DOT_6, 3) + T_4 * Math.Pow(DOT_6, 4) + T_5 * Math.Pow(DOT_6, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_7 + T_2 * Math.Pow(DOT_7, 2) + T_3 * Math.Pow(DOT_7, 3) + T_4 * Math.Pow(DOT_7, 4) + T_5 * Math.Pow(DOT_7, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_8 + T_2 * Math.Pow(DOT_8, 2) + T_3 * Math.Pow(DOT_8, 3) + T_4 * Math.Pow(DOT_8, 4) + T_5 * Math.Pow(DOT_8, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_9 + T_2 * Math.Pow(DOT_9, 2) + T_3 * Math.Pow(DOT_9, 3) + T_4 * Math.Pow(DOT_9, 4) + T_5 * Math.Pow(DOT_9, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_10 + T_2 * Math.Pow(DOT_10, 2) + T_3 * Math.Pow(DOT_10, 3) + T_4 * Math.Pow(DOT_10, 4) + T_5 * Math.Pow(DOT_10, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_11 + T_2 * Math.Pow(DOT_11, 2) + T_3 * Math.Pow(DOT_11, 3) + T_4 * Math.Pow(DOT_11, 4) + T_5 * Math.Pow(DOT_11, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_12 + T_2 * Math.Pow(DOT_12, 2) + T_3 * Math.Pow(DOT_12, 3) + T_4 * Math.Pow(DOT_12, 4) + T_5 * Math.Pow(DOT_12, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_13 + T_2 * Math.Pow(DOT_13, 2) + T_3 * Math.Pow(DOT_13, 3) + T_4 * Math.Pow(DOT_13, 4) + T_5 * Math.Pow(DOT_13, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_14 + T_2 * Math.Pow(DOT_14, 2) + T_3 * Math.Pow(DOT_14, 3) + T_4 * Math.Pow(DOT_14, 4) + T_5 * Math.Pow(DOT_14, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_15 + T_2 * Math.Pow(DOT_15, 2) + T_3 * Math.Pow(DOT_15, 3) + T_4 * Math.Pow(DOT_15, 4) + T_5 * Math.Pow(DOT_15, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_16 + T_2 * Math.Pow(DOT_16, 2) + T_3 * Math.Pow(DOT_16, 3) + T_4 * Math.Pow(DOT_16, 4) + T_5 * Math.Pow(DOT_16, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_17 + T_2 * Math.Pow(DOT_17, 2) + T_3 * Math.Pow(DOT_17, 3) + T_4 * Math.Pow(DOT_17, 4) + T_5 * Math.Pow(DOT_17, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_18 + T_2 * Math.Pow(DOT_18, 2) + T_3 * Math.Pow(DOT_18, 3) + T_4 * Math.Pow(DOT_18, 4) + T_5 * Math.Pow(DOT_18, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_19 + T_2 * Math.Pow(DOT_19, 2) + T_3 * Math.Pow(DOT_19, 3) + T_4 * Math.Pow(DOT_19, 4) + T_5 * Math.Pow(DOT_19, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_20 + T_2 * Math.Pow(DOT_20, 2) + T_3 * Math.Pow(DOT_20, 3) + T_4 * Math.Pow(DOT_20, 4) + T_5 * Math.Pow(DOT_20, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_21 + T_2 * Math.Pow(DOT_21, 2) + T_3 * Math.Pow(DOT_21, 3) + T_4 * Math.Pow(DOT_21, 4) + T_5 * Math.Pow(DOT_21, 5), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_22 + T_2 * Math.Pow(DOT_22, 2) + T_3 * Math.Pow(DOT_22, 3) + T_4 * Math.Pow(DOT_22, 4) + T_5 * Math.Pow(DOT_22, 5), _DIG));
                        }
                        else if (_Degree == 6)
                        {
                            _G.Add(Math.Round(T_0 + T_1 * DOT_1 + T_2 * Math.Pow(DOT_1, 2) + T_3 * Math.Pow(DOT_1, 3) + T_4 * Math.Pow(DOT_1, 4) + T_5 * Math.Pow(DOT_1, 5) + T_6 * Math.Pow(DOT_1, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_2 + T_2 * Math.Pow(DOT_2, 2) + T_3 * Math.Pow(DOT_2, 3) + T_4 * Math.Pow(DOT_2, 4) + T_5 * Math.Pow(DOT_2, 5) + T_6 * Math.Pow(DOT_2, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_3 + T_2 * Math.Pow(DOT_3, 2) + T_3 * Math.Pow(DOT_3, 3) + T_4 * Math.Pow(DOT_3, 4) + T_5 * Math.Pow(DOT_3, 5) + T_6 * Math.Pow(DOT_3, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_4 + T_2 * Math.Pow(DOT_4, 2) + T_3 * Math.Pow(DOT_4, 3) + T_4 * Math.Pow(DOT_4, 4) + T_5 * Math.Pow(DOT_4, 5) + T_6 * Math.Pow(DOT_4, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_5 + T_2 * Math.Pow(DOT_5, 2) + T_3 * Math.Pow(DOT_5, 3) + T_4 * Math.Pow(DOT_5, 4) + T_5 * Math.Pow(DOT_5, 5) + T_6 * Math.Pow(DOT_5, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_6 + T_2 * Math.Pow(DOT_6, 2) + T_3 * Math.Pow(DOT_6, 3) + T_4 * Math.Pow(DOT_6, 4) + T_5 * Math.Pow(DOT_6, 5) + T_6 * Math.Pow(DOT_6, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_7 + T_2 * Math.Pow(DOT_7, 2) + T_3 * Math.Pow(DOT_7, 3) + T_4 * Math.Pow(DOT_7, 4) + T_5 * Math.Pow(DOT_7, 5) + T_6 * Math.Pow(DOT_7, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_8 + T_2 * Math.Pow(DOT_8, 2) + T_3 * Math.Pow(DOT_8, 3) + T_4 * Math.Pow(DOT_8, 4) + T_5 * Math.Pow(DOT_8, 5) + T_6 * Math.Pow(DOT_8, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_9 + T_2 * Math.Pow(DOT_9, 2) + T_3 * Math.Pow(DOT_9, 3) + T_4 * Math.Pow(DOT_9, 4) + T_5 * Math.Pow(DOT_9, 5) + T_6 * Math.Pow(DOT_9, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_10 + T_2 * Math.Pow(DOT_10, 2) + T_3 * Math.Pow(DOT_10, 3) + T_4 * Math.Pow(DOT_10, 4) + T_5 * Math.Pow(DOT_10, 5) + T_6 * Math.Pow(DOT_10, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_11 + T_2 * Math.Pow(DOT_11, 2) + T_3 * Math.Pow(DOT_11, 3) + T_4 * Math.Pow(DOT_11, 4) + T_5 * Math.Pow(DOT_11, 5) + T_6 * Math.Pow(DOT_11, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_12 + T_2 * Math.Pow(DOT_12, 2) + T_3 * Math.Pow(DOT_12, 3) + T_4 * Math.Pow(DOT_12, 4) + T_5 * Math.Pow(DOT_12, 5) + T_6 * Math.Pow(DOT_12, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_13 + T_2 * Math.Pow(DOT_13, 2) + T_3 * Math.Pow(DOT_13, 3) + T_4 * Math.Pow(DOT_13, 4) + T_5 * Math.Pow(DOT_13, 5) + T_6 * Math.Pow(DOT_13, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_14 + T_2 * Math.Pow(DOT_14, 2) + T_3 * Math.Pow(DOT_14, 3) + T_4 * Math.Pow(DOT_14, 4) + T_5 * Math.Pow(DOT_14, 5) + T_6 * Math.Pow(DOT_14, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_15 + T_2 * Math.Pow(DOT_15, 2) + T_3 * Math.Pow(DOT_15, 3) + T_4 * Math.Pow(DOT_15, 4) + T_5 * Math.Pow(DOT_15, 5) + T_6 * Math.Pow(DOT_15, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_16 + T_2 * Math.Pow(DOT_16, 2) + T_3 * Math.Pow(DOT_16, 3) + T_4 * Math.Pow(DOT_16, 4) + T_5 * Math.Pow(DOT_16, 5) + T_6 * Math.Pow(DOT_16, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_17 + T_2 * Math.Pow(DOT_17, 2) + T_3 * Math.Pow(DOT_17, 3) + T_4 * Math.Pow(DOT_17, 4) + T_5 * Math.Pow(DOT_17, 5) + T_6 * Math.Pow(DOT_17, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_18 + T_2 * Math.Pow(DOT_18, 2) + T_3 * Math.Pow(DOT_18, 3) + T_4 * Math.Pow(DOT_18, 4) + T_5 * Math.Pow(DOT_18, 5) + T_6 * Math.Pow(DOT_18, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_19 + T_2 * Math.Pow(DOT_19, 2) + T_3 * Math.Pow(DOT_19, 3) + T_4 * Math.Pow(DOT_19, 4) + T_5 * Math.Pow(DOT_19, 5) + T_6 * Math.Pow(DOT_19, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_20 + T_2 * Math.Pow(DOT_20, 2) + T_3 * Math.Pow(DOT_20, 3) + T_4 * Math.Pow(DOT_20, 4) + T_5 * Math.Pow(DOT_20, 5) + T_6 * Math.Pow(DOT_20, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_21 + T_2 * Math.Pow(DOT_21, 2) + T_3 * Math.Pow(DOT_21, 3) + T_4 * Math.Pow(DOT_21, 4) + T_5 * Math.Pow(DOT_21, 5) + T_6 * Math.Pow(DOT_21, 6), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_22 + T_2 * Math.Pow(DOT_22, 2) + T_3 * Math.Pow(DOT_22, 3) + T_4 * Math.Pow(DOT_22, 4) + T_5 * Math.Pow(DOT_22, 5) + T_6 * Math.Pow(DOT_22, 6), _DIG));
              

                        }
                        else if (_Degree == 7)
                        {
                            _G.Add(Math.Round(T_0 + T_1 * DOT_1 + T_2 * Math.Pow(DOT_1, 2) + T_3 * Math.Pow(DOT_1, 3) + T_4 * Math.Pow(DOT_1, 4) + T_5 * Math.Pow(DOT_1, 5) + T_6 * Math.Pow(DOT_1, 6) + T_7 * Math.Pow(DOT_1, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_2 + T_2 * Math.Pow(DOT_2, 2) + T_3 * Math.Pow(DOT_2, 3) + T_4 * Math.Pow(DOT_2, 4) + T_5 * Math.Pow(DOT_2, 5) + T_6 * Math.Pow(DOT_2, 6) + T_7 * Math.Pow(DOT_2, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_3 + T_2 * Math.Pow(DOT_3, 2) + T_3 * Math.Pow(DOT_3, 3) + T_4 * Math.Pow(DOT_3, 4) + T_5 * Math.Pow(DOT_3, 5) + T_6 * Math.Pow(DOT_3, 6) + T_7 * Math.Pow(DOT_3, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_4 + T_2 * Math.Pow(DOT_4, 2) + T_3 * Math.Pow(DOT_4, 3) + T_4 * Math.Pow(DOT_4, 4) + T_5 * Math.Pow(DOT_4, 5) + T_6 * Math.Pow(DOT_4, 6) + T_7 * Math.Pow(DOT_4, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_5 + T_2 * Math.Pow(DOT_5, 2) + T_3 * Math.Pow(DOT_5, 3) + T_4 * Math.Pow(DOT_5, 4) + T_5 * Math.Pow(DOT_5, 5) + T_6 * Math.Pow(DOT_5, 6) + T_7 * Math.Pow(DOT_5, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_6 + T_2 * Math.Pow(DOT_6, 2) + T_3 * Math.Pow(DOT_6, 3) + T_4 * Math.Pow(DOT_6, 4) + T_5 * Math.Pow(DOT_6, 5) + T_6 * Math.Pow(DOT_6, 6) + T_7 * Math.Pow(DOT_6, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_7 + T_2 * Math.Pow(DOT_7, 2) + T_3 * Math.Pow(DOT_7, 3) + T_4 * Math.Pow(DOT_7, 4) + T_5 * Math.Pow(DOT_7, 5) + T_6 * Math.Pow(DOT_7, 6) + T_7 * Math.Pow(DOT_7, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_8 + T_2 * Math.Pow(DOT_8, 2) + T_3 * Math.Pow(DOT_8, 3) + T_4 * Math.Pow(DOT_8, 4) + T_5 * Math.Pow(DOT_8, 5) + T_6 * Math.Pow(DOT_8, 6) + T_7 * Math.Pow(DOT_8, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_9 + T_2 * Math.Pow(DOT_9, 2) + T_3 * Math.Pow(DOT_9, 3) + T_4 * Math.Pow(DOT_9, 4) + T_5 * Math.Pow(DOT_9, 5) + T_6 * Math.Pow(DOT_9, 6) + T_7 * Math.Pow(DOT_9, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_10 + T_2 * Math.Pow(DOT_10, 2) + T_3 * Math.Pow(DOT_10, 3) + T_4 * Math.Pow(DOT_10, 4) + T_5 * Math.Pow(DOT_10, 5) + T_6 * Math.Pow(DOT_10, 6) + T_7 * Math.Pow(DOT_10, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_11 + T_2 * Math.Pow(DOT_11, 2) + T_3 * Math.Pow(DOT_11, 3) + T_4 * Math.Pow(DOT_11, 4) + T_5 * Math.Pow(DOT_11, 5) + T_6 * Math.Pow(DOT_11, 6) + T_7 * Math.Pow(DOT_11, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_12 + T_2 * Math.Pow(DOT_12, 2) + T_3 * Math.Pow(DOT_12, 3) + T_4 * Math.Pow(DOT_12, 4) + T_5 * Math.Pow(DOT_12, 5) + T_6 * Math.Pow(DOT_12, 6) + T_7 * Math.Pow(DOT_12, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_13 + T_2 * Math.Pow(DOT_13, 2) + T_3 * Math.Pow(DOT_13, 3) + T_4 * Math.Pow(DOT_13, 4) + T_5 * Math.Pow(DOT_13, 5) + T_6 * Math.Pow(DOT_13, 6) + T_7 * Math.Pow(DOT_13, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_14 + T_2 * Math.Pow(DOT_14, 2) + T_3 * Math.Pow(DOT_14, 3) + T_4 * Math.Pow(DOT_14, 4) + T_5 * Math.Pow(DOT_14, 5) + T_6 * Math.Pow(DOT_14, 6) + T_7 * Math.Pow(DOT_14, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_15 + T_2 * Math.Pow(DOT_15, 2) + T_3 * Math.Pow(DOT_15, 3) + T_4 * Math.Pow(DOT_15, 4) + T_5 * Math.Pow(DOT_15, 5) + T_6 * Math.Pow(DOT_15, 6) + T_7 * Math.Pow(DOT_15, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_16 + T_2 * Math.Pow(DOT_16, 2) + T_3 * Math.Pow(DOT_16, 3) + T_4 * Math.Pow(DOT_16, 4) + T_5 * Math.Pow(DOT_16, 5) + T_6 * Math.Pow(DOT_16, 6) + T_7 * Math.Pow(DOT_16, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_17 + T_2 * Math.Pow(DOT_17, 2) + T_3 * Math.Pow(DOT_17, 3) + T_4 * Math.Pow(DOT_17, 4) + T_5 * Math.Pow(DOT_17, 5) + T_6 * Math.Pow(DOT_17, 6) + T_7 * Math.Pow(DOT_17, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_18 + T_2 * Math.Pow(DOT_18, 2) + T_3 * Math.Pow(DOT_18, 3) + T_4 * Math.Pow(DOT_18, 4) + T_5 * Math.Pow(DOT_18, 5) + T_6 * Math.Pow(DOT_18, 6) + T_7 * Math.Pow(DOT_18, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_19 + T_2 * Math.Pow(DOT_19, 2) + T_3 * Math.Pow(DOT_19, 3) + T_4 * Math.Pow(DOT_19, 4) + T_5 * Math.Pow(DOT_19, 5) + T_6 * Math.Pow(DOT_19, 6) + T_7 * Math.Pow(DOT_19, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_20 + T_2 * Math.Pow(DOT_20, 2) + T_3 * Math.Pow(DOT_20, 3) + T_4 * Math.Pow(DOT_20, 4) + T_5 * Math.Pow(DOT_20, 5) + T_6 * Math.Pow(DOT_20, 6) + T_7 * Math.Pow(DOT_20, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_21 + T_2 * Math.Pow(DOT_21, 2) + T_3 * Math.Pow(DOT_21, 3) + T_4 * Math.Pow(DOT_21, 4) + T_5 * Math.Pow(DOT_21, 5) + T_6 * Math.Pow(DOT_21, 6) + T_7 * Math.Pow(DOT_21, 7), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_22 + T_2 * Math.Pow(DOT_22, 2) + T_3 * Math.Pow(DOT_22, 3) + T_4 * Math.Pow(DOT_22, 4) + T_5 * Math.Pow(DOT_22, 5) + T_6 * Math.Pow(DOT_22, 6) + T_7 * Math.Pow(DOT_22, 7), _DIG));
                           
                        }
                        else if (_Degree == 8)
                        {
                            _G.Add(Math.Round(T_0 + T_1 * DOT_1 + T_2 * Math.Pow(DOT_1, 2) + T_3 * Math.Pow(DOT_1, 3) + T_4 * Math.Pow(DOT_1, 4) + T_5 * Math.Pow(DOT_1, 5) + T_6 * Math.Pow(DOT_1, 6) + T_7 * Math.Pow(DOT_1, 7) + T_8 * Math.Pow(DOT_1, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_2 + T_2 * Math.Pow(DOT_2, 2) + T_3 * Math.Pow(DOT_2, 3) + T_4 * Math.Pow(DOT_2, 4) + T_5 * Math.Pow(DOT_2, 5) + T_6 * Math.Pow(DOT_2, 6) + T_7 * Math.Pow(DOT_2, 7) + T_8 * Math.Pow(DOT_2, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_3 + T_2 * Math.Pow(DOT_3, 2) + T_3 * Math.Pow(DOT_3, 3) + T_4 * Math.Pow(DOT_3, 4) + T_5 * Math.Pow(DOT_3, 5) + T_6 * Math.Pow(DOT_3, 6) + T_7 * Math.Pow(DOT_3, 7) + T_8 * Math.Pow(DOT_3, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_4 + T_2 * Math.Pow(DOT_4, 2) + T_3 * Math.Pow(DOT_4, 3) + T_4 * Math.Pow(DOT_4, 4) + T_5 * Math.Pow(DOT_4, 5) + T_6 * Math.Pow(DOT_4, 6) + T_7 * Math.Pow(DOT_4, 7) + T_8 * Math.Pow(DOT_4, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_5 + T_2 * Math.Pow(DOT_5, 2) + T_3 * Math.Pow(DOT_5, 3) + T_4 * Math.Pow(DOT_5, 4) + T_5 * Math.Pow(DOT_5, 5) + T_6 * Math.Pow(DOT_5, 6) + T_7 * Math.Pow(DOT_5, 7) + T_8 * Math.Pow(DOT_5, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_6 + T_2 * Math.Pow(DOT_6, 2) + T_3 * Math.Pow(DOT_6, 3) + T_4 * Math.Pow(DOT_6, 4) + T_5 * Math.Pow(DOT_6, 5) + T_6 * Math.Pow(DOT_6, 6) + T_7 * Math.Pow(DOT_6, 7) + T_8 * Math.Pow(DOT_6, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_7 + T_2 * Math.Pow(DOT_7, 2) + T_3 * Math.Pow(DOT_7, 3) + T_4 * Math.Pow(DOT_7, 4) + T_5 * Math.Pow(DOT_7, 5) + T_6 * Math.Pow(DOT_7, 6) + T_7 * Math.Pow(DOT_7, 7) + T_8 * Math.Pow(DOT_7, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_8 + T_2 * Math.Pow(DOT_8, 2) + T_3 * Math.Pow(DOT_8, 3) + T_4 * Math.Pow(DOT_8, 4) + T_5 * Math.Pow(DOT_8, 5) + T_6 * Math.Pow(DOT_8, 6) + T_7 * Math.Pow(DOT_8, 7) + T_8 * Math.Pow(DOT_8, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_9 + T_2 * Math.Pow(DOT_9, 2) + T_3 * Math.Pow(DOT_9, 3) + T_4 * Math.Pow(DOT_9, 4) + T_5 * Math.Pow(DOT_9, 5) + T_6 * Math.Pow(DOT_9, 6) + T_7 * Math.Pow(DOT_9, 7) + T_8 * Math.Pow(DOT_9, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_10 + T_2 * Math.Pow(DOT_10, 2) + T_3 * Math.Pow(DOT_10, 3) + T_4 * Math.Pow(DOT_10, 4) + T_5 * Math.Pow(DOT_10, 5) + T_6 * Math.Pow(DOT_10, 6) + T_7 * Math.Pow(DOT_10, 7) + T_8 * Math.Pow(DOT_10, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_11 + T_2 * Math.Pow(DOT_11, 2) + T_3 * Math.Pow(DOT_11, 3) + T_4 * Math.Pow(DOT_11, 4) + T_5 * Math.Pow(DOT_11, 5) + T_6 * Math.Pow(DOT_11, 6) + T_7 * Math.Pow(DOT_11, 7) + T_8 * Math.Pow(DOT_11, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_12 + T_2 * Math.Pow(DOT_12, 2) + T_3 * Math.Pow(DOT_12, 3) + T_4 * Math.Pow(DOT_12, 4) + T_5 * Math.Pow(DOT_12, 5) + T_6 * Math.Pow(DOT_12, 6) + T_7 * Math.Pow(DOT_12, 7) + T_8 * Math.Pow(DOT_12, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_13 + T_2 * Math.Pow(DOT_13, 2) + T_3 * Math.Pow(DOT_13, 3) + T_4 * Math.Pow(DOT_13, 4) + T_5 * Math.Pow(DOT_13, 5) + T_6 * Math.Pow(DOT_13, 6) + T_7 * Math.Pow(DOT_13, 7) + T_8 * Math.Pow(DOT_13, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_14 + T_2 * Math.Pow(DOT_14, 2) + T_3 * Math.Pow(DOT_14, 3) + T_4 * Math.Pow(DOT_14, 4) + T_5 * Math.Pow(DOT_14, 5) + T_6 * Math.Pow(DOT_14, 6) + T_7 * Math.Pow(DOT_14, 7) + T_8 * Math.Pow(DOT_14, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_15 + T_2 * Math.Pow(DOT_15, 2) + T_3 * Math.Pow(DOT_15, 3) + T_4 * Math.Pow(DOT_15, 4) + T_5 * Math.Pow(DOT_15, 5) + T_6 * Math.Pow(DOT_15, 6) + T_7 * Math.Pow(DOT_15, 7) + T_8 * Math.Pow(DOT_15, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_16 + T_2 * Math.Pow(DOT_16, 2) + T_3 * Math.Pow(DOT_16, 3) + T_4 * Math.Pow(DOT_16, 4) + T_5 * Math.Pow(DOT_16, 5) + T_6 * Math.Pow(DOT_16, 6) + T_7 * Math.Pow(DOT_16, 7) + T_8 * Math.Pow(DOT_16, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_17 + T_2 * Math.Pow(DOT_17, 2) + T_3 * Math.Pow(DOT_17, 3) + T_4 * Math.Pow(DOT_17, 4) + T_5 * Math.Pow(DOT_17, 5) + T_6 * Math.Pow(DOT_17, 6) + T_7 * Math.Pow(DOT_17, 7) + T_8 * Math.Pow(DOT_17, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_18 + T_2 * Math.Pow(DOT_18, 2) + T_3 * Math.Pow(DOT_18, 3) + T_4 * Math.Pow(DOT_18, 4) + T_5 * Math.Pow(DOT_18, 5) + T_6 * Math.Pow(DOT_18, 6) + T_7 * Math.Pow(DOT_18, 7) + T_8 * Math.Pow(DOT_18, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_19 + T_2 * Math.Pow(DOT_19, 2) + T_3 * Math.Pow(DOT_19, 3) + T_4 * Math.Pow(DOT_19, 4) + T_5 * Math.Pow(DOT_19, 5) + T_6 * Math.Pow(DOT_19, 6) + T_7 * Math.Pow(DOT_19, 7) + T_8 * Math.Pow(DOT_19, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_20 + T_2 * Math.Pow(DOT_20, 2) + T_3 * Math.Pow(DOT_20, 3) + T_4 * Math.Pow(DOT_20, 4) + T_5 * Math.Pow(DOT_20, 5) + T_6 * Math.Pow(DOT_20, 6) + T_7 * Math.Pow(DOT_20, 7) + T_8 * Math.Pow(DOT_20, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_21 + T_2 * Math.Pow(DOT_21, 2) + T_3 * Math.Pow(DOT_21, 3) + T_4 * Math.Pow(DOT_21, 4) + T_5 * Math.Pow(DOT_21, 5) + T_6 * Math.Pow(DOT_21, 6) + T_7 * Math.Pow(DOT_21, 7) + T_8 * Math.Pow(DOT_21, 8), _DIG));
                            _G.Add(Math.Round(T_0 + T_1 * DOT_22 + T_2 * Math.Pow(DOT_22, 2) + T_3 * Math.Pow(DOT_22, 3) + T_4 * Math.Pow(DOT_22, 4) + T_5 * Math.Pow(DOT_22, 5) + T_6 * Math.Pow(DOT_22, 6) + T_7 * Math.Pow(DOT_22, 7) + T_8 * Math.Pow(DOT_22, 8), _DIG));
                    

                        }
                        return _G;
                    }
                    else
                    {
                        return null;
                    }
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

        /// <summary>
        /// 趋势曲线
        /// time_begin :开始时间
        /// time_end：结束时间
        ///  item1:数据个数  item2（key:标识符,item1：时间，item2：数据点）
        /// </summary>
        /// <returns></returns>
        public Tuple< int, Dictionary<string,List<Tuple<DateTime,double>>>> _Trend_Curve(DateTime time_begin,DateTime time_end)
        {
            try
            {
                Dictionary<string, List<Tuple<DateTime, double>>> _G = new Dictionary<string, List<Tuple<DateTime, double>>>();//数据容器
                #region 查询数据并赋值
                //查询出当前时间及倒退的实际值
                string sql_MC_BTPCAL_PREDICT = "select " +
                    "TIMESTAMP," +
                    "BTP_NOW," +
                    "BRP_NOW," +
                    "TRP_NOW," +
                    "BTP_PREDICT_TIMESTAMP ," +
                    "BRP_PREDICT_TIMESTAMP  " +
                    "from MC_BTPCAL_PREDICT " +
                    "where TIMESTAMP between '" + time_begin + "' and '" + time_end + "' order by TIMESTAMP asc  ";
                DataTable dataTable_Now = _dBSQL.GetCommand(sql_MC_BTPCAL_PREDICT);
                if (dataTable_Now.Rows.Count > 0 && dataTable_Now != null)
                {
                    int count_time = dataTable_Now.Rows.Count;
                    //收集正常的实际数据
                    for (int x = 0; x < count_time; x++)
                    {
                        DateTime _time = DateTime.Parse(dataTable_Now.Rows[x]["TIMESTAMP"].ToString());
                        #region BTP实际
                        if (_G.ContainsKey("BTP"))
                        {
                            _G["BTP"].Add(new Tuple<DateTime, double>(_time, double.Parse(dataTable_Now.Rows[x]["BTP_NOW"].ToString())));
                        }
                        else
                        {
                            List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                            tuples.Add(new Tuple<DateTime, double>(_time, double.Parse(dataTable_Now.Rows[x]["BTP_NOW"].ToString())));
                            _G.Add("BTP", tuples);
                        }
                        #endregion

                        #region BRP实际
                        if (_G.ContainsKey("BRP"))
                        {
                            _G["BRP"].Add(new Tuple<DateTime, double>(_time, double.Parse(dataTable_Now.Rows[x]["BRP_NOW"].ToString())));
                        }
                        else
                        {
                            List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                            tuples.Add(new Tuple<DateTime, double>(_time, double.Parse(dataTable_Now.Rows[x]["BRP_NOW"].ToString())));
                            _G.Add("BRP", tuples);
                        }
                        #endregion

                        #region TRP实际
                        if (_G.ContainsKey("TRP"))
                        {
                            _G["TRP"].Add(new Tuple<DateTime, double>(_time, double.Parse(dataTable_Now.Rows[x]["TRP_NOW"].ToString())));
                        }
                        else
                        {
                            List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                            tuples.Add(new Tuple<DateTime, double>(_time, double.Parse(dataTable_Now.Rows[x]["TRP_NOW"].ToString())));
                            _G.Add("TRP", tuples);
                        }
                        #endregion
                    }
                    //btp预测时间
                    DateTime time_BTP = DateTime.Parse(dataTable_Now.Rows[count_time - 1]["BTP_PREDICT_TIMESTAMP"].ToString());
                    //brp预测时间
                    DateTime time_BRP = DateTime.Parse(dataTable_Now.Rows[count_time - 1]["BRP_PREDICT_TIMESTAMP"].ToString());
                    //实际时间
                    DateTime time_Now = DateTime.Parse(dataTable_Now.Rows[count_time - 1]["TIMESTAMP"].ToString());
                    //人工添加预测数据时间x轴
                    int _A = 0;//时间差
                    if (time_BTP > time_BRP)
                    {
                        TimeSpan time_deffer = time_BTP - time_Now;
                        _A = (int)time_deffer.TotalMinutes;
                    }
                    else
                    {
                        TimeSpan time_deffer = time_BRP - time_Now;
                        _A = (int)time_deffer.TotalMinutes;
                    }
                    for (int x = 0; x < _A; x++)
                    {
                        DateTime _time = time_Now.AddMinutes(x + 1);
                        #region BTP
                        if (_G.ContainsKey("BTP"))
                        {
                            _G["BTP"].Add(new Tuple<DateTime, double>(_time, double.NaN));
                        }
                        else
                        {
                            List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                            tuples.Add(new Tuple<DateTime, double>(_time, double.NaN));
                            _G.Add("BTP", tuples);
                        }
                        #endregion

                        #region BRP
                        if (_G.ContainsKey("BRP"))
                        {
                            _G["BRP"].Add(new Tuple<DateTime, double>(_time, double.NaN));
                        }
                        else
                        {
                            List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                            tuples.Add(new Tuple<DateTime, double>(_time, double.NaN));
                            _G.Add("BRP", tuples);
                        }
                        #endregion

                        #region TRP
                        if (_G.ContainsKey("TRP"))
                        {
                            _G["TRP"].Add(new Tuple<DateTime, double>(_time, double.NaN));
                        }
                        else
                        {
                            List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                            tuples.Add(new Tuple<DateTime, double>(_time, double.NaN));
                            _G.Add("TRP", tuples);
                        }
                        #endregion
                    }
                    List<Tuple<DateTime, double>> ts = _G["BTP"];//获取x轴数据
                    //通过实际查询实际及人工添加预测时间去查询对应的预测数据
                    for (int x = 0; x < ts.Count; x++)
                    {
                        DateTime time1 = ts[x].Item1;
                        DateTime time2 = time1.AddMinutes(1);
                        #region BTP预测
                        var sql_BTP = "select top (1) " +
                         "BTP_PREDICT " +
                         "from MC_BTPCAL_PREDICT " +
                         "where BTP_PREDICT_TIMESTAMP >= '" + time1 + "' and  BTP_PREDICT_TIMESTAMP <= '" + time2 + "' order by BTP_PREDICT_TIMESTAMP desc";
                        DataTable data_BTP = _dBSQL.GetCommand(sql_BTP);
                        if (data_BTP.Rows.Count > 0 && data_BTP != null)
                        {
                            #region BTP_PREDICT
                            if (_G.ContainsKey("BTP_PREDICT"))
                            {
                                _G["BTP_PREDICT"].Add(new Tuple<DateTime, double>(time1, double.Parse(data_BTP.Rows[0]["BTP_PREDICT"].ToString())));
                            }
                            else
                            {
                                List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                                tuples.Add(new Tuple<DateTime, double>(time1, double.Parse(data_BTP.Rows[0]["BTP_PREDICT"].ToString())));
                                _G.Add("BTP_PREDICT", tuples);
                            }
                            #endregion
                        }
                        else
                        {
                            //查询出空值或者无数据则赋值上一个数据点
                            if (x == 0)
                            {
                                List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                                tuples.Add(new Tuple<DateTime, double>(time1, double.NaN));
                                _G.Add("BTP_PREDICT", tuples);
                            }
                            else
                            {
                                _G["BTP_PREDICT"].Add(new Tuple<DateTime, double>(time1, _G["BTP_PREDICT"][x-1].Item2));
                            }

                        }
                        #endregion

                        #region BRP预测
                        var sql_BRP = "select top (1) " +
                          "BRP_PREDICT " +
                           "from MC_BTPCAL_PREDICT " +
                           "where BRP_PREDICT_TIMESTAMP >= '" + time1 + "' and  BRP_PREDICT_TIMESTAMP <= '" + time2 + "' order by BRP_PREDICT_TIMESTAMP desc";
                        DataTable data_BRP = _dBSQL.GetCommand(sql_BRP);
                        if (data_BRP.Rows.Count > 0 && data_BRP != null)
                        {
                            #region BRP_PREDICT
                            if (_G.ContainsKey("BRP_PREDICT"))
                            {
                                _G["BRP_PREDICT"].Add(new Tuple<DateTime, double>(time1, double.Parse(data_BRP.Rows[0]["BRP_PREDICT"].ToString())));
                            }
                            else
                            {
                                List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                                tuples.Add(new Tuple<DateTime, double>(time1, double.Parse(data_BRP.Rows[0]["BRP_PREDICT"].ToString())));
                                _G.Add("BRP_PREDICT", tuples);
                            }
                            #endregion
                        }
                        else
                        {
                            //查询出空值或者无数据则赋值上一个数据点
                            if (x == 0)
                            {
                                List<Tuple<DateTime, double>> tuples = new List<Tuple<DateTime, double>>();
                                tuples.Add(new Tuple<DateTime, double>(time1, double.NaN));
                                _G.Add("BRP_PREDICT", tuples);
                            }
                            else
                            {
                                _G["BRP_PREDICT"].Add(new Tuple<DateTime, double>(time1, _G["BRP_PREDICT"][x-1].Item2));
                            }
                        }
                        #endregion 
                    }
                    return new Tuple<int, Dictionary<string, List<Tuple<DateTime, double>>>>(ts.Count,_G);
                }
                else
                {
                    return null;
                }


                #endregion
            }
            catch(Exception ee)
            {
                return null;
            }
        }
    }
}
