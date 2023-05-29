using ContactDetails.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactDetails.API.Data
{
    public class ContactDetailsDbContext: DbContext
    {
        public ContactDetailsDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
