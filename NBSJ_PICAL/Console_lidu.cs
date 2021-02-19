
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;
using System.Reflection;
using System.Data;
using System.Threading;
using MathNet.Numerics.Statistics;

namespace NBSJ_PICAL
{
    public class Console_lidu 
    {
        public M_PICAL_PAR _M_PICAL_PAR = new M_PICAL_PAR();
        SqlSugarClient db_sugar = GetInstance();
        DateTime LastDateTime2 = DateTime.MinValue;
        DateTime LastDateTime3 = DateTime.MinValue;
        public double JPU;
        public double[] Cor_Result;
        public static double[,] RELEVANCE_SZ_l;
        
        System.Timers.Timer timer;

        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = ADODB.ConnectionString, DbType = SqlSugar.DbType.SqlServer, IsAutoCloseConnection = true });
            return db;
        }
        private void OnTimer_OneSec(object source, System.Timers.ElapsedEventArgs e)
        {
            RunTask(DateTime.Now);
        }

        private void OnTimer_Cycle()
        {
            while (true)
            {
                try
                {


                    RunTask(DateTime.Now);
                }
                catch (Exception ee)
                {
                     LogHelper.LogWhite("调用OnTimer_Cycle失败" + ee.Message);
                }
                //间隔5min 一个小时计算一次
                Thread.Sleep(1000);
            }
        }
            public  void RunTask(DateTime currentTime)
        {
            
            string log = "";
            try
            {  if(LastDateTime2.AddMinutes(_M_PICAL_PAR.PAR_T2) < DateTime.Now)
                {
                    LogHelper.LogInfo("JPU计算开始，周期"+ _M_PICAL_PAR.PAR_T2+"min");
                    _M_PICAL_PAR = GET_M_PICAL_PAR();

                    LastDateTime2 = DateTime.Now;
                    JPU = JPU_CAL(_M_PICAL_PAR, currentTime);
                    
                    LogHelper.LogInfo("JPU计算完成");
                }
                if ((LastDateTime3.AddMinutes(_M_PICAL_PAR.PAR_T1) < DateTime.Now))
                {
                    LogHelper.LogInfo("PICAL相关性计算开始，周期"+ _M_PICAL_PAR.PAR_T1+"min");
                    _M_PICAL_PAR = GET_M_PICAL_PAR();
                    LastDateTime3 = DateTime.Now;
                    SET_SAVE(JPU, _M_PICAL_PAR, currentTime);
                    int M_ROW = _M_PICAL_PAR.PAR_NUMBER_ROW;

                    Cor_Result=BEISHAO_JICHUSHUJU(M_ROW);
                    SetResultValues();
                    
                    LogHelper.LogInfo("PICAL相关性计算完成");
                    LogHelper.LogDEL();
                }
               
            }
            catch (Exception ee)
            {
                throw ee;
            }
            finally
            {
                //释放资源
                // log = null;
            }
        }
        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="iDataBase"></param>
        /// <returns></returns>
        public M_PICAL_PAR GET_M_PICAL_PAR()
        {

            M_PICAL_PAR item = null;
            string strSQL = "select top(1) * from M_PICAL_PAR ";
           item = db_sugar.SqlQueryable<M_PICAL_PAR>(strSQL).ToList().FirstOrDefault();
            return item;
        }

       

        /// <summary>
        /// 读取二混粒度数据
        /// </summary>
        /// <param name="iDataBase"></param>
        /// <returns></returns>
        public MD_PHY_PARTICLE_INFO_IN GET_MD_PHY_PARTICLE_INFO_IN(M_PICAL_PAR M_PICAL_PAR_)
        {
            DateTime End_Times = DateTime.Now;
            DateTime StaterTimes = DateTime.Now.AddMinutes(-M_PICAL_PAR_.PAR_T2);
            string str_sql = string.Format("select isnull(round(avg(GRIT_LOW_1),3),0) as GRIT_LOW_1,isnull(round(avg(GRIT_1_3),3),0) as GRIT_1_3,isnull(round(avg(GRIT_UP_3),3),0) as GRIT_UP_3,isnull(round(avg(MATERIAL_TE),3),0) as MATERIAL_TE     from MD_PHY_PARTICLE_INFO_IN where timestamp >'{1}' and timestamp<='{0}' and L2_CODE=901", End_Times, StaterTimes);
            // return iDataBase.GetSingle<MD_PHY_PARTICLE_INFO_IN>(str_sql); <MC_BTPCAL_RESULT_1MIN>(sql).ToList();
            return db_sugar.SqlQueryable<MD_PHY_PARTICLE_INFO_IN>(str_sql).ToList().FirstOrDefault();
        }
       
        /// <summary>
        /// 主抽温度,主抽1风量、主抽2风量、主抽1负压、主抽2负压
        /// </summary>
        /// <param name="M_PICAL_PAR_"></param>
        /// <param name="MC_MICAL_PAR_"></param>
        /// <param name="MC_MICAL_RESULT_"></param>
        /// <param name="MD_PHY_PARTICLE_INFO_IN_"></param>
        /// <param name="iDataBase"></param>
        /// <returns></returns>
        public C_SIN_PLC_1MIN GET_C_SIN_PLC_1MIN(M_PICAL_PAR M_PICAL_PAR)
        {
            int SIN_U_TIME_PV = Convert.ToInt16(M_PICAL_PAR.PAR_K2);
            DateTime End_Times = DateTime.Now;
            DateTime StaterTimes = DateTime.Now.AddMinutes(-SIN_U_TIME_PV);
            string str_sql = string.Format("select  isnull(round(avg((PS_MA_SB_1_FLUE_TE)/2),3),0) as PS_MA_SB_1_FLUE_TE , isnull(round(avg(PS_MA_SB_1_FLUE_FT),3),0) as PS_MA_SB_1_FLUE_FT, isnull(round(avg(PS_MA_SB_2_FLUE_FT),3),0) as PS_MA_SB_2_FLUE_FT, isnull(round(avg(PS_MA_SB_1_FLUE_PT),3),0) as PS_MA_SB_1_FLUE_PT , isnull(round(avg(PS_MA_SB_2_FLUE_PT),3),0) as PS_MA_SB_2_FLUE_PT from C_SIN_PLC_1MIN where timestamp >'{1}' and timestamp<='{0}'", End_Times, StaterTimes);
            return db_sugar.SqlQueryable<C_SIN_PLC_1MIN>(str_sql).ToList().FirstOrDefault();
        }
        
        /// <summary>
        /// 垂直烧结速度
        /// </summary>
        /// <param name="M_PICAL_PAR_"></param>
        /// <param name="MC_MICAL_PAR_"></param>
        /// <param name="MC_MICAL_RESULT_"></param>
        /// <param name="MD_PHY_PARTICLE_INFO_IN_"></param>
        /// <param name="iDataBase"></param>
        /// <returns></returns>
        public MC_BTPCAL_RESULT_1MIN GET_MC_BTPCAL_RESULT_1MIN(M_PICAL_PAR M_PICAL_PAR_)
        {
            DateTime End_Times = DateTime.Now;
            DateTime StaterTimes = DateTime.Now.AddMinutes(-M_PICAL_PAR_.PAR_T2);
            string str_sql = string.Format("select  isnull(round(avg(BTPCAL_V),3),0) as BTPCAL_V  from MC_BTPCAL_RESULT_1MIN where timestamp >'{1}' and timestamp<'{0}'", End_Times, StaterTimes);
            return db_sugar.SqlQueryable<MC_BTPCAL_RESULT_1MIN>(str_sql).ToList().FirstOrDefault();
        }
        /// <summary>
        /// 透气性计算
        /// </summary>
        /// <param name="T_MA_PGD_PLC_1MIN_"></param>
        /// <param name="T_CLOTH_PLC_1MIN_"></param>
        /// <param name="M_PICAL_PAR_"></param>
        /// <param name="MC_MICAL_PAR_"></param>
        /// <returns></returns>
        public double JPU_CAL(M_PICAL_PAR M_PICAL_PAR_, DateTime CurTime)
        {
            double JPU = 0;
            double fenmu = 0;
            double Q = 0;
            double K1 = 0;
            double A = 0;
            double P = 0;
            double H = 0;
            double K2 = 0;
            double n = 0.6;
            string log = "";
            M_PICAL_BREATH_RESULT_T2 _M_PICAL_BREATH_RESULT_T2 = new M_PICAL_BREATH_RESULT_T2();
            _M_PICAL_BREATH_RESULT_T2 = Get_PICAL_B01_ZT_PV(M_PICAL_PAR_, CurTime);

            //Q = M_PICAL_PAR_.PAR_RATED_Q;
            Q =Get_Q(M_PICAL_PAR_, CurTime);
            K2 = M_PICAL_PAR_.PAR_K2;
            K1 = M_PICAL_PAR_.PAR_K1;
            double  NUM = M_PICAL_PAR_.PAR_NUM;
            A = M_PICAL_PAR_.PAR_A;
            n = M_PICAL_PAR_.PAR_N;
            double W = _M_PICAL_BREATH_RESULT_T2.PICAL_W;
            double B01_ZT_PV = _M_PICAL_BREATH_RESULT_T2.PICAL_B01_ZT_PV;
            P = _M_PICAL_BREATH_RESULT_T2.PICAL_P;
            n = M_PICAL_PAR_.PAR_N;
            fenmu = K2 * A;
            H = _M_PICAL_BREATH_RESULT_T2.PICAL_H;
            if (A > 0 && NUM > 0 && P > 0)
            {
                //JPU = K2 * (B01_ZT_PV / 100) *((Q* W*(1-K1/100))/(NUM*A)) * (Math.Pow(H / (P * 1000), n));
                JPU = K2 * ((Q * (1 - K1 / 100)) / A) * (Math.Pow(H / (P * 1000), n));
            }
            _M_PICAL_BREATH_RESULT_T2.PICAL_Q = Q;
            _M_PICAL_BREATH_RESULT_T2.PICAL_JPU = JPU;
            if (Set_Admin_addT2(_M_PICAL_BREATH_RESULT_T2))
            {
                log = "M_PICAL_BREATH_RESULT_T2保存完成";
                LogHelper.LogInfo(log);
            }
            else
            {
                log = "M_PICAL_BREATH_RESULT_T2保存失败";
                LogHelper.LogError(log);
            }
           
            return JPU;
        }

        public double Get_Q(M_PICAL_PAR M_PICAL_PAR_, DateTime Time)
        {
            double Q = 0;
            string str_sql = "select  isnull(round(avg((SIN_PLC_MA_SB_1_FLUE_FT+SIN_PLC_MA_SB_2_FLUE_FT)),3),0) as SIN_PLC_FLUE_FT from C_SIN_PLC_1MIN where timestamp>= '" + Time.AddMinutes(-M_PICAL_PAR_.PAR_T2) + "' and timestamp<'" + Time + "'";
            Q = db_sugar.Ado.GetDouble(str_sql);
            return Q;
        }
        public M_PICAL_BREATH_RESULT_T2 Get_PICAL_B01_ZT_PV(M_PICAL_PAR M_PICAL_PAR_, DateTime Time)
        {
            M_PICAL_BREATH_RESULT_T2 _M_PICAL_BREATH_RESULT_T2 = new M_PICAL_BREATH_RESULT_T2();
            _M_PICAL_BREATH_RESULT_T2.TIMESTAMP = Time;
            C_SIN_PLC_1MIN _C_SIN_PLC_1MIN = new C_SIN_PLC_1MIN();
            //20210218 @lt 修改数据源
            //   string str_sql = "select  isnull(round(avg((SIN_PLC_B01_ZT_L_PV+SIN_PLC_B01_ZT_R_PV)/2),3),0) as SIN_PLC_B01_ZT_L_PV,isnull(round(avg((SIN_PLC_MA_FAN_1_PV+SIN_PLC_MA_FAN_2_PV)/2),3),0) as SIN_PLC_MA_FAN_1_PV,isnull(round(avg(abs(SIN_PLC_B01_PT_L+SIN_PLC_B01_PT_R)/2),3),0) as SIN_PLC_B01_PT_L  from C_SIN_PLC_1MIN where timestamp>= '" + Time.AddMinutes(-M_PICAL_PAR_.PAR_T2) +"' and timestamp<'"+Time+"'";
            string str_sql = "select  isnull(round(avg((SIN_PLC_B01_ZT_L_PV+SIN_PLC_B01_ZT_R_PV)/2),3),0) as SIN_PLC_B01_ZT_L_PV,isnull(round(avg((SIN_PLC_MA_FAN_1_PV+SIN_PLC_MA_FAN_2_PV)/2),3),0) as SIN_PLC_MA_FAN_1_PV,isnull(round(avg(abs(SIN_PLC_MA_IN_1_FLUE_PT+SIN_PLC_MA_IN_2_FLUE_PI)/2),3),0) as SIN_PLC_B01_PT_L  from C_SIN_PLC_1MIN where timestamp>= '" + Time.AddMinutes(-M_PICAL_PAR_.PAR_T2) + "' and timestamp<'" + Time + "'";

            _C_SIN_PLC_1MIN = db_sugar.SqlQueryable<C_SIN_PLC_1MIN>(str_sql).ToList().FirstOrDefault();
            _M_PICAL_BREATH_RESULT_T2.PICAL_B01_ZT_PV = _C_SIN_PLC_1MIN.SIN_PLC_B01_ZT_L_PV;
            if (M_PICAL_PAR_.PAR_RATED_MA_FAN != 0)
            {
                _M_PICAL_BREATH_RESULT_T2.PICAL_W = _C_SIN_PLC_1MIN.SIN_PLC_MA_FAN_1_PV / M_PICAL_PAR_.PAR_RATED_MA_FAN;
            }
            _M_PICAL_BREATH_RESULT_T2.PICAL_P = _C_SIN_PLC_1MIN.SIN_PLC_B01_PT_L;
            
            string sql = "select  isnull(round(avg(F_PLC_THICK_PV),3),0) as F_PLC_THICK_PV from C_MFI_PLC_1MIN where timestamp>= '" + Time.AddMinutes(-M_PICAL_PAR_.PAR_T2) + "' and timestamp<'" + Time + "'";           
            _M_PICAL_BREATH_RESULT_T2.PICAL_H = db_sugar.Ado.GetDouble(sql);
            return _M_PICAL_BREATH_RESULT_T2;
        }
        public void SET_SAVE(double JPU, M_PICAL_PAR M_PICAL_PAR_,DateTime curTime)
        {
            M_PICAL_BREATH_RESULT M_PICAL_BREATH_RESULT_ = null;
            string log = "";
            try
            {
                M_PICAL_BREATH_RESULT_ = new M_PICAL_BREATH_RESULT();

                ///
                #region  计算时间段内的平均值
                M_PICAL_BREATH_RESULT_ = Get_M_PICAL_BREATH_RESULT(JPU, M_PICAL_PAR_,curTime);
                #endregion
                #region  二混到烧结机台车耗时
                int RHDSJJTCHS = (GetRHDSJJTCHS(curTime)+ _M_PICAL_PAR.PAR_T1);
                Get_M_PICAL_BREATH_RESULT1(curTime, RHDSJJTCHS, M_PICAL_BREATH_RESULT_);
                #endregion
                #region 二混到一混倒推耗时
                int RHDYHDTSJHS = (GetRHDYHDTSJHS(curTime)+ RHDSJJTCHS + _M_PICAL_PAR.PAR_T1);
                Get_M_PICAL_BREATH_RESULT2(curTime, RHDYHDTSJHS, M_PICAL_BREATH_RESULT_);

                #endregion
                #region 一混到配料倒推耗时time3
                int YHDPLDTHS = (GetYHDPLDTHS(curTime)+ RHDSJJTCHS+ RHDYHDTSJHS+ _M_PICAL_PAR.PAR_T1);
                Get_M_PICAL_BREATH_RESULT3(curTime, YHDPLDTHS, M_PICAL_BREATH_RESULT_);


                #endregion 
            }
            catch (Exception ee)
            {
                LogHelper.LogError(ee.Message);
                throw;
            }
            if (Set_Admin_add(M_PICAL_BREATH_RESULT_))
            {
                log = "M_PICAL_BREATH_RESULT保存完成";
                LogHelper.LogInfo(log);
            }
            else
            {
                log = "M_PICAL_BREATH_RESULT保存失败";
                LogHelper.LogError(log);
            }
           

        }
        private int GetRHDSJJTCHS(DateTime curTime)
        {
            DateTime End_Times = curTime;
            DateTime StaterTimes = DateTime.Now.AddMinutes(-30);
            string str_sql = string.Format("select  isnull(round(avg(MICAL_CLO_2M_TIME),3),0) as MICAL_CLO_2M_TIME  from MC_MICAL_RESULT where timestamp >'{1}' and timestamp<='{0}' and DATANUM = 3", End_Times, StaterTimes);
            int T = (int)db_sugar.Ado.GetDouble(str_sql);
            return T;
        }
        private int GetRHDYHDTSJHS(DateTime curTime)
        {
            DateTime End_Times = curTime;
            DateTime StaterTimes = DateTime.Now.AddMinutes(-30);
            string str_sql = string.Format("select  isnull(round(avg(MICAL_2M_1M_TIME),3),0) as MICAL_2M_1M_TIME  from MC_MICAL_RESULT where timestamp >'{1}' and timestamp<='{0}' and DATANUM = 2", End_Times, StaterTimes);
            int T = (int)db_sugar.Ado.GetDouble(str_sql);
            return T;
        }
        private int GetYHDPLDTHS(DateTime curTime)
        {
            DateTime End_Times = curTime;
            DateTime StaterTimes = DateTime.Now.AddMinutes(-40);
            string str_sql = string.Format("select  isnull(round(avg(MICAL_2M_1M_TIME),3),0) as MICAL_2M_1M_TIME  from MC_MICAL_RESULT where timestamp >'{1}' and timestamp<='{0}' and DATANUM = 1", End_Times, StaterTimes);
            int T = (int)db_sugar.Ado.GetDouble(str_sql);
            return T;
        }
        private M_PICAL_BREATH_RESULT Get_M_PICAL_BREATH_RESULT(double JPU, M_PICAL_PAR M_PICAL_PAR_,DateTime curTime)
        {
            M_PICAL_BREATH_RESULT _M_PICAL_BREATH_RESULT = new M_PICAL_BREATH_RESULT();
            _M_PICAL_BREATH_RESULT.TIMESTAMP = curTime;
            _M_PICAL_BREATH_RESULT.PICAL_JPU = JPU;
            string str = "select isnull(round(avg(PICAL_JPU),3),0) from M_PICAL_BREATH_RESULT_T2 where (PICAL_JPU!=0 or PICAL_JPU!=null) and timestamp>'" + curTime .AddMinutes(-M_PICAL_PAR_.PAR_T1) + "' and timestamp<'"+ curTime + "'";
            _M_PICAL_BREATH_RESULT.PICAL_JPU = db_sugar.Ado.GetDouble(str);

            //20210218 @lt 修改数据源 点火温度计算修改使用两个数据平均值
            // String str1 = "select isnull(F_PLC_THICK_PV,0) as F_PLC_THICK_PV,isnull((I_PLC_IG_01_TE+I_PLC_IG_02_TE+I_PLC_IG_03_TE)/3,0) as I_PLC_IG_01_TE,isnull(F_PLC_SIN_SPEED_PV,0) as F_PLC_SIN_SPEED_PV  from C_MFI_PLC_1MIN where timestamp>'" + curTime.AddMinutes(-M_PICAL_PAR_.PAR_T1) + "' and timestamp<'" + curTime + "'";
            String str1 = "select isnull(F_PLC_THICK_PV,0) as F_PLC_THICK_PV,isnull((I_PLC_IG_01_TE+I_PLC_IG_02_TE)/2,0) as I_PLC_IG_01_TE,isnull(F_PLC_SIN_SPEED_PV,0) as F_PLC_SIN_SPEED_PV  from C_MFI_PLC_1MIN where timestamp>'" + curTime.AddMinutes(-M_PICAL_PAR_.PAR_T1) + "' and timestamp<'" + curTime + "'";

            List<C_MFI_PLC_1MIN> _C_MFI_PLC_1MIN = new List<C_MFI_PLC_1MIN>();
            _C_MFI_PLC_1MIN = db_sugar.SqlQueryable<C_MFI_PLC_1MIN>(str1).ToList();
            double[] SUM = new double[3];
            int[] COUNT = new int[3];
            foreach (var item in _C_MFI_PLC_1MIN)
            {
                if (item.F_PLC_THICK_PV != 0)
                {
                    SUM[0] += item.F_PLC_THICK_PV;
                    COUNT[0]++;
                }
                if (item.I_PLC_IG_01_TE!=0)
                {
                    SUM[1] += item.I_PLC_IG_01_TE;
                    COUNT[1]++;
                }
                if (item.F_PLC_SIN_SPEED_PV != 0)
                {
                    SUM[2] += item.F_PLC_SIN_SPEED_PV;
                    COUNT[2]++;
                }
            }
            if (COUNT[0]>0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_H = SUM[0] / COUNT[0];
            }
            if (COUNT[1] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_IG_01_TE = SUM[1] / COUNT[1];
            }
            if (COUNT[2] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_SPARE13 = SUM[2] / COUNT[2];
            }

            //string str1 = "select isnull(round(avg(F_PLC_THICK_PV),3),0) as F_PLC_THICK_PV,isnull(round(avg((I_PLC_IG_01_TE+I_PLC_IG_02_TE+I_PLC_IG_03_TE)/3),3),0) as I_PLC_IG_01_TE,isnull(round(avg(F_PLC_SIN_SPEED_PV),3),0) as F_PLC_SIN_SPEED_PV   from C_MFI_PLC_1MIN ";
            //str1 += "where (F_PLC_THICK_PV!=0 or F_PLC_THICK_PV!=null) and (I_PLC_IG_01_TE!=0 or I_PLC_IG_01_TE!=null) and (I_PLC_IG_02_TE!=0 or I_PLC_IG_02_TE!=null) ";
            //str1 += "and (I_PLC_IG_03_TE!=0 or I_PLC_IG_03_TE!=null) and (F_PLC_SIN_SPEED_PV!=0 or F_PLC_SIN_SPEED_PV!=null) and timestamp>'" + curTime.AddMinutes(-M_PICAL_PAR_.PAR_T1) + "' and timestamp<'" + curTime + "'";
            //C_MFI_PLC_1MIN _C_MFI_PLC_1MIN = new C_MFI_PLC_1MIN();
            //_C_MFI_PLC_1MIN = db_sugar.SqlQueryable<C_MFI_PLC_1MIN>(str1).ToList().FirstOrDefault();

            //if (_C_MFI_PLC_1MIN != null)
            //{
            //    _M_PICAL_BREATH_RESULT.PICAL_BREATH_H = _C_MFI_PLC_1MIN.F_PLC_THICK_PV;
            //    _M_PICAL_BREATH_RESULT.PICAL_BREATH_IG_01_TE = _C_MFI_PLC_1MIN.I_PLC_IG_01_TE;
            //    _M_PICAL_BREATH_RESULT.PICAL_BREATH_SPARE13 = _C_MFI_PLC_1MIN.F_PLC_SIN_SPEED_PV;

            //}
            // string str2 = "select ISNULL((SIN_PLC_MA_SB_1_FLUE_FT+SIN_PLC_MA_SB_2_FLUE_FT)/2,0) as SIN_PLC_MA_SB_1_FLUE_FT,ISNULL((SIN_PLC_MA_SB_1_FLUE_TE+SIN_PLC_MA_SB_2_FLUE_TE)/2,0) as SIN_PLC_MA_SB_1_FLUE_TE,ISNULL((SIN_PLC_B01_PT_L+SIN_PLC_B01_PT_R)/2,0) as SIN_PLC_B01_PT_L   from C_SIN_PLC_1MIN where timestamp>'" + curTime.AddMinutes(-M_PICAL_PAR_.PAR_T1) + "' and timestamp<'" + curTime + "'";
            //20210218 @lt主抽负压添加修改数据源
            string str2 = "select ISNULL((SIN_PLC_MA_SB_1_FLUE_FT+SIN_PLC_MA_SB_2_FLUE_FT)/2,0) as SIN_PLC_MA_SB_1_FLUE_FT,ISNULL((SIN_PLC_MA_SB_1_FLUE_TE+SIN_PLC_MA_SB_2_FLUE_TE)/2,0) as SIN_PLC_MA_SB_1_FLUE_TE,ISNULL((SIN_PLC_MA_IN_1_FLUE_PT+SIN_PLC_MA_IN_2_FLUE_PI)/2,0) as SIN_PLC_B01_PT_L   from C_SIN_PLC_1MIN where timestamp>'" + curTime.AddMinutes(-M_PICAL_PAR_.PAR_T1) + "' and timestamp<'" + curTime + "'";

            List<C_SIN_PLC_1MIN> _C_SIN_PLC_1MIN = new List<C_SIN_PLC_1MIN>();
            _C_SIN_PLC_1MIN = db_sugar.SqlQueryable<C_SIN_PLC_1MIN>(str2).ToList();
            SUM = new double[3];
            COUNT = new int[3];
            foreach (var item in _C_SIN_PLC_1MIN)
            {
                if (item.SIN_PLC_MA_SB_1_FLUE_FT != 0)
                {
                    SUM[0] += item.SIN_PLC_MA_SB_1_FLUE_FT;
                    COUNT[0]++;
                }
                if (item.SIN_PLC_MA_SB_1_FLUE_TE != 0)
                {
                    SUM[1] += item.SIN_PLC_MA_SB_1_FLUE_TE;
                    COUNT[1]++;
                }
                if (item.SIN_PLC_B01_PT_L != 0)
                {
                    SUM[2] += item.SIN_PLC_B01_PT_L;
                    COUNT[2]++;
                }
            }
            if (COUNT[0] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_Q = SUM[0] / COUNT[0];
            }
            if (COUNT[1] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_SPARE14 = SUM[1] / COUNT[1];
            }
            if (COUNT[2] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_P = SUM[2] / COUNT[2];
            }
            return _M_PICAL_BREATH_RESULT;
        }
        private void Get_M_PICAL_BREATH_RESULT1(DateTime curTime,int RHDSJJTCHS,M_PICAL_BREATH_RESULT _M_PICAL_BREATH_RESULT)
        {
            String str1 = "select isnull(M_PLC_2M_FT_SP,0) as M_PLC_2M_FT_SP,isnull(M_PLC_2M_A_WATER_PV,0) as M_PLC_2M_A_WATER_PV   from C_MFI_PLC_1MIN where timestamp>'" + curTime.AddMinutes(-RHDSJJTCHS) + "' and timestamp<'" + curTime.AddMinutes(-_M_PICAL_PAR.PAR_T1) + "'";
            List<C_MFI_PLC_1MIN> _C_MFI_PLC_1MIN = new List<C_MFI_PLC_1MIN>();
            _C_MFI_PLC_1MIN = db_sugar.SqlQueryable<C_MFI_PLC_1MIN>(str1).ToList();
            double[] SUM = new double[2];
            int[] COUNT = new int[2];
            foreach (var item in _C_MFI_PLC_1MIN)
            {
                if (item.M_PLC_2M_FT_SP != 0)
                {
                    SUM[0] += item.M_PLC_2M_FT_SP;
                    COUNT[0]++;
                }
                if (item.M_PLC_2M_A_WATER_PV != 0)
                {
                    SUM[1] += item.M_PLC_2M_A_WATER_PV;
                    COUNT[1]++;
                }
                
            }
            if (COUNT[0] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_2M_FT_PV = SUM[0] / COUNT[0];
            }
            if (COUNT[1] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_2M_NEX_WAT = SUM[1] / COUNT[1];
            }
            


        }
        private void Get_M_PICAL_BREATH_RESULT2(DateTime curTime, int RHDS, M_PICAL_BREATH_RESULT _M_PICAL_BREATH_RESULT)
        {

            String str1 = "select isnull(M_PLC_1M_FT_SP,0) as M_PLC_1M_FT_SP,isnull(M_PLC_1M_A_WATER_PV,0) as M_PLC_1M_A_WATER_PV from C_MFI_PLC_1MIN where timestamp>'" + curTime.AddMinutes(-RHDS) + "' and timestamp<'" + curTime.AddMinutes(-_M_PICAL_PAR.PAR_T1) + "'";
            List<C_MFI_PLC_1MIN> _C_MFI_PLC_1MIN = new List<C_MFI_PLC_1MIN>();
            _C_MFI_PLC_1MIN = db_sugar.SqlQueryable<C_MFI_PLC_1MIN>(str1).ToList();
            double[] SUM = new double[2];
            int[] COUNT = new int[2];
            foreach (var item in _C_MFI_PLC_1MIN)
            {
                if (item.M_PLC_1M_FT_SP != 0)
                {
                    SUM[0] += item.M_PLC_1M_FT_SP;
                    COUNT[0]++;
                }
                if (item.M_PLC_1M_A_WATER_PV != 0)
                {
                    SUM[1] += item.M_PLC_1M_A_WATER_PV;
                    COUNT[1]++;
                }
                
            }
            if (COUNT[0] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_1M_FT_PV = SUM[0] / COUNT[0];
            }
            if (COUNT[1] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_1M_NEX_WAT = SUM[1] / COUNT[1];
            }





        }
        private void Get_M_PICAL_BREATH_RESULT3(DateTime curTime, int RHDS, M_PICAL_BREATH_RESULT _M_PICAL_BREATH_RESULT)
        {

            String str1 = "select isnull(SINCAL_MAT_DRY_1,0) as MAT_L2_DQBFB_1,isnull(SINCAL_MAT_DRY_5,0) as MAT_L2_DQBFB_3,isnull(SINCAL_MAT_DRY_7,0) as MAT_L2_DQBFB_7 from MC_MIXCAL_RESULT_1MIN where timestamp>'" + curTime.AddMinutes(-RHDS) + "' and timestamp<'" + curTime.AddMinutes(-_M_PICAL_PAR.PAR_T1) + "'";
            List<M_MACAL_INTERFACE_RESULT_HIST> _result = new List<M_MACAL_INTERFACE_RESULT_HIST>();
            _result = db_sugar.SqlQueryable<M_MACAL_INTERFACE_RESULT_HIST>(str1).ToList();
            double[] SUM = new double[3];
            int[] COUNT = new int[3];
            if (_result.Count == 0)
            {
                //20210218 @lt 修改数据源
                //  str1 = "select top(1) isnull(SINCAL_BFES_ORE_BILL_DRY,0) as MAT_L2_DQBFB_1,isnull(SINCAL_FLUX_BILL_DRY,0) as MAT_L2_DQBFB_3,isnull(SINCAL_BRUN_DRY,0) as MAT_L2_DQBFB_7 from MC_MIXCAL_RESULT_1MIN  order by timestamp desc";
                str1 = "select top(1) isnull(SINCAL_MAT_DRY_1,0) as MAT_L2_DQBFB_1,isnull(SINCAL_MAT_DRY_5,0) as MAT_L2_DQBFB_3,isnull(SINCAL_MAT_DRY_7,0) as MAT_L2_DQBFB_7 from MC_MIXCAL_RESULT_1MIN  order by timestamp desc";

                M_MACAL_INTERFACE_RESULT_HIST _resultl = new M_MACAL_INTERFACE_RESULT_HIST();
                _result = db_sugar.SqlQueryable<M_MACAL_INTERFACE_RESULT_HIST>(str1).ToList();
                _result.Add(_resultl);
           

            }
            foreach (var item in _result)
            {
                if (item.MAT_L2_DQBFB_1 != 0)
                {
                    SUM[0] += item.MAT_L2_DQBFB_1;
                    COUNT[0]++;
                }
                if (item.MAT_L2_DQBFB_3 != 0)
                {
                    SUM[1] += item.MAT_L2_DQBFB_3;
                    COUNT[1]++;
                }
                if (item.MAT_L2_DQBFB_7 != 0)
                {
                    SUM[2] += item.MAT_L2_DQBFB_7;
                    COUNT[2]++;
                }

            }
            if (COUNT[0] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_BFO_BILL = SUM[0] / COUNT[0];
            }
            if (COUNT[1] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_LIME_BILL = SUM[1] / COUNT[1];
            }
            if (COUNT[2] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_SRM_BILL = SUM[2] / COUNT[2];
            }

            
            String str2 = "select isnull(SINCAL_SIN_PV_R,0) as SINCAL_SIN_PV_R,isnull(SINCAL_SIN_PV_MGO,0) as SINCAL_SIN_PV_MGO,isnull(SINCAL_MIX_PV_C,0) as SINCAL_MIX_PV_C from MC_MIXCAL_RESULT_1MIN where timestamp>'" + curTime.AddMinutes(-RHDS) + "' and timestamp<'" + curTime.AddMinutes(-_M_PICAL_PAR.PAR_T1) + "'";
            List<MC_MIXCAL_RESULT_1MIN> result = new List<MC_MIXCAL_RESULT_1MIN>();
            result = db_sugar.SqlQueryable<MC_MIXCAL_RESULT_1MIN>(str2).ToList();
            SUM = new double[3];
            COUNT = new int[3];
            foreach (var item in result)
            {
                if (item.SINCAL_SIN_PV_R != 0)
                {
                    SUM[0] += item.SINCAL_SIN_PV_R;
                    COUNT[0]++;
                }
                if (item.SINCAL_SIN_PV_MGO != 0)
                {
                    SUM[1] += item.SINCAL_SIN_PV_MGO;
                    COUNT[1]++;
                }
                if (item.SINCAL_MIX_PV_C != 0)
                {
                    SUM[1] += item.SINCAL_MIX_PV_C;
                    COUNT[2]++;
                }

            }
            if (COUNT[0] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_R = SUM[0] / COUNT[0];
            }
            if (COUNT[1] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_MGO = SUM[1] / COUNT[1];
            }
            if (COUNT[2] > 0)
            {
                _M_PICAL_BREATH_RESULT.PICAL_BREATH_C = SUM[2] / COUNT[2];
            }
        }


            private DataTable  Get_TOP_M_PICAL_BREATH_RESULT(int count)
        {
            DataTable dt = new DataTable();
            List<M_PICAL_BREATH_RESULT> List_M_PICAL_BREATH_RESULT = new List<M_PICAL_BREATH_RESULT>();
            
            string str = string.Format(@"select top({0})  isnull(PICAL_JPU,0),
            isnull(PICAL_BREATH_BFO_BILL, 0),
            isnull(PICAL_BREATH_LIME_BILL, 0),
            isnull(PICAL_BREATH_SRM_BILL, 0),
            isnull(PICAL_BREATH_R, 0),
            isnull(PICAL_BREATH_MGO, 0),
            isnull(PICAL_BREATH_C, 0),
            isnull(PICAL_BREATH_1M_NEX_WAT, 0),
            isnull(PICAL_BREATH_H, 0),
            isnull(PICAL_BREATH_IG_01_TE, 0),
            isnull(PICAL_BREATH_SPARE13, 0),
            isnull(PICAL_BREATH_Q, 0),
            isnull(PICAL_BREATH_SPARE14, 0),
            isnull(PICAL_BREATH_P, 0) from M_PICAL_BREATH_RESULT order by timestamp desc", count);
           
                
             dt = db_sugar.Ado.GetDataTable(str);
            return dt;
        }
    
 
  
     
      

        public bool Set_Admin_add(M_PICAL_BREATH_RESULT model)
        {
            bool listmodel = true;
            try
            {
                db_sugar.Insertable<M_PICAL_BREATH_RESULT>(model).ExecuteCommand();
            }
            catch
            {
                listmodel = false;
            }


            //Type t = model.GetType();
            //PropertyInfo[] PropertyList = t.GetProperties();
            //List<string> Property = new List<string>();
            //List<object> PropertyValue = new List<object>();

            //foreach (PropertyInfo item in PropertyList)
            //{
            //    if (item.Name != "TIMESTAMP")
            //    {
            //        Property.Add(item.Name);
            //        object value = item.GetValue(model, null);
            //        PropertyValue.Add(value);
            //    }

            //}

            //string sql = "insert into M_PICAL_BREATH_RESULT(";
            //foreach (var itvar in Property)
            //{
            //    sql += itvar + ",";
            //}
            //sql += "TIMESTAMP) Values(";
            //foreach (var itvar in PropertyValue)
            //{
            //    sql += itvar + ",";
            //}
            //sql += "'" + DateTime.Now + "')";
            //try
            //{
            //    iDataBase.ExecuteSQL1(sql);
            //}
            //catch
            //{
            //    listmodel = false;
            //}
            return listmodel;
        }
        public bool Set_Admin_addT2(M_PICAL_BREATH_RESULT_T2 model)
        {
            bool listmodel = true;
            try
            {
                db_sugar.Insertable<M_PICAL_BREATH_RESULT_T2>(model).ExecuteCommand();
            }
            catch
            {
                listmodel = false;
            }

            return listmodel;
        }

        public DataTable Get_M_PICAL_BREATH_RESULT(int a)
        {
            string str_sql = string.Format(@"select * from  (select  isnull(PICAL_JPU, 0),
       isnull(PICAL_Q, 0),
       isnull(PICAL_P, 0),
       isnull(PICAL_BREATH_SRM_BILL, 0),
       isnull(PICAL_BREATH_COKE_BILL, 0),
       isnull(PICAL_BREATH_SPARE12, 0),
       isnull(PICAL_BREATH_SPARE2, 0),
       isnull(PICAL_BREATH_SPARE3, 0),
       isnull(PICAL_BREATH_SPARE4, 0),
       isnull(PICAL_BREATH_2M_NEX_WAT, 0),
       isnull(PICAL_H, 0),
       isnull(PICAL_BREATH_IG_03_TE, 0),
       isnull(PICAL_BREATH_SPARE13, 0),
       isnull(PICAL_BREATH_SPARE17, 0)
  from M_PICAL_BREATH_RESULT
 order by TIMESTAMP desc)
 where  rownum <'" + a + "'");
            return db_sugar.Ado.GetDataTable(str_sql);
           // return iDataBase.GetDataTable(str_sql);
        }
        public int Config_Sys_Value()
        {
            string str_sql = string.Format(@"select VALUE from CONFIG_SYS_VALUE where id=3");
            return db_sugar.Ado.GetInt(str_sql);
        }
        public double[] BEISHAO_JICHUSHUJU(int M_ROW)
        {
            DataTable dt = Get_TOP_M_PICAL_BREATH_RESULT(M_ROW);
            double[] result = new double[13];
            string log = "";
            try
            {
                if (dt.Rows.Count >= 14)
                {
               
                RELEVANCE_SZ_l = new double[dt.Rows.Count, 14];
                double[,] RELEVANCE_SZ_2 = new double[dt.Rows.Count, 14];
                if (dt.Rows.Count > 1)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int i1 = 0; i1 < 14; i1++)
                        {
                            RELEVANCE_SZ_l[i, i1] = Convert.ToDouble(dt.Rows[i][i1].ToString());
                        }
                    }
                }
                List<double[]> list = new List<double[]>();
                for (int i = 0; i < 14; i++)
                {
                    double[] D = new double[dt.Rows.Count];
                    for (int i1 = 0; i1 < dt.Rows.Count; i1++)
                    {
                        D[i1] = RELEVANCE_SZ_l[i1, i];
                    }
                    list.Add(D);
                }
                for (int i = 0; i < 14; i++)
                {
                    for (int i1 = 0; i1 < 14; i1++)
                    {
                        RELEVANCE_SZ_2[i, i1] = nanInfinity(Correlation.Spearman(list[i], list[i1]));
                    }
                }
                    for (int i=0;i<13;i++)
                    {
                        result[i]= RELEVANCE_SZ_2[i+1, 0];
                    }
                
              }
            }
            catch (Exception ee)
            {
                log = "";
            }
            return result;
        }
        public void SetResultValues()
        {
            M_PICAL_BREATH_RESULT M_PICAL_BREATH_RESULT_ = null;
            M_PICAL_BREATH_RESULT_ = new M_PICAL_BREATH_RESULT();
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_BF = nanInfinity(Cor_Result[0]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_FLUX = nanInfinity(Cor_Result[1]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_SIN = nanInfinity(Cor_Result[2]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_R = nanInfinity(Cor_Result[3]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_MGO = nanInfinity(Cor_Result[4]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_COKE_GRI = nanInfinity(Cor_Result[5]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_WAT = nanInfinity(Cor_Result[6]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_THICK = nanInfinity(Cor_Result[7]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_IG_TE = nanInfinity(Cor_Result[8]);
            M_PICAL_BREATH_RESULT_.PICAL_BREATH_SPARE11 = nanInfinity(Cor_Result[9]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_BLEND = nanInfinity(Cor_Result[10]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_MA_TE = nanInfinity(Cor_Result[11]);
            M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_FLUE_PT = nanInfinity(Cor_Result[12]);
            if (Set_update(M_PICAL_BREATH_RESULT_))
            {
                string log = "更新完成";
                
                LogHelper.LogInfo(log + "M_PICAL_BREATH_RESULT");
            }
            else
            {
                string log = "更新失败";
              
                LogHelper.LogError(log + "M_PICAL_BREATH_RESULT");
            }
          
           

        }
        public double nanInfinity(double num)
        {
            if (double.IsNaN(num))
            {
                num = 0;
            }
            if (double.IsInfinity(num))
            {
                num = 0;
            }
            return num;
        }

        public bool Set_update(M_PICAL_BREATH_RESULT M_PICAL_BREATH_RESULT_)
        {
            string log = "";
            bool listmodel = false;
            try
            {
                string str = "select max(timestamp) from M_PICAL_BREATH_RESULT";
                DateTime db = db_sugar.Ado.GetDateTime(str);
                var updateObj = new M_PICAL_BREATH_RESULT()
                {
                    PICAL_RELAT_BR_BL_BF = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_BF,
                    PICAL_RELAT_BR_BL_FLUX = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_FLUX,
                    PICAL_RELAT_BR_BL_SIN = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_SIN,
                    PICAL_RELAT_BR_R = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_R,
                    PICAL_RELAT_BR_BL_MGO = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_MGO,
                    PICAL_RELAT_BR_COKE_GRI = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_COKE_GRI,
                    PICAL_RELAT_BR_WAT = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_WAT,
                    PICAL_RELAT_BR_THICK = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_THICK,
                    PICAL_RELAT_BR_BL_IG_TE = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_IG_TE,
                    PICAL_BREATH_SPARE11 = M_PICAL_BREATH_RESULT_.PICAL_BREATH_SPARE11,
                    PICAL_RELAT_BR_BL_BLEND = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_BLEND,
                     PICAL_RELAT_BR_MA_TE = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_MA_TE,
                    //PICAL_RELAT_BR_MA_TE = 88,
                    PICAL_RELAT_BR_FLUE_PT = M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_FLUE_PT
                };
                //var t=db_sugar.Updateable(updateObj).Where(it=>it.TIMESTAMP== db).ExecuteCommand();

                var t = db_sugar.Updateable(updateObj).UpdateColumns(
                    "PICAL_RELAT_BR_BL_BF",
                    "PICAL_RELAT_BR_BL_FLUX",
                    "PICAL_RELAT_BR_BL_SIN",
                    "PICAL_RELAT_BR_R",
                    "PICAL_RELAT_BR_BL_MGO",
                    "PICAL_RELAT_BR_COKE_GRI",
                    "PICAL_RELAT_BR_WAT",
                    "PICAL_RELAT_BR_THICK",
                    "PICAL_RELAT_BR_BL_IG_TE",
                    "PICAL_BREATH_SPARE11",
                    "PICAL_RELAT_BR_BL_BLEND",
                    "PICAL_RELAT_BR_MA_TE",
                    "PICAL_RELAT_BR_FLUE_PT"
                ).Where(it => it.TIMESTAMP == db).ExecuteCommand();

                //string sql = "";
                //    sql = string.Format(@"update M_PICAL_BREATH_RESULT set PICAL_RELAT_BR_BL_BF =" + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_BF + ""
                //    + ",PICAL_RELAT_BR_BL_FLUX = " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_FLUX + ""
                //    + ",PICAL_RELAT_BR_BL_SIN= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_SIN + ""
                //    + ",PICAL_RELAT_BR_R= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_R + ""
                //    + ",PICAL_RELAT_BR_BL_MGO= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_MGO + ""
                //    + ",PICAL_RELAT_BR_COKE_GRI= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_COKE_GRI + ""
                //    + ",PICAL_RELAT_BR_WAT= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_WAT + ""
                //    + ",PICAL_RELAT_BR_THICK= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_THICK + ""
                //    + ",PICAL_RELAT_BR_BL_IG_TE= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_IG_TE + ""
                //    + ",PICAL_BREATH_SPARE11= " + M_PICAL_BREATH_RESULT_.PICAL_BREATH_SPARE11 + ""
                //    + ",PICAL_RELAT_BR_MA_TE= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_MA_TE + " "
                //     + ",PICAL_RELAT_BR_BL_BLEND= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_BL_BLEND + ""
                //    + ",PICAL_RELAT_BR_FLUE_PT= " + M_PICAL_BREATH_RESULT_.PICAL_RELAT_BR_FLUE_PT + " "
                //     + "where timestamp = (select max(timestamp) from M_PICAL_BREATH_RESULT)");
                //// iDataBase.ExecuteSQL1(string.Format(sql));

                //db_sugar.Updateable<M_PICAL_BREATH_RESULT>(sql).ExecuteCommand();;

                listmodel = true;
                    return listmodel;
                
            }
            catch(Exception ee)
            {
                LogHelper.LogError("更新M_PICAL_BREATH_RESULT 失败");
                return listmodel;
            }
        }

        public bool Set_MD_PHY_PARTICLE_INFO_IN()
        {
            string log = "";
            bool listmodel = false;
            try
            {
                
                    //string sql = "";
                    //sql = "update MD_PHY_PARTICLE_INFO_IN set FLAG_GRIT =0";
                    //iDataBase.ExecuteSQL1(string.Format(sql));
             
                    //log = string.Format("更新标志为完成！");
                   
                    listmodel = true;
                    return listmodel;
               
            }
            catch
            {
                return listmodel;
            }
        }

       


    }
    public class MC_SINCAL_result_1min
    {
        /// <summary>
        /// 混合料碱度(%)
        /// </summary>
        public double SINCAL_SIN_PV_R { get; set; }
        /// <summary>
        /// 混合料MgO(%)
        /// </summary>
        public double SINCAL_SIN_PV_MGO { get; set; }
        /// <summary>
        /// 混合料含碳(%)
        /// </summary>
        public double SINCAL_MIX_PV_C { get; set; }


    }


}