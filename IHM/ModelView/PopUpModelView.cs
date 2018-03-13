using IHM.Helpers;
using IHM.Model;
using IHM.View;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IHM.ModelView
{
    class PopUpModelView : ObservableObject, IPageViewModel
    {
        private PopUp app;
        private List<Projet> lstPChecked = new List<Projet>();
        public string Name => "PopUp";


        #region [Constructeur]
        public PopUpModelView(PopUp _app)
        {
            Singleton.GetInstance().SetPopUp(this);
            _lstProjet = new ObservableCollection<Projet>();
            app = _app;
            LoadProject();
        }

        #endregion

        #region [Binding]
        private ObservableCollection<Projet> _lstProjet;
        public ObservableCollection<Projet> LstProjet
        {
            get { return this._lstProjet; }
            set
            {
                if (!string.Equals(this._lstProjet, value))
                {
                    this._lstProjet = value;
                    RaisePropertyChanged(nameof(LstProjet));
                }
            }
        }
        #endregion

        #region [Methods]
        private void LoadProject()
        {
            ObservableCollection<Projet> _lst = new ObservableCollection<Projet>();
            _lst.Add(new Projet() { Nom = "toto", Ischecked = false });
            _lst.Add(new Projet() { Nom = "tata", Ischecked = false });
            _lst.Add(new Projet() { Nom = "titi", Ischecked = false });
            LstProjet = _lst;
        }

        public void setLstPChecked( string nomProjet)
        {
            var projet = LstProjet.FirstOrDefault(n => n.Nom.Equals(nomProjet));
            lstPChecked.Add(projet); //liste des projets cochés
        }
        #endregion
    }
}
