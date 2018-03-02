using IHM.Helpers;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
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

        private string _Mdp ;
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
            ForgetPsswd = new RelayCommand(ActionForgetPsswd);
            LogIn = new RelayCommand(ActionLogiIn);
            Register = new RelayCommand(ActionResgister);
        }
        #endregion

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
            Singleton.GetInstance().GetMainWindowViewModel().CurrentPageViewModel = new ListModelView();
        }

        /**
         * Renvoie à une page s'inscrire
         * */
        private void ActionResgister(object p)
        {
            Singleton.GetInstance().GetMainWindowViewModel().CurrentPageViewModel= new RegisterViewModel();
        }
        #endregion
    }
}
