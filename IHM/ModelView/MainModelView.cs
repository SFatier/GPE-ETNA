using GPE;
using IHM.Helpers;
using IHM.Model;
using IHM.ModelView;
using IHM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace IHM.ViewModel
{
    public class MainModelView : ObservableObject
    {
        // DAO.User u = DAO.ReferentielManager.DBConnect.Instance.GetUserByID(1);
        #region Fields
       
        public ICommand SeConnecter { get; set; }
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private ListModelView lMVM = new ListModelView();

        #endregion
        public MainModelView()
        {
            PageViewModels.Add(lMVM);
            PageViewModels.Add(new LoginModelView());
            CurrentPageViewModel = PageViewModels[0];

            //SeConnecter = new RelayCommand(ActionSeConnecterFilm);
            ConnecterDP = new RelayCommand(ActionConnecterDropbox);

            Singleton.GetInstance().SetMainWindowViewModel(this);
        }

        //#region [Binding ]
        //private string _isConnecter = Connection.GetInstance().GetStrConnection();
        //public string isConnecter
        //{
        //    get { return this._isConnecter; }
        //    set
        //    {
        //        if (!string.Equals(this._isConnecter, value))
        //        {
        //            this._isConnecter = value;
        //            RaisePropertyChanged(nameof(isConnecter));
        //        }
        //    }
        //}

        //private bool _vConnecter = (Connection.GetInstance().GetStrConnection().Equals("Connecté") ? false : false);
        //public bool vConnecter
        //{
        //    get { return this._vConnecter; }
        //    set
        //    {
        //        if (!string.Equals(this._vConnecter, value))
        //        {
        //            this._vConnecter = value;
        //            RaisePropertyChanged(nameof(vConnecter));
        //        }
        //    }
        //}
        //#endregion
        
                   
        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                    RaisePropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }
        
        public void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        ////Se connecter
        //private void ActionSeConnecterFilm(object parameter)
        //{
        //    LoginView app = new LoginView();
        //    LoginViewModel context = new LoginViewModel();
        //    app.DataContext = context;
        //    app.Show();
        //}
        
        #region Dropbox
        private string strAppKey = "wvay6mx0i0a2gbo";
        public string strAppSecret = "1qgfe6zpe62mqp3";
        private string strAccessToken = string.Empty;
        private string strAuthenticationURL = string.Empty;
        private DropBoxBase DBB;
        public ICommand ConnecterDP { get; set; }
        
        
        private string _strDP = " Connecter Droppbox";
        public string  strDP
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

        private void ActionConnecterDropbox(object paramter)
        {
            try
            {
                if (string.IsNullOrEmpty(strAppKey))
                {
                    MessageBox.Show("Please enter valid App Key !");
                    return;
                }
                if (DBB == null)
                {
                    DBB = new DropBoxBase(strAppKey, "PTM_Centralized");

                    strAuthenticationURL = DBB.GeneratedAuthenticationURL();
                    strAccessToken = DBB.GenerateAccessToken();
                    strDP = "Dropbox connecté";
                    lMVM.DgFiles = DBB.getEntries();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
