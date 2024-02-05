using BikerConnectDIW.DTO;
using DAL.Entidades;

namespace BikerConnectDIW.Servicios
{
    public interface IConvertirAdto
    {
        public UsuarioDTO usuarioToDto(Usuario u);

        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario);

        public MotoDTO motoToDto(Moto u);

        public List<MotoDTO> listaMotosToDto(List<Moto> listaMotos);

        public List<QuedadaDTO> listaQuedadaToDto(List<Quedada> listaQuedada);

        public QuedadaDTO quedadaToDto(Quedada q);
    }
}
