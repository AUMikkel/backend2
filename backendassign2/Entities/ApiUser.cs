using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace backendassign2.Entities
{
    public class ApiUser : IdentityUser
    {

        [MaxLength(100)]
        public string FullName { get; set; }
    }

}