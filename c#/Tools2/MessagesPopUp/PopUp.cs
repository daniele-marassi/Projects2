using System;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Additional;

namespace Tools.MessagesPopUp
{
    public partial class PopUp : Form
    {
        private string[] _args;
        private static System.Timers.Timer tmr = null;
        private static System.Timers.Timer tmrAnimationStart = null;
        private static System.Timers.Timer tmrAnimationEnd = null;
        private Argument parameters;
        private int height = 0;
        private int width = 0;
        private int _height = 0;
        private int _width = 0;
        private double _heightCoef = 0;
        private double _widthCoef = 0;

        public PopUp(string[] args)
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            _args = args;

            var json = _args.Count() > 0 ? _args[0]: "";

            json = json.Replace(@"@DOUBLEQUOTES@", @"""");
            json = json.Replace(@"@SPACE@", @" ");

            var _json = @"{""Title"":""title"",""Message"":""message111111"",""Type"":""Error"",""TimeToClosePopUpInMilliseconds"":200000000}";

            //json = _json;

            try
            {
                parameters = new JavaScriptSerializer().Deserialize<Argument>(json);

                var processAlreadyActive = ProcessAlreadyActive();

                if (processAlreadyActive > 0)
                {
                    parameters.TimeToClosePopUpInMilliseconds = parameters.TimeToClosePopUpInMilliseconds * processAlreadyActive;
                }

                Tmr_Set();
                TmrAnimationStart_Set();
                TmrAnimationEnd_Set();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arguments missed or invalid: this is the format: " + _json + ". Is necessary replace all 'doublequotes' with '@DOUBLEQUOTES@' and 'spaces' with '@SPACE@'.", "MessagePopUp");
                System.Environment.Exit(0);
            }
        }

        private void Tmr_Set()
        {
            tmr = new System.Timers.Timer(parameters.TimeToClosePopUpInMilliseconds);
            tmr.Elapsed += new ElapsedEventHandler(Tmr_Tick);
            tmr.Interval = parameters.TimeToClosePopUpInMilliseconds;
            tmr.Enabled = false;
        }

        private void Tmr_Tick(object source, ElapsedEventArgs e)
        {
            tmr.Enabled = false;
            tmrAnimationEnd.Enabled = true;
        }

        private void TmrAnimationStart_Set()
        {
            tmrAnimationStart = new System.Timers.Timer(10000);
            tmrAnimationStart.Elapsed += new ElapsedEventHandler(TmrAnimationStart_Tick);
            tmrAnimationStart.Interval = 10;
            tmrAnimationStart.Enabled = false;
        }

        private void TmrAnimationStart_Tick(object source, ElapsedEventArgs e)
        {
            _height -=(int) _heightCoef;
            if (_height < 0) _height = 0;

            _width -= (int)_widthCoef;
            if (_width < 0) _width = 0;

            LocationForm();

            if (_height == 0)
            {
                tmrAnimationStart.Enabled = false;
                tmr.Enabled = true;
            }
        }

        private void TmrAnimationEnd_Set()
        {
            tmrAnimationEnd = new System.Timers.Timer(10000);
            tmrAnimationEnd.Elapsed += new ElapsedEventHandler(TmrAnimationEnd_Tick);
            tmrAnimationEnd.Interval = 10;
            tmrAnimationEnd.Enabled = false;
        }

        private void TmrAnimationEnd_Tick(object source, ElapsedEventArgs e)
        {
            _height += (int)_heightCoef;
            if (_height > height) _height = height;

            _width += (int)_widthCoef;
            if (_width > width) _width = width;

            LocationForm();

            if (_height == height)
            {
                tmrAnimationEnd.Enabled = false;
                System.Environment.Exit(0);
            }
        }

        private void LocationForm()
        {
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - width + _width,
                                   Screen.PrimaryScreen.WorkingArea.Height - height + _height);
        }

        public int ProcessAlreadyActive()
        {
            var result = 0;
            try
            {
                Process[] process = Process.GetProcessesByName(nameof(MessagesPopUp));

                if (process.Count() > 1)
                {
                    process = process.OrderBy(_ => _.Id).ToArray();
                    var firstProcess = process.FirstOrDefault();

                    var utility = new Utility();
                    utility.SetFocusByProcess(firstProcess);

                }

                result = process.Count();
            }
            catch (Exception)
            {
            }

            return result;
        }

        private void PopUp_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;

            height = int.Parse(this.Height.ToString());
            width = int.Parse(this.Width.ToString());
            _height = int.Parse(this.Height.ToString());
            _width = int.Parse(this.Width.ToString());

            _heightCoef = _height / 10d;
            _widthCoef = _width / 10d;

            LocationForm();
            this.Title.Text = parameters.Title;

            var color = Color.FromArgb(255, 100, 192, 100);

            if (parameters.Type == MessageType.Info.ToString()) color = Color.FromArgb(255, 100, 192, 100);
            if (parameters.Type == MessageType.Error.ToString()) color = Color.FromArgb(255, 192, 100, 100);

            this.Title.BackColor = color;
            this.Ico.BackColor = color;

            this.Message.Text = parameters.Message;

            this.Message.SelectionStart = this.Message.Text.Length;
            tmrAnimationStart.Enabled = true;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
    }
}
