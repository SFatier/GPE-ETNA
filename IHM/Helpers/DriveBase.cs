using Com.CloudRail.SI.Types;
using IHM.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace IHM.Helpers
{
    public abstract class DriveBase
    {

        public long espace_utilise;

        #region [Constructeur]
        public DriveBase() { }
        public DriveBase(string ApiKey, string ApiSecret, string ApplicationName = "TestApp") { }
        #endregion

        public abstract string Connect();
        public abstract bool CreateFolder(string path, string nameFolder);
        public abstract bool Delete(string path);
        public abstract string GetCompteClient();
        public abstract bool Download(string pathGoogle, string pathLocal);
        public abstract List<Fichier> GetItemsFolder(string folderPath);
        public abstract List<Fichier> GetItems();
        public abstract bool FolderExists(string path);
        public abstract List<Fichier> Search(string str);
        public abstract bool Upload(string path);
        public abstract string CreateShareLink(string filepath);
        public abstract bool Watch(string nom, string path);
        public abstract Fichier GetItemsByPath(string path);
        public abstract List<Fichier> GetItemsNoShared();
        public abstract string CreateShareLink(Fichier fichier);

        //**//
        public abstract List<Fichier> GetFilesShared();
        public abstract bool getSpace();
        public abstract bool SharingFile( Fichier fichier, Utilisateur utilisateur);
       

        protected List<Fichier> ConvertMedatadaToFile(List<CloudMetaData> result)
        {
            List<Fichier> FileList = new List<Fichier>();
            foreach (CloudMetaData c in result)
            {
                Fichier File = new Fichier();
                File.Nom = c.GetName();
                File.Taille = c.GetSize().ToString();
                File.path = c.GetPath();
                File.Type = (c.GetFolder() ? "dossier de fichiers" : (Path.GetExtension(c.GetName()).Split('.').Length != 2 ? "" : Path.GetExtension(c.GetName()).Split('.')[1]));
                //File.IMG = c.GetImageMetaData().ToString();
                File.DateDeCreation = new DateTime(c.GetModifiedAt());
                FileList.Add(File);
            }
            return FileList;
        }

    }
}
