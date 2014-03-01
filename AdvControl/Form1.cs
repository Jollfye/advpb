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

        private void advProgressBar1_AdvMouseClick(object sender, AdvProgressBar.AdvProgressBar.AdvMouseEventArgs e)
        {
            textBox2.Text = e.Value.ToString() + " ( " + e.X + "; " + e.Y + ")";
        }

        private void advProgressBar1_AdvMouseMove(object sender, AdvProgressBar.AdvProgressBar.AdvMouseEventArgs e)
        {
            textBox2.Text = e.Value.ToString() + " ( " + e.X + "; " + e.Y + ")";
        }

        private void advProgressBar1_AdvValueChanged(object sender, AdvProgressBar.AdvProgressBar.AdvValueChangedEventArgs e)
        {
            trackBar1.Value = e.Value;
            textBox1.Text = e.Value.ToString();
        }
    }
}
