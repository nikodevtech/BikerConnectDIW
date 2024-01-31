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
    }
}
