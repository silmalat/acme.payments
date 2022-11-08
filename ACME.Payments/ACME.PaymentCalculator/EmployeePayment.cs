using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator
{
    public record EmployeePayment(string EmployeeName, decimal AmountToPay);
}
