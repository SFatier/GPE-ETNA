using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
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
            //_u.LstFiles = GetUs();
            
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

        public void LoadAction()
        {
            throw new NotImplementedException();
        }
    }
}
