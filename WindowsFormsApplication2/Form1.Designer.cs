namespace WindowsFormsApplication2
{
    partial class Form1
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
            this.glControl1 = new OpenTK.GLControl();
            this.button1 = new System.Windows.Forms.Button();
            this.waveFormBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timeBox = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.FFTBox = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.currentWaveFormBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.waveFormBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FFTBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentWaveFormBox)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(-1, 0);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(752, 251);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.GLLoad);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.GlPaint);
            this.glControl1.Resize += new System.EventHandler(this.GLResize);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(666, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.StartVisualization);
            // 
            // waveFormBox
            // 
            this.waveFormBox.Enabled = false;
            this.waveFormBox.Location = new System.Drawing.Point(-1, 94);
            this.waveFormBox.Name = "waveFormBox";
            this.waveFormBox.Size = new System.Drawing.Size(148, 140);
            this.waveFormBox.TabIndex = 2;
            this.waveFormBox.TabStop = false;
            this.waveFormBox.Paint += new System.Windows.Forms.PaintEventHandler(this.WaveFormBoxPaint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "CurrentTime";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(6, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "TotalTime";
            // 
            // timeBox
            // 
            this.timeBox.Enabled = false;
            this.timeBox.Location = new System.Drawing.Point(290, 286);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(936, 28);
            this.timeBox.TabIndex = 6;
            this.timeBox.TabStop = false;
            this.timeBox.Paint += new System.Windows.Forms.PaintEventHandler(this.TimeBoxPaint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(1226, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(109, 84);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(39, 59);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 59);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 9;
            // 
            // FFTBox
            // 
            this.FFTBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FFTBox.Location = new System.Drawing.Point(290, 271);
            this.FFTBox.Name = "FFTBox";
            this.FFTBox.Size = new System.Drawing.Size(461, 133);
            this.FFTBox.TabIndex = 8;
            this.FFTBox.TabStop = false;
            this.FFTBox.Paint += new System.Windows.Forms.PaintEventHandler(this.FFTBoxPaint);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(0, 9);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(125, 138);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Controls.Add(this.richTextBox1);
            this.groupBox2.Location = new System.Drawing.Point(-1, 257);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 147);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.Location = new System.Drawing.Point(224, 36);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown2.TabIndex = 18;
            this.numericUpDown2.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.TransparencyChange);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(131, 114);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(87, 20);
            this.textBox6.TabIndex = 17;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(131, 88);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(87, 20);
            this.textBox5.TabIndex = 16;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(131, 62);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(87, 20);
            this.textBox4.TabIndex = 15;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(131, 36);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(87, 20);
            this.textBox3.TabIndex = 14;
            this.textBox3.Text = "Transperancy";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(131, 10);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(87, 20);
            this.textBox2.TabIndex = 13;
            this.textBox2.Text = "Intensity";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(224, 10);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown1.TabIndex = 12;
            this.numericUpDown1.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // currentWaveFormBox
            // 
            this.currentWaveFormBox.Enabled = false;
            this.currentWaveFormBox.Location = new System.Drawing.Point(456, 12);
            this.currentWaveFormBox.Name = "currentWaveFormBox";
            this.currentWaveFormBox.Size = new System.Drawing.Size(285, 117);
            this.currentWaveFormBox.TabIndex = 10;
            this.currentWaveFormBox.TabStop = false;
            this.currentWaveFormBox.Paint += new System.Windows.Forms.PaintEventHandler(this.currentWaveFormBoxPaint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 401);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.currentWaveFormBox);
            this.Controls.Add(this.FFTBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.waveFormBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.waveFormBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FFTBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentWaveFormBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox waveFormBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox timeBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox FFTBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox currentWaveFormBox;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
    }
}

