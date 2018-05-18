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
using System.Windows;

namespace IHM.ModelView
{
    class PopUpModelView : ObservableObject, IPageViewModel
    {
        private PopUp app;
        private Fichier file;
        private List<Projet> lstPChecked = new List<Projet>();
        public string Name => "PopUp";
        
        #region [Constructeur]
        public PopUpModelView(PopUp _app, Fichier _file)
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

        /**
         * Récupère tous les projets dans la popUp
         * */
        private void LoadProject()
        {
            if (file != null) { 
                var lst = Singleton.GetInstance().GetAllProject();
                List<Projet> l = new List<Projet>();
                foreach (Projet p in lst)
                {
                    p.Ischecked = false;

                    if (p.LstFiles != null)
                    {
                        Fichier isFIle = p.LstFiles.FirstOrDefault(f => f.IdDropbox.Equals(file.IdDropbox)); //utilisé car la date du fichier se modifie lors du partage 
                        if (isFIle != null)
                        {
                            p.Ischecked = true;
                        }
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

        /**
         * Ajoute un fichier à un ou des projets
         * Partage le fichier avec les utilisateurs liés au(x) projet(s)
         * */
        public void setLstPChecked( string nomProjet)
        {
    
            var projet = Singleton.GetInstance().GetAllProject().FirstOrDefault(n => n.Nom.Equals(nomProjet));
            lstPChecked.Add(projet); //liste des projets cochés

            if (app.IsActive)
            {
                Fichier isFIle = projet.LstFiles.FirstOrDefault(f => f.IdDropbox.Equals(file.IdDropbox)); //utilisé car la date du fichier se modifie lors du partage 
                if (isFIle == null)
                { 
                    DoShare(projet, file);
                    projet.LstFiles.Add(file);
                    UpdateProject();
                }
                else
                {
                    MessageBox.Show("Le projet possède déjà ce fichier.");
                }
            }
        }

        /**
         * Mets a jour le projet dans le fichier JSON
         * */
        private void UpdateProject()
        {
            using (StreamWriter file = File.CreateText(@ConfigurationSettings.AppSettings["ProjetJSON"]))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Singleton.GetInstance().GetAllProject());
            }
        }

        /**
        * Partage le fichier entre le chef de projet et les utilisateurs
        * */
        private void DoShare(Projet _projet, Fichier _file)
        {
            foreach (Utilisateur _utilisateur in _projet.LstUser)
            {
                Singleton.GetInstance().GetDBB().SharingFile(_file, _utilisateur);
            }
        }

        public void LoadAction()
        {
            //
        }
        
        #endregion
    }
}
