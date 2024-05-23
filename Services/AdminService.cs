using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Dictionar.Services
{
    public class AdminUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AdminService
    {
        private const string FilePath = "E:\\AN II Sem 2\\MVP\\Dictionar\\Data\\Admins.json"; // Update this path accordingly

        public bool IsValidAdmin(string username, string password)
        {
            if (!File.Exists(FilePath)) return false;

            var jsonData = File.ReadAllText(FilePath);
            var admins = JsonConvert.DeserializeObject<List<AdminUser>>(jsonData);

            return admins.Any(admin => admin.Username == username && admin.Password == password);
        }
    }

}
