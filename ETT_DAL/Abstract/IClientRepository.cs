using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Models.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IClientRepository
    {
        Client GetClientByID(int cId, Session currentSession = null);
        ClientType GetClientTypeByID(int ctId, Session currentSession = null);
        ClientType GetClientTypeByCode(string ctCode, Session currentSession = null);
        ContactPerson GetContactPersonByID(int cpId, Session currentSession = null);
        int SaveClient(Client model, int userID = 0);
        int SaveClientType(ClientType model, int userID = 0);
        int SaveContactPerson(ContactPerson model, int userID = 0);
        bool DeleteClient(int cId);
        bool DeleteClient(Client model);
        bool DeleteClientType(int ctId);
        bool DeleteClientType(ClientType model);
        bool DeleteContactPerson(int cpId);
        bool DeleteContactPerson(ContactPerson model);
        int GetContactPersonCountByClientID(int cId, Session currentSession = null);
        List<SupplierAPIModel> GetSuppliersMobile();
    }
}
