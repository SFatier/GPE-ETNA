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
        public string Name => "Liste des documents du cloud dropbox";
        public List<Files> FilesShared { get; internal set; }

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

        //constructeur
        public ListModelView()
        {
            _DgFiles = new List<Files>();

            LoadProject();
            LoadIcon();
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
        }

        private void LoadIcon()
        {
            BtnEdit = path_img + "edit.png";
            BtnTrash = path_img + "trash.png";
            BtnOpen = path_img + "open.png";
            BtnAdd = path_img + "add.png";
            BtnReload = path_img + "reload.png";
            BtnUpload = path_img + "upload.png";
            BtnDownload = path_img + "download.png";
            BtnProject = path_img + "link.png";
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
                    LstProjets = items.OrderByDescending(x => x.DateDeCreation).Select(x=>x.Nom).ToList(); 
                }
            } catch (Exception)
            {
                items = new List<Projet>();
                LstProjets = new List<string>();
            }
            Singleton.GetInstance().SetListProject(items);
        }

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
                        Singleton.GetInstance().GetHomeModelView().GetFiles();
                        Singleton.GetInstance().GetHomeModelView().GetFilesShared();

                        List<Files> rslt = new List<Files>();
                        var lst = Singleton.GetInstance().GetAllProject().SingleOrDefault(x => x.Nom.Equals(value)).LstFiles;

                        if (lst.Count() > 0)
                        {
                            foreach (Files f in lst)
                            {
                                try
                                {
                                    var newDeck = f.path.Split('/');
                                    newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();

                                    var lstItems = Singleton.GetInstance().GetDBB().GetItemsFolder(string.Join("/", newDeck));

                                    if (lstItems.Count > 0)
                                        rslt.Add(lstItems[0]);

                                    DgFiles = rslt;
                                    this._ProjetFiltre = value;
                                }
                                catch (Exception)
                                {
                                    if (DgFiles.FirstOrDefault(x => x.IdDropbox.Equals(f.IdDropbox)) != null)
                                    {
                                        rslt.Add(DgFiles.FirstOrDefault(x => x.IdDropbox.Equals(f.IdDropbox)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Singleton.GetInstance().GetHomeModelView().GetFiles();
                            MessageBox.Show(lst.Count() == 0 ? "Aucun fichier associé." : "");
                        }
                    }
                    RaisePropertyChanged(nameof(ProjetFiltre));
                }
            }
        }

        private List<Files> _Results;
        public List<Files> Results
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

        private List<Files> _DgFiles;
        public List<Files> DgFiles
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

        private Files _filesSelected;
        public Files filesSelected
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

        private string _BtnEdit;
        public string BtnEdit
        {
            get { return this._BtnEdit; }
            set
            {
                if (!string.Equals(this._BtnEdit, value))
                {
                    this._BtnEdit = value;
                    RaisePropertyChanged(nameof(BtnEdit));
                }
            }
        }

        private string _BtnProject;
        public string BtnProject
        {
            get { return this._BtnProject; }
            set
            {
                if (!string.Equals(this._BtnProject, value))
                {
                    this._BtnProject = value;
                    RaisePropertyChanged(nameof(BtnProject));
                }
            }
        }

        private string _BtnTrash;
        public string BtnTrash
        {
            get { return this._BtnTrash; }
            set
            {
                if (!string.Equals(this._BtnTrash, value))
                {
                    this._BtnTrash = value;
                    RaisePropertyChanged(nameof(BtnTrash));
                }
            }
        }

        private string _BtnOpen;
        public string BtnOpen
        {
            get { return this._BtnOpen; }
            set
            {
                if (!string.Equals(this._BtnOpen, value))
                {
                    this._BtnOpen = value;
                    RaisePropertyChanged(nameof(BtnOpen));
                }
            }
        }

        private string _BtnAdd;
        public string BtnAdd
        {
            get { return this._BtnAdd; }
            set
            {
                if (!string.Equals(this._BtnAdd, value))
                {
                    this._BtnAdd = value;
                    RaisePropertyChanged(nameof(BtnAdd));
                }
            }
        }

        private string _BtnReload;
        public string BtnReload
        {
            get { return this._BtnReload; }
            set
            {
                if (!string.Equals(this._BtnReload, value))
                {
                    this._BtnReload = value;
                    RaisePropertyChanged(nameof(BtnReload));
                }
            }
        }

        private string _BtnUpload;
        public string BtnUpload
        {
            get { return this._BtnUpload; }
            set
            {
                if (!string.Equals(this._BtnUpload, value))
                {
                    this._BtnUpload = value;
                    RaisePropertyChanged(nameof(BtnUpload));
                }
            }
        }

        private string _BtnDownload;
        public string BtnDownload
        {
            get { return this._BtnDownload; }
            set
            {
                if (!string.Equals(this._BtnDownload, value))
                {
                    this._BtnDownload = value;
                    RaisePropertyChanged(nameof(BtnDownload));
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
                
        private string _startDate;
        public string startDate
        {
            get { return this._startDate; }
            set
            {
                if (!string.Equals(this._startDate, value))
                {
                    this._startDate = value;
                    RaisePropertyChanged(nameof(startDate));
                }
            }
        }
        private string _endDate;
        public string endDate
        {
            get { return this._endDate; }
            set
            {
                if (!string.Equals(this._endDate, value))
                {
                    this._endDate = value;
                    RaisePropertyChanged(nameof(endDate));
                }
            }
        }


        #endregion

        #region [Methods]

        public void GetFolder()
        {
            if (filesSelected != null && filesSelected.IsFile == false)
            {
                DgFiles =  Singleton.GetInstance().GetDBB().GetItemsFolder(filesSelected.path);
            }
        }

        /**
         * Récupère une ico en fonction du type de l'image
         * */
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

        #region {Action ICommand]
        /**
         * Lie un fichier à un projet 
         * Partage le fichier/dossier aux utilisateurs
         * */
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

        /**
         * Supprimer un dossier ou un fichier
         * */
        private void ActionSupprimer(object parameter)
        {
            if (filesSelected != null)
            {
                MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer  " + filesSelected.Nom + "?", "Infos", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string test = filesSelected.path;
                        Singleton.GetInstance().GetDBB().Delete(filesSelected.path);
                        //notification 
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

        /**
        * Lie un fichier à un projet 
        * Partage le fichier/dossier aux utilisateurs
        * */
        private void ActionCreateFolder(object parameter)
        {
            try
            {
                //AddFolderView _AddFolder = new AddFolderView();
                //AddFolderModelView context = new AddFolderModelView(_AddFolder);
                //_AddFolder.Width = 300;
                //_AddFolder.Height = 200;
                //_AddFolder.Show();

                Singleton.GetInstance().GetDBB().CreateFolder("/test__");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\"" + ex.Message);
            }
        }

        /**
         * Reload Grid
         * */
        private void ActionReloadDataGrid(object parameter)
        {
            try
            {
                DgFiles.Clear();
                Singleton.GetInstance().GetHomeModelView().GetFiles();
                Singleton.GetInstance().GetHomeModelView().GetFilesShared();
            } catch (Exception ex)
            {
                MessageBox.Show("Error:\"" + ex.Message);
            }
        }

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

        /**
       * Download File
       * */
        private void ActionDownload(object paramater)
        {
            if (filesSelected != null)
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
                    Singleton.GetInstance().GetDBB().Download("/", DropboxFileName, DownloadFolderPath, DownloadFileName);
                }
            }
            else
            {
                MessageBox.Show("Aucun fichier(s) sélectioné(s).");
            }
        }

        private void ActionOpen(object paramater)
        {
            if (filesSelected != null)
            {
                if (filesSelected.PreviewUrl == null)
                {
                    string DropboxFileName = filesSelected.Nom;
                    string DropboxFolderPath = filesSelected.path;
                    string fileName = System.IO.Path.GetTempPath() + DropboxFileName;
                    Singleton.GetInstance().GetDBB().Download("/", DropboxFileName, fileName, DropboxFileName);
                    System.Diagnostics.Process.Start(fileName);
                }
                else
                {
                    System.Diagnostics.Process.Start(filesSelected.PreviewUrl);
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
            string nomRechercher = Nom;
            Results = new List<Files>();
            bool trouve = false;

            foreach (Files item in DgFiles)
            {

                if (item.Nom.Contains(nomRechercher))
                {
                    trouve = true;
                    Results.Add(item);
                    Console.WriteLine(Results);
                    DgFiles = Results;
                }
            }
            if (trouve == false)
            {
                MessageBox.Show("Le fichier avec le nom indiqué n’existe pas");
            }
        }

        private void ActionRecherchePeriode(object obj)
        {
            string recherchePeriode = this.Date;
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(20);
            Results = new List<Files>();
            if (endDate < startDate)
            {
                throw new ArgumentException("endDate doit être supérieur ou égal à  startDate");
            }

            foreach (Files item in DgFiles)
            {
                if (item.DateDeCreation > endDate && item.DateDeCreation > startDate)
                {
                    startDate = startDate.AddDays(1);
                    Console.WriteLine(Results);
                    DgFiles = Results;
                }
            }
        }

        private void ActionRechercheDate(object obj)
        {
            string rechercheDate = this.Date;
            char[] delimiters = new char[] { '/', ' ' };
            string[] words = rechercheDate.Split(delimiters);
            int month = int.Parse(words[0]);
            int day = int.Parse(words[1]);
            int year = int.Parse(words[2]);

            Results = new List<Files>();
            bool trouve = false;

            foreach (Files item in DgFiles)
            {
                if (item.DateDeCreation.Value.Year == year && item.DateDeCreation.Value.Month == month && item.DateDeCreation.Value.Day == day)
                {
                    trouve = true;
                    Results.Add(item);
                    Console.WriteLine(Results);
                    DgFiles = Results;
                }

                if (item.DateInvitation.Value.Year == year && item.DateInvitation.Value.Month == month && item.DateInvitation.Value.Day == day)
                {
                    trouve = true;
                    Results.Add(item);
                    Console.WriteLine(Results);
                    DgFiles = Results;
                }
            }
            if (trouve == false)
            {
                MessageBox.Show("La date séléctioné  n’existe pas");
            }
        }

        #endregion
    }
}