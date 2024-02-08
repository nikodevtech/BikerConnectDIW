using BikerConnectDIW.DTO;
using BikerConnectDIW.Utils;
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
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método usuarioToDto() de la clase ConvertirAdtoImpl");

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

                    if (u.Foto != null)
                    {
                        dto.Foto = u.Foto;
                    }

                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método usuarioToDto() de la clase ConvertirAdtoImpl");
                return dto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - usuarioToDto()] - Error al convertir usuario DAO a usuarioDTO (return null): {e}");
                return null;
            }
        }

        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaUsuarioToDto() de la clase ConvertirAdtoImpl");

                List<UsuarioDTO> listaDto = new List<UsuarioDTO>();
                foreach (Usuario u in listaUsuario)
                {
                    listaDto.Add(usuarioToDto(u));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaUsuarioToDto() de la clase ConvertirAdtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - listaUsuarioToDto()] - Error al convertir lista de usuario DAO a lista de usuarioDTO (return null): {e}");
            }
            return null;
        }

        public MotoDTO motoToDto(Moto m)
        {
            MotoDTO moto = new MotoDTO();

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método motoToDto() de la clase ConvertirAdtoImpl");

                moto.Id = m.IdMoto;
                moto.Marca = m.Marca;
                moto.Modelo = m.Modelo;
                moto.Año = m.Año;
                moto.Color = m.Color;
                moto.DescModificaciones = m.DescModificaciones;
                moto.IdPropietario = (long)m.IdUsuarioPropietario;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método motoToDto() de la clase ConvertirAdtoImpl");
                return moto;

            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - motoToDto()] - Al convertir entidad Moto a DTO (return null): {e}");
                return null;
            }
        }

        public List<MotoDTO> listaMotosToDto(List<Moto> listaMotos)
        {
            List<MotoDTO> listaDto = new List<MotoDTO>();

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método listaMotosToDto() de la clase ConvertirAdtoImpl");

                foreach (Moto moto in listaMotos)
                {
                    listaDto.Add(motoToDto(moto));
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaMotosToDto() de la clase ConvertirAdtoImpl");
                return listaDto;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - listaMotosToDto()] - Al convertir lista de entidades Moto a DTO (return null): {e}");
            }
            return null;
        }

        public QuedadaDTO quedadaToDto(Quedada q)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método quedadaToDto() de la clase ConvertirAdtoImpl");

                QuedadaDTO quedadaDTO = new QuedadaDTO();

                quedadaDTO.Lugar = q.Lugar;
                quedadaDTO.FechaHora = q.FchHoraEncuentro;
                quedadaDTO.Id = q.IdQuedada;
                quedadaDTO.Descripcion = q.DescQuedada;
                quedadaDTO.UsuarioOrganizador = q.UsuarioOrganizador;
                quedadaDTO.Estado = q.Estado;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método quedadaToDto() de la clase ConvertirAdtoImpl");
                return quedadaDTO;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - quedadaToDto()] - Al convertir entidad Quedada a DTO (return null): {e}");
                return null;
            }
        }

        public List<QuedadaDTO> listaQuedadaToDto(List<Quedada> listaQuedadas)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método quedadaToDto() de la clase ConvertirAdtoImpl");

                List<QuedadaDTO> listaQuedadasDTO = new List<QuedadaDTO>();

                foreach (var q in listaQuedadas)
                {
                    var quedadaDto = quedadaToDto(q);
                    if (quedadaDto != null)
                    {
                        listaQuedadasDTO.Add(quedadaDto);
                    }
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método listaQuedadaToDto() de la clase ConvertirAdtoImpl");
                return listaQuedadasDTO;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ConvertirAdtoImpl - ListaQuedadToDto()] - Al convertir lista Quedada a DTO (return null): {e}");
                return null;
            }
        }

    }
}
