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
        private List<Utilisateur> lstUser;
       
           
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
