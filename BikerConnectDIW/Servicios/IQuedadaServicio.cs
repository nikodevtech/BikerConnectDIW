using BikerConnectDIW.DTO;

namespace BikerConnectDIW.Servicios
{
    public interface IQuedadaServicio
    {
        public List<QuedadaDTO> obtenerQuedadas();

        public bool crearQuedada(QuedadaDTO quedadaDTO);
    }
}
