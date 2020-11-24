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

    public partial class InventoryDeliveries : XPLiteObject
    {
        int fInventoryDeliveriesID;
        [Key(true)]
        public int InventoryDeliveriesID
        {
            get { return fInventoryDeliveriesID; }
            set { SetPropertyValue<int>(nameof(InventoryDeliveriesID), ref fInventoryDeliveriesID, value); }
        }
        DeliveryNoteItem fDeliveryNoteItemID;
        [Association(@"InventoryDeliveriesReferencesDeliveryNoteItem")]
        public DeliveryNoteItem DeliveryNoteItemID
        {
            get { return fDeliveryNoteItemID; }
            set { SetPropertyValue<DeliveryNoteItem>(nameof(DeliveryNoteItemID), ref fDeliveryNoteItemID, value); }
        }
        InventoryDeliveriesPackages fInventoryDeliveriesPackagesID;
        [Association(@"InventoryDeliveriesReferencesInventoryDeliveriesPackages")]
        public InventoryDeliveriesPackages InventoryDeliveriesPackagesID
        {
            get { return fInventoryDeliveriesPackagesID; }
            set { SetPropertyValue<InventoryDeliveriesPackages>(nameof(InventoryDeliveriesPackagesID), ref fInventoryDeliveriesPackagesID, value); }
        }
        string fSupplierProductCode;
        [Size(50)]
        public string SupplierProductCode
        {
            get { return fSupplierProductCode; }
            set { SetPropertyValue<string>(nameof(SupplierProductCode), ref fSupplierProductCode, value); }
        }
        string fAtomeUID250;
        [Size(250)]
        public string AtomeUID250
        {
            get { return fAtomeUID250; }
            set { SetPropertyValue<string>(nameof(AtomeUID250), ref fAtomeUID250, value); }
        }
        string fPackagesUIDs;
        [Size(800)]
        public string PackagesUIDs
        {
            get { return fPackagesUIDs; }
            set { SetPropertyValue<string>(nameof(PackagesUIDs), ref fPackagesUIDs, value); }
        }
        string fPackagesSIDs;
        [Size(500)]
        public string PackagesSIDs
        {
            get { return fPackagesSIDs; }
            set { SetPropertyValue<string>(nameof(PackagesSIDs), ref fPackagesSIDs, value); }
        }
        string fNotes;
        [Size(1250)]
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
        InventoryStock fInventoryStockID;
        [Association(@"InventoryDeliveriesReferencesInventoryStock")]
        public InventoryStock InventoryStockID
        {
            get { return fInventoryStockID; }
            set { SetPropertyValue<InventoryStock>(nameof(InventoryStockID), ref fInventoryStockID, value); }
        }
        Location fLastLocationID;
        [Association(@"InventoryDeliveriesReferencesLocation")]
        public Location LastLocationID
        {
            get { return fLastLocationID; }
            set { SetPropertyValue<Location>(nameof(LastLocationID), ref fLastLocationID, value); }
        }
        decimal fQuantity;
        public decimal Quantity
        {
            get { return fQuantity; }
            set { SetPropertyValue<decimal>(nameof(Quantity), ref fQuantity, value); }
        }
        MeasuringUnit fUnitOfMeasureID;
        [Association(@"InventoryDeliveriesReferencesMeasuringUnit")]
        public MeasuringUnit UnitOfMeasureID
        {
            get { return fUnitOfMeasureID; }
            set { SetPropertyValue<MeasuringUnit>(nameof(UnitOfMeasureID), ref fUnitOfMeasureID, value); }
        }
        [Association(@"InventoryDeliveriesLocationReferencesInventoryDeliveries")]
        public XPCollection<InventoryDeliveriesLocation> InventoryDeliveriesLocations { get { return GetCollection<InventoryDeliveriesLocation>(nameof(InventoryDeliveriesLocations)); } }
    }

}