using Additional;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using System.Globalization;
using System.Threading;
using System.Timers;
using System.Reflection;

namespace Additional
{
    public class ScrollBar
    {
        private bool mouseDown = false;
        private Point positionControl;
        private int scrollBarBGPnlWidth = 0;
        private int scrollBarCursorBtnWidth = 0;
        private int scrollBarBGPnlHeight = 0;
        private int scrollBarCursorBtnHeight = 0;
        private string _name;
        private dynamic _container;
        private const double textProportion = 2.857;
        private static int _percentageValue = 0;
        private static string _classFullPathofMethod = String.Empty;
        private static string _MethodNameOnChange = String.Empty;
        private static int positionCursor = 0;

        private static Utility utility = new Utility();

        public enum ScrollBarOrientation
        {
            Horizontal,
            Vertical
        }
        public enum ScrollBarType
        {
            Directional,
            Incremental
        }

        public void SetValue(Form form, string name, int value, ScrollBarOrientation orientation)
        {
            if (orientation == ScrollBarOrientation.Horizontal)
            {
                Control ScrollBarPnl = utility.GetControlByChildName(form, "ScrollBarHorizontalPnl_" + name);
                bool haveControls = ScrollBarPnl.Controls.Count > 0 ? true : false;
                if (!haveControls) { return; }
                Control ScrollBarCursorBtn = utility.GetControlByChildName(form, "ScrollBarHorizontalCursorBtn_" + name);
                Control ScrollBarBGPnl = utility.GetControlByChildName(form, "ScrollBarHorizontalBG_" + name);

                var _params = ScrollBarPnl.Tag.ToString().Split(';');
                _percentageValue = int.Parse(_params[0]);
                _classFullPathofMethod = _params[1];
                _MethodNameOnChange = _params[2];
                
                Control ScrollBarHorizontalCursorBtn = utility.GetControlByChildName(form, "ScrollBarHorizontalCursorBtn_" + name);

                positionCursor = Convert.ToInt32(((double)value * (double)(ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width) / (double)_percentageValue));
                ScrollBarHorizontalCursorBtn.Left = positionCursor;
                ScrollBarHorizontalCursorBtn.BringToFront();
                ScrollBarHorizontalCursorBtn.Refresh();
            }
            if (orientation == ScrollBarOrientation.Vertical)
            {
                Control ScrollBarPnl = utility.GetControlByChildName(form, "ScrollBarVerticalPnl_" + name);
                bool haveControls = ScrollBarPnl.Controls.Count > 0 ? true : false;
                if (!haveControls) { return; }
                Control ScrollBarCursorBtn = utility.GetControlByChildName(form, "ScrollBarVerticalCursorBtn_" + name);
                Control ScrollBarBGPnl = utility.GetControlByChildName(form, "ScrollBarVerticalBG_" + name);

                var _params = ScrollBarPnl.Tag.ToString().Split(';');
                _percentageValue = int.Parse(_params[0]);
                _classFullPathofMethod = _params[1];
                _MethodNameOnChange = _params[2];

                Control ScrollBarVerticalCursorBtn = utility.GetControlByChildName(form, "ScrollBarVerticalCursorBtn_" + name);
                positionCursor = (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height) - Convert.ToInt32(((double)value * (double)(ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height) / (double)_percentageValue));
                ScrollBarVerticalCursorBtn.Top = positionCursor;
                ScrollBarVerticalCursorBtn.BringToFront();
                ScrollBarVerticalCursorBtn.Refresh();
            }
            if (_MethodNameOnChange != String.Empty)
            {
                bool result = utility.CallMethodByName(_classFullPathofMethod, _MethodNameOnChange, value.ToString());
            }
        }

