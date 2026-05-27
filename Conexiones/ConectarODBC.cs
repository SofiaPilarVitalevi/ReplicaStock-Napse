using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

public class ConectarODBC
{
    private OdbcConnection odbcConnection;
    private string connector = Utils.Config.DataJSON.HanaConnection.Driver;
    private string userbase = Utils.Config.DataJSON.HanaConnection.UID;
    private string passbase = Utils.Config.DataJSON.HanaConnection.PWD;
    private string server09 = Utils.Config.DataJSON.HanaConnection.SERVERNODE;
    private string database = Utils.Config.DataJSON.HanaConnection.DATABASE;
    public ConectarODBC()
    {
        odbcConnection = new OdbcConnection();
    }

    public OdbcConnection ConnectToODBC()
    {
        //string connectionString = "Driver={SAP HANA ODBC Driver};ServerNode=NombreDelServidor:30015;UID=UsuarioBD;PWD=ContraseñaBD;";
        //odbcConnection.ConnectionString = connectionString;
        string odbcstr = "Driver=" + connector + ";UID=" + userbase + ";PWD=" + passbase + ";SERVERNODE=" + server09 + ";DATABASE=" + database;
        odbcConnection = new OdbcConnection(odbcstr);

        try
        {
            odbcConnection.Open();
            //Console.WriteLine("Conexión exitosa a SAP Business One mediante ODBC.");
        }
        catch (Exception ex)
        {

            throw new Exception("Error al conectar ODBC . " + ex.Message);
        }

        return odbcConnection;
    }
}



