using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using Utils;

namespace Entidades
{
    public class SapCompanyConfig
    {
        public string CompanyDB { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Server { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }

        public Company DatosSAP_BD()
        {
            Company oCompany = new Company();
            dynamic jsonConfig = Config.DataJSON.SapCompanyConnection;

            oCompany.Server = !string.IsNullOrEmpty(Server) ? Server : (string)jsonConfig.Server;
            oCompany.DbUserName = !string.IsNullOrEmpty(DbUserName) ? DbUserName : (string)jsonConfig.DbUserName;
            oCompany.DbPassword = !string.IsNullOrEmpty(DbPassword) ? DbPassword : (string)jsonConfig.DbPassword;
            oCompany.CompanyDB = !string.IsNullOrEmpty(CompanyDB) ? CompanyDB : (string)jsonConfig.CompanyDB;
            oCompany.UserName = !string.IsNullOrEmpty(UserName) ? UserName : (string)jsonConfig.UserName;
            oCompany.Password = !string.IsNullOrEmpty(Password) ? Password : (string)jsonConfig.Password;
            oCompany.DbServerType = BoDataServerTypes.dst_HANADB;

            return oCompany;
        }

    }
}