        public int GetValue(Form form, string name, ScrollBarOrientation orientation)
        {
            int value = 0;
            double dividend = 0;
            if (orientation == ScrollBarOrientation.Horizontal)
            {

                Control ScrollBarPnl = utility.GetControlByChildName(form, "ScrollBarHorizontalPnl_" + name);
                bool haveControls = ScrollBarPnl.Controls.Count > 0 ? true:false;
                if (!haveControls) { return value; }
                Control ScrollBarCursorBtn = utility.GetControlByChildName(form, "ScrollBarHorizontalCursorBtn_" + name);
                Control ScrollBarBGPnl = utility.GetControlByChildName(form, "ScrollBarHorizontalBG_" + name);

                var _params = ScrollBarPnl.Tag.ToString().Split(';');
                _percentageValue = int.Parse(_params[0]);
                _classFullPathofMethod = _params[1];
                _MethodNameOnChange = _params[2];

                positionCursor = ScrollBarCursorBtn.Left;
                dividend = (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width);
                if(dividend > 0 ) value = Convert.ToInt32(((double)positionCursor / dividend * _percentageValue));
            }
            if (orientation == ScrollBarOrientation.Vertical)
            {
                Control ScrollBarPnl = utility.GetControlByChildName(form, "ScrollBarVerticalPnl_" + name);
                bool haveControls = ScrollBarPnl.Controls.Count > 0 ? true : false;
                if (!haveControls) { return value; }
                Control ScrollBarCursorBtn = utility.GetControlByChildName(form, "ScrollBarVerticalCursorBtn_" + name);
                Control ScrollBarBGPnl = utility.GetControlByChildName(form, "ScrollBarVerticalBG_" + name);
                
                var _params = ScrollBarPnl.Tag.ToString().Split(';');
                _percentageValue = int.Parse(_params[0]);
                _classFullPathofMethod = _params[1];
                _MethodNameOnChange = _params[2];

                positionCursor = ScrollBarCursorBtn.Top;
                dividend = (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height);
                if (dividend > 0) value = _percentageValue - Convert.ToInt32(((double)positionCursor / dividend * _percentageValue));
            }
            return value;
        }

        public class ScrollBarParameters
        {
            public Form form { get; set; }
            public dynamic container { get; set; }
            public string name { get; set; }
            public int percentageValue { get; set; }
            public int top { get; set; }
            public int left { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public Color color { get; set; }
            public Color foreColor { get; set; }
            public ScrollBarType type { get; set; }
            public string classFullPathofMethod { get; set; } 
            public string MethodNameOnChange { get; set; } = "";
            public string MethodNameOnLeave { get; set; } = "";
        }

        public class CallMethodParam
        {
            public string ClassFullPathofMethod { get; set; }
            public string MethodNameOnChange { get; set; }
            public int Value { get; set; }
        }

