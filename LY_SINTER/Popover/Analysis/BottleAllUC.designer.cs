namespace UserControlIndex
{
    partial class BottleAllUC
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
            this.pipeLine3 = new UserControlIndex.PipeLine();
            this.pipeLine4 = new UserControlIndex.PipeLine();
            this.SuspendLayout();
            // 
            // pipeLine3
            // 
            this.pipeLine3.Location = new System.Drawing.Point(23, 30);
            this.pipeLine3.MoveSpeed = 2.5F;
            this.pipeLine3.Name = "pipeLine3";
            this.pipeLine3.PipeTurnRight = UserControlIndex.PipeTurnDirection.Down;
            this.pipeLine3.Size = new System.Drawing.Size(229, 15);
            this.pipeLine3.TabIndex = 2;
            // 
            // pipeLine4
            // 
            this.pipeLine4.Location = new System.Drawing.Point(256, 44);
            this.pipeLine4.MoveSpeed = 2.5F;
            this.pipeLine4.Name = "pipeLine4";
            this.pipeLine4.PipeLineStyle = UserControlIndex.PipeLineStyle.Vertical;
            this.pipeLine4.Size = new System.Drawing.Size(15, 49);
            this.pipeLine4.TabIndex = 1;
            // 
            // BottleAllUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.Controls.Add(this.pipeLine3);
            //this.Controls.Add(this.pipeLine4);
            this.Name = "BottleAllUC";
            this.Size = new System.Drawing.Size(290, 112);
            this.ResumeLayout(false);

        }

        #endregion

        private PipeLine pipeLine4;
        private PipeLine pipeLine3;


    }
}
