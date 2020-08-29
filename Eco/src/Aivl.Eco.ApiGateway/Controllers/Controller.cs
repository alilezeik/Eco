namespace Eco.ApiGateway.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public abstract class Controller : ControllerBase
    {
        protected UserInfo CurrentUserInfo
        {
            get
            {
                var userInfo = new UserInfo();

                var claims = HttpContext.User.Claims.ToList();

                userInfo.UserName = claims.FirstOrDefault(c => c.Type == ClaimsKeys.UserName)?.Value;

                userInfo.Email = claims.FirstOrDefault(c => c.Type == ClaimsKeys.Email)?.Value;

                var roles = claims.FirstOrDefault(c => c.Type == ClaimsKeys.Roles)?.Value;


                if (!string.IsNullOrEmpty(roles))
                {
                    userInfo.Roles = roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                return userInfo;
            }
        }
    }
}
