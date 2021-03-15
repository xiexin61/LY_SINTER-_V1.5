using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DataBase;
using VLog;
using System.Threading.Tasks;
//using System.Windows.Forms

namespace SCZZJH
{
    public class His_CL
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public vLog vLog { get; set; }
        //public Timer Timer_1 { get; set; }
        //public Timer Timer_2 { get; set; }
        //20201219测试代码修改函数名，原函数名His_CL
        public His_CL()
        {
            if (vLog == null)
                vLog = new vLog(".\\log\\FireRet\\");
        }

        //烧结矿计划
        public void Plan()
        {
            //DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            DateTime Start = GetStartTime();
            double Xmes = 0.0, Sum = 0.0, X = 0.0;
            string date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00");
            //string sql1 = "select POPCAL_MON_PL from MC_POPCAL_MON_PL where POPCAL_MON='" + 202009 + "'";
            string sql1 = "select POPCAL_MON_PL from MC_POPCAL_MON_PL where POPCAL_MON='" + date + "'";
            DateTime Start1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            System.Data.DataTable table1 = dBSQL.GetCommand(sql1);
            if (table1.Rows.Count > 0)
            {
                Xmes = Convert.ToDouble(table1.Rows[0]["POPCAL_MON_PL"]);
            }
            int D = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            if (DateTime.Now.Day == 1)
            {
                X = Xmes / D;
                //插入数据库

            }
            else
            {
                String sql = "select isnull(sum(A_AL_OUTPUT),0) from MC_POPCAL_OUT where TIMESTAMP >= '" + Start1 + "' and  TIMESTAMP < '" + Start + "';";
                System.Data.DataTable table2 = dBSQL.GetCommand(sql);
                if (Convert.ToDouble(table2.Rows[0][0]) != 0)
                {
                    Sum = Convert.ToDouble(table2.Rows[0][0]);
                }
                else
                {
                    String sql2 = "select sum(CFP_PLC_PROD_DELT1_FQ)/60 from C_CFP_PLC_1MIN where TIMESTAMP >= '" + Start1 + "' and  TIMESTAMP < '" + Start + "';";
                    System.Data.DataTable table3 = dBSQL.GetCommand(sql2);
                    if (table3.Rows.Count > 0)
                    {
                        Sum = Convert.ToDouble(table3.Rows[0][0]);
                    }
                }
                X = (Xmes - Sum) / D - (DateTime.Now.Day - 1);
            }
            string his = "IF EXISTS (SELECT TIMESTAMP FROM MC_POPCAL_RESULT WHERE TIMESTAMP='" + Start + "')" +
                "update MC_POPCAL_RESULT set POPCAL_A_OUT_PL=" + X + ",POPCAL_D_OUT_PL=" + X / 2 + ",POPCAL_N_OUT_PL=" + X / 2 + " ,FLAG_1=0 WHERE TIMESTAMP='" + Start + "'else " +
                "insert into MC_POPCAL_RESULT(TIMESTAMP,POPCAL_A_OUT_PL,POPCAL_D_OUT_PL,POPCAL_N_OUT_PL)" +
                " values('" + Start + "'," + X + "," + X / 2 + "," + X / 2 + ");";
            long _urs = dBSQL.CommandExecuteNonQuery(his);

        }
        //理论批重计算，只在每晚8点计算一次
        public void LLPiZhong()
        {
            //DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            /*string sql = "select PAR_DAY_START,PAR_OUT_T from MC_POPCAL_PAR";
            int PAR_DAY_START = 0, Time = 0;
            System.Data.DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                PAR_DAY_START = Convert.ToInt32(table.Rows[0][0]);
                Time = Convert.ToInt32(table.Rows[0][1]);

            }
            DateTime Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PAR_DAY_START, 0, 0);*/
            DateTime Start = GetStartTime();
            double Q_BW_THR = 0.0, M_P = 0.0, K = 0.0, Q_THR = 0.0, Q_REA = 0.0, R_SIN = 0.0, R_LOT = 0.0, R_H2O = 0.0;
            string sM_P = "select POPCAL_A_OUT_PL from MC_POPCAL_RESULT where TIMESTAMP = '" + Start + "'";
            System.Data.DataTable TM_P = dBSQL.GetCommand(sM_P);
            if (TM_P.Rows.Count > 0)
            {
                M_P = Convert.ToDouble(TM_P.Rows[0][0]);
            }
            //string sK = "select PAR_K from MC_POPCAL_PAR where TIMESTAMP = '" + Start + "'";
            string sK = "select top(1) PAR_K from MC_POPCAL_PAR";
            System.Data.DataTable TK = dBSQL.GetCommand(sK);
            if (TK.Rows.Count > 0)
            {
                K = Convert.ToDouble(TK.Rows[0][0]);
            }
            string sQ_THR = "select isnull(sum(SINCAL_OUTPUT_PV),0) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + Start.AddDays(-1) + "'and '" + Start + "'";
            System.Data.DataTable TQ_THR = dBSQL.GetCommand(sQ_THR);
            if (TQ_THR.Rows.Count > 0)
            {
                Q_THR = Convert.ToDouble(TQ_THR.Rows[0][0]);
            }
            string sQ_REA = "select isnull(sum(CFP_PLC_PROD_DELT1_FQ),0) from C_CFP_PLC_1MIN where TIMESTAMP between '" + Start.AddDays(-1) + "'and '" + Start + "'";
            System.Data.DataTable TQ_REA = dBSQL.GetCommand(sQ_REA);
            if (TQ_REA.Rows.Count > 0)
            {
                Q_REA = Convert.ToDouble(TQ_REA.Rows[0][0]) / 60;
            }
            string sR_SIN = "select isnull(avg(MAT_L2_PBBFB_7),0) from MC_MIXCAL_BILL_INFO_RESULT where TIMESTAMP between '" + Start.AddDays(-1) + "'and '" + Start + "'";
            System.Data.DataTable TR_SIN = dBSQL.GetCommand(sR_SIN);
            if (TR_SIN.Rows.Count > 0)
            {
                R_SIN = Convert.ToDouble(TR_SIN.Rows[0][0]);
            }
            /*string sR_H2O = "select avg(SINCAL_MIX_SP_LOT),avg(SINCAL_MIX_SP_H2O_2) from MC_SINCAL_result_1min where TIMESTAMP between '" + Start.AddDays(-1) + "'and '" + Start + "'";
            System.Data.DataTable TR_H2O = dBSQL.GetCommand(sR_H2O);
            if (TR_H2O.Rows.Count > 0)
            {
                R_LOT = Convert.ToDouble(TR_H2O.Rows[0][0]);
                R_H2O = Convert.ToDouble(TR_H2O.Rows[0][1]);
            }*/
            R_LOT = 1.2; R_H2O = 1.3;
            Q_BW_THR = K * (Q_REA / Q_THR) * ((M_P / 24) / (1 - R_SIN / 100 - R_LOT / 100 + R_H2O / 100));
            string LLPZ =
            "IF EXISTS (SELECT TIMESTAMP FROM MC_POPCAL_RESULT WHERE TIMESTAMP='" + Start + "')" +
                "update MC_POPCAL_RESULT set POPCAL_BW_THR=" + Q_BW_THR + ",POPCAL_OUT_THR=" + Q_THR + ",POPCAL_OUT_REA=" + Q_REA + ", POPCAL_SIN_RM=" + R_SIN + "" +
                ", POPCAL_LOT=" + R_LOT + ", POPCAL_H2O=" + R_H2O + " WHERE TIMESTAMP='" + Start + "'  else " +
                "insert into MC_POPCAL_RESULT(TIMESTAMP,POPCAL_BW_THR,POPCAL_OUT_THR,POPCAL_OUT_REA,POPCAL_SIN_RM,POPCAL_LOT,POPCAL_H2O)" +
                " values('" + Start + "'," + Q_BW_THR + "," + Q_THR + "," + Q_REA + "," + R_SIN + "," + R_LOT + "," + R_H2O + ");";

            long _urs = dBSQL.CommandExecuteNonQuery(LLPZ);
            if (_urs > 0)
            {
                vLog.writelog("理论批重插入成功", 0);
                //Console.WriteLine("理论批重插入成功");
            }
            else
            {
                vLog.writelog("理论批重插入失败", -1);
                //Console.WriteLine("理论批重插入失败");
            }
        }

        public DateTime GetStartTime()
        {
            //DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select PAR_DAY_START,PAR_OUT_T from MC_POPCAL_PAR";
            int PAR_DAY_START = 0, Time = 0;
            System.Data.DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                PAR_DAY_START = Convert.ToInt32(table.Rows[0][0]);
                Time = Convert.ToInt32(table.Rows[0][1]);

            }
            DateTime Start = new DateTime();
            if (DateTime.Now.Hour < PAR_DAY_START)
            {
                Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-1).Day, PAR_DAY_START, 0, 0);
            }
            else
            {
                Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, PAR_DAY_START, 0, 0);
            }
            return Start;
        }
        //历史产量计算
        public void His_CLJS()
        {
            //DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            double QTPlan = 0.0, BBPlan = 0.0, YBPlan = 0.0;
            double QTtheory = 0.0, BBtheory = 0.0, YBtheory = 0.0;
            double QTactual = 0.0, BBactual = 0.0, YBactual = 0.0;
            DateTime Start = GetStartTime();
            string sQTPlan = "select TOP 1 POPCAL_A_OUT_PL from MC_POPCAL_RESULT order by TIMESTAMP desc";
            System.Data.DataTable TQTPlan = dBSQL.GetCommand(sQTPlan);
            if (TQTPlan.Rows.Count > 0)
            {
                QTPlan = Convert.ToDouble(TQTPlan.Rows[0][0]);
            }
            BBPlan = QTPlan / 2;
            YBPlan = QTPlan / 2;
            //全天理论产量
            string theory = "select sum(SINCAL_OUTPUT_PV) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + Start + "' and '" + DateTime.Now + "'";
            System.Data.DataTable Tllcl = dBSQL.GetCommand(theory);
            if (Tllcl.Rows.Count > 0)
            {
                QTtheory = Convert.ToDouble(Tllcl.Rows[0][0]);
            }
            //白班理论产量
            string theory1 = "select sum(SINCAL_OUTPUT_PV) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + Start + "' and '" + DateTime.Now + "'";
            System.Data.DataTable Tybllcl = dBSQL.GetCommand(theory1);
            if (Tybllcl.Rows.Count > 0)
            {
                BBtheory = Convert.ToDouble(Tybllcl.Rows[0][0]);
            }
            //夜班理论产量
            string theory2 = "select sum(SINCAL_OUTPUT_PV) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + Start.AddHours(12) + "' and '" + Start.AddDays(1) + "'";
            System.Data.DataTable Tbbllcl = dBSQL.GetCommand(theory2);
            if (Tbbllcl.Rows.Count > 0)
            {
                YBtheory = Convert.ToDouble(Tbbllcl.Rows[0][0]);
            }
            //全天实际产量
            string sjcl = "select sum(CFP_PLC_PROD_DELT1_FQ)/60 from C_CFP_PLC_1MIN where TIMESTAMP between '" + Start + "' and '" + DateTime.Now + "'";
            System.Data.DataTable Tsjcl = dBSQL.GetCommand(sjcl);
            if (Tsjcl.Rows.Count > 0)
            {
                QTactual = Convert.ToDouble(Tsjcl.Rows[0][0]);
            }
            //白班实际产量
            string ybsjcl = "select sum(CFP_PLC_PROD_DELT1_FQ)/60 from C_CFP_PLC_1MIN where TIMESTAMP between '" + Start + "' and '" + DateTime.Now + "'";
            System.Data.DataTable Tybsjcl = dBSQL.GetCommand(ybsjcl);
            if (Tybsjcl.Rows.Count > 0)
            {
                BBactual = Convert.ToDouble(Tybsjcl.Rows[0][0]);
            }
            //夜班实际产量
            string bbsjcl = "select sum(CFP_PLC_PROD_DELT1_FQ)/60 from C_CFP_PLC_1MIN where TIMESTAMP between '" + Start.AddHours(12) + "' and '" + Start.AddDays(1) + "'";
            System.Data.DataTable Tbbsjcl = dBSQL.GetCommand(bbsjcl);
            if (Tbbsjcl.Rows.Count > 0)
            {
                YBactual = Convert.ToDouble(Tbbsjcl.Rows[0][0]);
            }

            string his = "IF EXISTS (SELECT TIMESTAMP FROM MC_POPCAL_OUT WHERE TIMESTAMP='" + Start + "')" +
                "update MC_POPCAL_OUT set P_AL_OUTPUT=" + QTPlan + ",P_D_OUTPUT=" + BBPlan + ",P_N_OUTPUT=" + YBPlan + ",T_AL_OUTPUT=" + QTtheory + ",T_D_OUTPUT=" + BBtheory + ",T_N_OUTPUT=" + YBtheory
                + ",A_AL_OUTPUT=" + QTactual + ",A_D_OUTPUT=" + BBactual + ",A_N_OUTPUT=" + YBactual + " where TIMESTAMP='" + Start + "' else " +
                "insert into MC_POPCAL_OUT(TIMESTAMP,P_AL_OUTPUT,P_D_OUTPUT,P_N_OUTPUT,T_AL_OUTPUT,T_D_OUTPUT,T_N_OUTPUT,A_AL_OUTPUT,A_D_OUTPUT,A_N_OUTPUT)" +
                " values('" + Start + "'," + QTPlan + "," + BBPlan + "," + YBPlan + "," + QTtheory + "," + BBtheory + "," + YBtheory + "," + QTactual + "," + BBactual + "," + YBactual + ");";
            long _urs = dBSQL.CommandExecuteNonQuery(his);
            if (_urs > 0)
            {
                vLog.writelog("历史产量插入成功", 0);
            }
            else
            {
                vLog.writelog("历史产量插入失败", -1);
            }
        }
    }
}
