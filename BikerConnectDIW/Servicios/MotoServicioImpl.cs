using BikerConnectDIW.DTO;
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
                Usuario? usuarioPropietario = _contexto.Usuarios.Find(motoDTO.IdPropietario);

                Moto moto = _convertirAdao.motoToDao(motoDTO);

                if (usuarioPropietario != null)
                {
                    moto.IdUsuarioPropietarioNavigation = usuarioPropietario;

                    _contexto.Motos.Add(moto);
                    _contexto.SaveChanges();

                    return true;
                }        

                return false;
            }
            catch (DbUpdateException dbe)
            {
                Console.WriteLine($"\n[ERROR MotoServicioImpl - RegistrarMoto()] - Error de persistencia al registrar nueva moto de un usuario: {dbe}");
                return false;
            }
        }

        public MotoDTO buscarPorId(long id)
        {
            try
            {
                Moto? moto = _contexto.Motos.Find(id);
                if (moto != null) 
                {
                    return _convertirAdto.motoToDto(moto);
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine($"\n[ERROR MotoServicioImpl - buscarPorId()] - Al buscar una moto de un usuario por su id: {e}");
            }
            return null;

        }

        public void eliminarMoto(long id)
        {
            try
            {
                Moto? moto = _contexto.Motos.Find(id);
                if (moto != null)
                {
                    _contexto.Motos.Remove(moto);
                    _contexto.SaveChanges();
                }
            }
            catch (DbUpdateException dbe)
            {
                Console.WriteLine($"[Error MotoServicioImpl - eliminarMoto()] Error de persistencia al eliminar una moto por su id: {dbe.Message}");
            }
        }

        public List<MotoDTO> obtenerMotosPorPropietarioId(long id)
        {
            try 
            {
                List<Moto> listaMotos = _contexto.Motos.Where(m => m.IdUsuarioPropietario == id).ToList();
                return _convertirAdto.listaMotosToDto(listaMotos);
            } catch (ArgumentNullException e) 
            {
                Console.WriteLine($"\n[ERROR MotoServicioImpl - obtenerMotosPorPropietarioId()] - Argumento id es NULL al obtener las motos de un usuario: {e}");
                return null;
            }
        }
    }
}
