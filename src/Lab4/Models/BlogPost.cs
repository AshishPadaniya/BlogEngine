using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    public class Blogpost
    {

        public int BlogPostId 
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("UserId")]
        public int UserId 
        {
            get;
            set;
        }

        [Required]
        [StringLength(1000)]
        public string Title
        {
            get;
            set;
        }

        [Required]
        [StringLength(1000)]
        public string Content 
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Posted 
        {
            get;
            set;
        }
        
        

    }
}
