using ContactBook.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.API.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDBContext dBContext;

        public ContactRepository(AppDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<(bool IsSuccess, Contact Contact, string ErrorMessage)> CreateContactAsync(Contact contact)
        {
            try
            {
                var isContactExist = await dBContext.Contacts.FirstOrDefaultAsync(a => a.PhoneNumber == contact.PhoneNumber);
                if (isContactExist == null)
                {
                    var result = await dBContext.Contacts.AddAsync(contact);
                    await dBContext.SaveChangesAsync();
                    return (true, contact, "");
                }
                else
                    return (false, null, "Record is already exist");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Contact Contact, string ErrorMessage)> DeleteContactAsync(int id)
        {
            try
            {
                var deletedData = await dBContext.Contacts.FirstOrDefaultAsync(a => a.Id == id);
                if (deletedData != null)
                {
                    dBContext.Contacts.Remove(deletedData);
                    await dBContext.SaveChangesAsync();
                    return (true, deletedData, "");
                }
                else
                    return (true, deletedData, "Record is not deleted");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Contact> Contacts, string ErrorMessage)> GetAllContactsAsync()
        {
            try
            {
                var contactList = await dBContext.Contacts.ToListAsync();
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

        public async Task<(bool IsSuccess, Contact Contact, string ErrorMessage)> GetContactByIdAsync(int id)
        {
            try
            {
                var contact = await dBContext.Contacts.FirstOrDefaultAsync(a => a.Id == id);
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

        public async Task<(bool IsSuccess, Contact Contact, string ErrorMessage)> UpdateContactAsync(Contact contact)
        {
            try
            {
                if (contact != null)
                {
                    dBContext.Contacts.Update(contact);
                    await dBContext.SaveChangesAsync();
                    return (true, contact, "");
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
