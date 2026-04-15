using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDR.DomainEntities.Entities.HR
{
    public class PayrollPeriod
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public bool IsClosed { get; set; }
    }

}
