using IHM.Helpers;
using IHM.Model;
using IHM.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class PopInModelView : ObservableObject
    {
        public Fichier file { get; set; }
        private List<Projet> lstPChecked = new List<Projet>();
        public ICommand QuitCommand { get; set; }
        public Action CloseHandler { get; set; }

        #region [Constructeur]
        public PopInModelView()
        {
            Singleton.GetInstance().SetPopUp(this);
            LoadProject();
            LoadAction();
        }

        private void onQuit(object obj)
        {
            if (CloseHandler != null)
                CloseHandler();
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

        /// <summary>
        /// Récupère tous les projets dans la popUp
        /// </summary>
        public void LoadProject()
        {
            List<Projet> l = new List<Projet>();
            var lst = Singleton.GetInstance().GetAllProject();

            if (file != null)
            {
                lst.ForEach(p =>
                {
                    p.Ischecked = false;

                    if (p.LstFiles != null)
                    {
                        Fichier isFIle = GetFichierByProjet(p);

                        if (isFIle != null)
                        {
                            p.Ischecked = true;
                        }
                    }
                    l.Add(p);
                });
                LstProjet = l;
            }
            else
            {
                lst.ForEach(p =>
                {
                    p.Ischecked = false;
                });
            }
        }

        /// <summary>
        /// Ajoute un fichier à un ou des projets
        ///Partage le fichier avec les utilisateurs liés au(x) projet(s)
        /// </summary>
        /// <param name="nomProjet"></param>
        public void setLstPChecked(string nomProjet)
        {
            var projet = Singleton.GetInstance().GetAllProject().FirstOrDefault(n => n.Nom.Equals(nomProjet));
            lstPChecked.Add(projet); //liste des projets cochés

                Fichier isFIle = GetFichierByProjet(projet);

                if (isFIle == null)
                {
                    DoShare(projet, file);
                    projet.LstFiles.Add(file);
                    UpdateProject();
                }
        }

        /// <summary>
        /// Récupère les fichiers par projet
        /// </summary>
        /// <param name="projet"></param>
        /// <returns></returns>
        private Fichier GetFichierByProjet(Projet projet)
        {
            Fichier isFIle = null;
            projet.LstFiles.ForEach(f =>
            {
                if (file.IdDropbox != null && f.IdDropbox != null)
                {
                    if (f.IdDropbox.Equals(file.IdDropbox))
                    {
                        isFIle = f;
                    }
                }
                if (file.IdGoogle != null && f.IdGoogle != null)
                {
                    if (f.IdGoogle.Equals(file.IdGoogle))
                    {
                        isFIle = f;
                    }
                }
            });
            return isFIle;
        }

        /// <summary>
        /// Mets a jour le projet dans le fichier JSON
        /// </summary>
        private void UpdateProject()
        {
            using (StreamWriter file = File.CreateText(@ConfigurationSettings.AppSettings["ProjetJSON"]))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Singleton.GetInstance().GetAllProject());
            }
        }

        /// <summary>
        /// Partage le fichier entre le chef de projet et les utilisateurs
        /// </summary>
        /// <param name="_projet"></param>
        /// <param name="_file"></param>
        private void DoShare(Projet _projet, Fichier _file)
        {
            if (_file.IdDropbox != null)
            {
                foreach (Utilisateur _utilisateur in _projet.LstUser)
                {
                    Singleton.GetInstance().GetDBB().SharingFile(_file, _utilisateur);
                }
            }
            else
            {
                //partage google
            }
        }

        public void LoadAction()
        {
            QuitCommand = new RelayCommand(onQuit);
        }
        
        #endregion
    }
}
