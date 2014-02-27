using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace AdvProgressBar
{
    #region ---Interface---

    /* собственные делегаты */
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

    /* собственный интерфейс */
    public interface IAdvEventProvider
    {
        event AdvMouseClickEventHandler AdvMouseClick;

        event AdvMouseMoveEventHandler AdvMouseMove;

        event AdvValueChangedEventHandler AdvValueChanged;
    }

    #endregion

    #region ---AdvProgressBar---

    /* установка иконки элемента управления */
    [ToolboxItem(true)]
    [ToolboxBitmapAttribute(typeof(AdvProgressBar), "AdvProgressBar.bmp")]
    /* определение элемента управления и использование собственного интерфейса */
    public class AdvProgressBar : ProgressBar, IAdvEventProvider
    {

        #region --Constructors--

        /* конструкторы элемента управления */
        public AdvProgressBar()
        {
            InitializeGradient(null);
            this.BaseColor = _baseColor;
            this.BackColor = _backColor;
            this.ForeColor = _foreColor;
            this.Location = new Point(0, 0);
            this.Size = new Size(100, 100);
        }

        /* использование структуры Nullable<T> как необязательный параметр в конструкторе */
        public AdvProgressBar(List<Color> gradientColorList = null, int? thickness = null)
        {
            InitializeGradient(gradientColorList);
            this.Thickness = thickness.HasValue ? thickness.Value : _thickness;
            this.BaseColor = _baseColor;
            this.BackColor = _backColor;
            this.ForeColor = _foreColor;
        }

        public AdvProgressBar(List<Color> gradientColorList = null, int? thickness = null, Size? size = null, Point? location = null) 
        {
            InitializeGradient(gradientColorList);
            this.Thickness = thickness.HasValue ? thickness.Value : _thickness;
            this.Size = size.HasValue ? size.Value : new Size(100, 100);
            this.Location = location.HasValue ? location.Value : new Point(0, 0);
            this.BaseColor = _baseColor;
            this.BackColor = _backColor;
            this.ForeColor = _foreColor;
        }

        public AdvProgressBar(List<Color> gradientColorList = null, int? thickness = null, Color? baseColor = null, Color? backColor = null, Color? foreColor = null)
        {
            InitializeGradient(gradientColorList);
            this.Thickness = thickness.HasValue ? thickness.Value : _thickness;
            this.BaseColor = baseColor.HasValue ? baseColor.Value : _baseColor;
            this.BackColor = backColor.HasValue ? backColor.Value : _backColor;
            this.ForeColor = foreColor.HasValue ? foreColor.Value : _foreColor;
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

        #endregion

        #region --Properties--

        private bool _active = true;
        [Description("Indicates whether the control reaction on value change")]
        [CategoryAttribute("Behavior")]
        [DefaultValueAttribute(true)]
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
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

        public new int Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                try
                {
                    if (value < this.Minimum || value > this.Maximum)
                        throw new ArgumentOutOfRangeException();
                    base.Value = value;
                    OnAdvValueChanged(new AdvValueChangedEventArgs(value));
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Exception caught", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }
        }

        private bool IsAdvMouseDownIn;

        #endregion

        #region --Methods--

        /* определение принадлежности точки эллипсу с заданным отступом от границ элемента управления */
        public bool IsPointInEllipse(Point point, int offset)
        {
            return Math.Pow(point.X - this.Width / 2, 2) / Math.Pow((this.Width - offset * 2) / 2, 2) +
                   Math.Pow(point.Y - this.Height / 2, 2) / Math.Pow((this.Height - offset * 2) / 2, 2) <= 1.0;
        }

        /* получение значения индикатора по точке указателя мыши */
        public int GetPointEllipseValue(Point point)
        {
            double dx = (point.X - this.Width / 2);
            double dy = (point.Y - this.Height / 2);
            double theta = Math.Atan2(dx, -dy);
            double sweepAngle = ((theta * 180 / Math.PI) + 360) % 360;
            int value = Convert.ToInt32(sweepAngle / (360f / this.Maximum));
            return value;
        }

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
            return new Rectangle(offset, offset, this.Width - offset * 2, this.Height - offset * 2);
        }

        /* инициализация элемента управления и градиента индикатора */
        private void InitializeGradient(List<Color> gradientColorList)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint,
              true);

            if (gradientColorList != null && gradientColorList.Count > 1)
            {
                _gradientColorList.Clear();

                foreach (Color color in gradientColorList)
                    _gradientColorList.Add(color);
            }

            FillGradientFullColorList(_gradientColorList);

            OnGradientColorListCollectionChanged();
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
            g.DrawEllipse(baseColorPen, GetRectangle(1));
            g.FillEllipse(baseColorBrush, GetRectangle(1));

            /* отрисовка градиента индикатора */
            int i = 0;
            if (Gradient)
            {
                float endAngle = 360f * this.Value / this.Maximum - 90f;
                float sweepAngle = 360f / _gradientFullColorList.Count;
                for (float startAngle = -90f; startAngle < endAngle; startAngle += sweepAngle)
                {
                    if (i < _gradientFullColorList.Count)
                    {
                        g.DrawPie(new Pen(_gradientFullColorList[i]), GetRectangle(1), startAngle, sweepAngle);
                        g.FillPie(new SolidBrush(_gradientFullColorList[i++]), GetRectangle(1), startAngle, sweepAngle);
                    }
                }
            }
            /* заливка индикатора сплошным цветом, градиент отключен */
            else
            {
                float startAngle = -90f;
                float sweepAngle = (360f / this.Maximum) * this.Value;
                g.DrawPie(foreColorPen, GetRectangle(1), startAngle, sweepAngle);
                g.FillPie(foreColorBrush, GetRectangle(1), startAngle, sweepAngle);
            }

            /* отрисовка внутреннего эллипса элемента управления */
            g.DrawEllipse(backColorPen, GetRectangle(this.Thickness));
            g.FillEllipse(backColorBrush, GetRectangle(this.Thickness));

            /* создание надписи со значением индикатора в центре элемента управления */
            string strValue = this.Value.ToString();
            float size = (float)(Math.Min(this.Width - this.Thickness * 2, this.Height - this.Thickness * 2) / 2);
            Font strFont = new Font("Cambria", (size > 0 ? size : 1), FontStyle.Bold, GraphicsUnit.Pixel);
            int strBrushColorIndex = i - 1 > 0 ? i - 1 : 0;
            Brush strBrush = Gradient ? new SolidBrush(_gradientFullColorList[strBrushColorIndex]) : foreColorBrush;
            SizeF strLen = g.MeasureString(strValue, strFont);
            Point strLoc = new Point((int)((this.Width / 2) - (strLen.Width / 2)), (int)((this.Height / 2) - (strLen.Height / 2)));
            g.DrawString(strValue, strFont, strBrush, strLoc);

            e.Graphics.DrawImage(bmp, Point.Empty);
        }

        #endregion

        #region --Events--

        /* собсвтенные события элемента управления */
        [CategoryAttribute("Mouse")]
        public event AdvMouseMoveEventHandler AdvMouseMove;

        [CategoryAttribute("Action")]
        public event AdvMouseClickEventHandler AdvMouseClick;

        [CategoryAttribute("Property Changed")]
        public event AdvValueChangedEventHandler AdvValueChanged;

        /* перегрузка базовых методов элемента управления для реализации событий */
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (AdvMouseMove != null && Active && e.Button == MouseButtons.Left && IsAdvMouseDownIn)
            {
                int value = GetPointEllipseValue(new Point(e.X, e.Y));
                this.Value = value;
                AdvMouseMove(this, new AdvMouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, value));
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Point point = new Point(e.X, e.Y);
            if (AdvMouseMove != null && Active && !IsPointInEllipse(point, this.Thickness) && IsPointInEllipse(point, 0))
                this.IsAdvMouseDownIn = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (AdvMouseMove != null && Active)
                this.IsAdvMouseDownIn = false;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Point point = new Point(e.X, e.Y);
            if (AdvMouseClick != null && Active && !IsPointInEllipse(point, this.Thickness) && IsPointInEllipse(point, 0))
            {
                int value = GetPointEllipseValue(point);
                this.Value = value;
                AdvMouseClick(this, new AdvMouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, value));
            }
        }

        /* метод для реализации события индикации изменения значения элемента управления */
        protected virtual void OnAdvValueChanged(AdvValueChangedEventArgs e)
        {
            if (AdvValueChanged != null)
                AdvValueChanged(this, e);
        }

        #endregion

    }

    #endregion
}
