using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleReader.WorkScheduleValidation
{
    public class MinimumSetsInData<T> : IWorkScheduleValidator<T>
    {
        public const int DEFAULTMINIMUM = 5;
        private readonly int _minimumNumberOfSets;

        public MinimumSetsInData() 
            : this(DEFAULTMINIMUM)
        {
        }

        public MinimumSetsInData(int minimumNumberOfSets)
        {
            _minimumNumberOfSets = minimumNumberOfSets;
        }

        public void ValidateWorkScheduleData(IEnumerable<T> data)
        {
            // Validate request that input data should at least contain "_minimumNumberOfSets" entries
            if (data.Count() < _minimumNumberOfSets)
            {
                throw new InsufficientSetsOfDataException(_minimumNumberOfSets, data.Count());
            }
        }
    }
}
