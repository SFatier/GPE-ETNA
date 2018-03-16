using GPE;
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
        List<Utilisateur> lstUtilisateur = new List<Utilisateur>();
        List<Projet> lstProject = new List<Projet>();
        DropBoxBase DBB;
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

        /********/

        public void addUtilisateur(Utilisateur u)
        {
            lstUtilisateur.Add(u);
        }
        public List<Utilisateur> GetAllUtilisateur()
        {
            return lstUtilisateur;
        }

        public void SetListUtilisateur(List<Utilisateur> lstu)
        {
            lstUtilisateur.Clear();
            lstUtilisateur = lstu;
        }

        /********/

        public void SetDBB(DropBoxBase _DBB)
        {
            DBB = _DBB;
        }
        public DropBoxBase GetDBB()
        {
            return DBB;
        }

        /********/

        public void addProject(Projet p)
        {
            lstProject.Add(p);
        }
        public List<Projet> GetAllProject()
        {
            return lstProject;
        }

        public void SetListProject(List<Projet> _lstProject)
        {
            lstProject.Clear();
            lstProject = _lstProject;
        }

        /********/
    }
}

