using BikerConnectDIW.DTO;
using System.Collections.Generic;

namespace BikerConnectDIW.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos necesarios para la gestión de usuarios.
    /// </summary>
    public interface IUsuarioServicio
    {
        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="userDTO">DTO del usuario a registrar.</param>
        /// <returns>DTO del usuario registrado.</returns>
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO);

        /// <summary>
        /// Confirma la cuenta de usuario utilizando un token.
        /// </summary>
        /// <param name="token">Token de confirmación de cuenta.</param>
        /// <returns>Booleano que indica si la cuenta fue confirmada correctamente.</returns>
        public bool confirmarCuenta(string token);

        /// <summary>
        /// Inicia el proceso de recuperación de contraseña para un usuario dado su correo electrónico.
        /// </summary>
        /// <param name="emailUsuario">Correo electrónico del usuario.</param>
        /// <returns>Booleano que indica si el proceso de recuperación fue iniciado correctamente.</returns>
        public bool iniciarProcesoRecuperacion(string emailUsuario);

        /// <summary>
        /// Obtiene un usuario utilizando un token de identificación.
        /// </summary>
        /// <param name="token">Token de identificación del usuario.</param>
        /// <returns>DTO del usuario encontrado.</returns>
        public UsuarioDTO obtenerUsuarioPorToken(string token);

        /// <summary>
        /// Modifica la contraseña de un usuario utilizando un token de identificación.
        /// </summary>
        /// <param name="usuario">DTO del usuario con la nueva contraseña y el token de identificación.</param>
        /// <returns>Booleano que indica si la contraseña fue modificada correctamente.</returns>
        public bool modificarContraseñaConToken(UsuarioDTO usuario);

        /// <summary>
        /// Verifica las credenciales de un usuario.
        /// </summary>
        /// <param name="emailUsuario">Correo electrónico del usuario.</param>
        /// <param name="claveUsuario">Clave de acceso del usuario.</param>
        /// <returns>Booleano que indica si las credenciales son válidas.</returns>
        bool verificarCredenciales(string emailUsuario, string claveUsuario);

        /// <summary>
        /// Obtiene un usuario por su dirección de correo electrónico.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <returns>DTO del usuario encontrado.</returns>
        public UsuarioDTO obtenerUsuarioPorEmail(string email);

        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema.
        /// </summary>
        /// <returns>Lista de DTOs de usuarios registrados.</returns>
        public List<UsuarioDTO> obtenerTodosLosUsuarios();

        /// <summary>
        /// Busca un usuario por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        /// <returns>DTO del usuario encontrado.</returns>
        public UsuarioDTO buscarPorId(long id);

        /// <summary>
        /// Elimina un usuario del sistema por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del usuario a eliminar.</param>
        public void eliminar(long id);

        /// <summary>
        /// Actualiza la información de un usuario en el sistema.
        /// </summary>
        /// <param name="usuarioModificado">DTO del usuario con la información actualizada.</param>
        public void actualizarUsuario(UsuarioDTO usuarioModificado);

        /// <summary>
        /// Cuenta el número de usuarios con un determinado rol.
        /// </summary>
        /// <param name="rol">Rol de usuario a contar.</param>
        /// <returns>Número de usuarios con el rol especificado.</returns>
        public int contarUsuariosPorRol(string rol);

        /// <summary>
        /// Busca usuarios cuyos correos electrónicos contienen una palabra específica.
        /// </summary>
        /// <param name="palabra">Palabra clave para la búsqueda de usuarios.</param>
        /// <returns>Lista de DTOs de usuarios encontrados.</returns>
        public List<UsuarioDTO> buscarPorCoincidenciaEnEmail(string palabra);
    }
}
