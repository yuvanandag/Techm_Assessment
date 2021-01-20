using ContactBook.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.API.Repository
{
    public interface IContactRepository
    {
        Task<(bool IsSuccess, Contact Contact, string ErrorMessage)> CreateContactAsync(Contact contact);
        Task<(bool IsSuccess, IEnumerable<Contact> Contacts, string ErrorMessage)> GetAllContactsAsync();
        Task<(bool IsSuccess, Contact Contact, string ErrorMessage)> GetContactByIdAsync(int id);
        Task<(bool IsSuccess, Contact Contact, string ErrorMessage)> DeleteContactAsync(int id);
        Task<(bool IsSuccess, Contact Contact, string ErrorMessage)> UpdateContactAsync(Contact contact);
    }
}
