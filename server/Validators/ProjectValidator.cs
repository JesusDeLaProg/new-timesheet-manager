
using System.Text.RegularExpressions;
using Google.Protobuf;
using System.Collections.Generic;

namespace new_timesheet_manager_server.Validators;

public class ProjectValidator(ILogger<ProjectValidator> logger) : BaseValidator(logger)
{
    public override List<ValidationError> ValidateMessage(IMessage message)
    {
        if (message is not Project pb)
        {
            throw new Exceptions.InvalidMessageTypeException("Message is not a Project");
        }

        var violations = new List<ValidationError>();

        if (string.IsNullOrEmpty(pb.Code))
        {
            violations.Add(new ValidationError { Path = "code", Error = "Code is required." });
        }
        else if (!Regex.IsMatch(pb.Code, "^[0-9]{2,4}-[0-9]{2,}$"))
        {
            violations.Add(new ValidationError { Path = "code", Error = "Code must match pattern '^[0-9]{2,4}-[0-9]{2,}$'." });
        }

        if (string.IsNullOrEmpty(pb.Name))
        {
            violations.Add(new ValidationError { Path = "name", Error = "Name is required." });
        }

        if (pb.ClientId.IsEmpty)
        {
            violations.Add(new ValidationError { Path = "client_id", Error = "Client ID is required." });
        }

        if (pb.Type == Project.Types.Type.Unspecified)
        {
            violations.Add(new ValidationError { Path = "type", Error = "Project type must be specified." });
        }

        return violations;
    }
}
