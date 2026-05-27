using System;

namespace Utils
{
    /// <summary>
    /// Genera distintos formatos de fechas para utilizar.
    /// </summary>
    public abstract class Date
    {
        #region MÉTODOS PÚBLICOS

        /// <summary>
        /// Devuelve la fecha y hora en curso.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetAhora()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Devuelve la fecha en formato DD/MM/YYYY si conBarras = true, si no devuelve la fecha en formato DDMMYYYY
        /// </summary>
        /// <param name="conBarras"></param>
        /// <returns></returns>
        public static string GetDDMMYYYY(DateTime dtFecha, bool conBarras)
        {
            //return conBarras ? dtFecha.Day + "/" + dtFecha.Month + "/" + dtFecha.Year : dtFecha.Day.ToString() + dtFecha.Month.ToString() + dtFecha.Year.ToString();
            string formatTrue = dtFecha.ToString("dd/MM/yyyy");
            string formatFalse = dtFecha.ToString("ddMMyyyy");
            return conBarras ? formatTrue : formatFalse;
        }

        /// <summary>
        /// Devuelve la hora en formato hh:mm:ss si conPuntos = true, si no devuelve la hora en formato hhmmss.
        /// </summary>
        /// <param name="conPuntos"></param>
        /// <returns></returns>e
        public static string GetHHMMSS(DateTime dtFecha, bool conPuntos)
        {
            //return conPuntos ? dtFecha.Hour + ":" + dtFecha.Minute + ":" + dtFecha.Second : dtFecha.Hour.ToString() + dtFecha.Minute.ToString() + dtFecha.Second.ToString();
            string formatTrue = dtFecha.ToString("HH:mm:ss");
            string formatFalse = dtFecha.ToString("HHmmss");
            return conPuntos ? formatTrue : formatFalse;
        }

        /// <summary>
        /// Devuelve la fecha y hora en formato: DD/MM/YYYY hh:mm:ss.
        /// </summary>
        /// <returns></returns>
        public string getTimestampParaMensaje()
        {
            return Date.GetDDMMYYYY(GetAhora(), true) + " " + Date.GetHHMMSS(GetAhora(), true);
        }

        #endregion
    }
}
