using BikerConnectDIW.DTO;
using DAL.Entidades;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace BikerConnectDIW.Servicios
{
    public class ConvertirAdtoImpl : IConvertirAdto
    {
        public UsuarioDTO usuarioToDto(Usuario u)
        {
            try
            {
                UsuarioDTO dto = new UsuarioDTO();
                string[] nombreApellidos = u.NombreApellidos.Split(' ');

                if (nombreApellidos.Length > 0)
                {
                    dto.NombreUsuario = nombreApellidos[0];

                    if (nombreApellidos.Length > 1)
                    {
                        StringBuilder apellidos = new StringBuilder();
                        for (int i = 1; i < nombreApellidos.Length; i++)
                        {
                            apellidos.Append(nombreApellidos[i]).Append(" ");
                        }
                        dto.ApellidosUsuario = apellidos.ToString().Trim();
                    }

                    dto.TlfUsuario = u.TlfMovil;
                    dto.EmailUsuario = u.Email;
                    dto.ClaveUsuario = u.Contraseña;
                    dto.Token = u.TokenRecuperacion;
                    dto.ExpiracionToken = u.FchExpiracionToken;
                    dto.Id = u.IdUsuario;
                    dto.FechaRegistro = u.FchRegistro;
                    dto.CuentaConfirmada = u.CuentaConfirmada;
                    dto.Rol = u.Rol;

                }

                return dto;
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"\n[ERROR UsuarioToDtoImpl - UsuarioToDto()] - Error al convertir usuario DAO a usuarioDTO (return null): {e}");
                return null;
            }
        }

        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario)
        {
            try
            {
                List<UsuarioDTO> listaDto = new List<UsuarioDTO>();
                foreach (Usuario u in listaUsuario)
                {
                    listaDto.Add(usuarioToDto(u));
                }
                return listaDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"\n[ERROR UsuarioToDtoImpl - ListaUsuarioToDto()] - Error al convertir lista de usuario DAO a lista de usuarioDTO (return null): {e}");
            }
            return null;
        }

        public MotoDTO motoToDto(Moto m)
        {
            MotoDTO moto = new MotoDTO();

            try
            {
                moto.Id = m.IdMoto;
                moto.Marca = m.Marca;
                moto.Modelo = m.Modelo;
                moto.Año = m.Año;
                moto.Color = m.Color;
                moto.DescModificaciones = m.DescModificaciones;
                moto.IdPropietario = (long)m.IdUsuarioPropietario;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n[ERROR MotoToDtoImpl - MotoToDto()] - Al convertir entidad Moto a DTO (return null): {e}");
                return null;
            }

            return moto;
        }

        public List<MotoDTO> listaMotosToDto(List<Moto> listaMotos)
        {
            List<MotoDTO> listaDto = new List<MotoDTO>();

            try
            {
                foreach (Moto moto in listaMotos)
                {
                    listaDto.Add(motoToDto(moto));
                }
                return listaDto;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n[ERROR MotoToDtoImpl - ListaMotosToDto()] - Al convertir lista de entidades Moto a DTO (return null): {e}");
            }
            return null;
        }

    }
}
