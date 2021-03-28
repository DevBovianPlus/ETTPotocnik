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

    public partial class DeliveryNoteStatus : XPLiteObject
    {
        int fDeliveryNoteStatusID;
        [Key(true)]
        public int DeliveryNoteStatusID
        {
            get { return fDeliveryNoteStatusID; }
            set { SetPropertyValue<int>(nameof(DeliveryNoteStatusID), ref fDeliveryNoteStatusID, value); }
        }
        string fCode;
        [Size(30)]
        public string Code
        {
            get { return fCode; }
            set { SetPropertyValue<string>(nameof(Code), ref fCode, value); }
        }
        string fName;
        [Size(150)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref fName, value); }
        }
        string fNotes;
        [Size(500)]
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
        [Association(@"DeliveryNoteReferencesDeliveryNoteStatus")]
        public XPCollection<DeliveryNote> DeliveryNotes { get { return GetCollection<DeliveryNote>(nameof(DeliveryNotes)); } }
    }

}
