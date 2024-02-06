using BikerConnectDIW.DTO;
using DAL.Entidades;
using System.Runtime.InteropServices;

namespace BikerConnectDIW.Servicios
{
    public interface IQuedadaServicio
    {
        public List<QuedadaDTO> obtenerQuedadas();

        public bool crearQuedada(QuedadaDTO quedadaDTO);

        public QuedadaDTO obtenerQuedadaPorId(long id);

        public List<UsuarioDTO> obtenerUsuariosParticipantes(long idQuedada);

        public string unirseQuedada(long idQuedada, string emailUsuario);

        public bool estaUsuarioUnido(long idQuedada, String emailUsuario);

        public bool cancelarAsistenciaQuedada(long idQuedada, string emailUsuario);

        public List<QuedadaDTO> obtenerQuedadasDelUsuario(long idUsuario);
    }
}
