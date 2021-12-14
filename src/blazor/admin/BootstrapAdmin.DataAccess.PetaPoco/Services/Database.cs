using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services
{
    abstract class BaseDatabase
    {
        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        protected IDatabase? Database { get; set; }
    }
}
