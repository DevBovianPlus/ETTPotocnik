using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IIssueDocumentRepository
    {
        IssueDocument GetIssueDocumentByID(int ID, Session currentSession = null);
        int SaveIssueDocument(IssueDocument model, int userID = 0);
        bool DeleteIssueDocument(int ID);
        bool DeleteIssueDocument(IssueDocument model);

        IssueDocumentPosition GetIssueDocumentPositionByID(int ID, Session currentSession = null);
        int SaveIssueDocumentPosition(IssueDocumentPosition model, int userID = 0);
        void SaveIssueDocumentPositions(List<IssueDocumentPosition> model, int issueDocumentID, int userID = 0);
        bool DeleteIssueDocumentPosition(int ID);
        bool DeleteIssueDocumentPosition(IssueDocumentPosition model);

        IssueDocumentStatus GetIssueDocumentStatusByID(int ID, Session currentSession = null);
        IssueDocumentStatus GetIssueDocumentStatusByCode(object statusCode, Session currentSession = null);

        int CreateIssueDocumentFromMobileTransactions(List<int> mobileTransactionsID, int userID, int buyerID);
        List<IssueDocumentPosition> GetIssueDocumentPositionsByDocumentID(int ID, Session currentSession = null);
    }
}
