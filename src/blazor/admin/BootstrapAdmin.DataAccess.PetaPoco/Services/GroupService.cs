using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.Extensions.Primitives;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class GroupService : IGroup, IDisposable
{
    private const string GroupServiceGetAllCacheKey = "GroupService-GetAll";

    private const string GroupServiceGetGroupsByUserIdCacheKey = "GroupService-GetGroupsByUserId";

    private const string GroupServiceGetGroupsByRoleIdCacheKey = "GroupService-GetGroupsByRoleId";

    private CancellationTokenSource? GetGroupsByUserIdCancellationTokenSource { get; set; }

    private CancellationTokenSource? GetGroupsByRoleIdCancellationTokenSource { get; set; }

    private IDatabase Database { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public GroupService(IDatabase db) => Database = db;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Group> GetAll() => CacheManager.GetOrAdd(GroupServiceGetAllCacheKey, entry => Database.Fetch<Group>());

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<string> GetGroupsByUserId(string? userId) => CacheManager.GetOrAdd($"{GroupServiceGetGroupsByUserIdCacheKey}-{userId}", entry =>
    {
        GetGroupsByUserIdCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(10));
        var token = new CancellationChangeToken(GetGroupsByUserIdCancellationTokenSource.Token);
        entry.ExpirationTokens.Add(token);
        return Database.Fetch<string>("select GroupID from UserGroup where UserID = @0", userId);
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    public bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from UserGroup where UserID = @0", userId);
            Database.InsertBatch("UserGroup", groupIds.Select(g => new { GroupID = g, UserID = userId }));
            Database.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            Database.AbortTransaction();
            throw;
        }

        if (ret)
        {
            GetGroupsByUserIdCancellationTokenSource?.Cancel();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<string> GetGroupsByRoleId(string? roleId) => CacheManager.GetOrAdd($"{GroupServiceGetGroupsByRoleIdCacheKey}-{roleId}", entry =>
    {
        GetGroupsByRoleIdCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(10));
        var token = new CancellationChangeToken(GetGroupsByRoleIdCancellationTokenSource.Token);
        entry.ExpirationTokens.Add(token);
        return Database.Fetch<string>("select GroupID from RoleGroup where RoleID = @0", roleId);
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    public bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from RoleGroup where RoleID = @0", roleId);
            Database.InsertBatch("RoleGroup", groupIds.Select(g => new { GroupID = g, RoleID = roleId }));
            Database.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            Database.AbortTransaction();
            throw;
        }

        if (ret)
        {
            // reset cache
            GetGroupsByRoleIdCancellationTokenSource?.Cancel();
        }
        return ret;
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            GetGroupsByRoleIdCancellationTokenSource?.Cancel();
            GetGroupsByRoleIdCancellationTokenSource?.Dispose();

            GetGroupsByUserIdCancellationTokenSource?.Cancel();
            GetGroupsByUserIdCancellationTokenSource?.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
