using HKDR.DomainEntities.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.DomainEntities.Entities.HR
{
    public class Company : BaseEntity
    {
        // 🔑 Identifiers
        public int Id { get; set; }

        // 🏷️ Basic Info
        public string Name { get; set; } = null!;
        public string? LegalName { get; set; }        // الاسم القانوني
        public string? RegistrationNumber { get; set; } // رقم السجل التجاري
        public string? TaxNumber { get; set; }        // الرقم الضريبي

        // 📞 Contact Info
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }

        // 📍 Address
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }

        // ⚙️ SaaS / System
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // 🧾 Subscription (لـ SaaS لاحقًا)
        public string? SubscriptionPlan { get; set; }   // Free, Basic, Pro
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsTrial { get; set; }

        // 🎨 UI / Localization
        public string? PreferredLanguage { get; set; }  // ar, en
        public string? TimeZone { get; set; }            // Asia/Amman
        public string? LogoUrl { get; set; }

        // 👤 Audit
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
