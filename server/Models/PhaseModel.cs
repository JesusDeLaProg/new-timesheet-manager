using Google.Cloud.Firestore;

namespace new_timesheet_manager_server.Models
{
    public class PhaseModel : BaseModel<Phase>
    {
        public PhaseModel(FirestoreDb db) : base(db, "phases")
        {
        }
    }
}
