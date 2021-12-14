using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services
{
    class DictsService : BaseDatabase, IDicts
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public DictsService(IDatabase db) => Database = db;
    }
}
