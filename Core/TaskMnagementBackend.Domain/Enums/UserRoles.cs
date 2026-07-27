using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Domain.Enums
{
    public static class UserRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";
        public const string TeamLead = "TeamLead";
        public const string CompanyOwner = "CompanyOwner";

        public static readonly string[] All =
        {
            SuperAdmin,
            Admin,
            User,
            TeamLead,
            CompanyOwner
        };
    }
}
