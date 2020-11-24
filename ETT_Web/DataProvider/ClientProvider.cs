using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class ClientProvider : ServerMasterPage
    {
        public void SetSupplierModel(Client model)
        {
            AddValueToSession(Enums.SupplierSession.ClientModel, model);
        }

        public Client GetSupplierModel()
        {
            if (SessionHasValue(Enums.SupplierSession.ClientModel))
                return (Client)GetValueFromSession(Enums.SupplierSession.ClientModel);

            return null;
        }

        public void SetContactPersonModel(ContactPerson model)
        {
            AddValueToSession(Enums.SupplierSession.ContactPersonModel, model);
        }

        public ContactPerson GetContactPersonModel()
        {
            if (SessionHasValue(Enums.SupplierSession.ContactPersonModel))
                return (ContactPerson)GetValueFromSession(Enums.SupplierSession.ContactPersonModel);

            return null;
        }

    }
}