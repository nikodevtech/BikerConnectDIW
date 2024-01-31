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
        private readonly IServicioEmail _servicioEmail;

        public UsuarioServicioImpl(
            BikerconnectContext contexto, 
            IServicioEncriptar servicioEncriptar,
            IConvertirAdao convertirAdao,
            IConvertirAdto convertirAdto,
            IServicioEmail servicioEmail
        )
        {
            _contexto = contexto;
            _servicioEncriptar = servicioEncriptar;
            _convertirAdao = convertirAdao;
            _convertirAdto = convertirAdto;
            _servicioEmail = servicioEmail;
        }
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO)
        {
            var usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == userDTO.EmailUsuario && !u.CuentaConfirmada);

            if (usuarioExistente != null)
            {
                userDTO.EmailUsuario = "EmailNoConfirmado";
                //ViewData["EmailNoConfirmado"] = "Ya existe un usuario registrado con ese email. Por favor, confirme su correo electrónico.";
                return userDTO;
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
                _servicioEmail.enviarEmailConfirmacion(userDTO.EmailUsuario, nombreUsuario, token);

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

        public bool ConfirmarCuenta(string token)
        {
            try
            {
                Usuario? usuarioExistente = _contexto.Usuarios.Where(u => u.TokenRecuperacion == token).FirstOrDefault();

                if (usuarioExistente != null && !usuarioExistente.CuentaConfirmada)
                {
                    // Entra en esta condición si el usuario existe y su cuenta no se ha confirmado
                    usuarioExistente.CuentaConfirmada = true;
                    usuarioExistente.TokenRecuperacion = null;
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    return true;
                }
                else
                {
                    Console.WriteLine("La cuenta no existe o ya está confirmada");
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - ConfirmarCuenta()] Error al confirmar la cuenta " + ae.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - ConfirmarCuenta()] Error al confirmar la cuenta " + e.Message);
                return false;
            }
        }

    }
}
