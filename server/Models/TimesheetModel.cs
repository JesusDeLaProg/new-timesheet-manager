using Google.Cloud.Firestore;

namespace new_timesheet_manager_server.Models
{
    public class TimesheetModel : BaseModel<Timesheet>
    {
        public TimesheetModel(FirestoreDb db) : base(db, "timesheets")
        {
        }
    }
}
