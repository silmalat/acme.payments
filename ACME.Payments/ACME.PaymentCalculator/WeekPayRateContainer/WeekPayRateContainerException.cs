using System.Runtime.Serialization;

namespace ACME.PaymentCalculator.WeekPayRateContainer
{
    [Serializable]
    internal class WeekPayRateContainerException : Exception, IPaymentCalculatorException
    {
        public WeekPayRateContainerException()
        {
        }

        public WeekPayRateContainerException(string message) : base(message)
        {
        }

        public WeekPayRateContainerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WeekPayRateContainerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}