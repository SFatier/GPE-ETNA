﻿using IHM.Helpers;
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
        public ListView()
        {
            InitializeComponent();

            if (Singleton.GetInstance().GetUtilisateur().Role != "Chef de projet")
            {
                BtnProject.Visibility = Visibility.Hidden;
            }
        }

        private void DgFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Singleton.GetInstance().GetHomeModelView().lMVM.GetFolder();
        }        
    }
}
