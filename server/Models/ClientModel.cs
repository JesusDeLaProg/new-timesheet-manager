using Google.Cloud.Firestore;

namespace new_timesheet_manager_server.Models
{
    public class ClientModel : BaseModel<Client>
    {
        public ClientModel(FirestoreDb db) : base(db, "clients")
        {
        }
    }
}
