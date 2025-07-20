
using ProtoValidate;

namespace new_timesheet_manager_server.Validators;

public class UserValidator(ILogger<UserValidator> logger, IValidator validator) : BaseValidator(logger, validator)
{
}
