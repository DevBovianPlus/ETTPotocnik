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

    public partial class IssueDocumentPosition : XPLiteObject
    {
        int fIssueDocumentPositionID;
        [Key(true)]
        public int IssueDocumentPositionID
        {
            get { return fIssueDocumentPositionID; }
            set { SetPropertyValue<int>(nameof(IssueDocumentPositionID), ref fIssueDocumentPositionID, value); }
        }
        IssueDocument fIssueDocumentID;
        [Association(@"IssueDocumentPositionReferencesIssueDocument")]
        public IssueDocument IssueDocumentID
        {
            get { return fIssueDocumentID; }
            set { SetPropertyValue<IssueDocument>(nameof(IssueDocumentID), ref fIssueDocumentID, value); }
        }
        Client fSupplierID;
        [Association(@"IssueDocumentPositionReferencesClient")]
        public Client SupplierID
        {
            get { return fSupplierID; }
            set { SetPropertyValue<Client>(nameof(SupplierID), ref fSupplierID, value); }
        }
        decimal fQuantity;
        public decimal Quantity
        {
            get { return fQuantity; }
            set { SetPropertyValue<decimal>(nameof(Quantity), ref fQuantity, value); }
        }
        string fUID250;
        [Size(250)]
        public string UID250
        {
            get { return fUID250; }
            set { SetPropertyValue<string>(nameof(UID250), ref fUID250, value); }
        }
        string fName;
        [Size(300)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref fName, value); }
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
        string fNotes;
        [Size(1000)]
        public string Notes
        {
            get { return fNotes; }
            set { SetPropertyValue<string>(nameof(Notes), ref fNotes, value); }
        }
        Product fProductID;
        [Association(@"IssueDocumentPositionReferencesProduct")]
        public Product ProductID
        {
            get { return fProductID; }
            set { SetPropertyValue<Product>(nameof(ProductID), ref fProductID, value); }
        }
    }

}
