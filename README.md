# ACME Payments Processor
### Silvio Malatini - ioet coding exercise
#### Requirements
1. NET 6.0 (SDK for build)
2. Visual Studio 2022 (optional - you can use command line, see below)
#### How to build (CLI)
1. Access the folder where the project has been cloned (you should see "ACME.Payments.sln" file)
2. Run **dotnet build ACME.Payments.sln**
#### How to run the included sample (CLI)
1. Build
2. Access the "bin" folder of the Payment Application (**.\ACME.Payments\ACME.PaymentsApplication\bin\Debug\net6.0**)
3. Run **PaymentsCalculator samplepayments.txt**
#### How to run test suite (CLI)
1. Access the folder where the project has been cloned (you should see "ACME.Payments.sln" file)
2. Run **dotnet test ACME.Payments.sln**

#### Project summary
The solutions cosists of three projects
- ACME.PaymentCalculator (the library that implements the payment processor)
- ACME.PaymentCalculator.UnitTests (unit testing suite)
- ACME.PaymentsApplication (console application that uses the PaymentCalculator library to process payments)

The main entry point of the library is **PaymentCalculator** class, the constructor for this class receives 3 arguments
- an "IWeekPayRateContainer" a repository for the payment rates
- a "WorkScheduleReader" this is the provider of the employees schedules (this class is generic so it can be used with any "schedule item format" not just string)
- an "IWorkScheduleFormatProcessor" that is the class that parses and converts individual schedule items into "EmployeeWorkSchedule" instances

The PaymentCalculator **CalculatePayments** calculates the payments for the Work Schedules provided given the Pay Rates that the object has defined. The computation process
is performed by getting an Employee Work Schedule and for it checking each Payment Segment to determine if the work schedule start/end is outside, inside, includes partially or completely covers the payment segment. See chart below:
```            
            Three cases to consider here:
             Pay Rate          |----------|--------------|-----------------|
             Work Scenario1      |----|
             Work Scenario2                      |----------------|
             Work Scenario3         |---------------------------------|
```             


##### IPaymentRatesProvider: 
Interface that allows to implement the Payment Rates retrieval from any source, current implementation includes: 
1. **CollectionPaymentRatesProvider** it receives a collection of PaymentRate objects a data
2. **HardcodedPaymentRatesProvider** it contains the Payment Rates data provided as sample in the exercise hardcoded 

##### IWeekPayRateContainer: 
Defines a container for the week full set of Payment Rates. Current implementation **WeekPayRate** is able to properly handle input PaymentRate segments that crosses midnight.

##### IWorkScheduleFormatProcessor<T>: 
Defines how the input Work Schedules will be converted into "EmployeeWorkSchedule" objects. Current implementation **ConcatenatedStringFormatProcessor** handles the format provided as samples in the exercise.

##### IWorkScheduleValidator<T>: 
Defines validations over the input Work Schedules. Current implementation **MinimumSetsInData** validates (as required in the exercise) that the set contains at least 5 work schedules.

#### WorkScheduleReader<T>
Abstract class that reads and validates the employees work schedule items. 
Current implementation **PlainTextReader** reads the schedule items from a file, its constructor receives three parameters: the filename, an IFileSystem object (in order to
abstract the file system and allowing an easier Unit testing), and a list of **IWorkScheduleValidator** instances to perform validations on the input.
