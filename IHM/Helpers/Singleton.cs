using IHM.Model;
using IHM.ModelView;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Helpers
{
    class Singleton
    {
        MainModelView cMain;
        HomeModelView cContent;
        Utilisateur cUtilisateur;
        PopUpModelView popUp;

        static Singleton _instance;
        public static Singleton GetInstance()
        {
            if (_instance == null)
                _instance = new Singleton();
            return _instance;
        }

        /********/

        public void SetMainWindowViewModel(MainModelView mainWindowViewModel)
        {
            cMain = mainWindowViewModel;
        }
        public MainModelView GetMainWindowViewModel()
        {
            return cMain;
        }

        /********/

        public void SetHomeModelView(HomeModelView homeModelView)
        {
            cContent = homeModelView;
        }
        public HomeModelView GetHomeModelView()
        {
            return cContent;
        }

        /********/

        public void SetUtilisateur(Utilisateur u)
        {
            cUtilisateur = u;
        }
        public Utilisateur GetUtilisateur()
        {
            return cUtilisateur;
        }

        /********/

        public void SetPopUp(PopUpModelView popup)
        {
            popUp = popup;
        }
        public PopUpModelView GetPopUp()
        {
            return popUp;
        }
    }
}

