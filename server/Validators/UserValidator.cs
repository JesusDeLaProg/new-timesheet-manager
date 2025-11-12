
using Google.Protobuf;
using System.Collections.Generic;

namespace new_timesheet_manager_server.Validators;

public class UserValidator(ILogger<UserValidator> logger) : BaseValidator(logger)
{
    public override List<ValidationError> ValidateMessage(IMessage message)
    {
        if (message is not User pb)
        {
            throw new Exceptions.InvalidMessageTypeException("Message is not a User");
        }

        var violations = new List<ValidationError>();

        if (string.IsNullOrEmpty(pb.Username))
        {
            violations.Add(new ValidationError { Path = "username", Error = "Username is required." });
        }
        else if (pb.Username.Length < 3)
        {
            violations.Add(new ValidationError { Path = "username", Error = "Username must be at least 3 characters long." });
        }

        if (pb.Role == User.Types.Role.Unspecified)
        {
            violations.Add(new ValidationError { Path = "role", Error = "User role must be specified." });
        }

        if (string.IsNullOrEmpty(pb.Email))
        {
            violations.Add(new ValidationError { Path = "email", Error = "Email is required." });
        }
        else
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(pb.Email);
                if (addr.Address != pb.Email)
                {
                    violations.Add(new ValidationError { Path = "email", Error = "Invalid email format." });
                }
            }
            catch
            {
                violations.Add(new ValidationError { Path = "email", Error = "Invalid email format." });
            }
        }

        return violations;
    }
}
