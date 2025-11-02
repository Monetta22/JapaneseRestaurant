using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace JapaneseRestaurant.Services
{
    public class UserService
    {
        private readonly string usersFile = Path.Combine(AppContext.BaseDirectory, "Data", "users.json");
        private List<UserRecord> _users = new();

        public UserService()
        {
            EnsureDataFile();
            Load();
        }

        // Ensure the directory and initial file (with default admin user) exist
        private void EnsureDataFile()
        {
            var dir = Path.GetDirectoryName(usersFile)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(usersFile))
            {
                _users = new List<UserRecord>
                {
                    new UserRecord { Username = "admin", PasswordHash = Hash("admin") }
                };
                Save();
            }
        }

        // Load users from JSON file
        private void Load()
        {
            try
            {
                var json = File.ReadAllText(usersFile);
                var list = JsonSerializer.Deserialize<List<UserRecord>>(json);
                if (list != null)
                    _users = list;
            }
            catch
            {
                _users = new();
            }
        }

        // Persist current users to disk
        private void Save()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(usersFile, json);
        }

        // Validate credentials
        public bool ValidateUser(string user, string password)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
                return false;

            var hash = Hash(password);
            return _users.Any(u => u.Username.Equals(user, StringComparison.OrdinalIgnoreCase)
                                   && u.PasswordHash == hash);
        }

        // Register a new user (returns false if username already exists)
        public bool RegisterUser(string user, string password)
        {
            if (_users.Any(u => u.Username.Equals(user, StringComparison.OrdinalIgnoreCase)))
                return false;

            _users.Add(new UserRecord { Username = user, PasswordHash = Hash(password) });
            Save();
            return true;
        }

        // Hash password with SHA256
        private string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        // Internal record representing a stored user
        private class UserRecord
        {
            public string Username { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
        }
    }
}