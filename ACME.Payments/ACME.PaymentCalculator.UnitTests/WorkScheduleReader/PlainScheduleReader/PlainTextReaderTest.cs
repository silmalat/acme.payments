using ACME.PaymentCalculator.WorkScheduleReader;
using ACME.PaymentCalculator.WorkScheduleReader.WorkScheduleValidation;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace ACME.PaymentCalculator.UnitTests.WorkScheduleReader.PlainScheduleReader;

[TestClass]
public class PlainTextReaderTest
{
    [TestMethod]
    public void TestNormalFile()
    {
        // Setup
        var workScheduleValidators = new IWorkScheduleValidator<string>[] { new MinimumSetsInData<string>() };
        var filename = @"C:\employeedata\normal.txt";
        var mockFileSystem = new MockFileSystem();
        var mockInputFile = new MockFileData(@"RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
            JOHN=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
            MARIA=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
            JANE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
            JILL=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00");
        mockFileSystem.AddFile(filename, mockInputFile);

        // Action
        var reader = new PlainTextReader((FileInfoBase)new FileInfo(filename), mockFileSystem, workScheduleValidators);
        var schedules = reader.Read();

        // Assert
        Assert.AreEqual(5, schedules.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(InsufficientSetsOfDataException))]
    public void TestFileContainsAtLeast5Lines()
    {
        // Setup
        var workScheduleValidators = new IWorkScheduleValidator<string>[] { new MinimumSetsInData<string>() };
        var filename = @"C:\employeedata\normal.txt";
        var mockFileSystem = new MockFileSystem();
        var mockInputFile = new MockFileData(@"RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
            MARIA=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
            JANE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
            JILL=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00");
        mockFileSystem.AddFile(filename, mockInputFile);

        // Action
        var reader = new PlainTextReader(mockFileSystem.FileInfo.FromFileName(filename), mockFileSystem, workScheduleValidators);
        var schedules = reader.Read();

        // Assert - Should throw exception before this line
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestMissingFile()
    {
        // Setup
        var workScheduleValidators = new IWorkScheduleValidator<string>[] { new MinimumSetsInData<string>() };
        var mockFileSystem = new MockFileSystem();

        // Action
        var reader = new PlainTextReader(mockFileSystem.FileInfo.FromFileName(@"C:\dummy\missing.txt"), mockFileSystem, workScheduleValidators);
        var schedules = reader.Read();

        // Assert
        // See catch exception attribute
    }

    [TestMethod]
    public void TestFileWithEmptyLines()
    {
        // Setup
        var workScheduleValidators = new IWorkScheduleValidator<string>[] { new MinimumSetsInData<string>() };
        var filename = @"C:\employeedata\normal.txt";
        var mockFileSystem = new MockFileSystem();
        var mockInputFile = new MockFileData(@"
            RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
            JOHN=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00

            MARIA=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
            JANE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00

            JILL=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00
        ");
        mockFileSystem.AddFile(filename, mockInputFile);

        // Action
        var reader = new PlainTextReader(mockFileSystem.FileInfo.FromFileName(filename), mockFileSystem, workScheduleValidators);
        var schedules = reader.Read();

        // Assert
        Assert.AreEqual(5, schedules.Count());
    }
}