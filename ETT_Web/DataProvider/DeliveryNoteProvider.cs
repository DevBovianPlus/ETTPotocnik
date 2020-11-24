using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class DeliveryNoteProvider : ServerMasterPage
    {
        public void SetDeliveryNoteModel(DeliveryNote model)
        {
            AddValueToSession(Enums.DeliveryNoteSession.DeliveryNoteModel, model);
        }

        public DeliveryNote GetDeliveryNoteModel()
        {
            if (SessionHasValue(Enums.DeliveryNoteSession.DeliveryNoteModel))
                return (DeliveryNote)GetValueFromSession(Enums.DeliveryNoteSession.DeliveryNoteModel);

            return null;
        }

        public void SetDeliveryNoteStatus(Enums.DeliveryNoteStatus status)
        {
            AddValueToSession(Enums.DeliveryNoteSession.DeliveryNoteCurrentStatus, status);
        }

        public Enums.DeliveryNoteStatus GetDeliveryNoteStatus()
        {
            if (SessionHasValue(Enums.DeliveryNoteSession.DeliveryNoteCurrentStatus))
                return (Enums.DeliveryNoteStatus)GetValueFromSession(Enums.DeliveryNoteSession.DeliveryNoteCurrentStatus);

            return Enums.DeliveryNoteStatus.Not_Processed;
        }
    }
}