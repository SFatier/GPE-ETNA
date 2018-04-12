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
            LoadProject();
            LoadAction();
        }

        private void LoadProject()
        {
            if (LstProject != null)
                LstProject.Clear();

             List<Projet> lstProject = Singleton.GetInstance().GetAllProject();
            foreach (Projet p in lstProject)
            {
                string img = p.IcoIsArchived;
                p.IcoArchived = path_img + img;
                if (img != "validate.png")
                {
                    p.IcoToolTip = "Projet fini";
                    p.IsprojetEncours = true;
                    p.IsprojetFin = false;
                }
                else
                {
                    p.IcoToolTip = "Projet en cours";
                    p.IsprojetEncours = false;
                    p.IsprojetFin = true;
                }
                p.RbEncours = path_img + "notvalidate.png";
                p.RbFini = path_img + "validate.png";
            }

            LstProject = lstProject;
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
        public string BtnTrash
        {
            get { return this._btnTrash; }
            set
            {
                if (!string.Equals(this._btnTrash, value))
                {
                    this._btnTrash = value;
                    RaisePropertyChanged(nameof(BtnTrash));
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

        public void LoadAction()
        {
            AddProject = new RelayCommand(ActionAddProject);
        }
    }
}
