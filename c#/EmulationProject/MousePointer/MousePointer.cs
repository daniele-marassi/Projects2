using Additional;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emulation
{
    public partial class MousePointer : Form
    {
        private static Emulation.MouseClicking mouseClicking = new Emulation.MouseClicking(10, default(Point), Color.FromArgb(255,255,255,255));
        private static Emulation.Tooltip tooltip;
        private static int _size = 0;
        private static Color _color = new Color();
        private static bool _visible = false;

        private static PrivateFontCollection fonts { get; set; } = new PrivateFontCollection();
        private static string mainPath = null;
        public MousePointer(int size = 30, Color color = new Color(), bool visible = false)
        {
            InitializeComponent();
            _size = size;
            _color = color;
            _visible = visible;
            if (_color == new Color())
            {
                _color = Color.FromArgb(255, 255, 255, 255);
            }
        }

        private void MousePointer_Load(object sender, EventArgs e)
        {
            Utility utility = new Utility();
            mainPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\').ToString();

            fonts.AddFontFile($@"{mainPath}\\Resources\\Fonts\\Cursors.ttf");

            int fontSize = Convert.ToInt32(_size * 1.10);

            this.Pointer.Font = utility.GetFonts("Cursors", fontSize, fonts);
            this.Pointer.ForeColor = _color;
            this.Pointer.Visible = _visible;
            this.TransparencyKey = Color.Gray;
            this.BackColor = Color.Gray;

            this.Width = Pointer.Width;
            this.Height = Pointer.Height;

            this.Location = new Point(100, 100);
            mouseClicking.Show();
            RepositionMouseClicking();
        }

        public void MoveToControl(Form form, string controlName)
        {
            Utility utility = new Utility();

            Control control;
            Point positionControl;
            control = utility.GetControlByChildName(form, controlName);
            positionControl = utility.GetPositionInForm(control);

            int HeightBorderForm = form.Height - form.ClientRectangle.Height;
            int WidthBorderForm = form.Width - form.ClientRectangle.Width;

            Point newPosition = new Point((positionControl.X + form.Location.X + WidthBorderForm) + Convert.ToInt32((control.Width / 2)) - 13, (positionControl.Y + form.Location.Y + HeightBorderForm) + Convert.ToInt32(control.Height / 2) - 8);

            int x = 0;
            int y = 0;

            bool mousePointerPositionatedX = false;
            bool mousePointerPositionatedY = false;
            this.Focus();
            while (mousePointerPositionatedX == false || mousePointerPositionatedY == false)
            {
                Application.DoEvents();
                if (mousePointerPositionatedX == false)
                {
                    if (this.Location.X <= newPosition.X) { x = 1; } else { x = -1; }
                    this.Location = new Point(this.Location.X + x, this.Location.Y);
                    if (this.Location.X == newPosition.X) { mousePointerPositionatedX = true; }
                }

                if (mousePointerPositionatedY == false)
                {
                    if (this.Location.Y <= newPosition.Y) { y = 1; } else { y = -1; }
                    this.Location = new Point(this.Location.X, this.Location.Y + y);
                    if (this.Location.Y == newPosition.Y) { mousePointerPositionatedY = true; }
                }
                RepositionMouseClicking();
                utility.Sleep(1);
            }

        }

        public void MoveToLeft(int left)
        {
            Utility utility = new Utility();

            Point newPosition = new Point(this.Location.X + left, this.Location.Y );

            int x = 0;
            int y = 0;

            bool mousePointerPositionatedX = false;
            bool mousePointerPositionatedY = false;
            this.Focus();
            while (mousePointerPositionatedX == false || mousePointerPositionatedY == false)
            {
                Application.DoEvents();
                if (mousePointerPositionatedX == false)
                {
                    if (this.Location.X <= newPosition.X) { x = 1; } else { x = -1; }
                    this.Location = new Point(this.Location.X + x, this.Location.Y);
                    if (this.Location.X == newPosition.X) { mousePointerPositionatedX = true; }
                }

                if (mousePointerPositionatedY == false)
                {
                    if (this.Location.Y <= newPosition.Y) { y = 1; } else { y = -1; }
                    this.Location = new Point(this.Location.X, this.Location.Y + y);
                    if (this.Location.Y == newPosition.Y) { mousePointerPositionatedY = true; }
                }
                RepositionMouseClicking();
                utility.Sleep(1);
            }
        }

        private void RepositionMouseClicking()
        {
            mouseClicking.Location = new Point(this.Location.X - Convert.ToInt32(mouseClicking.Width / 3.8), this.Location.Y - Convert.ToInt32(mouseClicking.Height / 2.0));
        }

        public void MoveToTop(int top)
        {
            Utility utility = new Utility();

            Point newPosition = new Point(this.Location.X, this.Location.Y + top);

            int x = 0;
            int y = 0;

            bool mousePointerPositionatedX = false;
            bool mousePointerPositionatedY = false;
            this.Focus();
            while (mousePointerPositionatedX == false || mousePointerPositionatedY == false)
            {
                Application.DoEvents();
                if (mousePointerPositionatedX == false)
                {
                    if (this.Location.X <= newPosition.X) { x = 1; } else { x = -1; }
                    this.Location = new Point(this.Location.X + x, this.Location.Y);
                    if (this.Location.X == newPosition.X) { mousePointerPositionatedX = true; }
                }

                if (mousePointerPositionatedY == false)
                {
                    if (this.Location.Y <= newPosition.Y) { y = 1; } else { y = -1; }
                    this.Location = new Point(this.Location.X, this.Location.Y + y);
                    if (this.Location.Y == newPosition.Y) { mousePointerPositionatedY = true; }
                }
                RepositionMouseClicking();
                utility.Sleep(1);
            }
        }

        public void ClickDown(bool soundActive = false)
        {
            mouseClicking.Focus();
            this.Focus();
            RepositionMouseClicking();
            mouseClicking.Down(soundActive);
        }

        public void ClickUp(bool soundActive = false)
        {
            mouseClicking.Focus();
            this.Focus();
            RepositionMouseClicking();
            mouseClicking.Up(soundActive);
        }

        public void ShowTooltip(string text, Color foreColor = new Color(), Color backColor = new Color())
        {
            tooltip = new Emulation.Tooltip();
            tooltip.Show();
            tooltip.Focus();
            Point position = new Point { X = this.Left + this.Width, Y = this.Top + this.Height };
            tooltip.ShowTooltip(text,position, foreColor,backColor);
            this.Focus();
        }

        public void MoveTooltip()
        {
            tooltip.Focus();
            Point position = new Point { X = this.Left + this.Width, Y = this.Top + this.Height };
            tooltip.Move(position);
            this.Focus();
        }

        public void CloseMousePointer()
        {
            tooltip.Close();
            mouseClicking.Close();
            this.Close();
        }

        public void CloseTooltip()
        {
            tooltip.Close();
        }
    }
}
