using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class AdminModelView : ObservableObject, IPageViewModel
    {
        public string Name => "Adminstration GED";
        private static string path_img = ConfigurationSettings.AppSettings["FolderIMG"]; //a modifier par rapport à votre ordinateur

        public ICommand AddProject { get; set; }

        #region [Constructor]
        public AdminModelView()
        {
            LstProject = Singleton.GetInstance().GetAllProject(); ;

            // btnTrash = path_img + "trash.png";

            AddProject = new RelayCommand(ActionAddProject);
        }
        #endregion

        #region [Binding]        
        private List<Projet> lstProject;
        public List<Projet> LstProject
        {
            get { return this.lstProject; }
            set
            {
                if (!string.Equals(this.lstProject, value))
                {
                    this.lstProject = value;
                    RaisePropertyChanged(nameof(LstProject));
                }
            }
        }

        private string _btnTrash;
        public string btnTrash
        {
            get { return this._btnTrash; }
            set
            {
                if (!string.Equals(this._btnTrash, value))
                {
                    this._btnTrash = value;
                    RaisePropertyChanged(nameof(btnTrash));
                }
            }
        }
        #endregion


        #region [Action]
        private void ActionAddProject(object parameter)
        {
            Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new AddProjectModelView();
        }

        #endregion
    }
}
