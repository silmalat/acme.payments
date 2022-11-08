using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleReader.WorkScheduleValidation
{
    public class InsufficientSetsOfDataException : Exception, IPaymentCalculatorException
    {
        public InsufficientSetsOfDataException(int minimumSets, int setsFound)
            : base($"At least {minimumSets} of data are required. Found={setsFound}")
        {
        }
    }
}
