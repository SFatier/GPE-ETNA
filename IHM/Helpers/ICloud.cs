using Google.Apis.Drive.v3.Data;
using IHM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Helpers
{
    /// <summary>
    /// Enumération des types de clouds utilisés dans l'application
    /// </summary>
    public enum Drive {
        DP = 0, GG = 1
    }

    /// <summary>
    /// Classe permettant de selectionner la bonne fonction en fonction du cloud
    /// </summary>
    public class Cloud : ICloud
    {
        /// <summary>
        /// Creer un dossier sur un cloud
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CreateFolder(Drive drive, string path, string nameFolder)
        {
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        Singleton.GetInstance().GetDBB().CreateFolder(path);
                        break;
                    case Drive.GG:
                        Singleton.GetInstance().GetGoogle().CreateFolder(nameFolder);
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Supprime un fichier en fonction de son Id ou son chemin
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Delete(Drive drive, string path, string fileId)
        {
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        Singleton.GetInstance().GetDBB().Delete(path);
                        break;
                    case Drive.GG:
                        Singleton.GetInstance().GetGoogle().Delete(fileId);
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Télécharge un fichier correspondant à un cloud.
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="FolderPath"></param>
        /// <param name="FileName"></param>
        /// <param name="DownloadFolderPath"></param>
        /// <param name="DownloadFileName"></param>
        /// <returns></returns>
        public bool Download(Drive drive, string FolderPath, string FileName, string DownloadFolderPath, string DownloadFileName, string fileId, string mimeType)
        {
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        Singleton.GetInstance().GetDBB().Download(FolderPath, FileName, DownloadFolderPath, DownloadFileName);
                        break;
                    case Drive.GG:
                      Singleton.GetInstance().GetGoogle().Download(FileName,  fileId, DownloadFolderPath);
                        break;
                }
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Vérifie si un dossier existe avant de le creer
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FolderExists(Drive drive, string path, string nameFolder)
        {
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        Singleton.GetInstance().GetDBB().FolderExists(path);
                        break;
                    case Drive.GG:
                        Singleton.GetInstance().GetGoogle().FolderExists(nameFolder);
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Récupère les fichiers en consultation
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public List<Fichier> GetFilesShared(Drive drive)
        {
            List<Fichier> list = new List<Fichier>();
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        Singleton.GetInstance().GetDBB().GetFilesShared();
                        break;
                    case Drive.GG:
                        Singleton.GetInstance().GetGoogle().GetFilesShared();
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        /// <summary>
        /// Récupère la listes des fichiers correspondant à un cloud
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public List<Fichier> GetItems(Drive drive)
        {
            List<Fichier> lst = new List<Fichier>();

            try {
                switch (drive)
                {
                    case Drive.DP:
                        if (Singleton.GetInstance().GetDBB() != null)
                            lst = Singleton.GetInstance().GetDBB().GetItems();
                        break;
                    case Drive.GG:
                        if (Singleton.GetInstance().GetGoogle() != null)
                            lst = Singleton.GetInstance().GetGoogle().GetItems();
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lst;
        }

        /// <summary>
        /// Récupèr l'espace disponible sur le drive
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public bool getSpace(Drive drive)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Partage un fichier avec un utilisateur
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="fichier"></param>
        /// <param name="utilisateur"></param>
        /// <returns></returns>
        public bool SharingFile(Drive drive, Fichier fichier, Utilisateur utilisateur)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upload un fichier sur un drive
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="UploadfolderPath"></param>
        /// <param name="UploadfileName"></param>
        /// <param name="SourceFilePath"></param>
        /// <returns></returns>
        public bool Upload(Drive drive, string UploadfolderPath, string UploadfileName, string SourceFilePath, string _parent)
        {
            try
            {
                switch (drive)
                {
                    case Drive.DP:
                        Singleton.GetInstance().GetDBB().Upload(UploadfolderPath,  UploadfileName,  SourceFilePath);
                        break;
                    case Drive.GG:
                        Singleton.GetInstance().GetGoogle().Upload(UploadfolderPath, UploadfileName, SourceFilePath);
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Visualiser un fichier
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public Channel Watch(Drive drive, string fileId)
        {
            Channel channel = new Channel();
            try
            {
                if(drive == Drive.GG)
                    channel = Singleton.GetInstance().GetGoogle().Watch(fileId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return channel;
        }
    }

    public interface ICloud
    {
        List<Fichier> GetItems(Drive drive);
        bool Download(Drive drive, string DropboxFolderPath, string DropboxFileName, string DownloadFolderPath, string DownloadFileName, string fileId, string mimeType);
        bool CreateFolder(Drive drive, string path, string nameFolder);
        bool FolderExists(Drive drive, string path, string nameFolder);
        bool Delete(Drive drive, string path, string fileId);
        bool Upload(Drive drive, string UploadfolderPath, string UploadfileName, string SourceFilePath,string _parent);
        bool SharingFile(Drive drive, Fichier fichier, Utilisateur utilisateur);
        List<Fichier> GetFilesShared(Drive drive);
        bool getSpace(Drive drive);
    }
}