namespace UserControlIndex
{
    partial class PipeLine_Two
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
            this.pipeLine2 = new UserControlIndex.PipeLine();
            this.pipeLine1 = new UserControlIndex.PipeLine();
            this.pipeLine3 = new UserControlIndex.PipeLine();
            //this.pipeLine4 = new UserControlIndex.PipeLine();
            this.SuspendLayout();
            // 
            // pipeLine2
            // 
            this.pipeLine2.Location = new System.Drawing.Point(30, 17);
            this.pipeLine2.MoveSpeed = 2.5F;
            this.pipeLine2.Name = "pipeLine2";
            this.pipeLine2.Size = new System.Drawing.Size(291, 15);
            this.pipeLine2.TabIndex = 1;
            // 
            // pipeLine1
            // 
            this.pipeLine1.Location = new System.Drawing.Point(187, 6);
            this.pipeLine1.MoveSpeed = 2.5F;
            this.pipeLine1.Name = "pipeLine1";
            this.pipeLine1.Size = new System.Drawing.Size(191, 15);
            this.pipeLine1.TabIndex = 0;
            // 
            // pipeLine3
            // 
            this.pipeLine3.Location = new System.Drawing.Point(46, 35);
            this.pipeLine3.MoveSpeed = 2.5F;
            this.pipeLine3.Name = "pipeLine3";
            this.pipeLine3.Size = new System.Drawing.Size(291, 15);
            this.pipeLine3.TabIndex = 2;
            // 
            // pipeLine4
            // 
            /*this.pipeLine4.Location = new System.Drawing.Point(4, 64);
            this.pipeLine4.MoveSpeed = 2.5F;
            this.pipeLine4.Name = "pipeLine4";
            this.pipeLine4.Size = new System.Drawing.Size(291, 15);
            this.pipeLine4.TabIndex = 3;*/
            // 
            // PipeLine_Two
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.Controls.Add(this.pipeLine4);
            this.Controls.Add(this.pipeLine3);
            this.Controls.Add(this.pipeLine2);
            this.Controls.Add(this.pipeLine1);
            this.Name = "PipeLine_Two";
            this.Size = new System.Drawing.Size(383, 84);
            this.ResumeLayout(false);

        }

        #endregion

        private PipeLine pipeLine1;
        private PipeLine pipeLine2;
        private PipeLine pipeLine3;
        //private PipeLine pipeLine4;
    }
}
