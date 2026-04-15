using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.Infrastructure.Abstractions
{
    public interface ICurrentCompanyService
    {
        int CompanyId { get; }
    }
}
