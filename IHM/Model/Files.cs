using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Model
{
    public class Files
    {
        public int Id{ get; set; }
        public string IdDropbox{ get; set; }
        public string IMG { get; set; }
        public string Nom{ get; set; }
        public string Type{ get; set; }
        public DateTime DateDeCreation{ get; set; }
        public DateTime ModifieLe { get; set; }
        public int Taille{ get; set; }
        public bool IsFile { get; set; }

        public Files()
        {

        }

        public Files(int _Id, string _IdDropbox, string _IMG, string _Nom, string _Type, DateTime _DateDeCreation, DateTime _ModifieLe, int _Taille, bool _IsFile)
        {
            Id = _Id;
            IdDropbox = _IdDropbox;
            IMG = _IMG;
            Nom = _Nom;
            Type = _Type;
            DateDeCreation = _DateDeCreation;
            ModifieLe = _ModifieLe;
            Taille = _Taille;
            IsFile = _IsFile;
        }

        public Files(string _IdDropbox, string _Nom, string _IMG, string _Type, DateTime _DateDeCreation, DateTime _ModifieLe, int _Taille, bool _IsFile)
        {
            IdDropbox = _IdDropbox;
            Nom = _Nom;
            IMG = _IMG;
            Type = _Type;
            DateDeCreation = _DateDeCreation;
            ModifieLe = _ModifieLe;
            Taille = _Taille;
            IsFile = _IsFile;
        }
    }
}
