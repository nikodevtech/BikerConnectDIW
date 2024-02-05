using BikerConnectDIW.DTO;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BikerConnectDIW.Servicios
{
    public class QuedadaServicioImpl : IQuedadaServicio
    {

        private readonly BikerconnectContext _contexto;
        private readonly IConvertirAdto _convertirAdto;
        private readonly IConvertirAdao _convertirAdao;

        public QuedadaServicioImpl(
            BikerconnectContext contexto, 
            IConvertirAdto convertirAdto,
            IConvertirAdao convertirAdao
            )
        {
            _convertirAdto = convertirAdto;
            _contexto = contexto;
            _convertirAdao = convertirAdao;
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

        public bool crearQuedada(QuedadaDTO quedadaDTO)
        {
            try
            {
                Quedada quedada = _convertirAdao.quedadaToDao(quedadaDTO);

                _contexto.Quedadas.Add(quedada);
                _contexto.SaveChanges();

                return true;
            }
            catch (DbUpdateException dbe) 
            {
                Console.WriteLine($"\n[ERROR QuedadaServicioImpl - crearQuedada()] - Erro de persistencia al registrar nueva quedada: {dbe}");
                return false;
            }
           
        }
    }
}
