
using Google.Protobuf;

using System.Collections.Generic;



namespace new_timesheet_manager_server.Validators;



public class BaseValidator

{

    protected readonly ILogger Logger;



    public BaseValidator(ILogger logger)

    {

        Logger = logger;

    }



    public virtual List<ValidationError> ValidateMessage(IMessage message)

    {

        return new List<ValidationError>();

    }

}


