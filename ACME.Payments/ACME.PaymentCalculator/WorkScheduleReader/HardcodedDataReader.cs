using ACME.PaymentCalculator.WorkScheduleReader.WorkScheduleValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleReader
{
    public class HardcodedDataReader : WorkScheduleReader<string>
    {
        public HardcodedDataReader(IEnumerable<IWorkScheduleValidator<string>> workScheduleValidators)
            : base(workScheduleValidators)
        {
        }

        protected override IEnumerable<string> ReadImplementation()
        {
            return new string[]
            {
                "EMP=MO09:01-10:00",
                "EMP240=MO10:00-00:00",
                //"EMP25=MO08:00-09:00",
                //"EMP40=MO08:00-10:00",
                //"RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00",
                //"ASTRID=MO10:00-12:00,TH12:00-14:00,SU20:00-21:00",
                "ASTRID=MO10:00-12:00,TH12:00-14:00,SU20:00-21:00",
                "ASTRID=MO10:00-12:00,TH12:00-14:00,SU20:00-21:00"
            };
        }
    }
}
