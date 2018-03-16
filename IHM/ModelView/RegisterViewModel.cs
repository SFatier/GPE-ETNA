using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            List<Utilisateur> items;
            try
            {
                StreamReader r;
                using (r = new StreamReader(@"C:\Users\sigt_sf\Documents\GitHub\GPE-ETNA\IHM\utilisateur.json"))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Utilisateur>>(json);
                }
            }
            catch (Exception ex)
            {
                items = new List<Utilisateur>();
            }
            Singleton.GetInstance().SetListUtilisateur(items);

            List<string> lst = new List<string>();
            lst.Add("Administration");
            lst.Add("Utilisateur GED");
            lst.Add("Gestionnaire de cloud");

            lstRole = lst;
            Inscription = new RelayCommand(ActionInscription);
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
        public List<string> lstRole
        {
            get { return this._lstRole; }
            set
            {
                if (!string.Equals(this._lstRole, value))
                {
                    this._lstRole = value;
                    RaisePropertyChanged(nameof(lstRole));
                }
            }
        }
        
        #endregion

        public void ActionInscription(object parameter)
        {
            //Enregistrement de l'utilisateur 
            Utilisateur Nouvelle_Utilisateur = new Utilisateur();
            Nouvelle_Utilisateur.Login = Login;
            Nouvelle_Utilisateur.MDP = Mdp;
            Nouvelle_Utilisateur.Email = Email;
            Nouvelle_Utilisateur.Role = Role;
            Singleton.GetInstance().addUtilisateur(Nouvelle_Utilisateur);
            Singleton.GetInstance().SetUtilisateur(Nouvelle_Utilisateur);
                        
             #region [Ecriture de l'utilisateur dans le fichier .JSON]
            try
            {
                using (StreamWriter file = File.CreateText(@"C:\Users\sigt_sf\Documents\GitHub\GPE-ETNA\IHM\utilisateur.json"))
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

            HomeModelView HMV = new HomeModelView(Nouvelle_Utilisateur);
            HMV.IsConnect = "Se deconnecter";
            Singleton.GetInstance().GetMainWindowViewModel().CurrentPageViewModel = HMV;
        }
    }
}
