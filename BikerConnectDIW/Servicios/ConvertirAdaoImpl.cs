using BikerConnectDIW.DTO;
using DAL.Entidades;

namespace BikerConnectDIW.Servicios
{
    public class ConvertirAdaoImpl : IConvertirAdao
    {

        public Usuario usuarioToDao(UsuarioDTO usuarioDTO)
        {
            Usuario usuarioDao = new Usuario();

            try
            {
                usuarioDao.IdUsuario = usuarioDTO.Id;
                usuarioDao.NombreApellidos = $"{usuarioDTO.NombreUsuario} {usuarioDTO.ApellidosUsuario}";
                usuarioDao.Email = usuarioDTO.EmailUsuario;
                usuarioDao.Contraseña = usuarioDTO.ClaveUsuario;
                usuarioDao.TlfMovil = usuarioDTO.TlfUsuario;

                return usuarioDao;
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"\n[ERROR UsuarioToDaoImpl - UsuarioToDao()] - Al convertir usuarioDTO a usuarioDAO (return null): {e}");
                return null;
            }
        }

        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO)
        {
            List<Usuario> listaUsuarioDao = new List<Usuario>();

            try
            {
                foreach (UsuarioDTO usuarioDTO in listaUsuarioDTO)
                {
                    listaUsuarioDao.Add(usuarioToDao(usuarioDTO));
                }

                return listaUsuarioDao;
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"\n[ERROR UsuarioToDaoImpl - ListUsuarioToDao()] - Al convertir lista de usuarioDTO a lista de usuarioDAO (return null): {e}");
            }
            return null;
        }

        public Moto motoToDao(MotoDTO motoDTO)
        {
            try
            {
                Moto moto = new Moto();

                moto.Marca = motoDTO.Marca;
                moto.Modelo = motoDTO.Modelo;
                moto.Año = motoDTO.Año;
                moto.Color = motoDTO.Color;
                moto.DescModificaciones = motoDTO.DescModificaciones;   
                moto.IdUsuarioPropietario = motoDTO.IdPropietario;

                return moto;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n[ERROR MotoToDaoImpl - MotoToDao()] - Al convertir MotoDTO a DAO (return null): {e}");
            }

            return null;
        }

        public Quedada quedadaToDao(QuedadaDTO quedadaDTO)
        {
            try
            {
                Quedada q = new Quedada();

                q.Lugar = quedadaDTO.Lugar;
                q.FchHoraEncuentro = quedadaDTO.FechaHora;
                q.UsuarioOrganizador = quedadaDTO.UsuarioOrganizador;
                q.DescQuedada = quedadaDTO.Descripcion;
                q.Estado = quedadaDTO.Estado;
                q.UsuarioOrganizador = quedadaDTO.UsuarioOrganizador;

                return q;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n[ERROR QuedadaToDaoImpl - QuedadaToDao()] - Al convertir QuedadaDTO a DAO (return null): {e}");
            }

            return null;
        }


    }
}
