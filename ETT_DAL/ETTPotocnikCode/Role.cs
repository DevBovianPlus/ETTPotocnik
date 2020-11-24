using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ETT_DAL.ETTPotocnik
{

    public partial class Role
    {
        public Role(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
