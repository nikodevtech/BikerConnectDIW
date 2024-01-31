using System.Security.Cryptography;
using System.Text;

namespace Poyecto_Gestor_Biblioteca_Web_Los_Rapidos.Servicios
{
    /// <summary>
    /// Clase que implementa y detalla la lógica para encriptar contraseña
    /// </summary>
    public class ServicioEncriptarImpl : IServicioEncriptar
    {
        public string Encriptar(string contraseña)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(contraseña);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
