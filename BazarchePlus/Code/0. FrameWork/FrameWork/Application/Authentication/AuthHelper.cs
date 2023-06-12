﻿using System.Security.Claims;
using FrameWork.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace FrameWork.Application.Authentication
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public AuthViewModel CurrentAccountInfo()
        {
            var result = new AuthViewModel();
            if (!IsAuthenticated())
                return result;

            var claims = _contextAccessor.HttpContext.User.Claims.ToList();
            result.Id = long.Parse(claims.FirstOrDefault(x => x.Type == "AccountId")!.Value);
            result.Username = claims.FirstOrDefault(x => x.Type == "Username")!.Value;
            result.RoleId = long.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)!.Value);
            result.Fullname = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value;
            result.ProfilePicture = claims.FirstOrDefault(x => x.Type == "ProfilePicture")!.Value;
            result.Role = Roles.GetRoleBy(result.RoleId);
            return result;
        }

        public List<int> GetPermissions()
        {
            if (!IsAuthenticated())
                return new List<int>();

            var permissions = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "permissions")
                ?.Value;
            return JsonConvert.DeserializeObject<List<int>>(permissions!)!;
        }

        public long CurrentAccountId()
        {
            return IsAuthenticated()
                ? long.Parse(_contextAccessor.HttpContext.User.Claims.First(x => x.Type == "AccountId")?.Value!)
                : 0;
        }

        public string CurrentAccountMobile()
        {
            return (IsAuthenticated()
                ? _contextAccessor.HttpContext.User.Claims.First(x => x.Type == "Mobile")?.Value
                : "")!;
        }

        public string? CurrentAccountRole()
        {
            return IsAuthenticated() ? _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value! : null;
        }

        public bool IsAuthenticated()
        {
            return _contextAccessor.HttpContext.User.Identity!.IsAuthenticated;

        }

        public void Signin(AuthViewModel account)
        {
            var permissions = JsonConvert.SerializeObject(account.Permissions);
            var claims = new List<Claim>
            {
                new Claim("AccountId", account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Fullname),
                new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                new Claim("Username", account.Username), 
                new Claim("permissions", permissions),
                new Claim("Mobile", account.Mobile),
                new Claim("ProfilePicture", account.ProfilePicture)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
            };

            _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public void SignOut()
        {
            _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}