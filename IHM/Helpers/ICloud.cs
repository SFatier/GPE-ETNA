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
    public enum  Drive {
        DP = 0, GG = 1
    }

    public class Cloud : ICloud
    {
        /// <summary>
        /// Télécharge un fichier correspondant à un cloud.
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="FolderPath"></param>
        /// <param name="FileName"></param>
        /// <param name="DownloadFolderPath"></param>
        /// <param name="DownloadFileName"></param>
        /// <returns></returns>
        public bool Download(Drive drive, string FolderPath, string FileName, string DownloadFolderPath, string DownloadFileName)
        {
            switch (drive)
            {
                case Drive.DP:
                    Singleton.GetInstance().GetDBB().Download(FolderPath, FileName, DownloadFolderPath, DownloadFileName);
                    break;
                case Drive.GG:
                    Singleton.GetInstance().GetGoogle().Download();
                    break;
            }
            return true;
        }

        /// <summary>
        /// Récupère la listes des fichiers correspondant à un cloud
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public List<Files> GetItems(Drive drive)
        {
            List<Files> lst = new List<Files>();

            switch (drive)
            {
                case Drive.DP:
                    Singleton.GetInstance().GetDBB().GetItems();
                    break;
                case Drive.GG:
                    Singleton.GetInstance().GetGoogle().GetItems();
                    break;
            }
            return lst;
        }
    }

    public interface ICloud
    {
        List<Files> GetItems(Drive drive);
        bool Download(Drive drive, string DropboxFolderPath, string DropboxFileName, string DownloadFolderPath, string DownloadFileName);
    }
}
