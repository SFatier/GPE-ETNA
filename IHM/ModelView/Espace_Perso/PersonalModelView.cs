using GPE;
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
    public class PersonalModelView : ObservableObject, IPageViewModel
    {
        public string Name => "Espace personnel";
        public ICommand ConnecterDP { get; set; }
        public ICommand MiseAJourUser { get; set; }

        private Utilisateur u;
        private DropBox DBB;
        private string strAccessToken = string.Empty;
        private string strAuthenticationURL = string.Empty;

        public PersonalModelView()
        {
            u = Singleton.GetInstance().GetUtilisateur();
            Login = u.Login;
            Mdp = u.MDP;
            Email = u.Email;
            Role = u.Role;
            if (u.Token != null)
            {
                strDP = "Dropbox connecté";
            }
            LoadAction();
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

        private string _strDP = " Connecter Droppbox";
        public string strDP
        {
            get { return this._strDP; }
            set
            {
                if (!string.Equals(this._strDP, value))
                {
                    this._strDP = value;
                    RaisePropertyChanged(nameof(strDP));
                }
            }
        }
        #endregion

        public void LoadAction()
        {
            MiseAJourUser = new RelayCommand(ActionMiseAJourUser);
            ConnecterDP = new RelayCommand(ActionConnecterDropbox);
        }

        #region Dropbox        
        /**
         * Ouvre une nouvelle fenêtre qui demande l'autorasition de se connecter à dropbox
         * */
        private void ActionConnecterDropbox(object parameter)
        {
            DBB = Singleton.GetInstance().GetDBB();
            if (u.Token == null)
            {
                try
                {
                    if (string.IsNullOrEmpty(ConfigurationSettings.AppSettings["strAppKey"]))
                    {
                        MessageBox.Show("Rentrer l'API Clé inclus dans le APP.Config");
                        return;
                    }
                    if (DBB != null)
                    {
                        strAuthenticationURL = DBB.GeneratedAuthenticationURL();
                        strAccessToken = DBB.GenerateAccessToken();
                        var uUpdate = Singleton.GetInstance().GetAllUtilisateur().FirstOrDefault(user => u.Equals(user));
                        if (uUpdate != null)
                        {
                            uUpdate.Token = strAccessToken;
                        }
                        UpdateUtilisateur();
                        Singleton.GetInstance().GetHomeModelView().GetFiles();
                        strDP = "Dropbox connecté";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Impossible d'autoriser l'application à se connecter à l'application");
                }
            }
        }

        /**
         * Met à jour l'utilisateur
         * */
        private void UpdateUtilisateur()
        {
            StreamWriter file;
            using (file = File.CreateText(@ConfigurationSettings.AppSettings["UtilisateurJSON"]))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Singleton.GetInstance().GetAllUtilisateur());
            }
        }
  
        #endregion

        private void ActionMiseAJourUser(object obj)
        {
            List<Utilisateur> lst = Singleton.GetInstance().GetAllUtilisateur();
            Utilisateur _u = lst.FirstOrDefault(item => item.Login.Equals(Login));
            if (_u != null && _u.Email != Email) {
                lst.FirstOrDefault(item => item.Login.Equals(Login)).Email = Email;

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

                MessageBox.Show("L'utilisateur a été mise à jour");
            }
        }
    }
}
