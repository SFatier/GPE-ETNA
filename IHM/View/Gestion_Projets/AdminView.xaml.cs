using IHM.Helpers;
using IHM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IHM.View
{
    /// <summary>
    /// Logique d'interaction pour AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        public AdminView()
        {
            InitializeComponent();
            if (Singleton.GetInstance().GetUtilisateur().Role != "Chef de projet")
            {
                //btnAjouterProjet.Visibility = Visibility.Hidden;
                //ItemsList.ContextMenu.IsEnabled = false;
                //ItemsList.ContextMenu.Visibility = Visibility.Hidden;
                //lblPageProjets.Content = "Mes projets attribués";
            }
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock test = sender as TextBlock;
            Fichier fichierSelected = test.DataContext as Fichier;
            string path = fichierSelected.path;
            Singleton.GetInstance().GetListModelView().OpenFile(fichierSelected);
        }
    }
}
