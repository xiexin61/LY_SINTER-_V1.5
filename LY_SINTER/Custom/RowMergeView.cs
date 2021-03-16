using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

/// <summary>
/// DataGridView行合并.请对属性MergeColumnNames 赋值既可
/// </summary>
public partial class RowMergeView : DataGridView
{
    #region 构造函数

    public RowMergeView()
    {
        MergeByColumnIndex = -1;
        MergeByColumnName = string.Empty;
        _mergeColumnHeaderBgColor = SystemColors.Control;
        InitializeComponent();
    }

    #endregion 构造函数

    #region 重写的事件

    protected override void OnPaint(PaintEventArgs pe)
    {
        // TODO: 在此处添加自定义绘制代码

        // 调用基类 OnPaint
        base.OnPaint(pe);
    }

    protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
    {
        try
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                DrawCell(e);
            }
            else
            {
                //二维表头
                if (e.RowIndex == -1)
                {
                    DrawHeader(e);
                }
            }
            base.OnCellPainting(e);
        }
        catch
        { }
    }

    protected override void OnCellClick(DataGridViewCellEventArgs e)
    {
        base.OnCellClick(e);
    }

    #endregion 重写的事件

    #region 自定义方法

    /// <summary>
    /// 画列头
    /// </summary>
    /// <param name="e"></param>
    private void DrawHeader(DataGridViewCellPaintingEventArgs e)
    {
        if (_spanRows.ContainsKey(e.ColumnIndex)) //被合并的列
        {
            //画边框
            var g = e.Graphics;
            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            int left = e.CellBounds.Left, top = e.CellBounds.Top + 2;
            int right = e.CellBounds.Right, bottom = e.CellBounds.Bottom;

            switch (_spanRows[e.ColumnIndex].Position)
            {
                case 1:
                    left += 2;
                    break;

                case 2:
                    break;

                case 3:
                    right -= 2;
                    break;
            }

            //画上半部分底色
            g.FillRectangle(new SolidBrush(_mergeColumnHeaderBgColor), left, top, right - left, (bottom - top) / 2);

            //画中线
            g.DrawLine(new Pen(GridColor), left, (top + bottom) / 2, right, (top + bottom) / 2);

            //写小标题
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(e.Value + "", e.CellStyle.Font, Brushes.Black, new Rectangle(left, (top + bottom) / 2, right - left, (bottom - top) / 2), sf);

            left = GetColumnDisplayRectangle(_spanRows[e.ColumnIndex].Left, true).Left - 2;
            if (left < 0)
            {
                left = GetCellDisplayRectangle(-1, -1, true).Width;
            }

            right = GetColumnDisplayRectangle(_spanRows[e.ColumnIndex].Right, true).Right - 2;
            if (right < 0)
            {
                right = Width;
            }

            g.DrawString(_spanRows[e.ColumnIndex].Text, e.CellStyle.Font, Brushes.Black, new Rectangle(left, top, right - left, (bottom - top) / 2), sf);
            e.Handled = true;
        }
    }

    /// <summary>
    /// 画单元格
    /// </summary>
    /// <param name="e"></param>
    private void DrawCell(DataGridViewCellPaintingEventArgs e)
    {
        if (e.CellStyle.Alignment == DataGridViewContentAlignment.NotSet)
        {
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        //Lee@20210106
        if (MergeByColumnIndex < 0 && string.IsNullOrWhiteSpace(MergeByColumnName))
        {
            return;
        }

        if (MergeColumnNames.Contains(Columns[e.ColumnIndex].Name) && e.RowIndex != -1)
        {
            var gridBrush = new SolidBrush(GridColor);
            var backBrush = new SolidBrush(e.CellStyle.BackColor);
            var fontBrush = new SolidBrush(e.CellStyle.ForeColor);

            var upRows = 0;// 上面相同的行数
            var downRows = 0;// 下面相同的行数
            var count = 0;// 总行数

            var cellWidth = e.CellBounds.Width;
            var gridLinePen = new Pen(gridBrush);
            var curValue = e.Value == null ? "" : e.Value.ToString().Trim();
            if (!string.IsNullOrEmpty(curValue))
            {
                #region 获取下面的行数
                for (var i = e.RowIndex; i < Rows.Count; i++)
                {
                    //Lee@20210106
                    if (((MergeByColumnIndex > 0 && Rows[i].Cells[MergeByColumnIndex].Value.Equals(Rows[e.RowIndex].Cells[MergeByColumnIndex].Value)) || (MergeByColumnName.Length > 0 && Rows[i].Cells[MergeByColumnName].Value.Equals(Rows[e.RowIndex].Cells[MergeByColumnName].Value)))
                        && Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                    {
                        downRows++;
                        if (e.RowIndex != i)
                        {
                            cellWidth = cellWidth < Rows[i].Cells[e.ColumnIndex].Size.Width ? cellWidth : Rows[i].Cells[e.ColumnIndex].Size.Width;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                #endregion 获取下面的行数

                #region 获取上面的行数
                for (var i = e.RowIndex; i >= 0; i--)
                {
                    //Lee@20210106
                    if (((MergeByColumnIndex > 0 && Rows[i].Cells[MergeByColumnIndex].Value.Equals(Rows[e.RowIndex].Cells[MergeByColumnIndex].Value)) || (MergeByColumnName.Length > 0 && Rows[i].Cells[MergeByColumnName].Value.Equals(Rows[e.RowIndex].Cells[MergeByColumnName].Value)))
                        && Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                    {
                        //this.Rows[i].Cells[e.ColumnIndex].Selected = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected;
                        upRows++;
                        if (e.RowIndex != i)
                        {
                            cellWidth = cellWidth < Rows[i].Cells[e.ColumnIndex].Size.Width ? cellWidth : Rows[i].Cells[e.ColumnIndex].Size.Width;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                #endregion 获取上面的行数

                count = downRows + upRows - 1;
                if (count < 2)
                {
                    return;
                }
            }
            if (Rows[e.RowIndex].Selected)
            {
                backBrush.Color = e.CellStyle.SelectionBackColor;
                fontBrush.Color = e.CellStyle.SelectionForeColor;
            }
            //以背景色填充
            e.Graphics.FillRectangle(backBrush, e.CellBounds);
            //画字符串
            PaintingFont(e, cellWidth, upRows, downRows, count);
            if (downRows == 1)
            {
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                count = 0;
            }
            // 画右边线
            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

            e.Handled = true;
        }
    }

    /// <summary>
    /// 画字符串
    /// </summary>
    /// <param name="e"></param>
    /// <param name="cellWidth"></param>
    /// <param name="upRows"></param>
    /// <param name="downRows"></param>
    /// <param name="count"></param>
    private void PaintingFont(DataGridViewCellPaintingEventArgs e, int cellWidth, int upRows, int downRows, int count)
    {
        var fontBrush = new SolidBrush(e.CellStyle.ForeColor);
        var fontHeight = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Height;
        var fontWidth = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Width;
        var cellHeight = e.CellBounds.Height;

        switch (e.CellStyle.Alignment)
        {
            case DataGridViewContentAlignment.BottomCenter:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellWidth - fontWidth) / 2, e.CellBounds.Y + cellHeight * downRows - fontHeight);
                break;

            case DataGridViewContentAlignment.BottomLeft:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y + cellHeight * downRows - fontHeight);
                break;

            case DataGridViewContentAlignment.BottomRight:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X + cellWidth - fontWidth, e.CellBounds.Y + cellHeight * downRows - fontHeight);
                break;

            case DataGridViewContentAlignment.MiddleCenter:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellWidth - fontWidth) / 2, e.CellBounds.Y - cellHeight * (upRows - 1) + (cellHeight * count - fontHeight) / 2);
                break;

            case DataGridViewContentAlignment.MiddleLeft:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellHeight * (upRows - 1) + (cellHeight * count - fontHeight) / 2);
                break;

            case DataGridViewContentAlignment.MiddleRight:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X + cellWidth - fontWidth, e.CellBounds.Y - cellHeight * (upRows - 1) + (cellHeight * count - fontHeight) / 2);
                break;

            case DataGridViewContentAlignment.TopCenter:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellWidth - fontWidth) / 2, e.CellBounds.Y - cellHeight * (upRows - 1));
                break;

            case DataGridViewContentAlignment.TopLeft:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellHeight * (upRows - 1));
                break;

            case DataGridViewContentAlignment.TopRight:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X + cellWidth - fontWidth, e.CellBounds.Y - cellHeight * (upRows - 1));
                break;

            default:
                e.Graphics.DrawString((e.Value.ToString()), e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellWidth - fontWidth) / 2, e.CellBounds.Y - cellHeight * (upRows - 1) + (cellHeight * count - fontHeight) / 2);
                break;
        }
    }

    #endregion 自定义方法

    #region 属性

    /// <summary>
    /// 设置或获取合并列的集合
    /// </summary>
    [MergableProperty(false)]
    [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Localizable(true)]
    [Description("设置或获取合并列的集合"), Browsable(true), Category("单元格合并")]
    public List<string> MergeColumnNames { get; set; } = new List<string>();

    #region 即根据哪一列的值来合并上下单元格 Lee@20210106

    /// <summary>
    /// 合并单元格所依据值的列下标，即根据哪一列的值来合并上下单元格
    /// </summary>
    public int MergeByColumnIndex { get; set; }

    /// <summary>
    /// 合并单元格所依据值的列名称，即根据哪一列的值来合并上下单元格
    /// </summary>
    public string MergeByColumnName { get; set; }

    #endregion 即根据哪一列的值来合并上下单元格 Lee@20210106

    #endregion 属性

    #region 二维表头

    private struct SpanInfo //表头信息
    {
        public SpanInfo(string text, int position, int left, int right)
        {
            Text = text;
            Position = position;
            Left = left;
            Right = right;
        }

        public string Text; //列主标题
        public int Position; //位置，1:左，2中，3右
        public int Left; //对应左行
        public int Right; //对应右行
    }

    /// <summary>
    /// 二维表头的背景颜色
    /// </summary>
    [Description("二维表头的背景颜色"), Browsable(true), Category("二维表头")]
    private readonly Color _mergeColumnHeaderBgColor;

    private readonly Dictionary<int, SpanInfo> _spanRows = new Dictionary<int, SpanInfo>();//需要2维表头的列

    #region Events

    private void DataGridViewEx_Scroll(object sender, ScrollEventArgs e)
    {
        if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)// && e.Type == ScrollEventType.EndScroll)
        {
            timer1.Enabled = false; timer1.Enabled = true;
        }
    }

    private void Timer1_Tick(object sender, EventArgs e)
    {
        timer1.Enabled = false;
        ReDrawHead();
    }

    #endregion Events

    //刷新显示表头
    public void ReDrawHead()
    {
        foreach (var si in _spanRows.Keys)
        {
            Invalidate(GetCellDisplayRectangle(si, -1, true));
        }
    }

    /// <summary>
    /// 合并列
    /// </summary>
    /// <param name="colIndex">列的索引</param>
    /// <param name="colCount">需要合并的列数</param>
    /// <param name="text">合并列后的文本</param>
    public void AddSpanHeader(int colIndex, int colCount, string text)
    {
        /*if (colCount < 2)
        {
            throw new Exception("行宽应大于等于2，合并1列无意义。");
        }*/
        //将这些列加入列表
        var right = colIndex + colCount - 1; //同一大标题下的最后一列的索引
        _spanRows[colIndex] = new SpanInfo(text, 1, colIndex, right); //添加标题下的最左列
        _spanRows[right] = new SpanInfo(text, 3, colIndex, right); //添加该标题下的最右列
        for (var i = colIndex + 1; i < right; i++) //中间的列
        {
            _spanRows[i] = new SpanInfo(text, 2, colIndex, right);
        }
    }

    /// <summary>
    /// 清除合并的列
    /// </summary>
    public void ClearSpanInfo()
    {
        _spanRows.Clear();
        //ReDrawHead();
    }

    #endregion 二维表头

    private void timer1_Tick_1(object sender, EventArgs e)
    {

    }
}