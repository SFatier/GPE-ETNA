﻿using IHM.Helpers;
using IHM.Model;
using IHM.ModelView.Gestion_Utilisateurs;
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
    public class ListUsersModelView : ObservableObject, IPageViewModel
    {
        public string Name => "Liste utilisateurs";
        public ICommand SearchUserBar { get; set; }
        public ICommand FicheUtilisateur { get; set; }
        public ICommand DeleteUtilisateur { get; set; }

        public ListUsersModelView()
        {
            UsersList = Singleton.GetInstance().GetAllUtilisateur();
            BtnSearch = ConfigurationSettings.AppSettings["FolderIMG"] +"search.png";
            LoadAction();
        }

        #region [Binding]
        private string _BtnSearch;
        public string BtnSearch
        {
            get { return this._BtnSearch; }
            set
            {
                if (!string.Equals(this._BtnSearch, value))
                {
                    this._BtnSearch = value;
                    RaisePropertyChanged(nameof(BtnSearch));
                }
            }
        }

        private List<Utilisateur> _UsersList;
        public List<Utilisateur> UsersList
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

        private List<Utilisateur> _SearchUser;
        public List<Utilisateur> SearchUser
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

        private Utilisateur _UserSelected;
        public Utilisateur UserSelected
        {
            get { return this._UserSelected; }
            set
            {
                if (!string.Equals(this._UserSelected, value))
                {
                    this._UserSelected = value;
                    RaisePropertyChanged(nameof(UserSelected));
                }
            }
        }
        #endregion

        public void LoadAction()
        {
            SearchUserBar = new RelayCommand(ActionSearchUserBar);
            FicheUtilisateur = new RelayCommand(ActionFiche);
            DeleteUtilisateur = new RelayCommand(ActionDeleteUser);
        }

        private void ActionFiche(object obj)
        {
            if (UserSelected != null)
            {
                Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new UtilisateurViewModel(UserSelected);
            }
        }
        
        private void ActionSearchUserBar(object obj)
        {
            if (SearchUser != null)
            {

            }
        }

        private void ActionDeleteUser(object obj)
        {
            if (SearchUser != null)
            {

            }
        }
    }
}