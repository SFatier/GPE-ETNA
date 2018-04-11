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
        private ListModelView lMVM = new ListModelView();
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
            curentUtilisateur = u;
            DBB = new DropBoxBase(ConfigurationSettings.AppSettings["strAppKey"], "PTM_Centralized");
            Singleton.GetInstance().SetDBB(DBB); //Instance de la classe Dropboxbase

            BtnHome = path_img + "home.png";
            BtnGestionUtilisateur = path_img + "GestionUtilisateur.png";
            BtnGestionProject = path_img + "project.png";
            BtnPerso = path_img + "perso.png";
            Background = path_img + "background.jpg";
            BtnGestionFichiers = path_img + "GestionFichiers.png";
            BtnGestionRoles = path_img + "role.png";

            if (curentUtilisateur.Token != null)
            {
                DBB.GetDBClient(curentUtilisateur.Token);
                //GetFiles();
            }

            ContentViewModels.Add(new HomePageModelView());
            CurrentContentViewModel = ContentViewModels[0];

            LoadAction();
            Singleton.GetInstance().SetHomeModelView(this);
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

        private string _BtnHome;
        public string BtnHome
        {
            get { return this._BtnHome; }
            set
            {
                if (!string.Equals(this._BtnHome, value))
                {
                    this._BtnHome = value;
                    RaisePropertyChanged(nameof(BtnHome));
                }
            }
        }

        private string _BtnGestionUtilisateur;
        public string BtnGestionUtilisateur
        {
            get { return this._BtnGestionUtilisateur; }
            set
            {
                if (!string.Equals(this._BtnGestionUtilisateur, value))
                {
                    this._BtnGestionUtilisateur = value;
                    RaisePropertyChanged(nameof(BtnGestionUtilisateur));
                }
            }
        }

        private string _BtnGestionProject;
        public string BtnGestionProject
        {
            get { return this._BtnGestionProject; }
            set
            {
                if (!string.Equals(this._BtnGestionProject, value))
                {
                    this._BtnGestionProject = value;
                    RaisePropertyChanged(nameof(BtnGestionProject));
                }
            }
        }

        private string _BtnPerso;
        public string BtnPerso
        {
            get { return this._BtnPerso; }
            set
            {
                if (!string.Equals(this._BtnPerso, value))
                {
                    this._BtnPerso = value;
                    RaisePropertyChanged(nameof(BtnPerso));
                }
            }
        }

        private string _BtnGestionFichiers;
        public string BtnGestionFichiers
        {
            get { return this._BtnGestionFichiers; }
            set
            {
                if (!string.Equals(this._BtnGestionFichiers, value))
                {
                    this._BtnGestionFichiers = value;
                    RaisePropertyChanged(nameof(BtnGestionFichiers));
                }
            }
        }

        private string _BtnGestionRoles;
        public string BtnGestionRoles
        {
            get { return this._BtnGestionRoles; }
            set
            {
                if (!string.Equals(this._BtnGestionRoles, value))
                {
                    this._BtnGestionRoles = value;
                    RaisePropertyChanged(nameof(BtnGestionRoles));
                }
            }
        }
        

        private string _Background;
        public string Background
        {
            get { return this._Background; }
            set
            {
                if (!string.Equals(this._Background, value))
                {
                    this._Background = value;
                    RaisePropertyChanged(nameof(Background));
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
            lMVM.DgFiles = DBB.getEntries(lMVM);
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
