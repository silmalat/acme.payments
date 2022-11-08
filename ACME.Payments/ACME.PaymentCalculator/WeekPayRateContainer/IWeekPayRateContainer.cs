using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WeekPayRateContainer;

public interface IWeekPayRateContainer
{
    public IEnumerable<PaymentRate> Get { get; }
}
