using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;
using VLog;

namespace SCZZJH
{
    class SCYLXH
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public vLog vLog { get; set; }
        int zhouqi = getTime();
        public static int getTime()
        {
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            int PAR_OUT_T = 0;
            //每PAR_OUT_T时间计算一次
            string sql = "select PAR_OUT_T from MC_POPCAL_PAR";
            System.Data.DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                PAR_OUT_T = Convert.ToInt32(table.Rows[0][0]);
            }
            return PAR_OUT_T;
        }
        public DateTime getStartDate(DateTime now)
        {
            DateTime start=DateTime.Now;
            if (now.Hour >= 8 && now.Hour < 20)
            {
                now= new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            }
            else
            {
                now = new DateTime(now.Year, now.Month, now.AddDays(-1).Day, 20, 0, 0);
            }
            return start;
        }
        public DateTime getEndDate(DateTime now)
        {
            DateTime end=DateTime.Now;
            if (now.Hour >= 8 && now.Hour < 20)
            {
                now = new DateTime(now.Year, now.Month, now.Day, 19, 59, 0);
            }
            else
            {
                now = new DateTime(now.Year, now.Month, now.Day, 7, 59, 0);
            }
            return end;
        }
        public double getSum(double[] ACC)
        {
            double sum = 0;
            if (ACC[ACC.Length - 1] - ACC[0] >= 0)
            {
                sum = ACC[ACC.Length - 1] - ACC[0];
            }
            //小于0，每分钟逐条相减并求和
            else
            {
                for (int i = ACC.Length - 1; i >= 0; i--)
                {
                    if (ACC[i] - ACC[i - 1] > 0)
                    {
                        sum = sum + (ACC[i] - ACC[i - 1]);
                    }
                    else
                    {
                        if (ACC[i] > ACC[i - 1])
                        {
                            sum = sum + ACC[i - 1];
                        }
                        else
                        {
                            sum = sum + ACC[i];
                        }
                    }
                }
            }
            return sum;
        }
        //原料消耗计算
        public void ylxh()
        {
            try
            {
                DateTime start;
                DateTime end;
                string class1 = "";
                DateTime now = DateTime.Now;
                //时间获取
                string sql1 = "select START_TIME,END_TIME,SHIFT_FLAG from M_CLASS_PLAN where START_TIME < '" + now + "' and END_TIME > '" + now + "'";
                System.Data.DataTable table = dBSQL.GetCommand(sql1);
                if (table.Rows.Count > 0)
                {
                    start = Convert.ToDateTime(table.Rows[0]["START_TIME"]);
                    end = Convert.ToDateTime(table.Rows[0]["END_TIME"]);
                    class1 = table.Rows[0]["SHIFT_FLAG"].ToString();
                }
                else
                {
                    start = getStartDate(DateTime.Now);
                    end = getEndDate(DateTime.Now);
                }
                double[] xh = new double[20];
                
                //累计值计算
                string sql2 = "select TIMESTAMP,MAT_PLC_B_ACC_1,MAT_PLC_B_ACC_2,MAT_PLC_B_ACC_3,MAT_PLC_B_ACC_4,MAT_PLC_B_ACC_5,MAT_PLC_B_ACC_6,MAT_PLC_B_ACC_7," +
                    "MAT_PLC_B_ACC_8,MAT_PLC_B_ACC_9,MAT_PLC_B_ACC_10,MAT_PLC_B_ACC_11,MAT_PLC_B_ACC_12,MAT_PLC_B_ACC_13,MAT_PLC_B_ACC_14,MAT_PLC_B_ACC_15,MAT_PLC_B_ACC_16," +
                    "MAT_PLC_B_ACC_17,MAT_PLC_B_ACC_18,MAT_PLC_B_ACC_19,MAT_PLC_B_ACC_20 from C_MAT_PLC_1MIN where " +
                    "TIMESTAMP between '"+ start + "' and '" + DateTime.Now + "' order by TIMESTAMP; ";
                /*string sql2 = "select TIMESTAMP,MAT_PLC_B_ACC_1,MAT_PLC_B_ACC_2,MAT_PLC_B_ACC_3,MAT_PLC_B_ACC_4,MAT_PLC_B_ACC_5,MAT_PLC_B_ACC_6,MAT_PLC_B_ACC_7," +
                    "MAT_PLC_B_ACC_8,MAT_PLC_B_ACC_9,MAT_PLC_B_ACC_10,MAT_PLC_B_ACC_11,MAT_PLC_B_ACC_12,MAT_PLC_B_ACC_13,MAT_PLC_B_ACC_14,MAT_PLC_B_ACC_15,MAT_PLC_B_ACC_16," +
                    "MAT_PLC_B_ACC_17,MAT_PLC_B_ACC_18,MAT_PLC_B_ACC_19,MAT_PLC_B_ACC_20 from C_MAT_PLC_1MIN where " +
                    "TIMESTAMP between ' 2020-8-30 8:00 ' and '2020-8-30 20:00 ' order by TIMESTAMP; ";*/
                System.Data.DataTable table2 = dBSQL.GetCommand(sql2);
                double[] ACC1 = new double[table2.Rows.Count];
                double[] ACC2 = new double[table2.Rows.Count];
                double[] ACC3 = new double[table2.Rows.Count];
                double[] ACC4 = new double[table2.Rows.Count];
                double[] ACC5 = new double[table2.Rows.Count];
                double[] ACC6 = new double[table2.Rows.Count];
                double[] ACC7 = new double[table2.Rows.Count];
                double[] ACC8 = new double[table2.Rows.Count];
                double[] ACC9 = new double[table2.Rows.Count];
                double[] ACC10 = new double[table2.Rows.Count];
                double[] ACC11 = new double[table2.Rows.Count];
                double[] ACC12 = new double[table2.Rows.Count];
                double[] ACC13 = new double[table2.Rows.Count];
                double[] ACC14 = new double[table2.Rows.Count];
                double[] ACC15 = new double[table2.Rows.Count];
                double[] ACC16 = new double[table2.Rows.Count];
                double[] ACC17 = new double[table2.Rows.Count];
                double[] ACC18 = new double[table2.Rows.Count];
                double[] ACC19 = new double[table2.Rows.Count];
                double[] ACC20 = new double[table2.Rows.Count];
                double sum = 0;
                if (table2.Rows.Count > 0)
                {
                    for (int i = 0; i < table2.Rows.Count; i++)
                    {
                        ACC1[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_1"]);
                        ACC2[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_2"]);
                        ACC3[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_3"]);
                        ACC4[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_4"]);
                        ACC5[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_5"]);
                        ACC6[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_6"]);
                        ACC7[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_7"]);
                        ACC8[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_8"]);
                        ACC9[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_9"]);
                        ACC10[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_10"]);
                        ACC11[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_11"]);
                        ACC12[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_12"]);
                        ACC13[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_13"]);
                        ACC14[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_14"]);
                        ACC15[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_15"]);
                        ACC16[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_16"]);
                        ACC17[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_17"]);
                        ACC18[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_18"]);
                        ACC19[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_19"]);
                        //ACC20[i] = Convert.ToDouble(table2.Rows[i]["MAT_PLC_B_ACC_20"]);
                    }
                    //当前ACC值-开始时间ACC值大于0，则使用该值
                    xh[0] = getSum(ACC1);
                    xh[1] = getSum(ACC2);
                    xh[2] = getSum(ACC3);
                    xh[3] = getSum(ACC4);
                    xh[4] = getSum(ACC5);
                    xh[5] = getSum(ACC6);
                    xh[6] = getSum(ACC7);
                    xh[7] = getSum(ACC8);
                    xh[8] = getSum(ACC9);
                    xh[9] = getSum(ACC10);
                    xh[10] = getSum(ACC11);
                    xh[11] = getSum(ACC12);
                    xh[12] = getSum(ACC13);
                    xh[13] = getSum(ACC14);
                    xh[14] = getSum(ACC15);
                    xh[15] = getSum(ACC16);
                    xh[16] = getSum(ACC17);
                    xh[17] = getSum(ACC18);
                    xh[18] = getSum(ACC19);
                    xh[19] = getSum(ACC20);
                }
                //list中存放二级编码,l2code中存放二级编码和对应的累计消耗求和
                Dictionary<double, double> l2code = new Dictionary<double, double>();
                List<double> list = new List<double>();
                //double[] sum = new double[list.Count];
                //物料种类对应
                string sql3 = "select distinct L2_CODE from M_MATERIAL_BINS";
                System.Data.DataTable table3 = dBSQL.GetCommand(sql3);
                if (table3.Rows.Count > 0)
                {
                    for (int i = 0; i < table3.Rows.Count; i++)
                    {
                        list.Add(Convert.ToDouble(table3.Rows[i][0]));
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    double[] xhsum = new double[l2code.Count];
                    double sum1 = 0.0;
                    string sql4 = "select BIN_NUM from M_MATERIAL_BINS where L2_CODE=" + list[i] + "";
                    System.Data.DataTable table4 = dBSQL.GetCommand(sql4);
                    if (table4.Rows.Count > 0)
                    {
                        for (int j = 0; j < table4.Rows.Count; j++)
                        {
                            int bin_num = Convert.ToInt16(table4.Rows[j][0]);
                            if (!l2code.ContainsKey(list[i]))
                            {
                                l2code.Add(list[i], xh[bin_num - 1]);//不含则加
                            }
                            else
                            {
                                l2code[list[i]] = l2code[list[i]] + xh[bin_num - 1];//含则累加
                            }
                        }
                    }
                }
                //l2code是二级编码对应的累计消耗求和
                //根据二级编码找到物料描述,并存入数据库

                foreach (double key in l2code.Keys)
                {
                    string sql7 = "select MAT_DESC from M_MATERIAL_COOD where L2_CODE=" + key + " and MAT_DESC in (select MAT_DESC from MC_POPCAL_CONSUME where TIMESTAMP='" + start + "' and POP_CLASS='" + class1 + "');";
                    System.Data.DataTable table7 = dBSQL.GetCommand(sql7);
                    string sql5 = "select MAT_DESC from M_MATERIAL_COOD where L2_CODE=" + key + "";
                    System.Data.DataTable table5 = dBSQL.GetCommand(sql5);
                    string value = table5.Rows[0][0].ToString();
                    if (table7.Rows.Count > 0)
                    {
                        //该物料已存在表中
                        string sql8 = "update MC_POPCAL_CONSUME set MAT_VALUE='" + l2code[key] + "' where TIMESTAMP='" + start + "' and POP_CLASS='" + class1 + "' and MAT_DESC='" + value + "'";
                        long _urs = dBSQL.CommandExecuteNonQuery(sql8);
                        if (_urs > 0)
                        {
                            vLog.writelog("MC_POPCAL_CONSUME表中更新" + value + "记录成功", 0);
                        }
                        else
                        {
                            vLog.writelog("MC_POPCAL_CONSUME表中更新" + value + "记录失败", -1);
                        }
                    }
                    else
                    {
                        //该物料不存在插入
                        string sql6 = "insert into MC_POPCAL_CONSUME(TIMESTAMP,POP_CLASS,L2_CODE,MAT_DESC,MAT_VALUE) values('" + start + "','" + class1 + "','" + key + "','" + value + "','" + l2code[key] + "');";
                        long _urs = dBSQL.CommandExecuteNonQuery(sql6);
                        if (_urs > 0)
                        {
                            vLog.writelog("MC_POPCAL_CONSUME表中插入"+ value + "记录成功", 0);
                        }
                        else
                        {
                            vLog.writelog("MC_POPCAL_CONSUME表中插入" + value + "记录失败", -1);
                        }
                    } 
                }
            }
            catch
            {

            }   
        }
        //固体燃耗计算
        double PAR_SO_T=0.0, K1=0.0, K2=0.0, H_FP_ACCU=0.0,H_Coke_ACCU=0.0, H_TSC_CON=0.0,H_T_ACCU = 0.0,H_TSC_CON_L=0.0, H_Coke_ACCU_1=0.0,H2O=0.0,H_TSC_CON_D=0.0, H_TSC_CON_L_D=0.0;
        public void gtrh()
        {
            try
            {
                string sql = "select PAR_SO_T,PAR_K1,PAR_K2 from MC_POPCAL_PAR";
                System.Data.DataTable table = dBSQL.GetCommand(sql);
                if (table.Rows.Count > 0)
                {
                    PAR_SO_T = Convert.ToDouble(table.Rows[0][0]);
                    K1 = Convert.ToDouble(table.Rows[0][1]);
                    K2 = Convert.ToDouble(table.Rows[0][1]);
                }
                //周期内成品矿重累计值
                string sql2 = "select sum(CFP_PLC_PROD_DELT1_FQ)/60 from C_CFP_PLC_1MIN where TIMESTAMP between '" + DateTime.Now.AddMonths(-2).AddMinutes(-PAR_SO_T) + "' and '" + DateTime.Now.AddMonths(-2) + "';";
                //string sql2 = "select sum(CFP_PLC_PROD_DELT1_FQ)/60 from C_CFP_PLC_1MIN where TIMESTAMP between '"+DateTime.Now.AddMinutes(-PAR_SO_T)+"' and '"+DateTime.Now+"';";
                System.Data.DataTable table2 = dBSQL.GetCommand(sql2);
                if (table2.Rows.Count > 0)
                {
                    H_FP_ACCU = Convert.ToDouble(table2.Rows[0][0]);
                }
                //周期内焦炭配料累计值
                string sql3 = "select BIN_NUM from M_MATERIAL_BINS where L2_CODE between 300 and 399";
                System.Data.DataTable table3 = dBSQL.GetCommand(sql3);
                if (table3.Rows.Count > 0)
                {
                    for (int i = 0; i < table3.Rows.Count; i++)
                    {
                        string sqli = "select MAT_PLC_B_ACC_" + table3.Rows[i][0] + " from C_MAT_PLC_1MIN where TIMESTAMP between '" + DateTime.Now.AddMonths(-2).AddMinutes(-PAR_SO_T) + "' and '" + DateTime.Now.AddMonths(-2) + "';";
                        //string sqli = "select MAT_PLC_B_ACC_" + table3.Rows[i][0]+" from C_MAT_PLC_1MIN where TIMESTAMP between '" + DateTime.Now.AddMinutes(-PAR_SO_T) + "' and '" + DateTime.Now + "';";
                        System.Data.DataTable tablei = dBSQL.GetCommand(sqli);
                        if (tablei.Rows.Count > 0)
                        {
                            double[] acc = new double[tablei.Rows.Count];
                            for (i = 0; i < tablei.Rows.Count; i++)
                            {
                                acc[i] = Convert.ToDouble(tablei.Rows[i][0]);
                            }
                            H_Coke_ACCU = H_Coke_ACCU + getSum(acc);
                        }
                    }
                }
                //实际固体燃耗湿基
                H_TSC_CON = K1 * (H_Coke_ACCU / K2 * H_FP_ACCU);
                //周期内理论产量累计值
                string sql4 = "select sum(SINCAL_OUTPUT_PV) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + DateTime.Now.AddMonths(-2).AddMinutes(-PAR_SO_T) + "' and '" + DateTime.Now.AddMonths(-2) + "';";
                //string sql4 = "select sum(SINCAL_OUTPUT_PV) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + DateTime.Now.AddMinutes(-PAR_SO_T) + "' and '" + DateTime.Now + "';";
                System.Data.DataTable table4 = dBSQL.GetCommand(sql4);
                if (table4.Rows.Count > 0)
                {
                    H_T_ACCU = Convert.ToDouble(table4.Rows[0][0]);
                }
                //理论固体燃耗湿基
                H_TSC_CON_L = K1 * (H_Coke_ACCU / K2 * H_T_ACCU);
                string sH2O = "select avg(C_H2O) from M_MATERIAL_BINS where L2_CODE between 300 and 399";
                System.Data.DataTable tH2O = dBSQL.GetCommand(sH2O);
                if (tH2O.Rows.Count > 0)
                {
                    H2O = Convert.ToDouble(tH2O.Rows[0][0]);
                }
                H_TSC_CON_D = K1 * ((H_Coke_ACCU * (1 - H2O / 100)) / (K2 * H_FP_ACCU));
                H_TSC_CON_L_D = K1 * ((H_Coke_ACCU * (1 - H2O / 100) / K2 * H_T_ACCU));
                string sql5 = "insert MC_POPCAL_RESULT_HOUR(TIMESTAMP,POPCAL_H_TSC_CON,POPCAL_H_COKE_ACCU,POPCAL_H_FP_ACCU,POPCAL_H_TSC_CON_LL,POPCAL_H_COKE_ACC1,POPCAL_H_LL_ACCU,POPCAL_H_TSC_CON_DRY," +
                    "POPCAL_H_COKE_H2O,POPCAL_H_TSC_CON_LL_DRY) values('" + DateTime.Now.AddMonths(-2) + "'," + H_TSC_CON + "," + H_Coke_ACCU + "," + H_FP_ACCU + "," + H_TSC_CON_L + "," + H_Coke_ACCU + "," +
                    "" + H_T_ACCU + "," + H_TSC_CON_D + "," + H2O + "," + H_TSC_CON_L_D + ")";
                //string sql5 = "insert MC_POPCAL_RESULT_HOUR(TIMESTAMP,POPCAL_H_TSC_CON,POPCAL_H_COKE_ACCU,POPCAL_H_FP_ACCU,POPCAL_H_TSC_CON_LL,POPCAL_H_COKE_ACC1,POPCAL_H_LL_ACCU,POPCAL_H_TSC_CON_DRY," +
                //   "POPCAL_H_COKE_H2O,POPCAL_H_TSC_CON_LL_DRY,POPCAL_H_COKE_ACCU_1) values('" + DateTime.Now + "'," + H_TSC_CON + "," + H_Coke_ACCU + "," + H_FP_ACCU + "," + H_TSC_CON_L + "," + H_Coke_ACCU + "," +
                //   "" + H_T_ACCU + "," + H_TSC_CON_D + "," + H2O + "," + H_TSC_CON_L_D + "," + H_Coke_ACCU + ")";
                long _urs = dBSQL.CommandExecuteNonQuery(sql5);
                if (_urs > 0)
                {
                    vLog.writelog("固体燃耗插入成功", 0);
                }
                else
                {
                    vLog.writelog("固体燃耗插入失败", -1);
                }
            }
            catch { }
        }
    }
}
