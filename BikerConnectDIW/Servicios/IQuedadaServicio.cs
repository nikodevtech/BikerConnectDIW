using BikerConnectDIW.DTO;
using DAL.Entidades;
using System.Runtime.InteropServices;

namespace BikerConnectDIW.Servicios
{
    /// <summary>
    /// Interfaz que declara los métodos que deben ser implementados para el servicio relacionado con las quedadas.
    /// </summary>
    public interface IQuedadaServicio
    {
        /// <summary>
        /// Obtiene todas las quedadas disponibles.
        /// </summary>
        /// <returns>Lista de DTOs de quedadas.</returns>
        public List<QuedadaDTO> obtenerQuedadas();

        /// <summary>
        /// Crea una nueva quedada.
        /// </summary>
        /// <param name="quedadaDTO">DTO de la quedada a crear.</param>
        /// <returns>True si la quedada se creó correctamente, False en caso contrario.</returns>
        public bool crearQuedada(QuedadaDTO quedadaDTO);

        /// <summary>
        /// Obtiene una quedada por su ID.
        /// </summary>
        /// <param name="id">ID de la quedada a obtener.</param>
        /// <returns>DTO de la quedada encontrada.</returns>
        public QuedadaDTO obtenerQuedadaPorId(long id);

        /// <summary>
        /// Obtiene una lista de usuarios participantes en una quedada.
        /// </summary>
        /// <param name="idQuedada">ID de la quedada.</param>
        /// <returns>Lista de DTOs de usuarios participantes.</returns>
        public List<UsuarioDTO> obtenerUsuariosParticipantes(long idQuedada);

        /// <summary>
        /// Permite a un usuario unirse a una quedada.
        /// </summary>
        /// <param name="idQuedada">ID de la quedada a la que unirse.</param>
        /// <param name="emailUsuario">Email del usuario que se une.</param>
        /// <returns>Un mensaje indicando el resultado de la operación.</returns>
        public string unirseQuedada(long idQuedada, string emailUsuario);

        /// <summary>
        /// Verifica si un usuario está unido a una quedada.
        /// </summary>
        /// <param name="idQuedada">ID de la quedada.</param>
        /// <param name="emailUsuario">Email del usuario.</param>
        /// <returns>True si el usuario está unido a la quedada, False en caso contrario.</returns>
        public bool estaUsuarioUnido(long idQuedada, string emailUsuario);

        /// <summary>
        /// Cancela la asistencia de un usuario a una quedada.
        /// </summary>
        /// <param name="idQuedada">ID de la quedada.</param>
        /// <param name="emailUsuario">Email del usuario.</param>
        /// <returns>True si se canceló la asistencia correctamente, False en caso contrario.</returns>
        public bool cancelarAsistenciaQuedada(long idQuedada, string emailUsuario);

        /// <summary>
        /// Obtiene todas las quedadas de un usuario por su ID.
        /// </summary>
        /// <param name="idUsuario">ID del usuario.</param>
        /// <returns>Lista de DTOs de quedadas del usuario.</returns>
        public List<QuedadaDTO> obtenerQuedadasDelUsuario(long idUsuario);

        /// <summary>
        /// Cancela una quedada por su ID.
        /// </summary>
        /// <param name="idQuedada">ID de la quedada a cancelar.</param>
        /// <returns>Un mensaje indicando el resultado de la operación.</returns>
        public string cancelarQuedada(long idQuedada);

        /// <summary>
        /// Actualiza el estado de una quedada.
        /// </summary>
        /// <param name="quedadaDTO">DTO con la info de la quedada</param>
        public void actualizarQuedada(QuedadaDTO quedadaDTO);
    }
}
