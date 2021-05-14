using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;


using System.Net;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Diagnostics;

namespace AlertSpaceDisk
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string status = "WARNING; NOME SERVER; UNITA; SPAZIO LIBERO GB; SPAZIO TOTALE GB" + Environment.NewLine;

            status += CalculateFreeUsed("DANBTPLMPROD31");
            status += CalculateFreeUsed("Danbtplmprod43");


            status += CalculateFreeUsed("DANBTPLMPROD41");
            status += CalculateFreeUsed("Danbtplmprod45");
            status += CalculateFreeUsed("Danbtplmprod46");


            status += CalculateFreeUsed("DFERYPLMPROD41");
            status += CalculateFreeUsed("DFERYPLMPROD45");
            status += CalculateFreeUsed("DFERYPLMPROD46");


            status += CalculateFreeUsed("IBHCMPLMPROD41");
            status += CalculateFreeUsed("IBHCMPLMPROD45");
            status += CalculateFreeUsed("IBHCMPLMPROD46");


            status += CalculateFreeUsed("DCSCSPLMPROD41");
            status += CalculateFreeUsed("DCSCSPLMPROD45");
            status += CalculateFreeUsed("DCSCSPLMPROD46");


            status += CalculateFreeUsed("DILSCPLMPROD41");
            status += CalculateFreeUsed("DILSCPLMPROD45");
            status += CalculateFreeUsed("DILSCPLMPROD46");



            status += CalculateFreeUsed("DANBTPLMTEST11");
            status += CalculateFreeUsed("DANBTSRM1T");


            status += CalculateFreeUsed("SQLBT15");
            status += CalculateFreeUsed("DANBTSRM1");


            Console.WriteLine(status);
            

            string path = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),"Status.csv");
            File.WriteAllText(path, status);

            //sendEMail("In allegato lo stato dei servers", path, "AlertSpaceDisk", "d.marassi@ext.danieli.it");

            Process.Start("Excel.exe", path);

            Console.ReadLine();
        }

        //method to send email to outlook
        public static void sendEMail(string _HTMLBody, string pathAttachment, string subject, string sendTo)
        {
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                oMsg.HTMLBody = _HTMLBody;
                //Add an attachment.
                String sDisplayName = Path.GetFileNameWithoutExtension(pathAttachment);
                int iPosition = (int)oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                //now attached the file

                Outlook.Attachment oAttach = null;
                try
                {
                    if(pathAttachment != String.Empty && pathAttachment != null)
                        oAttach = oMsg.Attachments.Add(pathAttachment, iAttachType, iPosition, sDisplayName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                
                //Subject line
                oMsg.Subject = subject;
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(sendTo);
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
            }//end of try block
            catch (Exception ex)
            {
            }//end of catch
        }//end of Email Method

        private static string CalculateFreeUsed(string srvname)
        {
            string result = String.Empty;
            try
            {
                // Connection credentials to the remote computer, not needed if the logged account has access
                ConnectionOptions oConn = new ConnectionOptions();

                //oConn.Username = "MatinXie";
                //oConn.Password = "*********";
                string strNameSpace = @"\\";

                if (srvname != "")
                    strNameSpace += srvname;
                else
                    strNameSpace += ".";

                strNameSpace += @"\root\cimv2";

                ManagementScope oMs = new ManagementScope(strNameSpace, oConn);

                //get Fixed disk state

                ObjectQuery oQuery = new ObjectQuery("select FreeSpace,Size,Name from Win32_LogicalDisk where DriveType=3");

                //Execute the query
                ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);

                //Get the results
                ManagementObjectCollection oReturnCollection = oSearcher.Get();

                //loop through found drives and write out info
                double D_Freespace = 0;
                double D_Totalspace = 0;

                foreach (ManagementObject oReturn in oReturnCollection)
                {
                    // Disk name
                    
                    // Free Space in bytes
                    string strFreespace = oReturn["FreeSpace"].ToString();
                    D_Freespace = System.Convert.ToDouble(strFreespace);
                    // Size in bytes
                    string strTotalspace = oReturn["Size"].ToString();
                    D_Totalspace =  System.Convert.ToDouble(strTotalspace);

                    string warn = String.Empty;
                    if (Convert.ToUInt64(D_Freespace) <= 1073741824)
                        warn = "ATTENZIONE";

                    result += $"{warn}; {srvname.ToLower()}; {oReturn["Name"].ToString()}; {ConvertSize(D_Freespace, "GB").ToString()}; {ConvertSize(D_Totalspace, "GB").ToString()}" + Environment.NewLine;
                }
            }
            catch
            {
                result += $"Failed to obtain Server Information.The node you are trying to scan can be a Filer or a node which you don't have administrative priviledges. Please use the UNC convention to scan the shared folder in the server; {srvname}; ; ;" + Environment.NewLine;
            }

            return result;
        }

        public static double ConvertSize(double bytes, string type)
        {
            try
            {
                const int CONVERSION_VALUE = 1024;
                //determine what conversion they want
                switch (type)
                {
                    case "BY":
                        //convert to bytes (default)
                        return bytes;
                    case "KB":
                        //convert to kilobytes
                        return (bytes / CONVERSION_VALUE);
                    case "MB":
                        //convert to megabytes
                        return (bytes / CalculateSquare(CONVERSION_VALUE));
                    case "GB":
                        //convert to gigabytes
                        return (bytes / CalculateCube(CONVERSION_VALUE));
                    default:
                        //default
                        return bytes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Function to calculate the square of the provided number
        /// </summary>
        /// <param name="number">Int32 -> Number to be squared</param>
        /// <returns>Double -> THe provided number squared</returns>
        /// <remarks></remarks>
        public static double CalculateSquare(Int32 number)
        {
            return Math.Pow(number, 2);
        }


        /// <summary>
        /// Function to calculate the cube of the provided number
        /// </summary>
        /// <param name="number">Int32 -> Number to be cubed</param>
        /// <returns>Double -> THe provided number cubed</returns>
        /// <remarks></remarks>
        public static double CalculateCube(Int32 number)
        {
            return Math.Pow(number, 3);
        }

    }
}
