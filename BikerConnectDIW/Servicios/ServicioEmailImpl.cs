using BikerConnectDIW.Utils;
using System.Net.Mail;

namespace BikerConnectDIW.Servicios
{
    /// <summary>
    /// Clase que implementa la interfaz IServicioEmail y detalla la lógica de los metodos que serán necesarios para el envío de correos electrónicos
    /// </summary>
    public class ServicioEmailImpl : IServicioEmail
    {
        void IServicioEmail.enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método enviarEmailConfirmacion() de la clase ServicioEmailImpl");

                string urlDominio = "https://localhost:7142";

                string EmailOrigen = "nikoalvarezzapata@gmail.com";
                //Se crea la URL de recuperación con el token que se enviará al mail del user.
                string urlDeRecuperacion = String.Format("{0}/auth/confirmar-cuenta/?token={1}", urlDominio, token);

                //Hacemos que el texto del email sea un archivo html que se encuentra en la carpeta Plantilla.
                string directorioProyecto = System.IO.Directory.GetCurrentDirectory();
                string rutaArchivo = System.IO.Path.Combine(directorioProyecto, "Plantilla/ConfirmarCorreo.html");
                string htmlContent = System.IO.File.ReadAllText(rutaArchivo);
                //Asignamos el nombre de usuario que tendrá el cuerpo del mail y el URL de recuperación con el token al HTML.
                htmlContent = String.Format(htmlContent, nombreUsuario, urlDeRecuperacion);

                MailMessage mensajeDelCorreo = new MailMessage(EmailOrigen, emailDestino, "CONFIRMAR EMAIL BIKERCONNECT", htmlContent);

                mensajeDelCorreo.IsBodyHtml = true;

                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, "");

                smtpCliente.Send(mensajeDelCorreo);

                smtpCliente.Dispose();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método enviarEmailConfirmacion() de la clase ServicioEmailImpl");

            }
            catch (IOException ioe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEmailImpl - enviarEmailConfirmacion()] Error al leer el fichero html para enviar email de confirmacion: " + ioe.Message);
            }
            catch (SmtpException se)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEmailImpl - enviarEmailConfirmacion()] Error con el protocolo de envio de email: " + se.Message);
            }

        }

        void IServicioEmail.enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método enviarEmailRecuperacion() de la clase ServicioEmailImpl");

                string urlDominio = "https://localhost:7142";

                string EmailOrigen = "nikoalvarezzapata@gmail.com";
                //Se crea la URL de recuperación con el token que se enviará al mail del user.
                string urlDeRecuperacion = String.Format("{0}/auth/recuperar/?token={1}", urlDominio, token);

                //Hacemos que el texto del email sea un archivo html que se encuentra en la carpeta Plantilla.
                string directorioProyecto = System.IO.Directory.GetCurrentDirectory();
                string rutaArchivo = System.IO.Path.Combine(directorioProyecto, "Plantilla/RecuperacionContraseña.html");
                string htmlContent = System.IO.File.ReadAllText(rutaArchivo);
                //Asignamos el nombre de usuario que tendrá el cuerpo del mail y el URL de recuperación con el token al HTML.
                htmlContent = String.Format(htmlContent, nombreUsuario, urlDeRecuperacion);

                MailMessage mensajeDelCorreo = new MailMessage(EmailOrigen, emailDestino, "RESTABLECER CONTRASEÑA BIKERCONNECT", htmlContent);

                mensajeDelCorreo.IsBodyHtml = true;

                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, "");

                smtpCliente.Send(mensajeDelCorreo);

                smtpCliente.Dispose();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método enviarEmailRecuperacion() de la clase ServicioEmailImpl");

            }
            catch (IOException ioe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEmailImpl - enviarEmailRecuperacion()] Error al leer el fichero html para enviar email de recuperacion: " + ioe.Message);
            }
            catch (SmtpException se)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEmailImpl - enviarEmailRecuperacion()] Error con el protocolo de envio de email: " + se.Message);
            }

        }
    }
}
