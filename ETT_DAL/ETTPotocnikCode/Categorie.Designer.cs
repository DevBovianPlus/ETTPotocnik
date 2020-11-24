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

    public partial class Categorie : XPLiteObject
    {
        int fCategorieID;
        [Key(true)]
        public int CategorieID
        {
            get { return fCategorieID; }
            set { SetPropertyValue<int>(nameof(CategorieID), ref fCategorieID, value); }
        }
        string fName;
        [Size(150)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref fName, value); }
        }
        string fCode;
        [Size(20)]
        public string Code
        {
            get { return fCode; }
            set { SetPropertyValue<string>(nameof(Code), ref fCode, value); }
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
        [Association(@"ProductReferencesCategorie")]
        public XPCollection<Product> Products { get { return GetCollection<Product>(nameof(Products)); } }
    }

}