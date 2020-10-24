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
        public int post_id { get; set; }
        [Required]
        public string title { get; set; }
        public string content { get; set; }
        public DateTime created_at { get; set; }
        //fks
        public int category_id { get; set; }
        //public int id {get; set; }

        public virtual Category Category { get; set; }
    }

    public class PostDBContext : DbContext
    {
        public PostDBContext() : base("DBConnectionString") { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}