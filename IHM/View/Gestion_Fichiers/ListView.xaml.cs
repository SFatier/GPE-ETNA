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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IHM.View
{
    /// <summary>
    /// Logique d'interaction pour ListView.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        Utilisateur cUtilisateur = Singleton.GetInstance().GetUtilisateur();
        public ListView()
        {
            InitializeComponent();

            if (cUtilisateur.Role != "Chef de projet")
                BtnProject.Visibility = Visibility.Hidden;
            if (cUtilisateur.Token_DP != null)
                TabDropbox.Visibility = Visibility.Visible;
            if (cUtilisateur.Token_GG != null)
                TabGoogle.Visibility = Visibility.Visible;

            Singleton.GetInstance().GetHomeModelView().lMVM.RefreshTab();
        }

        private void DgFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Singleton.GetInstance().GetHomeModelView().lMVM.GetFolder();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            var Date = RechercheDate.SelectedDate;

            Singleton.GetInstance().GetHomeModelView().lMVM.Recherche_Date();


        }
        private void Periode_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {


            var Periode = RecherchePeriode.SelectedDate;

            Singleton.GetInstance().GetHomeModelView().lMVM.Recherche_Periode();
        }
    }
}
