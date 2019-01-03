namespace DOTNET_PLAY
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Bplay = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Tip = new System.Windows.Forms.TextBox();
            this.Tport = new System.Windows.Forms.TextBox();
            this.CBmainstream = new System.Windows.Forms.CheckBox();
            this.Bstart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Bplay
            // 
            this.Bplay.Location = new System.Drawing.Point(520, 497);
            this.Bplay.Name = "Bplay";
            this.Bplay.Size = new System.Drawing.Size(75, 25);
            this.Bplay.TabIndex = 0;
            this.Bplay.Text = "Comenzar";
            this.Bplay.UseVisualStyleBackColor = true;
            this.Bplay.Click += new System.EventHandler(this.Bplay_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(645, 476);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // BLogin
            // 
            this.BLogin.Location = new System.Drawing.Point(434, 496);
            this.BLogin.Name = "BLogin";
            this.BLogin.Size = new System.Drawing.Size(75, 25);
            this.BLogin.TabIndex = 2;
            this.BLogin.Text = "Login";
            this.BLogin.UseVisualStyleBackColor = true;
            this.BLogin.Click += new System.EventHandler(this.BLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 509);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 508);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "puerto";
            // 
            // Tip
            // 
            this.Tip.Location = new System.Drawing.Point(61, 503);
            this.Tip.Name = "Tip";
            this.Tip.Size = new System.Drawing.Size(124, 20);
            this.Tip.TabIndex = 4;
            this.Tip.Text = "192.168.0.253";
            // 
            // Tport
            // 
            this.Tport.Location = new System.Drawing.Point(258, 503);
            this.Tport.Name = "Tport";
            this.Tport.Size = new System.Drawing.Size(67, 20);
            this.Tport.TabIndex = 4;
            this.Tport.Text = "8091";
            // 
            // CBmainstream
            // 
            this.CBmainstream.AutoSize = true;
            this.CBmainstream.Location = new System.Drawing.Point(368, 504);
            this.CBmainstream.Name = "CBmainstream";
            this.CBmainstream.Size = new System.Drawing.Size(62, 17);
            this.CBmainstream.TabIndex = 5;
            this.CBmainstream.Text = "主码流";
            this.CBmainstream.UseVisualStyleBackColor = true;
            // 
            // Bstart
            // 
            this.Bstart.Location = new System.Drawing.Point(602, 496);
            this.Bstart.Name = "Bstart";
            this.Bstart.Size = new System.Drawing.Size(75, 25);
            this.Bstart.TabIndex = 6;
            this.Bstart.Text = "启动485";
            this.Bstart.UseVisualStyleBackColor = true;
            this.Bstart.Click += new System.EventHandler(this.Bstart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 536);
            this.Controls.Add(this.Bstart);
            this.Controls.Add(this.CBmainstream);
            this.Controls.Add(this.Tport);
            this.Controls.Add(this.Tip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BLogin);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Bplay);
            this.Name = "Form1";
            this.Text = "Play video demo in C#";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Bplay;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button BLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Tip;
        private System.Windows.Forms.TextBox Tport;
        private System.Windows.Forms.CheckBox CBmainstream;
        private System.Windows.Forms.Button Bstart;
    }
}

