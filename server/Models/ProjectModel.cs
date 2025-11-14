using Google.Cloud.Firestore;

namespace new_timesheet_manager_server.Models
{
    public class ProjectModel : BaseModel<Project>
    {
        public ProjectModel(FirestoreDb db) : base(db, "projects")
        {
        }
    }
}
