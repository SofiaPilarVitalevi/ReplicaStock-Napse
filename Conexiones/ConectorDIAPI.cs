using SAPbobsCOM;
using System;
using System.Data;

namespace Conexiones
{
    public class ConectorDIAPI
    {
        #region ATRIBUTOS

        private readonly string server;
        private readonly string companyDB;
        private readonly BoDataServerTypes dbServerType;
        private readonly string username;
        private readonly string password;

        private Company company;
        private CompanyService companyService;

        #endregion

        /// <summary>
        /// Constructor que toma los datos de conexión desde el archivo de configuraciones.
        /// </summary>
        public ConectorDIAPI(string db)
        {
            this.server = Utils.Config.DataJSON.diApiAccess.server;
            this.username = Utils.Config.DataJSON.diApiAccess.user;
            this.password = Utils.Config.DataJSON.diApiAccess.pass;
            this.dbServerType = BoDataServerTypes.dst_HANADB;

            this.companyDB = db;

            this.company = this.GenerarCompany();
        }

        #region PROPIEDADES

        public SAPbobsCOM.Company Compañia
        {
            get { return this.company; }
        }

        public SAPbobsCOM.CompanyService ServicioCompañia
        {
            get { return this.companyService; }
        }

        public string DataBase
        {
            get { return this.companyDB; }
        }

        #endregion

        #region MÉTODOS PÚBLICOS

        /// <summary>
        /// Permite conectarse a la DI API de la sociedad.
        /// </summary>
        /// <returns></returns>
        public bool Conectar()
        {
            bool resp = false;

            this.company.Disconnect();

            var conect = this.company.Connect();

            if (conect != 0)
            {
                Utils.Logger.EscribirLog("Error al generar la conexión a DIAPI: " + this.GetErroresDiApi(), true);

                resp = false;
            }
            else
            {
                resp = true;
            }

            return resp;
        }

        /// <summary>
        /// Permite desconectarse a la DI API de la sociedad
        /// </summary>
        public void Desconectar()
        {
            bool diapiConectada = company.Connected;

            if (diapiConectada)
            {
                this.company.Disconnect();
            }
        }

        /// <summary>
        /// Permite ejecutar una query a la DI API de la sociedad.
        /// </summary>
        /// <returns></returns>
        public DataTable Consultar(string query)
        {
            try
            {
                Recordset oRecordSet = (Recordset)this.company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string.Format(query);

                oRecordSet.DoQuery(query);

                return RsTODataTablaV2(oRecordSet);
            }
            catch (Exception e)
            {
                throw new Exception("Error al consultar a la BD (" + query + "): " + this.GetErroresDiApi() + " | " + e.Message);
            }
        }

        /// <summary>
        /// Devuelve las credenciales de la conexióna la DIAPI.
        /// </summary>
        /// <returns></returns>
        public string GetCredencialesDiApi()
        {
            return "DIAPI: " + this.server + " | " + this.companyDB + " | " + this.username + " | " + this.password;
        }

        /// <summary>
        /// Devuelve errores de tipo DIAPI.
        /// </summary>
        /// <returns></returns>
        public string GetErroresDiApi()
        {
            return "(" + this.company.GetLastErrorCode() + ") - " + this.company.GetLastErrorDescription();
        }

        #endregion

        #region MÉTODOS PRIVADOS

        /// <summary>
        /// Instancia el objecto Company con los datos del archivo de configuración.
        /// </summary>
        /// <returns></returns>
        private Company GenerarCompany()
        {
            Company companyPrimitiva = new Company();

            companyPrimitiva.Server = this.server;
            companyPrimitiva.CompanyDB = this.companyDB;
            companyPrimitiva.DbServerType = this.dbServerType;
            companyPrimitiva.UserName = this.username;
            companyPrimitiva.Password = this.password;
            companyPrimitiva.UseTrusted = true;

            return companyPrimitiva;
        }

        /// <summary>
        /// Transforma el recordset de SAP en un dataTable.
        /// </summary>
        /// <param name="_rs"></param>
        /// <returns></returns>
        private DataTable RsTODataTablaV2(Recordset _rs)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < _rs.Fields.Count; i++)
                dt.Columns.Add(_rs.Fields.Item(i).Description);
            while (!_rs.EoF)
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < _rs.Fields.Count; i++)
                    row[i] = _rs.Fields.Item(i).Value;
                dt.Rows.Add(row.ItemArray);
                _rs.MoveNext();
            }

            return dt;
        }

        #endregion

    }
}
