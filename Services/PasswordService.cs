using System.Security.Cryptography;
using System.Text;

namespace PharmaStock.Services
{
    public static class PasswordService
    {
        public static string HashPassword(string password)
        {
           return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VerifyPassword(string enteredPassrod,string storedHash){//compara la contraseña ingresada con la contraseña almacenada
            return BCrypt.Net.BCrypt.Verify(enteredPassrod,storedHash);
        }
    }
}