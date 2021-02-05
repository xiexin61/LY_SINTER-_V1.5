using DataBase;
using LY_SINTER.Model;
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

namespace LY_SINTER
{
    public partial class Register : Form
    {
        public vLog vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Register()
        {
            InitializeComponent();
            if (vLog == null)
                vLog = new vLog(".\\log_Page\\Register_Page\\");
            int x = (System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Size.Width) / 2;
            int y = (System.Windows.Forms.SystemInformation.WorkingArea.Height - this.Size.Height) / 2;
            this.StartPosition = FormStartPosition.Manual; //窗体的位置由Location属性决定
            this.Location = (Point)new Size(x, y);         //窗体的起始位置为(x,y)
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            string pw = textBox2.Text.Trim();
            if (name == "")
            {
                MessageBox.Show("请输入用户名");
                textBox1.Focus();
                return;
            }
            else
            {
                if (pw == "")
                {
                    MessageBox.Show("请输入用户密码");
                    textBox1.Focus();
                    return;
                }
                else
                {
                    string sql = "select CODE , USER_NAME,AUTHORITY,USER_PWD from USER_AUTHORITY where USER_NAME = '" + name + "' and USER_PWD = '" + pw + "'";
                    DataTable dataTable = dBSQL.GetCommand(sql);

                    if (dataTable.Rows.Count == 0 || dataTable == null)
                    {
                        MessageBox.Show("登录用户或密码不正确");
                        return;
                    }
                    else
                    {
                        User_Level.Authority = int.Parse(dataTable.Rows[0]["AUTHORITY"].ToString());
                        User_Level.User_name = dataTable.Rows[0]["USER_NAME"].ToString();
                        this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态  
                        //日志输入
                        string operation = "用户" + User_Level.User_name + "成功登录程序";
                        vLog.writelog(operation, 0);
                        this.Close();

                    }
                }
            }
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            textBox2.Text = null;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                simpleButton1_Click(sender, e);

        }
    }
}
