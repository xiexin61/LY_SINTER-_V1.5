namespace UserControlIndex
{
    partial class PipeLine_Right
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.pipeLine2 = new UserControlIndex.PipeLine();
            this.pipeLine1 = new UserControlIndex.PipeLine();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // pipeLine2
            // 
            this.pipeLine2.Location = new System.Drawing.Point(254, 3);
            this.pipeLine2.MoveSpeed = 2.5F;
            this.pipeLine2.Name = "pipeLine2";
            this.pipeLine2.PipeLineStyle = UserControlIndex.PipeLineStyle.Vertical;
            this.pipeLine2.PipeTurnRight = UserControlIndex.PipeTurnDirection.Left;
            this.pipeLine2.Size = new System.Drawing.Size(15, 102);
            this.pipeLine2.TabIndex = 1;
            // 
            // pipeLine1
            // 
            this.pipeLine1.Location = new System.Drawing.Point(116, 90);
            this.pipeLine1.MoveSpeed = 2.5F;
            this.pipeLine1.Name = "pipeLine1";
            this.pipeLine1.Size = new System.Drawing.Size(138, 15);
            this.pipeLine1.TabIndex = 0;
            // 
            // PipeLine_Right
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pipeLine2);
            this.Controls.Add(this.pipeLine1);
            this.Name = "PipeLine_Right";
            this.Size = new System.Drawing.Size(272, 162);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PipeLine pipeLine1;
        private PipeLine pipeLine2;
        private System.Windows.Forms.Label label1;
    }
}
