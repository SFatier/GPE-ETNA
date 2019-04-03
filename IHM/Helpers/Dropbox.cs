using Com.CloudRail.SI;
using Com.CloudRail.SI.Interfaces;
using Com.CloudRail.SI.ServiceCode.Commands.CodeRedirect;
using Com.CloudRail.SI.Types;
using Dropbox.Api;
using Dropbox.Api.Sharing;
using IHM.Helpers;
using IHM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace GPE
{
    public class DropBoxCloud : DriveBase
    {

        ICloudStorage serviceCloudStorage;
        private Utilisateur u = Singleton.GetInstance().GetUtilisateur();

        public DropBoxCloud()
        {
            CloudRail.AppKey = Constant.LicenceCloudRail;
            Com.CloudRail.SI.Services.Dropbox dropboxdrive = new Com.CloudRail.SI.Services.Dropbox(new LocalReceiver(8082),Constant.strAppKey,Constant.strAppSecret, "http://localhost:8082/authorize", "someState");
            serviceCloudStorage = dropboxdrive;            
        }

        public override string Connect()
        {
            serviceCloudStorage.Login();
            String result = serviceCloudStorage.SaveAsString();
            serviceCloudStorage.LoadAsString(result);
            return result;
        }

        public override string GetCompteClient()
        {
            if (serviceCloudStorage == null)
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);

            return serviceCloudStorage.GetUserLogin();
        }

        public override List<Fichier> GetItemsNoShared()
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
            List<CloudMetaData> result = serviceCloudStorage.GetChildren("/");
            List<Fichier> FileList = ConvertMedatadaToFile(result);
            return FileList.OrderBy(f => f.Nom).ToList();
        }

        public override List<Fichier> GetItems()
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
            List<CloudMetaData> result = serviceCloudStorage.GetChildren("/");
            List<Fichier> FileList = ConvertMedatadaToFile(result);
            FileList.AddRange(GetFilesShared());
            return FileList.OrderBy(f => f.Nom).ToList();
        }

        public override Fichier GetItemsByPath(string path)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
            try { 
                CloudMetaData result = serviceCloudStorage.GetMetadata(path);
                Fichier File = new Fichier();
                File.Nom = result.GetName();
                File.Taille = result.GetSize().ToString();
                File.path = result.GetPath();
                File.Type = (result.GetFolder() ? "dossier de fichiers" : (Path.GetExtension(result.GetName()).Split('.').Length != 2 ? "" : Path.GetExtension(result.GetName()).Split('.')[1]));
                //File.IMG = c.GetImageMetaData().ToString();
                File.DateDeCreation = new DateTime(result.GetModifiedAt());
                return File;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
}

        public override List<Fichier> GetItemsFolder(string folderPath)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
            List<CloudMetaData> result = serviceCloudStorage.GetChildren(folderPath);
            List<Fichier> FileList = ConvertMedatadaToFile(result);
            return FileList.OrderBy(f => f.Nom).ToList();
        }

        public override bool CreateFolder(string path, string nameFolder)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
                serviceCloudStorage.CreateFolder(path + nameFolder);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Delete(string path)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
                serviceCloudStorage.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Download(string pathdropbox, string pathLocal)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
            try
            {
                Stream stream = serviceCloudStorage.Download(pathdropbox);

                using (var fileStream = new FileStream(pathLocal, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public override bool FolderExists(string path)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
            return serviceCloudStorage.Exists("/myFolder");
        }

        public override List<Fichier> Search(string str)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
            List<CloudMetaData> result = serviceCloudStorage.Search(str);
            List<Fichier> FileList = ConvertMedatadaToFile(result);
            return FileList.OrderBy(f => f.Nom).ToList(); ;
        }

        public override bool Upload(string path)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
                String pathToFile = "/" + path.Split(Path.DirectorySeparatorChar).Last();

                String strPhoto = (@path);
                FileStream fs = new FileStream(strPhoto, FileMode.Open, FileAccess.Read);
                serviceCloudStorage.Upload(pathToFile, fs, fs.Length, true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public override string CreateShareLink(string filepath)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
            return serviceCloudStorage.CreateShareLink(filepath);
        }

        public override bool Watch(string nom, string pathDropbox)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
                string pathLocal = System.IO.Path.GetTempPath() + nom.Replace(" ", "");
                if (System.IO.File.Exists(pathLocal))
                    System.IO.File.Delete(pathLocal);

                Download(pathDropbox, pathLocal);
                System.Diagnostics.Process.Start(pathLocal);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        
        public override List<Fichier> GetFilesShared()
        {
            DropboxClient DBClient = GetDBClient(u);
            List<Fichier> lstFiles;
            try
            {
                lstFiles = new List<Fichier>();
                var ListReceivedFiles = DBClient.Sharing.ListReceivedFilesAsync(100, null).Result.Entries;

                foreach (var metadata in ListReceivedFiles)
                {
                    var type = Path.GetExtension(metadata.Name).Split('.')[1];
                    //string IMG = Singleton.GetInstance().GetHomeModelView().lMVM.GetIcoByType(type);

                    Fichier f = new Fichier(string.Empty, metadata.Name, null, type, null, null, string.Empty, true);
                    f.PreviewUrl = metadata.PreviewUrl;
                    f.DateInvitation = metadata.TimeInvited;
                    f.IdDropbox = metadata.Id;

                    lstFiles.Add(f);
                }
                return lstFiles;
            }
            catch (Exception)
            {
                return lstFiles = new List<Fichier>();
            }
        }

        private DropboxClient GetDBClient(Utilisateur u)
        {
            string AccessTocken = "";
            List<Dictionary<string, string>> obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(u.CrededentielCloudRailDropbox);

            foreach (Dictionary<string, string> lst in obj)
            {
                foreach (KeyValuePair<string, string> item in lst)
                {
                    AccessTocken = item.Value;
                    break;
                }
                break;
            }
            DropboxClientConfig CC = new DropboxClientConfig("TestApp", 1);
            HttpClient HTC = new HttpClient();
            HTC.Timeout = TimeSpan.FromMinutes(10);
            CC.HttpClient = HTC;
            return new DropboxClient(AccessTocken, CC);
        }

        public override bool getSpace()
        {
            DropboxClient DBClient = GetDBClient(u);
            try
            {
                espace_utilise = (long)DBClient.Users.GetSpaceUsageAsync().Result.Used;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool SharingFile(Fichier fichier, Utilisateur utilisateur)
        {
            DropboxClient DBClient = GetDBClient(u);
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

        public override string CreateShareLink(Fichier fichier)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailDropbox);
                string link = serviceCloudStorage.CreateShareLink(fichier.path);
                return link;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
