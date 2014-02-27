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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.advProgressBar2 = new AdvProgressBar.AdvProgressBar();
            this.advProgressBar1 = new AdvProgressBar.AdvProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(36, 271);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(200, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Value = 75;
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(570, 293);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Create AdvProgressBar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // advProgressBar2
            // 
            this.advProgressBar2.GradientColorList = ((System.Collections.ObjectModel.ObservableCollection<System.Drawing.Color>)(resources.GetObject("advProgressBar2.GradientColorList")));
            this.advProgressBar2.Location = new System.Drawing.Point(310, 77);
            this.advProgressBar2.Maximum = 60;
            this.advProgressBar2.Name = "advProgressBar2";
            this.advProgressBar2.Size = new System.Drawing.Size(175, 175);
            this.advProgressBar2.TabIndex = 5;
            this.advProgressBar2.Thickness = 17;
            // 
            // advProgressBar1
            // 
            this.advProgressBar1.GradientColorList = ((System.Collections.ObjectModel.ObservableCollection<System.Drawing.Color>)(resources.GetObject("advProgressBar1.GradientColorList")));
            this.advProgressBar1.Location = new System.Drawing.Point(36, 42);
            this.advProgressBar1.Name = "advProgressBar1";
            this.advProgressBar1.Size = new System.Drawing.Size(200, 200);
            this.advProgressBar1.TabIndex = 0;
            this.advProgressBar1.Thickness = 20;
            this.advProgressBar1.Value = 67;
            this.advProgressBar1.AdvMouseMove += new AdvProgressBar.AdvMouseMoveEventHandler(this.advProgressBar1_AdvMouseMove);
            this.advProgressBar1.AdvMouseClick += new AdvProgressBar.AdvMouseClickEventHandler(this.advProgressBar1_AdvMouseClick);
            this.advProgressBar1.AdvValueChanged += new AdvProgressBar.AdvValueChangedEventHandler(this.advProgressBar1_AdvValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 424);
            this.Controls.Add(this.advProgressBar2);
            this.Controls.Add(this.advProgressBar1);
            this.Controls.Add(this.button1);
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
        private System.Windows.Forms.Button button1;
        private AdvProgressBar.AdvProgressBar advProgressBar1;
        private AdvProgressBar.AdvProgressBar advProgressBar2;
        private System.Windows.Forms.Timer timer1;

    }
}

