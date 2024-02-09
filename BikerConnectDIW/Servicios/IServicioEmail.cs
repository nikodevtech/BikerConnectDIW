namespace BikerConnectDIW.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos necesarios para enviar correos electrónicos relacionados con la recuperación de contraseña y la confirmación de cuentas.
    /// </summary>
    public interface IServicioEmail
    {
        /// <summary>
        /// Envía un correo electrónico de recuperación de contraseña.
        /// </summary>
        /// <param name="emailDestino">Dirección de correo electrónico del destinatario.</param>
        /// <param name="nombreUsuario">Nombre del usuario.</param>
        /// <param name="token">Token generado para la recuperación de contraseña.</param>
        public void enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token);

        /// <summary>
        /// Envía un correo electrónico de confirmación de cuenta.
        /// </summary>
        /// <param name="emailDestino">Dirección de correo electrónico del destinatario.</param>
        /// <param name="nombreUsuario">Nombre del usuario.</param>
        /// <param name="token">Token generado para la confirmación de cuenta.</param>
        void enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token);
    }
}
