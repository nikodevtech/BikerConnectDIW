using System.Security.Cryptography;
using System.Text;

namespace BikerConnectDIW.Servicios
{
    public class ServicioEncriptarImpl : IServicioEncriptar
    {
        public string Encriptar(string contraseña)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(contraseña);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return hash;
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("[Error  ServicioEncriptarImpl - Encriptar()] Error al encriptar: " + e.Message);
                return null;
            }

        }
    }
}
