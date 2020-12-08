using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ETT_Utilities.Common
{
    public class Enums
    {
        public enum UserAction : int
        {
            Add = 1,
            Edit = 2,
            Delete = 3
        }

        public enum UserRole
        {
            SuperAdmin = 4,
            Admin = 3,
            Miner = 2,
            Editor = 1,
            All = 5,
            None = -1
        }

        public enum CommonSession
        {
            ShowWarning,
            ShowWarningMessage,
            UserActionPopUp,
            activeTab,
            PrintModel,
            PreviousPageName,
            PreviousPageSessions,
            DownloadDocument,
            UserActionPopUpInPopUp,
            OpenPopupAfterRefresh
        }

        public enum QueryStringName
        {
            action,
            recordId,
            printReport,
            printId,
            showPreviewReport,
            successMessage,
            activeTab,
            notifyProcessing,
        }

        public enum SystemServiceSatus
        {
            UnProcessed = 0,
            Processed = 1,
            Error = 2,
            RecipientError = 3
        }

        public enum Cookies
        {
            UserLastRequest,
            SessionExpires
        }

        public enum CookieCommonValue
        {
            STOP
        }

        public enum SystemEmailMessage
        {
            SystemEmailMessageID,
            SystemEmailMessageBody
        }

        public enum SystemEmailMessageStatus
        {
            UnProcessed = 0,
            Processed = 1,
            Error = 2,
            RecipientError = 3
        }

        public enum ClientType
        {
            DOBAVITELJ,
            KUPEC
        }

        public enum MeasuringUnitSession
        {
            MeasuringUnitID,
            MeasuringUnitModel
        }

        public enum SupplierSession
        {
            ClientID,
            ClientModel,
            ContactPersonModel,
            ContactPersonID
        }

        public enum LocationSession
        {
            LocationID,
            LocationModel
        }

        public enum CategorieSession
        {
            CategorieID,
            CategorieModel
        }

        public enum ProductSession
        {
            ProductID,
            ProductModel
        }

        public enum EmployeeSession
        {
            EmployeeID,
            EmployeeModel,
            UserModel,
            UserID
        }

        public enum UserActivityEnum
        {
            LOGIN,
            LOGOUT,
            ADD_ENTRY,
            EDIT_ENTRY,
            DELETE_ENTRY
        }

        public enum DeliveryNoteSession
        {
            DeliveryNoteID,
            DeliveryNoteModel,
            DeliveryNoteCurrentStatus
        }

        public enum XMLTagName
        {
            Shipment,
            SummaryItems,
            SummaryItem,
            Units,
            ProducerProductCode,
            ProducerProductName,
            ItemQuantity,
            CountOfTradeUnits,
            PackagingLevel,
            ProductionDate,
            NEW,
            Length,
            UnitOfMeasure,
            Posiljka,
            Vsebina,
            Artikli,
            Ident,
            Sifra_ident,
            Naziv_ident,
            Pakiranje_ident,
            EM_ident,
            Kolicina_ident,
            Serijske,
            Barkoda_serijske,
            Tip_pakiranja_serijske,
            Naziv_serijske,
            Kolicina_serijske,
            Serijske_podnivo,
            SP_Barkoda_serijske,
            SP_Tip_pakiranja_serijske,
            SP_Kolicina_serijske
        }

        public enum XMLTagAttributeName
        {
            SID,
            UID,
            PSN
        }

        public enum InventoryStockSession
        {
            InventoryStockID,
            InventoryStockModel
        }

        public enum MobileTransactionSession
        {
            MobileTransactionID,
            MobileTransactionModel
        }
        public enum InventoryDeliveriesSession
        {
            InventoryDeliveriesID,
            InventoryDeliveriesModel
        }

        public enum IssueDocumentSession
        {
            IssueDocumentID,
            IssueDocumentModel,
            IssueDocumentPositionID,
            IssueDocumentPositionModel,
            SearchUIDValue
        }

        public enum IssueDocumentStatus
        {
            DELOVNA,
            ZAKLJUCENO,
            PRENOS
        }

      
        public enum PackageCodeStructure
        {
            [Description("UID stevilka izdelka oz. pakiranja")]
            UIDStevilkaIzdelka = 250,
            [Description("Država in tovarna porekla")]
            Poreklo = 90,
        }

        public enum DeliveryNoteStatus
        {
            In_Process,
            Completed,
            Error,
            Not_Processed
        }
    }
}