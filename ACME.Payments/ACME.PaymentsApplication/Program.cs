using ACME.PaymentCalculator;
using ACME.PaymentCalculator.PaymentRatesProvider;
using ACME.PaymentCalculator.WeekPayRateContainer;
using ACME.PaymentCalculator.WorkScheduleFormat;
using ACME.PaymentCalculator.WorkScheduleReader;
using ACME.PaymentCalculator.WorkScheduleReader.WorkScheduleValidation;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

Console.WriteLine("ACME Employee Payments Calculator");
Console.WriteLine(new string('-', 80));

if (args?.Length != 1)
{
    Console.WriteLine("\nWrong arguments\nUsage PaymentsCalculator.exe <filename>");
}
else
{
    // Register Services for the App
    IServiceProvider _svcProvider = RegisterServices(args[0]);

    try
    {
        // Get the calculator and compute payments
        var paymentCalculator = _svcProvider.GetService<PaymentCalculator<string>>();
        var payments = paymentCalculator.CalculatePayments();

        foreach (var payment in payments)
        {
            Console.WriteLine($"The amount to pay {payment.EmployeeName} is: {payment.AmountToPay:F2} USD");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error - {e.Message} {(e is IPaymentCalculatorException ? string.Empty : e.StackTrace)}");
    }
}
Console.WriteLine("Press a key to exit...");
Console.ReadKey();

static IServiceProvider RegisterServices(string inputFilePath)
{
    var svcs = new ServiceCollection();

    svcs.AddScoped<IFileInfo>(s => (FileInfoBase)new FileInfo(inputFilePath));
    svcs.AddScoped<IFileSystem, FileSystem>();
    svcs.AddScoped<IEnumerable<IWorkScheduleValidator<string>>>(s => new IWorkScheduleValidator<string>[] { new MinimumSetsInData<string>() });
    svcs.AddScoped<IWeekPayRateContainer, WeekPayRate>();
    svcs.AddScoped<IPaymentRatesProvider, HardcodedPaymentRatesProvider>();
    svcs.AddScoped<WorkScheduleReader<string>, PlainTextReader>();
    svcs.AddScoped<IWorkScheduleFormatProcessor<string>, ConcatenatedStringFormatProcessor>();
    svcs.AddScoped<PaymentCalculator<string>>();

    return svcs.BuildServiceProvider();
}