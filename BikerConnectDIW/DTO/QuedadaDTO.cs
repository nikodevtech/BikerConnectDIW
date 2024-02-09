namespace BikerConnectDIW.DTO
{
    /// <summary>
    /// Clase que representa un objeto quedada como DTO para almacenar los datos de quedadas y moverlo entre las distintas capas de la aplicación.
    /// </summary>
    public class QuedadaDTO
    {
        public long Id { get; set; }
        public string Lugar { get; set; }
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string UsuarioOrganizador { get; set; }

        public QuedadaDTO()
        {
        }

        // Métodos

        public override string ToString()
        {
            return $"QuedadaDTO [Id={Id}, Lugar={Lugar}, FechaHora={FechaHora}, Descripcion={Descripcion}, Estado={Estado}, UsuarioOrganizador={UsuarioOrganizador}]";
        }
    }
}
