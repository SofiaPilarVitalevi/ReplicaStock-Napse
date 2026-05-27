using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ReplicaStock.ServiceReference1;
using Utils;


namespace ReplicaStock
{
    class Program
    {
        //static void Main(string[] args)
        static async Task Main(string[] args)
        {
            new Negocio.Stock().CreateUpdatereplicaStockDestino();
            //new Negocio.Supplier().CreateUpdateSupplierDestino();
            // Usás tu helper para convertir el objeto a XML
            //string requestXml = XmlHelper.SerializeToXml(stock);

            //var binding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.Transport);
            //var endpoint = new System.ServiceModel.EndpointAddress("https://bmc.dev.napse.global:45503/bridge/services/bridgeCoreSOAP");

            //var client = new BridgeCoreSOAPServiceClient(binding, endpoint);

            //string response = client.execute("stock", requestXml, "Store01"); Console.WriteLine(response);
            //Console.ReadLine();
            //client.Close();

        }
        static string BuildStockRequest(string storeCode, string locationCode, string inventoryState, string revenueCenter, List<(string itemCode, decimal stockUnits)> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<stocks>");
            sb.AppendLine("  <stock>");
            sb.AppendLine($"    <storeCode>{storeCode}</storeCode>");
            sb.AppendLine($"    <locationCode>{locationCode}</locationCode>");
            sb.AppendLine($"    <itemInventoryState>{inventoryState}</itemInventoryState>");
            sb.AppendLine($"    <revenueCenter>{revenueCenter}</revenueCenter>");
            sb.AppendLine("    <items>");

            foreach (var item in items)
            {
                sb.AppendLine("      <item>");
                sb.AppendLine($"        <itemCode>{item.itemCode}</itemCode>");
                sb.AppendLine($"        <stockUnits>{item.stockUnits}</stockUnits>");
                sb.AppendLine("      </item>");
            }

            sb.AppendLine("    </items>");
            sb.AppendLine("  </stock>");
            sb.AppendLine("</stocks>");

            return sb.ToString();
        }
    }
}
