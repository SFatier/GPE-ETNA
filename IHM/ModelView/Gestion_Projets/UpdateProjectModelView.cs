using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
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
    public class UpdateProjectModelView : ObservableObject, IPageViewModel
    {
        private  Projet Projet { get; set; }
        public ICommand Save { get; set; }

        public UpdateProjectModelView(Projet project)
        {
            try
            {
                Projet = Singleton.GetInstance().GetAllProject().FirstOrDefault(p => p.NomProject.Equals(project.NomProject));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :\" " + ex.Message);
            }

            TitrePage = "Modifier un projet";
            LoadInformation();
            LoadAction();
        }

        #region [Binding]        
        private string titrePage;
        public string TitrePage
        {
            get { return this.titrePage; }
            set
            {
                if (!string.Equals(this.titrePage, value))
                {
                    this.titrePage = value;
                    RaisePropertyChanged(nameof(TitrePage));
                }
            }
        }

        private string nomProjet;
        public string NomProjet
        {
            get { return this.nomProjet; }
            set
            {
                if (!string.Equals(this.nomProjet, value))
                {
                    this.nomProjet = value;
                    RaisePropertyChanged(nameof(NomProjet));
                }
            }
        }

        private string descriptionProjet;
        public string DescriptionProjet
        {
            get { return this.descriptionProjet; }
            set
            {
                if (!string.Equals(this.descriptionProjet, value))
                {
                    this.descriptionProjet = value;
                    RaisePropertyChanged(nameof(DescriptionProjet));
                }
            }
        }

        private List<string> _lstUser;
        public List<string> LstUser
        {
            get { return _lstUser; }
            set
            {
                if (!string.Equals(this._lstUser, value))
                {
                    this._lstUser = value;
                    RaisePropertyChanged(nameof(LstUser));
                }
            }
        }

        private ObservableCollection<string> _selectedUsers;
        public ObservableCollection<string> SelectedUsers
        {
            get
            {
                return _selectedUsers;
            }
        }
        #endregion

        private void LoadInformation()
        {
            NomProjet = Projet.NomProject;
            DescriptionProjet = Projet.Description;
            List<Utilisateur> lstUtilisateur = Singleton.GetInstance().GetAllUtilisateur();
            LstUser = new List<string>();

            foreach (Utilisateur u in lstUtilisateur)
            {
                Utilisateur utilisateur = Projet.LstUser.FirstOrDefault(user => user.Login.Equals(u.Login));
                if( utilisateur != null)
                {
                    LstUser.Add(utilisateur.Login);
                }
            }
        }

        public void LoadAction()
        {
            Save = new RelayCommand(ActionUpdateProject);
        }

        private void ActionUpdateProject(object obj)
        {
            List<Projet> lst = Singleton.GetInstance().GetAllProject();
            Projet = lst.FirstOrDefault(item => item.NomProject.Equals(Projet.NomProject));

            Projet.NomProject = nomProjet;
            Projet.Description = DescriptionProjet;
    
            #region [Ecriture de l'utilisateur dans le fichier .JSON]
            try
            {
                using (StreamWriter file = File.CreateText(@ConfigurationSettings.AppSettings["ProjetJSON"]))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, Singleton.GetInstance().GetAllProject());
                }

                Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new AdminModelView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :\" " + ex.Message);
            }
            #endregion
        }

        private List<Utilisateur> GetUserProject()
        {
            List<Utilisateur> lst = new List<Utilisateur>();
            if (SelectedUsers != null)
            {
                List<Utilisateur> lstUtilisateur = Singleton.GetInstance().GetAllUtilisateur();

                foreach (var item in SelectedUsers)
                {
                    Utilisateur u = lstUtilisateur.FirstOrDefault(user => user.Login == item);
                    if (u != null)
                    {
                        lst.Add(u);
                    }
                }
            }
            return lst;
        }
    }
}
