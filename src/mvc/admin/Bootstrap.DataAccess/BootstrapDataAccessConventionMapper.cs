using PetaPoco;
using System;

namespace Bootstrap.DataAccess
{
    internal class BootstrapDataAccessConventionMapper : ConventionMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pocoType"></param>
        /// <returns></returns>
        public override TableInfo GetTableInfo(Type pocoType)
        {
            var ti = base.GetTableInfo(pocoType);
            ti.AutoIncrement = true;
            ti.SequenceName = $"SEQ_{ti.TableName.ToUpperInvariant()}_ID";
            return ti;
        }
    }
}
