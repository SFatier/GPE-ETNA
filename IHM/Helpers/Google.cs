
using Com.CloudRail.SI;
using Com.CloudRail.SI.Interfaces;
using Com.CloudRail.SI.ServiceCode.Commands.CodeRedirect;
using Com.CloudRail.SI.Services;
using Com.CloudRail.SI.Types;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Download;
using IHM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using File = Google.Apis.Drive.v3.Data.File;


namespace IHM.Helpers
{
    public class GoogleCloud : DriveBase
    {
        ICloudStorage serviceCloudStorage = null;
        private Utilisateur u = Singleton.GetInstance().GetUtilisateur();

        public GoogleCloud()  {

            CloudRail.AppKey = Constant.LicenceCloudRail;
            GoogleDrive googledrive =  new GoogleDrive(new LocalReceiver(8082), Constant.GoogleKey, Constant.GooGleSecret,"http://localhost:8082/auth", "someState");
            serviceCloudStorage = googledrive;
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
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);

            return serviceCloudStorage.GetUserLogin();
        }

        public override List<Fichier> GetItems()
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            List<CloudMetaData> result = serviceCloudStorage.GetChildren("/");
            List<Fichier> FileList = ConvertMedatadaToFile(result);
            return FileList.OrderBy(f => f.Nom).ToList(); 
        }
        
        public override List<Fichier> GetItemsFolder(string folderPath)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            List<CloudMetaData> result = serviceCloudStorage.GetChildren(folderPath);
            List<Fichier> FileList = ConvertMedatadaToFile(result);
            return FileList.OrderBy(f => f.Nom).ToList();
        }
        
        public override bool CreateFolder( string path, string nameFolder)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
                serviceCloudStorage.CreateFolder(path + nameFolder);
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public override bool Delete( string path)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
                serviceCloudStorage.Delete(path);
                return true;
            }catch(Exception)
            {
                return false;
            }
        }

        public override bool Download(string pathGoogle, string pathLocal)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);

            try
            {
                Stream stream = serviceCloudStorage.Download(pathGoogle);

                using (var fileStream = new FileStream(pathLocal, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                return true;
            }catch (Exception)
            {
                return false;
            }
        }

        public override bool FolderExists(string path)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            return serviceCloudStorage.Exists("/myFolder");
        }

        public override List<Fichier> Search(string str)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            List<CloudMetaData> result = serviceCloudStorage.Search( str);
            List<Fichier> FileList = ConvertMedatadaToFile(result);
            return FileList.OrderBy(f => f.Nom).ToList(); 
        }

        public override bool Upload(string path)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
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

        public override string CreateShareLink( string filepath)
        {
            serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
            return serviceCloudStorage.CreateShareLink(filepath);
        }

        public override bool Watch(string nom, string pathGoogle)
        {
            try
            {
                serviceCloudStorage.LoadAsString(u.CrededentielCloudRailGoogle);
                string pathLocal = System.IO.Path.GetTempPath() + nom;
                if (System.IO.File.Exists(pathLocal))
                    System.IO.File.Delete(pathLocal);

                Download(pathGoogle, pathLocal);
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
            throw new NotImplementedException();
        }

        public override bool getSpace()
        {
            throw new NotImplementedException();
        }

        public override bool SharingFile(Fichier fichier, Utilisateur utilisateur)
        {
            throw new NotImplementedException();
        }
                
    }
}
