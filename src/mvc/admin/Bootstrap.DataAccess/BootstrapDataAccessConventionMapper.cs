// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

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
