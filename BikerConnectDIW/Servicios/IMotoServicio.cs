using BikerConnectDIW.DTO;

namespace BikerConnectDIW.Servicios
{
    public interface IMotoServicio
    {
        public bool registrarMoto(MotoDTO motoDTO);

        public void eliminarMoto(long id);

        public MotoDTO buscarPorId(long id);

        List<MotoDTO> obtenerMotosPorPropietarioId(long id);
    }
}
