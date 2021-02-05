using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SqlSugar;
using NBSJ_MAIN_UC.Model;
using NBSJ_MAIN_UC;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Data.SqlClient;


namespace UserControlIndex
{
    public partial class FormDaoTui : XtraForm
    {
        SqlSugarClient db_sugar = GetInstance();
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
        public  List<CONFIG_GRIDVIEWCOLS> GetGritData(string viewname)
        {
            List<CONFIG_GRIDVIEWCOLS> listmodel = new List<CONFIG_GRIDVIEWCOLS>();
            string sql = "select * from CONFIG_GRIDVIEWCOLS where NAME='" + viewname + "'";
            try
            {
                listmodel = db_sugar.SqlQueryable<CONFIG_GRIDVIEWCOLS>(sql).ToList();

            }
            catch (Exception ee)
            {
                LogHelper.LogError(ee.Message);
            }
            if (listmodel.Count > 0)
            {
                listmodel = listmodel.OrderBy(i=>i.COLUMNINDEX).ToList();
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
           
                string strSQL = "select top(1) *  from MC_MICAL_RESULT where DATANUM="+ DATANUM+" order by timestamp desc";
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
            GenerateGridColumns(gridView3, columnsList,DATANUM);

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
        public  void GenerateGridColumns(GridView gridView, List<CONFIG_GRIDVIEWCOLS> list,int datanum, bool isUpper = true)
        {
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
                        column.ColumnEdit = BindingLoopUpEdit(gridView.GridControl, GetList<ItemClass>(list[i].DESC_EN, list[i].FORMATSTRING, datanum));

                    }
                    else if (list[i].FORMATSTRING=="datetime")
                    {
                        column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;  // 列数据样式
                        column.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";   // 列格式样式  事件 yyyy-mm-dd HH:mm:ss
                    }
                   // column.ColumnEdit = BindingLoopUpEdit(gridView.GridControl, GetList<ItemClass>(list[i].FORMATSTRING_FLAG));


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

        }
        static RepositoryItemLookUpEdit BindingLoopUpEdit(DevExpress.XtraGrid.GridControl GridControl1, string jsonString, bool isString = false)
        {
            return BindingLoopUpEdit(GridControl1, jsonString);
        }
        static RepositoryItemLookUpEdit BindingLoopUpEdit(DevExpress.XtraGrid.GridControl GridControl1, List<ItemClass> list, bool isString = false)
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
               _MC_MICAL_RESULT= db_sugar.SqlQueryable<MC_MICAL_RESULT>(sql).ToList();
            return _MC_MICAL_RESULT;
        }
        public  List<T> GetList<T>(string data,string format,int datanum) where T:class, new()
        {
            string sql = "";
            if (format == "float")
            {
                 sql = "select top(1) isnull(" + data + ",0) AS text,isnull(" + data + " ,0) AS Value from mc_mical_result where DATANUM="+ datanum + " order by timestamp desc";

            }
            else if(format=="datetime")
            {
                sql = "select top(1) isnull(" + data + ",0) AS text,isnull(" + data + ",0)  AS Keys,isnull(" + data + ",0)  as Tag from mc_mical_result where DATANUM=" + datanum + " order by timestamp desc";
            }
            List<T> R = db_sugar.SqlQueryable<T>(sql).ToList();
          
           // List<T> R = db_sugar.Ado.SqlQuery<T>(sql);
            


            return R;
           

        }

      
    }

}
