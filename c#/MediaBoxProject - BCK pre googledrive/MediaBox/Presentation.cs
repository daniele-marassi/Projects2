using Additional;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MediaBox
{
    public class Presentation
    {
        public void NextMedia()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int range = rnd.Next(2, 8); ;
            if (range > (Statics.Presentation.CountElements / 4)) { range = (Statics.Presentation.CountElements / 4); }
           
            
            Statics.Presentation.Next = rnd.Next(0, Statics.Presentation.CountElements);


            if (Statics.Presentation.Views == null) { Statics.Presentation.Views = new List<int>() { }; }

            if (Statics.Presentation.Views.Count == Statics.Presentation.CountElements)
            {
                Statics.Presentation.Views.Clear();
            }
            else
            {
                while (Statics.Presentation.Views.Where(_ => _ == Statics.Presentation.Next).Count() > 0 ? true : false)
                {
                    Statics.Presentation.Next = rnd.Next(0, Statics.Presentation.CountElements);
                }
            }
            Statics.Presentation.ImagesSequence = false;
            rnd = new Random(DateTime.Now.Millisecond);
            if (rnd.Next(0, 2) == 1)
            {
                int index = Statics.Presentation.Next;
                int newIndex = index;
                if (Statics.DB.TblCatalog.Data.Rows.Count == 0) { return; }
                DataRow row = Statics.DB.TblCatalog.Data.Rows[index];

                DateTimeOffset currentDateCreated = DateTimeOffset.Parse(((dynamic)row["DateCreated"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK"));
                DateTimeOffset preiousDateCreated = currentDateCreated;

                string currentPath = Path.GetDirectoryName(row["PathFile"].ToString());
                string previousPath = String.Empty;
                Utility utility = new Utility();

                bool isNearTimes = true;

                while (isNearTimes)
                {
                    isNearTimes = false;
                    previousPath = String.Empty;
                    newIndex--;
                    if (newIndex >= 0)
                    {
                        row = Statics.DB.TblCatalog.Data.Rows[newIndex];
                        
                        preiousDateCreated = DateTimeOffset.Parse(((dynamic)row["DateCreated"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK"));
                        previousPath = Path.GetDirectoryName(row["PathFile"].ToString());
                        if (
                            utility.DiffInMillisecondsBetweenTwoDate(currentDateCreated, preiousDateCreated) <= Statics.Presentation.MaximumDifferenceBetweenImagesInMilliseconds 
                            && currentDateCreated != preiousDateCreated
                            && previousPath == currentPath
                            )
                        { currentDateCreated = preiousDateCreated; Statics.Presentation.Next = newIndex; isNearTimes = true; Statics.Presentation.ImagesSequence = true; }
                    }
                }
            }

            Statics.Presentation.Previous = Statics.Presentation.Next - range;

            if (Statics.Presentation.Previous < 0) { Statics.Presentation.Previous = 0; }

            Statics.Presentation.Between = Statics.Presentation.Previous;

            if (Statics.FormParam.DemoRuning == true && Statics.FormParam.DemoFirstMedia == true)
            {
                Statics.FormParam.DemoFirstMedia = false;
                int indexWelcome = -1;
                for (int i = 0; i < Statics.DB.TblCatalog.Data.Rows.Count; i++)
                {
                    if (Statics.DB.TblCatalog.Data.Rows[i][1].ToString().Contains("WelcomeToMediaBoxExperience"))
                    { indexWelcome = i; break; }
                }
                if (indexWelcome == -1) { indexWelcome = 0; }
                Statics.Presentation.Next = indexWelcome;
                Statics.Presentation.Previous = indexWelcome;
                Statics.Presentation.Between = indexWelcome - 1;
            } 
        }

        public void DeselectPictureBox(PictureBox pictureBox, int index)
        {
            if (index < 0 || index > Statics.DB.TblCatalog.Data.Rows.Count-1) { return; }
            DataRow row = Statics.DB.TblCatalog.Data.Rows[index];
            int height = Convert.ToInt32(row["HeightThumbnail"]);
            if (pictureBox != null)
            {
                pictureBox.BorderStyle = BorderStyle.None;
                pictureBox.BackColor = Color.Transparent;
                pictureBox.Height = height;
            }
        }

        public void SelectPictureBox(PictureBox pictureBox, int index)
        {
            if (index < 0 || index > Statics.DB.TblCatalog.Data.Rows.Count-1) { return; }
            DataRow row = Statics.DB.TblCatalog.Data.Rows[index];
            int height = Convert.ToInt32(row["HeightThumbnail"]);
            if (pictureBox != null)
            {
                pictureBox.BorderStyle = BorderStyle.FixedSingle;
                pictureBox.BackColor = Statics.FormParam.ForeColor;
                pictureBox.Height = height + 4;
            }
        }

        public bool Execution(Form form)
        {
            bool founded = false;
            if (Statics.Presentation.Running == true) { return founded; }
            Statics.Presentation.Running = true;
            Utility utility = new Utility();
            PictureBox pictureBox = null;
            PictureBox pictureBoxPrevious = null;

            if (Statics.Presentation.Between == Statics.Presentation.Next)
            {
                var rnd = new Random(DateTime.Now.Millisecond);
                //if (MediaBoxStatic.Presentation.CountFounded > 0 || rnd.Next(0, 2) == 1)
                if(1 == 1)
                {
                    founded = true;
                }
                else
                {
                    Statics.Presentation.Between = 0;
                    NextMedia();
                }
                Statics.Presentation.CountFounded++;
            }
            
            if (Statics.Presentation.Previous <= Statics.Presentation.Next)
            {
                pictureBoxPrevious = (PictureBox)Statics.MainForm.Controls["PicturePnl"].Controls[$"PictureBox_{Statics.Presentation.Between}"];
                DeselectPictureBox(pictureBoxPrevious, Statics.Presentation.Between);
                if (founded == false) { Statics.Presentation.Between++; }
            }
            if (Statics.Presentation.Previous > Statics.Presentation.Next)
            {
                pictureBoxPrevious = (PictureBox)Statics.MainForm.Controls["PicturePnl"].Controls[$"PictureBox_{Statics.Presentation.Between}"];
                DeselectPictureBox(pictureBoxPrevious, Statics.Presentation.Between);
                if (founded == false) { Statics.Presentation.Between--; }
            }
            Application.DoEvents();
            pictureBox = (PictureBox)Statics.MainForm.Controls["PicturePnl"].Controls[$"PictureBox_{Statics.Presentation.Between}"];
            Statics.Presentation.Selected = Statics.Presentation.Between;
            if (pictureBox == null) { return false; }
            
            Dictionary<string, int> location = utility.GetCenterLocation(Statics.FormParam.WidthArea, Statics.FormParam.HeightArea, pictureBox.Width, pictureBox.Height, pictureBox.Top, pictureBox.Left, 100);

            int centerTop = ( location["Top"]) - pictureBox.Top;
            int centerLeft = ( location["Left"]) - pictureBox.Left;
            int newTop = 0;
            int newLeft = 0;
            int width = Statics.MainForm.Controls["PicturePnl"].Width;
            int height = Statics.MainForm.Controls["PicturePnl"].Height;

            newTop = Statics.FormParam.HeightArea - height;
            newLeft = Statics.FormParam.WidthArea - width;

            if (centerTop > 0) { newTop = 0; } 
            if (centerLeft > 0) { newLeft = 0; }

            if (centerTop <= 0 && (Math.Abs(centerTop) <= Math.Abs(Statics.FormParam.HeightArea - height) && height > Statics.FormParam.HeightArea)) { newTop = centerTop; }
            if (centerLeft <= 0 && (Math.Abs(centerLeft) <= Math.Abs(Statics.FormParam.WidthArea - width) && width > Statics.FormParam.WidthArea)) { newLeft = centerLeft; }

            int t = 0;
            int l = 0;

            int mediaMovementSpeed = Statics.FormParam.MediaMovementSpeed;

            int slowMovement = 1;

            

            for (int i = 0; i <= Math.Abs(newLeft - Statics.MainForm.Controls["PicturePnl"]?.Left ?? 0); i++)
            {
                Application.DoEvents();
                Services.ServiceMediaMovementSpeed(Statics.MainForm);
                if (mediaMovementSpeed != Statics.FormParam.MediaMovementSpeed) { break; }

                int sleep = 0;
                double _move = (double)(mediaMovementSpeed - 66) * Statics.MainForm.Controls["PicturePnl"].Width / 100d;
                int move = Convert.ToInt32(_move);

                if (mediaMovementSpeed >= 0 && mediaMovementSpeed < 23)
                {
                    move = 1;
                    sleep = 2300 - (mediaMovementSpeed * 100);
                }
                else if (mediaMovementSpeed >= 23 && mediaMovementSpeed < 33)
                {
                    move = 1;
                    sleep =  90- ((mediaMovementSpeed-23) * 10 );
                    if (sleep == 0) { sleep = 1; }
                }
                else if (mediaMovementSpeed >= 33 && mediaMovementSpeed < 66)
                {
                    move = Convert.ToInt32(mediaMovementSpeed);
                    sleep = 40;
                }
                else if (mediaMovementSpeed >= 66 && mediaMovementSpeed <= 100)
                {
                    move = Convert.ToInt32(_move);
                    sleep = 20;
                }

                int maxHorizontalMovement = Math.Abs(newLeft - Statics.MainForm.Controls["PicturePnl"]?.Left ?? 0);
                if (maxHorizontalMovement > 20 && i <= 10) {
                    slowMovement = slowMovement * 2; if (slowMovement < move) move = slowMovement; }
                else if (maxHorizontalMovement > 20 && i >= (maxHorizontalMovement - 10)) {
                    slowMovement = slowMovement / 2; if(slowMovement < move) move = slowMovement; }

                l = l + move;
                if (l > Math.Abs(newLeft - Statics.MainForm.Controls["PicturePnl"]?.Left ?? 0))
                { l = Math.Abs(newLeft - Statics.MainForm.Controls["PicturePnl"]?.Left ?? 0); ; }

                if (Statics.MainForm.Controls["PicturePnl"] != null && Statics.Presentation.Active == true)
                {
                    if (newLeft > Statics.MainForm.Controls["PicturePnl"].Left)
                    { Statics.MainForm.Controls["PicturePnl"].Left += l; }
                    else { Statics.MainForm.Controls["PicturePnl"].Left -= l; }
                    utility.Sleep(sleep);
                }
                else { break; }
            }

            slowMovement = 1;
            

            for (int i = 0; i <= Math.Abs(newTop - Statics.MainForm.Controls["PicturePnl"]?.Top ?? 0); i++)
            {
                Application.DoEvents();
                Services.ServiceMediaMovementSpeed(Statics.MainForm);
                if (mediaMovementSpeed != Statics.FormParam.MediaMovementSpeed) { break; }
                int sleep = 0;

                double _move = (double)(mediaMovementSpeed - 66) * Statics.MainForm.Controls["PicturePnl"].Height / 100d;
                int move = Convert.ToInt32(_move);

                if (mediaMovementSpeed >= 0 && mediaMovementSpeed < 23)
                {
                    move = 1;
                    sleep = 2300 - (mediaMovementSpeed * 100);
                }
                else if (mediaMovementSpeed >= 23 && mediaMovementSpeed < 33)
                {
                    move = 1;
                    sleep = 90 - ((mediaMovementSpeed - 23) * 10);
                    if (sleep == 0) { sleep = 1; }
                }
                else if (mediaMovementSpeed >= 33 && mediaMovementSpeed < 66)
                {
                    move = Convert.ToInt32(mediaMovementSpeed);
                    sleep = 40;
                }
                else if (mediaMovementSpeed >= 66 && mediaMovementSpeed <= 100)
                {
                    move = Convert.ToInt32(_move);
                    sleep = 20;
                }

                int maxVerticalMovement = Math.Abs(newTop - Statics.MainForm.Controls["PicturePnl"]?.Top ?? 0);
                if (maxVerticalMovement > 20 && i <= 10) { slowMovement = slowMovement * 2; if (slowMovement < move) move = slowMovement; }
                else if (maxVerticalMovement > 20 && i >= (maxVerticalMovement - 10)) { slowMovement = slowMovement / 2; if (slowMovement < move) move = slowMovement; }

                t = t + move;
                if (t > Math.Abs(newTop - Statics.MainForm.Controls["PicturePnl"]?.Top ?? 0))
                { t = Math.Abs(newTop - Statics.MainForm.Controls["PicturePnl"]?.Top ?? 0); ; }
                if (Statics.MainForm.Controls["PicturePnl"] != null && Statics.Presentation.Active == true)
                {
                    if (newTop > Statics.MainForm.Controls["PicturePnl"].Top)
                    { Statics.MainForm.Controls["PicturePnl"].Top += t; }
                    else { Statics.MainForm.Controls["PicturePnl"].Top -= t; }
                    utility.Sleep(sleep);
                }
                else { break; }

            }

            if (Statics.Presentation.Running == true && Statics.Presentation.Active == true)
            {
                if (founded == false) { utility.Sleep(500); }
                
                DeselectPictureBox(pictureBox, Statics.Presentation.Between);
                SelectPictureBox(pictureBox, Statics.Presentation.Between);
                Application.DoEvents();
                if (founded == false) { utility.Sleep(1000); }
            }

            if (founded == true && Statics.Presentation.Active == true)
            {
                Statics.Presentation.Views.Add(Statics.Presentation.Next);
                Media media = new Media();
                Statics.Media.Index = Statics.Presentation.Next;
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();
                media.StartWait();

                bool exists = media.InizializeAnimate(Statics.MainForm, pictureBox.Top, pictureBox.Left, pictureBox.Width, pictureBox.Height);
                if (exists == true)
                {
                    Statics.MainForm = form;
                    media.OpenMedia(Statics.Presentation.Next, Statics.MainForm, pictureBox.Top, pictureBox.Left, pictureBox.Width, pictureBox.Height, true);
                    DeselectPictureBox(pictureBox, Statics.Presentation.Between);

                    Statics.Presentation.CountFounded = 0;
                    Statics.Presentation.Between = 0; 
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    media.StopWait();
                    founded = false;
                }
            }

            Statics.Presentation.Running = false;
            return founded;
        }

        public void StartStopPresentationTmr()
        {
            if (Statics.Presentation.Start == true)
            { Statics.Presentation.Start = false;}
            else { Statics.Presentation.Start = true; }
        }

        public void ActiveDeactivatePresentation()
        {
            if (Statics.Presentation.Active == true)
            { Statics.Presentation.Active = false; }
            else { Statics.Presentation.Active = true; }
        }
    }
}
