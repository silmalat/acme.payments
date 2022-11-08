using ACME.PaymentCalculator.WorkScheduleFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.UnitTests.WorkScheduleFormat.ConcatenatedStringFormatProcessorTest;

[TestClass]
public class ConcatenatedStringFormatProcessorTest
{
    [TestMethod]
    public void ValidInputTests()
    {
        // Setup
        var input1 = "RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00";
        var processor = new ConcatenatedStringFormatProcessor();
        var sched0 = new WorkScheduleItem { DayOfWeek = DayOfWeek.Monday, StartTime = new TimeOnly(10, 00), EndTime = new TimeOnly(12, 00) };
        var sched1 = new WorkScheduleItem { DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeOnly(10, 00), EndTime = new TimeOnly(12, 00) };
        var sched2 = new WorkScheduleItem { DayOfWeek = DayOfWeek.Thursday, StartTime = new TimeOnly(1, 00), EndTime = new TimeOnly(3, 00) };
        var sched3 = new WorkScheduleItem { DayOfWeek = DayOfWeek.Saturday, StartTime = new TimeOnly(14, 00), EndTime = new TimeOnly(18, 00) };
        var sched4 = new WorkScheduleItem { DayOfWeek = DayOfWeek.Sunday, StartTime = new TimeOnly(20, 00), EndTime = new TimeOnly(21, 00) };

        // Action
        var result = processor.Process(input1);

        // Assert
        Assert.AreEqual("RENE", result.EmployeeName);
        Assert.AreEqual(5, result.WorkScheduleItems.Count());

        var scheduleItems = result.WorkScheduleItems.ToArray();
        Assert.AreEqual(sched0, scheduleItems[0]);
        Assert.AreEqual(sched1, scheduleItems[1]);
        Assert.AreEqual(sched2, scheduleItems[2]);
        Assert.AreEqual(sched3, scheduleItems[3]);
        Assert.AreEqual(sched4, scheduleItems[4]);
    }

    [TestMethod]
    public void ValidSingleInputTests()
    {
        // Setup
        var input1 = "RENE=MO10:00-12:00";
        var processor = new ConcatenatedStringFormatProcessor();
        var sched0 = new WorkScheduleItem { DayOfWeek = DayOfWeek.Monday, StartTime = new TimeOnly(10, 00), EndTime = new TimeOnly(12, 00) };

        // Action
        var result = processor.Process(input1);

        // Assert
        Assert.AreEqual("RENE", result.EmployeeName);
        Assert.AreEqual(1, result.WorkScheduleItems.Count());

        var scheduleItems = result.WorkScheduleItems.ToArray();
        Assert.AreEqual(sched0, scheduleItems[0]);
    }

    [TestMethod]
    public void ValidNoSchedulesInputTests()
    {
        // Setup
        var input1 = "RENE=";
        var processor = new ConcatenatedStringFormatProcessor();

        // Action
        var result = processor.Process(input1);

        // Assert
        Assert.AreEqual("RENE", result.EmployeeName);
        Assert.AreEqual(0, result.WorkScheduleItems.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(InputStringFormatException))]
    public void MissingNameTest()
    {
        // Setup
        var input1 = "=MO10:00-12:00";
        var processor = new ConcatenatedStringFormatProcessor();

        // Action - Should get an exception here
        processor.Process(input1);
    }   
    
    [TestMethod]
    [ExpectedException(typeof(InputStringFormatException))]
    public void MissingEqualTest()
    {
        // Setup
        var input1 = "MO10:00-12:00";
        var processor = new ConcatenatedStringFormatProcessor();

        // Action - Should get an exception here
        processor.Process(input1);
    }


    [TestMethod]
    [ExpectedException(typeof(InputStringFormatException))]
    public void WrongDateTest()
    {
        // Setup
        var input1 = "RENE=XX10:00-12:00";
        var processor = new ConcatenatedStringFormatProcessor();

        // Action - Should throw an exception here
        var _ = processor.Process(input1).WorkScheduleItems.ToArray();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void WrongTimeTest()
    {
        // Setup
        var input1 = "RENE=FR25:00-12:00";
        var processor = new ConcatenatedStringFormatProcessor();

        // Action 
        var result = processor.Process(input1);
        // - Should throw an exception here
        Assert.AreEqual(0, result.WorkScheduleItems.Count());
    }

}
