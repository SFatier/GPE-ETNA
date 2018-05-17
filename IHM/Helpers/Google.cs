using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Drive.v3.Data;
using IHM.Model;
using System.Configuration;
using Newtonsoft.Json;
using System.Windows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;
using IHM.ModelView;
using Google.Apis.Download;

namespace IHM.Helpers
{
    public class GoogleCloud
    {
        //defined scope.
        public static string[] Scopes = { DriveService.Scope.Drive };
        public static DriveService service = null;

        public GoogleCloud()
        {
            /**
             * BEGIN
             *  Si connexion on ouvre le navigateur on récupère un code 
             *      puis on l'enregistre sous format JSON
             *  Sinon
             *      On récupère le token , on créé le credential
             * Fin
             * On connecte le Service
             **/
            UserCredential rsltCredential;
            Utilisateur u = Singleton.GetInstance().GetUtilisateur();
            if (u.Token_GG == null)
            {
                rsltCredential = GetAuthorization();
                SaveToken(rsltCredential);
            }
            else
            {
                var accesstoken = /*Singleton.GetInstance().Decrypt(Encoding.ASCII.GetBytes(*/u.Token_GG; //));
                var refreshtoken = /*Singleton.GetInstance().Decrypt(Encoding.ASCII.GetBytes(*/u.RefreshToken; //));
                rsltCredential = CreateCredential(new TokenResponse { AccessToken = accesstoken, RefreshToken = refreshtoken});
            }
            GetGoogleService(rsltCredential);
            GetItems();
        }

        /// <summary>
        /// Réupération de l'autorisation de l'utilisateur
        /// </summary>
        /// <returns></returns>
        public static UserCredential GetAuthorization()
        {
            UserCredential credential;
            try
            {
                using (var stream = new FileStream(ConfigurationSettings.AppSettings["ClientSecretJSON"], FileMode.Open, FileAccess.Read))
                {
                    String FolderPath = @"C:\";
                    String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
                   
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(FilePath, true)).Result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return credential;            
        }
        
        /// <summary>
        /// Enregistrement du token
        /// </summary>
        /// <param name="credential"></param>
        public void SaveToken(UserCredential credential)
        {
            //Récupération du token 
            var token = new TokenResponse
            {
                AccessToken = credential.Token.AccessToken,
                RefreshToken = credential.Token.RefreshToken
            };
            
            List<Utilisateur> lst = Singleton.GetInstance().GetAllUtilisateur();
            Utilisateur _u = lst.FirstOrDefault(item => item.Login.Equals(Singleton.GetInstance().GetUtilisateur().Login));
            lst.FirstOrDefault(item => item.Login.Equals(Singleton.GetInstance().GetUtilisateur().Login)).Token_GG = /*Singleton.GetInstance().Encrypt(*/token.AccessToken/*).ToString()*/;
            lst.FirstOrDefault(item => item.Login.Equals(Singleton.GetInstance().GetUtilisateur().Login)).RefreshToken = /*Singleton.GetInstance().Encrypt(*/token.RefreshToken/*).ToString()*/;
            
            #region [Ecriture de l'utilisateur dans le fichier .JSON]
            try
            {
                string test = ConfigurationSettings.AppSettings["UtilisateurJSON"];
                using (StreamWriter file = System.IO.File.CreateText(@test))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, lst);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :\" " + ex.Message);
            }
            #endregion
        }

        /// <summary>
        /// A faire verification avant l'ajout
        /// </summary>
        /// <param name="nameFolder"></param>
        public void FolderExists(string nameFolder)
        {
            //a faire
        }

