using BikerConnectDIW.DTO;
using DAL.Entidades;
using Poyecto_Gestor_Biblioteca_Web_Los_Rapidos.Servicios;

namespace BikerConnectDIW.Servicios
{
    public class UsuarioServicioImpl : IUsuarioServicio
    {
        private readonly BikerconnectContext _contexto;
        private readonly IServicioEncriptar _servicioEncriptar;

        public UsuarioServicioImpl(BikerconnectContext contexto, IServicioEncriptar servicioEncriptar)
        {
            _contexto = contexto;
            _servicioEncriptar = servicioEncriptar;
        }
        UsuarioDTO IUsuarioServicio.registrarUsuario(UsuarioDTO userDTO)
        {
            var Usuario = _contexto.Usuarios.FirstOrDefault(u => u.Email == userDTO.EmailUsuario);

            if (Usuario != null)
            {
                return null;
            }
            else
            {
                //_contexto.Usuarios.Add();
                //_contexto.SaveChanges();
                return userDTO;
            }

        }
    }
}
