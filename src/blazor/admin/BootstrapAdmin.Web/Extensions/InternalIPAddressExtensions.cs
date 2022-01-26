// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.Net;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// IPAddress 内部操作扩展类
/// </summary>
internal static class InternalIPAddressExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public static string ToIPv4String(this IPAddress? address)
    {
        var ipv4Address = (address ?? IPAddress.IPv6Loopback).ToString();
        return ipv4Address.StartsWith("::ffff:") ? (address ?? IPAddress.IPv6Loopback).MapToIPv4().ToString() : ipv4Address;
    }
}
