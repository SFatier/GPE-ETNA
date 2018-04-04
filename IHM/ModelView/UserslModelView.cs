using IHM.Helpers;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IHM.ModelView
{
    public  class UserslModelView : ObservableObject, IPageViewModel
    {
        public string Name => "Liste utilisateurs";
        private static string path_img = ConfigurationSettings.AppSettings["FolderIMG"];
        public ICommand SearchUserBar { get; set; }

        public UserslModelView()
        {
            UsersList = Singleton.GetInstance().GetAllUtilisateur();
        }

        #region [Binding]
        private List<Model.Utilisateur> _UsersList;
        public List<Model.Utilisateur> UsersList
        {
            get { return this._UsersList; }
            set
            {
                if (!string.Equals(this._UsersList, value))
                {
                    this._UsersList = value;
                    RaisePropertyChanged(nameof(UsersList));
                }
            }
        }

        private List<Model.Utilisateur> _SearchUser;
        public List<Model.Utilisateur> SearchUser
        {
            get { return this._SearchUser; }
            set
            {
                if (!string.Equals(this._SearchUser, value))
                {
                    this._SearchUser = value;
                    RaisePropertyChanged(nameof(SearchUser));
                }
            }
        }

        private string _SourceImage = path_img + "user.png";
        public string SourceImage
        {
            get { return this._SourceImage; }
            set
            {
                if (!string.Equals(this._SourceImage, value))
                {
                    this._SourceImage = value;
                    RaisePropertyChanged(nameof(SourceImage));
                }
            }
        }
        #endregion

        public void LoadAction()
        {
            SearchUserBar = new RelayCommand(ActionSearchUserBar);
        }

        private void ActionSearchUserBar(object obj)
        {
            //
        }
    }
}
