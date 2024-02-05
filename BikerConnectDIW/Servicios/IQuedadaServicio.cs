using BikerConnectDIW.DTO;
using System.Runtime.InteropServices;

namespace BikerConnectDIW.Servicios
{
    public interface IQuedadaServicio
    {
        public List<QuedadaDTO> obtenerQuedadas();

        public bool crearQuedada(QuedadaDTO quedadaDTO);

        public QuedadaDTO obtenerQuedadaPorId(long id);

        public List<UsuarioDTO> obtenerUsuariosParticipantes(long idQuedada);
    }
}
