namespace Electrical_Weathering
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.Source = new System.Windows.Forms.TextBox();
            this.Select = new System.Windows.Forms.Button();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.Compressing = new System.Windows.Forms.TrackBar();
            this.WeatheringValue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lable2 = new System.Windows.Forms.Label();
            this.Greening = new System.Windows.Forms.TrackBar();
            this.GreeningValue = new System.Windows.Forms.Label();
            this.Generate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Noise = new System.Windows.Forms.TrackBar();
            this.noiseValue = new System.Windows.Forms.Label();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.zooming = new System.Windows.Forms.TrackBar();
            this.zoomingValue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Default_L = new System.Windows.Forms.RadioButton();
            this.Default_M = new System.Windows.Forms.RadioButton();
            this.Default_H = new System.Windows.Forms.RadioButton();
            this.Customized = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Compressing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Greening)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Noise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zooming)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(36, 46);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(500, 500);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            // 
            // Source
            // 
            this.Source.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Source.Location = new System.Drawing.Point(614, 49);
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            this.Source.Size = new System.Drawing.Size(305, 30);
            this.Source.TabIndex = 1;
            this.Source.TextChanged += new System.EventHandler(this.Source_TextChanged);
            // 
            // Select
            // 
            this.Select.Location = new System.Drawing.Point(925, 49);
            this.Select.Name = "Select";
            this.Select.Size = new System.Drawing.Size(94, 30);
            this.Select.TabIndex = 2;
            this.Select.Text = "选择";
            this.Select.UseVisualStyleBackColor = true;
            this.Select.Click += new System.EventHandler(this.Select_Click);
            // 
            // openFile
            // 
            this.openFile.FileName = "openFile";
            this.openFile.Filter = "\"All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff\"";
            // 
            // Compressing
            // 
            this.Compressing.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.Compressing.Enabled = false;
            this.Compressing.Location = new System.Drawing.Point(666, 233);
            this.Compressing.Maximum = 100;
            this.Compressing.Name = "Compressing";
            this.Compressing.Size = new System.Drawing.Size(328, 56);
            this.Compressing.TabIndex = 3;
            this.Compressing.TickFrequency = 20;
            this.Compressing.Scroll += new System.EventHandler(this.Weathering_Scroll);
            this.Compressing.ValueChanged += new System.EventHandler(this.Compressing_ValueChanged);
            // 
            // WeatheringValue
            // 
            this.WeatheringValue.AutoSize = true;
            this.WeatheringValue.Location = new System.Drawing.Point(1000, 233);
            this.WeatheringValue.Name = "WeatheringValue";
            this.WeatheringValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.WeatheringValue.Size = new System.Drawing.Size(41, 25);
            this.WeatheringValue.TabIndex = 4;
            this.WeatheringValue.Text = "0%";
            this.WeatheringValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(609, 233);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "压缩";
            // 
            // lable2
            // 
            this.lable2.AutoSize = true;
            this.lable2.Location = new System.Drawing.Point(609, 171);
            this.lable2.Name = "lable2";
            this.lable2.Size = new System.Drawing.Size(52, 25);
            this.lable2.TabIndex = 5;
            this.lable2.Text = "绿化";
            // 
            // Greening
            // 
            this.Greening.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.Greening.Enabled = false;
            this.Greening.Location = new System.Drawing.Point(666, 171);
            this.Greening.Maximum = 100;
            this.Greening.Name = "Greening";
            this.Greening.Size = new System.Drawing.Size(328, 56);
            this.Greening.TabIndex = 3;
            this.Greening.TickFrequency = 20;
            this.Greening.Scroll += new System.EventHandler(this.Greening_Scroll);
            this.Greening.ValueChanged += new System.EventHandler(this.Greening_ValueChanged);
            // 
            // GreeningValue
            // 
            this.GreeningValue.AutoSize = true;
            this.GreeningValue.Location = new System.Drawing.Point(1000, 171);
            this.GreeningValue.Name = "GreeningValue";
            this.GreeningValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.GreeningValue.Size = new System.Drawing.Size(41, 25);
            this.GreeningValue.TabIndex = 4;
            this.GreeningValue.Text = "0%";
            this.GreeningValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Generate
            // 
            this.Generate.Enabled = false;
            this.Generate.Location = new System.Drawing.Point(939, 367);
            this.Generate.Name = "Generate";
            this.Generate.Size = new System.Drawing.Size(119, 42);
            this.Generate.TabIndex = 2;
            this.Generate.Text = "生成";
            this.Generate.UseVisualStyleBackColor = true;
            this.Generate.Click += new System.EventHandler(this.Weatherizing_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(609, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "噪点";
            // 
            // Noise
            // 
            this.Noise.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.Noise.Enabled = false;
            this.Noise.Location = new System.Drawing.Point(666, 109);
            this.Noise.Maximum = 100;
            this.Noise.Name = "Noise";
            this.Noise.Size = new System.Drawing.Size(328, 56);
            this.Noise.TabIndex = 3;
            this.Noise.TickFrequency = 20;
            this.Noise.Scroll += new System.EventHandler(this.Noise_Scroll);
            this.Noise.ValueChanged += new System.EventHandler(this.Noise_ValueChanged);
            // 
            // noiseValue
            // 
            this.noiseValue.AutoSize = true;
            this.noiseValue.Location = new System.Drawing.Point(1000, 109);
            this.noiseValue.Name = "noiseValue";
            this.noiseValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.noiseValue.Size = new System.Drawing.Size(41, 25);
            this.noiseValue.TabIndex = 4;
            this.noiseValue.Text = "0%";
            this.noiseValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Enabled = false;
            this.SaveBtn.Location = new System.Drawing.Point(939, 482);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(119, 42);
            this.SaveBtn.TabIndex = 2;
            this.SaveBtn.Text = "保存";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(9, 31);
            this.linkLabel1.Location = new System.Drawing.Point(634, 551);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(397, 29);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "唯一指定官方网站 Https://ZangAi.Family/";
            this.linkLabel1.UseCompatibleTextRendering = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(569, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(2, 550);
            this.label3.TabIndex = 7;
            // 
            // zooming
            // 
            this.zooming.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.zooming.Enabled = false;
            this.zooming.Location = new System.Drawing.Point(666, 295);
            this.zooming.Maximum = 100;
            this.zooming.Name = "zooming";
            this.zooming.Size = new System.Drawing.Size(328, 56);
            this.zooming.TabIndex = 3;
            this.zooming.TickFrequency = 20;
            this.zooming.Value = 100;
            this.zooming.Scroll += new System.EventHandler(this.zooming_Scroll);
            // 
            // zoomingValue
            // 
            this.zoomingValue.AutoSize = true;
            this.zoomingValue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.zoomingValue.Location = new System.Drawing.Point(1000, 295);
            this.zoomingValue.Name = "zoomingValue";
            this.zoomingValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.zoomingValue.Size = new System.Drawing.Size(63, 25);
            this.zoomingValue.TabIndex = 4;
            this.zoomingValue.Text = "100%";
            this.zoomingValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(609, 295);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 25);
            this.label5.TabIndex = 5;
            this.label5.Text = "缩放";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.Customized);
            this.groupBox1.Controls.Add(this.Default_H);
            this.groupBox1.Controls.Add(this.Default_M);
            this.groupBox1.Controls.Add(this.Default_L);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(666, 357);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 167);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // Default_L
            // 
            this.Default_L.AutoSize = true;
            this.Default_L.Location = new System.Drawing.Point(9, 24);
            this.Default_L.Name = "Default_L";
            this.Default_L.Size = new System.Drawing.Size(153, 29);
            this.Default_L.TabIndex = 0;
            this.Default_L.TabStop = true;
            this.Default_L.Text = "景泰蓝（低）";
            this.Default_L.UseVisualStyleBackColor = true;
            this.Default_L.Click += new System.EventHandler(this.Default_L_Click);
            // 
            // Default_M
            // 
            this.Default_M.AutoSize = true;
            this.Default_M.Location = new System.Drawing.Point(9, 59);
            this.Default_M.Name = "Default_M";
            this.Default_M.Size = new System.Drawing.Size(153, 29);
            this.Default_M.TabIndex = 1;
            this.Default_M.TabStop = true;
            this.Default_M.Text = "元青花（中）";
            this.Default_M.UseVisualStyleBackColor = true;
            this.Default_M.Click += new System.EventHandler(this.Default_M_Click);
            // 
            // Default_H
            // 
            this.Default_H.AutoSize = true;
            this.Default_H.Location = new System.Drawing.Point(9, 94);
            this.Default_H.Name = "Default_H";
            this.Default_H.Size = new System.Drawing.Size(153, 29);
            this.Default_H.TabIndex = 2;
            this.Default_H.TabStop = true;
            this.Default_H.Text = "唐三彩（高）";
            this.Default_H.UseVisualStyleBackColor = true;
            this.Default_H.Click += new System.EventHandler(this.Default_H_Click);
            // 
            // Customized
            // 
            this.Customized.AutoSize = true;
            this.Customized.Location = new System.Drawing.Point(9, 129);
            this.Customized.Name = "Customized";
            this.Customized.Size = new System.Drawing.Size(93, 29);
            this.Customized.TabIndex = 3;
            this.Customized.TabStop = true;
            this.Customized.Text = "自定义";
            this.Customized.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(609, 367);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 25);
            this.label4.TabIndex = 5;
            this.label4.Text = "预设";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 609);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lable2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.noiseValue);
            this.Controls.Add(this.GreeningValue);
            this.Controls.Add(this.zoomingValue);
            this.Controls.Add(this.WeatheringValue);
            this.Controls.Add(this.Noise);
            this.Controls.Add(this.Greening);
            this.Controls.Add(this.zooming);
            this.Controls.Add(this.Compressing);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.Generate);
            this.Controls.Add(this.Select);
            this.Controls.Add(this.Source);
            this.Controls.Add(this.pictureBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "表情包电子包浆  By:KingsZNHONE";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Compressing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Greening)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Noise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zooming)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Source;
        private System.Windows.Forms.Button Select;
        public System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.TrackBar Compressing;
        private System.Windows.Forms.Label WeatheringValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lable2;
        private System.Windows.Forms.TrackBar Greening;
        private System.Windows.Forms.Label GreeningValue;
        private System.Windows.Forms.Button Generate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar Noise;
        private System.Windows.Forms.Label noiseValue;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar zooming;
        private System.Windows.Forms.Label zoomingValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Customized;
        private System.Windows.Forms.RadioButton Default_H;
        private System.Windows.Forms.RadioButton Default_M;
        private System.Windows.Forms.RadioButton Default_L;
        private System.Windows.Forms.Label label4;
    }
}

