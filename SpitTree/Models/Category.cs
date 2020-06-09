using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpitTree.Models
{
    public class Category
    {
        //properties
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string Name { get; set; }

        //navigational property
        public List<Post> Posts { get; set; }
    }
}