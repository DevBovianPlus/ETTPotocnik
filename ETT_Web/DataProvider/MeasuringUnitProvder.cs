using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class MeasuringUnitProvder : ServerMasterPage
    {
        public void SetMeasuringUnitModel(MeasuringUnit model)
        {
            AddValueToSession(Enums.MeasuringUnitSession.MeasuringUnitModel, model);
        }

        public MeasuringUnit GetMeasuringUnitModel()
        {
            if (SessionHasValue(Enums.MeasuringUnitSession.MeasuringUnitModel))
                return (MeasuringUnit)GetValueFromSession(Enums.MeasuringUnitSession.MeasuringUnitModel);

            return null;
        }

    }
}