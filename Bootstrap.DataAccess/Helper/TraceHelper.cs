using Longbow.Data;
using Longbow.Web.Mvc;
using PetaPoco;
using System;

namespace Bootstrap.DataAccess
{
    public static class TraceHelper
    {

        /// <summary>
        /// 保存访问历史记录
        /// </summary>
        /// <param name="p"></param>
        public static void Save(Trace p) => DbContextManager.Create<Trace>().Save(p);

        /// <summary>
        /// 获得指定IP历史访问记录
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static Page<Trace> Retrieves(PaginationOption po, DateTime? startTime, DateTime? endTime) => DbContextManager.Create<Trace>().Retrieves(po, startTime, endTime);
    }
}
