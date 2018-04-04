using IHM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHM.Model
{
    public class Utilisateur : ObservableObject
    {
        private string image;
        private string token;
        private string login;
        private string email;
        private string mdp;
        private string role;

        public string Image
        {
            get { return this.image; }
            set
            {
                if (!string.Equals(this.image, value))
                {
                    this.image = value;
                    RaisePropertyChanged(nameof(Image));
                }
            }
        }

        public string Login
        {
            get { return this.login; }
            set
            {
                if (!string.Equals(this.login, value))
                {
                    this.login = value;
                    RaisePropertyChanged(nameof(Login));
                }
            }
        }

        public string Email
        {
            get { return this.email; }
            set
            {
                if (!string.Equals(this.email, value))
                {
                    this.email = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }

        public string MDP
        {
            get { return this.mdp; }
            set
            {
                if (!string.Equals(this.mdp, value))
                {
                    this.mdp = value;
                    RaisePropertyChanged(nameof(MDP));
                }
            }
        }

        public string Token
        {
            get { return this.token; }
            set
            {
                if (!string.Equals(this.token, value))
                {
                    this.token = value;
                    RaisePropertyChanged(nameof(Token));
                }
            }
        }

        public string Role
        {
            get { return this.role; }
            set
            {
                if (!string.Equals(this.role, value))
                {
                    this.role = value;
                    RaisePropertyChanged(nameof(Role));
                }
            }
        }
    }
}
