
using Google.Protobuf;
using System.Collections.Generic;

namespace new_timesheet_manager_server.Validators;

public class ClientValidator(ILogger<ClientValidator> logger) : BaseValidator(logger)
{
    public override List<ValidationError> ValidateMessage(IMessage message)
    {
        if (message is not Client pb)
        {
            throw new Exceptions.InvalidMessageTypeException("Message is not a Client");
        }

        var violations = new List<ValidationError>();

        if (string.IsNullOrEmpty(pb.Name))
        {
            violations.Add(new ValidationError { Path = "name", Error = "Name is required." });
        }

        if (pb.DefaultProjectType == Project.Types.Type.Unspecified)
        {
            violations.Add(new ValidationError { Path = "default_project_type", Error = "Default project type must be specified." });
        }

        return violations;
    }
}
