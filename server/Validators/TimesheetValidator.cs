using Buf.Validate;
using Google.Protobuf;
using ProtoValidate;

namespace new_timesheet_manager_server.Validators;

public class TimesheetValidator(ILogger<TimesheetValidator> logger, IValidator validator, IConfiguration configuration) : BaseValidator(logger, validator)
{
    private readonly IConfiguration _configuration = configuration;

    protected override ValidationResult ValidateMessage(IMessage message)
    {
        if (message is Timesheet pb)
        {
            var errors = base.ValidateMessage(message);
            var extraErrors = [
                ValidateUniqueLines(pb)
            ];
            errors.Violations.AddRange(extraErrors.Where(e => e is not null));

            return errors;
        }
        else
        {
            throw new Exceptions.InvalidMessageTypeException("Message is not a Timesheet");
        }
    }

    private Violation? ValidateUniqueLines(Timesheet timesheet)
    {
        var lines = timesheet.Lines.Select(l => l.ProjectId + ":" + l.PhaseId + ":" + l.ActivityId);
        if (lines.Count() != lines.Distinct().Count())
        {
            return new Violation
            {
                ConstraintId = "lines.unique",
                Message = "Lines must be unique",
                FieldPath = "lines",
                ForKey = true
            };
        }
        return null;
    }
}