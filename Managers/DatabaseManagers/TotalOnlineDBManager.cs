using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.DatabaseManagers
{
    public class TotalOnlineDBManager : ManagerBase<TotalOnlineDBManager>
    {
        public void Save(int Count)
        {
            var save_key = string.Format("{0}_total_online", DateTime.Now.ToString("yyyy_MM_dd"));
            var total_online = DatabaseManager.Instance.Reports.GetDocument<TotalOnline>(save_key);
            if (total_online == null)
            {
                total_online = new TotalOnline();
                total_online.Id = save_key;
                total_online.CreatedAt = DateTime.Now;
            }
            if (total_online.Online < Count)
            {
                total_online.Online = Count;
                total_online.UpdatedAt = DateTime.Now;
                DatabaseManager.Instance.Reports.SaveDocument(total_online);
            }
        }
    }
}
