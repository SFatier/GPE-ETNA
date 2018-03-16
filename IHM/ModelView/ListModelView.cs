using GPE;
using IHM.Helpers;
using IHM.Model;
using IHM.View;
using IHM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static string path_img = "C:\\Users\\sigt_sf\\Documents\\GitHub\\GPE-ETNA\\IHM\\IMG\\"; //a modifier par rapport à votre ordinateur
        public string Name => "Liste des documents du cloud dropbox";

        public ICommand  LinkProject { get; set; }
        public ICommand Supprimer { get; set; }
        public ICommand CreateFolder { get; set; }
        public ICommand recherche { get; set; } //nom de ton binding

        //constructeur
        public ListModelView()
        {
            _DgFiles = new ObservableCollection<Files>();

            LoadProject();
            LoadIcon();
            LoadAction();
        }

        private void LoadAction()
        {
            LinkProject = new RelayCommand(ActionLinkProject);
            Supprimer = new RelayCommand(ActionSupprimer);
            CreateFolder = new RelayCommand(ActionCreateFolder);
        }

        private void LoadIcon()
        {
            btnEdit = path_img + "edit.png";
            btnTrash = path_img + "trash.png";
            btnOpen = path_img + "open.png";
            btnAdd = path_img + "add.png";
            btnReload = path_img + "reload.png";
            btnUpload = path_img + "upload.png";
            btnDownload = path_img + "download.png";
            btnProject = path_img + "link.png";
        }

        private void LoadProject()
        {
            List<Projet> items;
            try
            {
                StreamReader r;
                using (r = new StreamReader(@"C:\Users\sigt_sf\Documents\GitHub\GPE-ETNA\IHM\projets.json"))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Projet>>(json);
                }
            }catch(Exception ex)
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

        private Files _lstFiles;
        public Files  lstFiles
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

        private string _btnEdit;
        public string btnEdit
        {
            get { return this._btnEdit; }
            set
            {
                if (!string.Equals(this._btnEdit, value))
                {
                    this._btnEdit = value;
                    RaisePropertyChanged(nameof(btnEdit));
                }
            }
        }

        private string _btnProject;
        public string btnProject
        {
            get { return this._btnProject; }
            set
            {
                if (!string.Equals(this._btnProject, value))
                {
                    this._btnProject = value;
                    RaisePropertyChanged(nameof(btnProject));
                }
            }
        }        

        private string _btnTrash;
        public string btnTrash
        {
            get { return this._btnTrash; }
            set
            {
                if (!string.Equals(this._btnTrash, value))
                {
                    this._btnTrash = value;
                    RaisePropertyChanged(nameof(btnTrash));
                }
            }
        }

        private string _btnOpen;
        public string btnOpen
        {
            get { return this._btnOpen; }
            set
            {
                if (!string.Equals(this._btnOpen, value))
                {
                    this._btnOpen = value;
                    RaisePropertyChanged(nameof(btnOpen));
                }
            }
        }

        private string _btnAdd;
        public string btnAdd
        {
            get { return this._btnAdd; }
            set
            {
                if (!string.Equals(this._btnAdd, value))
                {
                    this._btnAdd = value;
                    RaisePropertyChanged(nameof(btnAdd));
                }
            }
        }

        private string _btnReload;
        public string btnReload
        {
            get { return this._btnReload; }
            set
            {
                if (!string.Equals(this._btnReload, value))
                {
                    this._btnReload = value;
                    RaisePropertyChanged(nameof(btnReload));
                }
            }
        }

        private string _btnUpload;
        public string btnUpload
        {
            get { return this._btnUpload; }
            set
            {
                if (!string.Equals(this._btnUpload, value))
                {
                    this._btnUpload = value;
                    RaisePropertyChanged(nameof(btnUpload));
                }
            }
        }

        private string _btnDownload;
        public string btnDownload
        {
            get { return this._btnDownload; }
            set
            {
                if (!string.Equals(this._btnDownload, value))
                {
                    this._btnDownload = value;
                    RaisePropertyChanged(nameof(btnDownload));
                }
            }
        }

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
            string Nouveau_dossier  = "/test";
            Singleton.GetInstance().GetDBB().CreateFolder(Nouveau_dossier);
        }
        
        #endregion
    }
}