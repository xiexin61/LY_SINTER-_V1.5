using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;
using LY_SINTER.Custom;
using LY_SINTER.Model;
using System.IO;

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_Ripe_analysis_CLASS : Form
    {
        private DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        public static bool isopen = false;
        private ANALYSIS_MODEL _MODEL = new ANALYSIS_MODEL();
        public System.Timers.Timer _Timer1 { get; set; }

        private string[] _Getdate_name = { "RECORD_TIME",
"SHIFT_FLAG",
"C_TFE",
"C_FEO",
"C_CAO",
"C_SIO2",
"C_AL2O3",
"C_MGO",
"C_S",
"C_P2O5",
"C_R",
"C_K2O",
"C_NA2O",
"C_PB",
"C_ZN",
"C_MNO",
"C_TIO2",
"C_AS",
"C_CU",
"C_K",
"C_V2O5",
"C_AL2O3_SIO2",
"C_MGO_AL2O3",
"C_TI",
"RI",
"RDI_0_5",
"RDI_3_15",
"RDI_6_3",
"SCRENING",
"GRIT_5",
"GRIT_5_10",
"GRIT_10_16",
"GRIT_16_25",
"GRIT_25_40",
"GRIT_40",
"PATCL_AV",
"GRIT_10",
"GRIT_10_40",
"R_005_CONTROL",
"R_008_CONTROL",
"FEO_1_CONTROL",
"MGO_015_CONTROL",
"GRIT_10_CONTROL",
"TFE_STANDARD",
"SIO2_STANDARD",
"CAO_STANDARD",
"R_STANDARD",
"FEO_STANDARD",
"MGO_STANDARD"  };

        public vLog _vLog { get; set; }
        private List<bool> _CHANGE = new List<bool>();

        public Frm_Ripe_analysis_CLASS()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Analysis\\Frm_Ripe_analysis_CLASS\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            //  DATA_TEXT(1, comboBox1.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());//填充数据
            DATA_TEXT(3, comboBox1.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());//填充数据
            _Timer1 = new System.Timers.Timer(1000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }

        private void Timer1_Tick_1()
        {
            Action invokeAction = new Action(Timer1_Tick_1);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                this.dataGridView1.Rows[7].Frozen = true;
                for (int s = 0; s < 8; s++)
                {
                    this.dataGridView1.Rows[s].DefaultCellStyle.BackColor = Color.AliceBlue;
                }
                //  this.dataGridView1.Rows[8].DefaultCellStyle.BackColor = Color.Red;
            }
        }

        /// <summary>
        /// 开始时间&结束时间赋值
        /// </summary>
        public void dateTimePicker_value()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-3);

            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }

        /// <summary>
        /// DATA_TEXT获取班级数据
        /// _flag：烧结机号
        /// _CLASS_NAME：班组名称
        /// _time_begin:开始时间
        /// _time_end：结束时间
        /// </summary>
        /// <param name="_flag"></param>
        /// <param name="_time_begin"></param>
        /// <param name="_time_end"></param>
        public void DATA_TEXT(int _flag, string _CLASS_NAME, string _time_begin, string _time_end)
        {
            try
            {
                if (_flag == 3)
                {
                    DataTable _data = new DataTable();
                    for (int x = 0; x < _Getdate_name.Count(); x++)
                    {
                        _data.Columns.Add(_Getdate_name[x]);
                    }
                    _CHANGE.Clear();
                    if (true)
                    {
                        for (int X = 0; X < 4; X++)
                        {
                            _CHANGE.Add(true);
                        }
                    }
                    //else
                    //{
                    //    if (_CLASS_NAME == "甲")
                    //    {
                    //        _CHANGE.Add(true);
                    //        _CHANGE.Add(false);
                    //        _CHANGE.Add(false);
                    //        _CHANGE.Add(false);
                    //    }
                    //    else if (_CLASS_NAME == "乙")
                    //    {
                    //        _CHANGE.Add(false);
                    //        _CHANGE.Add(true);
                    //        _CHANGE.Add(false);
                    //        _CHANGE.Add(false);
                    //    }
                    //    else if (_CLASS_NAME == "丙")
                    //    {
                    //        _CHANGE.Add(false);
                    //        _CHANGE.Add(false);
                    //        _CHANGE.Add(true);
                    //        _CHANGE.Add(false);
                    //    }
                    //    else if (_CLASS_NAME == "丁")
                    //    {
                    //        _CHANGE.Add(false);
                    //        _CHANGE.Add(false);
                    //        _CHANGE.Add(false);
                    //        _CHANGE.Add(true);
                    //    }
                    //}
                    var _sql_1 = "";
                    if (_CLASS_NAME == "全部")
                    {
                        _sql_1 = "select RECORD_TIME,SHIFT_FLAG,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P2O5,C_R,C_K2O,C_NA2O,C_PB,C_ZN,C_MNO,C_TIO2,C_AS,C_CU,C_K,C_V2O5,C_AL2O3_SIO2,C_MGO_AL2O3,C_TI,RI,RDI_0_5,RDI_3_15,RDI_6_3,SCRENING,GRIT_5,GRIT_5_10,GRIT_10_16,GRIT_16_25,GRIT_25_40,GRIT_40,PATCL_AV,GRIT_10,GRIT_10_40,R_005_CONTROL,R_008_CONTROL,FEO_1_CONTROL,MGO_015_CONTROL,GRIT_10_CONTROL,TFE_STANDARD,SIO2_STANDARD,CAO_STANDARD,R_STANDARD,FEO_STANDARD,MGO_STANDARD  from MC_NUMCAL_INTERFACE_6_CLASS where DL_FLAG = " + _flag + " and RECORD_TIME >= '" + _time_begin + "' and RECORD_TIME <= '" + _time_end + "' order by RECORD_TIME desc";
                    }
                    else
                    {
                        _sql_1 = "select RECORD_TIME,SHIFT_FLAG,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P2O5,C_R,C_K2O,C_NA2O,C_PB,C_ZN,C_MNO,C_TIO2,C_AS,C_CU,C_K,C_V2O5,C_AL2O3_SIO2,C_MGO_AL2O3,C_TI,RI,RDI_0_5,RDI_3_15,RDI_6_3,SCRENING,GRIT_5,GRIT_5_10,GRIT_10_16,GRIT_16_25,GRIT_25_40,GRIT_40,PATCL_AV,GRIT_10,GRIT_10_40,R_005_CONTROL,R_008_CONTROL,FEO_1_CONTROL,MGO_015_CONTROL,GRIT_10_CONTROL,TFE_STANDARD,SIO2_STANDARD,CAO_STANDARD,R_STANDARD,FEO_STANDARD,MGO_STANDARD  from MC_NUMCAL_INTERFACE_6_CLASS where SHIFT_FLAG = '" + _CLASS_NAME + "' and  DL_FLAG = " + _flag + " and RECORD_TIME >= '" + _time_begin + "' and RECORD_TIME <= '" + _time_end + "' order by RECORD_TIME desc";
                    }
                    var _a = "select RECORD_TIME,SHIFT_FLAG,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P2O5,C_R,C_K2O,C_NA2O,C_PB,C_ZN,C_MNO,C_TIO2,C_AS,C_CU,C_K,C_V2O5,C_AL2O3_SIO2,C_MGO_AL2O3,C_TI,RI,RDI_0_5,RDI_3_15,RDI_6_3,SCRENING,GRIT_5,GRIT_5_10,GRIT_10_16,GRIT_16_25,GRIT_25_40,GRIT_40,PATCL_AV,GRIT_10,GRIT_10_40,R_005_CONTROL,R_008_CONTROL,FEO_1_CONTROL,MGO_015_CONTROL,GRIT_10_CONTROL,TFE_STANDARD,SIO2_STANDARD,CAO_STANDARD,R_STANDARD,FEO_STANDARD,MGO_STANDARD  from MC_NUMCAL_INTERFACE_6_CLASS where DL_FLAG = " + _flag + " and RECORD_TIME >= '" + _time_begin + "' and RECORD_TIME <= '" + _time_end + "' order by RECORD_TIME desc";
                    DataTable _Tem_date_1 = _dBSQL.GetCommand(_a);
                    DataTable _Tem_date = _dBSQL.GetCommand(_sql_1);
                    if (_Tem_date != null && _Tem_date.Rows.Count > 0)
                    {
                        #region 最优数据

                        var _A = _MODEL._GETAVG(_Tem_date_1, 2, "甲", "SHIFT_FLAG", 3); //甲班平均
                        if (_A.Item1)
                        {
                            DataRow row_1 = _A.Item2;
                            DataRow row_11 = _data.NewRow();

                            row_11["RECORD_TIME"] = "甲班平均";
                            row_11["SHIFT_FLAG"] = "-";
                            for (int x = 2; x < row_1.Table.Columns.Count; x++)
                            {
                                row_11[x] = row_1[x].ToString();
                            }
                            _data.Rows.Add(row_11);
                        }
                        var _B = _MODEL._GETAVG(_Tem_date_1, 2, "乙", "SHIFT_FLAG", 3); //乙班平均
                        if (_B.Item1)
                        {
                            DataRow row_1 = _B.Item2;
                            DataRow row_11 = _data.NewRow();

                            row_11["RECORD_TIME"] = "乙班平均";
                            row_11["SHIFT_FLAG"] = "-";
                            for (int x = 2; x < row_1.Table.Columns.Count; x++)
                            {
                                row_11[x] = row_1[x].ToString();
                            }
                            _data.Rows.Add(row_11);
                        }
                        var _C = _MODEL._GETAVG(_Tem_date_1, 2, "丙", "SHIFT_FLAG", 3); //丙班平均
                        if (_C.Item1)
                        {
                            DataRow row_1 = _C.Item2;
                            DataRow row_11 = _data.NewRow();

                            row_11["RECORD_TIME"] = "丙班平均";
                            row_11["SHIFT_FLAG"] = "-";
                            for (int x = 2; x < row_1.Table.Columns.Count; x++)
                            {
                                row_11[x] = row_1[x].ToString();
                            }
                            _data.Rows.Add(row_11);
                        }
                        var _D = _MODEL._GETAVG(_Tem_date_1, 2, "丁", "SHIFT_FLAG", 3); //丁班平均
                        if (_D.Item1)
                        {
                            DataRow row_1 = _D.Item2;
                            DataRow row_11 = _data.NewRow();

                            row_11["RECORD_TIME"] = "丁班平均";
                            row_11["SHIFT_FLAG"] = "-";
                            for (int x = 2; x < row_1.Table.Columns.Count; x++)
                            {
                                row_11[x] = row_1[x].ToString();
                            }
                            _data.Rows.Add(row_11);
                        }
                        if (_CHANGE[0])
                        {
                            String _SQL1 = "select * from MC_NUMCAL_INTERFACE_6_C_OPTIMAL where CLASS_FLAG = 1 ";//甲班最优
                            DataTable data_c1 = _dBSQL.GetCommand(_SQL1);
                            if (data_c1 != null && data_c1.Rows.Count > 0)
                            {
                                DataRow row_5 = _data.NewRow();
                                row_5["RECORD_TIME"] = "甲班最优(" + DateTime.Parse(data_c1.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : data_c1.Rows[0]["timestamp"].ToString()).Month + "月" + DateTime.Parse(data_c1.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : data_c1.Rows[0]["timestamp"].ToString()).Day + "日)";
                                row_5["SHIFT_FLAG"] = "-";
                                for (int y = 2; y < _Getdate_name.Count(); y++)
                                {
                                    row_5[_Getdate_name[y]] = data_c1.Rows[0][_Getdate_name[y]].ToString();
                                }
                                _data.Rows.Add(row_5);
                            }
                        }

                        if (_CHANGE[1])
                        {
                            String _SQL2 = "select * from MC_NUMCAL_INTERFACE_6_C_OPTIMAL where CLASS_FLAG = 2 ";//甲班最优
                            DataTable data_c2 = _dBSQL.GetCommand(_SQL2);
                            if (data_c2 != null && data_c2.Rows.Count > 0)
                            {
                                DataRow row_5 = _data.NewRow();
                                row_5["RECORD_TIME"] = "乙班最优(" + DateTime.Parse(data_c2.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : data_c2.Rows[0]["timestamp"].ToString()).Month + "月" + DateTime.Parse(data_c2.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : data_c2.Rows[0]["timestamp"].ToString()).Day + "日)";
                                row_5["SHIFT_FLAG"] = "-";
                                for (int y = 2; y < _Getdate_name.Count(); y++)
                                {
                                    row_5[_Getdate_name[y]] = data_c2.Rows[0][_Getdate_name[y]].ToString();
                                }
                                _data.Rows.Add(row_5);
                            }
                        }

                        if (_CHANGE[2])
                        {
                            String _SQL3 = "select * from MC_NUMCAL_INTERFACE_6_C_OPTIMAL where CLASS_FLAG = 3 ";//丙班最优
                            DataTable data_c3 = _dBSQL.GetCommand(_SQL3);
                            if (data_c3 != null && data_c3.Rows.Count > 0)
                            {
                                DataRow row_5 = _data.NewRow();
                                row_5["RECORD_TIME"] = "丙班最优(" + DateTime.Parse(data_c3.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : data_c3.Rows[0]["timestamp"].ToString()).Month + "月" + DateTime.Parse(data_c3.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : data_c3.Rows[0]["timestamp"].ToString()).Day + "日)";
                                row_5["SHIFT_FLAG"] = "-";
                                for (int y = 2; y < _Getdate_name.Count(); y++)
                                {
                                    row_5[_Getdate_name[y]] = data_c3.Rows[0][_Getdate_name[y]].ToString();
                                }
                                _data.Rows.Add(row_5);
                            }
                        }

                        if (_CHANGE[3])
                        {
                            String _SQL4 = "select * from MC_NUMCAL_INTERFACE_6_C_OPTIMAL where CLASS_FLAG = 4 ";//丁班最优
                            DataTable data_c4 = _dBSQL.GetCommand(_SQL4);
                            if (data_c4 != null && data_c4.Rows.Count > 0)
                            {
                                DataRow row_5 = _data.NewRow();
                                row_5["RECORD_TIME"] = "丁班最优(" + DateTime.Parse(data_c4.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : data_c4.Rows[0]["timestamp"].ToString()).Month + "月" + DateTime.Parse(data_c4.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : data_c4.Rows[0]["timestamp"].ToString()).Day + "日)";
                                row_5["SHIFT_FLAG"] = "-";
                                for (int y = 2; y < _Getdate_name.Count(); y++)
                                {
                                    row_5[_Getdate_name[y]] = data_c4.Rows[0][_Getdate_name[y]].ToString();
                                }
                                _data.Rows.Add(row_5);
                            }
                        }

                        // var _sql = "select R_003_CONTROL, R_005_CONTROL,R_008_CONTROL,FEO_06_CONTROL,FEO_1_CONTROL,MGO_015_CONTROL,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_R,C_ZN,C_AL2O3_SIO2,C_MGO_AL2O3,C_TOTAL,C_TI,RI,RDI_3_15,GRIT_5,GRIT_5_10,PATCL_AV,GRIT_10,GRIT_10_40,TFE_STANDARD,SIO2_STANDARD,CAO_STANDARD,R_STANDARD,FEO_STANDARD,MGO_STANDARD from MC_NUMCAL_INTERFACE_6_OPTIMAL where DL_FLAG = 1 order by CLASS_FLAG asc";
                        //var _sql = "select * from MC_NUMCAL_INTERFACE_6_OPTIMAL where DL_FLAG = " + _flag + " order by CLASS_FLAG asc";
                        //DataTable table_1 = _dBSQL.GetCommand(_sql);
                        //if (table_1 != null && table_1.Rows.Count > 0)
                        //{
                        //    row_1["RECORD_TIME"] = "历史最优";
                        //    row_2["RECORD_TIME"] = "今年最优";
                        //    row_3["RECORD_TIME"] = "今年平均";
                        //    row_4["RECORD_TIME"] = "连续3月最优";
                        //    row_5["RECORD_TIME"] = "连续6月最优";

                        //    row_1["SHIFT_FLAG"] = "-";
                        //    row_2["SHIFT_FLAG"] = "-";
                        //    row_3["SHIFT_FLAG"] = "-";
                        //    row_4["SHIFT_FLAG"] = "-";
                        //    row_5["SHIFT_FLAG"] = "-";
                        //    for (int x = 2; x < _Getdate_name.Count(); x++)
                        //    {
                        //        row_1[_Getdate_name[x]] = table_1.Rows[0][_Getdate_name[x]].ToString();//历史最优
                        //        row_2[_Getdate_name[x]] = table_1.Rows[1][_Getdate_name[x]].ToString();//今年最优
                        //        row_3[_Getdate_name[x]] = table_1.Rows[2][_Getdate_name[x]].ToString();//今年平均
                        //        row_4[_Getdate_name[x]] = table_1.Rows[3][_Getdate_name[x]].ToString();//连续3月最优
                        //        row_5[_Getdate_name[x]] = table_1.Rows[4][_Getdate_name[x]].ToString();//连续6月最优
                        //    }

                        //    _data.Rows.Add(row_2);
                        //    _data.Rows.Add(row_3);
                        //    _data.Rows.Add(row_4);
                        //    _data.Rows.Add(row_5);
                        //}

                        #endregion 最优数据

                        #region 基础数据组合

                        for (int x = 0; x < _Tem_date.Rows.Count; x++)
                        {
                            DataRow _row_now = _data.NewRow();
                            for (int y = 0; y < _Getdate_name.Count(); y++)
                            {
                                _row_now[_Getdate_name[y]] = _Tem_date.Rows[x][_Getdate_name[y]].ToString();
                            }
                            _data.Rows.Add(_row_now);
                        }
                    }

                    #endregion 基础数据组合

                    this.dataGridView1.DataSource = _data;
                }
                else if (_flag == 2)
                {
                    DataTable _data = new DataTable();
                    for (int x = 0; x < _Getdate_name.Count(); x++)
                    {
                        _data.Columns.Add(_Getdate_name[x]);
                    }

                    #region 最优数据

                    DataRow row_1 = _data.NewRow();//历史最优
                    DataRow row_2 = _data.NewRow();//今年最优
                    DataRow row_3 = _data.NewRow();//今年平均
                    DataRow row_4 = _data.NewRow();//连续3月最优
                    DataRow row_5 = _data.NewRow();//连续6月最优
                                                   // var _sql = "select R_003_CONTROL, R_005_CONTROL,R_008_CONTROL,FEO_06_CONTROL,FEO_1_CONTROL,MGO_015_CONTROL,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_R,C_ZN,C_AL2O3_SIO2,C_MGO_AL2O3,C_TOTAL,C_TI,RI,RDI_3_15,GRIT_5,GRIT_5_10,PATCL_AV,GRIT_10,GRIT_10_40,TFE_STANDARD,SIO2_STANDARD,CAO_STANDARD,R_STANDARD,FEO_STANDARD,MGO_STANDARD from MC_NUMCAL_INTERFACE_6_OPTIMAL where DL_FLAG = 1 order by CLASS_FLAG asc";
                    var _sql = "select * from MC_NUMCAL_INTERFACE_6_OPTIMAL where DL_FLAG = " + _flag + " order by CLASS_FLAG asc";
                    DataTable table_1 = _dBSQL.GetCommand(_sql);
                    if (table_1 != null && table_1.Rows.Count > 0)
                    {
                        row_1["RECORD_TIME"] = "历史最优";
                        row_2["RECORD_TIME"] = "今年最优";
                        row_3["RECORD_TIME"] = "今年平均";
                        row_4["RECORD_TIME"] = "连续3月最优";
                        row_5["RECORD_TIME"] = "连续6月最优";

                        row_1["SHIFT_FLAG"] = "-";
                        row_2["SHIFT_FLAG"] = "-";
                        row_3["SHIFT_FLAG"] = "-";
                        row_4["SHIFT_FLAG"] = "-";
                        row_5["SHIFT_FLAG"] = "-";
                        for (int x = 2; x < _Getdate_name.Count(); x++)
                        {
                            row_1[_Getdate_name[x]] = table_1.Rows[0][_Getdate_name[x]].ToString();//历史最优
                            row_2[_Getdate_name[x]] = table_1.Rows[1][_Getdate_name[x]].ToString();//今年最优
                            row_3[_Getdate_name[x]] = table_1.Rows[2][_Getdate_name[x]].ToString();//今年平均
                            row_4[_Getdate_name[x]] = table_1.Rows[3][_Getdate_name[x]].ToString();//连续3月最优
                            row_5[_Getdate_name[x]] = table_1.Rows[4][_Getdate_name[x]].ToString();//连续6月最优
                        }
                        _data.Rows.Add(row_1);
                        _data.Rows.Add(row_2);
                        _data.Rows.Add(row_3);
                        _data.Rows.Add(row_4);
                        _data.Rows.Add(row_5);
                    }

                    #endregion 最优数据

                    #region 基础数据组合

                    var _sql_1 = "";
                    if (_CLASS_NAME == "全部")
                    {
                        _sql_1 = "select * from MC_NUMCAL_INTERFACE_6_CLASS where DL_FLAG = " + _flag + " and RECORD_TIME >= '" + _time_begin + "' and RECORD_TIME <= '" + _time_end + "'";
                    }
                    else
                    {
                        _sql_1 = "select * from MC_NUMCAL_INTERFACE_6_CLASS where SHIFT_FLAG = '" + _CLASS_NAME + "' and  DL_FLAG = " + _flag + " and RECORD_TIME >= '" + _time_begin + "' and RECORD_TIME <= '" + _time_end + "'";
                    }
                    DataTable _date = _dBSQL.GetCommand(_sql_1);
                    if (_date != null && _date.Rows.Count > 0)
                    {
                        for (int x = 0; x < _date.Rows.Count; x++)
                        {
                            DataRow _row_now = _data.NewRow();
                            for (int y = 0; y < _Getdate_name.Count(); y++)
                            {
                                _row_now[_Getdate_name[y]] = _date.Rows[x][_Getdate_name[y]].ToString();
                            }
                            _data.Rows.Add(_row_now);
                        }
                    }

                    #endregion 基础数据组合

                    this.dataGridView2.DataSource = _data;
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("DATA_TEXT方法错误" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DATA_TEXT(3, comboBox1.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());//填充数据
            _Timer1.Enabled = true;                                                                                                  //  DATA_TEXT(2, comboBox1.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());//填充数据
        }

        /// <summary>
        /// 实时按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            dateTimePicker_value();
            DATA_TEXT(3, comboBox1.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());//填充数据
                                                                                                                // DATA_TEXT(2, comboBox1.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());//填充数据
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Execl files (*.xls)|*.xls";
                dlg.FilterIndex = 0;
                dlg.RestoreDirectory = true;
                dlg.CreatePrompt = true;
                dlg.Title = "保存为Excel文件";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream;
                    myStream = dlg.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                    string columnTitle = "";
                    try
                    {
                        //写入列标题
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (i > 0)
                            {
                                columnTitle += "\t";
                            }
                            columnTitle += dataGridView1.Columns[i].HeaderText;
                        }
                        sw.WriteLine(columnTitle);

                        //写入列内容
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            string columnValue = "";
                            for (int k = 0; k < dataGridView1.Columns.Count; k++)
                            {
                                if (k > 0)
                                {
                                    columnValue += "\t";
                                }
                                if (dataGridView1.Rows[j].Cells[k].Value == null)
                                    columnValue += "";
                                else
                                    columnValue += dataGridView1.Rows[j].Cells[k].Value.ToString().Trim();
                            }
                            sw.WriteLine(columnValue);
                        }
                        sw.Close();
                        myStream.Close();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(e.ToString());
                    }
                    finally
                    {
                        sw.Close();
                        myStream.Close();
                    }
                }
            }
            catch
            {
            }
        }
    }
}