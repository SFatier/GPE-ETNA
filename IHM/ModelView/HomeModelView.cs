using GPE;
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
    public class HomeModelView : ObservableObject, IPageViewModel
    {
        private ListModelView lMVM = new ListModelView();
        public string Name => "Home";
        private IPageViewModel _currentContentViewModel;
        private List<IPageViewModel> _contentViewModels;

        public HomeModelView()
        {
            ConnecterDP = new RelayCommand(ActionConnecterDropbox);
            ContentViewModels.Add(lMVM);
            ContentViewModels.Add(new ListModelView());
            CurrentContentViewModel = ContentViewModels[0];
            Singleton.GetInstance().SetHomeModelView(this);
        }

        #region Dropbox
        private string strAppKey = "wvay6mx0i0a2gbo";
        public string strAppSecret = "1qgfe6zpe62mqp3";
        private string strAccessToken = string.Empty;
        private string strAuthenticationURL = string.Empty;
        private DropBoxBase DBB;
        public ICommand ConnecterDP { get; set; }

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
                    Singleton.GetInstance().SetDBB(DBB); //Instance de la classe Dropboxbase
                    strAuthenticationURL = DBB.GeneratedAuthenticationURL();
                    strAccessToken = DBB.GenerateAccessToken();
                    strDP = "Dropbox connecté";
                    lMVM.DgFiles = DBB.getEntries(lMVM);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ChangeContent

        public List<IPageViewModel> ContentViewModels
        {
            get
            {
                if (_contentViewModels == null)
                    _contentViewModels = new List<IPageViewModel>();

                return _contentViewModels;
            }
        }

        public IPageViewModel CurrentContentViewModel
        {
            get
            {
                return _currentContentViewModel;
            }
            set
            {
                if (_currentContentViewModel != value)
                {
                    _currentContentViewModel = value;
                    OnPropertyChanged("CurrentContentViewModel");
                    RaisePropertyChanged(nameof(CurrentContentViewModel));
                }
            }
        }

        public void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!ContentViewModels.Contains(viewModel))
                ContentViewModels.Add(viewModel);

            CurrentContentViewModel = ContentViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }
        #endregion
        
        private string _IsConnect = "Se connecter";
        public string IsConnect
        {
            get { return this._IsConnect; }
            set
            {
                if (!string.Equals(this._IsConnect, value))
                {
                    this._IsConnect = value;
                    RaisePropertyChanged(nameof(IsConnect));
                }
            }
        }
    }
}