        public void CreateHorizontal(ScrollBarParameters parameters)
        {

            _name = parameters.name;
            _container = parameters.container;
            _percentageValue = parameters.percentageValue;
            _classFullPathofMethod = parameters.classFullPathofMethod;
            _MethodNameOnChange = parameters.MethodNameOnChange;

            Color bgColor = utility.ConvertARGBtoRGB(Color.FromArgb(200, parameters.color.R, parameters.color.G, parameters.color.B));
            Color bgColor2 = utility.ConvertARGBtoRGB(Color.FromArgb(180, parameters.color.R, parameters.color.G, parameters.color.B));

            Panel ScrollBarPnl = new Panel();
            ScrollBarPnl.Width = parameters.width;
            ScrollBarPnl.Height = parameters.height;
            ScrollBarPnl.Top = parameters.top;
            ScrollBarPnl.Left = parameters.left;
            ScrollBarPnl.BorderStyle = BorderStyle.None;
            ScrollBarPnl.BackColor = parameters.color;
            ScrollBarPnl.Name = "ScrollBarHorizontalPnl_" + parameters.name;
            ScrollBarPnl.Tag = parameters.percentageValue.ToString() + ";" + parameters.classFullPathofMethod + ";" + parameters.MethodNameOnChange;


            Panel ScrollBarBGPnl = new Panel();
            ScrollBarBGPnl.Width = parameters.width - (parameters.height * 2);
            ScrollBarBGPnl.Height = parameters.height;
            ScrollBarBGPnl.Top = 0;
            ScrollBarBGPnl.Left = parameters.height;
            ScrollBarBGPnl.BorderStyle = BorderStyle.None;
            ScrollBarBGPnl.BackColor = bgColor;
            ScrollBarBGPnl.Name = "ScrollBarHorizontalBG_" + parameters.name;

            ScrollBarPnl.Controls.Add(ScrollBarBGPnl);

            int sizeCursor = parameters.height * 3 / 4;

            Label ScrollBarLbl = new Label();
            ScrollBarLbl.Width = ScrollBarBGPnl.Width - sizeCursor;
            ScrollBarLbl.Height = parameters.height;
            ScrollBarLbl.Top = 0;
            ScrollBarLbl.Left = -ScrollBarLbl.Width;
            ScrollBarLbl.ForeColor = parameters.foreColor;
            ScrollBarLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            ScrollBarLbl.Name = "ScrollBarHorizontalLbl_" + parameters.name;

            ScrollBarBGPnl.Controls.Add(ScrollBarLbl);

            Button ScrollBarCursorBtn = new Button();
            ScrollBarCursorBtn.FlatStyle = FlatStyle.Flat;
            ScrollBarCursorBtn.Width = sizeCursor;
            ScrollBarCursorBtn.Height = parameters.height;
            ScrollBarCursorBtn.Top = 0;
            ScrollBarCursorBtn.Left = 0;
            ScrollBarCursorBtn.BackColor = bgColor2;
            ScrollBarCursorBtn.ForeColor = parameters.foreColor;
            ScrollBarCursorBtn.Name = "ScrollBarHorizontalCursorBtn_" + parameters.name;
            ScrollBarCursorBtn.Cursor = Cursors.Hand;



            int HeightBorderForm = parameters.form.Height - parameters.form.ClientRectangle.Height;
            int WidthBorderForm = parameters.form.Width - parameters.form.ClientRectangle.Width;

            ScrollBarCursorBtn.MouseDown += delegate (object sender, MouseEventArgs e)
            {
                mouseDown = true;
                positionControl = utility.GetPositionInForm(ScrollBarBGPnl);
                positionControl.X += WidthBorderForm + e.Location.X;
                positionControl.Y += HeightBorderForm + e.Location.Y;
            };

            ScrollBarCursorBtn.MouseUp += delegate
            {
                mouseDown = false;
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString());
                }
            };

            ScrollBarCursorBtn.MouseMove += delegate
            {
                if (mouseDown)
                {
                    int topCursor = (Cursor.Position.Y - positionControl.Y - parameters.form.Location.Y);
                    int leftCursor = (Cursor.Position.X - positionControl.X - parameters.form.Location.X);
                    if (leftCursor >= 0 && leftCursor <= (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width) ) { ScrollBarCursorBtn.Left = leftCursor; positionCursor = ScrollBarCursorBtn.Left;  ScrollBarCursorBtn.Refresh(); }
                }
            };

            ScrollBarCursorBtn.Move += delegate
            {
                Application.DoEvents();
                ScrollBarLbl.Left = ScrollBarCursorBtn.Left - ScrollBarLbl.Width;

            };

