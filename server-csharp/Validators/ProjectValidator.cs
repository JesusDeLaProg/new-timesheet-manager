
using ProtoValidate;

namespace new_timesheet_manager_server.Validators;

public class ProjectValidator(ILogger<ProjectValidator> logger, IValidator validator) : BaseValidator(logger, validator)
{
}
