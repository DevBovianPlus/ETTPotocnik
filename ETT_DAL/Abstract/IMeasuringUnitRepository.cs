using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IMeasuringUnitRepository
    {
        MeasuringUnit GetMeasuringUnitByID(int muId, Session currentSession = null);
        int SaveMeasuringUnit(MeasuringUnit model, int userID = 0);
        bool DeleteMeasuringUnit(int muId);
        bool DeleteMeasuringUnit(MeasuringUnit model);
        MeasuringUnit GetMeasuringUnitByCode(string code, Session currentSession = null);
    }
}
