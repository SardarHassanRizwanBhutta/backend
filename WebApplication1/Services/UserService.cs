using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class UserService
    {
        // Here the DatabaseContext services is requested, which is used to call different DatabaseContext methods
        // Does not need to create instance of DatabaseContext, it is created by the DI containers
        private readonly DatabaseContext _context; // declaring a reference variable of type DatabaseContext

        public UserService(DatabaseContext context)
        {
            _context = context; // defining reference variable and assigning it to the context reference variable
        }

    //    discarding ActionResult as this will have extra key value pairs
    // we can use List<User> when fetching users

        public async Task<User?> GetUserByEmail(string email) {
           var result = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u=> u.Email == email);
           return result;
        }
        public async Task<IEnumerable<User>> GetUsers() {
            return await _context.Users.AsNoTracking().Include(u => u.Role).ToListAsync();
        }

        public async Task<User?> GetUserById(long id) {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> AddUser(User user) {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUser(User user) {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user) {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}
