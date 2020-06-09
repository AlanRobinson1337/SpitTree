using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SpitTree.Models
{
    public class Post
    {
        //properties
        [Key]
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Location { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name ="Date Posted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:d}")] //formatting as short date time
        public DateTime DatePosted { get; set; }

        [Display(Name = "Date Expired")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")] //formatting as short date time
        public DateTime DateExpired { get; set; }

        //navigation property
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        //navigational Property
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}