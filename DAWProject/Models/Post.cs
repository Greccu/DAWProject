using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAWProject.Models;


namespace DAWProject.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required(ErrorMessage = "You must put a title for this post")]
        [StringLength(100, ErrorMessage = "The Title can't be longer than 50 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Write something in the post!")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        //fks
        [Required(ErrorMessage = "What is your post about?")]
        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public IEnumerable<SelectListItem> Categ { get; set; }
    }

}