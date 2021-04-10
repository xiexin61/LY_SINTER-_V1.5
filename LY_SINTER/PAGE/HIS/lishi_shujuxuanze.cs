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

namespace LY_SINTER.PAGE.HIS
{
    public partial class lishi_shujuxuanze : Form
    {
        //选择放到已选框中
        private int columnRule = 8;

        private int rowFlag = 0;
        private int columnFlag = 0;
        private int IsEqualFlag = 0;
        private int dataview2CellsCount = 0;
        private int dataCellsCount = 8;
        private int select_dataview1CellsCount = 0;
        private int select_dataview3CellsCount = 0;
        public static bool isopen = false;
        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public lishi_shujuxuanze()
        {
            InitializeComponent();
            EditProperty();
        }

        //datagridview1对应checkbox勾选限定+赋值
        private void datagridview1_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox check = (CheckBox)sender;
                string checbox_name = check.Name;
                if (check.Checked)
                {
                    switch (checbox_name)
                    {
                        case "checkBox1":
                            {
                                checkBox2.Enabled = false;
                                checkBox2.ForeColor = Color.Gray;
                                checkBox3.Enabled = false;
                                checkBox3.ForeColor = Color.Gray;
                                checkBox4.Enabled = false;
                                checkBox4.ForeColor = Color.Gray;
                                checkBox5.Enabled = false;
                                checkBox5.ForeColor = Color.Gray;
                                checkBox6.Enabled = false;
                                checkBox6.ForeColor = Color.Gray;
                                checkBox7.Enabled = false;
                                checkBox7.ForeColor = Color.Gray;
                                break;
                            }
                        case "checkBox2":
                            {
                                checkBox1.Enabled = false;
                                checkBox1.ForeColor = Color.Gray;
                                checkBox3.Enabled = false;
                                checkBox3.ForeColor = Color.Gray;
                                checkBox4.Enabled = false;
                                checkBox4.ForeColor = Color.Gray;
                                checkBox5.Enabled = false;
                                checkBox5.ForeColor = Color.Gray;
                                checkBox6.Enabled = false;
                                checkBox6.ForeColor = Color.Gray;
                                checkBox7.Enabled = false;
                                checkBox7.ForeColor = Color.Gray;
                                break;
                            }
                        case "checkBox3":
                            {
                                checkBox1.Enabled = false;
                                checkBox1.ForeColor = Color.Gray;
                                checkBox2.Enabled = false;
                                checkBox2.ForeColor = Color.Gray;
                                checkBox4.Enabled = false;
                                checkBox4.ForeColor = Color.Gray;
                                checkBox5.Enabled = false;
                                checkBox5.ForeColor = Color.Gray;
                                checkBox6.Enabled = false;
                                checkBox6.ForeColor = Color.Gray;
                                checkBox7.Enabled = false;
                                checkBox7.ForeColor = Color.Gray;
                                break;
                            }
                        case "checkBox4":
                            {
                                checkBox1.Enabled = false;
                                checkBox1.ForeColor = Color.Gray;
                                checkBox2.Enabled = false;
                                checkBox2.ForeColor = Color.Gray;
                                checkBox3.Enabled = false;
                                checkBox3.ForeColor = Color.Gray;
                                checkBox5.Enabled = false;
                                checkBox5.ForeColor = Color.Gray;
                                checkBox6.Enabled = false;
                                checkBox6.ForeColor = Color.Gray;
                                checkBox7.Enabled = false;
                                checkBox7.ForeColor = Color.Gray;
                                break;
                            }
                        case "checkBox5":
                            {
                                checkBox1.Enabled = false;
                                checkBox1.ForeColor = Color.Gray;
                                checkBox2.Enabled = false;
                                checkBox2.ForeColor = Color.Gray;
                                checkBox3.Enabled = false;
                                checkBox3.ForeColor = Color.Gray;
                                checkBox4.Enabled = false;
                                checkBox4.ForeColor = Color.Gray;
                                checkBox6.Enabled = false;
                                checkBox6.ForeColor = Color.Gray;
                                checkBox7.Enabled = false;
                                checkBox7.ForeColor = Color.Gray;
                                break;
                            }
                        case "checkBox6":
                            {
                                checkBox1.Enabled = false;
                                checkBox1.ForeColor = Color.Gray;
                                checkBox2.Enabled = false;
                                checkBox2.ForeColor = Color.Gray;
                                checkBox3.Enabled = false;
                                checkBox3.ForeColor = Color.Gray;
                                checkBox4.Enabled = false;
                                checkBox4.ForeColor = Color.Gray;
                                checkBox5.Enabled = false;
                                checkBox5.ForeColor = Color.Gray;
                                checkBox7.Enabled = false;
                                checkBox7.ForeColor = Color.Gray;
                                break;
                            }
                        case "checkBox7":
                            {
                                checkBox1.Enabled = false;
                                checkBox1.ForeColor = Color.Gray;
                                checkBox2.Enabled = false;
                                checkBox2.ForeColor = Color.Gray;
                                checkBox3.Enabled = false;
                                checkBox3.ForeColor = Color.Gray;
                                checkBox4.Enabled = false;
                                checkBox4.ForeColor = Color.Gray;
                                checkBox5.Enabled = false;
                                checkBox5.ForeColor = Color.Gray;
                                checkBox6.Enabled = false;
                                checkBox6.ForeColor = Color.Gray;
                                break;
                            }
                    }
                    dataGridView1.Rows.Clear();
                    string str = check.Text;
                    DataTable dataTable = new DataTable();
                    switch (str)
                    {
                        case "配料":
                            {
                                int flag = 1;
                                List<string> aa = selectaa(flag);
                                List<string> bb = selectbb(flag);
                                int q = aa.Count % dataCellsCount > 0 ? 1 : 0;
                                dataGridView1.RowCount = aa.Count / dataCellsCount + q;             //定义的行数
                                dataGridView1.ColumnCount = dataCellsCount;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / dataCellsCount + q; i++)
                                {
                                    for (int b = 0; b < dataCellsCount; b++)
                                    {
                                        dataGridView1.Rows[i].Cells[b].Value = aa[index];
                                        if (bb[index] == "1")
                                        {
                                            dataGridView1.Rows[i].Cells[b].Style.BackColor = Color.LightBlue;
                                        }
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "混合":
                            {
                                int flag = 2;
                                List<string> aa = selectaa(flag);
                                List<string> bb = selectbb(flag);
                                int q = aa.Count % dataCellsCount > 0 ? 1 : 0;
                                dataGridView1.RowCount = aa.Count / dataCellsCount + q;             //定义的行数
                                dataGridView1.ColumnCount = dataCellsCount;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / dataCellsCount + q; i++)
                                {
                                    for (int b = 0; b < dataCellsCount; b++)
                                    {
                                        dataGridView1.Rows[i].Cells[b].Value = aa[index];
                                        if (bb[index] == "1")
                                        {
                                            dataGridView1.Rows[i].Cells[b].Style.BackColor = Color.LightBlue;
                                        }
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "布料":
                            {
                                int flag = 3;
                                List<string> aa = selectaa(flag);
                                List<string> bb = selectbb(flag);
                                int q = aa.Count % dataCellsCount > 0 ? 1 : 0;
                                dataGridView1.RowCount = aa.Count / dataCellsCount + q;             //定义的行数
                                dataGridView1.ColumnCount = dataCellsCount;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / dataCellsCount + q; i++)
                                {
                                    for (int b = 0; b < dataCellsCount; b++)
                                    {
                                        dataGridView1.Rows[i].Cells[b].Value = aa[index];
                                        if (bb[index] == "1")
                                        {
                                            dataGridView1.Rows[i].Cells[b].Style.BackColor = Color.LightBlue;
                                        }
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "点火":
                            {
                                int flag = 4;
                                List<string> aa = selectaa(flag);
                                List<string> bb = selectbb(flag);
                                int q = aa.Count % dataCellsCount > 0 ? 1 : 0;
                                dataGridView1.RowCount = aa.Count / dataCellsCount + q;             //定义的行数
                                dataGridView1.ColumnCount = dataCellsCount;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / dataCellsCount + q; i++)
                                {
                                    for (int b = 0; b < dataCellsCount; b++)
                                    {
                                        dataGridView1.Rows[i].Cells[b].Value = aa[index];
                                        if (bb[index] == "1")
                                        {
                                            dataGridView1.Rows[i].Cells[b].Style.BackColor = Color.LightBlue;
                                        }
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "烧结":
                            {
                                int flag = 5;
                                List<string> aa = selectaa(flag);
                                List<string> bb = selectbb(flag);
                                int q = aa.Count % dataCellsCount > 0 ? 1 : 0;
                                dataGridView1.RowCount = aa.Count / dataCellsCount + q;             //定义的行数
                                dataGridView1.ColumnCount = dataCellsCount;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / dataCellsCount + q; i++)
                                {
                                    for (int b = 0; b < dataCellsCount; b++)
                                    {
                                        dataGridView1.Rows[i].Cells[b].Value = aa[index];
                                        if (bb[index] == "1")
                                        {
                                            dataGridView1.Rows[i].Cells[b].Style.BackColor = Color.LightBlue;
                                        }
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "环冷":
                            {
                                int flag = 6;
                                List<string> aa = selectaa(flag);
                                List<string> bb = selectbb(flag);
                                int q = aa.Count % dataCellsCount > 0 ? 1 : 0;
                                dataGridView1.RowCount = aa.Count / dataCellsCount + q;             //定义的行数
                                dataGridView1.ColumnCount = dataCellsCount;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / dataCellsCount + q; i++)
                                {
                                    for (int b = 0; b < dataCellsCount; b++)
                                    {
                                        dataGridView1.Rows[i].Cells[b].Value = aa[index];
                                        if (bb[index] == "1")
                                        {
                                            dataGridView1.Rows[i].Cells[b].Style.BackColor = Color.LightBlue;
                                        }
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "成品":
                            {
                                int flag = 7;
                                List<string> aa = selectaa(flag);
                                List<string> bb = selectbb(flag);
                                int q = aa.Count % dataCellsCount > 0 ? 1 : 0;
                                dataGridView1.RowCount = aa.Count / dataCellsCount + q;             //定义的行数
                                dataGridView1.ColumnCount = dataCellsCount;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / dataCellsCount + q; i++)
                                {
                                    for (int b = 0; b < dataCellsCount; b++)
                                    {
                                        dataGridView1.Rows[i].Cells[b].Value = aa[index];

                                        if (bb[index] == "1")
                                        {
                                            dataGridView1.Rows[i].Cells[b].Style.BackColor = Color.LightBlue;
                                        }
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
                else
                {
                    switch (checbox_name)
                    {
                        case "checkBox1":
                            {
                                dataGridView1.Rows.Clear();
                                checkBox2.Enabled = true;
                                checkBox2.ForeColor = Color.Black;
                                checkBox3.Enabled = true;
                                checkBox3.ForeColor = Color.Black;
                                checkBox4.Enabled = true;
                                checkBox4.ForeColor = Color.Black;
                                checkBox5.Enabled = true;
                                checkBox5.ForeColor = Color.Black;
                                checkBox6.Enabled = true;
                                checkBox6.ForeColor = Color.Black;
                                checkBox7.Enabled = true;
                                checkBox7.ForeColor = Color.Black;
                                break;
                            }
                        case "checkBox2":
                            {
                                dataGridView1.Rows.Clear();
                                dataGridView1.Rows.Clear();
                                checkBox1.Enabled = true;
                                checkBox1.ForeColor = Color.Black;
                                checkBox3.Enabled = true;
                                checkBox3.ForeColor = Color.Black;
                                checkBox4.Enabled = true;
                                checkBox4.ForeColor = Color.Black;
                                checkBox5.Enabled = true;
                                checkBox5.ForeColor = Color.Black;
                                checkBox6.Enabled = true;
                                checkBox6.ForeColor = Color.Black;
                                checkBox7.Enabled = true;
                                checkBox7.ForeColor = Color.Black;
                                break;
                            }
                        case "checkBox3":
                            {
                                dataGridView1.Rows.Clear();
                                dataGridView1.Rows.Clear();
                                checkBox1.Enabled = true;
                                checkBox1.ForeColor = Color.Black;
                                checkBox2.Enabled = true;
                                checkBox2.ForeColor = Color.Black;
                                checkBox4.Enabled = true;
                                checkBox4.ForeColor = Color.Black;
                                checkBox5.Enabled = true;
                                checkBox5.ForeColor = Color.Black;
                                checkBox6.Enabled = true;
                                checkBox6.ForeColor = Color.Black;
                                checkBox7.Enabled = true;
                                checkBox7.ForeColor = Color.Black;
                                break;
                            }
                        case "checkBox4":
                            {
                                dataGridView1.Rows.Clear();
                                dataGridView1.Rows.Clear();
                                checkBox1.Enabled = true;
                                checkBox1.ForeColor = Color.Black;
                                checkBox2.Enabled = true;
                                checkBox2.ForeColor = Color.Black;
                                checkBox3.Enabled = true;
                                checkBox3.ForeColor = Color.Black;
                                checkBox5.Enabled = true;
                                checkBox5.ForeColor = Color.Black;
                                checkBox6.Enabled = true;
                                checkBox6.ForeColor = Color.Black;
                                checkBox7.Enabled = true;
                                checkBox7.ForeColor = Color.Black;
                                break;
                            }
                        case "checkBox5":
                            {
                                dataGridView1.Rows.Clear();
                                dataGridView1.Rows.Clear();
                                checkBox1.Enabled = true;
                                checkBox1.ForeColor = Color.Black;
                                checkBox2.Enabled = true;
                                checkBox2.ForeColor = Color.Black;
                                checkBox3.Enabled = true;
                                checkBox3.ForeColor = Color.Black;
                                checkBox4.Enabled = true;
                                checkBox4.ForeColor = Color.Black;
                                checkBox6.Enabled = true;
                                checkBox6.ForeColor = Color.Black;
                                checkBox7.Enabled = true;
                                checkBox7.ForeColor = Color.Black;
                                break;
                            }
                        case "checkBox6":
                            {
                                dataGridView1.Rows.Clear();
                                dataGridView1.Rows.Clear();
                                checkBox1.Enabled = true;
                                checkBox1.ForeColor = Color.Black;
                                checkBox2.Enabled = true;
                                checkBox2.ForeColor = Color.Black;
                                checkBox3.Enabled = true;
                                checkBox3.ForeColor = Color.Black;
                                checkBox4.Enabled = true;
                                checkBox4.ForeColor = Color.Black;
                                checkBox5.Enabled = true;
                                checkBox5.ForeColor = Color.Black;
                                checkBox7.Enabled = true;
                                checkBox7.ForeColor = Color.Black;
                                break;
                            }
                        case "checkBox7":
                            {
                                dataGridView1.Rows.Clear();
                                dataGridView1.Rows.Clear();
                                checkBox1.Enabled = true;
                                checkBox1.ForeColor = Color.Black;
                                checkBox2.Enabled = true;
                                checkBox2.ForeColor = Color.Black;
                                checkBox3.Enabled = true;
                                checkBox3.ForeColor = Color.Black;
                                checkBox4.Enabled = true;
                                checkBox4.ForeColor = Color.Black;
                                checkBox5.Enabled = true;
                                checkBox5.ForeColor = Color.Black;
                                checkBox6.Enabled = true;
                                checkBox6.ForeColor = Color.Black;
                                break;
                            }
                    }
                }
            }
            catch
            { }
        }

        //datagridview3对应checkbox勾选限定
        private void datagridview3_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox check = (CheckBox)sender;
                string checbox_name = check.Name;
                if (check.Checked)
                {
                    switch (checbox_name)
                    {
                        case "checkBox8":
                            {
                                checkBox9.Enabled = false;
                                checkBox9.ForeColor = Color.Gray;
                                checkBox10.Enabled = false;
                                checkBox10.ForeColor = Color.Gray;
                                checkBox11.Enabled = false;
                                checkBox11.ForeColor = Color.Gray;
                                break;
                            }
                        case "checkBox9":
                            {
                                checkBox8.Enabled = false;
                                checkBox8.ForeColor = Color.Gray;
                                checkBox10.Enabled = false;
                                checkBox10.ForeColor = Color.Gray;
                                checkBox11.Enabled = false;
                                checkBox11.ForeColor = Color.Gray;
                                break;
                            }
                        case "checkBox10":
                            {
                                checkBox8.Enabled = false;
                                checkBox8.ForeColor = Color.Gray;
                                checkBox9.Enabled = false;
                                checkBox9.ForeColor = Color.Gray;
                                checkBox11.Enabled = false;
                                checkBox11.ForeColor = Color.Gray;
                                break;
                            }
                        case "checkBox11":
                            {
                                checkBox8.Enabled = false;
                                checkBox8.ForeColor = Color.Gray;
                                checkBox9.Enabled = false;
                                checkBox9.ForeColor = Color.Gray;
                                checkBox10.Enabled = false;
                                checkBox10.ForeColor = Color.Gray;
                                break;
                            }
                    }
                    dataGridView3.Rows.Clear();
                    string str = check.Text;
                    DataTable dataTable = new DataTable();
                    switch (str)
                    {
                        case "配料":
                            {
                                int flag = 8;
                                List<string> aa = selectaa(flag);
                                int q = aa.Count % 4 > 0 ? 1 : 0;
                                dataGridView3.RowCount = aa.Count / 4 + q;             //定义的行数
                                dataGridView3.ColumnCount = 4;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / 4 + q; i++)
                                {
                                    for (int b = 0; b < 4; b++)
                                    {
                                        dataGridView3.Rows[i].Cells[b].Value = aa[index];
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "混合":
                            {
                                int flag = 9;
                                List<string> aa = selectaa(flag);
                                int q = aa.Count % 4 > 0 ? 1 : 0;
                                dataGridView3.RowCount = aa.Count / 4 + q;             //定义的行数
                                dataGridView3.ColumnCount = 4;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / 4 + q; i++)
                                {
                                    for (int b = 0; b < 4; b++)
                                    {
                                        dataGridView3.Rows[i].Cells[b].Value = aa[index];
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "布料":
                            {
                                int flag = 10;
                                List<string> aa = selectaa(flag);
                                int q = aa.Count % 4 > 0 ? 1 : 0;
                                dataGridView3.RowCount = aa.Count / 4 + q;             //定义的行数
                                dataGridView3.ColumnCount = 4;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / 4 + q; i++)
                                {
                                    for (int b = 0; b < 4; b++)
                                    {
                                        dataGridView3.Rows[i].Cells[b].Value = aa[index];
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "烧结":
                            {
                                int flag = 11;
                                List<string> aa = selectaa(flag);
                                int q = aa.Count % 4 > 0 ? 1 : 0;
                                dataGridView3.RowCount = aa.Count / 4 + q;             //定义的行数
                                dataGridView3.ColumnCount = 4;          //定义的列数
                                int index = 0;
                                for (int i = 0; i < aa.Count / 4 + q; i++)
                                {
                                    for (int b = 0; b < 4; b++)
                                    {
                                        dataGridView3.Rows[i].Cells[b].Value = aa[index];
                                        index++;
                                        if (index > aa.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
                else
                {
                    switch (checbox_name)
                    {
                        case "checkBox8":
                            {
                                dataGridView3.Rows.Clear();
                                checkBox9.Enabled = true;
                                checkBox9.ForeColor = Color.Black;
                                checkBox10.Enabled = true;
                                checkBox10.ForeColor = Color.Black;
                                checkBox11.Enabled = true;
                                checkBox11.ForeColor = Color.Black;
                                break;
                            }
                        case "checkBox9":
                            {
                                //dataGridView1.Rows.Clear();
                                dataGridView3.Rows.Clear();
                                checkBox8.Enabled = true;
                                checkBox8.ForeColor = Color.Black;
                                checkBox10.Enabled = true;
                                checkBox10.ForeColor = Color.Black;
                                checkBox11.Enabled = true;
                                checkBox11.ForeColor = Color.Black;
                                break;
                            }
                        case "checkBox10":
                            {
                                //dataGridView1.Rows.Clear();
                                dataGridView3.Rows.Clear();
                                checkBox8.Enabled = true;
                                checkBox8.ForeColor = Color.Black;
                                checkBox9.Enabled = true;
                                checkBox9.ForeColor = Color.Black;
                                checkBox11.Enabled = true;
                                checkBox11.ForeColor = Color.Black;
                                break;
                            }
                        case "checkBox11":
                            {
                                //dataGridView1.Rows.Clear();
                                dataGridView3.Rows.Clear();
                                checkBox8.Enabled = true;
                                checkBox8.ForeColor = Color.Black;
                                checkBox9.Enabled = true;
                                checkBox9.ForeColor = Color.Black;
                                checkBox10.Enabled = true;
                                checkBox10.ForeColor = Color.Black;
                                break;
                            }
                    }
                }
            }
            catch
            { }
        }

        //datagridview1选值放到datagridview2中
        private void DataGridView1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                dataGridView2.RowCount = 100;
                dataGridView2.ColumnCount = 8;
                int dataview2CellsCount = 0;
                int count = 0;

                //计算dataGridView2有多少数据
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView2.ColumnCount; j++)
                    {
                        if (dataGridView2.Rows[i].Cells[j].Value != null)
                        {
                            dataview2CellsCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                select_dataview1CellsCount = dataGridView1.SelectedCells.Count;
                select_dataview3CellsCount = dataGridView3.SelectedCells.Count;

                //第一步：将dataGridView1选择的值放入list列表
                List<object> m_list = new List<object>();
                for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
                {
                    if (dataGridView1.SelectedCells[i].Value != null)
                    {
                        m_list.Add(dataGridView1.SelectedCells[i].Value.ToString());
                    }
                }

                //第二步：将list列表值与dataGridView2现有项做轮询
                for (int k = 0; k < m_list.Count; k++)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        int j;
                        for (j = 0; j < 8; j++)
                        {
                            if (dataGridView2.Rows[i].Cells[j].Value == m_list[k])
                            {
                                IsEqualFlag = 1;
                                break;
                            }
                        }
                    }
                    if (IsEqualFlag != 1)
                    {
                        count++;
                        if (dataview2CellsCount + count <= 21)
                        {
                            //向dataGridView2添加插入当前list[k]值
                            dataGridView2.Rows[rowFlag].Cells[columnFlag].Value = m_list[k];
                            columnFlag++;
                            if (columnFlag > 7)
                            {
                                columnFlag = 0;
                                rowFlag++;
                            }
                        }
                        else
                        {
                            MessageBox.Show("最多选择21个数据,前21个数据已选择！");
                            break;
                        }
                    }
                    IsEqualFlag = 0;
                }
            }
            catch
            { }
        }

        //datagridview3选值放到datagridview2中
        private void DataGridView3_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                dataGridView2.RowCount = 100;
                dataGridView2.ColumnCount = 8;
                int dataview2CellsCount = 0;
                int count = 0;

                //计算dataGridView2有多少数据
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView2.ColumnCount; j++)
                    {
                        if (dataGridView2.Rows[i].Cells[j].Value != null)
                        {
                            dataview2CellsCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                select_dataview1CellsCount = dataGridView1.SelectedCells.Count;
                select_dataview3CellsCount = dataGridView3.SelectedCells.Count;

                //第一步：将dataGridView1选择的值放入list列表
                List<object> m_list = new List<object>();
                for (int i = 0; i < dataGridView3.SelectedCells.Count; i++)
                {
                    if (dataGridView3.SelectedCells[i].Value != null)
                    {
                        m_list.Add(dataGridView3.SelectedCells[i].Value.ToString());
                    }
                }

                //第二步：将list列表值与dataGridView2现有项做轮询
                for (int k = 0; k < m_list.Count; k++)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        int j;
                        for (j = 0; j < 8; j++)
                        {
                            if (dataGridView2.Rows[i].Cells[j].Value == m_list[k])
                            {
                                IsEqualFlag = 1;
                                break;
                            }
                        }
                    }

                    if (IsEqualFlag != 1)
                    {
                        count++;
                        if (dataview2CellsCount + count <= 21)
                        {
                            //向dataGridView2添加插入当前list[k]值
                            dataGridView2.Rows[rowFlag].Cells[columnFlag].Value = m_list[k];
                            columnFlag++;
                            if (columnFlag > 7)
                            {
                                columnFlag = 0;
                                rowFlag++;
                            }
                        }
                        else
                        {
                            MessageBox.Show("最多选择21个数据,前21个数据已选择！");
                            break;
                        }
                    }
                    IsEqualFlag = 0;
                }
            }
            catch
            { }
        }

        //双击删除
        private void dataGridView2_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                List<object> m_list = new List<object>();
                List<object> m_list1 = new List<object>();
                //int gridview2_rowrflag = 0;
                //int gridview2_columnflag = 0;
                rowFlag = 0;
                columnFlag = 0;
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        if (dataGridView2.Rows[i].Cells[j].Value != null)
                        {
                            m_list.Add(dataGridView2.Rows[i].Cells[j].Value.ToString());
                        }
                    }
                for (int i = 0; i < m_list.Count; i++)
                {
                    if (dataGridView2.SelectedCells[0].Value != m_list[i])
                    {
                        m_list1.Add(m_list[i]);
                    }
                }
                for (int i = 0; i < m_list1.Count; i++)
                {
                    //dataGridView2.Rows[gridview2_rowrflag].Cells[gridview2_columnflag].Value = m_list1[i];
                    //gridview2_columnflag++;
                    //if (gridview2_columnflag > 7)
                    //{
                    //    gridview2_columnflag = 0;
                    //    gridview2_rowrflag++;
                    //}
                    dataGridView2.Rows[rowFlag].Cells[columnFlag].Value = m_list1[i];
                    columnFlag++;
                    if (columnFlag > 7)
                    {
                        columnFlag = 0;
                        rowFlag++;
                    }
                }
                dataGridView2.Rows[rowFlag].Cells[columnFlag].Value = null;
            }
            catch
            { }
        }

        //不显示行标题和列标题
        private void EditProperty()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ColumnHeadersVisible = false;
        }

        //查询出数据
        private List<String> selectaa(int flag)
        {
            List<string> aa = new List<string>();
            try
            {
                string sql1 = "select DESCRIBE from CFG_HIST_DATE_INTERFACE where CLASS='" + flag + "' order by COLUMNINDEX";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    for (int a = 0; a < dataTable1.Rows.Count; a++)
                    {
                        string zdms = dataTable1.Rows[a][0].ToString();
                        if (zdms != "")
                        {
                            aa.Add(zdms);
                        }
                    }
                }
                else
                { }
            }
            catch
            {
            }
            return aa;
        }

        private List<String> selectbb(int flag)
        {
            List<string> aa = new List<string>();
            try
            {
                string sql1 = "select ISBLUE from CFG_HIST_DATE_INTERFACE where CLASS='" + flag + "' order by COLUMNINDEX";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    for (int a = 0; a < dataTable1.Rows.Count; a++)
                    {
                        string zdms = dataTable1.Rows[a][0].ToString();
                        if (zdms != "")
                        {
                            aa.Add(zdms);
                        }
                    }
                }
                else
                { }
            }
            catch
            {
            }
            return aa;
        }

        //清空按钮
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Rows.Clear();
                rowFlag = 0;
                columnFlag = 0;
            }
            catch
            { }
        }

        //确认按钮
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> ziduanming = new List<string>();
                List<string> zhongwenmiaoshu = new List<string>();
                for (int a = 0; a < dataGridView2.Rows.Count; a++)
                {
                    for (int b = 0; b < dataGridView2.Columns.Count; b++)
                    {
                        try
                        {
                            if (dataGridView2.Rows[a].Cells[b].Value != null)
                            {
                                if (ziduanming.Count < 21)
                                {
                                    string sql1 = "select NAME,DESCRIBE from CFG_HIST_DATE_INTERFACE where DESCRIBE='" + dataGridView2.Rows[a].Cells[b].Value + "'";
                                    DataTable dataTable1 = dBSQL.GetCommand(sql1);
                                    if (dataTable1.Rows.Count > 0)
                                    {
                                        string zdm = dataTable1.Rows[0][0].ToString();
                                        string zwms = dataTable1.Rows[0][1].ToString();
                                        ziduanming.Add(zdm);
                                        zhongwenmiaoshu.Add(zwms);
                                    }
                                    else
                                    { }
                                }
                                else
                                {
                                    MessageBox.Show("最多选择21个数据,前21个数据已选择！");
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch
                        {
                        }
                    }
                    //break;
                }
                MessageBox.Show("成功!");
                this.Close();
                lishiqushiquxianchaxun1.aaa1.Clear();
                lishiqushiquxianchaxun1.aaa1.AddRange(ziduanming);
                lishiqushiquxianchaxun1.bbb1.Clear();
                lishiqushiquxianchaxun1.bbb1.AddRange(zhongwenmiaoshu);
                lishiqushiquxianchaxun1.aaa1_pre.Clear();
                lishiqushiquxianchaxun1.aaa1_pre.AddRange(ziduanming);
                lishiqushiquxianchaxun1.bbb1_pre.Clear();
                lishiqushiquxianchaxun1.bbb1_pre.AddRange(zhongwenmiaoshu);
            }
            catch
            { }
        }
    }
}