using Google.Cloud.Firestore;

namespace new_timesheet_manager_server.Models
{
    public class UserModel : BaseModel<User>
    {
        public UserModel(FirestoreDb db) : base(db, "users")
        {
        }
    }
}
