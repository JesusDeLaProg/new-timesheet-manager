
using System.Text.RegularExpressions;
using Google.Protobuf;
using System.Collections.Generic;

namespace new_timesheet_manager_server.Validators;

public class ActivityValidator(ILogger<ActivityValidator> logger) : BaseValidator(logger)
{
    public override List<ValidationError> ValidateMessage(IMessage message)
    {
        if (message is not Activity pb)
        {
            throw new Exceptions.InvalidMessageTypeException("Message is not an Activity");
        }

        var violations = new List<ValidationError>();

        if (string.IsNullOrEmpty(pb.Code))
        {
            violations.Add(new ValidationError { Path = "code", Error = "Code is required." });
        }
        else if (!Regex.IsMatch(pb.Code, "^[a-zA-Z]{2}$"))
        {
            violations.Add(new ValidationError { Path = "code", Error = "Code must be 2 letters." });
        }

        if (string.IsNullOrEmpty(pb.Name))
        {
            violations.Add(new ValidationError { Path = "name", Error = "Name is required." });
        }
        else if (pb.Name.Length < 2)
        {
            violations.Add(new ValidationError { Path = "name", Error = "Name must be at least 2 characters long." });
        }

        return violations;
    }
}
