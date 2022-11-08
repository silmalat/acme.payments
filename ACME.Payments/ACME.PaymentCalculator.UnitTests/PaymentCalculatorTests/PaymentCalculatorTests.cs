using ACME.PaymentCalculator.PaymentRatesProvider;
using ACME.PaymentCalculator.WeekPayRateContainer;
using ACME.PaymentCalculator.WorkScheduleFormat;
using ACME.PaymentCalculator.WorkScheduleReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.UnitTests.PaymentCalculatorTests;

[TestClass]
public class PaymentCalculatorTests
{
    [TestMethod]
    public void PaymentCalculationBasic()
    {
        // Setup 
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

        var employeeScheduleData = new string[]
        {
            "RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00",
            "ASTRID=MO10:00-12:00,TH12:00-14:00,SU20:00-21:00"
        };

        var weekPaymentRates = new WeekPayRate(new CollectionPaymentRatesProvider(paymentRates));
        WorkScheduleReader<string> paymentInputReader = new StringWorkScheduleReader(employeeScheduleData);

        IWorkScheduleFormatProcessor<string> formatProcessor = new ConcatenatedStringFormatProcessor();

        // Action
        var paymentCalculator = new PaymentCalculator<string>(weekPaymentRates, paymentInputReader, formatProcessor);
        var payments = paymentCalculator.CalculatePayments();

        // Assert
        Assert.AreEqual(employeeScheduleData.Length, payments.Count());
        Assert.AreEqual(215.0M, payments.First(p => p.EmployeeName.Equals("RENE"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(85.0M, payments.First(p => p.EmployeeName.Equals("ASTRID"))?.AmountToPay ?? 0.0M);
    }


    [TestMethod]
    public void PaymentCalculationSegments()
    {
        // Setup 
        var payRate = (DayOfWeek DayOfTheWeek) =>
        {
            return new PaymentRate[] {
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(2, 0), EndTime = new TimeOnly(8, 59), Rate = 5.0M },
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(9, 59), Rate = 20.0M },
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 59), Rate = 50.0M },
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(21, 59), Rate = 80.0M },
                new PaymentRate { DayOfWeek = DayOfTheWeek, StartTime = new TimeOnly(22, 0), EndTime = new TimeOnly(1, 59), Rate = 80.0M }
            };
        };

        var paymentRates = new List<PaymentRate>();
        paymentRates.AddRange(payRate(DayOfWeek.Monday));
        paymentRates.AddRange(payRate(DayOfWeek.Tuesday));
        paymentRates.AddRange(payRate(DayOfWeek.Wednesday));
        paymentRates.AddRange(payRate(DayOfWeek.Thursday));
        paymentRates.AddRange(payRate(DayOfWeek.Friday));
        paymentRates.AddRange(payRate(DayOfWeek.Saturday));
        paymentRates.AddRange(payRate(DayOfWeek.Sunday));

        var employeeScheduleData = new string[]
        {
            "EMP30MINUTES_UPPERBOUND=MO09:30-10:00",
            "EMP30MINUTES_INNER=MO09:01-09:31",
            "EMP30MINUTES_LOWERBOUND=MO09:00-09:30",
            "EMP_FULLRANGE=MO09:00-10:00",
            "EMP_DOUBLEFULLRANGE=MO09:00-11:00",
            "EMP_FULLRANGE_PLUSMIDDLE=MO09:00-10:30",
            "EMP_MIDDLE_PLUSFULLRANGE=MO09:30-11:00",
            "EMP_MIDDLE_TO_MIDDLE_INCLUDEINNERFULLRANGE=MO09:30-11:30",
            "EMP_CROSS_MIDNIGHT=MO23:00-01:00",
            "EMP_LOWERMIDNIGHT=MO23:00-00:00",
            "EMP_UPPPERMIDNIGHT=MO00:00-01:00",
            "EMP_ALLDAY=MO00:00-00:00",
        };

        var weekPaymentRates = new WeekPayRate(new CollectionPaymentRatesProvider(paymentRates));
        WorkScheduleReader<string> paymentInputReader = new StringWorkScheduleReader(employeeScheduleData);

        IWorkScheduleFormatProcessor<string> formatProcessor = new ConcatenatedStringFormatProcessor();

        // Action
        var paymentCalculator = new PaymentCalculator<string>(weekPaymentRates, paymentInputReader, formatProcessor);
        var payments = paymentCalculator.CalculatePayments();

        // Assert
        Assert.AreEqual(employeeScheduleData.Length, payments.Count());
        Assert.AreEqual(10.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP30MINUTES_INNER"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(10.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP30MINUTES_LOWERBOUND"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(10.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP30MINUTES_UPPERBOUND"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(20.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP_FULLRANGE"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(70.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP_DOUBLEFULLRANGE"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(45.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP_FULLRANGE_PLUSMIDDLE"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(60.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP_MIDDLE_PLUSFULLRANGE"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(100.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP_MIDDLE_TO_MIDDLE_INCLUDEINNERFULLRANGE"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(160.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP_CROSS_MIDNIGHT"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(80.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP_LOWERMIDNIGHT"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(80.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP_UPPPERMIDNIGHT"))?.AmountToPay ?? 0.0M);
        Assert.AreEqual(1305.0M, payments.FirstOrDefault(p => p.EmployeeName.Equals("EMP_ALLDAY"))?.AmountToPay ?? 0.0M);
    }
}