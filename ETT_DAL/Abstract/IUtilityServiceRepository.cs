using DevExpress.Xpo;
using ETT_DAL.ETTPotocnik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETT_DAL.Abstract
{
    public interface IUtilityServiceRepository
    {
        void MatchMobileTransWithInventoryDeliveries(List<InventoryDeliveriesLocation> issueDocumentTransactions = null, UnitOfWork uow = null);
        void ClearStockByIssueDocumentID(List<IssueDocumentPosition> pos, UnitOfWork uow);
    }
}
