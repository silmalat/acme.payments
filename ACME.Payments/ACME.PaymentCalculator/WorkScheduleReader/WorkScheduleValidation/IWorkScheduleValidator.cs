using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleReader.WorkScheduleValidation
{
    public interface IWorkScheduleValidator<T>
    {
        void ValidateWorkScheduleData(IEnumerable<T> data);
    }
}
