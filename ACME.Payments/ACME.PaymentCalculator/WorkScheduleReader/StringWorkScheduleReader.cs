using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleReader
{
    public class StringWorkScheduleReader : WorkScheduleReader<string>
    {
        private readonly IEnumerable<string> _data;

        public StringWorkScheduleReader(IEnumerable<string> data)
        {
            _data = data;
        }
        protected override IEnumerable<string> ReadImplementation()
        {
            return _data;
        }
    }
}
