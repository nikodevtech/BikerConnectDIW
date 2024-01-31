using BikerConnectDIW.DTO;
using DAL.Entidades;
using System.Security.Cryptography;

namespace BikerConnectDIW.Servicios
{
    public class UsuarioServicioImpl : IUsuarioServicio 
    {
        private readonly BikerconnectContext _contexto;
        private readonly IServicioEncriptar _servicioEncriptar;
        private readonly IConvertirAdao _convertirAdao;
        private readonly IConvertirAdto _convertirAdto;

        public UsuarioServicioImpl(
            BikerconnectContext contexto, 
            IServicioEncriptar servicioEncriptar,
            IConvertirAdao convertirAdao,
            IConvertirAdto convertirAdto
        )
        {
            _contexto = contexto;
            _servicioEncriptar = servicioEncriptar;
            _convertirAdao = convertirAdao;
            _convertirAdto = convertirAdto;
        }
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO)
        {
            var usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == userDTO.EmailUsuario);

            if (usuario != null)
            {
                return null;
            }

            userDTO.ClaveUsuario = _servicioEncriptar.Encriptar(userDTO.ClaveUsuario);
            Usuario usuarioDao = _convertirAdao.usuarioToDao(userDTO);
            usuarioDao.FchRegistro = DateTime.Now;
            usuarioDao.Rol = "ROLE_USER";

            string token = GenerarToken();
            usuarioDao.TokenRecuperacion = token;
            usuarioDao.FchExpiracionToken = DateTime.Now.AddMinutes(10);
            string nombreUsuario = usuarioDao.NombreApellidos;
            //if (userDTO.CuentaConfirmada) 
            //{
            //    usuarioDao.CuentaConfirmada = true;
            //    _contexto.Usuarios.Add(usuarioDao);
            //}
            //else
            //{
            //    usuarioDao.CuentaConfirmada = false;

            //    string token = GenerarToken();
            //    usuarioDao.TokenRecuperacion = token;
            //    usuarioDao.FchExpiracionToken = DateTime.Now.AddMinutes(10);
            //    string nombreUsuario = usuarioDao.NombreApellidos;


            //}

            _contexto.SaveChanges();
            return userDTO;

        }

        private string GenerarToken()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenBytes = new byte[30];
                rng.GetBytes(tokenBytes);

                return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
            }
        }

    }
}
