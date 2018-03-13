using GPE;
using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class ListModelView : ObservableObject, IPageViewModel
    {
        public static string path_img = "C:\\Users\\sigt_sf\\Documents\\GitHub\\GPE-ETNA\\IHM\\IMG\\"; //a modifier par rapport à votre ordinateur
        public string Name => "Liste des documents du cloud dropbox";

        public ListModelView()
        {
            _DgFiles = new ObservableCollection<Files>();
            //_DgFiles.Add(new Files() { Id = 1, IMG= GetIcoByType(), Nom = "John Doe", Type = "Image JPEG", Taille = 2 ,  DateDeCreation = new DateTime(1971, 7, 23) , ModifieLe = DateTime.Now});
            //_DgFiles.Add(new Files() { Id = 2, IMG= GetIcoByType(), Nom = "Jane Doe", Type = "Doc", Taille = 2, DateDeCreation = new DateTime(1974, 1, 17), ModifieLe = DateTime.Now });
            //_DgFiles.Add(new Files() { Id = 3, IMG= GetIcoByType(), Nom = "Sammy Doe", Type = "Texte", Taille = 2, DateDeCreation = new DateTime(1991, 9, 2), ModifieLe = DateTime.Now });
            //DgFiles = _DgFiles;

            btnEdit = path_img + "edit.png";
            btnTrash = path_img + "trash.png";
            btnOpen = path_img + "open.png";
            btnAdd = path_img + "add.png";
            btnReload = path_img + "reload.png";
            btnUpload = path_img + "upload.png";
            btnDownload = path_img + "download.png";
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
    }
}