using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    public class Role
    {

        public int RoleId
        {
            get;
            set;
        }

        [Required]
        [StringLength(1000)]
        public string Name
        {
            get;
            set;
        }

    }
}
