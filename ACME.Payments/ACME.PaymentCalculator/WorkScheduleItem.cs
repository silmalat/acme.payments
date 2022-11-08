using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator
{
    public class WorkScheduleItem
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is WorkScheduleItem item)
            {
                return this.DayOfWeek.Equals(item.DayOfWeek)
                    && this.StartTime.Equals(item.StartTime)
                    && this.EndTime.Equals(item.EndTime);
            }
            return false;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
