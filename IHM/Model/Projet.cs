using IHM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Model
{
    public class Projet :  Base
    {
        public string Description { get; set; }
        private List<Files> lstFiles;
        private List<Utilisateur> lstUser;
        private bool isChecked = false;
        
        public bool Ischecked
        {
            get { return this.isChecked; }
            set
            {
                if (!string.Equals(this.isChecked, value))
                {
                    this.isChecked = value;
                    RaisePropertyChanged(nameof(Ischecked));
                    if (Singleton.GetInstance().GetPopUp() != null)
                        Singleton.GetInstance().GetPopUp().setLstPChecked(Nom);
                }
            }
        }
           
        public List<Utilisateur> LstUser
        {
            get { return this.lstUser; }
            set
            {
                if (!string.Equals(this.lstUser, value))
                {
                    this.lstUser = value;
                    RaisePropertyChanged(nameof(LstUser));
                }
            }
        }
    }
}
