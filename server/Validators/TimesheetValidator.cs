using ProtoValidate;

namespace new_timesheet_manager_server.Validators;

public class TimesheetValidator(ILogger<TimesheetValidator> logger, IValidator validator) : BaseValidator(logger, validator)
{
}