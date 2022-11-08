using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleFormat
{
    public class InputStringFormatException : Exception, IPaymentCalculatorException
    {
        public InputStringFormatException(string InputString, string ProbableCause) 
            : base($"Employee work schedule input string format is wrong. {ProbableCause}. Received=[{InputString}]")
        {
        }
    }
}
