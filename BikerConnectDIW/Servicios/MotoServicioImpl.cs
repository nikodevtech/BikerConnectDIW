using BikerConnectDIW.DTO;
using DAL.Entidades;

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
                // Buscar el usuario propietario por su Id
                Usuario? usuarioPropietario = _contexto.Usuarios.Find(motoDTO.IdPropietario);

                // Convertir el DTO a entidad Moto
                Moto moto = _convertirAdao.motoToDao(motoDTO);

                // Establecer la relación con el usuario propietario
                if (usuarioPropietario != null)
                {
                    moto.IdUsuarioPropietarioNavigation = usuarioPropietario;

                    // Guardar la nueva moto en el repositorio
                    _contexto.Motos.Add(moto);
                    _contexto.SaveChanges();

                    return true;
                }        

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n[ERROR MotoServicioImpl - RegistrarMoto()] - Al registrar nueva moto de un usuario: {e}");
                return false;
            }
        }

        public MotoDTO buscarPorId(long id)
        {
            throw new NotImplementedException();
        }

        public void eliminarMoto(long id)
        {
            throw new NotImplementedException();
        }

        public List<MotoDTO> obtenerMotosPorPropietarioId(long id)
        {
            try 
            {
                List<Moto> listaMotos = _contexto.Motos.Where(m => m.IdUsuarioPropietario == id).ToList();
                return _convertirAdto.listaMotosToDto(listaMotos);
            } catch (Exception e) 
            {
                Console.WriteLine($"\n[ERROR MotoServicioImpl - RegistrarMoto()] - Al registrar nueva moto de un usuario: {e}");
                return null;
            }
        }
    }
}
