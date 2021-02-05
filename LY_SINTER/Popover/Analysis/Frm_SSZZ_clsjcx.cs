﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBase;
using LY_SINTER.Custom;

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_SSZZ_clsjcx : Form
    {
        public static bool isopen = false;
        public Frm_SSZZ_clsjcx()
        {
            InitializeComponent();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
        }
        //查询按钮
        private void simpleButton2_click(object sender, EventArgs e)
        {
            DateTime d1 = Convert.ToDateTime(textBox_begin.Text);
            DateTime d2 = Convert.ToDateTime(textBox_end.Text);
            find(d1,d2);
        }
        string sj = "";
        double LL = 0.0, JH = 0.0, SJ = 0.0;
        public void find(DateTime d1, DateTime d2)
        {
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select TIMESTAMP,T_AL_OUTPUT,P_AL_OUTPUT,A_AL_OUTPUT,(A_AL_OUTPUT-T_AL_OUTPUT) as SJ_LL,(A_AL_OUTPUT-P_AL_OUTPUT) as SJ_JH from MC_POPCAL_OUT where TIMESTAMP between '" + d1 + "'and '" + d2 + "'";
            System.Data.DataTable table = dBSQL.GetCommand(sql);
            dataGridView1.DataSource = table;
            
         }
        //实时按钮
        private void simpleButton3_click(object sender, EventArgs e)
        {
            DateTime d1 = DateTime.Now.AddMonths(-1);
            DateTime d2 = DateTime.Now;
            find(d1, d2);
        }
    }
}
