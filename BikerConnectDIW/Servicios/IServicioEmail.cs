namespace BikerConnectDIW.Servicios
{
    public interface IServicioEmail
    {
        public void enviarEmailRecuperacion(String emailDestino, String nombreUsuario, String token);

        void enviarEmailConfirmacion(String emailDestino, String nombreUsuario, String token);


    }
}
