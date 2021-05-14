using Bentley.MicroStation;
using Bentley.MicroStation.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace csAddinsTest
{
    class LoadForms
    {
        public static LevelChangedClass myLevelChanged = null;
        public static AddIn.NewDesignFileEventHandler myNewDGNHandler = null;
        private static LevelChangedForm myLevelForm = null;
        private static OutputList outputList = new OutputList();

        public static void LevelChanged(string unparsed)
        {
            if (null == myLevelForm || myLevelForm.IsDisposed)
            {
                myLevelForm = new LevelChangedForm();
                myLevelForm.AttachAsTopLevelForm(AddinsTest.s_addin, false);
                myLevelForm.Show(); 
                myLevelChanged = new LevelChangedClass();
                Utilities.ComApp.AddLevelChangeEventsHandler(myLevelChanged); 
                myNewDGNHandler = new AddIn.NewDesignFileEventHandler(LevelChangedClass.MyAddin_NewDesignFileEvent);
                AddinsTest.s_addin.NewDesignFileEvent += myNewDGNHandler;
            }
            else
                myLevelForm.Activate();
        }

        public static void Toolbar(string unparsed)
        {
            ToolbarForm myForm = new ToolbarForm();
            myForm.AttachAsGuiDockable(AddinsTest.s_addin, "toolbar");
            myForm.Width = 200;
            myForm.Show();
        }

        public static void ToolSettings(string unparsed)
        {
            ToolSettings myForm = new ToolSettings();
            myForm.AttachAsTopLevelForm(AddinsTest.s_addin, false);
            myForm.Show();
        }

        public static void OutputList(string unparsed)
        {
            outputList = new OutputList();
            outputList.AttachAsTopLevelForm(AddinsTest.s_addin, false);
            outputList.Show();
        }

        public static void AddItemOutputList(string value)
        {
            outputList.AddItem(value);
        }
    }
}