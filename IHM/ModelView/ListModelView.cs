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
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Globalization;

namespace IHM.ModelView
{
    public class ListModelView : ObservableObject, IPageViewModel
    {
        private static string path_img = ConfigurationSettings.AppSettings["FolderIMG"];  //a modifier par rapport à votre ordinateur
        public string Name => "Liste des documents du cloud dropbox";

        public ICommand LinkProject { get; set; }
        public ICommand Supprimer { get; set; }
        public ICommand CreateFolder { get; set; }
        public ICommand ReloadDataGrid { get; set; }
        public ICommand Upload { get; set; }
        public ICommand Recherche { get; set; } //nom de ton binding
        public ICommand RechercheDate { get; set; }
        public string Mot => "Recherche";
   

        //constructeur
        public ListModelView()
        {
            _DgFiles = new ObservableCollection<Files>();
            
            //dgFiles = new ObservableCollection<Files>();
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

        private void LoadProject()
        {
            List<Projet> items;
            try
            {
                StreamReader r;
                using (r = new StreamReader(@ConfigurationSettings.AppSettings["ProjetJSON"]))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Projet>>(json);
                }
            } catch (Exception)
            {
                items = new List<Projet>();
            }
            Singleton.GetInstance().SetListProject(items);
        }

        #region [Binding]
        private ObservableCollection<Files> _DgFiles;
        public ObservableCollection<Files> DgFiles
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
        private ObservableCollection<Files> _Results;
        public ObservableCollection<Files> Results
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

        private Files _lstFiles;
        public Files lstFiles
        {
            get { return this._lstFiles; }
            set
            {
                if (!string.Equals(this._lstFiles, value))
                {
                    this._lstFiles = value;
                    RaisePropertyChanged(nameof(lstFiles));
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
        private string Title;
        private DateTime value;
        private object datePickDebut;

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

        public object myDatePicker { get; private set; }

        #endregion

        #region [Methods]
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
            if (lstFiles != null)
            {
                PopUp app = new PopUp();
                PopUpModelView context = new PopUpModelView(app, lstFiles);
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
            if (lstFiles != null)
            {
                MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer le film " + lstFiles.Nom + "?", "Infos", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string test = lstFiles.path;
                        Singleton.GetInstance().GetDBB().Delete(lstFiles.path);
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
                Singleton.GetInstance().GetHomeModelView().GetFiles();
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
        // search Files
        private void ActionRecherche(object par)
        {
            string nomRechercher = Nom;

          Results =   new ObservableCollection<Files>();
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
        private void ActionRechercheDate(object obj)
        {
            string rechercheDate = this.Date;
            char[] delimiters = new char[] { '/', ' ' };
            string[] words = rechercheDate.Split(delimiters);
            int month = int.Parse( words[0]);
            int day = int.Parse(words[1]);
            int year = int.Parse(words[2]);

            Results = new ObservableCollection<Files>();
            bool trouve = false;

            foreach (Files item in DgFiles)
            {

                if (item.DateDeCreation.Year==year && item.DateDeCreation.Month==month && item.DateDeCreation.Day == day)
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
        

}

}
        #endregion
    
