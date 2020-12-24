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

    public partial class Client : XPLiteObject
    {
        int fClientID;
        [Key(true)]
        public int ClientID
        {
            get { return fClientID; }
            set { SetPropertyValue<int>(nameof(ClientID), ref fClientID, value); }
        }
        ClientType fClientTypeID;
        [Association(@"ClientReferencesClientType")]
        public ClientType ClientTypeID
        {
            get { return fClientTypeID; }
            set { SetPropertyValue<ClientType>(nameof(ClientTypeID), ref fClientTypeID, value); }
        }
        string fName;
        [Size(150)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref fName, value); }
        }
        string fLongName;
        [Size(250)]
        public string LongName
        {
            get { return fLongName; }
            set { SetPropertyValue<string>(nameof(LongName), ref fLongName, value); }
        }
        string fAddress;
        [Size(50)]
        public string Address
        {
            get { return fAddress; }
            set { SetPropertyValue<string>(nameof(Address), ref fAddress, value); }
        }
        string fPostcode;
        [Size(15)]
        public string Postcode
        {
            get { return fPostcode; }
            set { SetPropertyValue<string>(nameof(Postcode), ref fPostcode, value); }
        }
        string fPostName;
        [Size(50)]
        public string PostName
        {
            get { return fPostName; }
            set { SetPropertyValue<string>(nameof(PostName), ref fPostName, value); }
        }
        string fCountry;
        [Size(50)]
        public string Country
        {
            get { return fCountry; }
            set { SetPropertyValue<string>(nameof(Country), ref fCountry, value); }
        }
        string fEmail;
        [Size(50)]
        public string Email
        {
            get { return fEmail; }
            set { SetPropertyValue<string>(nameof(Email), ref fEmail, value); }
        }
        string fPhone;
        [Size(50)]
        public string Phone
        {
            get { return fPhone; }
            set { SetPropertyValue<string>(nameof(Phone), ref fPhone, value); }
        }
        string fFAX;
        [Size(50)]
        public string FAX
        {
            get { return fFAX; }
            set { SetPropertyValue<string>(nameof(FAX), ref fFAX, value); }
        }
        string fBankAccount;
        [Size(40)]
        public string BankAccount
        {
            get { return fBankAccount; }
            set { SetPropertyValue<string>(nameof(BankAccount), ref fBankAccount, value); }
        }
        string fTaxNumber;
        [Size(30)]
        public string TaxNumber
        {
            get { return fTaxNumber; }
            set { SetPropertyValue<string>(nameof(TaxNumber), ref fTaxNumber, value); }
        }
        string fRegistrationNumber;
        [Size(50)]
        public string RegistrationNumber
        {
            get { return fRegistrationNumber; }
            set { SetPropertyValue<string>(nameof(RegistrationNumber), ref fRegistrationNumber, value); }
        }
        bool fTaxpayer;
        public bool Taxpayer
        {
            get { return fTaxpayer; }
            set { SetPropertyValue<bool>(nameof(Taxpayer), ref fTaxpayer, value); }
        }
        string fIdentificationNumber;
        [Size(50)]
        public string IdentificationNumber
        {
            get { return fIdentificationNumber; }
            set { SetPropertyValue<string>(nameof(IdentificationNumber), ref fIdentificationNumber, value); }
        }
        bool fEUMember;
        public bool EUMember
        {
            get { return fEUMember; }
            set { SetPropertyValue<bool>(nameof(EUMember), ref fEUMember, value); }
        }
        string fNotes;
        [Size(200)]
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
        [Association(@"ContactPersonReferencesClient")]
        public XPCollection<ContactPerson> ContactPersons { get { return GetCollection<ContactPerson>(nameof(ContactPersons)); } }
        [Association(@"DeliveryNoteReferencesClient")]
        public XPCollection<DeliveryNote> DeliveryNotes { get { return GetCollection<DeliveryNote>(nameof(DeliveryNotes)); } }
        [Association(@"ProductReferencesClient")]
        public XPCollection<Product> Products { get { return GetCollection<Product>(nameof(Products)); } }
        [Association(@"IssueDocumentReferencesClient")]
        public XPCollection<IssueDocument> IssueDocuments { get { return GetCollection<IssueDocument>(nameof(IssueDocuments)); } }
        [Association(@"IssueDocumentPositionReferencesClient")]
        public XPCollection<IssueDocumentPosition> IssueDocumentPositions { get { return GetCollection<IssueDocumentPosition>(nameof(IssueDocumentPositions)); } }
        [Association(@"MobileTransactionReferencesClient")]
        public XPCollection<MobileTransaction> MobileTransactions { get { return GetCollection<MobileTransaction>(nameof(MobileTransactions)); } }
        [Association(@"LocationReferencesClient")]
        public XPCollection<Location> Locations { get { return GetCollection<Location>(nameof(Locations)); } }
    }

}
