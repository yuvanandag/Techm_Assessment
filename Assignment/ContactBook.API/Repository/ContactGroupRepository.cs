using ContactBook.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.API.Repository
{
    public class ContactGroupRepository : IContactGroupRepository
    {
        private readonly AppDBContext dBContext;

        public ContactGroupRepository(AppDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<(bool IsSuccess, ContactGroup ContactGroup, string ErrorMessage)> CreateContactGroupAsync(ContactGroup contactGroup)
        {
            try
            {
                var contactGroupUpdate = await dBContext.ContactGroups.Include(a => a.Contacts).FirstOrDefaultAsync(a => a.GroupName == contactGroup.GroupName);
                if (contactGroupUpdate != null)
                {
                    var errorMessage = string.Empty;
                    foreach(var contact in contactGroup.Contacts)
                    {
                        if (!contactGroupUpdate.Contacts.Any(a => a.PhoneNumber == contact.PhoneNumber))
                            contactGroupUpdate.Contacts.Add(contact);
                        else
                            errorMessage += $"User {contact.FirstName} is already a member of contact {contactGroup.GroupName} group";
                    }
                    dBContext.ContactGroups.UpdateRange(contactGroupUpdate);
                    await dBContext.SaveChangesAsync();
                    return (true, contactGroupUpdate, errorMessage);
                }
                else
                {
                    var result = await dBContext.ContactGroups.AddAsync(contactGroup);
                    await dBContext.SaveChangesAsync();
                    return (true, contactGroup, ""); 
                }
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ContactGroup ContactGroup, string ErrorMessage)> DeleteContactGroupAsync(ContactGroup contactGroup)
        {
            try
            {
                var contactGroupDelete = await dBContext.ContactGroups.Include(a => a.Contacts).FirstOrDefaultAsync(a => a.GroupName == contactGroup.GroupName);
                if (contactGroupDelete != null)
                {
                    foreach (var contact in contactGroup.Contacts)
                    {
                        var contactTobeDeleted = contactGroupDelete.Contacts.FirstOrDefault(a => a.PhoneNumber == contact.PhoneNumber);
                        contactGroupDelete.Contacts.Remove(contactTobeDeleted);
                    }
                    dBContext.ContactGroups.UpdateRange(contactGroupDelete);
                    await dBContext.SaveChangesAsync();
                    return (true, contactGroup, "");
                }
                else
                    return (true, contactGroup, "Record is not deleted");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ContactGroup> ContactGroups, string ErrorMessage)> GetContactGroupsAsync()
        {
            try
            {
                var contactList = await dBContext.ContactGroups.Include(a => a.Contacts).ToListAsync();
                if (contactList != null && contactList.Any())
                    return (true, contactList, "");
                else
                    return (false, null, "Records are not available");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ContactGroup ContactGroup, string ErrorMessage)> GetContactGroupByIdAsync(int id)
        {
            try
            {
                var contact = await dBContext.ContactGroups.FirstOrDefaultAsync(a => a.Id == id);
                if (contact != null)
                    return (true, contact, "");
                else
                    return (false, null, "Record is not available");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ContactGroup ContactGroup, string ErrorMessage)> UpdateContactGroupAsync(ContactGroup contactGroup)
        {
            try
            {
                var contactGroupUpdate = await dBContext.ContactGroups.Include(a => a.Contacts).FirstOrDefaultAsync(a => a.GroupName == contactGroup.GroupName);
                if (contactGroupUpdate != null)
                {
                    contactGroupUpdate.Contacts.AddRange(contactGroup.Contacts);
                    dBContext.ContactGroups.UpdateRange(contactGroupUpdate);
                    await dBContext.SaveChangesAsync();
                    return (true, contactGroupUpdate, "");
                }
                else
                    return (false, null, "Record is not updated");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}
