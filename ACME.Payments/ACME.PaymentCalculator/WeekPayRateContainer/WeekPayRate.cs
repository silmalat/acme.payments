using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACME.PaymentCalculator.PaymentRatesProvider;

namespace ACME.PaymentCalculator.WeekPayRateContainer
{
    /// <summary>
    /// It contains (and validates) a full week payment segments
    /// </summary>
    public class WeekPayRate : IWeekPayRateContainer
    {
        private readonly IEnumerable<PaymentRate> _rates;

        public WeekPayRate(IPaymentRatesProvider ratesProvider)
        {
            _rates = ratesProvider.Get();
            // Create additional entries for segments spanning thru mid-night
            _rates = createAdditionalEntriesAfterMidnight(_rates);
            // Validate rates
            //ValidateRates(_rates);
        }

        public IEnumerable<PaymentRate> Get => _rates;

        private void ValidateRates(IEnumerable<PaymentRate> rates)
        {
            // Check that we have information for all the week and that each day has the 24 hours rate especified
            var ratesByDay = rates.GroupBy(r => r.DayOfWeek);
            if (ratesByDay.Count() != 7)
            {
                throw new WeekPayRateContainerException($"Missing information for full week payments. Expected 7 days received {ratesByDay.Count()}.");
            }


            var aaaaaaaaa1 = ratesByDay.First().Select(r => (r.EndTime - r.StartTime).TotalHours);
            var allSum24hoursValues = ratesByDay.Select(day => day.Select(r => (r.EndTime - r.StartTime).TotalHours).Sum());

            var allSum24hours = ratesByDay.Select(day => day.Select(r => (r.EndTime - r.StartTime).TotalHours).Sum().Equals(24.0)).All(v => v == true);
            if (!allSum24hours)
            {
                throw new WeekPayRateContainerException($"Missing information for day payment segment. Expected all days to have 24 hours especified.");
            }
        }

        private IEnumerable<PaymentRate> createAdditionalEntriesAfterMidnight(IEnumerable<PaymentRate> rates)
        {
            foreach (var rate in rates)
            {
                // Assuming that if StartTime > EndTime then the segment crossed midnigth
                if (rate.StartTime > rate.EndTime)
                {
                    yield return new PaymentRate { DayOfWeek = rate.DayOfWeek, StartTime = rate.StartTime, EndTime = new TimeOnly(23, 59), Rate = rate.Rate };
                    yield return new PaymentRate { DayOfWeek = rate.DayOfWeek, StartTime = new TimeOnly(0, 0), EndTime = rate.EndTime, Rate = rate.Rate };
                }
                else
                {
                    yield return rate;
                }
            }
        }
    }
}
