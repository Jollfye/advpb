using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
		
		private void trackBar1_Scroll(object sender, EventArgs e)
        {
            advProgressBar1.Value = trackBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Color> gradientColorList = new List<Color>();
            gradientColorList.Add(Color.FromArgb(255, 192, 255));
            gradientColorList.Add(Color.FromArgb(255, 128, 255));
            gradientColorList.Add(Color.FromArgb(230, 86, 230));
            AdvProgressBar.AdvProgressBar advProgressBar = new AdvProgressBar.AdvProgressBar(gradientColorList, 15, new Size(150, 150), new Point(560, 111));
            advProgressBar.Value = 75;
            this.Controls.Add(advProgressBar);
        }

        private void advProgressBar1_AdvMouseClick(object sender, AdvProgressBar.AdvMouseEventArgs e)
        {
            textBox2.Text = e.Value.ToString() + " ( " + e.X + "; " + e.Y + ")";
        }

        private void advProgressBar1_AdvMouseMove(object sender, AdvProgressBar.AdvMouseEventArgs e)
        {
            textBox2.Text = e.Value.ToString() + " ( " + e.X + "; " + e.Y + ")";
        }

        private void advProgressBar1_AdvValueChanged(object sender, AdvProgressBar.AdvValueChangedEventArgs e)
        {
            trackBar1.Value = e.Value;
            textBox1.Text = e.Value.ToString();
        }

        private int i = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (i > 60)
                i = 1;
            advProgressBar2.Value = i++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
