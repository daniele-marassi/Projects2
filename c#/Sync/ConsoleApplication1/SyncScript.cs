using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static SyncScript.Sync.Models.SyncModels;


namespace Sync
{
    class SyncScript
    {
        private static System.Timers.Timer aTimer;
        private static int interval;
        private string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            var method = sf.GetMethod();
            return $"{method.DeclaringType.FullName}.{method.Name}";
        }
        public void Scheduling()
        {
            Console.WriteLine(String.Format("*** Started: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
            var appSettings = ConfigurationManager.AppSettings;
            string millisecondsSyncTimer = appSettings["MillisecondsSyncTimer"];
            Console.WriteLine(String.Format("*** {0} - {1} ***", nameof(millisecondsSyncTimer), millisecondsSyncTimer));

            if (millisecondsSyncTimer == null || millisecondsSyncTimer == String.Empty)
            { Console.WriteLine(String.Format("*** millisecondsSyncTimer is empty ***")); }
            else
            {
                interval = int.Parse(appSettings["MillisecondsSyncTimer"]);
                aTimer = new System.Timers.Timer(10000);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = 1000; // set interval to first round
                aTimer.Enabled = true;
                Console.ReadLine(); // only to consoleapplication
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            aTimer.Interval = interval; // set interval to later rounds
            Processing();
        }

        private IEnumerable<SyncFolder> GetData()
        {
            Console.WriteLine(String.Format("*** Started: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
            var appSettings = ConfigurationManager.AppSettings;
            string owner = appSettings["owner"];
            Console.WriteLine(String.Format("*** {0} - {1} ***", nameof(owner), owner));

            if (owner == null || owner == String.Empty)
            { throw new Exception("*** owner is empty ***"); }

            List<SyncFolder> syncFolders = new List<SyncFolder>()
            {
                new SyncFolder
                {
                    Owner = "AdPortal2",
                    Folders = new List<SyncPath>()
                    {
                            new SyncPath
                            {
                                DestinationPath ="\\\\STORAGE2\\XDM\\Foto_2",
                                SourcePath = "C:\\Foto_1"
                            },
                            new SyncPath
                            {
                                DestinationPath ="\\\\STORAGE2\\XDM\\Foto_Bck_2",
                                SourcePath = "C:\\Foto_Bck_1"
                            }

                    }
                },
                new SyncFolder
                {
                    Owner = "AdPortal3",
                    Folders = new List<SyncPath>()
                    {
                            new SyncPath
                            {
                                DestinationPath ="C:\\Foto_2",
                                SourcePath = "C:\\Foto_1"
                            },
                            new SyncPath
                            {
                                DestinationPath ="C:\\Foto_Bck_2",
                                SourcePath = "C:\\Foto_Bck_1"
                            }

                    }
                },
                new SyncFolder
                {
                    Owner = "AdPortal4",
                    Folders = new List<SyncPath>()
                    {
                            new SyncPath
                            {
                                DestinationPath ="C:\\Foto_3",
                                SourcePath = "C:\\Foto_4"
                            },
                            new SyncPath
                            {
                                DestinationPath ="C:\\Foto_Bck_3",
                                SourcePath = "C:\\Foto_Bck_4"
                            }

                    }
                }
            };

            var filteredSyncFolders = syncFolders.Where(_ => _.Owner == owner);
            Console.WriteLine(String.Format("Count of {0} - {1}", nameof(filteredSyncFolders), filteredSyncFolders?.Count() ?? 0));

            if (filteredSyncFolders.Count() <= 0)
            { throw new Exception(String.Format("*** Not found syncFolders to owner [{0}] ***", owner)); }
            Console.WriteLine(String.Format("*** Ended: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
            return filteredSyncFolders;
        }

        private void Processing()
        {
            Console.WriteLine(String.Format("*** Started: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
            try
            {
                foreach (SyncFolder syncFolder in GetData())
                {
                    GetFilesAndSync(syncFolder);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format(e.Message));
            }
            Console.WriteLine(String.Format("*** Ended: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
        }

        private void GetFilesAndSync(SyncFolder syncFolder)
        {
            Console.WriteLine(String.Format("*** Started: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
            List<string> pathsFile = new List<string>() { };
            foreach (var folder in syncFolder.Folders)
            {
                ListOfFilesRelated destinationFiles = new ListOfFilesRelated { };
                ListOfFilesRelated sourceFiles = new ListOfFilesRelated { };
                string destinationPath = folder.DestinationPath;
                string sourcePath = folder.SourcePath;

                if (Directory.Exists(destinationPath))
                {
                    pathsFile = System.IO.Directory.GetFiles(destinationPath, "*.*", System.IO.SearchOption.AllDirectories).ToList();
                }
                else { Console.WriteLine(String.Format("*** [{0}] - [{1}] not exists ***", nameof(destinationPath), destinationPath)); }

                Console.WriteLine(String.Format("*** {0} - {1} Count of {2} - {3} ***", nameof(destinationPath), destinationPath, nameof(pathsFile), pathsFile?.Count ?? 0));

                destinationFiles =
                        new ListOfFilesRelated
                        {
                            PathMainFolder = folder.DestinationPath.TrimEnd('\\'),
                            PathsFile = pathsFile
                        };
                pathsFile = null;
                if (Directory.Exists(sourcePath))
                {
                    pathsFile = System.IO.Directory.GetFiles(sourcePath, "*.*", System.IO.SearchOption.AllDirectories).ToList();
                }
                else
                {
                    Console.WriteLine(String.Format("*** {0} - {1} not exists ***", nameof(sourcePath), sourcePath));
                    continue;
                }
                Console.WriteLine(String.Format("*** {0} - {1} Count of {2} - {3} ***", nameof(sourcePath), sourcePath, nameof(pathsFile), pathsFile?.Count ?? 0));
                sourceFiles =
                    new ListOfFilesRelated
                    {
                        PathMainFolder = folder.SourcePath.TrimEnd('\\'),
                        PathsFile = pathsFile
                    };

                SyncFiles(sourceFiles, destinationFiles);
            }
            Console.WriteLine(String.Format("*** Ended: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
        }

        private void SyncFiles(ListOfFilesRelated sourceFiles, ListOfFilesRelated destinationFiles)
        {
            Console.WriteLine(String.Format("*** Started: [{0}] at [{1}] ***", GetCurrentMethod(),DateTime.Now.ToString()));
            foreach (string sourceFilePath in sourceFiles.PathsFile)
            {
                var destinationFilePath = sourceFilePath.Replace(sourceFiles.PathMainFolder, destinationFiles.PathMainFolder);
                Console.WriteLine(String.Format("*** {0} - {1} ***", nameof(destinationFilePath), destinationFilePath));
                DateTime destinationLastWriteTime = DateTime.MinValue;
                DateTime sourceLastWriteTime = DateTime.MinValue;

                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine(String.Format("*** {0} - {1} not exists ***", nameof(sourceFilePath), sourceFilePath));
                    continue;
                }
                bool destinationFilePathExists = false;

                sourceLastWriteTime = File.GetLastWriteTimeUtc(sourceFilePath);
                Console.WriteLine(String.Format("*** {0} - {1} ***", nameof(sourceLastWriteTime), sourceLastWriteTime));
                if (File.Exists(destinationFilePath))
                {
                    destinationLastWriteTime = File.GetLastWriteTimeUtc(destinationFilePath);
                    destinationFilePathExists = true;
                }
                else
                {
                    Console.WriteLine(String.Format("*** {0} - {1} not exists ***", nameof(destinationFilePath), destinationFilePath));
                }

                Console.WriteLine(String.Format("*** {0} - {1} ***", nameof(destinationLastWriteTime), destinationLastWriteTime));

                if (destinationLastWriteTime >= sourceLastWriteTime)
                {
                    Console.WriteLine(String.Format("*** {0} - {1} is updated ***", nameof(destinationFilePath), destinationFilePath));
                    continue;
                }
                if (!destinationFilePathExists)
                {
                    createMissingDirectories(destinationFilePath);
                }
                Console.WriteLine(String.Format("*** Try to copy the file [{0}] ***", destinationFilePath));
                File.Copy(sourceFilePath, destinationFilePath, true);
            }
            Console.WriteLine(String.Format("*** Ended: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
        }

        private void createMissingDirectories(string filePath)
        {
            Console.WriteLine(String.Format("*** Started: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
            var destinationDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(destinationDirectory))
            {
                Console.WriteLine(String.Format("*** Try to creation of the path [{0}] ***", destinationDirectory));
                Directory.CreateDirectory(destinationDirectory);
            }
            Console.WriteLine(String.Format("*** Ended: [{0}] at [{1}] ***", GetCurrentMethod(), DateTime.Now.ToString()));
        }
    }
}

