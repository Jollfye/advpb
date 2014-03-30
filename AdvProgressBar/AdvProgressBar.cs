using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Collections;

namespace AdvProgressBar
{
    [ToolboxItem(true)]
    [ToolboxBitmapAttribute(typeof(AdvProgressBar), "AdvProgressBar.bmp")]
    public class AdvProgressBar : ProgressBar
    {
        public AdvProgressBar()
        {
            InitializeGradient(null);
            this.BaseColor = _baseColor;
            this.BackColor = _backColor;
            this.ForeColor = _foreColor;
            this.Location = new Point(0, 0);
            this.Size = new Size(100, 100);
        }

        public AdvProgressBar(List<Color> gradientColorList = null, int? thickness = null, Color? baseColor = null, Color? backColor = null, Color? foreColor = null, Size? size = null, Point? location = null)
        {
            InitializeGradient(gradientColorList);
            this.Thickness = thickness.HasValue ? thickness.Value : _thickness;
            this.BaseColor = baseColor.HasValue ? baseColor.Value : _baseColor;
            this.BackColor = backColor.HasValue ? backColor.Value : _backColor;
            this.ForeColor = foreColor.HasValue ? foreColor.Value : _foreColor;
            this.Size = size.HasValue ? size.Value : new Size(100, 100);
            this.Location = location.HasValue ? location.Value : new Point(0, 0);
        }

        private bool _gradient = true;
        [Description("Indicates whether the control has gradient or not")]
        [CategoryAttribute("Appearance")]
        [DefaultValueAttribute(true)]
        public bool Gradient
        {
            get
            {
                return _gradient;
            }
            set
            {
                _gradient = value;
                Invalidate();
            }
        }

        private ObservableCollection<Color> _gradientDefaultColorList = new ObservableCollection<Color>(new Color[] { Color.FromArgb(116, 194, 225), Color.FromArgb(1, 145, 200), Color.FromArgb(0, 91, 154) });
        private ObservableCollection<Color> _gradientFullColorList = new ObservableCollection<Color>();
        private ObservableCollection<Color> _gradientColorList = new ObservableCollection<Color>(new Color[] { Color.FromArgb(116, 194, 225), Color.FromArgb(1, 145, 200), Color.FromArgb(0, 91, 154) });
        [Description("The gradient color collection of the component")]
        [CategoryAttribute("Appearance")]
        public ObservableCollection<Color> GradientColorList
        {
            get
            {
                return _gradientColorList;
            }
            set
            {
                _gradientColorList = value;
                OnGradientColorListCollectionChanged();

                if (_gradientColorList.Count < 2)
                    FillGradientFullColorList(_gradientDefaultColorList);
                else
                    FillGradientFullColorList(_gradientColorList);
                Invalidate();
            }
        }

        private int _gradientSmoothness = 5;
        [Description("The gradient smoothness of the control")]
        [CategoryAttribute("Appearance")]
        [DefaultValueAttribute(5)]
        public int GradientSmoothness
        {
            get
            {
                return _gradientSmoothness;
            }
            set
            {
                try
                {
                    if (value < 0 || value > 5)
                        throw new ArgumentOutOfRangeException();
                    if (value == 0)
                        throw new ArgumentNullException();
                    _gradientSmoothness = value;
                    if (_gradientColorList.Count < 2)
                        FillGradientFullColorList(_gradientDefaultColorList);
                    else
                        FillGradientFullColorList(_gradientColorList);
                    Invalidate();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Exception caught", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }
        }
        
        private int _thickness = 10;
        [Description("The thickness of the control")]
        [CategoryAttribute("Layout")]
        [DefaultValueAttribute(10)]
        public int Thickness
        {
            get
            {
                return _thickness;
            }
            set
            {
                try
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException();
                    if (value == 0)
                        throw new ArgumentNullException();
                    _thickness = value;
                    Invalidate();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Exception caught", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }
        }

        private Color _baseColor = Color.FromArgb(204, 204, 204);
        [Description("The base color of the component")]
        [CategoryAttribute("Appearance")]
        [DefaultValueAttribute(typeof(Color), "204, 204, 204")]
        public Color BaseColor
        {
            get
            {
                return _baseColor;
            }
            set
            {
                _baseColor = value;
                Invalidate();
            }
        }

        private Color _backColor = SystemColors.Control;
        [DefaultValueAttribute(typeof(Color), "Control")]
        public new Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                Invalidate();
            }
        }

        private Color _foreColor = Color.FromArgb(1, 145, 200);
        [DefaultValueAttribute(typeof(Color), "1, 145, 200")]
        public new Color ForeColor
        {
            get
            {
                return _foreColor;
            }
            set
            {
                _foreColor = value;
                Invalidate();
            }
        }

        //[Browsable(false)]
        public new Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                try
                {
                    if (5 / 4 * value.Width > value.Height)
                        throw new Exception("Height must be more than 5/4 of Width!");
                    base.Size = value;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Exception caught", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    base.Size = new Size(value.Width, 5 / 4 * value.Width);
                }
            }
        }

