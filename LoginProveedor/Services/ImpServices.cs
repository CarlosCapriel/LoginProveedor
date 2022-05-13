using LoginProveedor.Model;
using LoginProveedor.Repository;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LoginProveedor.Services
{
    public class ImpServices : IServices
    {
        private ILoginAdmin _admins;
        public ImpServices(ILoginAdmin admins)
        {
            _admins = admins;
        }

        public bool EmailExist(string email)
        {
            return _admins.EmailExist(email);
        }

        public string GetUserName(string email)
        {
            return _admins.GetUserName(email);
        }

        public string hashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            string hashed = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8
                )
            );

            return hashed;
        }

        public object messageResponse(int status, string info)
        {
            return new
            {
                status = status,
                information = info
            };
        }

        public object messageResponse(int status, string info, string key, string value)
        {
            return new
            {
                status = status,
                information = info,
                key = value
            };
        }

        public bool PasswordNotTheSame(string email, string password)
        {
            return _admins.PasswordNotTheSame(email, password);
        }

        public bool SaveUser(LoginAdmin user)
        {
            if (user == null || EmailExist(user.Email))
            {
                return false;
            }

            user.Password = hashPassword(user.Password);
            _admins.SaveUser(user);

            return true;
        }

        public int UpdatePassword(string email, string password)
        {
            password = hashPassword(password);
            _admins.UpdatePassword(email, password);
            return 1;
        }

        public bool verifyAccount(string email, string password)
        {
            password = hashPassword(password);
            return _admins.adminExist(email, password);
        }

        public bool VerifyEmail(string email)
        {
            Regex regex = new Regex("^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$");
            if (regex.IsMatch(email))
            {
                return true;
            }
            return false;
        }

        public bool VerifyPassword(string password)
        {
            Regex regex = new Regex("^(?=.{8,}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?\\W)");
            if (regex.IsMatch(password))
            {
                return true;
            }
            return false;
        }

        public LoginAdmin[] getAdmins()
        {
            return _admins.getAdmin();
        }
    }
}
