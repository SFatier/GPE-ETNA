using IHM.Helpers;
using IHM.Model;
using IHM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IHM.ModelView
{
    public class AddProjectModelView : ObservableObject, IPageViewModel
    {
        public string Name => throw new NotImplementedException();
        public ICommand  Save{ get; set; }
        
        public AddProjectModelView()
        {
            Save = new RelayCommand(ActionAddProject);
        }

        #region [Binding]

        private string nomProjet;
        public string NomProjet
        {
            get { return this.nomProjet; }
            set
            {
                if (!string.Equals(this.nomProjet, value))
                {
                    this.nomProjet = value;
                    RaisePropertyChanged(nameof(NomProjet));
                }
            }
        }

        private string descriptionProjet;
        public string DescriptionProjet
        {
            get { return this.descriptionProjet; }
            set
            {
                if (!string.Equals(this.descriptionProjet, value))
                {
                    this.descriptionProjet = value;
                    RaisePropertyChanged(nameof(DescriptionProjet));
                }
            }
        }

        #endregion

        #region [Action]

        private void ActionAddProject(object parameter)
        {
            if (NomProjet != "" && DescriptionProjet != "")
            {
                Projet p = new Projet();
                p.Nom = NomProjet;
                p.Description = DescriptionProjet;
                p.LstFiles = new List<Files>();
                Singleton.GetInstance().addProject(p);

                #region [Ecriture de l'utilisateur dans le fichier .JSON]
                try
                {
                    using (StreamWriter file = File.CreateText(@"C:\Users\sigt_sf\Documents\GitHub\GPE-ETNA\IHM\projets.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, Singleton.GetInstance().GetAllProject());
                    }

                    Singleton.GetInstance().GetHomeModelView().CurrentContentViewModel = new AdminModelView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error :\" " + ex.Message);
                }
                #endregion
            }
            else
            {
                MessageBox.Show("Veuillez renseigner tous les champs obligatoires");
            }
           
            #endregion
        }
    }
}
