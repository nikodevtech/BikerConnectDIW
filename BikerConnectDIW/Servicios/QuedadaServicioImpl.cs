using BikerConnectDIW.DTO;
using DAL.Entidades;

namespace BikerConnectDIW.Servicios
{
    public class QuedadaServicioImpl : IQuedadaServicio
    {

        private readonly BikerconnectContext _contexto;
        private readonly IConvertirAdto _convertirAdto;

        public QuedadaServicioImpl(BikerconnectContext contexto, IConvertirAdto convertirAdto)
        {
            _convertirAdto = convertirAdto;
            _contexto = contexto;
        }

        public List<QuedadaDTO> obtenerQuedadas()
        {
            try 
            {
                List<Quedada> listaQuedadas = _contexto.Quedadas.ToList();
                if (listaQuedadas != null) 
                {
                    return _convertirAdto.listaQuedadaToDto(listaQuedadas);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n[ERROR QuedadaServicioImpl - obtenerQuedadas()] - Al obtener todas las quedadas (return null): {e}");
            }
            return null;
        }
    }
}
