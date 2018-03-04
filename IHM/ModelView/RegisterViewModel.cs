using IHM.Helpers;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.ModelView
{
    public class RegisterViewModel : ObservableObject, IPageViewModel
    {
        public string Name => "Register";

        public RegisterViewModel()
        {
            List<string> lst = new List<string>();
            lst.Add("Administration");
            lst.Add("Utilisateur GED");
            lst.Add("Gestionnaire de cloud");

            lstRole = lst;
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
    }
}
