using ContactDetails.API.Data;
using ContactDetails.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace ContactDetails.API.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactDetailsDbContext _contactDetailsDbContext;
        private readonly ILogger<ContactsController> _logger;
        private static Logger logger = LogManager.GetLogger("ContactsController");

        public ContactsController(ContactDetailsDbContext contactDetailsDbContext, ILogger<ContactsController> logger)
        {
            _contactDetailsDbContext = contactDetailsDbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            logger.Info("Get All Contacts Called");
            var contacts = await _contactDetailsDbContext.Contacts.ToListAsync();
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] Contact contactRequest)
        {
            logger.Info("Add Contacts Called");
            contactRequest.Id = Guid.NewGuid();
            await _contactDetailsDbContext.Contacts.AddAsync(contactRequest);
            await _contactDetailsDbContext.SaveChangesAsync();
            return Ok(contactRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            logger.Info("Get Contact Called");
            var contact = await _contactDetailsDbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
            if(contact== null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id,[FromBody] Contact updateContactRequest)
        {
            logger.Info("Update Contact Called");
            var contact = await _contactDetailsDbContext.Contacts.FindAsync(id);
            if(contact == null)
            {
                return NotFound();
            }
            contact.FirstName= updateContactRequest.FirstName;
            contact.LastName = updateContactRequest.LastName;
            contact.Email = updateContactRequest.Email;
            contact.PhoneNumber = updateContactRequest.PhoneNumber;
            contact.Address = updateContactRequest.Address;
            contact.City = updateContactRequest.City;
            contact.State = updateContactRequest.State;
            contact.Country = updateContactRequest.Country;
            contact.PostalCode = updateContactRequest.PostalCode;
            await _contactDetailsDbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            logger.Info("Delete Contact Called");
            var contact = await _contactDetailsDbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _contactDetailsDbContext.Contacts.Remove(contact);
            await _contactDetailsDbContext.SaveChangesAsync();

            return Ok(contact);
        }

    }
}
