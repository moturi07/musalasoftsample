using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MusalaDAL.Helpers
{
    public static class Helper
    {
        public static string GetUserId(this HttpContext context)
        {
            return context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
