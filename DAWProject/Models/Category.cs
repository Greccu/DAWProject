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
        public int category_id { get; set; }
        [Required]
        public string category_name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }

    public class CategoryDBContext : DbContext
    {
        public CategoryDBContext() : base("DBConnectionString") { }
        public DbSet<Category> Categories { get; set; }
    }
}