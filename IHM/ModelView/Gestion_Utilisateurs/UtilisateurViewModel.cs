using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.ModelView.Gestion_Utilisateurs
{
    public class UtilisateurViewModel : ObservableObject, IPageViewModel
    {
        public string Name => throw new NotImplementedException();
    
        public UtilisateurViewModel(Utilisateur _u)
        {
            _u.LstProjet = GetProjets(_u);
            ImgUser = ConfigurationSettings.AppSettings["FolderIMG"] + "user.png";
            Utilisateur = _u;
        }

        private List<Projet> GetProjets(Utilisateur u)
        {
            List<Projet> lst = Singleton.GetInstance().GetAllProject();
            List<Projet> rslt = new List<Projet>();
            if (lst.Count() > 0)
            {
                foreach(Projet p in lst)
                {
                    if (p.LstUser.Where(user => user.Email.Equals(u.Email)).Count() > 0)
                    {
                        rslt.Add(p);
                    }
                }
            }
            return rslt;
        }

        private Utilisateur _utilisateur;
        public Utilisateur Utilisateur
        {
            get { return this._utilisateur; }
            set
            {
                if (!string.Equals(this._utilisateur, value))
                {
                    this._utilisateur = value;
                    RaisePropertyChanged(nameof(Utilisateur));
                }
            }
        }

        private string _ImgUser;
        public string ImgUser
        {
            get { return this._ImgUser; }
            set
            {
                if (!string.Equals(this._ImgUser, value))
                {
                    this._ImgUser = value;
                    RaisePropertyChanged(nameof(ImgUser));
                }
            }
        }

        public void LoadAction()
        {
            throw new NotImplementedException();
        }
    }
}
