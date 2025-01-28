using Additional;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RepositionSpeech
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                //var appSettings = ConfigurationManager.AppSettings;
                //var windowWidth = int.Parse(appSettings["test"]);

                var _args = "";

                foreach (var arg in args)
                {
                    if (_args != "") _args += " ";
                     _args += arg;
                }

                //_args = "WindowWidth:0, WindowHeight:0, WindowCaption:window caption, FullScreen:true, Hide:false";
                args = _args.Split(',');

                var utilty = new Utility();
                var par = new Dictionary<string, string>() { };

                foreach (var arg in args)
                {
                    var sptit = arg.Split(':');
                    par[sptit[0].Trim().ToLower()] = sptit[1].Trim();
                }

                var windowWidth = int.Parse(par["WindowWidth".ToLower()]);
                var windowHeight = int.Parse(par["WindowHeight".ToLower()]);
                var windowCaption = par["WindowCaption".ToLower()];
                var fullScreen = bool.Parse(par["FullScreen".ToLower()]);
                var hide = bool.Parse(par["Hide".ToLower()]);

                var workingAreaWidth = Screen.PrimaryScreen.WorkingArea.Width;
                var workingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;
                var screenWidth = Screen.PrimaryScreen.Bounds.Width;
                var screenHeight = Screen.PrimaryScreen.Bounds.Height;

                try
                {
                    if (!hide)
                    {
                        if (fullScreen) utilty.MoveExtWindow(windowCaption, -10, -40, workingAreaWidth + 20, workingAreaHeight + 50);
                        else utilty.MoveExtWindow(windowCaption, (workingAreaWidth - windowWidth) + 10, (workingAreaHeight - windowHeight) + 10, windowWidth, windowHeight);
                    }
                    else
                    {
                        var windowX = 0;
                        var windowY = workingAreaHeight;

                        utilty.MoveExtWindow(windowCaption, windowX, windowY, windowWidth, windowHeight);
                    }

                }
                catch (System.Exception)
                {
                } 
            }
            catch (System.Exception ex)
            {
                var error = "Invalid parameters: correct use - WindowWidth:0, WindowHeight:0, WindowCaption:window caption, FullScreen:true, Hide:false";
                System.Windows.MessageBox.Show(error);
                Console.WriteLine(error);
            }
        }
    }
}
