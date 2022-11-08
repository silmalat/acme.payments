using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.PaymentRatesProvider
{
    public class CollectionPaymentRatesProvider : IPaymentRatesProvider
    {
        private readonly IEnumerable<PaymentRate> _paymentRates;

        public CollectionPaymentRatesProvider(IEnumerable<PaymentRate> paymentRates)
        {
            _paymentRates = paymentRates;
        }
        public IEnumerable<PaymentRate> Get()
        {
            return _paymentRates;
        }
    }
}
