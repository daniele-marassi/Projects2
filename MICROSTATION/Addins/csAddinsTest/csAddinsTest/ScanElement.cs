using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMI = Bentley.MicroStation.InteropServices;
using BCOM = Bentley.Interop.MicroStationDGN;
using System.Windows.Forms;
using Bentley.MicroStation.InteropServices;
using Bentley.Interop.MicroStationDGN;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace csAddinsTest
{
    class ScanElement
    {
        public static void Utility(string unparsed)
        {

            /*///////////////////////////////////////////*/

            ///*

            RunBatchConvert();
            //*/


            /*///////////////////////////////////////////*/

            /*
            var elementsActiveModelIds = new List<long>() { };
            var cellActiveModelIds = new List<long>() { };
            var elementsInToCellActiveModelIds = new List<long>() { };
            var elementsAttachmentModelIds = new List<long>() { };
            var cellAttachmentModelIds = new List<long>() { };
            var elementsInToCellAttachmentModelIds = new List<long>() { };

            ScanAllElements(elementsActiveModelIds, cellActiveModelIds, elementsInToCellActiveModelIds, elementsAttachmentModelIds, cellAttachmentModelIds, elementsInToCellAttachmentModelIds);
            //*/

            /*///////////////////////////////////////////*/

            /*
            BCOM.Application msApp = BMI.Utilities.ComApp;
            BCOM.Point3d org = msApp.Point3dZero();

            msApp.ActiveModelReference.Attachments.Add(@"C:\Users\Administrator\Documents\JVPC\dms00068\006271-ACS10111B-HVAC.dgn", "Default", null, null, ref org, ref org);
            //*/

            /*///////////////////////////////////////////*/

            /*
            var elementsToCopyIds = new List<long>() {12502 };
            var levelName = "LevelTEST3";

            CopyElementInSpecificLevel(elementsToCopyIds, levelName);
            //*/

            /*///////////////////////////////////////////*/

            /*
            var cellName = "DECID";
            var elementsToInsertInToCellIds = new List<long>() { 620, 621, 623 };
            CreateAndFillCell(elementsToInsertInToCellIds, cellName);
            //*/

            /*///////////////////////////////////////////*/

            /*
            var cellName = "DECID";
            var libraryName = "sample2.cel";

            CreateAndFillCellWithLibrary(cellName, libraryName);
            //*/

            /*///////////////////////////////////////////*/


        }

        public static void RunBatchConvert()
        {
            Bentley.Interop.MicroStationDGN.Application app = null;
            app = Utilities.ComApp;
            app.CadInputQueue.SendKeyin(@"mdl load batchconvert C:\BentleyV8i\MicroStation\mdlapps\BCNVfile.bcnv");
            app.CadInputQueue.SendKeyin("batchconvert process");
            MessageBox.Show("Run BatchConvert");

        }

        public static void CopyElementInSpecificLevel(List<long> elementsToCopyIds, string levelName)
        {
            if (AddinsTest.app.Visible == true) LoadForms.OutputList(String.Empty);
            BCOM.ElementScanCriteria esc = new BCOM.ElementScanCriteriaClass();
            BCOM.ElementEnumerator ee = BMI.Utilities.ComApp.ActiveModelReference.Scan(esc);
            List<Element> elements = new List<Element>() { };
            var ids = String.Empty;
            while (ee.MoveNext())
            {
                var elem = ee.Current;
                var id = ee.Current.ID;

                var metaDataString = String.Empty;
                var exePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

                if ((elementsToCopyIds.Count > 0 && elementsToCopyIds.Where(_ => _ == id).Count() > 0))
                {
                    CreateElement.CopyElement(elem, levelName);
                    if (AddinsTest.app.Visible == true) LoadForms.AddItemOutputList("Id: "+ elem.ID + " - Level: " + elem.Level?.Name);
                }

            }
        }

        public static void CreateAndFillCell(List<long> elementsToInsertInToCellIds, string cellName)
        {
            if (AddinsTest.app.Visible == true) LoadForms.OutputList(String.Empty);
            BCOM.ElementScanCriteria esc = new BCOM.ElementScanCriteriaClass();
            BCOM.ElementEnumerator ee = BMI.Utilities.ComApp.ActiveModelReference.Scan(esc);
            List<Element> elements = new List<Element>() { };
            var ids = String.Empty;
            while (ee.MoveNext())
            {
                var elem = ee.Current;
                var id = ee.Current.ID;

                var metaDataString = String.Empty;
                var exePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

                if ((elementsToInsertInToCellIds.Count > 0 && elementsToInsertInToCellIds.Where(_ => _ == id).Count() > 0))
                {
                    if (ids != String.Empty) ids += ", ";
                    ids += id;
                    elements.Add(elem);
                }

            }
            if (elements.Count() > 0) 
            {
                CreateElement.CreateAndFillCell(elements, cellName);
                if (AddinsTest.app.Visible == true) LoadForms.AddItemOutputList("Id: " + ids + " - Cell: " + cellName);
            }
        }

        public static void CreateAndFillCellWithLibrary(string cellName, string libraryName)
        {
            if (AddinsTest.app.Visible == true) LoadForms.OutputList(String.Empty);
            CreateElement.CreateAndFillCellWithLibrary(cellName, libraryName);
            if (AddinsTest.app.Visible == true) LoadForms.AddItemOutputList("libraryName: " + libraryName + " - Cell: " + cellName);
        }


        public static void ScanAllElements(List<long> elementsActiveModelIds, List<long> cellActiveModelIds, List<long> elementsInToCellActiveModelIds, List<long> elementsAttachmentModelIds, List<long> cellAttachmentModelIds, List<long> elementsInToCellAttachmentModelIds)
        {
            if (AddinsTest.app.Visible == true) LoadForms.OutputList(String.Empty);

            BCOM.ElementScanCriteria esc = new BCOM.ElementScanCriteriaClass();

            BCOM.ElementEnumerator ee = BMI.Utilities.ComApp.ActiveModelReference.Scan(esc);
            List<Element> elements = new List<Element>() { };
            var ids = String.Empty;
            while (ee.MoveNext())
            {
                var elem = ee.Current;
                var id = ee.Current.ID;

                var metaDataString = String.Empty;
                var exePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

                if ((cellActiveModelIds.Count > 0 && cellActiveModelIds.Where(_ => _ == id).Count() > 0) || (cellActiveModelIds.Count == 0))
                {
                    elements = CreateElement.ScanFromCell(elem, elementsInToCellActiveModelIds);

                    foreach (var item in elements)
                    {
                        var metaData = CreateElement.GetMetaDataFromElement(item);
                        metaDataString = String.Empty;
                        foreach (var prop in metaData.GetType().GetProperties())
                        {
                            if (metaDataString != String.Empty) metaDataString += ", ";
                            metaDataString += prop.Name + " - " + prop.GetValue(metaData, null);
                        }

                        Debug.WriteLine(metaDataString);
                        if (AddinsTest.app.Visible == true) LoadForms.AddItemOutputList("ScanAllElements FromCell - " + metaDataString);


                        using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(Path.Combine(exePath, "OutputList.txt"), true))
                        {
                            file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "       " + "ScanAllElements FromCell - " + metaDataString);
                        }
                    }
                }

                if ((elementsActiveModelIds.Count > 0 && elementsActiveModelIds.Where(_ => _ == id).Count() > 0) || (elementsActiveModelIds.Count == 0 && elem.IsGraphical))
                {
                    var metaData = CreateElement.GetMetaDataFromElement(elem);
                    metaDataString = String.Empty;
                    foreach (var prop in metaData.GetType().GetProperties())
                    {
                        if (metaDataString != String.Empty) metaDataString += ", ";
                        metaDataString += prop.Name + " - " + prop.GetValue(metaData, null);
                    }

                    Debug.WriteLine(metaDataString);
                    if (AddinsTest.app.Visible == true) LoadForms.AddItemOutputList("ScanAllElements - " + metaDataString);

                    using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(Path.Combine(exePath, "OutputList.txt"), true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "       " + "ScanAllElements - " + metaDataString);
                    }
                }
            }

            ScanAllElementsFromAttachments(elementsAttachmentModelIds, cellAttachmentModelIds, elementsInToCellAttachmentModelIds);
        }

        public static void ScanAllElementsFromAttachments(List<long> elementsAttachmentModelIds, List<long> cellAttachmentModelIds, List<long> elementsInToCellAttachmentModelIds)
        {
            if (AddinsTest.app.Visible == true) LoadForms.OutputList(String.Empty);

            BCOM.ElementScanCriteria esc = new BCOM.ElementScanCriteriaClass();
            List<Element> elements = new List<Element>() { };
            var ids = String.Empty;

            foreach (Attachment attachment in BMI.Utilities.ComApp.ActiveModelReference.Attachments)
            {
                BCOM.ElementEnumerator ee = attachment.Scan(esc);

                while (ee.MoveNext())
                {
                    var elem = ee.Current;
                    var id = ee.Current.ID;

                    var metaDataString = String.Empty;
                    var exePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

                    if ((cellAttachmentModelIds.Count > 0 && cellAttachmentModelIds.Where(_ => _ == id).Count() > 0) || (cellAttachmentModelIds.Count == 0))
                    {
                        elements = CreateElement.ScanFromCell(elem, elementsInToCellAttachmentModelIds);

                        foreach (var item in elements)
                        {
                            var metaData = CreateElement.GetMetaDataFromElement(item);
                            metaDataString = String.Empty;
                            foreach (var prop in metaData.GetType().GetProperties())
                            {
                                if (metaDataString != String.Empty) metaDataString += ", ";
                                metaDataString += prop.Name + " - " + prop.GetValue(metaData, null);
                            }

                            Debug.WriteLine(metaDataString);
                            if (AddinsTest.app.Visible == true) LoadForms.AddItemOutputList("ScanAllElementsFromAttachments FromCell - " + metaDataString);


                            using (System.IO.StreamWriter file =
                                new System.IO.StreamWriter(Path.Combine(exePath, "OutputList.txt"), true))
                            {
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "       " + "ScanAllElementsFromAttachments FromCell - " + metaDataString);
                            }
                        }
                    }

                    if ((elementsAttachmentModelIds.Count > 0 && elementsAttachmentModelIds.Where(_ => _ == id).Count() > 0) || (elementsAttachmentModelIds.Count == 0 && elem.IsGraphical))
                    {
                        var metaData = CreateElement.GetMetaDataFromElement(elem);
                        metaDataString = String.Empty;
                        foreach (var prop in metaData.GetType().GetProperties())
                        {
                            if (metaDataString != String.Empty) metaDataString += ", ";
                            metaDataString += prop.Name + " - " + prop.GetValue(metaData, null);
                        }

                        Debug.WriteLine(metaDataString);
                        if (AddinsTest.app.Visible == true) LoadForms.AddItemOutputList("ScanAllElementsFromAttachments - " + metaDataString);

                        using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(Path.Combine(exePath, "OutputList.txt"), true))
                        {
                            file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "       " + "ScanAllElementsFromAttachments - " + metaDataString);
                        }
                    }
                }
            }
        }

        public static void ScanAllElements_old(string unparsed)
        {
            if (AddinsTest.app.Visible == true) LoadForms.OutputList(String.Empty);

            BCOM.ElementScanCriteria esc = new BCOM.ElementScanCriteriaClass();
            esc.ExcludeAllTypes();
            esc.IncludeType(BCOM.MsdElementType.TextNode); 
            BCOM.ElementEnumerator ee = BMI.Utilities.ComApp.ActiveModelReference.Scan(esc);
            var hierarchyLevels = String.Empty;
            var elementText = String.Empty;
            var item = String.Empty;
            var exePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            while (ee.MoveNext())
            {
                BCOM.TextNodeElement oTxtNode = ee.Current.AsTextNodeElement();
                BCOM.ElementEnumerator ee2 = oTxtNode.GetSubElements();
                while (ee2.MoveNext())
                {
                    hierarchyLevels = String.Empty;
                    hierarchyLevels = GetLevel(ee2.Current?.Level, hierarchyLevels);
                    elementText = ee2.Current.AsTextElement().Text.ToString();
                    item = "Element Name:" + elementText + " - Hierarchy Levels:" + hierarchyLevels;

                    if(AddinsTest.app.Visible == true) LoadForms.AddItemOutputList(item);

                    using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(Path.Combine(exePath, "AllElements.txt"), true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "       " + item);
                    }

                    Debug.WriteLine("### ELEMENT TEXT ### " + elementText);
                    Debug.WriteLine("### HIERARCHY LEVELS ### " + hierarchyLevels);
                }
            }
            if (AddinsTest.app.Visible == true) MessageBox.Show(MethodBase.GetCurrentMethod().Name.ToUpper() + " executed!");
        }

        private static string GetLevel(Level level, string hierarchyLevels )
        {
            if (level != null && level?.Name != null && hierarchyLevels != String.Empty) hierarchyLevels = " -> " + hierarchyLevels;
            hierarchyLevels = level?.Name.ToString() + hierarchyLevels;
            if (level.ParentLevel != null)
            {
                level = level.ParentLevel;
                GetLevel(level, hierarchyLevels);
            }

            return hierarchyLevels;
        }
    }
}
