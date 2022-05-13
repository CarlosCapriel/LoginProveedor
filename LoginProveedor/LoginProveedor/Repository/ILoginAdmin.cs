using LoginProveedor.Model;

namespace LoginProveedor.Repository
{
    public interface ILoginAdmin
    {
        LoginAdmin[] getAdmin();
        bool adminExist(string email, string password);
        bool UpdatePassword(string email, string newPassword);
        bool PasswordNotTheSame(string email, string newPassword);
        string GetUserName(string email);
        void SaveUser(LoginAdmin admin);
        bool EmailExist(string email);
    }
}
