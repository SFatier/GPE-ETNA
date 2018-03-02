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

        static Singleton _instance;
        public static Singleton GetInstance()
        {
            if (_instance == null)
                _instance = new Singleton();
            return _instance;
        }

        public void SetMainWindowViewModel(MainModelView mainWindowViewModel)
        {
            cMain = mainWindowViewModel;
        }
        public MainModelView GetMainWindowViewModel()
        {
            return cMain;
        }
    }
}

