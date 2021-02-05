namespace UserControlIndex
{
    partial class PipeLine_Text
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
            this.pipeLine1 = new UserControlIndex.PipeLine();
            this.pipeLine2 = new UserControlIndex.PipeLine();
            this.pipeLine3 = new UserControlIndex.PipeLine();
            this.SuspendLayout();
            // 
            // pipeLine1
            // 
            this.pipeLine1.Location = new System.Drawing.Point(36, 38);
            this.pipeLine1.MoveSpeed = 2.5F;
            this.pipeLine1.Name = "pipeLine1";
            this.pipeLine1.Size = new System.Drawing.Size(335, 15);
            this.pipeLine1.TabIndex = 0;
            // 
            // pipeLine2
            // 
            this.pipeLine2.Location = new System.Drawing.Point(-14, 103);
            this.pipeLine2.MoveSpeed = 2.5F;
            this.pipeLine2.Name = "pipeLine2";
            this.pipeLine2.Size = new System.Drawing.Size(335, 15);
            this.pipeLine2.TabIndex = 1;
            // 
            // pipeLine3
            // 
            this.pipeLine3.Location = new System.Drawing.Point(-64, 168);
            this.pipeLine3.MoveSpeed = 2.5F;
            this.pipeLine3.Name = "pipeLine3";
            this.pipeLine3.Size = new System.Drawing.Size(335, 15);
            this.pipeLine3.TabIndex = 2;
            // 
            // PipeLine_Text
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pipeLine3);
            this.Controls.Add(this.pipeLine2);
            this.Controls.Add(this.pipeLine1);
            this.Name = "PipeLine_Text";
            this.Size = new System.Drawing.Size(177, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private PipeLine pipeLine1;
        private PipeLine pipeLine2;
        private PipeLine pipeLine3;
    }
}
