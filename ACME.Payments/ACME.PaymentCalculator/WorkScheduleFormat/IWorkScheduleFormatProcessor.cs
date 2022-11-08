using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleFormat
{
    public interface IWorkScheduleFormatProcessor<T>
    {
        /// <summary>
        /// Gets a WorkScheduleItem from any input representation of it
        /// </summary>
        /// <returns></returns>
        public EmployeeWorkSchedule Process(T elem);
    }
}
