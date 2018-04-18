using IHM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace IHM.Model
{
    public class Projet :  Base
    {
        public string description;
        private List<Utilisateur> lstUser;
        private string icoIsArchived;
        public string IcoToolTip { get; internal set; }
        
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

        public string Description
        {
            get { return this.description; }
            set
            {
                if (!string.Equals(this.description, value))
                {
                    this.description = value;
                    RaisePropertyChanged(nameof(Description));
                }
            }
        }

       public string IcoIsArchived
        {
            get { return this.icoIsArchived; }
            set
            {
                if (!string.Equals(this.icoIsArchived, value))
                {
                    this.icoIsArchived = value;
                    RaisePropertyChanged(nameof(IcoIsArchived));
                }
            }
        }

        private string rbEncours;
        public string RbEncours
        {
            get { return this.rbEncours; }
            set
            {
                if (!string.Equals(this.rbEncours, value))
                {
                    this.rbEncours = value;
                    RaisePropertyChanged(nameof(RbEncours));
                }
            }
        }

        private string rbFini;
        public string RbFini
        {
            get { return this.rbFini; }
            set
            {
                if (!string.Equals(this.rbFini, value))
                {
                    this.rbFini = value;
                    RaisePropertyChanged(nameof(RbFini));
                }
            }
        }

        public bool isprojetEncours;
        public bool IsprojetEncours
        {
            get { return this.isprojetEncours; }
            set
            {
                if (!string.Equals(this.isprojetEncours, value))
                {
                    this.isprojetEncours = value;
                    RaisePropertyChanged(nameof(IsprojetEncours));
                }
            }
        }

        public bool isprojetFin;
        public bool IsprojetFin
        {
            get { return this.isprojetFin; }
            set
            {
                if (!string.Equals(this.isprojetFin, value))
                {
                    this.isprojetFin = value;
                    RaisePropertyChanged(nameof(IsprojetFin));
                }
            }
        }

        public string IcoArchived { get; internal set; }
    }
}
