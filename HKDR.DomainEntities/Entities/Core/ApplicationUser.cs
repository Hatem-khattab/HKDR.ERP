using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HKDR.DomainEntities.Entities.Core
{
    public class ApplicationUser : IdentityUser 
    {
        public int CompanyId { get; set; }          // ⭐ أهم Property

        // 👤 Profile Info
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }

        // 🧠 Status / Control
        public bool IsActive { get; set; } = true;
        public bool IsCompanyAdmin { get; set; }    // Owner / Admin للشركة

        // 🌍 Localization
        public string? PreferredLanguage { get; set; } // ar / en
        public string? TimeZone { get; set; }           // Asia/Amman

        // 🕒 Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }
}
