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
                return userDTO;
            }

            var emailExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == userDTO.EmailUsuario && u.CuentaConfirmada);

            if (emailExistente != null)
            {
                userDTO.EmailUsuario = "EmailRepetido";
                return userDTO;
            }

            userDTO.ClaveUsuario = _servicioEncriptar.Encriptar(userDTO.ClaveUsuario);
            Usuario usuarioDao = _convertirAdao.usuarioToDao(userDTO);
            usuarioDao.FchRegistro = DateTime.Now;
            usuarioDao.Rol = "ROLE_USER";
            string token = generarToken();
            usuarioDao.TokenRecuperacion = token;

            _contexto.Usuarios.Add(usuarioDao);
            _contexto.SaveChanges();

            string nombreUsuario = usuarioDao.NombreApellidos;
            _servicioEmail.enviarEmailConfirmacion(userDTO.EmailUsuario, nombreUsuario, token);

            return userDTO;

        }
        private string generarToken()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenBytes = new byte[30];
                rng.GetBytes(tokenBytes);

                return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
            }
        }

        public bool confirmarCuenta(string token)
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

        public bool iniciarProcesoRecuperacion(string emailUsuario)
        {
            try
            {
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == emailUsuario);

                if (usuarioExistente != null)
                {
                    // Generar el token y establecer la fecha de expiración
                    string token = generarToken();
                    DateTime fechaExpiracion = DateTime.Now.AddMinutes(1);

                    // Actualizar el usuario con el nuevo token y la fecha de expiración
                    usuarioExistente.TokenRecuperacion = token;
                    usuarioExistente.FchExpiracionToken = fechaExpiracion;

                    // Actualizar el usuario en la base de datos
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    // Enviar el correo de recuperación
                    string nombreUsuario = usuarioExistente.NombreApellidos;
                    _servicioEmail.enviarEmailRecuperacion(emailUsuario, nombreUsuario, token);

                    return true;
                }
                else
                {
                    Console.WriteLine("El usuario con email " + emailUsuario + " no existe");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - IniciarProcesoRecuperacion()] Error al iniciar el proceso de recuperación: " + ex.Message);
                return false;
            }
        }

        public UsuarioDTO obtenerUsuarioPorToken(string token)
        {
            try
            {
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);

                if (usuarioExistente != null)
                {
                    UsuarioDTO usuario = _convertirAdto.usuarioToDto(usuarioExistente);
                    return usuario;
                }
                else
                {
                    Console.WriteLine("No existe el usuario con el token " + token);
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - ObtenerUsuarioPorToken()] Error al obtener usuario por token " + e.Message);
                return null;
            }
        }

        public bool modificarContraseñaConToken(UsuarioDTO usuario)
        {
            try
            {
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == usuario.Token);

                if (usuarioExistente != null)
                {
                    string nuevaContraseña = _servicioEncriptar.Encriptar(usuario.ClaveUsuario);
                    usuarioExistente.Contraseña = nuevaContraseña;
                    usuarioExistente.TokenRecuperacion = null; // Se establece como null para invalidar el token ya consumido al cambiar la contraseña
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - ModificarContraseñaConToken()] Error al modificar la contraseña con el token: " + e.Message);
            }
            return false;
        }

        public bool verificarCredenciales(string emailUsuario, string claveUsuario)
        {
            string contraseñaEncriptada = _servicioEncriptar.Encriptar(claveUsuario);
            Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == emailUsuario && u.Contraseña == contraseñaEncriptada);
            if (usuarioExistente == null)
            {
                return false;
            }
            if (!usuarioExistente.CuentaConfirmada)
            {
                return false;
            }

            return true;
        }

        public UsuarioDTO obtenerUsuarioPorEmail(string email) 
        {
            try
            {
                UsuarioDTO usuarioDTO = new UsuarioDTO();
                var usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == email);

                if (usuario != null) 
                {
                    usuarioDTO = _convertirAdto.usuarioToDto(usuario);
                }

                return usuarioDTO;
            }
            catch (Exception e) 
            {
                Console.WriteLine("[Error UsuarioServicioImpl - obtenerUsuarioPorEmail()] Error al obtener el usuario por email: " + e.Message);
                return null;
            }
        }
    }
}
