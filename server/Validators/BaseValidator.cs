
using Google.Protobuf;
using ProtoValidate;

namespace new_timesheet_manager_server.Validators;

public class BaseValidator(ILogger logger, IValidator validator)
{
    private readonly ILogger _logger = logger;
    protected readonly IValidator Validator = validator;

    protected ValidationResult ValidateMessage(IMessage message)
    {
        using (_logger.BeginScope("Validating message using ProtoValidate: {Message}", message))
        {
            return Validator.Validate(message, false);
        }
    }

}