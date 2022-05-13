
using LoginProveedor.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginProveedor.Repository
{
    public class ImpLoginAdmin : ILoginAdmin
    {
        private readonly ProjectContext.ProjectContext _connectionString; 
        public ImpLoginAdmin(ProjectContext.ProjectContext connectionString)
        {
            _connectionString = connectionString;
        }

        public bool adminExist(string email, string password)
        {
            //la contraseña ya se recibe hasheada
            var exist = _connectionString.LoginAdmin.Where(b => b.Email == email && b.Password == password);
            if (!(exist.Count() > 0))
            {
                return false;
            }

            return true;
        }

        public bool EmailExist(string email)
        {
            var exist = _connectionString.LoginAdmin.Where(b => b.Email == email);
            if (!(exist.Count() > 0))
            {
                return false;
            }

            return true;
        }

        public LoginAdmin[] getAdmin()
        {
            return _connectionString.LoginAdmin.ToArray();
        }

        public string GetUserName(string email)
        {
            var registro = _connectionString.LoginAdmin.Where(b => b.Email == email).Select(n => n.Nombre);

            string[] valor = registro.ToArray();

            return valor[0];
        }

        public bool PasswordNotTheSame(string email, string newPassword)
        {
            var registro = _connectionString.LoginAdmin.Where(b => b.Email == email).Select(n => n.Password);
            string[] register = registro.ToArray();
            if (register[0].Trim().Equals(newPassword))
            {
                return false;
            }

            return true;
        }

        public async void SaveUser(LoginAdmin admin)
        {
            await _connectionString.LoginAdmin.AddAsync(admin);
            _connectionString.SaveChangesAsync();
        }

        public bool UpdatePassword(string email, string newPassword)
        {
            var user = _connectionString.LoginAdmin.FirstOrDefault(item => item.Email == email);
            user.Password = newPassword;
            _connectionString.SaveChanges();

            return true;
        }
    }
}
