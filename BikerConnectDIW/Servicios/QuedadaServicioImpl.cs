using BikerConnectDIW.DTO;
using BikerConnectDIW.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;

namespace BikerConnectDIW.Servicios
{
    /// <summary>
    /// Clase que implementa la interfaz IQuedadaServicio y detalla la lógica de los metodos que serán necesarios para la gestión de las quedadas
    /// </summary>
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
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerQuedadas() de la clase QuedadaServicioImpl");

                List<Quedada> listaQuedadas = _contexto.Quedadas.ToList();
                if (listaQuedadas != null)
                {
                    return _convertirAdto.listaQuedadaToDto(listaQuedadas);
                }
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR QuedadaServicioImpl - obtenerQuedadas()] - Argumento null al obtener todas las quedadas: {e}");
            }
            return null;
        }

        public bool crearQuedada(QuedadaDTO quedadaDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método crearQuedada() de la clase QuedadaServicioImpl");

                Quedada quedada = _convertirAdao.quedadaToDao(quedadaDTO);

                _contexto.Quedadas.Add(quedada);
                _contexto.SaveChanges();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método crearQuedada() de la clase QuedadaServicioImpl. Quedada creada OK");
                return true;
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR QuedadaServicioImpl - crearQuedada()] - Error de persistencia al registrar nueva quedada: {dbe}");
                return false;
            }

        }

        public QuedadaDTO obtenerQuedadaPorId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerQuedadaPorId() de la clase QuedadaServicioImpl");

                Quedada? q = _contexto.Quedadas.Find(id);
                QuedadaDTO? quedada = new QuedadaDTO();
                quedada = _convertirAdto.quedadaToDto(q);
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerQuedadaPorId() de la clase QuedadaServicioImpl. Quedada obtenida por id OK");
                return quedada;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR QuedadaServicioImpl - obtenerQuedadaPorId()] - Error al obtener una quedada por su id: {e}");
                return null;
            }
        }

        public List<UsuarioDTO> obtenerUsuariosParticipantes(long idQuedada)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerUsuariosParticipantes() de la clase QuedadaServicioImpl");

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

                foreach (UsuarioDTO u in usuariosParticipantes)
                {
                    u.MisMotos = _motoServicio.obtenerMotosPorPropietarioId(u.Id);
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerUsuariosParticipantes() de la clase QuedadaServicioImpl. Obtenidos participantes OK");
                return usuariosParticipantes;
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR QuedadaServicioImpl - obtenerQuedadaPorId()] - Argumento null al obtener los usuarios participantes de una quedada: {e}");
                return null;
            }

        }

        public string unirseQuedada(long idQuedada, string emailUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método unirseQuedada() de la clase QuedadaServicioImpl");

                Quedada? quedada = _contexto.Quedadas.FirstOrDefault(q => q.IdQuedada == idQuedada);
                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == emailUsuario);

                if (usuario == null || quedada == null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método unirseQuedada() de la clase QuedadaServicioImpl. No se pudo unir, usuario o quedada no encontrado.");
                    return "Usuario o quedada no encontrado";
                }

                if (quedada.Estado == "Completada")
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método unirseQuedada() de la clase QuedadaServicioImpl. No se pudo unir, la quedada está completada.");
                    return "La quedada está completada";
                }

                bool participanteExistente = _contexto.Participantes
                    .Any(p => p.IdQuedada == idQuedada && p.IdUsuario == usuario.IdUsuario);

                if (participanteExistente)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método unirseQuedada() de la clase QuedadaServicioImpl. El usuario ya se encontraba unido a la quedada.");
                    return "Ya estás unido a esta quedada";
                }

                Participante nuevoParticipante = new Participante();
                nuevoParticipante.IdQuedada = quedada.IdQuedada;
                nuevoParticipante.IdUsuario = usuario.IdUsuario;

                _contexto.Participantes.Add(nuevoParticipante);
                _contexto.SaveChanges();
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método unirseQuedada() de la clase QuedadaServicioImpl. Usuario unido a la quedada OK.");

                return "Usuario unido a la quedada";
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR QuedadaServicioImpl - unirseQuedada()] - Error de persistencia al unirse a una quedada: {dbe}");
                return null;
            }
        }

        public bool estaUsuarioUnido(long idQuedada, string emailUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método estaUsuarioUnido() de la clase QuedadaServicioImpl");

                Quedada? quedada = _contexto.Quedadas.FirstOrDefault(q => q.IdQuedada == idQuedada);
                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == emailUsuario);

                if (quedada != null && usuario != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método estaUsuarioUnido() de la clase QuedadaServicioImpl");
                    return _contexto.Participantes.Any(p => p.IdQuedada == idQuedada && p.IdUsuario == usuario.IdUsuario);
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método estaUsuarioUnido() de la clase QuedadaServicioImpl.");
                return false;
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR QuedadaServicioImpl - estaUsuarioUnido()] - Error de argumento nulo comprobar si un usuario está unido a una quedada: {e}");
                return false;
            }

        }

        public bool cancelarAsistenciaQuedada(long idQuedada, string emailUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método cancelarAsistenciaQuedada() de la clase QuedadaServicioImpl");

                Quedada? quedada = _contexto.Quedadas.FirstOrDefault(q => q.IdQuedada == idQuedada);
                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == emailUsuario);

                if (usuario == null || quedada == null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cancelarAsistenciaQuedada() de la clase QuedadaServicioImpl. No se pudo cancelar la asistencia");
                    return false;
                }

                Participante? participante = _contexto.Participantes.FirstOrDefault(p => p.IdQuedada == idQuedada && p.IdUsuario == usuario.IdUsuario);

                if (participante == null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cancelarAsistenciaQuedada() de la clase QuedadaServicioImpl. No se pudo cancelar la asistencia.");
                    return false;
                }

                _contexto.Participantes.Remove(participante);
                _contexto.SaveChanges();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cancelarAsistenciaQuedada() de la clase QuedadaServicioImpl. Asistencia cancelada OK.");
                return true;
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR QuedadaServicioImpl - cancelarAsistenciaQuedada()] - Error de persistencia al cancelar asistencia a una quedada: {dbe}");
                return false;
            }
        }

        public List<QuedadaDTO> obtenerQuedadasDelUsuario(long idUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerQuedadasDelUsuario() de la clase QuedadaServicioImpl");

                List<Quedada> misQuedadas = _contexto.Participantes
                   .Where(p => p.IdUsuario == idUsuario)
                   .Select(p => p.IdQuedadaNavigation)
                   .ToList();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerQuedadasDelUsuario() de la clase QuedadaServicioImpl");

                return _convertirAdto.listaQuedadaToDto(misQuedadas);
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR QuedadaServicioImpl - obtenerQuedadasDelUsuario()] - Error de argumento nulo al obtener las quedadas del usuario: {e}");
                return null;
            }

        }

        public string cancelarQuedada(long idQuedada)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método cancelarQuedada() de la clase QuedadaServicioImpl");

                Quedada? q = _contexto.Quedadas.FirstOrDefault(q => q.IdQuedada == idQuedada);
                if (q != null)
                {
                    if (q.Estado == "Completada")
                    {
                        EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cancelarQuedada() de la clase QuedadaServicioImpl. No se canceló la quedada, estaba completada.");
                        return "Quedada completada";
                    }
                    else if (_contexto.Participantes.Any(p => p.IdQuedada == idQuedada))
                    {
                        EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cancelarQuedada() de la clase QuedadaServicioImpl. No se canceló la quedada, había participantes.");
                        return "Usuarios participantes";
                    }
                    else
                    {
                        q.Estado = "Cancelada";
                        _contexto.Quedadas.Update(q);
                        _contexto.SaveChanges();
                        EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método cancelarQuedada() de la clase QuedadaServicioImpl. Quedada cancelada OK");
                        return "Quedada cancelada";
                    }
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR QuedadaServicioImpl - cancelarQuedada()] - Error de persistencia al canlecar quedada: {dbe}");
                return "Error al cancelar la quedada";
            }
            return "";
        }
    }
}
