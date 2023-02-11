using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaDAL.Data
{
    public class ApplicationRole: IdentityRole
    {
        public string RoleDescription { get; set; }
    }
}
