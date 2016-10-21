using Longbow;
using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.Data;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace Bootstrap.DataAccess
{
    public static class TerminalHelper
    {
        private const string TerminalDataKey = "TerminalData-CodeTerminalHelper";

        /// <summary>
        /// 查询所有输入口
        /// </summary>
        /// <param name="pIds"></param>
        /// <returns></returns>
        public static IEnumerable<Terminal> RetrieveTerminals(string tId = null)
        {
            string sql = "select t.*, tc.RuleID, r.Name RuleName from Terminals t left join TerminalRuleConfig tc on t.ID = tc.TerminalId left join Rules r on tc.RuleId = r.Id order by t.Name";
            var ret = CacheManager.GetOrAdd(TerminalDataKey, CacheSection.RetrieveIntervalByKey(TerminalDataKey), key =>
            {
                List<Terminal> Terminals = new List<Terminal>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Terminals.Add(new Terminal()
                            {
                                ID = (int)reader[0],
                                Name = (string)reader[1],
                                ClientIP = (string)reader[2],
                                ClientPort = (int)reader[3],
                                ServerIP = (string)reader[4],
                                ServerPort = (int)reader[5],
                                DeviceIP = (string)reader[6],
                                DevicePort = (int)reader[7],
                                DatabaseName = LgbConvert.ReadValue(reader[8], string.Empty),
                                DatabaseUserName = LgbConvert.ReadValue(reader[9], string.Empty),
                                DatabasePassword = LgbConvert.ReadValue(reader[10], string.Empty),
                                Status = LgbConvert.ReadValue(reader[11], false),
                                RuleID = LgbConvert.ReadValue(reader[12], 0),
                                RuleName = LgbConvert.ReadValue(reader[13], string.Empty)
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return Terminals;
            }, CacheSection.RetrieveDescByKey(TerminalDataKey));
            return string.IsNullOrEmpty(tId) ? ret : ret.Where(t => tId.Equals(t.ID.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 删除输入口
        /// </summary>
        /// <param name="ids"></param>
        public static void DeleteTerminal(string ids)
        {
            if (string.IsNullOrEmpty(ids) || ids.Contains("'")) return;
            string sql = string.Format(CultureInfo.InvariantCulture, "Delete from Terminals where ID in ({0})", ids);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                ClearCache();
            }
        }

        /// <summary>
        /// 保存新建/更新的输入口信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SaveTerminal(Terminal p)
        {
            if (p == null) throw new ArgumentNullException("p");
            bool ret = false;
            if (p.Name.Length > 50) p.Name.Substring(0, 50);
            if (p.ClientIP.Length > 15) p.ClientIP.Substring(0, 15);
            p.ClientPort = Math.Max(0, p.ClientPort);
            if (p.ServerIP.Length > 15) p.ServerIP.Substring(0, 15);
            p.ServerPort = Math.Max(1, p.ServerPort);
            if (p.DeviceIP.Length > 15) p.DeviceIP.Substring(0, 15);
            p.DevicePort = Math.Max(1, p.DevicePort);
            if (!string.IsNullOrEmpty(p.DatabaseName) && p.DatabaseName.Length > 50) p.DatabaseName = p.DatabaseName.Substring(0, 50);
            if (!string.IsNullOrEmpty(p.DatabaseUserName) && p.DatabaseUserName.Length > 50) p.DatabaseUserName = p.DatabaseUserName.Substring(0, 50);
            if (!string.IsNullOrEmpty(p.DatabasePassword) && p.DatabasePassword.Length > 50) p.DatabasePassword = p.DatabasePassword.Substring(0, 50);
            string sql = p.ID == 0 ?
                "Insert Into Terminals (Name, ClientIP, ClientPort, ServerIP, ServerPort, DeviceIP, DevicePort, DatabaseName, DatabaseUserName, DatabasePassword) Values (@Name, @ClientIP, @ClientPort, @ServerIP, @ServerPort, @DeviceIP, @DevicePort, @DatabaseName, @DatabaseUserName, @DatabasePassword)" :
                "Update Terminals set Name = @Name, ClientIP = @ClientIP, ClientPort = @ClientPort, ServerIP = @ServerIP, ServerPort = @ServerPort, DeviceIP = @DeviceIP, DevicePort = @DevicePort, DatabaseName = @DatabaseName, DatabaseUserName = @DatabaseUserName, DatabasePassword = @DatabasePassword where ID = @ID";
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ID", p.ID, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@Name", p.Name, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ClientIP", p.ClientIP, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ClientPort", p.ClientPort, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ServerIP", p.ServerIP, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@ServerPort", p.ServerPort, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@DeviceIP", p.DeviceIP, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@DevicePort", p.DevicePort, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@DatabaseName", DBAccess.ToDBValue(p.DatabaseName), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@DatabaseUserName", DBAccess.ToDBValue(p.DatabaseUserName), ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@DatabasePassword", DBAccess.ToDBValue(p.DatabasePassword), ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
                ret = true;
                ClearCache();
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return ret;
        }

        // 更新缓存
        private static void ClearCache()
        {
            CacheManager.Clear(key => key.Contains("TerminalData-"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminalId"></param>
        /// <param name="ruleId"></param>
        public static void StartTerminal(int terminalId, int ruleId)
        {
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_StartTerminal"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@tId", terminalId, ParameterDirection.Input));
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@rId", ruleId, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminalId"></param>
        public static void StopTerminal(int terminalId)
        {
            try
            {
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_StopTerminal"))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@tId", terminalId, ParameterDirection.Input));
                    DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
                }
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
        }
    }
}
