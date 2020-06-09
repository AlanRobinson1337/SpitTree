using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpitTree.Models
{
    public class SpitTreeDbContext : IdentityDbContext<User>
    {
        //properties to add Posts and Categories to the database
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }

        public SpitTreeDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        public static SpitTreeDbContext Create()
        {
            return new SpitTreeDbContext();
        }
    }
}