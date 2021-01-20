using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.API.Models
{
    public class ContactGroup
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "GroupName is required.")]
        public string GroupName { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
