
namespace Entidades
{
    public class Supplier : NapseEntity
    {
        #region ATRIBUTOS

        private string code_;
        private string name_;
        private string address_;
        private string phone_;
        private string email_;
        private string fiscalId_;
        private bool activeFlag_;
        private string fantasyName_;

        #endregion

        #region PROPIEDADES

        public string code
        {
            get { return code_; }
            set { code_ = value; }
        }

        public string name
        {
            get { return name_; }
            set { name_ = value; }
        }

        public string address
        {
            get { return address_; }
            set { address_ = value; }
        }

        public string phone
        {
            get { return phone_; }
            set { phone_ = value; }
        }

        public string email
        {
            get { return email_; }
            set { email_ = value; }
        }

        public string fiscalId
        {
            get { return fiscalId_; }
            set { fiscalId_ = value; }
        }

        public bool activeFlag
        {
            get { return activeFlag_; }
            set { activeFlag_ = value; }
        }

        public string fantasyName
        {
            get { return fantasyName_; }
            set { fantasyName_ = value; }
        }

        #endregion
    }
}
