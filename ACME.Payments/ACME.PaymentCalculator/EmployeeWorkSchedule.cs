using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator
{
    public class EmployeeWorkSchedule
    {
        public string EmployeeName { get; set; }
        public IEnumerable<WorkScheduleItem> WorkScheduleItems { get; set; }
    }
}
