using Google.Cloud.Firestore;

namespace new_timesheet_manager_server.Models
{
    public class EmployeeModel : BaseModel<Employee>
    {
        public EmployeeModel(FirestoreDb db) : base(db, "employees")
        {
        }
    }
}
