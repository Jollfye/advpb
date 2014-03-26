namespace AdvControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.advProgressBar1 = new AdvProgressBar.AdvProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(36, 271);
            this.trackBar1.Maximum = 720;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(200, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Value = 6;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(86, 336);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(86, 362);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 3;
            // 
            // advProgressBar1
            // 
            this.advProgressBar1.GradientColorList = ((System.Collections.ObjectModel.ObservableCollection<System.Drawing.Color>)(resources.GetObject("advProgressBar1.GradientColorList")));
            this.advProgressBar1.Location = new System.Drawing.Point(307, 78);
            this.advProgressBar1.Maximum = 720;
            this.advProgressBar1.Name = "advProgressBar1";
            this.advProgressBar1.Size = new System.Drawing.Size(429, 422);
            this.advProgressBar1.TabIndex = 0;
            this.advProgressBar1.Thickness = 9;
            this.advProgressBar1.Value = 533;
            this.advProgressBar1.AdvMouseMove += new AdvProgressBar.AdvProgressBar.AdvMouseMoveEventHandler(this.advProgressBar1_AdvMouseMove);
            this.advProgressBar1.AdvMouseClick += new AdvProgressBar.AdvProgressBar.AdvMouseClickEventHandler(this.advProgressBar1_AdvMouseClick);
            this.advProgressBar1.AdvValueChanged += new AdvProgressBar.AdvProgressBar.AdvValueChangedEventHandler(this.advProgressBar1_AdvValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 566);
            this.Controls.Add(this.advProgressBar1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.trackBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private AdvProgressBar.AdvProgressBar advProgressBar1;

    }
}

