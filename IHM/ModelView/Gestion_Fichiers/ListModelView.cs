using GPE;
using IHM.Helpers;
using IHM.Model;
using IHM.View;
using IHM.ViewModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class ListModelView : ObservableObject, IPageViewModel
    {
        private static string path_img = ConfigurationSettings.AppSettings["FolderIMG"];
        private Utilisateur cUtilisateur = Singleton.GetInstance().GetUtilisateur();
        public string Name => "Liste des documents du cloud dropbox";

        public ICommand LinkProject { get; set; }
        public ICommand Supprimer { get; set; }
        public ICommand CreateFolder { get; set; }
        public ICommand ReloadDataGrid { get; set; }
        public ICommand Upload { get; set; }
        public ICommand Recherche { get; set; }
        public ICommand RechercheDate { get; set; }
        public ICommand RecherchePeriode { get; set; }
        public ICommand Download { get; set; }
        public ICommand Open { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public ListModelView()
        {
            DgFiles = new List<List<Fichier>>(); //init de la liste
            DgFiles.Add(new List<Fichier>()); //init list dropbox
            DgFiles.Add(new List<Fichier>()); //init list Google
            RefreshTab();
            LoadProject();
            LoadAction();
        }

        public void LoadAction()
        {
            LinkProject = new RelayCommand(ActionLinkProject);
            Supprimer = new RelayCommand(ActionSupprimer);
            CreateFolder = new RelayCommand(ActionCreateFolder);
            ReloadDataGrid = new RelayCommand(ActionReloadDataGrid);
            Upload = new RelayCommand(ActionUpload);
            Download = new RelayCommand(ActionDownload);
            Open = new RelayCommand(ActionOpen);
            Recherche = new RelayCommand(ActionRecherche);
            RechercheDate = new RelayCommand(ActionRechercheDate);
            RecherchePeriode = new RelayCommand(ActionRecherchePeriode);
        }

        public void LoadProject()
        {
            List<Projet> items;
            try
            {
                StreamReader r;
                using (r = new StreamReader(@ConfigurationSettings.AppSettings["ProjetJSON"]))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Projet>>(json);
                    LstProjets = items.OrderByDescending(x => x.DateDeCreation).Select(x => x.Nom).ToList();
                }
            } catch (Exception)
            {
                items = new List<Projet>();
                LstProjets = new List<string>();
            }
            Singleton.GetInstance().SetListProject(items);
        }

        #region [Binding dgFiles By Drive]
        private List<Fichier> dgFiles_DP;
        public List<Fichier> DgFiles_DP
        {
            get { return dgFiles_DP; }
            set
            {
                dgFiles_DP = value;
                RaisePropertyChanged(nameof(DgFiles_DP));
            }
        }

        private List<Fichier> dgFiles_GG;
        public List<Fichier> DgFiles_GG
        {
            get { return dgFiles_GG; }
            set
            {
                dgFiles_GG = value;
                RaisePropertyChanged(nameof(DgFiles_GG));
            }
        }
        #endregion

        #region [Binding]

        private List<string> lstProjets;
        public List<string> LstProjets
        {
            get { return lstProjets; }
            set {
                lstProjets = value;
                RaisePropertyChanged(nameof(LstProjets));
            }
        }

        private string _ProjetFiltre;
        public string ProjetFiltre
        {
            get { return this._ProjetFiltre; }
            set
            {
                if (!string.Equals(this._ProjetFiltre, value))
                {
                    if (value != _ProjetFiltre)
                    {
                        RechercheMetadataByProjet(value);
                    }
                    RaisePropertyChanged(nameof(ProjetFiltre));
                }
            }
        }
       
        private List<Fichier> _Results;
        public List<Fichier> Results
        {
            get { return this._Results; }
            set
            {
                if (!string.Equals(this._Results, value))
                {
                    this._Results = value;
                    RaisePropertyChanged(nameof(Results));
                }
            }
        }

        private List<List<Fichier>> _DgFiles;
        public List<List<Fichier>>  DgFiles
        {
            get { return this._DgFiles; }
            set
            {
                if (!string.Equals(this._DgFiles, value))
                {
                    this._DgFiles = value;
                    RaisePropertyChanged(nameof(DgFiles));
                }
            }
        }

        private string _Date;
        public string Date
        {
            get { return this._Date; }
            set
            {
                if (!string.Equals(this._Date, value))
                {
                    this._Date = value;
                    RaisePropertyChanged(nameof(Date));
                }
            }
        }

        private Fichier _filesSelected;
        public Fichier filesSelected
        {
            get { return this._filesSelected; }
            set
            {
                if (!string.Equals(this._filesSelected, value))
                {
                    this._filesSelected = value;
                    RaisePropertyChanged(nameof(filesSelected));
                }
            }
        }

        private string _Nom;
        public string Nom
        {
            get { return this._Nom; }
            set
            {
                if (!string.Equals(this._Nom, value))
                {
                    this._Nom = value;
                    RaisePropertyChanged(nameof(Nom));
                }
            }
        }
        
        //Recherche binding
        private DateTime _dateDebut;
        public DateTime dateDebut
        {
            get { return _dateDebut; }
            set
            {
                if (!DateTime.Equals(this._dateDebut, value))
                {
                    this._dateDebut = value;
                    RaisePropertyChanged(nameof(dateDebut));
                }
            }
        }
        private DateTime _dateFin;
        public DateTime dateFin
        {
            get { return _dateFin; }
            set
            {
                if (!DateTime.Equals(this._dateFin, value))
                {
                    this._dateFin = value;
                    RaisePropertyChanged(nameof(dateFin));
                }
            }
        }

        #endregion

        #region [Methods]

        public void RefreshTab()
        {
            DgFiles_DP = DgFiles[0];
            DgFiles_GG = DgFiles[1];
        }

        /// <summary>
        /// Récupère les items d'un dossier
        /// </summary>
        public void GetFolder()
        {
            if (filesSelected != null && filesSelected.IsFile == false)
            {
                DgFiles.Clear();
                if (filesSelected.IdDropbox != null)
                {
                    DgFiles[0] = Singleton.GetInstance().GetDBB().GetItemsFolder(filesSelected.path);
                }
                else
                {
                    //DgFiles[1].Add()
                }
            }
        }

        /// <summary>
        /// Recherche les fichiers d'un projet
        /// </summary>
        /// <param name="value"></param>
        private void RechercheMetadataByProjet(string value)
        {
            Singleton.GetInstance().GetHomeModelView().GetFilesDropbox();

            Projet projet = Singleton.GetInstance().GetAllProject().FirstOrDefault(x => x.Nom.Equals(value));

            List<Fichier> lst = new List<Fichier>();

            if (projet.LstFiles != null)
            {
                projet.LstFiles.ForEach(metadata =>
                {

                    if (metadata.IsFile == false)
                    {
                        var resultat = VerificationFichier(metadata.path);
                        if (resultat.Count() > 0)
                        {
                            resultat.ForEach(item =>
                            {
                                lst.Add(item);
                            });
                        }
                    }
                    else
                    {
                        lst.Add(metadata);
                    }
                });
            }
            DgFiles[0] = lst;
        }

        /// <summary>
        /// Vérifie si un dossier possède des autres dossiers ainsi que ses fichiers
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NomFichier"></param>
        /// <returns></returns>
        private List<Fichier> VerificationFichier(string path)
        {
            List<Fichier> rslt = new List<Fichier>();
            List<Fichier> lst = Singleton.GetInstance().GetDBB().GetItemsFolder(path);
            if (lst.Count() > 0)
            {
                lst.ForEach(doc =>
                {
                    if (doc.IsFile == false)
                    {
                        VerificationFichier(doc.path);
                    }
                    else
                    {
                        rslt.Add(doc);
                    }
                });
            }
            return rslt;
        }

        /// <summary>
        /// Récupère une ico en fonction du type de l'image
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetIcoByType(string type)
        {
            string str = string.Empty;

            switch (type)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    str = "image.ico";
                    break;
                case ".txt":
                    str = "text.ico";
                    break;
                case ".doc":
                case ".docx":
                    str = "doc.ico";
                    break;
                case ".pdf":
                    str = "pdf.ico";
                    break;
                case ".csv":
                case ".excel":
                    str = "excel.ico";
                    break;
                case "dossier":
                    str = "folder.ico";
                    break;
            }
            return path_img + str;
        }
      
        #endregion

        #region [Action ICommand]

        /// <summary>
        /// Lie un fichier à un projet 
        /// Partage le fichier/dossier aux utilisateurs
        /// </summary>
        /// <param name="parameter"></param>
        private void ActionLinkProject(object parameter)
        {
            if (filesSelected != null)
            {
                PopUp app = new PopUp();
                PopUpModelView context = new PopUpModelView(app, filesSelected);
                Singleton.GetInstance().SetPopUp(context);
                app.DataContext = context;
                app.Show();
            }
            else
            {
                MessageBox.Show("Aucun fichier(s) sélectioné(s).");
            }
        }

        /// <summary>
        /// Supprimer un dossier ou un fichier
        /// </summary>
        /// <param name="parameter"></param>
        private void ActionSupprimer(object parameter)
        {
            if (filesSelected != null)
            {
                MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer  " + filesSelected.Nom + "?", "Infos", MessageBoxButton.YesNo);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (filesSelected.IdDropbox != null)
                        {
                            string test = filesSelected.path;
                            Singleton.GetInstance().GetCloud().Delete(Drive.DP, filesSelected.path, string.Empty);
                        }
                        else
                        {
                            Singleton.GetInstance().GetCloud().Delete(Drive.GG, string.Empty, filesSelected.IdGoogle);
                        }
                        MessageBox.Show("Fichier supprimé.");
                        break;
                    case MessageBoxResult.No:
                        //
                        break;
                }
            }
            else
            {
                MessageBox.Show("Aucun fichier(s) sélectioné(s).");
            }
        }

        /// <summary>
        /// Lie un fichier à un projet 
        /// Partage le fichier/dossier aux utilisateurs
        /// </summary>
        /// <param name="parameter"></param>
        private void ActionCreateFolder(object parameter)
        {
            try
            {
                Drive DriveChecked = Drive.GG;

                if (Drive.DP == DriveChecked)
                {
                    Singleton.GetInstance().GetCloud().FolderExists(Drive.DP, "/", "test__");
                }
                else
                {
                    Singleton.GetInstance().GetCloud().CreateFolder(Drive.GG, "/", "test__");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\"" + ex.Message);
            }
        }

        /// <summary>
        /// Reload Grid
        /// </summary>
        /// <param name="parameter"></param>
        private void ActionReloadDataGrid(object parameter)
        {
            /* try
             {
                 if (cUtilisateur.Token_DP != null)
                 {
                     Singleton.GetInstance().GetHomeModelView().GetFilesDropbox();
                     Singleton.GetInstance().GetHomeModelView().GetFilesShared();
                 }
                 if (cUtilisateur.Token_GG != null)
                 {
                     DgFiles[1] = Singleton.GetInstance().GetCloud().GetItems(Drive.GG);
                     if (DgFiles[0].Count > 0 && cUtilisateur.Token_DP != null)
                         DgFiles[0] = Singleton.GetInstance().GetCloud().GetItems(Drive.DP);
                 }
             } catch (Exception ex)
             {
                 MessageBox.Show("Error:\"" + ex.Message);
             }*/

            Singleton.GetInstance().GetHomeModelView().GetFilesDropbox();
            Singleton.GetInstance().GetHomeModelView().GetFilesGoogle();
            RefreshTab();
        }

        /// <summary>
        /// Upload un fichier
        /// </summary>
        /// <param name="paramater"></param>
        private void ActionUpload(object paramater)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Multiselect = true;

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string SourceFilePath = "/";

            if (openFileDialog.ShowDialog() == true)
            {
                SourceFilePath = openFileDialog.FileName;
                Singleton.GetInstance().GetDBB().Upload("/", Path.GetFileName(SourceFilePath), SourceFilePath);
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="paramater"></param>
        private void ActionDownload(object paramater)
        {

            if (filesSelected != null)
            {
                if (filesSelected.IdGoogle != null)
                {

                    string fileName, fileId, mimeType;
                    fileName = filesSelected.Nom;
                    mimeType = filesSelected.Type;
                    fileId = filesSelected.IdDropbox;
                  
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = filesSelected.Nom;
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var DownloadFolderPath = saveFileDialog.FileName.Replace("\\", "/");
                        Singleton.GetInstance().GetCloud().Download(Drive.GG, "", fileName, DownloadFolderPath, fileName, filesSelected.IdGoogle, "");

                    }
                    else
                    {
                        MessageBox.Show("Aucun fichier(s) sélectioné(s).");
                    }


                }
                else
                {
                    string DropboxFolderPath = filesSelected.path;
                    string DropboxFileName = filesSelected.Nom;
                    string DownloadFolderPath = "";
                    string DownloadFileName = "";
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = filesSelected.Nom;

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string test = filesSelected.path;
                        DownloadFolderPath = saveFileDialog.FileName.Replace("\\", "/");
                        DownloadFileName = Path.GetFileName(saveFileDialog.FileName);
                        Singleton.GetInstance().GetCloud().Download(Drive.DP, "/", DropboxFileName, DownloadFolderPath, DownloadFileName,string.Empty, string.Empty);
                    
                    }
                    else
                    {
                        MessageBox.Show("Aucun fichier(s) sélectioné(s).");
                    }
                }
            }
    }

        /// <summary>
        /// Action qui visualise un fichier
        /// </summary>
        /// <param name="paramater"></param>
        private void ActionOpen(object paramater)
        {
            if (filesSelected != null)
            {
                if (filesSelected.IdDropbox != null)
                {
                    if (filesSelected.PreviewUrl == null)
                    {
                        string DropboxFileName = filesSelected.Nom;
                        string DropboxFolderPath = filesSelected.path;
                        string fileName = System.IO.Path.GetTempPath() + DropboxFileName;
                        Singleton.GetInstance().GetCloud().Download(Drive.GG, "", fileName, "" , fileName, filesSelected.IdGoogle, Path.GetExtension(fileName));
                        System.Diagnostics.Process.Start(fileName);
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(filesSelected.PreviewUrl);
                    }
                }
                else
                {
                    Singleton.GetInstance().GetCloud().Watch(Drive.GG, filesSelected.IdGoogle);
                }
            }
            else
            {
                MessageBox.Show("Aucun fichier(s) sélectioné(s).");
            }
        }


        // search Files
        private void ActionRecherche(object par)
        {

            Results = new List<Fichier>();
            char[] delimiters = new char[] { ' ', ',', '.', ':', '\t' };
            string[] words = Nom.Split(delimiters);
            bool trouve = false;

            if (DgFiles[0].Count() > 0)
            {
                foreach (Fichier item in DgFiles[0])
                {

                    if (words.Any(nomRechercher => nomRechercher == item.Nom))
                    {
                        trouve = true;
                        Results.Add(item);


                    }
                    DgFiles[0] = Results;
                }
                if (trouve == false)
                {
                    MessageBox.Show("Le fichier avec le nom indiqué n’existe pas");
                }
            }
        }
        public void Recherche_Periode()
        {
            Results = new List<Fichier>();
            var lstFilesDropbox = Singleton.GetInstance().GetCloud().GetItems(Drive.DP);

            if (dateDebut < dateFin)
            {
                throw new ArgumentException("endDate doit être supérieur ou égal à  startDate");
            }

            foreach (Fichier item in lstFilesDropbox)
            {
                if ((item.DateDeCreation != null && item.DateDeCreation > dateFin && item.DateDeCreation > dateDebut) ||
                                        (item.DateInvitation != null && item.DateInvitation > this.dateFin && item.DateInvitation > dateDebut))
                {
                    dateDebut = dateDebut.AddDays(1);
                    Console.WriteLine(Results);
                    Results.Add(item);
                }
            }
            DgFiles[0] = Results;
        }

        private void ActionRecherchePeriode(object obj)
        {
            Recherche_Periode();
        }
        public void Recherche_Date()
        {
            string dateDebut = this.Date;
            char[] delimiters = new char[] { '/', ' ' };
            string[] words = dateDebut.Split(delimiters);
            int month = int.Parse(words[0]);
            int day = int.Parse(words[1]);
            int year = int.Parse(words[2]);

            Results = new List<Fichier>();
            bool trouve = false;

            var lstFilesDropbox = Singleton.GetInstance().GetDBB().GetItems();

            foreach (Fichier item in lstFilesDropbox)
            {
                if (item.DateDeCreation != null)
                {
                    if (item.DateDeCreation.Value.Year == year && item.DateDeCreation.Value.Month == month && item.DateDeCreation.Value.Day == day)
                    {
                        trouve = true;
                        Results.Add(item);
                        Console.WriteLine(Results);
                    }
                }

                if (item.DateInvitation != null)
                {
                    if (item.DateInvitation.Value.Year == year && item.DateInvitation.Value.Month == month && item.DateInvitation.Value.Day == day)
                    {
                        trouve = true;
                        Results.Add(item);
                        Console.WriteLine(Results);

                    }
                }
            }
            if (trouve == false)
            {
                MessageBox.Show("La date séléctioné  n’existe pas");
            }

            DgFiles[0] = Results;
        }

        private void ActionRechercheDate(object obj)
        {
            Recherche_Date();
        }

        #endregion
    }
}