namespace CentralControl
{
    partial class MicroReactorForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.currentCmdTextBox = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SampleTimetxtb = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TailCarbontxtb = new System.Windows.Forms.TextBox();
            this.TailOxygentxtb = new System.Windows.Forms.TextBox();
            this.Pressuretxtb = new System.Windows.Forms.TextBox();
            this.Flowtxtb = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Rspeedtxtb = new System.Windows.Forms.TextBox();
            this.DOtxtb = new System.Windows.Forms.TextBox();
            this.Temperaturetxtb = new System.Windows.Forms.TextBox();
            this.PHtxtb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StartBtn = new System.Windows.Forms.Button();
            this.StopBtn = new System.Windows.Forms.Button();
            this.SetBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "模块选择";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "模块1",
            "模块2",
            "模块3",
            "模块4",
            "模块5",
            "模块6",
            "模块7",
            "模块8"});
            this.comboBox1.Location = new System.Drawing.Point(72, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 22;
            this.comboBox1.Text = "模块1";
            this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_textChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.currentCmdTextBox);
            this.groupBox1.Controls.Add(this.label36);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Location = new System.Drawing.Point(27, 432);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(596, 46);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "命令";
            // 
            // currentCmdTextBox
            // 
            this.currentCmdTextBox.Enabled = false;
            this.currentCmdTextBox.Location = new System.Drawing.Point(512, 17);
            this.currentCmdTextBox.Name = "currentCmdTextBox";
            this.currentCmdTextBox.ReadOnly = true;
            this.currentCmdTextBox.Size = new System.Drawing.Size(70, 21);
            this.currentCmdTextBox.TabIndex = 31;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(449, 20);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(47, 12);
            this.label36.TabIndex = 30;
            this.label36.Text = "command";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(366, 17);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 3;
            this.button10.Text = "自动";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(254, 17);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 2;
            this.button9.Text = "急停";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(134, 17);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 1;
            this.button8.Text = "开始";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(27, 17);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 0;
            this.button7.Text = "复位";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Location = new System.Drawing.Point(27, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(748, 378);
            this.panel1.TabIndex = 27;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SetBtn);
            this.groupBox3.Controls.Add(this.StopBtn);
            this.groupBox3.Controls.Add(this.StartBtn);
            this.groupBox3.Controls.Add(this.SampleTimetxtb);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.TailCarbontxtb);
            this.groupBox3.Controls.Add(this.TailOxygentxtb);
            this.groupBox3.Controls.Add(this.Pressuretxtb);
            this.groupBox3.Controls.Add(this.Flowtxtb);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.Rspeedtxtb);
            this.groupBox3.Controls.Add(this.DOtxtb);
            this.groupBox3.Controls.Add(this.Temperaturetxtb);
            this.groupBox3.Controls.Add(this.PHtxtb);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(15, 51);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(677, 304);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "模块属性";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // SampleTimetxtb
            // 
            this.SampleTimetxtb.Location = new System.Drawing.Point(78, 245);
            this.SampleTimetxtb.Name = "SampleTimetxtb";
            this.SampleTimetxtb.Size = new System.Drawing.Size(100, 21);
            this.SampleTimetxtb.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 248);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 16;
            this.label10.Text = "采样时间：";
            // 
            // TailCarbontxtb
            // 
            this.TailCarbontxtb.Location = new System.Drawing.Point(296, 177);
            this.TailCarbontxtb.Name = "TailCarbontxtb";
            this.TailCarbontxtb.ReadOnly = true;
            this.TailCarbontxtb.Size = new System.Drawing.Size(100, 21);
            this.TailCarbontxtb.TabIndex = 15;
            // 
            // TailOxygentxtb
            // 
            this.TailOxygentxtb.Location = new System.Drawing.Point(296, 129);
            this.TailOxygentxtb.Name = "TailOxygentxtb";
            this.TailOxygentxtb.ReadOnly = true;
            this.TailOxygentxtb.Size = new System.Drawing.Size(100, 21);
            this.TailOxygentxtb.TabIndex = 14;
            // 
            // Pressuretxtb
            // 
            this.Pressuretxtb.Location = new System.Drawing.Point(296, 34);
            this.Pressuretxtb.Name = "Pressuretxtb";
            this.Pressuretxtb.Size = new System.Drawing.Size(100, 21);
            this.Pressuretxtb.TabIndex = 13;
            // 
            // Flowtxtb
            // 
            this.Flowtxtb.Location = new System.Drawing.Point(78, 177);
            this.Flowtxtb.Name = "Flowtxtb";
            this.Flowtxtb.Size = new System.Drawing.Size(100, 21);
            this.Flowtxtb.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(228, 180);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 11;
            this.label9.Text = "尾气C02";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(225, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "尾气O2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(228, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "压力";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "流量";
            // 
            // Rspeedtxtb
            // 
            this.Rspeedtxtb.Location = new System.Drawing.Point(78, 129);
            this.Rspeedtxtb.Name = "Rspeedtxtb";
            this.Rspeedtxtb.Size = new System.Drawing.Size(100, 21);
            this.Rspeedtxtb.TabIndex = 7;
            // 
            // DOtxtb
            // 
            this.DOtxtb.Location = new System.Drawing.Point(296, 81);
            this.DOtxtb.Name = "DOtxtb";
            this.DOtxtb.ReadOnly = true;
            this.DOtxtb.Size = new System.Drawing.Size(100, 21);
            this.DOtxtb.TabIndex = 6;
            // 
            // Temperaturetxtb
            // 
            this.Temperaturetxtb.Location = new System.Drawing.Point(78, 81);
            this.Temperaturetxtb.Name = "Temperaturetxtb";
            this.Temperaturetxtb.Size = new System.Drawing.Size(100, 21);
            this.Temperaturetxtb.TabIndex = 5;
            // 
            // PHtxtb
            // 
            this.PHtxtb.Location = new System.Drawing.Point(78, 34);
            this.PHtxtb.Name = "PHtxtb";
            this.PHtxtb.Size = new System.Drawing.Size(100, 21);
            this.PHtxtb.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "搅拌转速";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(222, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "溶氧(DO)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "温度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "PH值";
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(477, 238);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(46, 46);
            this.StartBtn.TabIndex = 18;
            this.StartBtn.Text = "开始";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // StopBtn
            // 
            this.StopBtn.Location = new System.Drawing.Point(566, 238);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(46, 46);
            this.StopBtn.TabIndex = 19;
            this.StopBtn.Text = "停止";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // SetBtn
            // 
            this.SetBtn.Location = new System.Drawing.Point(307, 250);
            this.SetBtn.Name = "SetBtn";
            this.SetBtn.Size = new System.Drawing.Size(75, 23);
            this.SetBtn.TabIndex = 20;
            this.SetBtn.Text = "设定";
            this.SetBtn.UseVisualStyleBackColor = true;
            // 
            // MicroReactorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 490);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "MicroReactorForm";
            this.Text = "MicroReactorForm";
            this.Load += new System.EventHandler(this.MicroReactorForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox currentCmdTextBox;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox SampleTimetxtb;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TailCarbontxtb;
        private System.Windows.Forms.TextBox TailOxygentxtb;
        private System.Windows.Forms.TextBox Pressuretxtb;
        private System.Windows.Forms.TextBox Flowtxtb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Rspeedtxtb;
        private System.Windows.Forms.TextBox DOtxtb;
        private System.Windows.Forms.TextBox Temperaturetxtb;
        private System.Windows.Forms.TextBox PHtxtb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SetBtn;
        private System.Windows.Forms.Button StopBtn;
        private System.Windows.Forms.Button StartBtn;
    }
}