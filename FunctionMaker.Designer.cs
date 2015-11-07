namespace GrippingTest
{
    partial class FunctionMaker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxExp = new System.Windows.Forms.TextBox();
            this.textBoxRangeStart = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxRangeEnd = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelTestResult = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTestInput = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "f(t)=";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "t";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // textBoxExp
            // 
            this.textBoxExp.Location = new System.Drawing.Point(84, 16);
            this.textBoxExp.Multiline = true;
            this.textBoxExp.Name = "textBoxExp";
            this.textBoxExp.Size = new System.Drawing.Size(188, 142);
            this.textBoxExp.TabIndex = 1;
            // 
            // textBoxRangeStart
            // 
            this.textBoxRangeStart.Location = new System.Drawing.Point(84, 180);
            this.textBoxRangeStart.Name = "textBoxRangeStart";
            this.textBoxRangeStart.Size = new System.Drawing.Size(70, 21);
            this.textBoxRangeStart.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(167, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "-";
            this.label3.Click += new System.EventHandler(this.label2_Click);
            // 
            // textBoxRangeEnd
            // 
            this.textBoxRangeEnd.Location = new System.Drawing.Point(189, 180);
            this.textBoxRangeEnd.Name = "textBoxRangeEnd";
            this.textBoxRangeEnd.Size = new System.Drawing.Size(83, 21);
            this.textBoxRangeEnd.TabIndex = 2;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(200, 393);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(187, 21);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(47, 26);
            this.buttonTest.TabIndex = 4;
            this.buttonTest.Text = "test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelTestResult);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxTestInput);
            this.groupBox1.Controls.Add(this.buttonTest);
            this.groupBox1.Location = new System.Drawing.Point(17, 284);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 103);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "测试效果";
            // 
            // labelTestResult
            // 
            this.labelTestResult.AutoSize = true;
            this.labelTestResult.Location = new System.Drawing.Point(79, 65);
            this.labelTestResult.Name = "labelTestResult";
            this.labelTestResult.Size = new System.Drawing.Size(11, 12);
            this.labelTestResult.TabIndex = 7;
            this.labelTestResult.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "output";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "input";
            // 
            // textBoxTestInput
            // 
            this.textBoxTestInput.Location = new System.Drawing.Point(79, 25);
            this.textBoxTestInput.Name = "textBoxTestInput";
            this.textBoxTestInput.Size = new System.Drawing.Size(102, 21);
            this.textBoxTestInput.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 12);
            this.label6.TabIndex = 0;
            this.label6.Click += new System.EventHandler(this.label2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 240);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "color:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(84, 240);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(188, 21);
            this.textBox1.TabIndex = 7;
            // 
            // FunctionMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 424);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxRangeEnd);
            this.Controls.Add(this.textBoxRangeStart);
            this.Controls.Add(this.textBoxExp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FunctionMaker";
            this.Text = "FunctionMaker";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxExp;
        private System.Windows.Forms.TextBox textBoxRangeStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxRangeEnd;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelTestResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTestInput;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
    }
}