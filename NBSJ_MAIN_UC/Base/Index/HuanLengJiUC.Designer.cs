namespace UserControlIndex
{
    partial class HuanLengJiUC
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
            this.fanUC1 = new UserControlIndex.FanUC();
            this.fanUC2 = new UserControlIndex.FanUC();
            this.fanUC3 = new UserControlIndex.FanUC();
            this.fanUC4 = new UserControlIndex.FanUC();
            //this.fanUC5 = new UserControlIndex.FanUC();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // fanUC1
            // 
            this.fanUC1.Location = new System.Drawing.Point(2, 50);
            this.fanUC1.Name = "fanUC1";
            this.fanUC1.Size = new System.Drawing.Size(93, 110);
            this.fanUC1.TabIndex = 0;
            // 
            // fanUC2
            // 
            this.fanUC2.Location = new System.Drawing.Point(91, 50);
            this.fanUC2.Name = "fanUC2";
            this.fanUC2.NumValue = 2;
            this.fanUC2.Size = new System.Drawing.Size(93, 110);
            this.fanUC2.TabIndex = 1;
            // 
            // fanUC3
            // 
            this.fanUC3.Location = new System.Drawing.Point(190, 50);
            this.fanUC3.Name = "fanUC3";
            this.fanUC3.NumValue = 3;
            this.fanUC3.Size = new System.Drawing.Size(93, 110);
            this.fanUC3.TabIndex = 2;
            // 
            // fanUC4
            // 
            this.fanUC4.Location = new System.Drawing.Point(289, 50);
            this.fanUC4.Name = "fanUC4";
            this.fanUC4.NumValue = 4;
            this.fanUC4.Size = new System.Drawing.Size(93, 110);
            this.fanUC4.TabIndex = 3;
            // 
            // fanUC5
            // 
            /*this.fanUC5.Location = new System.Drawing.Point(375, 50);
            this.fanUC5.Name = "fanUC5";
            this.fanUC5.NumValue = 5;
            this.fanUC5.Size = new System.Drawing.Size(93, 110);
            this.fanUC5.TabIndex = 4;*/
            // 
            // radioButton1
            // 
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.Checked = true;
            this.radioButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButton1.Location = new System.Drawing.Point(226, 31);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(13, 13);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = false;
            this.radioButton1.Click += new System.EventHandler(this.rbtnDouble_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.BackColor = System.Drawing.Color.Transparent;
            this.radioButton2.Checked = true;
            this.radioButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButton2.Location = new System.Drawing.Point(289, 31);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(13, 13);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.UseVisualStyleBackColor = false;
            this.radioButton2.Click += new System.EventHandler(this.rbtnDouble_Click);
            // 
            // HuanLengJiUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            //this.Controls.Add(this.fanUC5);
            this.Controls.Add(this.fanUC4);
            this.Controls.Add(this.fanUC3);
            this.Controls.Add(this.fanUC2);
            this.Controls.Add(this.fanUC1);
            this.Name = "HuanLengJiUC";
            this.Size = new System.Drawing.Size(472, 183);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.HuanLengJiUC_MouseDoubleClick);
            this.ResumeLayout(false);

        }

        #endregion

        private FanUC fanUC1;
        private FanUC fanUC2;
        private FanUC fanUC3;
        private FanUC fanUC4;
        //private FanUC fanUC5;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}
