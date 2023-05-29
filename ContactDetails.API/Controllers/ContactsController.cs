using ContactDetails.API.Data;
using ContactDetails.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactDetails.API.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactDetailsDbContext _contactDetailsDbContext;

        public ContactsController(ContactDetailsDbContext contactDetailsDbContext)
        {
            _contactDetailsDbContext = contactDetailsDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _contactDetailsDbContext.Contacts.ToListAsync();
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] Contact contactRequest)
        {
            contactRequest.Id = Guid.NewGuid();
            await _contactDetailsDbContext.Contacts.AddAsync(contactRequest);
            await _contactDetailsDbContext.SaveChangesAsync();
            return Ok(contactRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
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
