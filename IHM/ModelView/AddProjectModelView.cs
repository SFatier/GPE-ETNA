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
        public string Name => throw new NotImplementedException();
        public ICommand  Save{ get; set; }
        
        public AddProjectModelView()
        {
            LoadAction();
            lstUser = Singleton.GetInstance().GetAllUtilisateur().Where(user => user.Role != "Admin").Select(u => u.Login).ToList() ;
        }

        #region [Binding]
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
        public List<string> lstUser
        {
            get { return _lstUser; }
            set
            {
                if (!string.Equals(this._lstUser, value))
                {
                    this._lstUser = value;
                    RaisePropertyChanged(nameof(lstUser));
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
                Singleton.GetInstance().addProject(p);
            
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
