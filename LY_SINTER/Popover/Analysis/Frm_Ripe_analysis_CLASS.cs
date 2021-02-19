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

namespace LY_SINTER.Popover.Analysis
{

    public partial class Frm_Ripe_analysis_CLASS : Form
    {
        DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        public static bool isopen = false;
        string[] _Getdate_name = { "RECORD_TIME",
                                    "SHIFT_FLAG",
                                    "R_003_CONTROL",
                                    "R_005_CONTROL",
                                    "R_008_CONTROL",
                                    "FEO_06_CONTROL",
                                    "FEO_1_CONTROL",
                                    "MGO_015_CONTROL",
                                    "C_TFE",
                                    "C_FEO",
                                    "C_CAO",
                                    "C_SIO2",
                                    "C_AL2O3",
                                    "C_MGO",
                                    "C_R",
                                    "C_ZN",
                                    "C_AL2O3_SIO2",
                                    "C_MGO_AL2O3",
                                    "C_TOTAL",
                                    "C_TI",
                                    "RI",
                                    "RDI_3_15",
                                    "GRIT_5",
                                    "GRIT_5_10",
                                    "PATCL_AV",
                                    "GRIT_10",
                                    "GRIT_10_40",
                                    "TFE_STANDARD",
                                    "SIO2_STANDARD",
                                    "CAO_STANDARD",
                                    "R_STANDARD",
                                    "FEO_STANDARD",
                                    "MGO_STANDARD"
                                    };
        public vLog _vLog { get; set; }
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
        public void DATA_TEXT(int _flag,string _CLASS_NAME,string _time_begin,string _time_end)
        {
            try
            {
                if (_flag == 3)
                {
                    DataTable _data = new DataTable();
                    for(int x = 0; x < _Getdate_name.Count(); x++)
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
                    var _sql = "select * from MC_NUMCAL_INTERFACE_6_OPTIMAL where DL_FLAG = "+ _flag + " order by CLASS_FLAG asc";
                    DataTable table_1 = _dBSQL.GetCommand(_sql);
                    if (table_1 != null && table_1.Rows.Count > 0 )
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
                        for (int x = 2;x< _Getdate_name.Count();x++)
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

                    #endregion

                    #region 基础数据组合
                    var _sql_1 = "";
                    if (_CLASS_NAME == "全部")
                    {
                         _sql_1 = "select * from MC_NUMCAL_INTERFACE_6_CLASS where DL_FLAG = " + _flag + " and RECORD_TIME >= '" + _time_begin + "' and RECORD_TIME <= '" + _time_end + "'";
                    }
                    else
                    {
                         _sql_1 = "select * from MC_NUMCAL_INTERFACE_6_CLASS where SHIFT_FLAG = '"+ _CLASS_NAME + "' and  DL_FLAG = " + _flag + " and RECORD_TIME >= '" + _time_begin + "' and RECORD_TIME <= '" + _time_end + "'";
                    }
                    DataTable _date = _dBSQL.GetCommand(_sql_1);
                    if (_date != null && _date.Rows.Count > 0 )
                    {
                        for (int x = 0; x < _date.Rows.Count;x++)
                        {
                            DataRow _row_now = _data.NewRow();
                            for (int y = 0; y< _Getdate_name.Count();y++)
                            {
                                _row_now[_Getdate_name[y]] = _date.Rows[x][_Getdate_name[y]].ToString();
                            }
                            _data.Rows.Add(_row_now);
                        }
                    }
                    #endregion
                    this.dataGridView1.DataSource = _data;
                }
                else if(_flag == 2)
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

                    #endregion

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
                    #endregion

                    this.dataGridView2.DataSource = _data;
                }

            }
            catch(Exception ee)
            {
                _vLog.writelog("DATA_TEXT方法错误"+ee.ToString(),-1);
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
          //  DATA_TEXT(2, comboBox1.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());//填充数据
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
    }
}
