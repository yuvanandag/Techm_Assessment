using ContactBook.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.API.Repository
{
    public interface IContactGroupRepository
    {
        Task<(bool IsSuccess, ContactGroup ContactGroup, string ErrorMessage)> CreateContactGroupAsync(ContactGroup ContactGroup);
        Task<(bool IsSuccess, IEnumerable<ContactGroup> ContactGroups, string ErrorMessage)> GetContactGroupsAsync();
        Task<(bool IsSuccess, ContactGroup ContactGroup, string ErrorMessage)> GetContactGroupByIdAsync(int id);
        Task<(bool IsSuccess, ContactGroup ContactGroup, string ErrorMessage)> DeleteContactGroupAsync(ContactGroup ContactGroup);
        Task<(bool IsSuccess, ContactGroup ContactGroup, string ErrorMessage)> UpdateContactGroupAsync(ContactGroup ContactGroup);
    }
}
