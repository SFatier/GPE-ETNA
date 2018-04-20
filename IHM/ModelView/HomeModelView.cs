using GPE;
using IHM.Helpers;
using IHM.Model;
using IHM.ModelView.HomePage;
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
    public class HomeModelView : ObservableObject, IPageViewModel
    {
        #region [ Fields ]   
        private static string path_img = ConfigurationSettings.AppSettings["FolderIMG"];
        private Utilisateur curentUtilisateur;
        public ListModelView lMVM = new ListModelView();
        public string Name => "Home";
        private IPageViewModel _currentContentViewModel;
        private List<IPageViewModel> _contentViewModels;        
        private DropBoxBase DBB;

        public ICommand PageAdmin { get; set; }
        public ICommand PageHome { get; set; }
        public ICommand PagePerso { get; set; }
        public ICommand PageUser { get; set; }
        public ICommand Disconnect { get; set; }
        public ICommand PageFichiers { get; set; }
        public ICommand PageRoles { get; set; }
        #endregion

        #region [Constructor]
        public HomeModelView(Utilisateur u)
        {
            Singleton.GetInstance().SetHomeModelView(this);
                        
            curentUtilisateur = u;
            DBB = new DropBoxBase(ConfigurationSettings.AppSettings["strAppKey"], "PTM_Centralized");
            Singleton.GetInstance().SetDBB(DBB); //Instance de la classe Dropboxbase
                        
            if (curentUtilisateur.Token != null && curentUtilisateur.Token != "")
            {
                DBB.GetDBClient(curentUtilisateur.Token);
                GetFiles();
                GetFilesShared();
            }

            ContentViewModels.Add(new HomePageModelView());
            CurrentContentViewModel = ContentViewModels[0];

            LoadAction();
        }

        public void GetFilesShared()
        {
            List<Files> lst = DBB.GetFilesShared();
            lMVM.FilesShared = lst;
            if (lMVM.FilesShared.Count > 0)
            {
                foreach (Files f in lMVM.FilesShared)
                {
                    lMVM.DgFiles.Add(f);
                }
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

        #region [Binding]
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
           
        
        #endregion

        #region [Actions]
        public void LoadAction()
        {       
            PageAdmin = new RelayCommand(ActionPageAdmin);
            PageHome = new RelayCommand(ActionPageHome);
            PagePerso = new RelayCommand(ActionPagePerso);
            PageUser = new RelayCommand(ActionPageUtilisateurs);
            PageFichiers = new RelayCommand(ActionPageFichiers);
            PageRoles = new RelayCommand(ActionPageRoles);
            Disconnect = new RelayCommand(ActionDisconnect);
        }

        private void ActionPageFichiers(object obj)
        {
            CurrentContentViewModel = lMVM;
        }

        public void GetFiles()
        {
            lMVM.DgFiles = DBB.getItemsDropbox();
        }

        public void GetProjets()
        {
            lMVM.LoadProject();
        }
                
        private void ActionPageRoles(object param)
        {
            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new RolesModelView();
        }

        /**
       * Retourne la page Administration
       * */
        private void ActionPageAdmin(object parameter)
        {
            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new AdminModelView();
        }

        /**
         * Retourne sur la page Home
         * */
        private void ActionPageHome(object parameter)
        {
            CurrentContentViewModel = new HomePageModelView();
        }

        private void ActionPagePerso(object paramter)
        {
            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new PersonalModelView();
        }

        private void ActionPageUtilisateurs(object paramter)
        {
            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new ListUsersModelView();
        }

         private void ActionDisconnect(object paramter)
        {
            Singleton.GetInstance().GetMainWindowViewModel().App.Close();
        }
        
        #endregion
    }
}
