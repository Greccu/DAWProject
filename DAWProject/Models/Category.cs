using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DAWProject.Models
{
    public class Category
    {
        [Key]
        public int Category_id { get; set; }
        [Required]
        public string Category_name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }

}