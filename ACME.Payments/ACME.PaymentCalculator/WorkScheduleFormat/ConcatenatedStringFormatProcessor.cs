using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ACME.PaymentCalculator.WorkScheduleFormat;

public class ConcatenatedStringFormatProcessor : IWorkScheduleFormatProcessor<string>
{
    private static Dictionary<string, DayOfWeek> _dayOfWeekTRanslator = new Dictionary<string, DayOfWeek> {
        { "MO", DayOfWeek.Monday  },
        { "TU", DayOfWeek.Tuesday  },
        { "WE", DayOfWeek.Wednesday  },
        { "TH", DayOfWeek.Thursday },
        { "FR", DayOfWeek.Friday  },
        { "SA", DayOfWeek.Saturday  },
        { "SU", DayOfWeek.Sunday  }
    };


    public ConcatenatedStringFormatProcessor()
    {
    }

    public EmployeeWorkSchedule Process(string Elem)
    {
        // Process inputs strings of the format 
        // RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00

        // This would be nice to be parsed by a Regular Expression, however this is not a trivial RegEx,
        // so it will be quick to implement (and probably easy to understand/modify) by just manually splitting/parsing the input string

        // Split by "=" and validate that you got two strings from it
        var assigmentParts = Elem.Split('=', StringSplitOptions.TrimEntries);
        if (assigmentParts.Length != 2)
        {
            throw new InputStringFormatException(Elem, "Missing assignment");
        }

        var employeeName = assigmentParts[0];
        if (string.IsNullOrWhiteSpace(employeeName))
        {
            throw new InputStringFormatException(Elem, "Missing employee name");
        }

        var workScheduleString = assigmentParts[1];

        // Process the workSchedule string // Part similar to "MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00"
        // Split by "," and then process/validate each schedule item
        var scheduleItemsInput = workScheduleString.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var scheduleItemsOutput = scheduleItemsInput.Select(item => processScheduleItem(Elem, item));

        return new EmployeeWorkSchedule
        {
            EmployeeName = employeeName,
            WorkScheduleItems = scheduleItemsOutput
        };
    }

    private WorkScheduleItem processScheduleItem(string elem, string item)
    {
        // Create a Regular Expresion to parse each Schedule item
        var daysOfWeekKeys = _dayOfWeekTRanslator.Keys.Aggregate((c, a) => $"{a}|{c}");
        // schedItemRegexPattern should be "^(MO|TU|TH|FR|SA|SU)([0-9]{2}):([0-9]{2})-([0-9]{2}):([0-9]{2})$"
        var schedItemRegexPattern = $"^({daysOfWeekKeys})([0-9]{{2}}):([0-9]{{2}})-([0-9]{{2}}):([0-9]{{2}})$";
        var schedItemRegex = new Regex(schedItemRegexPattern);

        // Expression should find exactly one match -> error otherwise
        var matches = schedItemRegex.Matches(item);
        if (matches.Count != 1)
        {
            throw new InputStringFormatException(elem, $"Schedule Item wrong format [{item}]");
        }

        // Matched Groups should be 1=Day of week, 2=Start Hour, 3=Start Minute, 4=End Hour, 5=End Minutes
        var matchGroups = matches[0].Groups;
        return new WorkScheduleItem
        {
            DayOfWeek = _dayOfWeekTRanslator[matchGroups[1].Value],
            StartTime = new TimeOnly(Int32.Parse(matchGroups[2].Value), Int32.Parse(matchGroups[3].Value), 0),
            EndTime = new TimeOnly(Int32.Parse(matchGroups[4].Value), Int32.Parse(matchGroups[5].Value), 0)
        };
    }
}
