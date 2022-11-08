using ACME.PaymentCalculator.WorkScheduleReader.WorkScheduleValidation;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleReader
{
    /// <summary>
    /// Reads a text file were its lines are employee work schedule data
    /// </summary>
    public class PlainTextReader : WorkScheduleReader<string>
    {
        private readonly string _scheduleFile;
        private readonly IFileSystem _fileSystem;

        public PlainTextReader(IFileInfo filename, IFileSystem fileSystem, IEnumerable<IWorkScheduleValidator<string>> workScheduleValidators) 
            : base(workScheduleValidators)
        {
            _scheduleFile = filename.FullName;
            _fileSystem = fileSystem;
        }

        protected override IEnumerable<string> ReadImplementation()
        {
            // Check that the file exists
            if (!_fileSystem.File.Exists(_scheduleFile))
            {
                throw new ArgumentException(nameof(_scheduleFile));
            }
            // Remove the empty lines before returning the data
            return _fileSystem.File.ReadAllLines(_scheduleFile).Where(l => !string.IsNullOrWhiteSpace(l));
        }
    }
}
