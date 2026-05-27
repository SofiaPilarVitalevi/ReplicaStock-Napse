using System;
using System.Collections.Generic;
using System.IO;

namespace Negocio
{
    public class Supplier
    {
        /// <summary>
        /// Genera/Actualiza un unico proveedor en destino.
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="cardCode"></param>
        /// <returns></returns>
        public void CreateUpdateSupplierDestino()
        {
            try
            {
                Utils.Logger.EscribirLog("Se creará/actualizará un Supplier en destino a partir del Socio de Negocios.", false);



                //** Creo el XML request del supplier en formato lista para enviar.

                string XMLrequest = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ser=""http://services.business.soap.bridge.synthesis.com/"">
                                       <soapenv:Header/>
                                        <soapenv:Body>
                                            <ser:execute>   
                                                <service>supplier</service>   
                                                <request><![CDATA[   
                                                   <bridgeCoreRequest>   
                                                       <operation>createOrUpdate</operation>   
                                                       <params>   
                                                <suppliers type = ""list""><supplier type = ""bean""><code>PL-001735</code><name>PRAMPARO VICTORIA</name><address/><phone></phone><email>vickypramparo@gmail.com</email><fiscalId>23337684084</fiscalId><activeFlag>true</activeFlag><fantasyName>PRAMPARO VICTORIA</fantasyName></supplier></suppliers>  
                                                       </params>
                                                   </bridgeCoreRequest>  
                                                ]]></request>  
                                                <store>0</store>  
                                             </ser:execute>   
                                           </soapenv:Body>   
                                         </soapenv:Envelope> ";//new Utils.XMLCreator().CreateXML("supplier", listSupplier);

                //** Creo el SOAP request del supplier.
                //string SOAPRequest = Utils.SOAPSender.SOAPCreator("supplier", "createOrUpdate", XMLrequest);

                //** Envío el supplier a destino.
                Utils.SOAPSender.Enviar(XMLrequest,null,null,0);

                //Utils.Logger.EscribirLog("Se creó/actualizó el Supplier " + cardCode + " correctamente en destino.", false);
            }
            catch (Exception e)
            {
                throw new Exception("Problemas al crear/actualizar el Supplier. " + e.Message);
            }
        }
    }
}
