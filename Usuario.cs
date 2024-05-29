using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupo_9__U20230358
{
    class Usuario
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public Usuario(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
