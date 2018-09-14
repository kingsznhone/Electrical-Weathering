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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Compressing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Greening)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Noise)).BeginInit();
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
            // 
            // Source
            // 
            this.Source.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Source.Location = new System.Drawing.Point(614, 85);
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            this.Source.Size = new System.Drawing.Size(305, 30);
            this.Source.TabIndex = 1;
            this.Source.TextChanged += new System.EventHandler(this.Source_TextChanged);
            // 
            // Select
            // 
            this.Select.Location = new System.Drawing.Point(925, 85);
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
            this.Compressing.Location = new System.Drawing.Point(662, 340);
            this.Compressing.Maximum = 100;
            this.Compressing.Name = "Compressing";
            this.Compressing.Size = new System.Drawing.Size(328, 56);
            this.Compressing.TabIndex = 3;
            this.Compressing.TickFrequency = 20;
            this.Compressing.Scroll += new System.EventHandler(this.Weathering_Scroll);
            // 
            // WeatheringValue
            // 
            this.WeatheringValue.AutoSize = true;
            this.WeatheringValue.Location = new System.Drawing.Point(996, 340);
            this.WeatheringValue.Name = "WeatheringValue";
            this.WeatheringValue.Size = new System.Drawing.Size(23, 25);
            this.WeatheringValue.TabIndex = 4;
            this.WeatheringValue.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(609, 340);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "压缩";
            // 
            // lable2
            // 
            this.lable2.AutoSize = true;
            this.lable2.Location = new System.Drawing.Point(609, 257);
            this.lable2.Name = "lable2";
            this.lable2.Size = new System.Drawing.Size(52, 25);
            this.lable2.TabIndex = 5;
            this.lable2.Text = "绿化";
            // 
            // Greening
            // 
            this.Greening.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.Greening.Enabled = false;
            this.Greening.Location = new System.Drawing.Point(662, 257);
            this.Greening.Maximum = 100;
            this.Greening.Name = "Greening";
            this.Greening.Size = new System.Drawing.Size(328, 56);
            this.Greening.TabIndex = 3;
            this.Greening.TickFrequency = 20;
            this.Greening.Scroll += new System.EventHandler(this.Greening_Scroll);
            // 
            // GreeningValue
            // 
            this.GreeningValue.AutoSize = true;
            this.GreeningValue.Location = new System.Drawing.Point(996, 257);
            this.GreeningValue.Name = "GreeningValue";
            this.GreeningValue.Size = new System.Drawing.Size(23, 25);
            this.GreeningValue.TabIndex = 4;
            this.GreeningValue.Text = "0";
            // 
            // Generate
            // 
            this.Generate.Enabled = false;
            this.Generate.Location = new System.Drawing.Point(900, 402);
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
            this.label2.Location = new System.Drawing.Point(609, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "噪点";
            // 
            // Noise
            // 
            this.Noise.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.Noise.Enabled = false;
            this.Noise.Location = new System.Drawing.Point(662, 175);
            this.Noise.Maximum = 100;
            this.Noise.Name = "Noise";
            this.Noise.Size = new System.Drawing.Size(328, 56);
            this.Noise.TabIndex = 3;
            this.Noise.TickFrequency = 20;
            this.Noise.Scroll += new System.EventHandler(this.Noise_Scroll);
            // 
            // noiseValue
            // 
            this.noiseValue.AutoSize = true;
            this.noiseValue.Location = new System.Drawing.Point(996, 175);
            this.noiseValue.Name = "noiseValue";
            this.noiseValue.Size = new System.Drawing.Size(23, 25);
            this.noiseValue.TabIndex = 4;
            this.noiseValue.Text = "0";
            // 
            // SaveBtn
            // 
            this.SaveBtn.Enabled = false;
            this.SaveBtn.Location = new System.Drawing.Point(900, 480);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 609);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lable2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.noiseValue);
            this.Controls.Add(this.GreeningValue);
            this.Controls.Add(this.WeatheringValue);
            this.Controls.Add(this.Noise);
            this.Controls.Add(this.Greening);
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
    }
}

