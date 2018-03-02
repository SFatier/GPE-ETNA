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
        public string Name => "Liste des documents du cloud dropbox";

        public ListModelView()
        {
            _DgFiles = new ObservableCollection<Files>();
            //_DgFiles.Add(new Files() { Id = 1, IMG= GetIcoByType(), Nom = "John Doe", Type = "Image JPEG", Taille = 2 ,  DateDeCreation = new DateTime(1971, 7, 23) , ModifieLe = DateTime.Now});
            //_DgFiles.Add(new Files() { Id = 2, IMG= GetIcoByType(), Nom = "Jane Doe", Type = "Doc", Taille = 2, DateDeCreation = new DateTime(1974, 1, 17), ModifieLe = DateTime.Now });
            //_DgFiles.Add(new Files() { Id = 3, IMG= GetIcoByType(), Nom = "Sammy Doe", Type = "Texte", Taille = 2, DateDeCreation = new DateTime(1991, 9, 2), ModifieLe = DateTime.Now });
            //DgFiles = _DgFiles;

        }

        #region [Binding]
        private ObservableCollection<Files>_DgFiles;
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
        #endregion

        /**
         * Récupère une ico en fonction du type de l'image
         * */
        public string GetIcoByType()
        {
            string str = string.Empty;
            return str;
        }
       
    }
}
