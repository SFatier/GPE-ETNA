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
                rsltCredential = CreateCredential(new TokenResponse { AccessToken = u.Token_GG, RefreshToken = u.RefreshToken});
            }
            GetGoogleService(rsltCredential);
            GetItems();
        }

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
            lst.FirstOrDefault(item => item.Login.Equals(Singleton.GetInstance().GetUtilisateur().Login)).Token_GG = token.AccessToken;
            lst.FirstOrDefault(item => item.Login.Equals(Singleton.GetInstance().GetUtilisateur().Login)).RefreshToken = token.RefreshToken;
            
            #region [Ecriture de l'utilisateur dans le fichier .JSON]
            try
            {
                string test = ConfigurationSettings.AppSettings["UtilisateurJSON"];
                using (StreamWriter file = File.CreateText(@test))
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

        public List<Files> GetItems()
        {
            //DriveService service = GetService();
            // define parameters of request.
            FilesResource.ListRequest FileListRequest = service.Files.List();

            //listRequest.PageSize = 10;
            //listRequest.PageToken = 10;
            FileListRequest.Fields = "nextPageToken, files(id, name, size, version, createdTime)";

            //get file list.
            IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;
            List<Files> FileList = new List<Files>();

            if (files != null && files.Count > 0)
            {
                //Files File = null;
                foreach (var file in files)
                {
                    Files File = new Files
                    {
                        IdGoogle = file.Id,
                        Nom = file.Name,
                        Size = file.Size,
                        Version = file.Version,
                        DateDeCreation = file.CreatedTime
                    };

                    FileList.Add(File);
                }
            }
            return FileList;
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

        internal void CreateFolder()
        {
            throw new NotImplementedException();
        }

        internal void Delete()
        {
            throw new NotImplementedException();
        }

        internal void GetFilesShared()
        {
            throw new NotImplementedException();
        }
    }
}
