using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using IHM.Helpers;
using IHM.Model;
using IHM.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GPE
{
    class DropBoxBase
    {
        #region Variables  
        public DropboxClient DBClient;
        private string oauth2State;
        private const string RedirectUri = "https://localhost/authorize"; // Same as we have configured Under [Application] -> settings -> redirect URIs.  
        #endregion

        #region Constructor  
        public DropBoxBase(string ApiKey, string ApiSecret, string ApplicationName = "TestApp")
        {
            try
            {
                AppKey = ApiKey;
                AppSecret = ApiSecret;
                AppName = ApplicationName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties  
        public string AppName
        {
            get; private set;
        }
        public string AuthenticationURL
        {
            get; private set;
        }
        public string AppKey
        {
            get; private set;
        }

        public string AppSecret
        {
            get; private set;
        }

        public string AccessTocken
        {
            get; private set;
        }
        public string Uid
        {
            get; private set;
        }
        #endregion

        #region UserDefined Methods  

        /// <summary>  
        /// This method is to generate Authentication URL to redirect user for login process in Dropbox.  
        /// </summary>  
        /// <returns></returns>  
        public string GeneratedAuthenticationURL()
        {
            try
            {
                this.oauth2State = Guid.NewGuid().ToString("N");
                Uri authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, AppKey, RedirectUri, state: oauth2State);
                AuthenticationURL = authorizeUri.AbsoluteUri.ToString();
                return authorizeUri.AbsoluteUri.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>  
        /// This method is to generate Access Token required to access dropbox outside of the environment (in ANy application).  
        /// </summary>  
        /// <returns></returns>  
        public string GenerateAccessToken()
        {
            try
            {
                string _strAccessToken = string.Empty;

                if (CanAuthenticate())
                {
                    if (string.IsNullOrEmpty(AuthenticationURL))
                    {
                        throw new Exception("AuthenticationURL is not generated !");

                    }
                    LogWindow login = new LogWindow(AppKey, AuthenticationURL, this.oauth2State);  
                    login.Owner = Application.Current.MainWindow;
                    login.ShowDialog();
                    if (login.Result)
                    {
                        _strAccessToken = login.AccessToken;
                        AccessTocken = login.AccessToken;
                        Uid = login.Uid;
                        GetDBClient(AccessTocken);
                    }
                    else
                    {
                        DBClient = null;
                        AccessTocken = string.Empty;
                        Uid = string.Empty;
                    }
                }

                return _strAccessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetDBClient(string AccessTocken)
        {
            DropboxClientConfig CC = new DropboxClientConfig(AppName, 1);
            HttpClient HTC = new HttpClient();
            HTC.Timeout = TimeSpan.FromMinutes(10);
            CC.HttpClient = HTC;
            DBClient = new DropboxClient(AccessTocken, CC);
        }

        /// <summary>  
        /// Method to create new folder on Dropbox  
        /// </summary>  
        /// <param name="path"> path of the folder we want to create on Dropbox</param>  
        /// <returns></returns>  
        public bool CreateFolder(string path)
        {
            try
            {
                var folderArg = new CreateFolderArg(path);
                var folder = DBClient.Files.CreateFolderAsync(folderArg);
                var result = folder.Result;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>  
        /// Method is to check that whether folder exists on Dropbox or not.  
        /// </summary>  
        /// <param name="path"> Path of the folder we want to check for existance.</param>  
        /// <returns></returns>  
        public bool FolderExists(string path)
        {
            try
            {
                if (AccessTocken == null)
                {
                    throw new Exception("AccessToken not generated !");
                }
                if (AuthenticationURL == null)
                {
                    throw new Exception("AuthenticationURI not generated !");
                }

                var folders = DBClient.Files.ListFolderAsync(path);
                var result = folders.Result;
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        /// <summary>  
        /// Method to delete file/folder from Dropbox  
        /// </summary>  
        /// <param name="path">path of file.folder to delete</param>  
        /// <returns></returns>  
        public bool Delete(string path)
        {
            try
            {
                var folders = DBClient.Files.DeleteAsync(path);
                var result = folders.Result;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>  
        /// Method to upload files on Dropbox  
        /// </summary>  
        /// <param name="UploadfolderPath"> Dropbox path where we want to upload files</param>  
        /// <param name="UploadfileName"> File name to be created in Dropbox</param>  
        /// <param name="SourceFilePath"> Local file path which we want to upload</param>  
        /// <returns></returns>  
        public bool Upload(string UploadfolderPath, string UploadfileName, string SourceFilePath)
        {
            try
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(SourceFilePath)))
                {
                    var response = DBClient.Files.UploadAsync(UploadfolderPath + UploadfileName, WriteMode.Overwrite.Instance, body: stream);
                    var rest = response.Result; //Added to wait for the result from Async method  
                }

                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        /// <summary>  
        /// Method to Download files from Dropbox  
        /// </summary>  
        /// <param name="DropboxFolderPath">Dropbox folder path which we want to download</param>  
        /// <param name="DropboxFileName"> Dropbox File name availalbe in DropboxFolderPath to download</param>  
        /// <param name="DownloadFolderPath"> Local folder path where we want to download file</param>  
        /// <param name="DownloadFileName">File name to download Dropbox files in local drive</param>  
        /// <returns></returns>  
        public bool Download(string DropboxFolderPath, string DropboxFileName, string DownloadFolderPath, string DownloadFileName)
        {
            try
            {
                var response = DBClient.Files.DownloadAsync(DropboxFolderPath + "/" + DropboxFileName);
                var result = response.Result.GetContentAsStreamAsync(); //Added to wait for the result from Async method  

                return true;
            }
            catch (Exception )
            {
                return false;
            }

        }

        /**
         * Récupère la liste des fichiers et dossiers du compte dropbox connecté
         * */
        public ObservableCollection<Files> getEntries(ListModelView lMVM)
        {
            var liste = DBClient.Files.ListFolderAsync(string.Empty);
            var Cursor = liste.Result.Cursor;
            var Entries = liste.Result.Entries;
            ObservableCollection<Files> lstFiles = new ObservableCollection<Files>();

            // folder
            List<String> lstFolder = new List<string>();
            foreach (var item in Entries.Where(i => i.IsFolder))
            {
                string IdFile = item.AsFolder.Id;
                string nom = item.Name;
                string type = "dossier de fichiers";
                string IMG = lMVM.GetIcoByType("dossier");
                DateTime dateDeCreation = DateTime.Now ; // item.AsFile.ClientModified;
                DateTime ModifieLe = DateTime.Now; // item.AsFile.ServerModified;
                int taille = 0; // Convert.ToInt32(item.AsFile.Size);
                string path = item.PathDisplay;
                Files f = new Files(IdFile, nom, IMG, type, dateDeCreation, ModifieLe, taille, false);
                f.path = path;
                lstFiles.Add(f);
            }

            //Files
            foreach (var item in Entries.Where(i => i.IsFile))
            {
                string IdFile = item.AsFile.Id;
                string nom = item.Name;
                var type = Path.GetExtension(item.Name);
                string IMG = lMVM.GetIcoByType(type); 
                DateTime dateDeCreation = item.AsFile.ClientModified;
                DateTime ModifieLe = item.AsFile.ServerModified;
                int taille = Convert.ToInt32(item.AsFile.Size);
                string path = item.PathDisplay;
                Files f = new Files(IdFile, nom, IMG,  type, dateDeCreation, ModifieLe, taille, true);
                f.path = path;
                lstFiles.Add(f);
            }

            //var rst = new  { lstFolder, lstFiles};
            //return rst;
            return lstFiles;
        }

        /**
         * Demande a consulté le fichier
         * */
        public bool SharingFile (Files fichier, Utilisateur utilisateur)
        {
            try
            {
                var members = new[] { new MemberSelector.Email(utilisateur.Email) };
                DBClient.Sharing.AddFileMemberAsync(fichier.path, members);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Validation Methods  
        /// <summary>  
        /// Validation method to verify that AppKey and AppSecret is not blank.  
        /// Mendatory to complete Authentication process successfully.  
        /// </summary>  
        /// <returns></returns>  
        public bool CanAuthenticate()
        {
            try
            {
                if (AppKey == null)
                {
                    throw new ArgumentNullException("AppKey");
                }
                if (AppSecret == null)
                {
                    throw new ArgumentNullException("AppSecret");
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
    }
}