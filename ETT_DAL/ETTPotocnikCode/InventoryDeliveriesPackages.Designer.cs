﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ETT_DAL.ETTPotocnik
{

    public partial class InventoryDeliveriesPackages : XPLiteObject
    {
        int fInventoryDeliveriesPackagesID;
        [Key(true)]
        public int InventoryDeliveriesPackagesID
        {
            get { return fInventoryDeliveriesPackagesID; }
            set { SetPropertyValue<int>(nameof(InventoryDeliveriesPackagesID), ref fInventoryDeliveriesPackagesID, value); }
        }
        string fElementUID250;
        [Size(250)]
        public string ElementUID250
        {
            get { return fElementUID250; }
            set { SetPropertyValue<string>(nameof(ElementUID250), ref fElementUID250, value); }
        }
        int fParentElementID;
        public int ParentElementID
        {
            get { return fParentElementID; }
            set { SetPropertyValue<int>(nameof(ParentElementID), ref fParentElementID, value); }
        }
        string fNotes;
        [Size(250)]
        public string Notes
        {
            get { return fNotes; }
            set { SetPropertyValue<string>(nameof(Notes), ref fNotes, value); }
        }
        DateTime ftsInsert;
        public DateTime tsInsert
        {
            get { return ftsInsert; }
            set { SetPropertyValue<DateTime>(nameof(tsInsert), ref ftsInsert, value); }
        }
        int ftsInsertUserID;
        public int tsInsertUserID
        {
            get { return ftsInsertUserID; }
            set { SetPropertyValue<int>(nameof(tsInsertUserID), ref ftsInsertUserID, value); }
        }
        DateTime ftsUpdate;
        public DateTime tsUpdate
        {
            get { return ftsUpdate; }
            set { SetPropertyValue<DateTime>(nameof(tsUpdate), ref ftsUpdate, value); }
        }
        int ftsUpdateUserID;
        public int tsUpdateUserID
        {
            get { return ftsUpdateUserID; }
            set { SetPropertyValue<int>(nameof(tsUpdateUserID), ref ftsUpdateUserID, value); }
        }
        [Association(@"InventoryDeliveriesReferencesInventoryDeliveriesPackages")]
        public XPCollection<InventoryDeliveries> InventoryDeliveriesCollection { get { return GetCollection<InventoryDeliveries>(nameof(InventoryDeliveriesCollection)); } }
    }

}