        private int _value;
        public new int Value
        {
            get
            {
                return _value;
            }
            set
            {
                try
                {
                    if (value < this.Minimum || value > this.Maximum)
                        throw new ArgumentOutOfRangeException();
                    _value = value;
                    OnAdvValueChanged(new AdvValueChangedEventArgs(value));
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Exception caught", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }
        }

        private bool IsAdvMouseDownIn;

        /* определение принадлежности точки эллипсу с заданным отступом от границ элемента управления */
        public bool IsPointInCircle(Point point, Point center, int rad)
        {
            return Math.Pow((point.X - center.X), 2) + Math.Pow((point.Y - center.Y), 2) <= Math.Pow(rad, 2);
        }
        /* получение значения индикатора по точке указателя мыши */
        public int GetPointEllipseValue(Point point)
        {
            double dx = (point.X - this.Width / 2);
            double dy = (point.Y - this.Width / 2);
            double theta = Math.Atan2(dx, -dy);
            double sweepAngle = ((theta * 180 / Math.PI) + 360) % 360;
            int value = Convert.ToInt32(sweepAngle / (360f / this.Maximum));
            return value;
        }

        private bool Alarm = false;

        /* получение среднего цвета по двум указанным */
        private Color GetMiddleColor(Color color1, Color color2)
        {
            return Color.FromArgb(Convert.ToInt32((Convert.ToInt32(color1.R) + Convert.ToInt32(color2.R)) / 2),
                                  Convert.ToInt32((Convert.ToInt32(color1.G) + Convert.ToInt32(color2.G)) / 2),
                                  Convert.ToInt32((Convert.ToInt32(color1.B) + Convert.ToInt32(color2.B)) / 2));
        }

        /* получение прямоугольника для построения и заливки заданного эллипса */
        private Rectangle GetRectangle(int offset)
        {
            int minrad = this.Width / 10;
            return new Rectangle(offset + minrad, offset + minrad, this.Width - offset * 2 - minrad * 2, this.Width - offset * 2 - minrad * 2);
        }

        /* инициализация элемента управления и градиента индикатора */
        private void InitializeGradient(List<Color> gradientColorList)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint,
              true);

            Timer timer = new Timer();

            timer.Tick += new EventHandler(Tick);
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Start();

            if (gradientColorList != null && gradientColorList.Count > 1)
            {
                _gradientColorList.Clear();

                foreach (Color color in gradientColorList)
                    _gradientColorList.Add(color);
            }

            FillGradientFullColorList(_gradientColorList);

            OnGradientColorListCollectionChanged();
        }

        void Tick(object sender, EventArgs e)
        {
            if (!Alarm)
            {
                DateTime now = DateTime.Now;
                Value = (now.Hour % 12) * 60 + now.Minute;
                Invalidate();
            }
        }

        /* заполнение расширенной коллекции цветов на основе заданной и гладкости градиента индикатора */
        private void FillGradientFullColorList(ObservableCollection<Color> _gradientColorList)
        {
            _gradientFullColorList.Clear();

            foreach (Color color in _gradientColorList)
                _gradientFullColorList.Add(color);

            for (int subdivideCount = 0; subdivideCount <= GradientSmoothness; subdivideCount++)
            {
                int i = 0;
                int gradientFullColorListCount = _gradientFullColorList.Count - 1;
                while (i < gradientFullColorListCount)
                {
                    _gradientFullColorList.Insert(i + 1, GetMiddleColor(_gradientFullColorList[i], _gradientFullColorList[i + 1]));
                    i += 2;
                    gradientFullColorListCount++;
                }
            }
        }

        private void OnGradientColorListCollectionChanged()
        {
            _gradientColorList.CollectionChanged += _gradientColorList_CollectionChanged;
        }

