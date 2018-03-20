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
    public class LoginModelView : ObservableObject, IPageViewModel
    {
        public string Name => "Se connecter";

        #region [Command]
        public ICommand ForgetPsswd { get; set; }
        public ICommand LogIn { get; set; }
        public ICommand Register { get; set; }
        #endregion

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
        #endregion
        
        #region [Constructor]
        public LoginModelView()
        {
            LoadAction();

            List<Utilisateur> items;
            try
            {
                StreamReader r;
                using (r = new StreamReader(@ConfigurationSettings.AppSettings["UtilisateurJSON"]))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Utilisateur>>(json);
                }
            }catch(Exception ex)
            {
                items = new List<Utilisateur>();
            }

            Singleton.GetInstance().SetListUtilisateur(items);

            Login = "fatier_s";
            Mdp = "pass";
        }
        #endregion

        public void LoadAction()
        {
            ForgetPsswd = new RelayCommand(ActionForgetPsswd);
            LogIn = new RelayCommand(ActionLogiIn);
            Register = new RelayCommand(ActionResgister);
        }

        #region [Action]

        /**
         * Renvoie le mot de passe à l'utilisateur
         * */
        private void ActionForgetPsswd(object p)
        {
            //Envoie d'une email
            MessageBox.Show("Non Implémenté");
        }

        /**
         * Se connecte à l'appplication
         * */
        private void ActionLogiIn(object p)
        {
            List<Utilisateur> lst = Singleton.GetInstance().GetAllUtilisateur();
            Utilisateur u = (Utilisateur) lst.FirstOrDefault(x => x.Login.Equals(Login) && x.MDP.Equals(Mdp));
            if (u != null)
            {
                HomeModelView HMV = new HomeModelView(u);
                HMV.IsConnect = "Se deconnecter";
                Singleton.GetInstance().GetMainWindowViewModel().CurrentPageViewModel = HMV;
            }
            else
            {
                MessageBox.Show("Aucun utilisateur trouvé.");
            }
        }

        /**
         * Renvoie à une page s'inscrire
         * */
        private void ActionResgister(object p)
        {
            Singleton.GetInstance().GetMainWindowViewModel().CurrentPageViewModel = new RegisterViewModel();
        }
        #endregion
        
    }
}
