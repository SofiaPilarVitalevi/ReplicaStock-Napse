using System;
using System.Data;
using System.Data.SqlClient;

namespace Conexiones
{
    public class ConectorMSSQL
    {
        #region ATRIBUTOS

        /// <summary>
        /// Datos necesarios para la conexión Microsoft Server SQL a la BD.
        /// </summary>
        private readonly string server;
        private readonly string user;
        private readonly string pass;
        private readonly string database;
        private readonly string connectionString;

        private SqlConnection conector;

        #endregion
       
        /// <summary>
        /// Constructor: Instancia los valores de las variables locales de la clase.
        /// </summary>
        public ConectorMSSQL(string db)
        {
            this.server = Utils.Config.DataJSON.mssqlAccess.server;
            this.user = Utils.Config.DataJSON.mssqlAccess.user;
            this.pass = Utils.Config.DataJSON.mssqlAccess.pass;
            this.database = db;

            this.connectionString = this.GenerarConnectionString();
            this.conector = new SqlConnection(this.connectionString);
        }

        #region PROPIEDADES

        public string DataBase
        {
            get { return this.database; }
        }

        public SqlConnection Conexion
        {
            get { return this.conector; }
        }

        public string CadenaConexion
        {
            get { return this.connectionString; }
        }

        #endregion

        #region MÉTODOS PÚBLICOS

        /// <summary>
        /// Permite conectarse a la BD de la sociedad.
        /// </summary>
        public bool Conectar()
        {
            bool resp = false;

            try
            {
                this.Conexion.Open();

                resp = true;
            }
            catch (Exception e)
            {
                Utils.Logger.EscribirLog("Error al generar la conexión MSSQL: " + e.Message, true);

                resp = false;
            }

            return resp;
        }

        /// <summary>
        /// Permite desconectarse a la BD de la sociedad.
        /// </summary>
        public void Desconectar()
        {
            ConnectionState estado = this.Conexion.State;

            if (estado != (int)ConnectionState.Closed)
            {
                this.Conexion.Close();
            }
        }

        /// <summary>
        /// Permite realizar consultas a la BD.
        /// </summary>
        /// <param name="comando"></param>
        /// <param name="isSP"></param>
        /// <param name="parametrosOdbc"></param>
        /// <returns></returns>
        public DataTable Consultar(string comando, bool isSP, SqlParameter[] parametrosOdbc = null)
        {
            var unaTabla = new DataTable();
            var objComando = new SqlCommand();

            try
            {
                if (isSP)
                {
                    objComando.CommandText = comando;
                    objComando.CommandType = CommandType.StoredProcedure;
                    objComando.Connection = this.conector;
                }
                else
                {
                    objComando.CommandText = comando;
                    objComando.CommandType = CommandType.Text;
                    objComando.Connection = this.conector;
                }

                if (parametrosOdbc != null)
                {
                    objComando.Parameters.AddRange(parametrosOdbc);
                }

                var objAdaptador = new SqlDataAdapter(objComando);

                objAdaptador.Fill(unaTabla);
            }
            catch (Exception e)
            {
                throw new Exception("Error al consultar a la BD (" + comando + "): " + e.Message);
            }

            return unaTabla;
        }
        public bool EjecutarSP(string nombreSP, string obj, string sent, int etapa, string[] codigos)
        {
            try
            {
                // Paso 1: Crear la tabla en memoria
                DataTable tablaCodigos = new DataTable();
                tablaCodigos.Columns.Add("value", typeof(string));

                // Suponiendo que tenés una lista de strings con los códigos
                foreach (string codigo in codigos)
                {
                    tablaCodigos.Rows.Add(codigo);
                }

                SqlCommand cmd = new SqlCommand(nombreSP, this.Conexion);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 180;

                cmd.Parameters.AddWithValue("@objType", obj);
                cmd.Parameters.AddWithValue("@sent", sent);
                cmd.Parameters.AddWithValue("@etapa", etapa + 1);

                SqlParameter parametroTVP = cmd.Parameters.AddWithValue("@docCodes", tablaCodigos);
                parametroTVP.SqlDbType = SqlDbType.Structured;
                parametroTVP.TypeName = "DocCodeTable";

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Utils.Logger.EscribirLog("Error al ejecutar SP: " + ex.Message, true);
                return false;
            }
        }
        public void BorrarOCRepetida(int docNum)
        {
            string query="";
            try
            {
                query = @"DELETE FROM ST_NAPSE_SET " +
                        "WHERE DocType='22' " +                       
                        "AND DocCode=" + docNum;

                var objComando = new SqlCommand();
                objComando.CommandText = query;
                objComando.CommandType = CommandType.Text;
                objComando.Connection = this.conector;

                objComando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar el delete (" + query + "): " + ex.Message);
            }
        }
        #region Parametros

