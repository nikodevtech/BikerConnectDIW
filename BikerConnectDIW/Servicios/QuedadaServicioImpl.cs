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
                Console.WriteLine($"\n[ERROR QuedadaServicioImpl - crearQuedada()] - Error de persistencia al registrar nueva quedada: {dbe}");
                return false;
            }
           
        }

        public QuedadaDTO obtenerQuedadaPorId(long id)
        {
            try 
            {
                Quedada? q = _contexto.Quedadas.Find(id);
                QuedadaDTO? quedada = new QuedadaDTO();
                quedada = _convertirAdto.quedadaToDto(q);
                return quedada;
            }
            catch(Exception e)
            {
                Console.WriteLine($"\n[ERROR QuedadaServicioImpl - obtenerQuedadaPorId()] - Error al obtener una quedada por su id: {e}");
                return null;
            }
        }

        public List<UsuarioDTO> obtenerUsuariosParticipantes(long idQuedada) 
        {
            try
            {
                var participantes = _contexto.Participantes
                    .Where(p => p.IdQuedada == idQuedada)
                    .Include(p => p.IdUsuarioNavigation) 
                    .ToList();

                List<UsuarioDTO> usuariosParticipantes = participantes
                    .Select(p => new UsuarioDTO
                    {
                        Id = p.IdUsuarioNavigation.IdUsuario,
                        NombreYapellidos = p.IdUsuarioNavigation.NombreApellidos,
                        EmailUsuario = p.IdUsuarioNavigation.Email,
                        TlfUsuario = p.IdUsuarioNavigation.TlfMovil,
                    })
                    .ToList();

                return usuariosParticipantes;
            }
            catch (Exception e) 
            {
                Console.WriteLine($"\n[ERROR QuedadaServicioImpl - obtenerQuedadaPorId()] - Error al obtener los usuarios participantes de una quedada: {e}");
                return null;
            }
        } 
    }
}
