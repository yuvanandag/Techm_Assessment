using ContactBook.API.Helper;
using ContactBook.API.Models;
using ContactBook.API.Models.Wrappers;
using ContactBook.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository contactRepository;
        private readonly IUriService uriService;

        public ContactsController(IContactRepository contactRepository, IUriService uriService)
        {
            this.contactRepository = contactRepository;
            this.uriService = uriService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                var result = await contactRepository.GetAllContactsAsync();
                if (result.IsSuccess)
                {
                    PaginationFilter filter = new PaginationFilter(1, 10);
                    var route = Request.Path.Value;
                    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                    var pagedData = result.Contacts.ToList()
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();

                    var totalRecords = pagedData.Count();
                    var pagedReponse = PaginationHelper.CreatePagedReponse<Contact>(pagedData, validFilter, totalRecords, uriService, route);
                    if (totalRecords > 0)
                        return Ok(pagedReponse);
                    return NotFound("Search Data is not available");
                }
                else
                    return NotFound(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactByIdAsync(int id)
        {
            try
            {
                var result = await contactRepository.GetContactByIdAsync(id);
                if (result.IsSuccess)
                    return Ok(result.Contact);
                return NotFound(result.ErrorMessage);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await contactRepository.CreateContactAsync(contact);
                    if (result.IsSuccess)
                        return Ok(result.Contact);
                    return StatusCode(409, "Contact already exists.");
                }
                return BadRequest("Please Provide Mandatory Fields");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateContactAsync(int id, [FromBody] Contact contact)
        {
            try
            {
                contact.Id = id;
                var result = await contactRepository.UpdateContactAsync(contact);
                if (result.IsSuccess)
                    return Ok(result.Contact);
                return NotFound(result.ErrorMessage);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactAsync(int id)
        {
            try
            {
                var result = await contactRepository.DeleteContactAsync(id);
                if (result.IsSuccess)
                    return Ok(result.Contact);
                return NotFound(result.ErrorMessage);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpGet]
        [Route("/api/ContactSearch/{contactSearch}")]
        public async Task<ActionResult> SearchRecord(string contactSearch)
        {
            if (!string.IsNullOrEmpty(contactSearch))
            {
                contactSearch = contactSearch.ToLower();
                var contactResult = await contactRepository.GetAllContactsAsync();
                if (contactResult.IsSuccess)
                {
                    PaginationFilter filter = new PaginationFilter(1, 10);
                    var route = Request.Path.Value;
                    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                    var contactList = contactResult.Contacts.ToList();
                    var pagedData = contactList.Where(a => a.FirstName.ToLower().Contains(contactSearch)
                            || a.LastName.ToLower().Contains(contactSearch)
                            || a.PhoneNumber.ToLower().Contains(contactSearch))
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();

                    var totalRecords = pagedData.Count();
                    var pagedReponse = PaginationHelper.CreatePagedReponse<Contact>(pagedData, validFilter, totalRecords, uriService, route);
                    if (totalRecords > 0)
                        return Ok(pagedReponse);
                    return NotFound("Search Data is not available");
                }
                return NotFound(contactResult.ErrorMessage);
            }
            else
                return BadRequest("contactSearch should provide");
        }
    }
}
