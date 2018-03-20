using IHM.Helpers;
using IHM.Model;
using IHM.View;
using IHM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;

namespace IHM.ModelView
{
    class PopUpModelView : ObservableObject, IPageViewModel
    {
        private PopUp app;
        private Files file;
        private List<Projet> lstPChecked = new List<Projet>();
        public string Name => "PopUp";
        
        #region [Constructeur]
        public PopUpModelView(PopUp _app, Files _file)
        {
            Singleton.GetInstance().SetPopUp(this);
            app = _app;
            file = _file;
            LoadProject();
        }

        #endregion

        #region [Binding]
        private List<Projet> _lstProjet;
        public List<Projet> LstProjet
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
            if (file != null) { 
                var lst = Singleton.GetInstance().GetAllProject();
                List<Projet> l = new List<Projet>();
                foreach (Projet p in lst)
                {
                    if (p.LstFiles.Contains(file))
                    {
                        p.Ischecked = true;
                    }
                    l.Add(p);
                }
                LstProjet = l;
            }
            else
            {
                LstProjet = Singleton.GetInstance().GetAllProject();
            }
        }

        public void setLstPChecked( string nomProjet)
        {
            var projet = LstProjet.FirstOrDefault(n => n.Nom.Equals(nomProjet));
            lstPChecked.Add(projet); //liste des projets cochés
            
            projet.LstFiles.Add(file);

            UpdateProject();
        }

        private void UpdateProject()
        {
            using (StreamWriter file = File.CreateText(@ConfigurationSettings.AppSettings["ProjetJSON"]))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Singleton.GetInstance().GetAllProject());
            }

        }

        public void LoadAction()
        {
            //
        }
        #endregion
    }
}
