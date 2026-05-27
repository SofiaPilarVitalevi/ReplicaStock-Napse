using Newtonsoft.Json;
using System.Dynamic;
using System.IO;

namespace Utils
{
    /// <summary>
    /// Levanta los datos en el archivo de configuracion json.
    /// </summary>
    public abstract class Config
    {
        #region ATRIBUTOS

        private static ExpandoObject _dataJSON = new ExpandoObject();

        #endregion

        #region PROPIEDADES

        public static dynamic DataJSON
        {
            get
            {
                //Archivo de configuración dentro de los archivos compilados del directorio del proyecto.
                string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\AppConfig.json");
                string json = File.ReadAllText(jsonFilePath);

                if (JsonConvert.DeserializeObject<ExpandoObject>(json) != null)
                {
                    //Convierte el texto plano del json en un objeto de tipo DataJson para tomar cada campo contenido.
                    return JsonConvert.DeserializeObject<ExpandoObject>(json);
                }
                else
                {
                    return new ExpandoObject();
                }
            }
        }

        #endregion
    }
}
