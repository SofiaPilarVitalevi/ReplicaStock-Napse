using System;
using System.IO;

namespace Utils
{
    /// <summary>
    /// Genera los logs de cada ejecución del desarrollo.
    /// </summary>
    public abstract class Logger
    {
        #region ATRIBUTOS

        //Carpeta de log dentro de los archivos compilados del directorio del proyecto.
        private static string _logPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\Log\\");
        private static string _logName = GenerarLogName();

        private const string FORMATO_LOG = ".log";
        private const string SALTO_LINEA = "\n";

        #endregion

        #region PROPIEDADES

        public static string LogPath
        {
            get { return _logPath; }
        }

        public static string LogName
        {
            get { return _logName; }
        }

        #endregion

        #region MÉTODOS PÚBLICOS

        /// <summary>
        /// Permite escribir una línea en el archivo de logs.
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="error"></param>
        public static void EscribirLog(string texto, bool error)
        {
            string tipoLog = error ? "[ERROR] " : "[INFO] ";
            string timestamp = GenerarTimestamp(Date.GetAhora());

            string lineaLog = timestamp + tipoLog + texto + SALTO_LINEA;
            string pathCompleto = _logPath + _logName;

            bool mostrarConsola = false;

            try
            {
                File.AppendAllText(pathCompleto, lineaLog);
            }
            catch (Exception ex)
            {
                texto = "[ERROR] " + ex.Source + " | " + ex.Message;
                lineaLog = timestamp + texto + SALTO_LINEA;
                mostrarConsola = true;
            }

            if (mostrarConsola)
                Console.WriteLine(lineaLog);
        }

        #endregion

        #region MÉTODOS PRIVADOS

        /// <summary>
        /// Genera el nombre del archivo de logs.
        /// </summary>
        /// <returns></returns>
        private static string GenerarLogName()
        {
            //Nombre de la solución + fecha (DDMMYYYY) + horario (hhmmss) + .log
            return AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "") + "_" + Date.GetDDMMYYYY(Date.GetAhora(), false) + "_" + Date.GetHHMMSS(Date.GetAhora(), false) + FORMATO_LOG;
        }

        /// <summary>
        /// Genera la fecha y hora que se mostrara en las líneas del log.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private static string GenerarTimestamp(DateTime fecha)
        {
            string timestamp = "";
            timestamp += "[" + Date.GetDDMMYYYY(fecha, true) + " | ";
            timestamp += Date.GetHHMMSS(fecha, true) + " ]";
            timestamp += " ";

            return timestamp;
        }

        #endregion
    }
}
