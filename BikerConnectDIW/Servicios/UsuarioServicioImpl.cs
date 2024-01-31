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

            if (userDTO.CuentaConfirmada)
            {
                usuarioDao.CuentaConfirmada = true;
                _contexto.Usuarios.Add(usuarioDao);
                _contexto.SaveChanges();

            }
            else
            {
                usuarioDao.CuentaConfirmada = false;
                string token = GenerarToken();
                usuarioDao.TokenRecuperacion = token;
                _contexto.Usuarios.Add(usuarioDao);
                _contexto.SaveChanges();
                string nombreUsuario = usuarioDao.NombreApellidos;
                //emailServicio.enviarEmailConfirmacion(userDto.getEmailUsuario(), nombreUsuario, token);

            }

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
