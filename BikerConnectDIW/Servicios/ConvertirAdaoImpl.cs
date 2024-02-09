using BikerConnectDIW.DTO;
using BikerConnectDIW.Utils;
using DAL.Entidades;

namespace BikerConnectDIW.Servicios
{
    /// <summary>
    /// Clase que implementa la interfaz IConvertirAdao y detalla la lógica de los metodos que serán necesarios para el paso de DTO a DAO
    /// </summary>
    public class ConvertirAdaoImpl : IConvertirAdao
    {

        public Usuario usuarioToDao(UsuarioDTO usuarioDTO)
        {

            try
            {
                Usuario usuarioDao = new Usuario();

                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método usuarioToDao() de la clase ConvertirAdaoImpl");

                usuarioDao.IdUsuario = usuarioDTO.Id;
                usuarioDao.NombreApellidos = $"{usuarioDTO.NombreUsuario} {usuarioDTO.ApellidosUsuario}";
                usuarioDao.Email = usuarioDTO.EmailUsuario;
                usuarioDao.Contraseña = usuarioDTO.ClaveUsuario;
                usuarioDao.TlfMovil = usuarioDTO.TlfUsuario;
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método usuarioToDao() de la clase ConvertirAdaoImpl");

                return usuarioDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - UsuarioToDao()] - Al convertir usuarioDTO a usuarioDAO (return null): {e}");
                return null;
            }
        }

        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO)
        {

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listUsuarioToDao() de la clase ConvertirAdaoImpl");

                List<Usuario> listaUsuarioDao = new List<Usuario>();
                foreach (UsuarioDTO usuarioDTO in listaUsuarioDTO)
                {
                    listaUsuarioDao.Add(usuarioToDao(usuarioDTO));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listUsuarioToDao() de la clase ConvertirAdaoImpl");

                return listaUsuarioDao;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - listUsuarioToDao()] - Al convertir lista de usuarioDTO a lista de usuarioDAO (return null): {e}");
            }
            return null;
        }

        public Moto motoToDao(MotoDTO motoDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listUsuarioToDao() de la clase ConvertirAdaoImpl");

                Moto moto = new Moto();

                moto.Marca = motoDTO.Marca;
                moto.Modelo = motoDTO.Modelo;
                moto.Año = motoDTO.Año;
                moto.Color = motoDTO.Color;
                moto.DescModificaciones = motoDTO.DescModificaciones;
                moto.IdUsuarioPropietario = motoDTO.IdPropietario;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método motoToDao() de la clase ConvertirAdaoImpl");
                return moto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - motoToDao()] - Al convertir MotoDTO a DAO (return null): {e}");
            }

            return null;
        }

        public Quedada quedadaToDao(QuedadaDTO quedadaDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método  quedadaToDao() de la clase ConvertirAdaoImpl");

                Quedada q = new Quedada();

                q.Lugar = quedadaDTO.Lugar;
                q.FchHoraEncuentro = quedadaDTO.FechaHora;
                q.UsuarioOrganizador = quedadaDTO.UsuarioOrganizador;
                q.DescQuedada = quedadaDTO.Descripcion;
                q.Estado = quedadaDTO.Estado;
                q.UsuarioOrganizador = quedadaDTO.UsuarioOrganizador;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método quedadaToDao() de la clase ConvertirAdaoImpl");
                return q;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"\n[ERROR ConvertirAdaoImpl - quedadaToDao()] - Al convertir QuedadaDTO a DAO (return null): {e}");
            }

            return null;
        }


    }
}
