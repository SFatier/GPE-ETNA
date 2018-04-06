﻿using IHM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Model
{
    public class Base : ObservableObject
    {
        private string nom;
        private DateTime dateDeCreation;
        private List<Files> lstFiles;

        public DateTime DateDeCreation
        {
            get { return this.dateDeCreation; }
            set
            {
                if (!string.Equals(this.dateDeCreation, value))
                {
                    this.dateDeCreation = value;
                    RaisePropertyChanged(nameof(DateDeCreation));
                }
            }
        }

        public List<Files> LstFiles
        {
            get { return this.lstFiles; }
            set
            {
                if (!string.Equals(this.lstFiles, value))
                {
                    this.lstFiles = value;
                    RaisePropertyChanged(nameof(LstFiles));
                }
            }
        }
        
        public string Nom
        {
            get { return this.nom; }
            set
            {
                if (!string.Equals(this.nom, value))
                {
                    this.nom = value;
                    RaisePropertyChanged(nameof(Nom));
                }
            }
        }
    }
}
