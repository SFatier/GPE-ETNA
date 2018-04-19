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
            if (Singleton.GetInstance().GetUtilisateur().Role != "Admin")
            {
                btnAjouterProjet.Visibility = Visibility.Hidden;
                ItemsList.ContextMenu.IsEnabled = false;
                ItemsList.ContextMenu.Visibility = Visibility.Hidden;
                lblPageProjets.Content = "Mes projets attribués";
            }
        }
    }
}
