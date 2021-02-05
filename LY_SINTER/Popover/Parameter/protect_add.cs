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

namespace LY_SINTER.Popover.Parameter
{
    public partial class protect_add : Form
    {
        /// <summary>
        /// 初始化颜色变化定时器
        /// </summary>
        public System.Timers.Timer _Timer1 { get; set; }
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        //声明委托和事件
        public delegate void TransfDelegate_YLBH_ADD();
        int _flag = 0;
        public vLog _vLog { get; set; }
        public protect_add()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Parametery\\Ingredient_Protect_Pop_ADD\\");
            Classify();
            Data_Init();
            _Timer1 = new System.Timers.Timer(1000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { _Timer1_Tick(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }
        /// <summary>
        /// 初始化颜色变化定时器响应事件
        /// </summary>
        private void _Timer1_Tick()
        {
            Action invokeAction = new Action(_Timer1_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                Color_Change();
            }
        }
        //声明委托和事件
        public event TransfDelegate_YLBH_ADD _transfDelegate_YLBH_ADD;
        /// <summary>
        /// 物料归属下拉框赋值
        /// </summary>
        private void Classify()
        {
            try
            {
                string sql = "select M_TYPE as value,M_DESC as name,CODE_MIN,CODE_MAX from M_MATERIAL_COOD_CONFIG order by M_TYPE asc";
                DataTable dataTable = dBSQL.GetCommand(sql);
                DataRow dr = dataTable.NewRow();
                dr["Name"] = "请选择物料";
                dr["value"] = 9;
                dr["CODE_MIN"] = 101;
                dr["CODE_MAX"] = 800;
                dataTable.Rows.Add(dr);
                this.comboBox1.DataSource = dataTable;
                this.comboBox1.DisplayMember = "name";
                this.comboBox1.ValueMember = "value";
                this.comboBox1.SelectedIndex = 9;
            }
            catch(Exception ee)
            {
                var mistake = "Classify方式失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combox_Code();
        }
        /// <summary>
        /// 二级编码自动生成
        /// </summary>
        private void Combox_Code()
        {
            try
            {
                this.textBox1.Text = null;
                int WLGS_NAME = comboBox1.SelectedIndex;
                if (WLGS_NAME != 9)
                {
                    //生成的二级编码
                    int code_new = 0;
                    WLGS_NAME = WLGS_NAME + 1;
                    _flag = WLGS_NAME;
                    string sql_1 = "select M_TYPE as value,M_DESC as name,CODE_MIN,CODE_MAX from M_MATERIAL_COOD_CONFIG where M_TYPE = '" + WLGS_NAME + "' order by M_TYPE asc";
                    DataTable dataTable = dBSQL.GetCommand(sql_1);
                    //原料最小值
                    int raw_min = int.Parse(dataTable.Rows[0]["CODE_MIN"].ToString());
                    //原料最大值
                    int raw_max = int.Parse(dataTable.Rows[0]["CODE_MAX"].ToString());
                    string sql_2 = " select max(L2_CODE) as L2_CODE from M_MATERIAL_COOD where L2_CODE >= " + raw_min + " and L2_CODE <= " + raw_max + " order by L2_CODE asc";
                    DataTable dataTable_2 = dBSQL.GetCommand(sql_2);
                    //判断查询的二级编码是否存在，不存在则查询M_MATERIAL_COOD_CONFIG表的归属范围
                    if (dataTable_2.Rows[0]["L2_CODE"].ToString() != "")
                    {
                        code_new = int.Parse(dataTable_2.Rows[0]["L2_CODE"].ToString()) + 1;
                    }
                    else
                    {
                        code_new = raw_min;
                    }
                    this.textBox1.Text = code_new.ToString();
                }
                else
                {
                    return;
                }
            }
            catch(Exception ee)
            {
                var mistake = "Combox_Code方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return;
            }
            
        }
        /// <summary>
        /// 初始化赋值，上限100，下限0
        /// </summary>
        public void Data_Init()
        {
            int index = this.dataGridView1.Rows.Add();
            for (int x = 0; x < dataGridView1.ColumnCount; x++)
            {
                int xx = x % 2;
                if (xx == 0)
                {
                    this.dataGridView1.Rows[index].Cells[x].Value = 100;
                }
                else
                {
                    this.dataGridView1.Rows[index].Cells[x].Value = 0;
                }
            }
        }

        /// <summary>
        /// 上下限颜色区分
        /// </summary>
        public void Color_Change()
        {
            try
            {
                for (int x = 0; x < dataGridView1.Columns.Count; x++)
                {
                    if (x % 2 == 0)
                    {
                        this.dataGridView1.Rows[0].Cells[x].Style.ForeColor = Color.Red;
                    }
                }
            }
            catch(Exception ee)
            {
                var mistake = "Color_Change方法失败" + ee.ToString();
            }
            
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //判断物料描述是否和数据库重复标志位
                bool WLMS_SINGEL = false;
                //三级编码重复
                bool L3_CODE_FIAG = false;
                //插入时间
                DateTime dateTime = DateTime.Now;
                //二级编码
                string L2_CODE = textBox1.Text.ToString();
                //三级编码
                string L3_CODE = textBox2.Text.ToString();
                //物料描述
                string WLMS = textBox3.Text.ToString();
                //产地
                string CD = textBox6.Text.ToString();
                //单价
                string DJ = textBox5.Text.ToString() == "" ? "NULL" : textBox5.Text.ToString();

                string sql_1 = "select MAT_DESC from M_MATERIAL_COOD where MAT_DESC = '" + WLMS + "' ";
                DataTable dataTable = dBSQL.GetCommand(sql_1);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    WLMS_SINGEL = true;
                }
                if (L2_CODE == "")
                {
                    MessageBox.Show("请选择物料种类");
                    return;
                }
                else
                {
                    if (L3_CODE == "")
                    {
                        MessageBox.Show("请输入正确的三级编码");
                        return;
                    }
                    else
                    {
                        if (WLMS == "")
                        {
                            MessageBox.Show("请输入物料描述");
                            return;
                        }
                        else
                        {

                                if (WLMS_SINGEL)
                                {
                                    MessageBox.Show("人工输入的物料描述 : " + WLMS + " 重复");
                                    return;
                                }
                                else
                                {

                                        if (L3_CODE_FIAG)
                                        {
                                            MessageBox.Show("人工输入的三级编码重复");
                                            return;
                                        }
                                        else if (false)
                                        {
                                            MessageBox.Show("请输入产地");
                                            return;
                                        }
                                        else if (false)
                                        {
                                            MessageBox.Show("请输入单价");
                                            return;
                                        }
                                        else

                                        {
                                    // string sql_insert = "INSERT INTO M_MATERIAL_COOD (TIMESTAMP,L2_CODE,L3_CODE,MAT_DESC)VALUES('" + dateTime + "'," + L2_CODE + "," + L3_CODE + ",'" + WLMS + "')";
                                    //   dBSQL.CommandExecuteNonQuery(sql_insert);
                                    //**begin 20200717添加MC_TYPE判断条件**//
                                    //int _L2_CODE = int.Parse(L2_CODE.ToString());
                                    //if (_L2_CODE >= 120 && _L2_CODE <= 299)
                                    //{
                                    //    string sql_insert = "INSERT INTO M_MATERIAL_COOD (TIMESTAMP,L2_CODE,L3_CODE,MAT_DESC,UNIT_PRICE,PLACE_ORIGIN)VALUES('" + dateTime + "'," + L2_CODE + ",'" + L3_CODE + "','" + WLMS + "','"+DJ+"','"+ CD+"')";
                                    //    dBSQL.CommandExecuteNonQuery(sql_insert);
                                    //    //向M_IRON_MATERIAL_NEW_RECORD表插入数据
                                    //////    string sql_inser1 = "INSERT INTO M_IRON_MATERIAL_NEW_RECORD (TIMESTAMP,L2_CODE,L3_CODE,M_DESC,FLAG) VALUES (getdate(),'" + L2_CODE + "','" + L3_CODE + "','" + WLMS + "','1')";
                                    //////    dBSQL.CommandExecuteNonQuery(sql_inser1);
                                    //}
                                    //else
                                    //{
                                    //    string sql_insert = "INSERT INTO M_MATERIAL_COOD (TIMESTAMP,L2_CODE,L3_CODE,MAT_DESC,)VALUES('" + dateTime + "'," + L2_CODE + ",'" + L3_CODE + "','" + WLMS + "')";
                                    //    dBSQL.CommandExecuteNonQuery(sql_insert);
                                    //}
                                    string sql_insert = "INSERT INTO M_MATERIAL_COOD (TIMESTAMP,L2_CODE,L3_CODE,MAT_DESC,UNIT_PRICE,PLACE_ORIGIN,MY_TYPE)VALUES('" + dateTime + "'," + L2_CODE + ",'" + L3_CODE + "','" + WLMS + "'," + DJ + ",'" + CD + "',"+_flag+")";
                                    dBSQL.CommandExecuteNonQuery(sql_insert);
                                    for (int x = 0; x < dataGridView1.ColumnCount; x++)
                                   {
                                                string name = dataGridView1.Columns[x].Name;
                                                float value = float.Parse(dataGridView1.Rows[0].Cells[x].Value.ToString());
                                                string sql_update = "UPDATE M_MATERIAL_COOD SET " + name + " = " + value + " WHERE  TIMESTAMP = '" + dateTime + "'";
                                                dBSQL.CommandExecuteNonQuery(sql_update);

                                    }
                                            ////判断插入的数据是不是属于矿粉种类，如果是MC_TYPE字段为2
                                            ////查询
                                            //string sql_KF = "select CODE_MIN,CODE_MAX from M_MATERIAL_COOD_CONFIG where M_TYPE = '2'";
                                            //DataTable dataTable_FK = dBSQL.GetCommand(sql_KF);
                                            //int CODE_MIN = int.Parse(dataTable_FK.Rows[0]["CODE_MIN"].ToString());
                                            //int CODE_MAX = int.Parse(dataTable_FK.Rows[0]["CODE_MAX"].ToString());
                                            //if (int.Parse(L2_CODE.ToString()) >= CODE_MIN && int.Parse(L2_CODE.ToString()) <= CODE_MAX)
                                            //{
                                            //    string sql_update = "UPDATE M_MATERIAL_COOD SET MC_TYPE = '2' WHERE  TIMESTAMP = '" + dateTime + "'";
                                            //    dBSQL.CommandExecuteNonQuery(sql_update);
                                            //}
                                            //else
                                            //{
                                            //    string sql_update = "UPDATE M_MATERIAL_COOD SET MC_TYPE = '1' WHERE  TIMESTAMP = '" + dateTime + "'";
                                            //    dBSQL.CommandExecuteNonQuery(sql_update);
                                            //}
                                            MessageBox.Show("操作成功");
                                            var text = "用户点击添加按钮，添加物料:"+WLMS+"成功";
                                            _vLog.writelog(text,0);
                                            _transfDelegate_YLBH_ADD();
                                            this.Dispose();
                                        }

                                }

                        }
                    }
                }
            }
            catch (Exception ee)
            {
                var mistake = "点击保存按钮失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
    }
}
