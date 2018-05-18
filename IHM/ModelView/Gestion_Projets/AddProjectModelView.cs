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
    public class AddProjectModelView : ObservableObject, IPageViewModel
    {
        ObservableCollection<string> _selectedUsers = new ObservableCollection<string>();
        public ICommand  Save{ get; set; }
        
        public AddProjectModelView()
        {
            TitrePage = "Ajouter un projet";
            LoadAction();
            LstUser = Singleton.GetInstance().GetAllUtilisateur().Where(user => user.Email != Singleton.GetInstance().GetUtilisateur().Email).Select(u => u.Login).ToList() ;
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

        public ObservableCollection<string> SelectedUsers
        {
            get
            {
                return _selectedUsers;
            }
        }
        #endregion

        #region [Action]

        private void ActionAddProject(object parameter)
        {
            if (NomProjet != "" && DescriptionProjet != "")
            {
                Projet p = new Projet();
                p.Nom = NomProjet;
                p.Description = DescriptionProjet;
                p.LstFiles = new List<Files>();
                p.LstUser = new List<Utilisateur>();
                p.LstUser = GetUserProject();
                p.IcoIsArchived = "notvalidate.png";
                p.IsprojetFin = false;
                p.IsprojetEncours = true;
                p.DateDeCreation = DateTime.Now;
                Singleton.GetInstance().addProject(p);
            
                #region [Ecriture de l'utilisateur dans le fichier .JSON]
                try
                {
                    using (StreamWriter file = File.CreateText(@ConfigurationSettings.AppSettings["ProjetJSON"]))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, Singleton.GetInstance().GetAllProject());
                    }

                    Singleton.GetInstance().GetHomeModelView().GetProjets();
                    Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new AdminModelView();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error :\" " + ex.Message);
                }
                #endregion
            }
            else
            {
                MessageBox.Show("Veuillez renseigner tous les champs obligatoires");
            }
        }

        #endregion

        public void LoadAction()
        {
            Save = new RelayCommand(ActionAddProject);
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
