using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.PaymentRatesProvider;

public class HardcodedPaymentRatesProvider : IPaymentRatesProvider
{
    public IEnumerable<PaymentRate> Get()
    {

        var payRate1 = (DayOfWeek DayOfTheWeek) =>
        {
            return new PaymentRate[] {
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(0, 1), EndTime = new TimeOnly(9, 0), Rate = 25.0M },
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(9, 1), EndTime = new TimeOnly(18, 0), Rate = 15.0M },
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(18, 1), EndTime = new TimeOnly(0, 0), Rate = 20.0M }
            };
        };

        var payRate2 = (DayOfWeek DayOfTheWeek) =>
        {
            return new PaymentRate[] {
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(0, 1), EndTime = new TimeOnly(9, 0), Rate = 30.0M },
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(9, 1), EndTime = new TimeOnly(18, 0), Rate = 20.0M },
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(18, 1), EndTime = new TimeOnly(0, 0), Rate = 25.0M }
            };
        };

        var paymentRates = new List<PaymentRate>();
        paymentRates.AddRange(payRate1(DayOfWeek.Monday));
        paymentRates.AddRange(payRate1(DayOfWeek.Tuesday));
        paymentRates.AddRange(payRate1(DayOfWeek.Wednesday));
        paymentRates.AddRange(payRate1(DayOfWeek.Thursday));
        paymentRates.AddRange(payRate1(DayOfWeek.Friday));
        paymentRates.AddRange(payRate2(DayOfWeek.Saturday));
        paymentRates.AddRange(payRate2(DayOfWeek.Sunday));

        return paymentRates;
    }
}
