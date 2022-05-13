using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginProveedor.Model
{
    public class LoginAdmin
    {
        
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string ApellidoP { get; set; }

        public string ApellidoM { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
