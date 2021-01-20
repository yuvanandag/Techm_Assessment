using ContactBook.API.Models;
using ContactBook.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using ContactBook.API.Models.Wrappers;
using ContactBook.API.Helper;

namespace ContactBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactGroupsController : ControllerBase
    {
        private readonly IContactGroupRepository contactGroupRepository;
        private readonly IUriService uriService;

        public ContactGroupsController(IContactGroupRepository contactRepository, IUriService uriService)
        {
            this.contactGroupRepository = contactRepository;
            this.uriService = uriService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContactGroups()
        {
            try
            {
                var result = await contactGroupRepository.GetContactGroupsAsync();
                if (result.IsSuccess)
                {
                    PaginationFilter filter = new PaginationFilter(1, 10);
                    var route = Request.Path.Value;
                    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                    var pagedData = result.ContactGroups.ToList()
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();

                    var totalRecords = pagedData.Count();
                    var pagedReponse = PaginationHelper.CreatePagedReponse<ContactGroup>(pagedData, validFilter, totalRecords, uriService, route);
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
        public async Task<IActionResult> GetContactById(int id)
        {
            try
            {
                var result = await contactGroupRepository.GetContactGroupByIdAsync(id);
                if (result.IsSuccess)
                    return Ok(result.ContactGroup);
                return NotFound(result.ErrorMessage);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactGroups([FromBody] ContactGroup contactGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await contactGroupRepository.CreateContactGroupAsync(contactGroup);
                    if (result.IsSuccess)
                        return Ok(result.ContactGroup);
                    return StatusCode(409, "Contact already exists.");
                }
                return BadRequest("Please Provide Mandatory Fields");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContactGroups([FromBody] ContactGroup contactGroup)
        {
            try
            {
                var result = await contactGroupRepository.UpdateContactGroupAsync(contactGroup);
                if (result.IsSuccess)
                    return Ok(result.ContactGroup);
                return NotFound(result.ErrorMessage);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContactGroups(ContactGroup ContactGroup)
        {
            try
            {
                var result = await contactGroupRepository.DeleteContactGroupAsync(ContactGroup);
                if (result.IsSuccess)
                    return Ok(result.ContactGroup);
                return NotFound(result.ErrorMessage);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpGet]
        [Route("/ContactGroupSearch{contactGroupSearch}")]
        public async Task<ActionResult> SearchContactGroupRecords(string contactGroupSearch)
        {
            if (!string.IsNullOrEmpty(contactGroupSearch))
            {
                contactGroupSearch = contactGroupSearch.ToLower();
                var contactResult = await contactGroupRepository.GetContactGroupsAsync();
                if (contactResult.IsSuccess)
                {
                    PaginationFilter filter = new PaginationFilter(1, 10);
                    var route = Request.Path.Value;
                    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                    var contactList = contactResult.ContactGroups.ToList();
                    var pagedData = contactList.Where(a => a.GroupName.ToLower().Contains(contactGroupSearch)
                            || a.Contacts.Any(b => b.FirstName.ToLower().Contains(contactGroupSearch))
                            || a.Contacts.Any(b => b.LastName.ToLower().Contains(contactGroupSearch))
                            || a.Contacts.Any(b => b.PhoneNumber.ToLower().Contains(contactGroupSearch))).ToList()
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();

                    var totalRecords = pagedData.Count();
                    var pagedReponse = PaginationHelper.CreatePagedReponse<ContactGroup>(pagedData, validFilter, totalRecords, uriService, route);
                    if(totalRecords > 0)
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
