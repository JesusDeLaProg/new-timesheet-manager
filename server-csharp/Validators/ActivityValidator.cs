
using ProtoValidate;

namespace new_timesheet_manager_server.Validators;

public class ActivityValidator(ILogger<ActivityValidator> logger, IValidator validator) : BaseValidator(logger, validator)
{
}
