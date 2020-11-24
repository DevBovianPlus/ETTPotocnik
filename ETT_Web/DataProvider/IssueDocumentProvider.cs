using ETT_DAL.ETTPotocnik;
using ETT_Utilities.Common;
using ETT_Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_Web.DataProvider
{
    public class IssueDocumentProvider : ServerMasterPage
    {
        public void SetIssueDocumentModel(IssueDocument model)
        {
            AddValueToSession(Enums.IssueDocumentSession.IssueDocumentModel, model);
        }

        public IssueDocument GetIssueDocumentModel()
        {
            if (SessionHasValue(Enums.IssueDocumentSession.IssueDocumentModel))
                return (IssueDocument)GetValueFromSession(Enums.IssueDocumentSession.IssueDocumentModel);

            return null;
        }

        public void SetIssueDocumentPositionModel(IssueDocumentPosition model)
        {
            AddValueToSession(Enums.IssueDocumentSession.IssueDocumentPositionModel, model);
        }

        public IssueDocumentPosition GetIssueDocumentPositionModel()
        {
            if (SessionHasValue(Enums.IssueDocumentSession.IssueDocumentPositionModel))
                return (IssueDocumentPosition)GetValueFromSession(Enums.IssueDocumentSession.IssueDocumentPositionModel);

            return null;
        }
    }
}