using IHM.Helpers;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class AddUserModelView : ObservableObject, IPageViewModel
    {
        ObservableCollection<string> _selectedRole = new ObservableCollection<string>();

        public ICommand Save { get; set; }
        public AddUserModelView()
        {
            LoadAction();
            LstRole = LoadRole();
        }

        private List<string> LoadRole()
        {
            List<string> lst = new List<string>();
            lst.Add("Sélectionnez un rôle...");
            lst.Add("Administration");
            lst.Add("Utilisateur GED");
            lst.Add("Gestionnaire de cloud");
            return lst;
        }

        public void LoadAction()
        {
            Save = new RelayCommand(ActionAddUser);
        }

        private void ActionAddUser(object obj)
        {

           
        }



        #region [Binding]
        private string loginUser;
        public string LoginUser
        {
            get { return this.loginUser; }
            set
            {
                if (!string.Equals(this.loginUser, value))
                {
                    this.loginUser = value;
                    RaisePropertyChanged(nameof(LoginUser));
                }
            }
        }

        private string emailUser;
        public string EmailUser
        {
            get { return this.emailUser; }
            set
            {
                if (!string.Equals(this.emailUser, value))
                {
                    this.emailUser = value;
                    RaisePropertyChanged(nameof(EmailUser));
                }
            }
        }

        private string mdpUser;
        public string MdpUser
        {
            get { return this.mdpUser; }
            set
            {
                if (!string.Equals(this.mdpUser, value))
                {
                    this.mdpUser = value;
                    RaisePropertyChanged(nameof(MdpUser));
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
        public ObservableCollection<string> SelectedRole
        {
            get
            {
                return _selectedRole;
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        #region [Action]
        
    }
    #endregion

   
}

