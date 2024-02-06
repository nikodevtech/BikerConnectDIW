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
        private readonly IMotoServicio _motoServicio;

        public QuedadaServicioImpl(
            BikerconnectContext contexto, 
            IConvertirAdto convertirAdto,
            IConvertirAdao convertirAdao,
            IMotoServicio motoServicio
            )
        {
            _convertirAdto = convertirAdto;
            _contexto = contexto;
            _convertirAdao = convertirAdao;
            _motoServicio = motoServicio;
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
                     .Include(p => p.IdUsuarioNavigation) 
                     .Where(p => p.IdQuedada == idQuedada)
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

                foreach(UsuarioDTO u in usuariosParticipantes) 
                {
                    u.MisMotos = _motoServicio.obtenerMotosPorPropietarioId(u.Id);
                }

                return usuariosParticipantes;
            }
            catch (Exception e) 
            {
                Console.WriteLine($"\n[ERROR QuedadaServicioImpl - obtenerQuedadaPorId()] - Error al obtener los usuarios participantes de una quedada: {e}");
                return null;
            }

        }

        public string unirseQuedada(long idQuedada, string emailUsuario)
        {
            try
            {
                Quedada? quedada = _contexto.Quedadas.FirstOrDefault(q => q.IdQuedada == idQuedada);
                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == emailUsuario);

                if (usuario == null || quedada == null)
                {
                    return "Usuario o quedada no encontrado";
                }

                if (quedada.Estado == "Completada")
                {
                    return "La quedada está completada";
                }

                bool participanteExistente = _contexto.Participantes
                    .Any(p => p.IdQuedada == idQuedada && p.IdUsuario == usuario.IdUsuario);

                if (participanteExistente)
                {
                    return "Ya estás unido a esta quedada";
                }

                Participante nuevoParticipante = new Participante();
                nuevoParticipante.IdQuedada = quedada.IdQuedada;
                nuevoParticipante.IdUsuario = usuario.IdUsuario;

                _contexto.Participantes.Add(nuevoParticipante);
                _contexto.SaveChanges();

                return "Usuario unido a la quedada";
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n[ERROR QuedadaServicioImpl - unirseQuedada()] - Error al unirse a una quedada: {e}");
                return null;
            }
        }

    }
}
