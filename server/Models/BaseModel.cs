using Google.Cloud.Firestore;
using Google.Protobuf;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace new_timesheet_manager_server.Models
{
    public abstract class BaseModel<T> where T : IMessage<T>, new()
    {
        protected readonly FirestoreDb _db;
        protected readonly CollectionReference _collection;

        public BaseModel(FirestoreDb db, string collectionName)
        {
            _db = db;
            _collection = _db.Collection(collectionName);
        }

        public virtual async Task<string> Create(T item)
        {
            var dictionary = ToDictionary(item);
            var document = await _collection.AddAsync(dictionary);
            return document.Id;
        }

        public virtual async Task<T> Get(string id)
        {
            var snapshot = await _collection.Document(id).GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return FromDictionary(snapshot.ToDictionary());
            }
            return default;
        }

        public virtual async Task Update(string id, T item)
        {
            await _collection.Document(id).SetAsync(ToDictionary(item), SetOptions.MergeAll);
        }

        public virtual async Task Delete(string id)
        {
            await _collection.Document(id).DeleteAsync();
        }

        public virtual async Task<Dictionary<string, T>> GetAll()
        {
            var snapshot = await _collection.GetSnapshotAsync();
            return snapshot.Documents.ToDictionary(doc => doc.Id, doc => FromDictionary(doc.ToDictionary()));
        }

        // Using JSON as an intermediary is a simple approach, but has limitations.
        // It doesn't handle some Firestore-specific types like Timestamp well for all Protobuf types.
        // For google.type.Date, this will require custom handling in derived classes if direct mapping fails.
        private static Dictionary<string, object> ToDictionary(IMessage item)
        {
            var json = JsonFormatter.Default.Format(item);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }

        private static T FromDictionary(IDictionary<string, object> dictionary)
        {
            var json = JsonSerializer.Serialize(dictionary);
            // This might fail for types like google.type.Date, which are stored as Timestamps in Firestore.
            // The Protobuf JSON parser expects a string for Date.
            // A more robust implementation would preprocess the dictionary to convert types.
            return JsonParser.Default.Parse<T>(json);
        }
    }
}
