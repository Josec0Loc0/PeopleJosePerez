using PeopleJosePerez.Models;
using SQLite;

namespace PeopleJosePerez
{
    public class PersonRepository
    {
        string _dbPath;

        private SQLiteConnection conn;
        public string StatusMessage { get; set; }
        // TODO: Add variable for the SQLite connection
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<PersonJP>();
        }

        public PersonRepository(string dbPath)
        {
            _dbPath = dbPath;
        }
        public void AddNewPerson(string name)
        {
            int result = 0;
            try
            {
                Init();
                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");
                result = conn.Insert(new PersonJP { Name = name });

                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }
        }
        public List<PersonJP> GetAllPeople()
        {
            try
            {
                Init();
                return conn.Table<PersonJP>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
            return new List<PersonJP>();
        }

        public void DeletePerson(int id)
        {
            try
            {
                Init();
                var personToDelete = conn.Find<PersonJP>(id);
                if (personToDelete != null)
                {
                    conn.Delete(personToDelete);
                    StatusMessage = $"Registro con Id {id} eliminado.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar el registro: {ex.Message}";
            }
        }
    }
}
