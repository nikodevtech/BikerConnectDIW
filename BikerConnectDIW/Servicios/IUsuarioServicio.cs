using BikerConnectDIW.DTO;

namespace BikerConnectDIW.Servicios
{
    public interface IUsuarioServicio
    {
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO);

        public bool confirmarCuenta(string token);

        public bool iniciarProcesoRecuperacion(string emailUsuario);

        public UsuarioDTO obtenerUsuarioPorToken(string token);

        public bool modificarContraseñaConToken(UsuarioDTO usuario);

        bool verificarCredenciales(string emailUsuario, string claveUsuario);

        public UsuarioDTO obtenerUsuarioPorEmail(string email);

        public List<UsuarioDTO> obtenerTodosLosUsuarios();

        public UsuarioDTO buscarPorId(long id);

        public void eliminar(long id);

        public void actualizarUsuario(UsuarioDTO usuarioModificado);
    }
}
