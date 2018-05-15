﻿using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using IHM.Helpers;
using IHM.Model;
using IHM.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GPE
{
    public class DropBox 
    {
        #region Variables  
        public DropboxClient DBClient;
        public long espace_utilise;
        private string oauth2State;

         private const string RedirectUri = "https://localhost/authorize"; // Same as we have configured Under [Application] -> settings -> redirect URIs.  
        #endregion

        #region Constructor  
        public DropBox(string ApiKey, string ApiSecret, string ApplicationName = "TestApp")
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
        /// Génère l'url pour demander l'autorisation de connection à dropbox
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
        /// Génère le token après autorisation 
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

        /// <summary>
        /// Récupère le Dbclient de dropbox
        /// </summary>
        /// <param name="AccessTocken"></param>
        public void GetDBClient(string AccessTocken)
        {
            DropboxClientConfig CC = new DropboxClientConfig(AppName, 1);
            HttpClient HTC = new HttpClient();
            HTC.Timeout = TimeSpan.FromMinutes(10);
            CC.HttpClient = HTC;
            DBClient = new DropboxClient(AccessTocken, CC);
        }

        /// <summary>  
        /// Création d'un dossier dans dropbox 
        /// </summary>  
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
        ///  Vérifie l'existance d'un dossier
        /// </summary>  
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
        /// Fonction qui supprime un dossier ou un fichier du dropbox connecté
        /// </summary>   
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
        /// Fonction d'importation depuis dropbox
        /// </summary>  
        /// <param name="UploadfolderPath">Chemin du fichier dans dropbox</param>  
        /// <param name="UploadfileName">Nom du fichier lors de l'importation</param>  
        /// <param name="SourceFilePath"> Chemin de destination du fichier sur le poste local</param>  
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
        /// Fonction de téléchargement de fichier depuis dropbox 
        /// </summary>  
        /// <param name="DropboxFolderPath">Dossier cible de dropbox</param>  
        /// <param name="DropboxFileName">Nom du fichier qui sera dans dropbox</param>  
        /// <param name="DownloadFolderPath">Dossier local qui possède le fichier</param>  
        /// <param name="DownloadFileName">Nom du fichier en local</param>  
        /// <returns></returns>  
        public bool Download(string DropboxFolderPath, string DropboxFileName, string DownloadFolderPath, string DownloadFileName)
        {
            try
            {
                var response = DBClient.Files.DownloadAsync(DropboxFolderPath + DropboxFileName);
                using (var fileStream = File.Create(DownloadFolderPath))
                {
                    response.Result.GetContentAsStreamAsync().Result.CopyTo(fileStream); //Added to wait for the result from Async method  
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        /// <summary>
        /// Récupère la liste des fichiers et dossiers du compte dropbox connecté
        /// </summary>
        public List<Files> GetItems()
        {
            var liste = DBClient.Files.ListFolderAsync(string.Empty);
            var Cursor = liste.Result.Cursor;
            var Entries = liste.Result.Entries;
            return GetFolderAndFiles(Entries.ToList());
        }

        /// <summary>
        /// Demande a consulté le fichier
        /// </summary>
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

        /// <summary>
        /// Récupère les fichiers que l'on a partagé avec le compte connecté
        ///</summary>
        public List<Files> GetFilesShared()
        {
            List<Files> lstFiles;
            try
            {
                lstFiles = new List<Files>();               
                var ListReceivedFiles = DBClient.Sharing.ListReceivedFilesAsync( 100,  null).Result.Entries;

                foreach (var metadata in ListReceivedFiles)
                {
                    var type = Path.GetExtension(metadata.Name);
                    string IMG = Singleton.GetInstance().GetHomeModelView().lMVM.GetIcoByType(type);
                    
                    Files f = new Files(string.Empty, metadata.Name, IMG, type, null, null, string.Empty, true);
                    f.PreviewUrl = metadata.PreviewUrl;
                    f.DateInvitation = metadata.TimeInvited;
                    f.IdDropbox = metadata.Id;

                    lstFiles.Add(f);
                }
                return lstFiles;
            }
            catch (Exception)
            {
                return lstFiles = new List<Files>();
            }
        }
    
        /// <summary>  
        ///Récupère l'espace utilisé
        /// </summary>  
        public bool getSpace()
        {
            if (DBClient != null)
            {
                espace_utilise = (long)DBClient.Users.GetSpaceUsageAsync().Result.Used;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>  
        /// Récupère les dossiers et fichiers d'un dossier 
        /// </summary> 
        public List<Files> GetItemsFolder(string folderPath)
        {
            List < Metadata > Entries =  new List<Metadata>(DBClient.Files.ListFolderAsync(folderPath).Result.Entries);
            return GetFolderAndFiles (Entries);
        }

        /// <summary>  
        /// Récupération des dossiers et des fichiers
        /// <summary>  
        public List<Files> GetFolderAndFiles(List<Metadata> Entries)
        {
            List<Files> lstFiles = new List<Files>();

            // folder
            List<String> lstFolder = new List<string>();
            foreach (var item in Entries.Where(i => i.IsFolder))
            {
                string IdFile = item.AsFolder.Id;
                string nom = item.Name;
                string type = "dossier de fichiers";
                string IMG = Singleton.GetInstance().GetHomeModelView().lMVM.GetIcoByType("dossier");
                string taille = "";
                string path = item.PathDisplay;
                Files f = new Files(IdFile, nom, IMG, type, null, null, taille, false);
                f.path = path;
                lstFiles.Add(f);
            }

            //Files
            foreach (var item in Entries.Where(i => i.IsFile))
            {
                string IdFile = item.AsFile.Id;
                string nom = item.Name;
                var type = Path.GetExtension(item.Name);
                string IMG = Singleton.GetInstance().GetHomeModelView().lMVM.GetIcoByType(type);
                DateTime dateDeCreation = Convert.ToDateTime(item.AsFile.ClientModified.ToString("f",  CultureInfo.CreateSpecificCulture("fr-FR")));
                DateTime ModifieLe = Convert.ToDateTime( item.AsFile.ServerModified.ToString("f",  CultureInfo.CreateSpecificCulture("fr-FR")));
                string taille = Convert.ToInt32(((item.AsFile.Size / 1024f) / 1024f) * 1024).ToString();
                string path = item.PathDisplay;
                Files f = new Files(IdFile, nom, IMG, type, dateDeCreation, ModifieLe, taille, true);
                f.path = path;
                lstFiles.Add(f);
            }
            return lstFiles;
        }

        #endregion

        #region Validation Methods  
        /// <summary>  
        /// Vérifie que la clé et l'id secret de drobox n'est pas vide.  
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