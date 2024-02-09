using BikerConnectDIW.DTO;

namespace BikerConnectDIW.Servicios
{
    /// <summary>
    /// Interfaz que declara los métodos que deben ser implementados para el servicio relacionado con las motos.
    /// </summary>
    public interface IMotoServicio
    {
        /// <summary>
        /// Registra una nueva moto.
        /// </summary>
        /// <param name="motoDTO">DTO de la moto a registrar.</param>
        /// <returns>True si la moto se registró correctamente, False en caso contrario.</returns>
        public bool registrarMoto(MotoDTO motoDTO);

        /// <summary>
        /// Elimina una moto por su ID.
        /// </summary>
        /// <param name="id">ID de la moto a eliminar.</param>
        public void eliminarMoto(long id);

        /// <summary>
        /// Busca una moto por su ID.
        /// </summary>
        /// <param name="id">ID de la moto a buscar.</param>
        /// <returns>DTO de la moto encontrada.</returns>
        public MotoDTO buscarPorId(long id);

        /// <summary>
        /// Obtiene una lista de motos por el ID del propietario.
        /// </summary>
        /// <param name="id">ID del propietario de las motos.</param>
        /// <returns>Lista de DTOs de motos pertenecientes al propietario.</returns>
        List<MotoDTO> obtenerMotosPorPropietarioId(long id);
    }
}
