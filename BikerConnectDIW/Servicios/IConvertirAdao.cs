using BikerConnectDIW.DTO;
using DAL.Entidades;

namespace BikerConnectDIW.Servicios
{
    public interface IConvertirAdao
    {
        public Usuario usuarioToDao(UsuarioDTO usuarioDTO);

        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO);

        public Moto motoToDao(MotoDTO motoDTO);
    }
}
