using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Electrical_Weathering
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFile.Filter = "All Image Files|*.bmp;*.jpeg;*.jpg;*.png";
            saveFileDialog1.Filter = "JPEG File|jpeg;*.jpg";
        }

        private void Select_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    Source.Text = openFile.FileName;
                    Image Emojpg = Image.FromFile(Source.Text);
                    if (Emojpg.Height > pictureBox.Height || Emojpg.Width > pictureBox.Width)
                    {
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                    pictureBox.Image = Emojpg;
                    Compressing.Enabled = true;
                    Greening.Enabled = true;
                    Noise.Enabled = true;
                    Generate.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("就你那破图还想电子包浆？", "Excuse me?", MessageBoxButtons.OK);
            }
        }


        private void Weathering_Scroll(object sender, EventArgs e)
        {
            WeatheringValue.Text = Convert.ToString(Compressing.Value);
        }

        private void Greening_Scroll(object sender, EventArgs e)
        {
            GreeningValue.Text = Convert.ToString(Greening.Value);
        }

        private void Weatherizing_Click(object sender, EventArgs e)
        {
            try
            {
                Image Emojpg = Image.FromFile(Source.Text);
                pictureBox.Image = Handler.Noising(Emojpg, Noise.Value);
                pictureBox.Image = Handler.Greening(pictureBox.Image, Greening.Value);
                pictureBox.Image = Handler.Weathering(pictureBox.Image, Compressing.Value);
                SaveBtn.Enabled = true;
            }
            catch
            {
                MessageBox.Show("就你那破图还想电子包浆？", "Excuse me?",MessageBoxButtons.OK);
            }

            
        }

        private void Noise_Scroll(object sender, EventArgs e)
        {
            noiseValue.Text = Convert.ToString(Noise.Value);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageCodecInfo myImageCodecInfo = Handler.GetEncoderInfo("image/jpeg");
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,100L);
                using (Bitmap bitmap = new Bitmap(pictureBox.Image))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(saveFileDialog1.FileName, myImageCodecInfo, myEncoderParameters);
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("Https://zangai.family/");
        }

        private void Source_TextChanged(object sender, EventArgs e)
        {
            Source.Focus();
            Source.Select(Source.TextLength, 0);
            Source.ScrollToCaret();
        }
    }
}
