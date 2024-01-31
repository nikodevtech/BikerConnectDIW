namespace BikerConnectDIW.Servicios
{
    public interface IServicioEncriptar
    {
        /// <summary>
        /// Interfaz del metodo encargado de encriptar 
        /// </summary>
        /// <param name="texto">String el cual se quiere encriptar</param>
        /// <returns>String encriptado</returns>
        public string Encriptar(string texto);
    }
}
