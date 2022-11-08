using ACME.PaymentCalculator.WorkScheduleReader.WorkScheduleValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleReader
{
    public abstract class WorkScheduleReader<T>
    {
        private readonly IEnumerable<IWorkScheduleValidator<T>> _dataValidators;
        public WorkScheduleReader()
            :this(Enumerable.Empty<IWorkScheduleValidator<T>>())
        {
        }

        public WorkScheduleReader(IEnumerable<IWorkScheduleValidator<T>> dataValidators)
        {
            _dataValidators = dataValidators;
        }

        public IEnumerable<T> Read()
        {
            var data = ReadImplementation();
            ValidateData(data);
            return data;
        }

        private void ValidateData(IEnumerable<T> data)
        {
            foreach(var validator in _dataValidators)
            {
                validator.ValidateWorkScheduleData(data);
            }
        }

        protected abstract IEnumerable<T> ReadImplementation();
    }
}
