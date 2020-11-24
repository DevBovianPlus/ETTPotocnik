using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_UtilityService
{
    public class NinjectBindings : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IUtilityServiceRepository>().To<UtilityServiceRepository>();
        }
    }
}
