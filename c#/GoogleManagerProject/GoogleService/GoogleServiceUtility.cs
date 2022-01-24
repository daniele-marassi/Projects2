using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using MediaToolkit;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using NLog;
using System.Reflection;
using Google.Apis.Download;
using DriveFile = Google.Apis.Drive.v3.Data.File;
using Additional;
using Additional.NLog;
using GoogleManagerModels;
using System.Drawing;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using Newtonsoft.Json;
using Google.Apis.Keep.v1;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;

namespace GoogleService
{
    public class GoogleServiceUtility
    {
        private static Utility utility;
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private static NLogUtility nLogUtility = new NLogUtility();

        public GoogleServiceUtility()
        {
            utility = new Utility();
        }

        /// <summary>
        /// Get DriveFiles
        /// </summary>
        /// <param name="service"></param>
        /// <param name="managerRequest"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private List<FileProperties> GetDriveFiles(DriveService service, ManagerRequest managerRequest, string folderName = null)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var fileList = new List<FileProperties>();
                var result = new List<Dictionary<string, string>>() { };

                try
                {
                    var allFolders = new List<DriveFile>() { };

                    var listRequestFolder = service.Files.List();
                    listRequestFolder.PageSize = 10;
                    var queryToFolders = "mimeType = 'application/vnd.google-apps.folder'";
                    var queryToFiles = "mimeType != 'application/vnd.google-apps.folder'";

                    listRequestFolder.Q = queryToFolders;
                    listRequestFolder.Fields = "nextPageToken, files(id, name, parents)";

                    var resultFolders = listRequestFolder.Execute();
                    var folders = resultFolders.Files;

                    logger.Info("Folders:");
                    var conteggioFolders = 0;

                    var noMoreElements = false;

                    while (noMoreElements == false)
                    {
                        foreach (var folder in folders)
                        {
                            conteggioFolders++;
                            logger.Info(conteggioFolders.ToString() + " - " + folder.Name);
                            allFolders.Add(folder);
                        }

                        noMoreElements = string.IsNullOrWhiteSpace(resultFolders.NextPageToken);

                        if (folders == null || folders.Count == 0) noMoreElements = true;

                        if (!noMoreElements)
                        {
                            listRequestFolder = service.Files.List();
                            listRequestFolder.PageToken = resultFolders.NextPageToken;
                            listRequestFolder.PageSize = 10;
                            listRequestFolder.Q = queryToFolders;
                            listRequestFolder.Fields = "nextPageToken, files(id, name, parents)";
                            resultFolders = listRequestFolder.Execute();
                            folders = resultFolders.Files;
                        }
                    }

                    if (folderName != null)
                    {
                        var rootId = allFolders.Where(_ => _.Name.Trim().ToLower() == folderName.Trim().ToLower()).FirstOrDefault()?.Id;

                        if (rootId != null)
                        {

                            GetAllParents(rootId, allFolders, result);
                            var _queryToFiles = String.Empty;
                            foreach (var item in result)
                            {
                                if (_queryToFiles != String.Empty) _queryToFiles += " or ";
                                _queryToFiles += $"parents = '{item["Id"]}'";

                            }
                            queryToFiles += " and (" + _queryToFiles + ")";
                        }
                    }

                    var listRequestFile = service.Files.List();
                    listRequestFile.PageSize = 10;
                    listRequestFile.Q = queryToFiles;
                    listRequestFile.IncludeTeamDriveItems = true;
                    listRequestFile.SupportsTeamDrives = true;
                    listRequestFile.Fields = "nextPageToken, files(id, name, parents, modifiedTime, createdTime, kind, size, videoMediaMetadata, fileExtension, imageMediaMetadata, mimeType, properties, appProperties )";

                    var resultFile = listRequestFile.Execute();
                    var files = resultFile.Files;

                    logger.Info("Files:");
                    var conteggioFiles = 0;

                    noMoreElements = false;

                    while (noMoreElements == false)
                    {
                        foreach (var file in files)
                        {
                            var path = String.Empty;
                            foreach (string Item in GetFullPath(file, allFolders).Reverse())
                            {
                                if (path != String.Empty) path += "/";
                                path += Item;
                            }
                            var pathTemp = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Temp");
                            var pathResources = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Resources");
                            int rndSize = 0;
                            Bitmap bitmap = default(Bitmap);
                            var thumbnail = new byte[] { };
                            var pathThumbnail = Path.Combine(pathTemp, "");
                            var rnd = new Random(DateTime.Now.Millisecond);
                            rndSize = rnd.Next(managerRequest.MinThumbnailSize, managerRequest.MaxThumbnailSize);
                            var thumbnailFile = String.Empty;
                            var type = String.Empty;
                            var googleDriveListAllFiles = new GoogleServiceUtility();

                            var serviceToDownload = new DriveService(new BaseClientService.Initializer()
                            {
                                HttpClientInitializer = googleDriveListAllFiles.CreateCredential(managerRequest, AccountType.Drive),
                                ApplicationName = "Google Service - GetDriveFiles",
                            });

                            var stream = googleDriveListAllFiles.DownloadFile(serviceToDownload, file.Id);
                            
                            if (!Directory.Exists(pathTemp)) Directory.CreateDirectory(pathTemp);
                            var filePath = Path.Combine(pathTemp, file.Name);
                            googleDriveListAllFiles.SaveStream(stream, filePath);

                            if (utility.IsImage("." + file.FileExtension))
                            {
                                type = "Image";
                                thumbnailFile = filePath;
                            }

                            if (utility.IsAudio("." + file.FileExtension))
                            {
                                type = "Audio";
                                thumbnailFile = Path.Combine(pathResources, "AudioThumbnail.gif");

                                var inputFile = new MediaToolkit.Model.MediaFile { Filename = filePath };
                                try
                                {
                                    using (var engine = new Engine())
                                    {
                                        engine.GetMetadata(inputFile);
                                    }

                                    file.VideoMediaMetadata = new DriveFile.VideoMediaMetadataData() { };
                                    file.VideoMediaMetadata.Width = 200;
                                    file.VideoMediaMetadata.Height = 200;
                                    file.VideoMediaMetadata.DurationMillis = (long)inputFile.Metadata.Duration.TotalMilliseconds;
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex.ToString());
                                }

                                try
                                {
                                    System.IO.File.Delete(filePath);
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex.ToString());
                                }
                            }

                            if (utility.IsVideo("." + file.FileExtension))
                            {
                                type = "Video";
                                var inputFile = new MediaToolkit.Model.MediaFile { Filename = filePath };

                                try
                                {
                                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                                    thumbnailFile = Path.Combine(pathTemp, managerRequest.Account + "_VideoThumbnail.jpeg");
                                    ffMpeg.GetVideoThumbnail(filePath, thumbnailFile);
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex.ToString());
                                }

                                try
                                {
                                    using (var engine = new Engine())
                                    {
                                        engine.GetMetadata(inputFile);
                                    }

                                    file.VideoMediaMetadata = new DriveFile.VideoMediaMetadataData() { };
                                    file.VideoMediaMetadata.Width = int.Parse(inputFile.Metadata.VideoData.FrameSize.Split('x')[0]);
                                    file.VideoMediaMetadata.Height = int.Parse(inputFile.Metadata.VideoData.FrameSize.Split('x')[1]);
                                    file.VideoMediaMetadata.DurationMillis = (long)inputFile.Metadata.Duration.TotalMilliseconds;
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex.ToString());
                                }
                            }

