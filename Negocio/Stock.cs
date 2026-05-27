using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using Utils;
using System.Runtime.InteropServices;

namespace Negocio
{
    public class Stock
    {
        Entidades.SapCompanyConfig conexion = new Entidades.SapCompanyConfig();
        

        public void CreateUpdatereplicaStockDestino()
        {
            //** Creo el request del item en formato lista para enviar.
            List<Entidades.Stock> listStock = new List<Entidades.Stock>();
            
            Company oCompany = conexion.DatosSAP_BD();
            string statusMessage = "Iniciado";

            try
            {
               
                int retVal = oCompany.Connect();
                if (retVal == 0)
                {

                    SBObob oSBObob = (SBObob)oCompany.GetBusinessObject(BoObjectTypes.BoBridge);
                    Recordset oRecordset = null;
                    oRecordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                    if (oRecordset != null)
                    {
                        var i = 0;
                        string almacen = "1200";
                        while (i < 2)
                        {
                            if (i == 1)
                            {
                                almacen = "1201";
                            }
                            oRecordset.DoQuery("SELECT DISTINCT \"ItemCode\", \"Total\"  FROM \"ST_NOVEDADES_STOCK\" WHERE \"Leido\" = 'N' AND \"Almacen\"='"+ almacen + "'");
                            if(oRecordset.RecordCount == 0)
                            {
                                Utils.Logger.EscribirLog($"No se encontraron registros en ST_NOVEDADES_STOCK.", false);
                                return;
                            }
                            
                            string itemCode = "";
                            decimal total = 0;

                            Entidades.Stock stockDestino = new Entidades.Stock();
                            stockDestino.storeCode = "1327";
                            stockDestino.locationCode = almacen;
                            stockDestino.itemInventoryState = "IISD";
                            stockDestino.revenueCenter = "RCD";
                            stockDestino.item = new List<Entidades.Item>(); // inicializada aquí

                            while (!oRecordset.EoF)
                            {
                                itemCode = oRecordset.Fields.Item("ItemCode").Value?.ToString() ?? "";
                                decimal.TryParse(oRecordset.Fields.Item("Total").Value?.ToString(), out total);

                                stockDestino.item.Add(new Entidades.Item()
                                {
                                    itemCode = itemCode,
                                    stockUnits = total
                                });

                                oRecordset.MoveNext();
                            }

                            Utils.Logger.EscribirLog($"Se encontraron {stockDestino.item.Count} registros en ST_NOVEDADES_STOCK.", false);

                            if (stockDestino.item.Count > 0)
                            {
                                listStock.Add(stockDestino);

                            }
                            i += 1;

                        }
                        string XMLrequest = new Utils.XMLCreator().CreateXML("stock", listStock);

                        //** Creo el SOAP request del item.
                        string SOAPRequest = Utils.SOAPSender.SOAPCreator("stock", "createOrUpdate", XMLrequest);

                        //** Envío los item a destino. Retorna la lista de errores.
                        List<string> errores = Utils.SOAPSender.Enviar(SOAPRequest, listStock, "4", 0);

                        //** Actualizo el estado de cada ítem enviado
                        //foreach (var itm in stockDestino.item)
                        
                        if (errores != null && errores.Count() > 0)
                        {
                            int e = 0;
                            foreach (var itm in listStock[e].item)
                            {
                                string estado = errores.Contains(itm.itemCode) ? "N" : "Y";
                                oRecordset.DoQuery($"CALL \"SP_NOVEDADES_STOCK\" ('{itm.itemCode}', '{estado}')");
                                e++;
                            }
                        }
                        else
                        {
                            //POngo todo en Y
                            int e = 0;
                            foreach (var itm in listStock[e].item)
                            {
                                oRecordset.DoQuery($"CALL \"SP_NOVEDADES_STOCK\" ('{itm.itemCode}', 'Y')");
                                e++;
                            }
                        }
                        


                    }
                }
                else
                {
                    string error = oCompany.GetLastErrorCode() + " -- " + oCompany.GetLastErrorDescription();
                    Utils.Logger.EscribirLog($"Error de conexión a SAP: {error}", false);
                }
            }
            catch (Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
                Utils.Logger.EscribirLog($"Excepción al procesar: {ex.Message}", false);
            }
            finally
            {
                if (oCompany.Connected) oCompany.Disconnect();
            }

        }
        
    }
}
