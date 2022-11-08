using ACME.PaymentCalculator.WeekPayRateContainer;
using ACME.PaymentCalculator.WorkScheduleFormat;
using ACME.PaymentCalculator.WorkScheduleReader;

namespace ACME.PaymentCalculator;

public class PaymentCalculator<T>
{
    private static TimeSpan _extraMinute = new TimeSpan(0, 1, 0);
    private readonly IEnumerable<PaymentRate> _paymentRates;
    private readonly WorkScheduleReader<T> _scheduleReader;
    private readonly IWorkScheduleFormatProcessor<T> _formatProcessor;

    public PaymentCalculator(IWeekPayRateContainer paymentRates, WorkScheduleReader<T> scheduleReader, IWorkScheduleFormatProcessor<T> formatProcessor)
    {
        _paymentRates = paymentRates.Get;
        _scheduleReader = scheduleReader;
        _formatProcessor = formatProcessor;
    }

    public IEnumerable<EmployeePayment> CalculatePayments(/*EmployeeWorkSchedule WorkSchedule*/)
    {
        // Read Schedules
        var scheduleItemsToProcess = _scheduleReader.Read();
        // Format readed schedules
        var employeeWorkSchedules = scheduleItemsToProcess.Select(ews => _formatProcessor.Process(ews));
        // Get payment for each employee work schedule
        var employeePayments = employeeWorkSchedules.Select(es => new EmployeePayment(
            es.EmployeeName,
            es.WorkScheduleItems.Select(i => CalculateSinglePayment(i)).Sum()
        ));
        return employeePayments;
    }


    private decimal calculateTotal(WorkScheduleItem workScheduleItem, PaymentRate payRate, bool segmentStartIncluded, bool segmentEndIncluded, bool segmentFullyIncluded)
    {
        var scenario1 = segmentStartIncluded && segmentEndIncluded;
        var scenario2 = (segmentStartIncluded && !segmentEndIncluded) || (!segmentStartIncluded && segmentEndIncluded);
        var scenario3 = segmentFullyIncluded;

        if (scenario1)
        {
            return payRate.Rate * ((decimal)(workScheduleItem.EndTime - workScheduleItem.StartTime).TotalHours);
        }
        else if (scenario2)
        {
            if (segmentStartIncluded)
            {
                // Consider an extra minute for the Pay Segment and Work Schedule EndTime limit comparison
                // (Pay=includes the end time (e.g <=)/Schedule considers as "lower than" (<))
                var elapsedTime = (payRate.EndTime - workScheduleItem.StartTime).Add(_extraMinute);
                return payRate.Rate * (decimal)elapsedTime.TotalHours;
            }
            else if (segmentEndIncluded)
            {
                return payRate.Rate * ((decimal)(workScheduleItem.EndTime - payRate.StartTime).TotalHours);
            }
        }
        else if (scenario3)
        {
            // Consider an extra minute for the Pay Segment and Work Schedule EndTime limit comparison
            // (Pay=includes the end time (e.g <=)/Schedule considers as "lower than" (<))
            var elapsedTime = (payRate.EndTime - payRate.StartTime).Add(_extraMinute); 
            return payRate.Rate * (decimal)elapsedTime.TotalHours;
        }
        return 0.0M;
    }

    private decimal CalculateSinglePayment(WorkScheduleItem workScheduleItem)
    {
        var ratesForDay = _paymentRates.Where(pr => pr.DayOfWeek == workScheduleItem.DayOfWeek);
        decimal total = 0.0M;

        foreach (var payRate in ratesForDay)
        {
            // Three cases to consider here:
            // Pay Rate          |----------|--------------|-----------------|
            // Work Scenario1      |----|
            // Work Scenario2                      |----------------|
            // Work Scenario3         |---------------------------------|


            var isFullDayJob = ((workScheduleItem.EndTime - workScheduleItem.StartTime).TotalHours == 0); // Special case => Job Schedule from 00:00 to 00:00

            var segmentStartIncluded = workScheduleItem.StartTime >= payRate.StartTime && workScheduleItem.StartTime <= payRate.EndTime && !isFullDayJob;
            var segmentEndIncluded = workScheduleItem.EndTime >= payRate.StartTime && workScheduleItem.EndTime <= payRate.EndTime && !isFullDayJob;
            var segmentFullyIncluded = workScheduleItem.StartTime < payRate.StartTime && workScheduleItem.EndTime > payRate.EndTime || isFullDayJob;

            total += calculateTotal(workScheduleItem, payRate, segmentStartIncluded, segmentEndIncluded, segmentFullyIncluded); ;
        }
        return total;
    }


}