using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using NBSJ_MAIN_UC;
using NBSJ_MAIN_UC.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace UserControlIndex
{
    public partial class FormDaoTui : XtraForm
    {
        private SqlSugarClient db_sugar = GetInstance();

        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = ADODB.ConnectionString, DbType = SqlSugar.DbType.SqlServer, IsAutoCloseConnection = true });
            return db;
        }

        public FormDaoTui()
        {
            InitializeComponent();
        }

        private void FormDaoTui_Load(object sender, EventArgs e)
        {
            //Init(14);
        }

        public List<CONFIG_GRIDVIEWCOLS> GetGritData(string viewname)
        {
            List<CONFIG_GRIDVIEWCOLS> listmodel = new List<CONFIG_GRIDVIEWCOLS>();
            string sql = "select * from CONFIG_GRIDVIEWCOLS where NAME='" + viewname + "'";
            try
            {
                listmodel = db_sugar.SqlQueryable<CONFIG_GRIDVIEWCOLS>(sql).ToList();

                //var xx = listmodel.Where(c => c.NAME.Contains("MV"));

                //LINQ lambda
                /*var nums = Enumerable.Range(1, 1000);
                var odds = nums.Where(n => n % 2 == 0).ToList();*/
            }
            catch (Exception ee)
            {
                LogHelper.LogError(ee.Message);
            }
            if (listmodel.Count > 0)
            {
                listmodel = listmodel.OrderBy(i => i.COLUMNINDEX).ToList();
            }
            //List<CONFIG_GRIDVIEWCOLS> listmodel = db_sugar.Queryable<CONFIG_GRIDVIEWCOLS>().Where(m => m.NAME == viewname).OrderBy("COLUMNINDEX").ToList();
            return listmodel;
        }

        private void showInit(int DATANUM)
        {
            groupControl6.Visible = false;
            if (DATANUM == 1)//一混
            {
                groupControl1.Visible = false;
                groupControl4.Visible = false;
                groupControl5.Visible = false;
                groupControl6.Visible = false;
                groupControl7.Visible = false;
                groupControl8.Visible = false;
                groupControl9.Visible = false;

                this.Height = 80 * 2 + 50;
            }
            if (DATANUM == 2)//二混
            {
                groupControl1.Visible = false;
                groupControl5.Visible = false;
                groupControl6.Visible = false;
                groupControl7.Visible = false;
                groupControl8.Visible = false;
                groupControl9.Visible = false;
                this.Height = 80 * 3 + 50;
            }
            if (DATANUM == 3)//布料 点火
            {
                groupControl1.Visible = false;
                groupControl7.Visible = false;
                groupControl8.Visible = false;
                groupControl9.Visible = false;
                this.Height = 80 * 4 + 50;
            }
            //if (DATANUM == 9)//烧结
            //{
            //    groupControl1.Visible = false;
            //    groupControl8.Visible = false;
            //    groupControl9.Visible = false;
            //}
            if (DATANUM >= 4 && DATANUM <= 10)//烧结机
            {
                groupControl1.Visible = false;
                groupControl8.Visible = false;
                groupControl9.Visible = false;

                //if (DATANUM != 10)
                //{
                //    gridView7.Columns[gridView7.Columns.Count - 1].Caption = string.Format("1-{0}#风箱耗时", (DATANUM - 3) * 3);
                //}
                this.Height = 80 * 5 + 50;
            }
            if (DATANUM == 12 || DATANUM == 11)//环冷机
            {
                groupControl1.Visible = false;
                groupControl9.Visible = false;
                this.Height = 80 * 6 + 30;
            }
            if (DATANUM == 13)//筛分
            {
                groupControl1.Visible = false;
                groupControl9.Visible = true;
                this.Height = 80 * 7 + 30;
            }
            if (DATANUM == 14)//取样点
            {
                groupControl9.Visible = true;
                this.Height = 80 * 8 + 30;
            }
        }

        public void Init(int DATANUM)
        {
            string desc = "";
            switch (DATANUM)
            {
                case 1:
                    desc = "一混";
                    break;

                case 2:
                    desc = "二混";
                    break;

                case 3:
                    desc = "布料";
                    break;

                case 4:
                    desc = "风箱1 - 3#";
                    break;

                case 5:
                    desc = "风箱4 - 6#";
                    break;

                case 6:
                    desc = "风箱7 - 9#";
                    break;

                case 7:
                    desc = "风箱10 - 14#";
                    break;

                case 8:
                    desc = "风箱15 - 17#";
                    break;

                case 9:
                    desc = "风箱18 - 20#";
                    break;

                case 10:
                    desc = "风箱21 - 22#";
                    break;

                case 11:
                    desc = "环冷入口";
                    break;

                case 12:
                    desc = "环冷出口";
                    break;

                case 13:
                    desc = "取样点1";
                    break;

                case 14:
                    desc = "取样点2";
                    break;
            }

            this.Text = string.Format("{0}({1})", this.Text, desc);
            List<MC_MICAL_RESULT> Lmmr = new List<MC_MICAL_RESULT>();

            string strSQL = "select top(1) *  from MC_MICAL_RESULT where DATANUM=" + DATANUM + " order by timestamp desc";
            try
            {
                Lmmr = db_sugar.SqlQueryable<MC_MICAL_RESULT>(strSQL).ToList();
            }
            catch (Exception ee)
            {
                LogHelper.LogError(ee.Message);
            }

            List<CONFIG_GRIDVIEWCOLS> columnsList = GetGritData("MICAL");
            //  GenerateGridColumns(gridView1, columnsList);

            //gridControl1.DataSource = Lmmr;
            //gridControl1.RefreshDataSource();

            var lists = new List<string>();
            lists.Add("MICAL0");
            lists.Add("MICAL1");
            lists.Add("MICAL2");
            lists.Add("MICAL3");
            lists.Add("MICAL4");
            lists.Add("MICAL5");
            lists.Add("MICAL6");
            lists.Add("MICAL7");
            lists.Add("MICAL8");

            var gvs = new Dictionary<int, GridView>();
            gvs.Add(0, gridView2);
            gvs.Add(1, gridView3);
            gvs.Add(2, gridView4);
            gvs.Add(3, gridView5);
            gvs.Add(4, gridView6);
            gvs.Add(5, gridView7);
            gvs.Add(6, gridView8);
            gvs.Add(7, gridView9);
            gvs.Add(8, gridView1);

            var gcs = new Dictionary<int, DevExpress.XtraGrid.GridControl>();
            gcs.Add(0, gridControl2);
            gcs.Add(1, gridControl3);
            gcs.Add(2, gridControl4);
            gcs.Add(3, gridControl5);
            gcs.Add(4, gridControl6);
            gcs.Add(5, gridControl7);
            gcs.Add(6, gridControl8);
            gcs.Add(7, gridControl9);
            gcs.Add(8, gridControl1);

            var keys = gcs.Keys;

            var xx = Parallel.ForEach(lists, c =>
              {
                  var idxStr = c.Substring("MICAL".Length);
                  if (int.TryParse(idxStr, out int idx))
                  {
                      var columnsListx = GetGritData(c);
                      GenerateGridColumns(gvs[idx], columnsListx, DATANUM);
                      gcs[idx].DataSource = Lmmr;
                      gcs[idx].RefreshDataSource();//配比信息
                  }
              });

            if (xx.IsCompleted)
            {
                showInit(DATANUM);
            }
        }

        public void GenerateGridColumns(GridView gridView, List<CONFIG_GRIDVIEWCOLS> list, int datanum, bool isUpper = true)
        {
            var sw = new Stopwatch();
            sw.Start();
            gridView.Columns.Clear();
            gridView.SortInfo.Clear();
            int ColumnCount = list.Count;
            int groupNum = 0;
            if (ColumnCount > 0)
            {
                GridColumn column = null;
                for (int i = 0; i < ColumnCount; i++)
                {
                    column = new GridColumn();
                    column.FieldName = isUpper ? list[i].FEILDNAME.ToUpper().Trim() : list[i].FEILDNAME;
                    column.Caption = list[i].DESC_FLAG.Trim();
                    if (list[i].FORMATSTRING == "float")
                    {
                        column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.None;

                        //Lee@20210321:不知道有什么用，但相当耗时，故注释
                        //var sw2 = new Stopwatch();
                        //sw2.Start();
                        column.ColumnEdit = BindingLoopUpEdit(gridView.GridControl, GetList<ItemClass>(list[i].DESC_EN, list[i].FORMATSTRING, datanum));

                        //sw2.Stop();
                        //Trace.WriteLine($"list loop {i}: { sw2.ElapsedMilliseconds}");
                        //Lee@20210321：End
                    }
                    else if (list[i].FORMATSTRING == "datetime")
                    {
                        column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;  // 列数据样式
                        column.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";   // 列格式样式  事件 yyyy-mm-dd HH:mm:ss
                    }
                    //column.ColumnEdit = BindingLoopUpEdit(gridView.GridControl, GetList<ItemClass>(list[i].FORMATSTRING_FLAG));

                    if (list[i].ISFIXEDWIDTH) // 是都固定宽度
                    {
                        column.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                    }
                    if (Convert.ToInt32(list[i].FIXEDWIDTH) > 0)
                    {
                        column.OptionsColumn.FixedWidth = true;   // 固定宽度
                        column.Width = Convert.ToInt32(list[i].FIXEDWIDTH);  // 列宽
                    }
                    if (list[i].VISABLE)
                    {
                        column.Visible = true;
                        column.VisibleIndex = i;
                    }
                    else
                    {
                        column.Visible = false;
                    }
                    column.OptionsFilter.AllowFilter = true;
                    column.OptionsColumn.AllowEdit = false;
                    column.OptionsColumn.AllowMove = false;
                    column.OptionsColumn.AllowShowHide = false;
                    column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    column.OptionsColumn.AllowSize = true;
                    // column.Name = list[i].fileName;

                    column.AppearanceHeader.Options.UseTextOptions = true;
                    column.AppearanceCell.Options.UseTextOptions = true;
                    column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    //分组列
                    if (list[i].IsGroupColumn == 1)
                    {
                        groupNum++;
                        gridView.SortInfo.Add(new DevExpress.XtraGrid.Columns.GridColumnSortInfo(column, DevExpress.Data.ColumnSortOrder.Ascending));
                    }

                    gridView.Columns.Add(column);
                }

                gridView.GroupCount = groupNum;
                gridView.OptionsBehavior.Editable = false;
                gridView.OptionsView.ShowGroupPanel = false;
                gridView.OptionsView.ShowIndicator = false;
                gridView.BestFitColumns();
            }
            sw.Stop();

            Trace.WriteLine($"{gridView.Name} takes {sw.ElapsedMilliseconds}ms");
        }

        public void Init_Ori(int DATANUM)
        {
            string desc = "";
            switch (DATANUM)
            {
                case 1:
                    desc = "一混";
                    break;

                case 2:
                    desc = "二混";
                    break;

                case 3:
                    desc = "布料";
                    break;

                case 4:
                    desc = "风箱1 - 3#";
                    break;

                case 5:
                    desc = "风箱4 - 6#";
                    break;

                case 6:
                    desc = "风箱7 - 9#";
                    break;

                case 7:
                    desc = "风箱10 - 14#";
                    break;

                case 8:
                    desc = "风箱15 - 17#";
                    break;

                case 9:
                    desc = "风箱18 - 20#";
                    break;

                case 10:
                    desc = "风箱21 - 22#";
                    break;

                case 11:
                    desc = "环冷入口";
                    break;

                case 12:
                    desc = "环冷出口";
                    break;

                case 13:
                    desc = "取样点1";
                    break;

                case 14:
                    desc = "取样点2";
                    break;
            }

            this.Text = string.Format("{0}({1})", this.Text, desc);
            List<MC_MICAL_RESULT> Lmmr = new List<MC_MICAL_RESULT>();

            string strSQL = "select top(1) *  from MC_MICAL_RESULT where DATANUM=" + DATANUM + " order by timestamp desc";
            try
            {
                Lmmr = db_sugar.SqlQueryable<MC_MICAL_RESULT>(strSQL).ToList();
            }
            catch (Exception ee)
            {
                LogHelper.LogError(ee.Message);
            }

            List<CONFIG_GRIDVIEWCOLS> columnsList = GetGritData("MICAL");
            //  GenerateGridColumns(gridView1, columnsList);

            //gridControl1.DataSource = Lmmr;
            //gridControl1.RefreshDataSource();

            columnsList = GetGritData("MICAL0");
            GenerateGridColumns(gridView2, columnsList, DATANUM);

            gridControl2.DataSource = Lmmr;
            gridControl2.RefreshDataSource();//配比信息

            columnsList = GetGritData("MICAL1");
            GenerateGridColumns(gridView3, columnsList, DATANUM);

            gridControl3.DataSource = Lmmr;
            gridControl3.RefreshDataSource();//一混

            columnsList = GetGritData("MICAL2");
            GenerateGridColumns(gridView4, columnsList, DATANUM);

            gridControl4.DataSource = Lmmr;
            gridControl4.RefreshDataSource();//二混

            columnsList = GetGritData("MICAL3");
            GenerateGridColumns(gridView5, columnsList, DATANUM);

            gridControl5.DataSource = Lmmr;
            gridControl5.RefreshDataSource();//布料信息

            columnsList = GetGritData("MICAL4");
            GenerateGridColumns(gridView6, columnsList, DATANUM);

            gridControl6.DataSource = Lmmr;
            gridControl6.RefreshDataSource();//点火信息

            columnsList = GetGritData("MICAL5");
            GenerateGridColumns(gridView7, columnsList, DATANUM);

            gridControl7.DataSource = Lmmr;
            gridControl7.RefreshDataSource();//烧结

            columnsList = GetGritData("MICAL6");
            GenerateGridColumns(gridView8, columnsList, DATANUM);

            gridControl8.DataSource = Lmmr;
            gridControl8.RefreshDataSource();//环冷信息

            columnsList = GetGritData("MICAL7");
            GenerateGridColumns(gridView9, columnsList, DATANUM);
            gridControl9.DataSource = Lmmr;
            gridControl9.RefreshDataSource();//信息

            columnsList = GetGritData("MICAL8");
            GenerateGridColumns(gridView1, columnsList, DATANUM);

            gridControl1.DataSource = Lmmr;
            gridControl1.RefreshDataSource();//筛分

            showInit(DATANUM);
        }

        private static RepositoryItemLookUpEdit BindingLoopUpEdit(DevExpress.XtraGrid.GridControl GridControl1, string jsonString, bool isString = false)
        {
            return BindingLoopUpEdit(GridControl1, jsonString);
        }

        /// <summary>
        /// 暂不清楚什么效果
        /// </summary>
        /// <param name="GridControl1"></param>
        /// <param name="list"></param>
        /// <param name="isString"></param>
        /// <returns></returns>
        private static RepositoryItemLookUpEdit BindingLoopUpEdit(DevExpress.XtraGrid.GridControl GridControl1, List<ItemClass> list, bool isString = false)
        {
            var itemLoopUpEdit = new RepositoryItemLookUpEdit();
            GridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            itemLoopUpEdit});
            itemLoopUpEdit.AutoHeight = false;
            itemLoopUpEdit.Name = Guid.NewGuid().ToString();
            itemLoopUpEdit.DisplayMember = "Text";
            itemLoopUpEdit.NullText = "";
            if (isString)
            {
                itemLoopUpEdit.ValueMember = "Keys";
            }
            else
            {
                itemLoopUpEdit.ValueMember = "Value";
            }
            itemLoopUpEdit.DataSource = list;
            itemLoopUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Text", " ")});
            return itemLoopUpEdit;
        }

        public List<MC_MICAL_RESULT> GetList(string sql)
        {
            List<MC_MICAL_RESULT> _MC_MICAL_RESULT = new List<MC_MICAL_RESULT>();
            _MC_MICAL_RESULT = db_sugar.SqlQueryable<MC_MICAL_RESULT>(sql).ToList();
            return _MC_MICAL_RESULT;
        }

        public List<T> GetList<T>(string data, string format, int datanum) where T : class, new()
        {
            /*var sw2 = new Stopwatch();
            sw2.Start();*/

            string sql = "";
            if (format == "float")
            {
                sql = "select top(1) isnull(" + data + ",0) AS text,isnull(" + data + " ,0) AS Value from mc_mical_result where DATANUM=" + datanum + " order by timestamp desc";
            }
            else if (format == "datetime")
            {
                sql = "select top(1) isnull(" + data + ",0) AS text,isnull(" + data + ",0)  AS Keys,isnull(" + data + ",0)  as Tag from mc_mical_result where DATANUM=" + datanum + " order by timestamp desc";
            }
            List<T> R = db_sugar.SqlQueryable<T>(sql).ToList();

            /*sw2.Stop();
            Trace.WriteLine($"GetList: { sw2.ElapsedMilliseconds}");*/
            // List<T> R = db_sugar.Ado.SqlQuery<T>(sql);

            return R;
        }
    }
}