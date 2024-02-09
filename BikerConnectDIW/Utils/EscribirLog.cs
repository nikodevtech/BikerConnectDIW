using System;
using System.IO;

namespace BikerConnectDIW.Utils
{
    /// <summary>
    /// Clase estática para escribir en el archivo de log
    /// </summary>
    public static class EscribirLog
    {
        /// <summary>
        /// Metodo que escribe en el archivo de log
        /// </summary>
        /// <param name="mensajeLog">El mensaje a escribir en el fichero</param>
        public static void escribirEnFicheroLog(string mensajeLog)
        {
            try
            {
                // En el scope de Using los recursos utilizados se cerran automaticamente
                // Abrir el archivo de registro en modo de escritura, creándolo si no existe. 
                using (FileStream fs = new FileStream(@AppDomain.CurrentDomain.BaseDirectory + "bikerconnect.log", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    // Crear un StreamWriter para escribir en el archivo
                    using (StreamWriter m_streamWriter = new StreamWriter(fs))
                    {
                        // Mover al final del archivo
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);

                        // Reemplazar los saltos de línea en el mensaje por "|"
                        mensajeLog = mensajeLog.Replace(Environment.NewLine, " | ");
                        mensajeLog = mensajeLog.Replace("\r\n", " | ").Replace("\n", " | ").Replace("\r", " | ");

                        // Escribir la fecha y hora actual seguida del mensaje en una nueva línea
                        m_streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + mensajeLog);

                        // Limpiar el búfer y escribir los datos en el archivo
                        m_streamWriter.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error EscribirLog - escribirEnFicheroLog()] Error al escribir en el fichero log:" + e.Message);
            }
        }
    }
}
