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

    public partial class InventoryDeliveriesLocation : XPLiteObject
    {
        int fInventoryDeliveriesLocationID;
        [Key(true)]
        public int InventoryDeliveriesLocationID
        {
            get { return fInventoryDeliveriesLocationID; }
            set { SetPropertyValue<int>(nameof(InventoryDeliveriesLocationID), ref fInventoryDeliveriesLocationID, value); }
        }
        InventoryDeliveries fInventoryDeliveriesID;
        [Association(@"InventoryDeliveriesLocationReferencesInventoryDeliveries")]
        public InventoryDeliveries InventoryDeliveriesID
        {
            get { return fInventoryDeliveriesID; }
            set { SetPropertyValue<InventoryDeliveries>(nameof(InventoryDeliveriesID), ref fInventoryDeliveriesID, value); }
        }
        Location fLocationFromID;
        [Association(@"InventoryDeliveriesLocationReferencesLocation1")]
        public Location LocationFromID
        {
            get { return fLocationFromID; }
            set { SetPropertyValue<Location>(nameof(LocationFromID), ref fLocationFromID, value); }
        }
        Location fLocationToID;
        [Association(@"InventoryDeliveriesLocationReferencesLocation")]
        public Location LocationToID
        {
            get { return fLocationToID; }
            set { SetPropertyValue<Location>(nameof(LocationToID), ref fLocationToID, value); }
        }
        string fNotes;
        [Size(300)]
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
        Users fUserID;
        [Association(@"InventoryDeliveriesLocationReferencesUsers")]
        public Users UserID
        {
            get { return fUserID; }
            set { SetPropertyValue<Users>(nameof(UserID), ref fUserID, value); }
        }
        bool fIsMobileTransaction;
        public bool IsMobileTransaction
        {
            get { return fIsMobileTransaction; }
            set { SetPropertyValue<bool>(nameof(IsMobileTransaction), ref fIsMobileTransaction, value); }
        }
        int fParentID;
        public int ParentID
        {
            get { return fParentID; }
            set { SetPropertyValue<int>(nameof(ParentID), ref fParentID, value); }
        }
        bool fNeedsMatching;
        public bool NeedsMatching
        {
            get { return fNeedsMatching; }
            set { SetPropertyValue<bool>(nameof(NeedsMatching), ref fNeedsMatching, value); }
        }
        [Association(@"MobileTransactionReferencesInventoryDeliveriesLocation")]
        public XPCollection<MobileTransaction> MobileTransactions { get { return GetCollection<MobileTransaction>(nameof(MobileTransactions)); } }
    }

}
