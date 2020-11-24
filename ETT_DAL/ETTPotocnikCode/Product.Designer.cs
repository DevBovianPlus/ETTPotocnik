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

    public partial class Product : XPLiteObject
    {
        int fProductID;
        [Key(true)]
        public int ProductID
        {
            get { return fProductID; }
            set { SetPropertyValue<int>(nameof(ProductID), ref fProductID, value); }
        }
        Client fSupplierID;
        [Association(@"ProductReferencesClient")]
        public Client SupplierID
        {
            get { return fSupplierID; }
            set { SetPropertyValue<Client>(nameof(SupplierID), ref fSupplierID, value); }
        }
        Categorie fCategoryID;
        [Association(@"ProductReferencesCategorie")]
        public Categorie CategoryID
        {
            get { return fCategoryID; }
            set { SetPropertyValue<Categorie>(nameof(CategoryID), ref fCategoryID, value); }
        }
        MeasuringUnit fMeasuringUnitID;
        [Association(@"ProductReferencesMeasuringUnit")]
        public MeasuringUnit MeasuringUnitID
        {
            get { return fMeasuringUnitID; }
            set { SetPropertyValue<MeasuringUnit>(nameof(MeasuringUnitID), ref fMeasuringUnitID, value); }
        }
        string fName;
        [Size(150)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref fName, value); }
        }
        string fSupplierCode;
        [Size(150)]
        public string SupplierCode
        {
            get { return fSupplierCode; }
            set { SetPropertyValue<string>(nameof(SupplierCode), ref fSupplierCode, value); }
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
        [Association(@"DeliveryNoteItemReferencesProduct")]
        public XPCollection<DeliveryNoteItem> DeliveryNoteItems { get { return GetCollection<DeliveryNoteItem>(nameof(DeliveryNoteItems)); } }
        [Association(@"InventoryStockReferencesProduct")]
        public XPCollection<InventoryStock> InventoryStocks { get { return GetCollection<InventoryStock>(nameof(InventoryStocks)); } }
        [Association(@"IssueDocumentPositionReferencesProduct")]
        public XPCollection<IssueDocumentPosition> IssueDocumentPositions { get { return GetCollection<IssueDocumentPosition>(nameof(IssueDocumentPositions)); } }
        [Association(@"MobileTransactionReferencesProduct")]
        public XPCollection<MobileTransaction> MobileTransactions { get { return GetCollection<MobileTransaction>(nameof(MobileTransactions)); } }
    }

}
