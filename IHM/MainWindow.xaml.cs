
using Com.CloudRail.SI;
using IHM.Helpers;
using IHM.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IHM
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CreateFileJSON();
        }

        /// <summary>
        /// Creation de fichier JSON dans le dossier temp de l'utilisateur
        /// </summary>
        private void CreateFileJSON()
        {
            string path_projet = Constant.path_projet;
            string path_role = Constant.path_role;
            string path_utilisateur = Constant.path_utilisateur;

            try
            {
                if (!File.Exists(path_projet))
                {
                    List<Projet> projet_demo = new List<Projet>();
                    projet_demo.Add(new Projet()
                    {
                        DateDeCreation = DateTime.Now,
                        IsprojetEncours = true,
                        LstUser = new List<Utilisateur>(),
                        LstFiles = new List<Fichier>(),
                        description = "Vous trouvez ci-dessous un exemple des documents de suivi et de contrôle qui s'utlisent pour tous les projets de développement soutenus par la FGC. Vous pouvez cliquer sur l'image du document pour visualiser ou télécharger le document complet.Les documents sont: le résumé illustré," +
                        "le questionnaire de projet,"+
                        "la fiche de suivi initiale," +
                        "le budget détaillé," +
                        "le budget recapitulatif," +
                        "l'organigramme du partenaire, le chronogramme d'activités," +
                        "la recommandation de la Commission technique," +
                        "le  rapport intermédiaire," +
                        "le rapport financier intermédiaire," +
                        "le rapport final," +
                        "la fiche de suivi finale," +
                        "le rapport financier final," +
                        "et la décharge de la Commission de contrôle financier.Voir aussi les documents de travail de la FGC.",
                        NomProject = "Projet Orissa-Badi, développement rural en Inde, Frères de nos Frères, 2010-2012"
                    });

                    projet_demo.Add(new Projet()
                    {
                        DateDeCreation = DateTime.Now,
                        LstUser = new List<Utilisateur>(),
                        LstFiles = new List<Fichier>(),
                        IsprojetEncours = true,
                        description = "Ce projet nous a été proposé par l'entreprise Beetween." +
                            "Beetween est une société de recrutement.Les recruteurs de la société reçoivent les" +
                            "demandes des entreprises qui cherchent un profil de candidat pour un poste particulier." +
                            "Beetween se charge de recruter la bonne personne et de la diriger vers l’entreprisedemandeuse." +
                            "En ce sens,les employés de Beetween reçoivent très souvent des demandeurs" +
                            "d’emploi.Les recruteurs se chargent de mener à bien un entretien pour déterminer si le profil" +
                            "de la personne correspond au besoin formulé par l’entreprise.L’entreprise demandeuse gagne" +
                            "donc du temps en ne s’entretenant qu’avec des candidats précédemment sélectionnés." +
                            "Toutefois,les coûts générés par le fait de recevoir des candidats quotidiennement est" +
                            "important,tant en terme de temps qu’en terme financier.L’idée de notre projet est alors" +
                            "venue." +
                            "Nous nous proposons de mettre en place un système de questionnaire vidéo,à mi - chemin" +
                            "entre un questionnaire papier et un entretien en face à face.Les questions et réponses se font" +
                            "par vidéo et la personne répondant au questionnaire doit répondre « du tac au tac » à chaque" +
                            "question posée, comme elle le ferait lors d'un entretien en face à face." +
                            "Les avantages sont nombreux,tant pour le recruteur que pour le candidat.Le candidat" +
                            "n’est plus obligé de se déplacer dans les locaux de l’entreprise et les recruteurs de Beetween" +
                            "peuvent s’atteler à d’autres tâches.De plus,les problèmes de disponibilité disparaissent.Les" +
                            "deux personnes peuvent consulter l’entretien ou y répondre quand ils le désirent puisque les" +
                            "entretiens ne sont font pas en direct mais par le biais de vidéos enregistrées. ",
                        NomProject = "Beetween"
                    });

                    using (StreamWriter file = File.CreateText(@path_projet))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, projet_demo);
                    }
                }

                if (!File.Exists(path_role))
                {

                    List<Fonctionnalites> fonctionnalite_secretaire = new List<Fonctionnalites>();
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Lister les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Downloader les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Supprimer les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Ajouter un dossier de dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Visualiser les fichiers d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Renommer un fichier d'un dropbox" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les roles des utilisateurs de l'application" });
                    fonctionnalite_secretaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Lier un fichier à un projet" });


                    List<Fonctionnalites> fonctionnalite_chef = new List<Fonctionnalites>();
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Lister les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Downloader les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Supprimer les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Ajouter un dossier de dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Visualiser les fichiers d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Renommer un fichier d'un dropbox" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les projets de l'application" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les roles des utilisateurs de l'application" });
                    fonctionnalite_chef.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Lier un fichier à un projet" });

                    List<Fonctionnalites> fonctionnalite_gestionnaire = new List<Fonctionnalites>();
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Lister les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Downloader les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Uploader les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Supprimer les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Ajouter un dossier de dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Visualiser les fichiers d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Renommer un fichier d'un dropbox" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gerer les utilisateurs de l'application" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Gerer les projets de l'application" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Gerer les roles des utilisateurs de l'application" });
                    fonctionnalite_gestionnaire.Add(new Fonctionnalites() { Ischecked = false, DateDeCreation = DateTime.Now, Nom = "Lier un fichier à un projet" });

                    List<Roles> lst_role = new List<Roles>();
                    lst_role.Add(new Roles { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Secrétaire", lstFontionnalites = fonctionnalite_secretaire });
                    lst_role.Add(new Roles() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Chef de projet", lstFontionnalites = fonctionnalite_chef });
                    lst_role.Add(new Roles() { Ischecked = true, DateDeCreation = DateTime.Now, Nom = "Gestionnaire de cloud", lstFontionnalites = fonctionnalite_gestionnaire });


                    using (StreamWriter file = File.CreateText(path_role))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, lst_role);
                    }
                }

                if (!File.Exists(path_utilisateur))
                {
                    using (StreamWriter file = File.CreateText(path_utilisateur))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, new List<Utilisateur>());
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}