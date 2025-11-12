using Google.Protobuf;
using Google.Type;
using System.Collections.Generic;
using System;
using System.Linq;

namespace new_timesheet_manager_server.Validators;

public class TimesheetValidator(ILogger<TimesheetValidator> logger, IConfiguration configuration) : BaseValidator(logger)
{
    private readonly IConfiguration _configuration = configuration;

    public override List<ValidationError> ValidateMessage(IMessage message)
    {
        if (message is not Timesheet pb)
        {
            throw new Exceptions.InvalidMessageTypeException("Message is not a Timesheet");
        }

        var violations = new List<ValidationError>();

        if (pb.EmployeeId.IsEmpty)
        {
            violations.Add(new ValidationError { Path = "employee_id", Error = "Employee ID is required." });
        }

        if (pb.Begin == null)
        {
            violations.Add(new ValidationError { Path = "begin", Error = "Begin date is required." });
        }

        if (pb.End == null)
        {
            violations.Add(new ValidationError { Path = "end", Error = "End date is required." });
        }

        if (pb.Lines.Count == 0)
        {
            violations.Add(new ValidationError { Path = "lines", Error = "At least one line is required." });
        }

        violations.AddRange(ValidateUniqueLines(pb));
        violations.AddRange(ValidateDates(pb));
        violations.AddRange(ValidateLines(pb));
        violations.AddRange(ValidateTravels(pb));

        return violations;
    }

    private static IEnumerable<ValidationError> ValidateUniqueLines(Timesheet timesheet)
    {
        var lines = timesheet.Lines.Select(l => l.ProjectId + ":" + l.PhaseId + ":" + l.ActivityId);
        if (lines.Count() != lines.Distinct().Count())
        {
            return
            [
                new ValidationError
                {
                    Path = "lines",
                    Error = "Lines must be unique"
                }
            ];
        }
        return [];
    }

    private static IEnumerable<ValidationError> ValidateDates(Timesheet timesheet)
    {
        var violations = new List<ValidationError>();

        if (timesheet.Begin == null || timesheet.End == null)
        {
            return violations;
        }

        var startDate = timesheet.Begin.ToDateTime();
        var endDate = timesheet.End.ToDateTime();

        if (startDate.DayOfWeek != System.DayOfWeek.Sunday)
        {
            violations.Add(new ValidationError { Path = "begin", Error = "Start date must be a Sunday" });
        }

        if (endDate.DayOfWeek != System.DayOfWeek.Saturday)
        {
            violations.Add(new ValidationError { Path = "end", Error = "End date must be a Saturday" });
        }

        if ((endDate - startDate).TotalDays != 13)
        {
            violations.Add(new ValidationError { Path = "end", Error = "Timesheet must span 2 full weeks" });
        }

        return violations;
    }

    private static IEnumerable<ValidationError> ValidateLines(Timesheet timesheet)
    {
        var violations = new List<ValidationError>();
        if (timesheet.Begin == null || timesheet.End == null)
        {
            return violations;
        }
        var startDate = timesheet.Begin.ToDateTime();
        var endDate = timesheet.End.ToDateTime();

        for (var i = 0; i < timesheet.Lines.Count; i++)
        {
            var line = timesheet.Lines[i];

            if (line.ProjectId.IsEmpty)
            {
                violations.Add(new ValidationError { Path = $"lines.{i}.project_id", Error = "Project ID is required." });
            }
            if (line.PhaseId.IsEmpty)
            {
                violations.Add(new ValidationError { Path = $"lines.{i}.phase_id", Error = "Phase ID is required." });
            }
            if (line.ActivityId.IsEmpty)
            {
                violations.Add(new ValidationError { Path = $"lines.{i}.activity_id", Error = "Activity ID is required." });
            }

            if (line.Entries.Count != 14)
            {
                violations.Add(new ValidationError { Path = $"lines.{i}.entries", Error = "There must be exactly 14 entries." });
            }

            var entryDates = line.Entries.Select(e => e.Date.ToDateTime());

            if (entryDates.Count() != entryDates.Distinct().Count())
            {
                violations.Add(new ValidationError { Path = $"lines.{i}.entries", Error = "Entry dates must be unique" });
            }

            foreach (var entry in line.Entries)
            {
                if (entry.Date == null)
                {
                    violations.Add(new ValidationError { Path = $"lines.{i}.entries", Error = "Entry date is required" });
                    continue;
                }
                var entryDate = entry.Date.ToDateTime();
                if (entryDate < startDate || entryDate > endDate)
                {
                    violations.Add(new ValidationError { Path = $"lines.{i}.entries", Error = "Entry date must be within the timesheet's date range" });
                }

                if (entry.Time != null && entry.Time.ToTimeSpan() < TimeSpan.Zero)
                {
                    violations.Add(new ValidationError { Path = $"lines.{i}.entries", Error = "Entry time must be positive" });
                }
            }
        }

        return violations;
    }

    private static IEnumerable<ValidationError> ValidateTravels(Timesheet timesheet)
    {
        var violations = new List<ValidationError>();
        if (timesheet.Begin == null || timesheet.End == null)
        {
            return violations;
        }
        var startDate = timesheet.Begin.ToDateTime();
        var endDate = timesheet.End.ToDateTime();

        for (var i = 0; i < timesheet.Travels.Count; i++)
        {
            var travel = timesheet.Travels[i];

            if (travel.ProjectId.IsEmpty)
            {
                violations.Add(new ValidationError { Path = $"travels.{i}.project_id", Error = "Project ID is required." });
            }
            if (travel.Date == null)
            {
                violations.Add(new ValidationError { Path = $"travels.{i}.date", Error = "Travel date is required." });
            }
            else
            {
                var travelDate = travel.Date.ToDateTime();
                if (travelDate < startDate || travelDate > endDate)
                {
                    violations.Add(new ValidationError { Path = $"travels.{i}.date", Error = "Travel date must be within the timesheet's date range" });
                }
            }

            if (string.IsNullOrEmpty(travel.From))
            {
                violations.Add(new ValidationError { Path = $"travels.{i}.from", Error = "From is required." });
            }
            if (string.IsNullOrEmpty(travel.To))
            {
                violations.Add(new ValidationError { Path = $"travels.{i}.to", Error = "To is required." });
            }

            var distance = travel.Distance;
            var hasDistance = distance != null && (distance.Units > 0 || distance.Nanos > 0);
            if (!hasDistance && travel.Expenses.Count == 0)
            {
                violations.Add(new ValidationError { Path = $"travels.{i}", Error = "A travel must have a distance or at least one expense." });
            }

            if (distance != null)
            {
                if (distance.Unit == Distance.Types.Unit.Unspecified)
                {
                    violations.Add(new ValidationError { Path = $"travels.{i}.distance.unit", Error = "Distance unit must be specified." });
                }
            }

            for (var j = 0; j < travel.Expenses.Count; j++)
            {
                var expense = travel.Expenses[j];
                if (string.IsNullOrEmpty(expense.Description))
                {
                    violations.Add(new ValidationError { Path = $"travels.{i}.expenses.{j}.description", Error = "Expense description is required." });
                }
                if (expense.Amount == null)
                {
                    violations.Add(new ValidationError { Path = $"travels.{i}.expenses.{j}.amount", Error = "Expense amount is required." });
                }
                else if (expense.Amount.Units <= 0 && expense.Amount.Nanos <= 0)
                {
                    violations.Add(new ValidationError { Path = $"travels.{i}.expenses.{j}.amount", Error = "amount should be a positive amount" });
                }
            }
        }

        return violations;
    }
}