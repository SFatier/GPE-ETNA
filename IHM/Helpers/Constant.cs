using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Helpers
{
    class Constant
    {
        public static string path_projet = System.IO.Path.GetTempPath() + "projets.json";
        public static string path_role = System.IO.Path.GetTempPath() + "roles.json";
        public static string path_utilisateur = System.IO.Path.GetTempPath() + "utilisateurs.json";
        public static string ClientSecretJSON =  Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName , "client_secret.json");
        public static string strAppSecretGoogle = System.IO.Path.GetTempPath() + "DriveServiceCredentials";
        public static string strAppKey = "wvay6mx0i0a2gbo";
        public static string strAppSecret = "1qgfe6zpe62mqp3";
     }
}
