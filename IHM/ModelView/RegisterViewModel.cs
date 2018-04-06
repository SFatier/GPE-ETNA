using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class RegisterViewModel : ObservableObject, IPageViewModel
    {
        public string Name => "Register";
        public ICommand Inscription { get; set; }

        public RegisterViewModel()
        {
            LoadAction();
            LoadUtilisateur();            
            LstRole = LoadRole();
            Role = "Sélectionnez un rôle...";
        }

        #region [Binding]

        private string _Login;
        public string Login
        {
            get { return this._Login; }
            set
            {
                if (!string.Equals(this._Login, value))
                {
                    this._Login = value;
                    RaisePropertyChanged(nameof(Login));
                }
            }
        }

        private string _Mdp;
        public string Mdp
        {
            get { return this._Mdp; }
            set
            {
                if (!string.Equals(this._Mdp, value))
                {
                    this._Mdp = value;
                    RaisePropertyChanged(nameof(Mdp));
                }
            }
        }

        private string _Email;
        public string Email
        {
            get { return this._Email; }
            set
            {
                if (!string.Equals(this._Email, value))
                {
                    this._Email = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }

        private string _Role;
        public string Role
        {
            get { return this._Role; }
            set
            {
                if (!string.Equals(this._Role, value))
                {
                    this._Role = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }

        private List<string> _lstRole;
        public List<string> LstRole
        {
            get { return this._lstRole; }
            set
            {
                if (!string.Equals(this._lstRole, value))
                {
                    this._lstRole = value;
                    RaisePropertyChanged(nameof(LstRole));
                }
            }
        }
        
        #endregion

        public void ActionInscription(object parameter)
        {
            //Enregistrement de l'utilisateur 
            if (Login != "" && Email != "" && Role != "")
            {
                if ( Singleton.GetInstance().GetAllUtilisateur().Select(user => user.Login.Equals(Email)).Count() > 0){

                    Utilisateur Nouvelle_Utilisateur = new Utilisateur();
                    Nouvelle_Utilisateur.Login = Login;
                    Nouvelle_Utilisateur.MDP = Mdp;
                    Nouvelle_Utilisateur.Email = Email;
                    Nouvelle_Utilisateur.Role = Role;
                    Singleton.GetInstance().addUtilisateur(Nouvelle_Utilisateur);

                    #region [Ecriture de l'utilisateur dans le fichier .JSON]
                    try
                    {
                        string test = ConfigurationSettings.AppSettings["UtilisateurJSON"];
                        using (StreamWriter file = File.CreateText(@test))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(file, Singleton.GetInstance().GetAllUtilisateur());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error :\" " + ex.Message);
                    }
                    #endregion

                    if (Singleton.GetInstance().GetUtilisateur() == null) // Inscription
                    {

                        Singleton.GetInstance().SetUtilisateur(Nouvelle_Utilisateur);

                        HomeModelView HMV = new HomeModelView(Nouvelle_Utilisateur);
                        HMV.IsConnect = "Se deconnecter";
                        Singleton.GetInstance().GetMainWindowViewModel().CurrentPageViewModel = HMV;
                    }
                    else // ajout d'un utilisateur
                    {
                        MessageBox.Show("L'utilisateur a été ajouté.");
                        ListUsersModelView lstUMV = new ListUsersModelView();
                        lstUMV.UsersList = Singleton.GetInstance().GetAllUtilisateur();
                        Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = lstUMV;
                    }
                }
                else
                {
                    MessageBox.Show("Il existe un utilisateur avec cette email");
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
            }
        }

        public void LoadAction()
        {
            Inscription = new RelayCommand(ActionInscription);
        }

        private void LoadUtilisateur()
        {
            List<Utilisateur> items;
            try
            {
                StreamReader r;
                using (r = new StreamReader(@ConfigurationSettings.AppSettings["UtilisateurJSON"]))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Utilisateur>>(json);
                }
            }
            catch (Exception)
            {
                items = new List<Utilisateur>();
            }
            Singleton.GetInstance().SetListUtilisateur(items);
        }

        private List<string> LoadRole()
        {
            List<string> lst = new List<string>();
            lst.Add("Sélectionnez un rôle...");
            lst.Add("Administrateur");
            lst.Add("Utilisateur GED");
            lst.Add("Gestionnaire de cloud");
            return lst;
        }
    }
}