        private void _gradientColorList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_gradientColorList.Count < 2)
                FillGradientFullColorList(_gradientDefaultColorList);
            else
                FillGradientFullColorList(_gradientColorList);
            Invalidate();
        }

        private bool IsAm = true;

        Dictionary<int, bool> Alarms = new Dictionary<int,bool>();

        /* отрисовка элемента управления */
        protected override void OnPaint(PaintEventArgs e)
        {
            Image bmp = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Pen baseColorPen = new Pen(this.BaseColor);
            Pen backColorPen = new Pen(this.BackColor);
            Pen foreColorPen = new Pen(this.ForeColor);

            SolidBrush baseColorBrush = new SolidBrush(this.BaseColor);
            SolidBrush backColorBrush = new SolidBrush(this.BackColor);
            SolidBrush foreColorBrush = new SolidBrush(this.ForeColor);

            /* отрисовка внешнего эллипса элемента управления */
            g.DrawEllipse(baseColorPen, GetRectangle(0));
            g.FillEllipse(baseColorBrush, GetRectangle(0));

            /* отрисовка градиента индикатора */
            int i = 0;
            float startAngle = -90f;
            float endAngle = 360f * this.Value / this.Maximum - 90f;
            float sweepAngle;
            if (Gradient)
            {
                sweepAngle = 360f / _gradientFullColorList.Count;
                for (; startAngle < endAngle; startAngle += sweepAngle)
                {
                    if (i < _gradientFullColorList.Count)
                    {
                        g.DrawPie(new Pen(_gradientFullColorList[i]), GetRectangle(0), startAngle, sweepAngle);
                        g.FillPie(new SolidBrush(_gradientFullColorList[i++]), GetRectangle(0), startAngle, sweepAngle);
                    }
                }
            }
            /* заливка индикатора сплошным цветом, градиент отключен */
            else
            {
                sweepAngle = (360f / this.Maximum) * this.Value;
                g.DrawPie(foreColorPen, GetRectangle(0), startAngle, sweepAngle);
                g.FillPie(foreColorBrush, GetRectangle(0), startAngle, sweepAngle);
            }
            if (!Alarm)
            {
                IEnumerator k = Alarms.GetEnumerator();
                for (int j = 0; j < Alarms.Count; j++)
                {
                    k.MoveNext();
                    var tmp = (KeyValuePair<int, bool>)k.Current;
                    int val = tmp.Key;
                    g.FillPie(Brushes.Red, GetRectangle(-1), -90f + val / 2 - 1f, 2f);

                }
            }
            /* отрисовка внутреннего эллипса элемента управления */
            g.DrawEllipse(backColorPen, GetRectangle(this.Thickness));
            g.FillEllipse(backColorBrush, GetRectangle(this.Thickness));

            startAngle = -90f;
            for (int j = 0; j < 12; j++)
            {
                g.FillPie(backColorBrush, GetRectangle(-1), startAngle + j * 30f - 1f, 2f);
            }

            int IsParity = 0;
            if (Thickness % 2 != 0) IsParity = 1;

            /* создание надписи со значением индикатора в центре элемента управления */
            string strValue = (this.Value/60 == 0 ? 12 :this.Value/60).ToString();
            float size = (float)(this.Width *0.7 /2);
            Font strFont = new Font("Cambria", (size > 0 ? size : 1), FontStyle.Bold, GraphicsUnit.Pixel);
            int GradientLastColorIndex = i - 1 > 0 ? i - 1 : 0;
            Brush foreBrush = Gradient ? new SolidBrush(_gradientFullColorList[GradientLastColorIndex]) : foreColorBrush;
            Pen forePen = Gradient ? new Pen(_gradientFullColorList[GradientLastColorIndex]) : foreColorPen;
            SizeF strLen = g.MeasureString(strValue, strFont);
            Point strLoc = new Point((int)((this.Width / 2) - (strLen.Width / 2) + 2 + (-1) * (1 - IsParity)), (int)((this.Width / 2) - (strLen.Height / 2)) + 2 + (-1) * (1 - IsParity));
            g.DrawString(strValue, strFont, foreBrush, strLoc);

            float rad; float minbigrad; int minx; int miny;
            int minrad = this.Width / 10;
            

            //
            endAngle = 360f * Value / this.Maximum - 90f;
            rad = this.Width / 2;
            minbigrad = (this.Width - Thickness - minrad * 2) / 2;
            minx = (int)(rad + minbigrad * Math.Cos(endAngle * Math.PI / 180));
            miny = (int)(rad + minbigrad * Math.Sin(endAngle * Math.PI / 180));
            int minxfore = minx - minrad - Thickness / 2 + 1 - IsParity;
            int minyfore = miny - minrad - Thickness / 2 + 1 - IsParity;
            int minxback = minx - minrad + Thickness / 2;
            int minyback = miny - minrad + Thickness / 2;
            int minwfore = minrad * 2 + Thickness - 2 + IsParity;
            int minhfore = minrad * 2 + Thickness - 2 + IsParity;
            int minwback = minrad * 2 - Thickness + IsParity;
            int minhback = minrad * 2 - Thickness + IsParity;




            Rectangle minrectfore = new Rectangle(minxfore, minyfore, minwfore, minhfore);
            g.FillEllipse(foreBrush, minrectfore);

            Rectangle minrectcarrfore = new Rectangle(minxfore + (Thickness - 4 - IsParity) / 2 + IsParity, minyfore + (Thickness - 4 - IsParity) / 2 + IsParity, minwfore - (Thickness - 4 - IsParity) - 2 * IsParity, minhfore - (Thickness - 4 - IsParity) - 2 * IsParity);
            g.FillPie(backColorBrush, minrectcarrfore, -90f + DateTime.Now.Second * 6f - 15f, 21f);

            Rectangle minrectcarrback = new Rectangle(minxback - (Thickness - 4 - IsParity) / 2, minyback - (Thickness - 4 - IsParity) / 2, minwback + (Thickness - 4 - IsParity), minhback + (Thickness - 4 - IsParity));
            g.FillEllipse(foreBrush, minrectcarrback);

            Rectangle minrectback = new Rectangle(minxback, minyback, minwback, minhback);
            g.FillEllipse(backColorBrush, minrectback);

            strValue = String.Format("{0:d2}", this.Value % 60);
            size = (float)(Math.Min(minwfore , minhfore) / 2);
            strFont = new Font("Cambria", (size > 0 ? size : 1), FontStyle.Bold, GraphicsUnit.Pixel);
            strLen = g.MeasureString(strValue, strFont);
            strLoc = new Point((int)((minxfore + minwfore / 2) - (strLen.Width / 2) + 2), (int)((minyfore + minhfore / 2) - (strLen.Height / 2)) + 2);
            g.DrawString(strValue, strFont, foreBrush, strLoc);

            //g.DrawRectangle(Pens.Black, new Rectangle(0, this.Width, this.Width, this.Width / 4));
            //g.DrawEllipse(Pens.Black, new Rectangle(Thickness/2, Thickness/2, this.Width - Thickness, this.Width - Thickness ));
            if (Alarm)
            {
                g.FillEllipse(foreBrush, Width / 2 - Width / 14, Width * 41 / 40, Width / 7, Width / 7);

                strValue = "OK";
                size = Width / 16;
                strFont = new Font("Times new roman", (size > 0 ? size : 1), FontStyle.Bold, GraphicsUnit.Pixel);
                strLen = g.MeasureString(strValue, strFont);
                strLoc = new Point((int)(Width / 2 - strLen.Width / 2 + 2), (int)(Width * 41 / 40 + Width / 14 - strLen.Height / 2));
                g.DrawString(strValue, strFont, backColorBrush, strLoc);
            }

            Brush ampmBrush = foreBrush;
            Brush strBrush = backColorBrush;

            if ( DateTime.Now.Hour < 12 || Alarm)
            {               
                strValue = "AM";
                size = Width / 24;
                strFont = new Font("Times new roman", (size > 0 ? size : 1), FontStyle.Bold, GraphicsUnit.Pixel);
                strLen = g.MeasureString(strValue, strFont);
                strLoc = new Point((int)(Width / 2 - strLen.Width / 2 + 2 - Width / 3 + Width / 20), (int)(Width * 37 / 40 + Width / 13 - strLen.Height / 2));               
                if (Alarm && !IsAm)
                {
                    ampmBrush = baseColorBrush;
                    strBrush = foreBrush;
                }                   

                g.FillEllipse(ampmBrush, Width / 2 - Width / 3, Width * 19 / 20, Width / 10, Width / 10); 
                g.DrawString(strValue, strFont, strBrush, strLoc);
            }
             
            if  (DateTime.Now.Hour >= 12 || Alarm)
            {
                ampmBrush = foreBrush;
                strBrush = backColorBrush;
                strValue = "PM";
                size = Width / 24;
                strFont = new Font("Times new roman", (size > 0 ? size : 1), FontStyle.Bold, GraphicsUnit.Pixel);
                strLen = g.MeasureString(strValue, strFont);
                strLoc = new Point((int)(Width / 2 - strLen.Width / 2 + 2 + Width / 3 - Width / 20), (int)(Width * 37 / 40 + Width / 13 - strLen.Height / 2));
                if (Alarm && IsAm)
                {
                    ampmBrush = baseColorBrush;
                    strBrush = foreBrush;
                }  
                g.FillEllipse(ampmBrush, Width / 2 + Width / 3 - Width / 10, Width * 19 / 20, Width / 10, Width / 10);
                g.DrawString(strValue, strFont, strBrush, strLoc);
            }


            e.Graphics.DrawImage(bmp, Point.Empty);
        }

        /* собсвтенные события элемента управления */
        [CategoryAttribute("Mouse")]
        public event AdvMouseMoveEventHandler AdvMouseMove;

        [CategoryAttribute("Action")]
        public event AdvMouseClickEventHandler AdvMouseClick;

        [CategoryAttribute("Property Changed")]
        public event AdvValueChangedEventHandler AdvValueChanged;

        public delegate void AdvMouseClickEventHandler(object sender, AdvMouseEventArgs e);

        public delegate void AdvMouseMoveEventHandler(object sender, AdvMouseEventArgs e);

        public delegate void AdvValueChangedEventHandler(object sender, AdvValueChangedEventArgs e);

        /* класс делегатов AdvMouseClickEventHandler и AdvMouseMoveEventHandler */
        public class AdvMouseEventArgs : MouseEventArgs
        {
            /* значение элемента управления */
            public int Value { get; set; }

            public AdvMouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta, int value)
                : base(button, clicks, x, y, delta)
            {
                Value = value;
            }

        }

        /* класс делегата AdvValueChangedEventHandler */
        public class AdvValueChangedEventArgs : EventArgs
        {
            public int Value { get; set; }
            public AdvValueChangedEventArgs(int value)
            {
                Value = value;
            }
        }

        /* перегрузка базовых методов элемента управления для реализации событий */
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (AdvMouseMove != null  && e.Button == MouseButtons.Left && IsAdvMouseDownIn)
            {
                int value = GetPointEllipseValue(new Point(e.X, e.Y));
                this.Value = value;
                AdvMouseMove(this, new AdvMouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, value));
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            int minrad = this.Width / 10;
            int rad = this.Width / 2;
            float minbigrad = (this.Width - Thickness - minrad * 2) / 2;
            float endAngle = 360f * this.Value / this.Maximum - 90f;
            int minx = (int)(rad + minbigrad * Math.Cos(endAngle * Math.PI / 180));
            int miny = (int)(rad + minbigrad * Math.Sin(endAngle * Math.PI / 180));
            Point clickpoint = new Point(e.X, e.Y);
            Point center = new Point(rad, rad);
            Point mincenter = new Point(minx, miny);
            if (AdvMouseMove != null && ((!IsPointInCircle(clickpoint, center, rad - this.Thickness - minrad) && IsPointInCircle(clickpoint, center, rad - minrad)) || (IsPointInCircle(clickpoint, mincenter, (int)(minrad + Thickness / 2)))))
                {
                    Alarm = true;
                    this.IsAdvMouseDownIn = true;
                }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (AdvMouseMove != null)
                this.IsAdvMouseDownIn = false;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            int minrad = this.Width / 10;
            int rad = this.Width / 2;
            Point clickpoint = new Point(e.X, e.Y);
            Point center = new Point(rad, rad);
            if (AdvMouseClick != null && !IsPointInCircle(clickpoint, center, rad - this.Thickness - minrad) && IsPointInCircle(clickpoint, center, rad - minrad))
            {
                Alarm = true;
                int value = GetPointEllipseValue(clickpoint);
                this.Value = value;
                AdvMouseClick(this, new AdvMouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, value));                
            }
            if (AdvMouseClick != null && IsPointInCircle(clickpoint, new Point(Width / 2 - Width / 3 + minrad/2, Width * 19 / 20 + minrad/2), minrad / 2))
                IsAm = true;
            if (AdvMouseClick != null && IsPointInCircle(clickpoint, new Point(Width / 2 + Width / 3 - Width / 10 + minrad / 2, Width * 19 / 20 + minrad / 2), minrad / 2))
                IsAm = false;
            if (AdvMouseClick != null && IsPointInCircle(clickpoint, new Point(Width / 2 - Width / 14 + this.Width / 14, Width * 41 / 40 + this.Width / 14), this.Width / 14))
            {
                Alarm = false;
                Alarms.Add(Value, IsAm);
            }
            
            Invalidate();
        }

        /* метод для реализации события индикации изменения значения элемента управления */
        protected virtual void OnAdvValueChanged(AdvValueChangedEventArgs e)
        {
            if (AdvValueChanged != null)
                AdvValueChanged(this, e);
        }
    }
}
