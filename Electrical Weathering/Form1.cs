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
            saveFile.Filter = "JPEG File|jpeg;*.jpg";
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
                    zooming.Enabled = true;
                    Generate.Enabled = true;
                    groupBox1.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("就你那破图还想电子包浆？", "Excuse me?", MessageBoxButtons.OK);
            }
        }


        private void Weathering_Scroll(object sender, EventArgs e)
        {
            Customized.Checked = true; 
        }

        private void Greening_Scroll(object sender, EventArgs e)
        {
            Customized.Checked = true;
        }
        private void Noise_Scroll(object sender, EventArgs e)
        {
            Customized.Checked = true;
        }

        private void zooming_Scroll(object sender, EventArgs e)
        {
            zoomingValue.Text = Convert.ToString(zooming.Value) + @"%";
            Customized.Checked = true;
        }

        private void Weatherizing_Click(object sender, EventArgs e)
        {
            try
            {
                Image Emojpg = Image.FromFile(Source.Text);
                pictureBox.Image = Handler.Noising(Emojpg, Noise.Value);
                pictureBox.Image = Handler.Greening(pictureBox.Image, Greening.Value);
                pictureBox.Image = Handler.Weathering(pictureBox.Image, Compressing.Value);
                pictureBox.Image = Handler.Zooming(pictureBox.Image, zooming.Value/100f);
                SaveBtn.Enabled = true;
            }
            catch (Exception err)
            {
                MessageBox.Show("就你那破图还想电子包浆？", "Excuse me?", MessageBoxButtons.OK);
                MessageBox.Show(err.ToString(), "Excuse me?", MessageBoxButtons.OK);
            }


        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    ImageCodecInfo myImageCodecInfo = Handler.GetEncoderInfo("image/jpeg");
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                    using (Bitmap bitmap = new Bitmap(pictureBox.Image))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitmap.Save(saveFile.FileName, myImageCodecInfo, myEncoderParameters);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("就你那破图还想电子包浆？", "Excuse me?", MessageBoxButtons.OK);
                MessageBox.Show(err.ToString(), "Excuse me?", MessageBoxButtons.OK);
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

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (pictureBox.Image.Height > pictureBox.Height || pictureBox.Image.Width > pictureBox.Width)
                {
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                }
                resolutionLb.Text = Convert.ToString(pictureBox.Image.Width) + " X " + Convert.ToString(pictureBox.Image.Height);
            }
            catch (Exception Err)
            {

            }
            
        }

        private void Default_L_Click(object sender, EventArgs e)
        {
            Noise.Value = 0;
            Greening.Value = 5;
            Compressing.Value = 80;
        }

        private void Default_M_Click(object sender, EventArgs e)
        {
            Noise.Value = 5;
            Greening.Value = 10;
            Compressing.Value = 85;
        }

        private void Default_H_Click(object sender, EventArgs e)
        {
            Noise.Value = 10;
            Greening.Value = 15;
            Compressing.Value = 90;
        }

        private void Noise_ValueChanged(object sender, EventArgs e)
        {
            noiseValue.Text = Convert.ToString(Noise.Value) + @"%";
        }

        private void Greening_ValueChanged(object sender, EventArgs e)
        {
            GreeningValue.Text = Convert.ToString(Greening.Value) + @"%";
        }

        private void Compressing_ValueChanged(object sender, EventArgs e)
        {
            WeatheringValue.Text = Convert.ToString(Compressing.Value) + @"%";
        }
    }
}
