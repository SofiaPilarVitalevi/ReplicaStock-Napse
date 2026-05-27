using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Utils
{
    public class Response 
    {
        private string ack_;
        private string message_;

        public string ack
        {
            get { return ack_; }
            set { ack_ = value; }
        }

        public string message
        {
            get { return message_; }
            set { message_ = value; }
        }

        public object additional { get; set; }
    }

    public abstract class SOAPSender
    {
        #region ATRIBUTOS

        private static string url_ = Config.DataJSON.apiDest.url;
        private static string codRespNapse = "";
       
        #endregion

        #region MÉTODOS PÚBLICOS

        /// <summary>
        /// Envía una petición a url determinado.
        /// </summary>
        /// <param name="requestBody"></param>
        public static List<string> Enviar(string requestBody, List<Entidades.Stock> stockAprocesar, string objType, int etapa)
        {
            
            try
            {
                // === NUEVO CÓDIGO DE PRUEBA: Guardar a TXT en vez de enviar ===
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Request_Envio.txt", requestBody);
                //return;
                // ==============================================================

                
                //Variables
                List<string> items = new List<string>();
                IEnumerable<string> exceptitemsAprocesar;
                List<string> itemsOK = new List<string>();

                //** Configurar la solicitud HTTP
                HttpWebRequest requestHTTP = (HttpWebRequest)WebRequest.Create(url_);
                requestHTTP.Method = "POST";
                requestHTTP.ContentType = "text/xml; charset=utf-8";
                // Ajustar tiempos de espera
                requestHTTP.Timeout = 600000 * 6;// Tiempo total para la operación (en milisegundos)
                requestHTTP.ReadWriteTimeout = 600000 * 6;

                requestHTTP.Headers.Add("SOAPAction", "");

                //** Enviar la solicitud
                using (StreamWriter writer = new StreamWriter(requestHTTP.GetRequestStream()))
                {
                    writer.Write(requestBody);
                }

                //** Obtener la respuesta
                using (HttpWebResponse responseHTTP = (HttpWebResponse)requestHTTP.GetResponse())
                {              
                    using (StreamReader reader = new StreamReader(responseHTTP.GetResponseStream()))
                    {
                        //** Obtener la cadena de la respuesta
                        string response = reader.ReadToEnd();

                        //** Decodificar de formato HTML a string
                        string respDecode = HttpUtility.HtmlDecode(response);

                        //** Decodificar el string para saber si el envío fue satisfactorio                       
                        return DecodeResponse(respDecode,objType,etapa, stockAprocesar);


                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Problemas al enviar el documento a destino: " + e.Message);
            }
        }

        /// <summary>
        /// Genera el request SOAP a partir del xml con los datos a enviar.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="XMLrequest"></param>
        /// <returns></returns>
        public static string SOAPCreator(string service, string method, string XMLrequest)
        {
            string SOAPRequest = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ser=""http://services.business.soap.bridge.synthesis.com/"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <ser:execute>
                                             <service>" + service + @"</service>
                                             <request><![CDATA[
                                                <bridgeCoreRequest>
                                                    <operation>" + method + @"</operation>
                                                    <params>
                                             " + XMLrequest + @"
                                                    </params>
                                                </bridgeCoreRequest>
                                             ]]></request>
                                             <store>0</store>
                                          </ser:execute>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";

           Logger.EscribirLog("Request a enviar: " + SOAPRequest, false);

            return SOAPRequest;
        }

        /// <summary>
        /// Decodifica la respuesta diferenciando aquellas satisfactorias de las erróneas.
        /// </summary>
        /// <param name="resp"></param>
        private static List<string> DecodeResponse(string resp, string objType, int etapa, List<Entidades.Stock> stockAprocesar)
        {
            //** Conexión con MSSQL.
            //string dbMSSQL = Utils.Config.DataJSON.dbs.a;
            //Conexiones.ConectorMSSQL socMSSQL = new Conexiones.ConectorMSSQL(dbMSSQL);


            string codeResp_ = string.Empty;
            string msgResp_ = string.Empty;

            //** Tomar las etiquetas de preferencia para buscar sus valores en la respuesta
            string patternAck = @"<ack>(.*?)</ack>";
            Match matchAck = Regex.Match(resp, patternAck);

            string patternMsg = @"<message>(.*?)</message>";
            Match matchMsg = Regex.Match(resp, patternMsg);

            string additional = @"<additional>(.*?)</additional>";
            Match matchAdd = Regex.Match(resp, additional);

            if (matchAck.Success && matchMsg.Success)
            {
                // Decodificar el contenido HTML
                codeResp_ = matchAck.Groups[1].Value;
                msgResp_ = matchMsg.Groups[1].Value;
                var jsonResp = JsonConvert.DeserializeObject<List<Response>>(msgResp_);
                List<string> items = new List<string>();
                List<string> ListItemErr = new List<string>();

                codRespNapse = codeResp_;

                foreach (var json in jsonResp)
                {
                    Utils.Logger.EscribirLog(json.message, false);
                    
                    if (json.additional != null)
                    {
                        if (json.additional is Newtonsoft.Json.Linq.JArray arr)
                        {
                            foreach (var addMsgToken in arr)
                            {
                                string addMsg = addMsgToken.ToString();
                                // Buscar la palabra después de "codigo"
                                Match matchCode = Regex.Match(addMsg, @"codigo ([\w\-]+)");
                                if (matchCode.Success)
                                {
                                    string itemCode = matchCode.Groups[1].Value;
                                    if (!ListItemErr.Contains(itemCode))
                                    {
                                        ListItemErr.Add(itemCode);
                                        Utils.Logger.EscribirLog("Error en ítem código: " + itemCode + " - Detalle: " + addMsg, false);
                                    }
                                }
                                else
                                {
                                    Utils.Logger.EscribirLog("Detalle adicional: " + addMsg, false);
                                }
                            }
                        }
                        else
                        {
                            // En caso de que sea un string simple (como un MongoError devuelto por la API)
                            string errorMsg = json.additional.ToString();
                            //Utils.Logger.EscribirLog("Error general de API en additional: " + errorMsg, false);
                            throw new Exception("Error general en Napse: " + errorMsg);
                        }
                    }

                }

                return ListItemErr;
            }
            else
            {
                throw new Exception("Error al decodificar la respuesta.");
            }
            
        }
        #endregion
        private static string getListaItemsError(List<string> listItemErr)
        {
            string itemsErr = "";
            string[] array = listItemErr.ToArray(); // Convertir la lista a un array

            foreach (var item in listItemErr)
            {
                itemsErr += "'" + item + "',";
            }

            itemsErr = itemsErr.Replace(" ","");

            return itemsErr.TrimEnd(',');
        }
       
    }
}
