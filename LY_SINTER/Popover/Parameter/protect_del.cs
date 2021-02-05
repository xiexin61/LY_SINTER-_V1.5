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
    public partial class protect_del : Form
    {
        public static bool isopen = false;
        //声明委托和事件
        public delegate void TransfDelegate_YLBH_DEL();
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public vLog _vLog { get; set; }
        public protect_del()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Parametery\\Ingredient_Protect_Pop_DEL\\");
            classify();

        }
        //声明委托和事件
        public event TransfDelegate_YLBH_DEL _transfDelegate_YLBH_DEL;
        /// <summary>
        /// 下拉框赋值
        /// </summary>
        private void classify()
        {
            try
            {
                string sql = "select M_TYPE as value,M_DESC as name,CODE_MIN,CODE_MAX from [M_MATERIAL_COOD_CONFIG] order by M_TYPE asc";
                DataTable dataTable = dBSQL.GetCommand(sql);
                this.comboBox1.DataSource = dataTable;
                this.comboBox1.DisplayMember = "name";
                this.comboBox1.ValueMember = "value";
                this.comboBox1.SelectedIndex = 0;
            }
            catch(Exception ee)
            {
                var mistake = "classify方法失败" + ee.ToString();
                _vLog.writelog(mistake,-1);
            }
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
         
           
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ///****判断逻辑，点击按钮之前判断是否输入了二级编码，在判断输入的二级编码和选择的物料种类是否对应，在判断是否为输入正确的二级编码
                string L2_CODE_TEXT = this.textBox1.Text.ToString();
                if (L2_CODE_TEXT != "")
                {


                    //判断输入的二级编码和所属的物料分类是否正确
                    bool L2_CODE_SIGNAL = true;
                    int L2_CODE = int.Parse(this.textBox1.Text.ToString());
                    string L3_CODE = this.textBox2.Text.ToString();
                    int WLGS_NAME = comboBox1.SelectedIndex;
                    WLGS_NAME = WLGS_NAME + 1;
                    string sql_1 = "select M_TYPE as value,M_DESC as name,CODE_MIN,CODE_MAX from [M_MATERIAL_COOD_CONFIG] where M_TYPE = '" + WLGS_NAME + "' order by M_TYPE asc";
                    DataTable dataTable = dBSQL.GetCommand(sql_1);
                    //原料最小值
                    int raw_min = int.Parse(dataTable.Rows[0]["CODE_MIN"].ToString());
                    //原料最大值
                    int raw_max = int.Parse(dataTable.Rows[0]["CODE_MAX"].ToString());
                    if (L2_CODE <= raw_max && L2_CODE >= raw_min)
                    {
                        L2_CODE_SIGNAL = true;
                    }
                    else
                    {
                        L2_CODE_SIGNAL = false;
                    }
                    if (L3_CODE == "")
                    {
                        MessageBox.Show("请输入正确的二级编码");
                    }
                    else
                    {
                        if (L2_CODE_SIGNAL)
                        {
                            DialogResult resule = MessageBox.Show("是否确认删除！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                            string a = resule.ToString();
                            if (a == "OK")
                            {
                                string sql_del = "delete from  M_MATERIAL_COOD where L2_CODE = " + L2_CODE + "";
                                dBSQL.CommandExecuteNonQuery(sql_del);
                                _transfDelegate_YLBH_DEL();
                                var text = "用户点击删除按钮，删除物料二级编码" + L2_CODE + "成功";
                                _vLog.writelog(text, 0);
                                this.Dispose();
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("选择正确的原料种类");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请输入物料编码");
                }
            }
            catch(Exception ee)
            {
                var mistake = "点击删除按钮失败 " + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
           
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string WLMS = textBox3.Text.ToString();
                if (WLMS == "")
                {
                    return;
                }
                else
                {
                    string SQL = "select L3_CODE,L2_CODE from M_MATERIAL_COOD where MAT_DESC = '" + WLMS + "'";
                    DataTable dataTable = dBSQL.GetCommand(SQL);
                    if (dataTable.Rows.Count > 0 && dataTable != null)
                    {

                        //  string L3_CODE = dataTable.Rows[0]["L3_CODE"].ToString();
                        string L3_CODE = dataTable.Rows[0]["L3_CODE"].ToString();
                         string L2_CODE = dataTable.Rows[0]["L2_CODE"].ToString();
                          this.textBox1.Text = L2_CODE;
                        //  this.textBox2.Text = L3_CODE;
                        this.textBox2.Text = L3_CODE;
                    }
                    else
                    {
                        this.textBox1.Text = null;
                        this.textBox2.Text = null;
                        this.textBox3.Text = null;
                        return;
                    }
                }
            }
            catch (Exception ee)
            {
                var mistake = "三级编码响应事件失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        private void textBox2_TextChanged(object sender, KeyEventArgs e)
        {
            try
            {
                string L3_CODE = textBox2.Text.ToString();
                if (L3_CODE == "")
                {
                    return;
                }
                else
                {
                    string SQL = "select MAT_DESC,L2_CODE from M_MATERIAL_COOD where L3_CODE = '" + L3_CODE + "'";
                    DataTable dataTable = dBSQL.GetCommand(SQL);
                    if (dataTable.Rows.Count > 0 && dataTable != null)
                    {

                        //  string L3_CODE = dataTable.Rows[0]["L3_CODE"].ToString();
                        string WLMS = dataTable.Rows[0]["MAT_DESC"].ToString();

                        string L2_CODE = dataTable.Rows[0]["L2_CODE"].ToString();
                        this.textBox1.Text = L2_CODE;
                        //  this.textBox2.Text = L3_CODE;
                        this.textBox3.Text = WLMS;
                    }
                    //else
                    //{
                    //    this.textBox1.Text = null;
                    //    this.textBox2.Text = null;
                    //    this.textBox3.Text = null;
                    //    return;
                    //}
                }
            }
            catch (Exception ee)
            {
                var mistake = "三级编码响应事件失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
    }
}
