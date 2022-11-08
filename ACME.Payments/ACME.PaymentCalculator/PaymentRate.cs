using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator;


/// <summary>
/// Defines the payment Rate for a Date/Start-End Time
/// </summary>
public class PaymentRate
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal Rate { get; set; }
}
