namespace GrippingTest
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
            this.pbTotal = new System.Windows.Forms.PictureBox();
            this.pbIndex = new System.Windows.Forms.PictureBox();
            this.pbMiddle = new System.Windows.Forms.PictureBox();
            this.pbRing = new System.Windows.Forms.PictureBox();
            this.pbLittle = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.comboBoxPorts = new System.Windows.Forms.ComboBox();
            this.label33 = new System.Windows.Forms.Label();
            this.buttonSerialSettings = new System.Windows.Forms.Button();
            this.gb1 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLittle)).BeginInit();
            this.gb1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbTotal
            // 
            this.pbTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTotal.Location = new System.Drawing.Point(12, 12);
            this.pbTotal.Name = "pbTotal";
            this.pbTotal.Size = new System.Drawing.Size(1024, 300);
            this.pbTotal.TabIndex = 0;
            this.pbTotal.TabStop = false;
            // 
            // pbIndex
            // 
            this.pbIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbIndex.Location = new System.Drawing.Point(13, 320);
            this.pbIndex.Name = "pbIndex";
            this.pbIndex.Size = new System.Drawing.Size(240, 200);
            this.pbIndex.TabIndex = 1;
            this.pbIndex.TabStop = false;
            // 
            // pbMiddle
            // 
            this.pbMiddle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMiddle.Location = new System.Drawing.Point(268, 318);
            this.pbMiddle.Name = "pbMiddle";
            this.pbMiddle.Size = new System.Drawing.Size(240, 200);
            this.pbMiddle.TabIndex = 1;
            this.pbMiddle.TabStop = false;
            // 
            // pbRing
            // 
            this.pbRing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRing.Location = new System.Drawing.Point(528, 318);
            this.pbRing.Name = "pbRing";
            this.pbRing.Size = new System.Drawing.Size(240, 200);
            this.pbRing.TabIndex = 1;
            this.pbRing.TabStop = false;
            // 
            // pbLittle
            // 
            this.pbLittle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbLittle.Location = new System.Drawing.Point(796, 318);
            this.pbLittle.Name = "pbLittle";
            this.pbLittle.Size = new System.Drawing.Size(240, 200);
            this.pbLittle.TabIndex = 1;
            this.pbLittle.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "用户名:";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(65, 26);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(171, 21);
            this.textBoxUserName.TabIndex = 5;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(8, 64);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(106, 23);
            this.buttonStart.TabIndex = 6;
            this.buttonStart.Text = "开始";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // comboBoxPorts
            // 
            this.comboBoxPorts.FormattingEnabled = true;
            this.comboBoxPorts.Location = new System.Drawing.Point(65, 14);
            this.comboBoxPorts.Name = "comboBoxPorts";
            this.comboBoxPorts.Size = new System.Drawing.Size(88, 20);
            this.comboBoxPorts.TabIndex = 3;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(6, 17);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(53, 12);
            this.label33.TabIndex = 2;
            this.label33.Text = "选择串口";
            // 
            // buttonSerialSettings
            // 
            this.buttonSerialSettings.Location = new System.Drawing.Point(161, 12);
            this.buttonSerialSettings.Name = "buttonSerialSettings";
            this.buttonSerialSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSerialSettings.TabIndex = 7;
            this.buttonSerialSettings.Text = "串口设置";
            this.buttonSerialSettings.UseVisualStyleBackColor = true;
            this.buttonSerialSettings.Click += new System.EventHandler(this.buttonSerialSettings_Click);
            // 
            // gb1
            // 
            this.gb1.Controls.Add(this.buttonSerialSettings);
            this.gb1.Controls.Add(this.label33);
            this.gb1.Controls.Add(this.comboBoxPorts);
            this.gb1.Location = new System.Drawing.Point(1053, 11);
            this.gb1.Name = "gb1";
            this.gb1.Size = new System.Drawing.Size(246, 48);
            this.gb1.TabIndex = 7;
            this.gb1.TabStop = false;
            this.gb1.Text = "串口";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxUserName);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Location = new System.Drawing.Point(1053, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 100);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "测试";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(1053, 176);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 341);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "函数";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(164, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "添加";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "载入方案";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(86, 20);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "保存方案";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1329, 534);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gb1);
            this.Controls.Add(this.pbLittle);
            this.Controls.Add(this.pbRing);
            this.Controls.Add(this.pbMiddle);
            this.Controls.Add(this.pbIndex);
            this.Controls.Add(this.pbTotal);
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLittle)).EndInit();
            this.gb1.ResumeLayout(false);
            this.gb1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbTotal;
        private System.Windows.Forms.PictureBox pbIndex;
        private System.Windows.Forms.PictureBox pbMiddle;
        private System.Windows.Forms.PictureBox pbRing;
        private System.Windows.Forms.PictureBox pbLittle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ComboBox comboBoxPorts;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button buttonSerialSettings;
        private System.Windows.Forms.GroupBox gb1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

