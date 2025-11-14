using Google.Cloud.Firestore;

namespace new_timesheet_manager_server.Models
{
    public class ActivityModel : BaseModel<Activity>
    {
        public ActivityModel(FirestoreDb db) : base(db, "activities")
        {
        }
    }
}
