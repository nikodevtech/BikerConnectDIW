using BikerConnectDIW.DTO;
using DAL.Entidades;

namespace BikerConnectDIW.Servicios
{
    /// <summary>
    /// Interface donde se declaran los metodos que serán necesarios implementar para el paso de DTO a DAO
    /// </summary>
    public interface IConvertirAdao
    {
        /// <summary>
        /// Convierte un objeto UsuarioDTO en un objeto Usuario de la capa de acceso a datos.
        /// </summary>
        /// <param name="usuarioDTO">Objeto UsuarioDTO a convertir.</param>
        /// <returns>Objeto Usuario convertido.</returns>
        public Usuario usuarioToDao(UsuarioDTO usuarioDTO);

        /// <summary>
        /// Convierte una lista de objetos UsuarioDTO en una lista de objetos Usuario de la capa de acceso a datos.
        /// </summary>
        /// <param name="listaUsuarioDTO">Lista de objetos UsuarioDTO a convertir.</param>
        /// <returns>Lista de objetos Usuario convertidos.</returns>
        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO);

        /// <summary>
        /// Convierte un objeto MotoDTO en un objeto Moto de la capa de acceso a datos.
        /// </summary>
        /// <param name="motoDTO">Objeto MotoDTO a convertir.</param>
        /// <returns>Objeto Moto convertido.</returns>
        public Moto motoToDao(MotoDTO motoDTO);

        /// <summary>
        /// Convierte un objeto QuedadaDTO en un objeto Quedada de la capa de acceso a datos.
        /// </summary>
        /// <param name="quedadaDTO">Objeto QuedadaDTO a convertir.</param>
        /// <returns>Objeto Quedada convertido.</returns>
        public Quedada quedadaToDao(QuedadaDTO quedadaDTO);
    }
}
