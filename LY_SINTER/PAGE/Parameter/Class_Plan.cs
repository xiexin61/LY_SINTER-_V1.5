using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LY_SINTER.Custom;
using DataBase;
using VLog;

namespace LY_SINTER.PAGE.Parameter
{
    public partial class Class_Plan : UserControl
    {
        public vLog _vLog { get; set; }
        public System.Timers.Timer _Timer1 { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        Color[] vs = { Color.Red, Color.Blue, Color.Black };
        DateTimePicker dtp_begin = new DateTimePicker();  //实例化一个DateTimePicker控件 开始时间
        /// <summary>
        /// key 序号，item1 班组 item2 修改时间 item3 开始时间 item4 结束时间 item5 白班夜班
        /// </summary>
        Dictionary<int, Tuple<string, DateTime, DateTime, DateTime, string>> dic = new Dictionary<int, Tuple<string, DateTime, DateTime, DateTime, string>>();
        /// <summary>
        /// 当班id  M_CLASS_PLAN_CFG表ID字段
        /// </summary>
        int CLASS_ID;
        /// <summary>
        /// 判断当班时间是否在排班指定表中
        /// </summary>
        bool FLAG = false;
        /// <summary>
        /// 排班规则的开始时间和结束时间和当前时间进行对比，判断是否唯一
        /// </summary>
        bool FLAG_1 = false;
        public Class_Plan()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Parameter\\Class_Plan_Page\\");
            Jurisdiction();
            textBox_begin.Text = DateTime.Now.ToString();
            DateTimeChoser.AddTo(textBox_begin);
            _Timer1 = new System.Timers.Timer(1000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { _Timer1_Tick(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            d1_show();
            d2_show();
            XLK();
            time_new();
            Cloums_Color();
            // TIMER_MON();
            // Color_CLASS();

        }
        private void _Timer1_Tick()
        {
            Action invokeAction = new Action(_Timer1_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                Color_CLASS();
            }
        }
        /// <summary>
        /// 权限
        /// </summary>
        public void Jurisdiction()
        {
          
        }

        /// <summary>
        /// 时间下拉框
        /// </summary>
        public void XLK()
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("name");
                dataTable.Columns.Add("value");

                DataRow row1 = dataTable.NewRow();
                row1["name"] = "12小时工作制";
                row1["value"] = 12;
                dataTable.Rows.Add(row1);

                DataRow row = dataTable.NewRow();
                row["name"] = "8小时工作制";
                row["value"] = 8;
                dataTable.Rows.Add(row);

                comboBox1.DataSource = dataTable;
                comboBox1.DisplayMember = "name";
                comboBox1.ValueMember = "value";
            }
            catch(Exception ee)
            {
                var mistake = "XLK方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
           

        }
        /// <summary>
        /// 最新调整时间
        /// </summary>
        public void time_new()
        {
            //最新调整时间
            string sql_time = "select top 1 TIMESTAMP  from M_CLASS_PLAN where TIMESTAMP = (select max(TIMESTAMP) from M_CLASS_PLAN)";
            DataTable data_time = dBSQL.GetCommand(sql_time);
            if (data_time.Rows.Count > 0)
            {
                label2.Text = "最新调整时间为:" + data_time.Rows[0][0].ToString();
            }
        }
        public void d1_show()
        {
            try
            {
                string sql_M_CLASS_PLAN_CFG = "select ID ,TIMESTAMP,SHIFT_FLAG,START_TIME,END_TIME,DAY_NIGHT as Class_name from M_CLASS_PLAN_CFG order by ID asc";
                DataTable data_M_CLASS_PLAN_CFG = dBSQL.GetCommand(sql_M_CLASS_PLAN_CFG);
                if (data_M_CLASS_PLAN_CFG.Rows.Count > 0)
                {
                    d1.DataSource = data_M_CLASS_PLAN_CFG;
                    textBox1.Text = data_M_CLASS_PLAN_CFG.Rows.Count.ToString();
                }
            }
            catch(Exception ee)
            {
                _vLog.writelog("d1_show方法失败" + ee.ToString(),-1);
            }
        }
        public void d2_show()
        {
            try
            {
                string sql_M_CLASS_PLAN = "select ID ,SHIFT_FLAG,START_TIME,END_TIME ,DAY_NIGHT FROM M_CLASS_PLAN ORDER BY ID ASC";
                DataTable data_M_CLASS_PLAN = dBSQL.GetCommand(sql_M_CLASS_PLAN);
                //查询出来的数据分成两列
                int rows = data_M_CLASS_PLAN.Rows.Count;
                if (rows == 120)
                {
                    DataTable data = new DataTable();
                    data.Columns.Add("ID_1");
                    data.Columns.Add("CLASS_1");
                    data.Columns.Add("TIME_BEGIN_1");
                    data.Columns.Add("TIME_END_1");
                    data.Columns.Add("CLASS_NAME_1");
                    data.Columns.Add("ID_2");
                    data.Columns.Add("CLASS_2");
                    data.Columns.Add("TIME_BEGIN_2");
                    data.Columns.Add("TIME_END_2");
                    data.Columns.Add("CLASS_NAME_2");
                    data.Columns.Add("ID_3");
                    data.Columns.Add("CLASS_3");
                    data.Columns.Add("TIME_BEGIN_3");
                    data.Columns.Add("TIME_END_3");
                    data.Columns.Add("CLASS_NAME_3");
                    //前60条数据
                    int rows_1 = 0;
                    //后60条数据
                    int rows_2 = 40;
                    int rows_3 = 80;
                    for (int x = 0; x < 40; x++)
                    {
                        DataRow row = data.NewRow();
                        //前60条数据
                        row["ID_1"] = data_M_CLASS_PLAN.Rows[rows_1]["ID"].ToString();
                        row["CLASS_1"] = data_M_CLASS_PLAN.Rows[rows_1]["SHIFT_FLAG"].ToString();
                        row["TIME_BEGIN_1"] = data_M_CLASS_PLAN.Rows[rows_1]["START_TIME"].ToString();
                        row["TIME_END_1"] = data_M_CLASS_PLAN.Rows[rows_1]["END_TIME"].ToString();
                        row["CLASS_NAME_1"] = data_M_CLASS_PLAN.Rows[rows_1]["DAY_NIGHT"].ToString();
                        rows_1 = rows_1 + 1;
                        //后60条数据
                        row["ID_2"] = data_M_CLASS_PLAN.Rows[rows_2]["ID"].ToString();
                        row["CLASS_2"] = data_M_CLASS_PLAN.Rows[rows_2]["SHIFT_FLAG"].ToString();
                        row["TIME_BEGIN_2"] = data_M_CLASS_PLAN.Rows[rows_2]["START_TIME"].ToString();
                        row["TIME_END_2"] = data_M_CLASS_PLAN.Rows[rows_2]["END_TIME"].ToString();
                        row["CLASS_NAME_2"] = data_M_CLASS_PLAN.Rows[rows_2]["DAY_NIGHT"].ToString();
                        rows_2 = rows_2 + 1;
                        row["ID_3"] = data_M_CLASS_PLAN.Rows[rows_3]["ID"].ToString();
                        row["CLASS_3"] = data_M_CLASS_PLAN.Rows[rows_3]["SHIFT_FLAG"].ToString();
                        row["TIME_BEGIN_3"] = data_M_CLASS_PLAN.Rows[rows_3]["START_TIME"].ToString();
                        row["TIME_END_3"] = data_M_CLASS_PLAN.Rows[rows_3]["END_TIME"].ToString();
                        row["CLASS_NAME_3"] = data_M_CLASS_PLAN.Rows[rows_3]["DAY_NIGHT"].ToString();
                        rows_3 = rows_3 + 1;
                        data.Rows.Add(row);
                    }
                    d2.DataSource = data;
                    this.d2.Columns["Column1"].DefaultCellStyle.BackColor = Color.LightGray;
                    this.d2.Columns["Column5"].DefaultCellStyle.BackColor = Color.LightGray;
                    this.d2.Columns["Column11"].DefaultCellStyle.BackColor = Color.LightGray;
                }
                else
                {
                    d2.DataSource = data_M_CLASS_PLAN;
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("d2_show方法失败" + ee.ToString() ,-1);
            }
        }
     

        /// <summary>
        /// 更新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resule = MessageBox.Show("是否修改排班规则", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                string a = resule.ToString();
                if (a == "OK")
                {
                    dic.Clear();

                    for (int rows = 0; rows < d1.Rows.Count; rows++)
                    {
                        //序号
                        int XH = int.Parse(d1.Rows[rows].Cells["ID"].Value.ToString());
                        //班组
                        string CLASS = d1.Rows[rows].Cells["CLASS"].Value.ToString();
                        //修改时间
                        DateTime TIME_CHANGE = DateTime.Parse(d1.Rows[rows].Cells["TIME"].Value.ToString());
                        //开始时间
                        DateTime TIME_BEGIN = DateTime.Parse(d1.Rows[rows].Cells["TIME_BEGIN"].Value.ToString());
                        //结束时间
                        DateTime time_END = DateTime.Parse(d1.Rows[rows].Cells["TIME_END"].Value.ToString());
                        //白班、夜班
                        string CLASS_NAME = d1.Rows[rows].Cells["Class_name"].Value.ToString();
                        if (CLASS != "")
                        {
                            if (dic.ContainsKey(XH))
                            {
                                dic.Remove(XH);
                                Tuple<string, DateTime, DateTime, DateTime, string> TUP = new Tuple<string, DateTime, DateTime, DateTime, string>(CLASS, TIME_CHANGE, TIME_BEGIN, time_END, CLASS_NAME);
                                dic.Add(XH, TUP);
                            }
                            else
                            {
                                Tuple<string, DateTime, DateTime, DateTime, string> TUP = new Tuple<string, DateTime, DateTime, DateTime, string>(CLASS, TIME_CHANGE, TIME_BEGIN, time_END, CLASS_NAME);
                                dic.Add(XH, TUP);
                            }
                        }
                        else
                        {
                            MessageBox.Show("请输入正确的班次规则");
                            return;
                        }


                    }
                    //查询当班班次对应的id
                    DateTime date_now = DateTime.Now;
                    string xx = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                    int rows_now = 0;
                    foreach (var x in dic)
                    {
                        if (x.Value.Item3 <= date_now && x.Value.Item4 > date_now)
                        {
                            CLASS_ID = x.Key;
                            rows_now = rows_now + 1;
                            FLAG = true;
                        }
                    }
                    if (rows_now > 1)
                    {
                        FLAG_1 = false;
                    }
                    else
                    {
                        FLAG_1 = true;
                    }
                    if (FLAG)
                    {
                        if (FLAG_1)
                        {
                            //删除M_CLASS_PLAN_CFG表已经存在的排班规则
                            string del_M_CLASS_PLAN_CFG = "delete from M_CLASS_PLAN_CFG";
                            dBSQL.CommandExecuteNonQuery(del_M_CLASS_PLAN_CFG);
                            //用户修改的排班规则更新到数据库中M_CLASS_PLAN_CFG
                            foreach (var x in dic)
                            {
                                string sql_insert = "insert into M_CLASS_PLAN_CFG (ID,TIMESTAMP,SHIFT_FLAG,START_TIME,END_TIME,DAY_NIGHT) values ('" + x.Key + "','" + x.Value.Item2 + "','" + x.Value.Item1 + "','" + x.Value.Item3 + "','" + x.Value.Item4 + "','" + x.Value.Item5 + "')";
                                dBSQL.CommandExecuteNonQuery(sql_insert);
                            }

                            //删除M_CLASS_PLAN已经存在的排班计划
                            string del_M_CLASS_PLAN = "delete from M_CLASS_PLAN";
                            dBSQL.CommandExecuteNonQuery(del_M_CLASS_PLAN);

                            //通过用户输入的规则制度计算120条排班计划
                            //行数周期循环
                            int row = 0;
                            //页面行数
                            int row_YE = d1.Rows.Count;
                            //班次周期
                            int count = int.Parse(textBox1.Text.ToString());
                            //系统当前时间
                            DateTime time = DateTime.Now;
                            //工作时间
                            double time_add = double.Parse(comboBox1.SelectedValue.ToString());
                            //当班开始时间
                            string date_begin_1 = DateTime.Parse(textBox_begin.Text.ToString()).ToString("yyyy-MM-dd HH:mm");
                            DateTime date_begin = DateTime.Parse(date_begin_1);
                            DateTime date_end = date_begin.AddHours(time_add).AddMinutes(-1);
                            //页面计算周期
                            for (int rows = 1; rows <= 120; rows++)
                            {
                                //通过行数周期序号和页面行数进行判断，如果行数周期循环大于页面行数，则把行数周期循环标志位置成0，重新去获取页面排版规则
                                if (row < row_YE)
                                {
                                    //M_CLASS_PLAN表FLAG标志位
                                    int FLAG = row + 1;
                                    //班次名称
                                    string CLASS_NAME = d1.Rows[row].Cells["CLASS"].Value.ToString();
                                    //白班、夜班
                                    string DAY_NIGHT = d1.Rows[row].Cells["Class_name"].Value.ToString();

                                    string sql_insert_M_CLASS_PLAN = "insert into M_CLASS_PLAN (ID,FLAG,TIMESTAMP,SHIFT_FLAG,START_TIME,END_TIME,DAY_NIGHT) values ('" + rows + "','" + FLAG + "','" + time + "','" + CLASS_NAME + "','" + date_begin + "','" + date_end + "','" + DAY_NIGHT + "')";
                                    dBSQL.CommandExecuteNonQuery(sql_insert_M_CLASS_PLAN);
                                    row = row + 1;
                                }
                                else
                                {
                                    row = 0;
                                    //M_CLASS_PLAN表FLAG标志位
                                    int FLAG = row + 1;
                                    //班次名称
                                    string CLASS_NAME = d1.Rows[row].Cells["CLASS"].Value.ToString();
                                    //白班、夜班
                                    string DAY_NIGHT = d1.Rows[row].Cells["Class_name"].Value.ToString();

                                    string sql_insert_M_CLASS_PLAN = "insert into M_CLASS_PLAN (ID,FLAG,TIMESTAMP,SHIFT_FLAG,START_TIME,END_TIME,DAY_NIGHT) values ('" + rows + "','" + FLAG + "','" + time + "','" + CLASS_NAME + "','" + date_begin + "','" + date_end + "','" + DAY_NIGHT + "')";
                                    dBSQL.CommandExecuteNonQuery(sql_insert_M_CLASS_PLAN);
                                    row = row + 1;
                                }
                                date_begin = date_begin.AddHours(time_add);
                                date_end = date_end.AddHours(time_add);
                            }

                            //向M_CLASS_PLAN_HOUR表插入工作时间制度
                            //获取页面用户选择的工作时间制度
                            int time_add_YM = int.Parse(comboBox1.SelectedValue.ToString());

                            string sql_M_CLASS_PLAN_HOUR = "insert into M_CLASS_PLAN_HOUR (TIMESTAMP,HOUR) values ('" + time + "','" + time_add_YM + "')";
                            dBSQL.CommandExecuteNonQuery(sql_M_CLASS_PLAN_HOUR);

                            MessageBox.Show("操作成功");
                            d2_show();
                            time_new();
                            Color_CLASS();
                            Cloums_Color();
                        }
                        else
                        {
                            MessageBox.Show("排班规则时间冲突");
                        }



                    }
                    else
                    {
                        MessageBox.Show("排班规划中请输入当前班组的工作时间");
                    }

                }
                else
                {
                    return;
                }

            }
            catch(Exception ee)
            {
                _vLog.writelog("更新按钮点击失败"+ee.ToString(), -1);
            }

        }
        /// <summary>
        /// 时间生成按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                //班次周期
                if (textBox1.Text.ToString() == "")
                {
                    MessageBox.Show("请输入班次计算周期");
                    return;
                }
                else
                {

                    int ROES = int.Parse(textBox1.Text.ToString());
                    DateTime time = DateTime.Now;
                    DataTable data = new DataTable();

                    data.Columns.Add("ID");

                    data.Columns.Add("TIMESTAMP");
                    data.Columns.Add("SHIFT_FLAG");
                    data.Columns.Add("START_TIME");
                    data.Columns.Add("END_TIME");
                    data.Columns.Add("Class_name");
                    for (int rows = 0; rows < ROES; rows++)
                    {
                        DataRow dr = data.NewRow();
                        dr["ID"] = rows + 1;
                        dr["SHIFT_FLAG"] = null;
                        dr["TIMESTAMP"] = time;
                        dr["START_TIME"] = null;
                        dr["END_TIME"] = null;
                        dr["Class_name"] = null;
                        data.Rows.Add(dr);
                    }
                    d1.DataSource = data;
                    //设置可编辑部分背景颜色

                    this.d1.Columns["CLASS"].DefaultCellStyle.BackColor = Color.LightGreen;


                    //班次周期
                    int count = int.Parse(textBox1.Text.ToString());
                    //工作时间
                    double time_add = double.Parse(comboBox1.SelectedValue.ToString());
                    //当班开始时间
                    string date_begin_1 = DateTime.Parse(textBox_begin.Text.ToString()).ToString("yyyy-MM-dd HH:mm");
                    DateTime date_begin = DateTime.Parse(date_begin_1);
                    DateTime date_end = date_begin.AddHours(time_add).AddMinutes(-1);
                    for (int row = 0; row < count; row++)
                    {

                        //判断用户排班工作制度，12小时工作制需要区分白班夜班，8小时工作制不需要区分
                        if (time_add == 8)
                        {
                            //当班开始时间
                            d1.Rows[row].Cells["TIME_BEGIN"].Value = date_begin;
                            date_begin = date_begin.AddHours(time_add);
                            //当班结束时间
                            d1.Rows[row].Cells["TIME_END"].Value = date_end;
                            date_end = date_end.AddHours(time_add);
                            //白班、夜班
                            d1.Rows[row].Cells["Class_name"].Value = "8小时工作制不区分";

                        }
                        else if (time_add == 12)
                        {
                            //当班开始时间
                            d1.Rows[row].Cells["TIME_BEGIN"].Value = date_begin;
                            //      d1.Rows[row].Cells["Column11"].Value = "白班";

                            //白班、夜班
                            if (date_begin.Hour >= 0 && date_begin.Hour < 12)
                            {
                                d1.Rows[row].Cells["Class_name"].Value = "白班";

                                // d1.Rows[row].Cells["CLASS_NAME"].Value = 1;
                            }
                            else if (date_begin.Hour >= 12 && date_begin.Hour < 24)
                            {
                                this.d1.Rows[row].Cells["Class_name"].Value = "夜班";
                            }
                            else
                            {
                                this.d1.Rows[row].Cells["Class_name"].Value = "未知";
                            }
                            date_begin = date_begin.AddHours(time_add);
                            //当班结束时间
                            d1.Rows[row].Cells["TIME_END"].Value = date_end;
                            date_end = date_end.AddHours(time_add);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("时间生成按钮点击失败" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 定时刷新d2部分当班显示部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void Color_CLASS()
        {
            try
            {
                //当前时间
                DateTime date = DateTime.Now;
                string sql_M_CLASS_PLAN = "select ID,SHIFT_FLAG,START_TIME,END_TIME from M_CLASS_PLAN where  START_TIME <='" + date + "' and END_TIME >= '" + date + "'";
                DataTable data_M_CLASS_PLAN = dBSQL.GetCommand(sql_M_CLASS_PLAN);
                if (data_M_CLASS_PLAN.Rows.Count > 0)
                {
                    int ID = int.Parse(data_M_CLASS_PLAN.Rows[0]["ID"].ToString());
                    string CLASS_NAME = data_M_CLASS_PLAN.Rows[0]["SHIFT_FLAG"].ToString();

                    for (int rows = 0; rows < d2.Rows.Count; rows++)
                    {
                        
                        if (ID == int.Parse(d2.Rows[rows].Cells["Column1"].Value.ToString()))
                        {
                            this.d2.Rows[rows].Cells["Column1"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column2"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column3"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column4"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column9"].Style.BackColor = Color.LightSeaGreen;

                        }
                        else if (ID == int.Parse(d2.Rows[rows].Cells["Column5"].Value.ToString()))
                        {
                            this.d2.Rows[rows].Cells["Column5"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column6"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column7"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column8"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column10"].Style.BackColor = Color.LightSeaGreen;
                        }
                        else if (ID == int.Parse(d2.Rows[rows].Cells["Column11"].Value.ToString()))
                        {
                            this.d2.Rows[rows].Cells["Column11"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column12"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column13"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column14"].Style.BackColor = Color.LightSeaGreen;
                            this.d2.Rows[rows].Cells["Column15"].Style.BackColor = Color.LightSeaGreen;
                        }

                    }
                }
                else
                {
                    return;
                }
            }
            catch(Exception ee)
            {
                _vLog.writelog("Color_CLASS方法失败" + ee.ToString(),-1);
            }
           

        }
        /// <summary>
        /// 定时器启用
        /// </summary>
        public void Timer_state()
        {
            _Timer1.Enabled = true;
        }
        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
            _Timer1.Enabled = false;
        }
        /// <summary>
        /// 控件关闭
        /// </summary>
        public void _Clear()
        {
            _Timer1.Close();//释放定时器资源
            this.Dispose();//释放资源
            GC.Collect();//调用GC
        }
        /// <summary>
        /// 颜色变化
        /// </summary>
        public void Cloums_Color()
        {

            //  this.d2.Columns[0].DefaultCellStyle.BackColor = vs[0];//背景颜色
            this.d2.Columns[1].DefaultCellStyle.ForeColor = vs[0];//背景颜色
            this.d2.Columns[2].DefaultCellStyle.ForeColor = vs[0];//背景颜色
            this.d2.Columns[3].DefaultCellStyle.ForeColor = vs[0];//背景颜色
            this.d2.Columns[4].DefaultCellStyle.ForeColor = vs[0];//背景颜色

            // this.d2.Columns[5].DefaultCellStyle.BackColor = vs[1];//背景颜色
            this.d2.Columns[6].DefaultCellStyle.ForeColor = vs[1];//背景颜色
            this.d2.Columns[7].DefaultCellStyle.ForeColor = vs[1];//背景颜色
            this.d2.Columns[8].DefaultCellStyle.ForeColor = vs[1];//背景颜色
            this.d2.Columns[9].DefaultCellStyle.ForeColor = vs[1];//背景颜色

            // this.d2.Columns[10].DefaultCellStyle.BackColor = vs[2];//背景颜色
            this.d2.Columns[11].DefaultCellStyle.ForeColor = vs[2];//背景颜色
            this.d2.Columns[12].DefaultCellStyle.ForeColor = vs[2];//背景颜色
            this.d2.Columns[13].DefaultCellStyle.ForeColor = vs[2];//背景颜色
            this.d2.Columns[14].DefaultCellStyle.ForeColor = vs[2];//背景颜色


        }

        private void d2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