        /// <summary>
        /// Creation d'un credential à partir d'un token
        /// </summary>
        public UserCredential CreateCredential(TokenResponse token)
        {
            ClientSecrets clientsecret;

            //récupération du client secret
            using (var stream = new FileStream(ConfigurationSettings.AppSettings["ClientSecretJSON"], FileMode.Open, FileAccess.Read))
            {
                clientsecret = GoogleClientSecrets.Load(stream).Secrets;
            }

            //recuperation du code d'authorization
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientsecret,
                Scopes = Scopes,
                DataStore = new FileDataStore("Store")
            });

            //creation du credential
            var googleServiceCredential = new UserCredential(flow, Environment.UserName, token);

            return googleServiceCredential;
        }

        public static void GetGoogleService(UserCredential credential)
        {
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleDriveRestAPI-v3",
            });
        }

        /// <summary>
        /// Récupère les fichiers de google drive
        /// </summary>
        /// <returns></returns>
        public List<Fichier> GetItems()
        {
            FilesResource.ListRequest FileListRequest = service.Files.List();
            FileListRequest.Fields = "nextPageToken, files(id, name, size, version, createdTime)";
            IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;
            List<Fichier> FileList = new List<Fichier>();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {

                    Fichier File = new Fichier();
                    File.IdGoogle = file.Id;
                        File.Nom = file.Name;
                        File.Taille = (file.Size == null ? "-" : file.Size.ToString());
                        File.Version = file.Version;
                        File.DateDeCreation = file.CreatedTime;
                        File.IsFile = (file.Parents == null ? true : false);
                        File.PreviewUrl = (file.WebContentLink == null ? "" : file.WebContentLink);
                        File.IMG = (File.IsFile != false ? "-" : Singleton.GetInstance().GetHomeModelView().lMVM.GetIcoByType("dossier"));
                        File.Type = (File.IsFile != false ? Path.GetExtension(file.Name) : "-");
                    FileList.Add(File);
                }
            }

            return FileList.OrderBy(f => f.Nom).ToList();
        } 

        internal void Download (string filename, string fileId, string savePath, string mimeType)
        {
           
            try
            {

                if (Path.HasExtension(filename))
                {
                    var request = service.Files.Get(fileId);

                    var stream = new System.IO.MemoryStream();
                    System.Diagnostics.Debug.WriteLine(fileId);
                    // Add a handler which will be notified on progress changes.
                    // It will notify on each chunk download and when the
                    // download is completed or failed.
                    request.MediaDownloader.ProgressChanged +=
                        (IDownloadProgress progress) =>
                        {
                            switch (progress.Status)
                            {
                                case DownloadStatus.Downloading:
                                    {
                                        System.Diagnostics.Debug.WriteLine(progress.BytesDownloaded);
                                        break;
                                    }
                                case DownloadStatus.Completed:
                                    {
                                        System.Diagnostics.Debug.WriteLine("Download complete.");
                                        break;
                                    }
                                case DownloadStatus.Failed:
                                    {
                                        System.Diagnostics.Debug.WriteLine("Download failed.");
                                        MessageBox.Show("File failed to download!!!", "Download Message", MessageBoxButton.OK, MessageBoxImage.Error);
                                        break;
                                    }
                            }
                        };
                    request.Download(stream);
                    convertMemoryStreamToFileStream(stream, savePath + @"\" + @filename);
                    stream.Dispose();
                }
                else
                {
                    string extension = "", converter = "";
                    foreach (MimeTypeConvert obj in MimeConverter.mimeList())
                    {
                        if (mimeType == obj.mimeType)
                        {
                            extension = obj.extension;
                            converter = obj.converterType;
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("{0} {1} {2}", fileId, extension, mimeType);
                    var request = service.Files.Export(fileId, converter);
                    var stream = new System.IO.MemoryStream();
                    // Add a handler which will be notified on progress changes.
                    // It will notify on each chunk download and when the
                    // download is completed or failed.
                    request.MediaDownloader.ProgressChanged +=
                            (IDownloadProgress progress) =>
                            {
                                switch (progress.Status)
                                {
                                    case DownloadStatus.Downloading:
                                        {
                                            Console.WriteLine(progress.BytesDownloaded);
                                            break;
                                        }
                                    case DownloadStatus.Completed:
                                        {
                                            Console.WriteLine("Download complete.");
                                            break;
                                        }
                                    case DownloadStatus.Failed:
                                        {
                                            Console.WriteLine("Download failed.");
                                            MessageBox.Show("File failed to download!!!", "Download Message", MessageBoxButton.OK, MessageBoxImage.Error);
                                            break;
                                        }
                                }
                            };
                    request.Download(stream);
                    convertMemoryStreamToFileStream(stream, savePath + @"\" + @filename + extension);
                    stream.Dispose();
                }

            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message + " Download From Drive Error");
              
            }
        }
        private static void convertMemoryStreamToFileStream(MemoryStream stream, string savePath)
        {
            FileStream fileStream;
            using (fileStream = new System.IO.FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                try
                {
                    // System.IO.File.Create(saveFile)
                    stream.WriteTo(fileStream);
                    fileStream.Close();
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.Message +" Convert Memory stream Error");
                }
            }
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public void Upload(string _uploadFile, string _paretn)
        {
            /*if (System.IO.File.Exists(_uploadFile))
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                body.Name = System.IO.Path.GetFileName(_uploadFile);
                body.Description = "File uploaded by Diamto Drive Sample";
                body.MimeType = GetMimeType(_uploadFile);
                body.Parents = new List() { new ParentReference() { Id = _parent } };

                // File's content.
                byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    service.Files. //(body, stream, GetMimeType(_uploadFile)).Upload();
                    //return request.ResponseBody;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    return null;
                }
            }
            else
            {
                Console.WriteLine("File does not exist: " + _uploadFile);
                return null;
            }
            */
        }

        /// <summary>
        /// Créé un fichier
        /// </summary>
        public void CreateFolder(string nameFolder)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = nameFolder,
                    MimeType = "application/vnd.google-apps.folder"
                };
                var request = service.Files.Create(fileMetadata);
                request.Fields = "id";
                var file = request.Execute();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Supprime un fichier dans Google Drive
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileId"></param>
        /// <param name="optional"></param>
        public void Delete(string fileId)
        {
            try
            {
                Verification(fileId);
                var request = service.Files.Delete(fileId);
                request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Echec de la suppression du fichier google.", ex);
            }
        }

        /// <summary>
        /// Vérifie que l'on possède un service et un id de fichier
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileId"></param>
        private static void Verification(string fileId)
        {
            if (service == null)
                throw new ArgumentNullException("Erreur de Service Google");
            if (fileId == null)
                throw new ArgumentNullException("Erreur de suppression du fichier google =>" + fileId);

        }

        /// <summary>
        /// Affichage d'un fichier depuis google drive
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public Channel Watch(string fileId)
        {
            try
            {
                Verification(fileId);
                var request = service.Files.Watch(new Channel(), fileId);
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Echec de la visualisation d'un document google.", ex);
            }
        }

        /// <summary>
        /// Copie un fichier dans google drive.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="fileId"></param>
        /// <param name="body"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public Google.Apis.Drive.v3.Data.File Copy(DriveService service, string fileId, Google.Apis.Drive.v3.Data.File body)
        {
            try
            {
                Verification(fileId);

                if (body == null)
                    throw new ArgumentNullException("body");

                var request = service.Files.Copy(body, fileId);

                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Echec de la copie d'un fichier", ex);
            }
        }

        /// <summary>
        /// Recuperation des fichiers partagés avec l'utilisateur connecté.
        /// </summary>
        public void GetFilesShared()
        {
            List<Fichier> lstFiles = new List<Fichier>();
            try
            {
                //var request = service.;
            }
            catch (Exception ex)
            {
                throw new Exception("Echec de la copie d'un fichier", ex);
            }
            //return lstFiles;
        }
    }
}
