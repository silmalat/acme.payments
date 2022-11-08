using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.PaymentRatesProvider
{
    public interface IPaymentRatesProvider
    {
        IEnumerable<PaymentRate> Get();
    }
}
