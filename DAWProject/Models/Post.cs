using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DAWProject.Models
{
    public class Post
    {
        [Key]
        public int Post_id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created_at { get; set; }
        //fks
        public int Category_id { get; set; }
        //public int id {get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

}