using System.Collections.Generic;
using System.Linq;
using MakeupReviewApp.Models;

namespace MakeupReviewApp.Repositories
{
    public class MockUserRepository
    {
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, FullName = "Alice Johnson", Email = "alice@example.com", Password = "password123" },
            new User { Id = 2, FullName = "Bob Smith", Email = "bob@example.com", Password = "securepass" },
            new User { Id= 3, FullName="Aless", Email="alesstongwen@gmail.com",   Password="123456"}
        };

        public User ValidateUser(string email, string password)
        {
            return _users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
