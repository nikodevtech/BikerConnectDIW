namespace BikerConnectDIW.DTO
{
    public class UsuarioDTO
    {
        // ATRIBUTOS
        public long Id { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidosUsuario { get; set; }
        public string TlfUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
        public DateTime? ExpiracionToken { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool CuentaConfirmada { get; set; }
        public string Rol { get; set; }
        public List<MotoDTO> MisMotos { get; set; } = new List<MotoDTO>();
        public List<QuedadaDTO> MisQuedadas { get; set; } = new List<QuedadaDTO>();

        // CONSTRUCTORES
        public UsuarioDTO()
        {
        }

        public UsuarioDTO(string nombreUsuario, string apellidosUsuario, string tlfUsuario, string emailUsuario,
            string claveUsuario)
        {
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            TlfUsuario = tlfUsuario;
            EmailUsuario = emailUsuario;
            ClaveUsuario = claveUsuario;
        }

        // METODOS

        public override string ToString()
        {
            return $"UsuarioDTO [Id={Id}, NombreUsuario={NombreUsuario}, ApellidosUsuario={ApellidosUsuario}, " +
                   $"TlfUsuario={TlfUsuario}, EmailUsuario={EmailUsuario}, ClaveUsuario={ClaveUsuario}, " +
                   $"Token={Token}, Password={Password}, Password2={Password2}, ExpiracionToken={ExpiracionToken}, " +
                   $"FechaRegistro={FechaRegistro}, CuentaConfirmada={CuentaConfirmada}, Rol={Rol}, " /*+
                   $"MisMotos={MisMotos}, MisQuedadas={MisQuedadas}]"*/;
        }
    }
}
