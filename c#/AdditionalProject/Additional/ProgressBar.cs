using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;


namespace Additional
{
    public class ProgressBar
    {
        private static int _rate = 0;
        private static string _info = null;
        private static int _countElement = 0;
        private static Color _color;
        private static Color _backColor;
        private static string _status = null;
        private static dynamic _container = null;
        private static int _interval = 100;

        private static string _currentStatusOption = String.Empty;

        private static System.Timers.Timer Tmr;
        private StatusOption statusOption = new StatusOption();

        public class StatusOption
        {
            public string Start { get; } = "Start";
            public string End { get; } = "End";
            public string Null { get; } = null;
        }

        private void Tmr_Set()
        {
            Tmr = new System.Timers.Timer(10000);
            Tmr.Elapsed += new ElapsedEventHandler(Tmr_Tick);
            Tmr.Interval = _interval;
            Tmr.Enabled = false;
        }

        public string Step(int countElement, string info, Color backColor, Color color, dynamic container, string status = null, int interval = 100)
        {
            Tmr.Enabled = false;
            _countElement = countElement;
            _info = info;
            _color = color;
            _backColor = backColor;
            _container = container;
            _status = status;
            _interval = interval;
            Tmr.Interval = interval;

            if (status == statusOption.Start)
            {
                _rate = 0;
                reset();
                _currentStatusOption = statusOption.Start;
                _container.Controls["ProgressBarPnl"].Visible = true;
                Tmr.Enabled = true;
            }
            else if (status == statusOption.End)
            {
                _rate = _countElement;
                Refresh();
                _currentStatusOption = statusOption.End;
            }
            else if (status == null)
            {
                if (_rate == 0)
                {
                    reset();
                    _currentStatusOption = statusOption.Start;
                    _container.Controls["ProgressBarPnl"].Visible = true;
                }
                Refresh();
            }
            return _currentStatusOption;
        }

        private void Refresh()
        {
            double value = 0;
            if (_countElement == 0) { _countElement = 1; }

            value = (((double)(_rate) / (double)(_countElement)) * 100d);
            if (value <= 100)
            {
                Label.CheckForIllegalCrossThreadCalls = false;
                for (int i = 1; i <= value; i++)
                {
                    _container.Controls["ProgressBarPnl"].Controls["ProgressBar_" + i].BackColor = _color;
                    _container.Controls["ProgressBarPnl"].Controls["ProgressBar_" + i].ForeColor = _color;
                }
                _container.Controls["ProgressBarPnl"].Controls["InfoLbl"].Text = Regex.Replace(_info, "([A-Z])", " $1").Trim();
                _container.Controls["ProgressBarPnl"].Controls["InfoLbl"].Refresh();
            }
            _rate++;
            if (value >= 100)
            {
                Tmr.Enabled = false;
                Application.DoEvents();
                if (_status != statusOption.Start)
                {
                    _container.Controls["ProgressBarPnl"].Visible = false;
                    _currentStatusOption = statusOption.End;
                }
                reset();
                value = 0;
                _rate = 0;
            }
            if (_status == statusOption.Start) Tmr.Enabled = true;
        }

        public void Create(dynamic container, int top, int left, int width, int height, Color backColor, bool infoLblVisible = true)
        {

            Panel ProgressBarPnl = new Panel();
            ProgressBarPnl.Width = 250 + width;
            ProgressBarPnl.Height = height;
            ProgressBarPnl.Top = top;
            ProgressBarPnl.Left = left;
            ProgressBarPnl.BorderStyle = BorderStyle.None;
            //ProgressBarPnl.ForeColor = backColor;
            ProgressBarPnl.Name = "ProgressBarPnl";
            if (!infoLblVisible)
            {
                ProgressBarPnl.Width = width;
            }
            container.Controls.Add(ProgressBarPnl);

            Tmr_Set();
            Label InfoLbl = new Label();
            InfoLbl.Width = 250;
            InfoLbl.Height = height;
            InfoLbl.Top = 0;
            InfoLbl.Left = 0;
            InfoLbl.BorderStyle = BorderStyle.None;
            InfoLbl.Font = new System.Drawing.Font("Verdana", 10);
            InfoLbl.ForeColor = backColor;
            InfoLbl.Name = "InfoLbl";
            if (!infoLblVisible)
            {
                InfoLbl.Width = 0;
                InfoLbl.Visible = false;
            }
            container.Controls["ProgressBarPnl"].Controls.Add(InfoLbl);

            if (width < 100) { width = 100; }
            for (int i = 0; i < 100; i++)
            {
                Label ProgressBar = new Label();
                ProgressBar.Width = (width / 100);
                ProgressBar.Height = height;
                ProgressBar.Top = 0;
                ProgressBar.Left = InfoLbl.Width + ((width / 100) * i);
                ProgressBar.BorderStyle = BorderStyle.None;
                ProgressBar.BackColor = backColor;
                ProgressBar.ForeColor = backColor;
                ProgressBar.Name = $"ProgressBar_{i + 1}";
                container.Controls["ProgressBarPnl"].Controls.Add(ProgressBar);
            }
        }
        private void Tmr_Tick(object source, ElapsedEventArgs e)
        {
            Tmr.Enabled = false;
            Refresh();
        }
        private void reset()
        {
            for (int i = 1; i <= 100; i++)
            {
                Application.DoEvents();
                _container.Controls["ProgressBarPnl"].Controls["ProgressBar_" + i].BackColor = _backColor;
                _container.Controls["ProgressBarPnl"].Controls["ProgressBar_" + i].ForeColor = _backColor;
            }
        }
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/* 
//HOW TO USE
private ProgressBar progressBar = new ProgressBar();
Color backForeColor = Color.FromArgb(255, 30, 30, 30);
progressBar.Create(*** CONTAINER ***, 2, 0, 200, 20, backForeColor);

public void ProgressBar(int countElement, string info, Form form, string status = null)
{
    Color color = Color.FromArgb(255, 255, 0, 0);
    Color backColor = Color.FromArgb(255, 100, 0, 0);
    ProgressBar.StatusOption statusOption = new ProgressBar.StatusOption();
    if (status == statusOption.Start) { ProgressBarStart(); }
    string currentStatusOption = progressBar.Step(countElement, info, backColor, color, form.Controls["*** NAME CONTAINER ***"], status);
    if (currentStatusOption == statusOption.End) { ProgressBarEnd(); }
}

public void ProgressBarStart()
{
    Form1 form1 = new Form1();
    form1.ProgressBarStart();
}

public void ProgressBarEnd()
{
    Form1 form1 = new Form1();
    form1.ProgressBarEnd();
}

////////////////////////////////////
Common common = new Common();
Utility utility = new Utility();
StatusOption statusOption = new StatusOption();
common.ProgressBar(100, utility.GetCurrentMethod().Name, form, statusOption.Start);
             
var data = GetData();

common.ProgressBar(100, utility.GetCurrentMethod().Name, form, statusOption.End);

////////////////////////////////////
Common common = new Common();
Utility utility = new Utility();
List<string> list = new List<string>(){ };
if (list.Count() > 0) common.ProgressBar(list.Count(), utility.GetCurrentMethod().Name, form);
for (int i = 0; i < list.Count(); i++)
{
    var data += "," + list[i];
    common.ProgressBar(list.Count(), utility.GetCurrentMethod().Name, form);
}
////////////////////////////////////

*/
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////