// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BootstrapAdmin.Api.Authencation;

/// <summary>
/// 
/// </summary>
public static class BootstrapAdminJwtHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static string CreateToken(string userName, Action<TokenValidateOption>? configure = null)
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity("Bearer");
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        TokenValidateOption tokenValidateOption = new TokenValidateOption();
        configure?.Invoke(tokenValidateOption);
        return jwtSecurityTokenHandler.CreateEncodedJwt(new SecurityTokenDescriptor
        {
            Issuer = tokenValidateOption.Issuer,
            Audience = tokenValidateOption.Audience,
            Subject = claimsIdentity,
            Expires = DateTime.Now.AddMinutes(tokenValidateOption.Expires),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValidateOption.SecurityKey)), "HS256")
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string CreateRefershToken()
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        TokenValidateOption tokenValidateOption = new TokenValidateOption();
        return jwtSecurityTokenHandler.CreateEncodedJwt(new SecurityTokenDescriptor
        {
            Issuer = tokenValidateOption.Issuer,
            Audience = tokenValidateOption.Audience,
            Expires = DateTime.Now.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValidateOption.SecurityKey)), "HS256")
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static bool ValidateTokenExpires(string token)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var parmeter = jwtSecurityTokenHandler.ReadJwtToken(token);
        var value = parmeter.Claims.FirstOrDefault(s => s.Type == ClaimTypes.Expired);

        return value!.Value != null;
    }
}
