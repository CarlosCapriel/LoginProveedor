using LoginProveedor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginProveedor.Services
{
    public interface IServices
    {
        bool SaveUser(LoginAdmin user);
        bool verifyAccount(string email, string password);
        bool VerifyEmail(string email);
        bool EmailExist(string email);
        string hashPassword(string password);
        bool VerifyPassword(string password);
        bool PasswordNotTheSame(string email, string password);
        int UpdatePassword(string email, string password);
        string GetUserName(string email);
        object messageResponse(int status, string info);
        object messageResponse(int status, string info, string key, string value);
        LoginAdmin[] getAdmins();
    }
}
