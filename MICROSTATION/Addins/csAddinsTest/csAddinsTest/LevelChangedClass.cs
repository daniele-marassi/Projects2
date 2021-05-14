using Bentley.Interop.MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Bentley.MicroStation.WinForms;
using Bentley.MicroStation.InteropServices;

using BCOM = Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation;

namespace csAddinsTest
{
    class LevelChangedClass : ILevelChangeEvents
    {
        public void LevelChanged(MsdLevelChangeType ChangeType, Level TheLevel, ModelReference TheModel)
        {
            if (MsdLevelChangeType.AfterCreate == ChangeType ||
                MsdLevelChangeType.AfterDelete == ChangeType ||
                MsdLevelChangeType.ChangeName == ChangeType)
                PopulateLevelList();
        }
        public static void MyAddin_NewDesignFileEvent
                           (AddIn sender, AddIn.NewDesignFileEventArgs eventArgs)
        {
            if (AddIn.NewDesignFileEventArgs.When.AfterDesignFileOpen == eventArgs.WhenCode)
                PopulateLevelList();
        }
        private static void PopulateLevelList()
        {
            LevelChangedForm myLevelChangedForm = null;
            foreach (Form myForm in System.Windows.Forms.Application.OpenForms)
                if ("LevelChangedForm" == myForm.Name)
                {
                    myLevelChangedForm = (LevelChangedForm)myForm;
                    break;
                }
            if (null != myLevelChangedForm)
            {
                myLevelChangedForm.listBox1.Items.Clear();
                foreach (Level myLvl in Utilities.ComApp.ActiveDesignFile.Levels)
                    myLevelChangedForm.listBox1.Items.Add(myLvl.Name);
            }
        }
    }
}
