
using ProtoValidate;

namespace new_timesheet_manager_server.Validators;

public class EmployeeValidator(ILogger<EmployeeValidator> logger, IValidator validator) : BaseValidator(logger, validator)
{
}