        /// <summary>
        /// Permite crear parámetros de tipo string.
        /// </summary>
        /// <param name="pNombre"></param>
        /// <param name="pValor"></param>
        /// <returns></returns>
        public SqlParameter CrearParametro(string pNombre, string pValor)
        {

            SqlParameter objParametro = new SqlParameter();

            objParametro.ParameterName = pNombre;
            objParametro.Value = pValor;
            objParametro.DbType = DbType.String;

            return objParametro;
        }

        /// <summary>
        /// Permite crear parámetros de tipo double.
        /// </summary>
        /// <param name="pNombre"></param>
        /// <param name="pValor"></param>
        /// <returns></returns>
        public SqlParameter CrearParametro(string pNombre, double pValor)
        {

            SqlParameter objParametro = new SqlParameter();

            objParametro.ParameterName = pNombre;
            objParametro.Value = pValor;
            objParametro.DbType = DbType.Double;

            return objParametro;
        }

        /// <summary>
        /// Permite crear parámetros de tipo datetime.
        /// </summary>
        /// <param name="pNombre"></param>
        /// <param name="pValor"></param>
        /// <returns></returns>
        public SqlParameter CrearParametro(string pNombre, DateTime pValor)
        {

            SqlParameter objParametro = new SqlParameter();

            objParametro.ParameterName = pNombre;
            objParametro.Value = pValor;
            objParametro.DbType = DbType.DateTime;

            return objParametro;
        }

        /// <summary>
        /// Permite crear parámetros de tipo timeSpan.
        /// </summary>
        /// <param name="pNombre"></param>
        /// <param name="pValor"></param>
        /// <returns></returns>
        public SqlParameter CrearParametro(string pNombre, TimeSpan pValor)
        {

            SqlParameter objParametro = new SqlParameter();

            objParametro.ParameterName = pNombre;
            objParametro.Value = pValor;
            objParametro.DbType = DbType.DateTime;

            return objParametro;
        }

        /// <summary>
        /// Permite crear parámetros de tipo int.
        /// </summary>
        /// <param name="pNombre"></param>
        /// <param name="pValor"></param>
        /// <returns></returns>
        public SqlParameter CrearParametro(string pNombre, int pValor)
        {

            SqlParameter objParametro = new SqlParameter();

            objParametro.ParameterName = pNombre;
            objParametro.Value = pValor;
            objParametro.DbType = DbType.Int32;

            return objParametro;
        }

        /// <summary>
        /// Permite crear parámetros de tipo booleano.
        /// </summary>
        /// <param name="pNombre"></param>
        /// <param name="pValor"></param>
        /// <returns></returns>
        public SqlParameter CrearParametro(string pNombre, Boolean pValor)
        {

            SqlParameter objParametro = new SqlParameter();

            objParametro.ParameterName = pNombre;
            objParametro.Value = pValor;
            objParametro.DbType = DbType.Boolean;

            return objParametro;
        }

        #endregion

        #endregion

        #region MÉTODOS PRIVADOS

        /// <summary>
        /// Genera la cadena de conexión con los valores de los atributos.
        /// </summary>
        /// <returns></returns>
        private string GenerarConnectionString()
        {
            string conn;

            conn = "Persist Security Info=False" + ";User ID=" + this.user + ";Password=" + this.pass + ";Initial Catalog=" + this.database + ";Data Source=" + this.server;

            return conn;
        }

        #endregion
    }
}