                            try
                            {
                                using (Bitmap bitmpaFromFile = (Bitmap)Image.FromFile(thumbnailFile))
                                {
                                    bitmap = utility.GetThumbnail(bitmpaFromFile, rndSize, 100, 0);
                                }
                                ImageConverter converter = new ImageConverter();
                                thumbnail = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
                            }
                            catch (Exception ex)
                            {
                                thumbnail = new byte[] { };
                                logger.Error(ex.ToString());
                            }

                            try
                            {
                                System.IO.File.Delete(filePath);
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.ToString());
                            }

                            if (type != "Audio")
                            {
                                try
                                {
                                    System.IO.File.Delete(thumbnailFile);
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex.ToString());
                                }
                            }

                            fileList.Add(
                                new FileProperties
                                {
                                    FileId = file.Id,
                                    Name = file.Name,
                                    Path = path,
                                    ModifiedTime = Convert.ToDateTime(file.ModifiedTime),
                                    CreatedTime = Convert.ToDateTime(file.CreatedTime),
                                    Size = file.Size != null ? (long)file.Size : 0,
                                    FileExtension = file.FileExtension,
                                    MimeType = file.MimeType,
                                    ImageHeight = file.ImageMediaMetadata != null ? file.ImageMediaMetadata?.Height : 0,
                                    ImageWidth = file.ImageMediaMetadata != null ? file.ImageMediaMetadata?.Width : 0,
                                    ImageTime = file.ImageMediaMetadata != null ? file.ImageMediaMetadata?.Time : null,
                                    ImageLocationAltitude = file.ImageMediaMetadata != null && file.ImageMediaMetadata?.Location != null ? file.ImageMediaMetadata.Location.Altitude : null,
                                    ImageLocationLatitude = file.ImageMediaMetadata != null && file.ImageMediaMetadata?.Location != null ? file.ImageMediaMetadata.Location.Latitude : null,
                                    ImageLocationLongitude = file.ImageMediaMetadata != null && file.ImageMediaMetadata?.Location != null ? file.ImageMediaMetadata.Location.Longitude : null,
                                    VideoDurationMillis = file.VideoMediaMetadata != null ? file.VideoMediaMetadata?.DurationMillis : 0,
                                    VideoHeight = file.VideoMediaMetadata != null ? file.VideoMediaMetadata?.Height : 0,
                                    VideoWidth = file.VideoMediaMetadata != null ? file.VideoMediaMetadata?.Width : 0,
                                    UserName = managerRequest.Account,
                                    Type = type,
                                    File = new byte[] { }, 
                                    Thumbnail = thumbnail, 
                                    ThumbnailHeight = bitmap?.Height != null ? (int)bitmap?.Height : 0, 
                                    ThumbnailWidth = bitmap?.Width != null ? (int)bitmap?.Width : 0,
                                    GoogleDriveAccountId = managerRequest.GoogleAccountId
                                }
                            );

                            conteggioFiles++;
                            logger.Info(conteggioFiles.ToString() + " - " + managerRequest.Account + "/" + path + "/" + file.Name);
                        }
                        noMoreElements = string.IsNullOrWhiteSpace(resultFile.NextPageToken);

                        if (folders == null || folders.Count == 0) noMoreElements = true;

                        if (!noMoreElements)
                        {
                            listRequestFile = service.Files.List();
                            listRequestFile.PageToken = resultFile.NextPageToken;
                            listRequestFile.PageSize = 10;
                            listRequestFile.Q = queryToFiles;
                            listRequestFile.IncludeTeamDriveItems = true;
                            listRequestFile.SupportsTeamDrives = true;
                            listRequestFile.Fields = "nextPageToken, files(id, name, parents, modifiedTime, createdTime, kind, size, videoMediaMetadata, fileExtension, imageMediaMetadata, mimeType, properties, appProperties )";
                            resultFile = listRequestFile.Execute();
                            files = resultFile.Files;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return fileList;
            }
        }

        /// <summary>
        /// The Get All Parents
        /// </summary>
        /// <param name="rootId"></param>
        /// <param name="folders"></param>
        /// <param name="result"></param>
        public void GetAllParents(string rootId, List<DriveFile> folders, List<Dictionary<string, string>> result)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    var parentId = String.Empty;
                    var parents = folders.Where(_ => _.Parents.FirstOrDefault() == rootId).ToList();
                    if (result == null || result.Count == 0)
                    {
                        var name = folders.Where(_ => _.Id == rootId).FirstOrDefault()?.Name;
                        result.Add(new Dictionary<string, string>() { { "Id", rootId }, { "Name", name } });
                    }

                    foreach (var _parent in parents)
                    {
                        parentId = _parent.Id;
                        var name = folders.Where(_ => _.Id == parentId).FirstOrDefault()?.Name;
                        result.Add(new Dictionary<string, string>() { { "Id", parentId }, { "Name", name } });
                        GetAllParents(parentId, folders, result);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get GoogleDrive Result
        /// </summary>
        /// <param name="service"></param>
        /// <param name="googleDriveAuth"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public List<FileProperties> GetGoogleDriveResult(DriveService service, ManagerRequest managerRequest, string folderName = null)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var googleDriveResultList = new List<FileProperties>() { };
                try
                {
                    googleDriveResultList = GetDriveFiles(service, managerRequest, folderName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return googleDriveResultList;
            }
        }

        /// <summary>
        /// Create Credential
        /// </summary>
        /// <param name="managerRequest"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public UserCredential CreateCredential(ManagerRequest managerRequest, AccountType accountType)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                UserCredential result = null;

                try
                {
                    logger.Warn(managerRequest.Account);
                    result = AuthenticateOauth(managerRequest, accountType);
                    logger.Info("Create Credential successfully");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }
        }

        /// <summary>
        /// The Get Full Path
        /// </summary>
        /// <param name="file"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        private IList<string> GetFullPath(DriveFile file, IList<DriveFile> files)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var path = new List<string>();
                try
                {
                    if (file.Parents == null || file.Parents.Count == 0)
                    {
                        return path;
                    }
                    var mainfile = file;

                    while (GetParentFromID(file.Parents[0], files) != null)
                    {
                        path.Add(GetFolderNameFromID(GetParentFromID(file.Parents[0], files).Id, files));
                        file = GetParentFromID(file.Parents[0], files);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return path;
            }
        }

        /// <summary>
        /// The Get Parent From ID
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        private DriveFile GetParentFromID(string fileID, IList<DriveFile> files)
        {
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Parents != null && file.Parents.Count > 0)
                    {
                        if (file.Id == fileID)
                        {
                            return file;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// The Get Folder Name From ID
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        private string GetFolderNameFromID(string folderID, IList<DriveFile> files)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var folderName = "";
                try
                {
                    if (files != null && files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            if (file.Id == folderID)
                            {
                                folderName = file.Name;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return folderName;
            }
        }

        /// <summary>
        /// The Download File
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public System.IO.MemoryStream DownloadFile(DriveService service, string fileId)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                var stream = new System.IO.MemoryStream();
                try
                {
                    var request = service.Files.Get(fileId);
                    // Add a handler which will be notified on progress changes.
                    // It will notify on each chunk download and when the
                    // download is completed or failed.
                    request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    logger.Info(progress.BytesDownloaded.ToString());
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    logger.Info("Download complete.");
                                    //SaveStream(stream, Path.Combine(saveTo, detailsFile.Name));
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    logger.Info("Download failed.");
                                    break;
                                }
                        }
                    };
                    request.Download(stream);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return stream;
            }
        }

        /// <summary>
        /// The Save Stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="saveTo"></param>
        public void SaveStream(System.IO.MemoryStream stream, string saveTo)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                try
                {
                    using (System.IO.FileStream file = new System.IO.FileStream(saveTo, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        stream.WriteTo(file);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Authenticate to Google Using Oauth2
        /// Documentation https://developers.google.com/accounts/docs/OAuth2
        /// </summary>
        /// <param name="managerRequest"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public UserCredential AuthenticateOauth(ManagerRequest managerRequest, AccountType accountType)
        {
            using (var logger = new NLogScope(classLogger, nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
            {
                UserCredential result= null;

                try
                {
                    if (string.IsNullOrEmpty(managerRequest.Auth.Installed.Client_id))
                        throw new ArgumentNullException("clientId");
                    if (string.IsNullOrEmpty(managerRequest.Auth.Installed.Client_secret))
                        throw new ArgumentNullException("clientSecret");
                    if (string.IsNullOrEmpty(managerRequest.Account))
                        throw new ArgumentNullException("userName");

                    var scopes = new List<string>() { };

                    if (accountType == AccountType.Drive)
                    {
                        scopes = new List<string>() { DriveService.Scope.Drive, DriveService.Scope.DriveFile};
                    }

                    if (accountType == AccountType.Calendar)
                    {
                        scopes = new List<string>() { Google.Apis.Calendar.v3.CalendarService.Scope.Calendar, Google.Apis.Calendar.v3.CalendarService.Scope.CalendarReadonly };
                    }

                    if (accountType == AccountType.Note)
                    {
                        scopes = new List<string>() { KeepService.Scope.Keep, KeepService.Scope.KeepReadonly };
                    }

                    var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    var resourcesPath = Path.Combine(appPath, "Resources");
                    if (!Directory.Exists(resourcesPath)) Directory.CreateDirectory(resourcesPath);
                    var tokenFilePath = System.IO.Path.Combine(resourcesPath, "TokenFiles");
                    if (!Directory.Exists(tokenFilePath)) Directory.CreateDirectory(tokenFilePath);

                    var s_cts = new CancellationTokenSource();
                    UserCredential credential = null;

                    try
                    {
                        s_cts.CancelAfter(60000);

                        if (managerRequest.RefreshToken == null)
                        {
                            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = managerRequest.Auth.Installed.Client_id, ClientSecret = managerRequest.Auth.Installed.Client_secret }
                                                                                 , scopes.ToArray()
                                                                                 , managerRequest.Account
                                                                                 , s_cts.Token
                                                                                 , new FileDataStore(tokenFilePath, true)).Result;
                        }
                        else
                        {
                            var init = new GoogleAuthorizationCodeFlow.Initializer
                            {
                                ClientSecrets = new ClientSecrets { ClientId = managerRequest.Auth.Installed.Client_id, ClientSecret = managerRequest.Auth.Installed.Client_secret },
                                Scopes = scopes.ToArray()
                            };
                            var token = new TokenResponse { RefreshToken = managerRequest.RefreshToken };
                            credential = new UserCredential(new Google.Apis.Auth.OAuth2.Flows.AuthorizationCodeFlow(init), "", token);
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        logger.Error("\nTasks cancelled: timed out.\n" + ex.Message);
                    }
                    finally
                    {
                        s_cts.Dispose();
                    }

                    result = credential;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }
        }
    }
}