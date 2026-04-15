using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.DomainEntities.Entities.Core
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        // SaaS
        public int CompanyId { get; set; }

        // Auditing
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // Soft Delete
        public bool IsDeleted { get; set; } = false;
    }
}
