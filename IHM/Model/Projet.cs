using IHM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Model
{
    public class Projet : ObservableObject
    {
        public string Nom { get; set; }
        private bool isChecked;

        public bool Ischecked
        {
            get { return this.isChecked; }
            set
            {
                if (!string.Equals(this.isChecked, value))
                {
                    this.isChecked = value;
                    RaisePropertyChanged(nameof(Ischecked));
                    Singleton.GetInstance().GetPopUp().setLstPChecked(Nom);
                }
            }
        }
        
    }
}
