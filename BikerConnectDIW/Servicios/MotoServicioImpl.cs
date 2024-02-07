using BikerConnectDIW.DTO;
using BikerConnectDIW.Utils;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BikerConnectDIW.Servicios
{
    public class MotoServicioImpl : IMotoServicio
    {
        private readonly BikerconnectContext _contexto;
        private readonly IConvertirAdao _convertirAdao;
        private readonly IConvertirAdto _convertirAdto;

        public MotoServicioImpl(
            BikerconnectContext contexto,
            IConvertirAdao convertirAdao,
            IConvertirAdto convertirAdto
            )
        {
            _contexto = contexto;
            _convertirAdao = convertirAdao;
            _convertirAdto = convertirAdto;
        }

        public bool registrarMoto(MotoDTO motoDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrarMoto() de la clase MotoServicioImpl");

                Usuario? usuarioPropietario = _contexto.Usuarios.Find(motoDTO.IdPropietario);

                Moto moto = _convertirAdao.motoToDao(motoDTO);

                if (usuarioPropietario != null)
                {
                    moto.IdUsuarioPropietarioNavigation = usuarioPropietario;

                    _contexto.Motos.Add(moto);
                    _contexto.SaveChanges();

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarMoto() de la clase MotoServicioImpl. Nueva moto registrada del usuario " + usuarioPropietario.Email);
                    return true;
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarMoto() de la clase MotoServicioImpl .No se encontró al usuario y no se pudo registrar la moto");
                return false;
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR MotoServicioImpl - RegistrarMoto()] - Error de persistencia al registrar nueva moto de un usuario: {dbe}");
                return false;
            }
        }

        public MotoDTO buscarPorId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorId() de la clase MotoServicioImpl");

                Moto? moto = _contexto.Motos.Find(id);
                if (moto != null)
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método buscarPorId() de la clase MotoServicioImpl");
                    return _convertirAdto.motoToDto(moto);
                }
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR MotoServicioImpl - buscarPorId()] - Al buscar una moto de un usuario por su id: {e}");
            }
            return null;

        }

        public void eliminarMoto(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método eliminarMoto() de la clase MotoServicioImpl");

                Moto? moto = _contexto.Motos.Find(id);
                if (moto != null)
                {
                    _contexto.Motos.Remove(moto);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminarMoto() de la clase MotoServicioImpl. Moto eliminada correctamente.");
                }
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog($"[Error MotoServicioImpl - eliminarMoto()] Error de persistencia al eliminar una moto por su id: {dbe.Message}");
            }
        }

        public List<MotoDTO> obtenerMotosPorPropietarioId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerMotosPorPropietarioId() de la clase MotoServicioImpl");

                List<Moto> listaMotos = _contexto.Motos.Where(m => m.IdUsuarioPropietario == id).ToList();
                return _convertirAdto.listaMotosToDto(listaMotos);
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR MotoServicioImpl - obtenerMotosPorPropietarioId()] - Argumento id es NULL al obtener las motos de un usuario: {e}");
                return null;
            }
        }
    }
}
