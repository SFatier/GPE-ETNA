﻿using IHM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Model
{
    public class Projet : ObservableObject
    {
        public string Nom { get; set; }
        private List<Files> lstFiles;
        private bool isChecked;

        public bool Ischecked
        {
            get { return this.isChecked; }
            set
            {
                if (!string.Equals(this.isChecked, value))
                {
                    this.isChecked = value;
                    RaisePropertyChanged(nameof(Ischecked));
                    Singleton.GetInstance().GetPopUp().setLstPChecked(Nom);
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
    }
}