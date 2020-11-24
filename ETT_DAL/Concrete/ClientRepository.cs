using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_DAL.Models.Supplier;
using ETT_Utilities.Common;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Concrete
{
    public class ClientRepository : IClientRepository
    {
        Session session;

        public ClientRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;
        }


        public Client GetClientByID(int cId, Session currentSession = null)
        {
            try
            {
                XPQuery<Client> client = null;

                if (currentSession != null)
                    client = currentSession.Query<Client>();
                else
                    client = session.Query<Client>();

                return client.Where(c => c.ClientID == cId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public ClientType GetClientTypeByID(int ctId, Session currentSession = null)
        {
            try
            {
                XPQuery<ClientType> clientType = null;

                if (currentSession != null)
                    clientType = currentSession.Query<ClientType>();
                else
                    clientType = session.Query<ClientType>();

                return clientType.Where(ct => ct.ClientTypeID == ctId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public ClientType GetClientTypeByCode(string ctCode, Session currentSession = null)
        {
            try
            {
                XPQuery<ClientType> clientType = null;

                if (currentSession != null)
                    clientType = currentSession.Query<ClientType>();
                else
                    clientType = session.Query<ClientType>();

                return clientType.Where(ct => ct.Code == ctCode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public ContactPerson GetContactPersonByID(int cpId, Session currentSession = null)
        {
            try
            {
                XPQuery<ContactPerson> contactPerson = null;

                if (currentSession != null)
                    contactPerson = currentSession.Query<ContactPerson>();
                else
                    contactPerson = session.Query<ContactPerson>();

                return contactPerson.Where(cp => cp.ContactPersonID == cpId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveClient(Client model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.ClientID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }
                
                model.Save();

                return model.ClientID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_08, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveClientType(ClientType model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.ClientTypeID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }
                
                model.Save();

                return model.ClientTypeID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_08, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveContactPerson(ContactPerson model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.ContactPersonID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.ContactPersonID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_08, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteClient(int cId)
        {
            try
            {
                GetClientByID(cId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_09, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteClient(Client model)
        {
            try
            {
                model.Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_09, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteClientType(int ctId)
        {
            try
            {
                GetClientTypeByID(ctId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_09, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteClientType(ClientType model)
        {
            try
            {
                model.Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_09, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteContactPerson(int cpId)
        {
            try
            {
                GetContactPersonByID(cpId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_09, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteContactPerson(ContactPerson model)
        {
            try
            {
                model.Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_09, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetContactPersonCountByClientID(int cId, Session currentSession = null)
        {
            try
            {
                XPQuery<ContactPerson> contactPerson = null;

                if (currentSession != null)
                    contactPerson = currentSession.Query<ContactPerson>();
                else
                    contactPerson = session.Query<ContactPerson>();

                return contactPerson.Where(cp => cp.ClientID.ClientID == cId).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<SupplierAPIModel> GetSuppliersMobile()
        {
            try
            {
                var clientType = GetClientTypeByCode(Enums.ClientType.DOBAVITELJ.ToString());
                XPQuery<Client> client = session.Query<Client>();

                var query = from c in client
                            where c.ClientTypeID.ClientTypeID == clientType.ClientTypeID
                            select new SupplierAPIModel
                            {
                                address = c.Address,
                                city = "",
                                contact_email = "",
                                contact_fax = "",
                                contact_name = "",
                                contact_phone = "",
                                contact_title = "",
                                country = c.Country,
                                created_at = c.tsInsert.ToShortDateString(),
                                id = c.ClientID.ToString(),
                                name = c.Name,
                                postal_code = c.Postcode,
                                region = "",
                                updated_at = c.tsUpdate.ToShortDateString(),
                                zip_code = ""
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}