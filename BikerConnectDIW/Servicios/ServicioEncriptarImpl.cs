using System.Security.Cryptography;
using System.Text;

namespace BikerConnectDIW.Servicios
{
    public class ServicioEncriptarImpl: IServicioEncriptar  
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
