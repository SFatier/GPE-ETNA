using IHM.Helpers;
using IHM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IHM.ModelView.HomePage
{
    public class HomePageModelView : ObservableObject, IPageViewModel
    {
        public string Name => "";

        public HomePageModelView()
        {
          //  EspaceDisponible = "90";
          //  EspaceIndisponible = "10";
        }

        //private string _EspaceDisponible ;
        //public string EspaceDisponible
        //{
        //    get { return this._EspaceDisponible; }
        //    set
        //    {
        //        if (!string.Equals(this._EspaceDisponible, value))
        //        {
        //            this._EspaceDisponible = value;
        //            RaisePropertyChanged(nameof(EspaceDisponible));
        //        }
        //    }
        //}

        //private string _EspaceIndisponible;
        //public string EspaceIndisponible
        //{
        //    get { return this._EspaceIndisponible; }
        //    set
        //    {
        //        if (!string.Equals(this._EspaceIndisponible, value))
        //        {
        //            this._EspaceIndisponible = value;
        //            RaisePropertyChanged(nameof(EspaceIndisponible));
        //        }
        //    }
        //}

        public void LoadAction()
        {
            
        }
    }
}
