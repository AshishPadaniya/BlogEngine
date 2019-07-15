using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    public class User
    {

        public int UserId
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("RoleId")]
        public int RoleId
        {
            get;
            set;
        }

        [Required]
        [StringLength(1000)]
        public string FirstName
        {
            get;
            set;
        }
        [Required]
        [StringLength(1000)]
        public string LastName
        {
            get;
            set;
        }
        [Required]
        [StringLength(1000)]
        public string EmailAddress
        {
            get;
            set;
        }
        [Required]
        [StringLength(32)]
        public string Password
        {
            get;
            set;
        }
        
        
        

    }
}
