using BikerConnectDIW.DTO;
using DAL.Entidades;

namespace BikerConnectDIW.Servicios
{
    /// <summary>
    /// Interface donde se declaran los métodos que serán necesarios implementar para el paso de DAO a DTO.
    /// </summary>
    public interface IConvertirAdto
    {
        /// <summary>
        /// Convierte un objeto Usuario de la capa de acceso a datos en un objeto UsuarioDTO.
        /// </summary>
        /// <param name="u">Objeto Usuario a convertir.</param>
        /// <returns>Objeto UsuarioDTO convertido.</returns>
        public UsuarioDTO usuarioToDto(Usuario u);

        /// <summary>
        /// Convierte una lista de objetos Usuario de la capa de acceso a datos en una lista de objetos UsuarioDTO.
        /// </summary>
        /// <param name="listaUsuario">Lista de objetos Usuario a convertir.</param>
        /// <returns>Lista de objetos UsuarioDTO convertidos.</returns>
        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario);

        /// <summary>
        /// Convierte un objeto Moto de la capa de acceso a datos en un objeto MotoDTO.
        /// </summary>
        /// <param name="u">Objeto Moto a convertir.</param>
        /// <returns>Objeto MotoDTO convertido.</returns>
        public MotoDTO motoToDto(Moto u);

        /// <summary>
        /// Convierte una lista de objetos Moto de la capa de acceso a datos en una lista de objetos MotoDTO.
        /// </summary>
        /// <param name="listaMotos">Lista de objetos Moto a convertir.</param>
        /// <returns>Lista de objetos MotoDTO convertidos.</returns>
        public List<MotoDTO> listaMotosToDto(List<Moto> listaMotos);

        /// <summary>
        /// Convierte una lista de objetos Quedada de la capa de acceso a datos en una lista de objetos QuedadaDTO.
        /// </summary>
        /// <param name="listaQuedada">Lista de objetos Quedada a convertir.</param>
        /// <returns>Lista de objetos QuedadaDTO convertidos.</returns>
        public List<QuedadaDTO> listaQuedadaToDto(List<Quedada> listaQuedada);

        /// <summary>
        /// Convierte un objeto Quedada de la capa de acceso a datos en un objeto QuedadaDTO.
        /// </summary>
        /// <param name="q">Objeto Quedada a convertir.</param>
        /// <returns>Objeto QuedadaDTO convertido.</returns>
        public QuedadaDTO quedadaToDto(Quedada q);
    }
}