            ScrollBarCursorBtn.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString()); }
            };

            ScrollBarBGPnl.Controls.Add(ScrollBarCursorBtn);

            Button ScrollBarPlusBtn = new Button();
            ScrollBarPlusBtn.FlatStyle = FlatStyle.Flat;
            ScrollBarPlusBtn.Width = parameters.height;
            ScrollBarPlusBtn.Height = parameters.height;
            ScrollBarPlusBtn.Top = 0;
            ScrollBarPlusBtn.Left = parameters.width - parameters.height;
            ScrollBarPlusBtn.ForeColor = parameters.foreColor;
            ScrollBarPlusBtn.BackColor =  Color.Transparent;
            ScrollBarPlusBtn.Name = "ScrollBarHorizontalPlusBtn_" + parameters.name;
            if (parameters.type == ScrollBarType.Directional) { ScrollBarPlusBtn.Text = "u"; ScrollBarPlusBtn.Font = new Font("Wingdings 3", Convert.ToInt32(parameters.height / textProportion)); }
            if (parameters.type == ScrollBarType.Incremental) { ScrollBarPlusBtn.Text = "+"; ScrollBarPlusBtn.Font = new Font("Arial", Convert.ToInt32(parameters.height / textProportion), FontStyle.Bold); }
            
            ScrollBarPlusBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ScrollBarPlusBtn.Cursor = Cursors.Hand;
            ScrollBarPlusBtn.MouseDown += delegate
            {
                mouseDown = true;
                int turn = 0;
                while (mouseDown)
                {
                    Application.DoEvents();
                    if (ScrollBarCursorBtn.Left >= 0 && ScrollBarCursorBtn.Left < (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width)) { ScrollBarCursorBtn.Left++; ScrollBarCursorBtn.Refresh(); }
                    if (turn == 0) { utility.Sleep(200); } else { utility.Sleep(5); }
                    turn++;
                    
                }
            };

            ScrollBarPlusBtn.Click += delegate
            {
                if (mouseDown == false)
                {
                    if (ScrollBarCursorBtn.Left >= 0 && ScrollBarCursorBtn.Left < (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width)) { ScrollBarCursorBtn.Left++; ScrollBarCursorBtn.Refresh(); }
                    if (parameters.MethodNameOnChange != String.Empty)
                    {
                        bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString());
                    }
                }
            };

            ScrollBarPlusBtn.MouseUp += delegate
            {
                mouseDown = false;
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString());
                }
            };

            ScrollBarPlusBtn.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString()); }
            };

            ScrollBarPnl.Controls.Add(ScrollBarPlusBtn);

            Button ScrollBarMinusBtn = new Button();
            ScrollBarMinusBtn.FlatStyle = FlatStyle.Flat;
            ScrollBarMinusBtn.Width = parameters.height;
            ScrollBarMinusBtn.Height = parameters.height;
            ScrollBarMinusBtn.Top = 0;
            ScrollBarMinusBtn.Left = 0;
            ScrollBarMinusBtn.ForeColor = parameters.foreColor;
            ScrollBarMinusBtn.BackColor = Color.Transparent;
            ScrollBarMinusBtn.Name = "ScrollBarHorizontalMinusBtn_" + parameters.name;
            if (parameters.type == ScrollBarType.Directional) { ScrollBarMinusBtn.Text = "t"; ScrollBarMinusBtn.Font = new Font("Wingdings 3", Convert.ToInt32(parameters.height / textProportion)); }
            if (parameters.type == ScrollBarType.Incremental) { ScrollBarMinusBtn.Text = "-"; ScrollBarMinusBtn.Font = new Font("Arial", Convert.ToInt32(parameters.height / textProportion), FontStyle.Bold); }
            
            ScrollBarMinusBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ScrollBarMinusBtn.Cursor = Cursors.Hand;
            ScrollBarMinusBtn.MouseDown += delegate
            {
                mouseDown = true;
                int turn = 0;
                while (mouseDown)
                {
                    Application.DoEvents();
                    if (ScrollBarCursorBtn.Left > 0 && ScrollBarCursorBtn.Left <= (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width)) { ScrollBarCursorBtn.Left--; ScrollBarCursorBtn.Refresh(); }
                    if (turn == 0) { utility.Sleep(200); } else { utility.Sleep(5); }
                    turn++;
                }
            };

            ScrollBarMinusBtn.Click += delegate
            {
                if (mouseDown == false)
                {
                    if (ScrollBarCursorBtn.Left > 0 && ScrollBarCursorBtn.Left <= (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width)) { ScrollBarCursorBtn.Left--; ScrollBarCursorBtn.Refresh(); }
                    if (parameters.MethodNameOnChange != String.Empty)
                    {
                        bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString());
                    }
                }
            };

            ScrollBarMinusBtn.MouseUp += delegate
            {
                mouseDown = false;
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString());
                }
            };

            ScrollBarMinusBtn.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString()); }
            };

            ScrollBarPnl.Controls.Add(ScrollBarMinusBtn);

            ScrollBarBGPnl.MouseClick += delegate
            {
                Application.DoEvents();
                if (ScrollBarCursorBtn.Left >= 0 && ScrollBarCursorBtn.Left < (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width))
                {
                    ScrollBarCursorBtn.Left += 20;
                    if (ScrollBarCursorBtn.Left > (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width)) { ScrollBarCursorBtn.Left = (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width); positionCursor = ScrollBarCursorBtn.Left; }
                    if (ScrollBarCursorBtn.Left < 0) { ScrollBarCursorBtn.Left = 0; positionCursor = ScrollBarCursorBtn.Left; }
                    ScrollBarCursorBtn.Refresh();
                }
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString());
                }
            };

            ScrollBarBGPnl.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString()); }
            };

            ScrollBarLbl.MouseClick += delegate
            {
                Application.DoEvents();
                if (ScrollBarCursorBtn.Left > 0 && ScrollBarCursorBtn.Left <= (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width))
                {
                    ScrollBarCursorBtn.Left -= 20;
                    if (ScrollBarCursorBtn.Left > (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width)) { ScrollBarCursorBtn.Left = (ScrollBarBGPnl.Width - ScrollBarCursorBtn.Width); positionCursor = ScrollBarCursorBtn.Left; }
                    if (ScrollBarCursorBtn.Left < 0 ) { ScrollBarCursorBtn.Left = 0; positionCursor = ScrollBarCursorBtn.Left; }
                    ScrollBarCursorBtn.Refresh();
                }
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString());
                }
            };

            ScrollBarLbl.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString()); }
            };

            parameters.container.Controls.Add(ScrollBarPnl);

            scrollBarBGPnlWidth = ScrollBarBGPnl.Width;
            scrollBarCursorBtnWidth = ScrollBarCursorBtn.Width;
            
        }

        public void CreateVertical(ScrollBarParameters parameters)
        {

            _name = parameters.name;
            _container = parameters.container;
            _percentageValue = parameters.percentageValue;
           _classFullPathofMethod = parameters.classFullPathofMethod;
           _MethodNameOnChange = parameters.MethodNameOnChange;

            Color bgColor = utility.ConvertARGBtoRGB(Color.FromArgb(200, parameters.color.R, parameters.color.G, parameters.color.B));
            Color bgColor2 = utility.ConvertARGBtoRGB(Color.FromArgb(180, parameters.color.R, parameters.color.G, parameters.color.B));

            Panel ScrollBarPnl = new Panel();
            ScrollBarPnl.Width = parameters.width;
            ScrollBarPnl.Height = parameters.height;
            ScrollBarPnl.Top = parameters.top;
            ScrollBarPnl.Left = parameters.left;
            ScrollBarPnl.BorderStyle = BorderStyle.None;
            ScrollBarPnl.BackColor = parameters.color;
            ScrollBarPnl.Name = "ScrollBarVerticalPnl_" + parameters.name;
            ScrollBarPnl.Tag = parameters.percentageValue.ToString() + ";" + parameters.classFullPathofMethod + ";" + parameters.MethodNameOnChange;


            Panel ScrollBarBGPnl = new Panel();
            ScrollBarBGPnl.Width = parameters.width;
            ScrollBarBGPnl.Height = parameters.height - (parameters.width * 2);
            ScrollBarBGPnl.Top = parameters.width;
            ScrollBarBGPnl.Left = 0;
            ScrollBarBGPnl.BorderStyle = BorderStyle.None;
            ScrollBarBGPnl.BackColor = bgColor;
            ScrollBarBGPnl.Name = "ScrollBarVerticalBG_" + parameters.name;

            ScrollBarPnl.Controls.Add(ScrollBarBGPnl);

            int sizeCursor = parameters.width * 3 / 4;

            Label ScrollBarLbl = new Label();
            ScrollBarLbl.Width = parameters.width;
            ScrollBarLbl.Height = ScrollBarBGPnl.Height - sizeCursor;
            ScrollBarLbl.Top = -ScrollBarLbl.Width;
            ScrollBarLbl.Left = 0;
            ScrollBarLbl.ForeColor = parameters.foreColor;
            ScrollBarLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            ScrollBarLbl.Name = "ScrollBarVerticalLbl_" + parameters.name;

            ScrollBarBGPnl.Controls.Add(ScrollBarLbl);

            Button ScrollBarCursorBtn = new Button();
            ScrollBarCursorBtn.FlatStyle = FlatStyle.Flat;
            ScrollBarCursorBtn.Width = parameters.width;
            ScrollBarCursorBtn.Height = sizeCursor;
            ScrollBarCursorBtn.Top = 0;
            ScrollBarCursorBtn.Left = 0;
            ScrollBarCursorBtn.BackColor = bgColor2;
            ScrollBarCursorBtn.ForeColor = parameters.foreColor;
            ScrollBarCursorBtn.Name = "ScrollBarVerticalCursorBtn_" + parameters.name;
            ScrollBarCursorBtn.Cursor = Cursors.Hand;

            int HeightBorderForm = parameters.form.Height - parameters.form.ClientRectangle.Height;
            int WidthBorderForm = parameters.form.Width - parameters.form.ClientRectangle.Width;

            ScrollBarCursorBtn.MouseDown += delegate (object sender, MouseEventArgs e)
            {
                mouseDown = true;
                positionControl = utility.GetPositionInForm(ScrollBarBGPnl);
                positionControl.X += WidthBorderForm + e.Location.X;
                positionControl.Y += HeightBorderForm + e.Location.Y;

            };

            ScrollBarCursorBtn.MouseUp += delegate
            {
                mouseDown = false;
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString());
                }
            };

            ScrollBarCursorBtn.MouseMove += delegate
            {
                
                if (mouseDown)
                {

                    int topCursor = (Cursor.Position.Y - positionControl.Y - parameters.form.Location.Y);
                    int leftCursor = (Cursor.Position.X - positionControl.X - parameters.form.Location.X);
                    if (topCursor >= 0 && topCursor <= (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height)) { ScrollBarCursorBtn.Top = topCursor; positionCursor = ScrollBarCursorBtn.Top; ScrollBarCursorBtn.Refresh(); }

                }
            };

            ScrollBarCursorBtn.Move += delegate 
            {
                Application.DoEvents();

                ScrollBarLbl.Top = ScrollBarCursorBtn.Top - ScrollBarLbl.Height;
            };

            ScrollBarBGPnl.Controls.Add(ScrollBarCursorBtn);

            Button ScrollBarPlusBtn = new Button();
            ScrollBarPlusBtn.FlatStyle = FlatStyle.Flat;
            ScrollBarPlusBtn.Width = parameters.width;
            ScrollBarPlusBtn.Height = parameters.width;
            ScrollBarPlusBtn.Top = parameters.height - parameters.width;
            ScrollBarPlusBtn.Left = 0;
            ScrollBarPlusBtn.ForeColor = parameters.foreColor;
            ScrollBarPlusBtn.BackColor = Color.Transparent;
            ScrollBarPlusBtn.Name = "ScrollBarVerticalPlusBtn_" + parameters.name;
            if (parameters.type == ScrollBarType.Directional) { ScrollBarPlusBtn.Text = "q"; ScrollBarPlusBtn.Font = new Font("Wingdings 3", Convert.ToInt32(parameters.width / textProportion)); }
            if (parameters.type == ScrollBarType.Incremental) { ScrollBarPlusBtn.Text = "-"; ScrollBarPlusBtn.Font = new Font("Arial", Convert.ToInt32(parameters.width / textProportion), FontStyle.Bold); }

            ScrollBarPlusBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ScrollBarPlusBtn.Cursor = Cursors.Hand;
            ScrollBarPlusBtn.MouseDown += delegate
            {
                mouseDown = true;
                int turn = 0;
                while (mouseDown)
                {
                    Application.DoEvents();
                    if (ScrollBarCursorBtn.Top >= 0 && ScrollBarCursorBtn.Top < (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height)) { ScrollBarCursorBtn.Top++; ScrollBarCursorBtn.Refresh(); }
                    if (turn == 0) { utility.Sleep(200); } else { utility.Sleep(5); }
                    turn++;
                }
            };

            ScrollBarPlusBtn.Click += delegate
            {
                if (mouseDown == false)
                {
                    if (ScrollBarCursorBtn.Top >= 0 && ScrollBarCursorBtn.Top < (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height)) { ScrollBarCursorBtn.Top++; ScrollBarCursorBtn.Refresh(); }
                    if (parameters.MethodNameOnChange != String.Empty)
                    {
                        bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString());
                    }
                }
            };

            ScrollBarPlusBtn.MouseUp += delegate
            {
                mouseDown = false;
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString());
                }
            };

            ScrollBarPlusBtn.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString()); }
            };

            ScrollBarPnl.Controls.Add(ScrollBarPlusBtn);

            ScrollBarCursorBtn.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString()); }
            };

            Button ScrollBarMinusBtn = new Button();
            ScrollBarMinusBtn.FlatStyle = FlatStyle.Flat;
            ScrollBarMinusBtn.Width = parameters.width;
            ScrollBarMinusBtn.Height = parameters.width;
            ScrollBarMinusBtn.Top = 0;
            ScrollBarMinusBtn.Left = 0;
            ScrollBarMinusBtn.ForeColor = parameters.foreColor;
            ScrollBarMinusBtn.BackColor = Color.Transparent;
            ScrollBarMinusBtn.Name = "ScrollBarVerticalMinusBtn_" + parameters.name;
            if (parameters.type == ScrollBarType.Directional) { ScrollBarMinusBtn.Text = "p"; ScrollBarMinusBtn.Font = new Font("Wingdings 3", Convert.ToInt32(parameters.width / textProportion)); }
            if (parameters.type == ScrollBarType.Incremental) { ScrollBarMinusBtn.Text = "+"; ScrollBarMinusBtn.Font = new Font("Arial", Convert.ToInt32(parameters.width / textProportion), FontStyle.Bold); }

            ScrollBarMinusBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ScrollBarMinusBtn.Cursor = Cursors.Hand;
            ScrollBarMinusBtn.MouseDown += delegate
            {
                mouseDown = true;
                int turn = 0;
                while (mouseDown)
                {
                    Application.DoEvents();
                    if (ScrollBarCursorBtn.Top > 0 && ScrollBarCursorBtn.Top <= (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height)) { ScrollBarCursorBtn.Top--; ScrollBarCursorBtn.Refresh(); }
                    if (turn == 0) { utility.Sleep(200); } else { utility.Sleep(5); }
                    turn++;
                }
            };

            ScrollBarMinusBtn.Click += delegate
            {
                if (mouseDown == false)
                {
                    if (ScrollBarCursorBtn.Top > 0 && ScrollBarCursorBtn.Top <= (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height)) { ScrollBarCursorBtn.Top--; ScrollBarCursorBtn.Refresh(); }
                    if (parameters.MethodNameOnChange != String.Empty)
                    {
                        bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Horizontal).ToString());
                    }
                }
            };

            ScrollBarMinusBtn.MouseUp += delegate
            {
                mouseDown = false;
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString());
                }
            };

            ScrollBarMinusBtn.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString()); }
            };

            ScrollBarPnl.Controls.Add(ScrollBarMinusBtn);

            ScrollBarBGPnl.MouseClick += delegate
            {
                Application.DoEvents();
                if (ScrollBarCursorBtn.Top >= 0 && ScrollBarCursorBtn.Top < (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height))
                {
                    ScrollBarCursorBtn.Top += 20;
                    if (ScrollBarCursorBtn.Top > (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height)) { ScrollBarCursorBtn.Top = (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height); positionCursor = ScrollBarCursorBtn.Top; }
                    if (ScrollBarCursorBtn.Top < 0) { ScrollBarCursorBtn.Top = 0; positionCursor = ScrollBarCursorBtn.Top; }
                    ScrollBarCursorBtn.Refresh();
                }
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString());
                }
            };

            ScrollBarBGPnl.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString()); }
            };

            ScrollBarLbl.MouseClick += delegate
            {
                Application.DoEvents();
                if (ScrollBarCursorBtn.Top > 0 && ScrollBarCursorBtn.Top <= (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height))
                {
                    ScrollBarCursorBtn.Top -= 20;
                    if (ScrollBarCursorBtn.Top > (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height)) { ScrollBarCursorBtn.Top = (ScrollBarBGPnl.Height - ScrollBarCursorBtn.Height); positionCursor = ScrollBarCursorBtn.Top; }
                    if (ScrollBarCursorBtn.Top < 0) { ScrollBarCursorBtn.Top = 0; positionCursor = ScrollBarCursorBtn.Top; }
                    ScrollBarCursorBtn.Refresh();
                }
                if (parameters.MethodNameOnChange != String.Empty)
                {
                    bool result = utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnChange, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString());
                }
            };

            ScrollBarLbl.Leave += delegate
            {
                Application.DoEvents();
                if (parameters.MethodNameOnLeave != String.Empty)
                { utility.CallMethodByName(parameters.classFullPathofMethod, parameters.MethodNameOnLeave, GetValue(parameters.form, parameters.name, ScrollBarOrientation.Vertical).ToString()); }
            };

            parameters.container.Controls.Add(ScrollBarPnl);

            scrollBarBGPnlHeight = ScrollBarBGPnl.Height;
            scrollBarCursorBtnHeight = ScrollBarCursorBtn.Height;
        }

    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/* 
//HOW TO USE
         private void Form1_Load(object sender, EventArgs e)
        {
            Additional.ScrollBar scrollBar1 = new Additional.ScrollBar();
            Additional.ScrollBar scrollBar2 = new Additional.ScrollBar();

            Color foreColor = Color.FromArgb(255,240, 240, 240);

            Color bgColor = Color.FromArgb(255, 80, 80, 80);
            Utility utility = new Utility();


            ScrollBarParameters parametersScrollBar1 = new ScrollBarParameters { form = this, container = panel1, name = 0, percentageValue = 100, top = 0, left = 0, width = 150, height = 20, color = bgColor, foreColor = foreColor, type = ScrollBarType.Incremental, classFullPathofMethod = utility.GetFullPathClass(this), MethodNameOnChange = "ScrollBar1_OnChange", MethodNameOnLeave = "" };
            ScrollBarParameters parametersScrollBar2 = new ScrollBarParameters { form = this, container = panel2, name = 0, percentageValue = 255, top = 0, left = 0, width = 20, height = 295, color = bgColor, foreColor = foreColor, type = ScrollBarType.Directional, classFullPathofMethod = utility.GetFullPathClass(this), MethodNameOnChange = "ScrollBar2_OnChange", MethodNameOnLeave = "" };


            scrollBar1.CreateHorizontal(parametersScrollBar1);
            scrollBar1.SetValue(100, ScrollBarOrientation.Horizontal);

            scrollBar2.CreateVertical(parametersScrollBar2);
            scrollBar2.SetValue(57, ScrollBarOrientation.Vertical);
        }

        public void ScrollBar1_OnChange(string stringValue)
        {
            //... Form1.ActiveForm.textBox1.text = stringValue;
        }
        public void ScrollBar2_OnChange(string stringValue)
        {
            //...
        } 
        public void ScrollBar1_OnLeave(string stringValue)
        {
            //...
        }
        public void ScrollBar2_OnLeave(string stringValue)
        {
            //...
        } 
*/
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
