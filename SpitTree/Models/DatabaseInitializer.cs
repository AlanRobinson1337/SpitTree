using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace SpitTree.Models
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<SpitTreeDbContext>
    {
        //seeding the database
        protected override void Seed(SpitTreeDbContext context)
        {
            if (!context.Users.Any())
            {
                //create roles and store them in the AspNetRoles Tables******************
                //create a role manager object to create and store roles
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //if the admin role doesn't exist create one
                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }
                //if the Member role doesn't exist create one
                if (!roleManager.RoleExists("Member"))
                {
                    roleManager.Create(new IdentityRole("Member"));
                }
                //save the new roles to the database
                context.SaveChanges();
            }

            //createing users and assigning them to roles***********************************
            //the user manager object lets us create users and store them in the database
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

            //if no user with the admin@spittree.com username exists
            if (userManager.FindByName("admin@spittree.com") == null)
            {
                //password vaildator with minimum requirements
                userManager.PasswordValidator = new PasswordValidator()
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequireLowercase = false,
                    RequireNonLetterOrDigit = false,
                    RequireUppercase = false
                };
                //create an admin user
                var admin = new User()
                {
                    UserName = "admin@spittree.com",
                    Email = "admin@spittree.com",
                    FirstName = "Jim",
                    LastName = "Smith",
                    Street = "56 High Street",
                    City = "Glasgow",
                    PostCode = "G1 67AD",
                    EmailConfirmed = true
                };
                //add the password to user
                userManager.Create(admin, "admin");
                //add the user to the Admin role
                userManager.AddToRole(admin.Id, "Admin");
            }

            //creating someusers to be added to the members role
            var member1 = new User()
            {
                UserName = "m1@gmail.com",
                Email = "m1@gmail.com",
                FirstName = "Tom",
                LastName = "Jones",
                Street = "5 Merry Street",
                City = "Glasgow",
                PostCode = "G11 1GG",
                EmailConfirmed = true
            };
            if (userManager.FindByName("m1@gmail.com") == null)
            {
                userManager.Create(member1, "password1");
                userManager.AddToRole(member1.Id, "Member");
            }

            var member2 = new User()
            {
                UserName = "m2@gmail.com",
                Email = "m2@gmail.com",
                FirstName = "Joe",
                LastName = "Bloggs",
                Street = "500 Springfield Road",
                City = "Glasgow",
                PostCode = "G0 41N",
                EmailConfirmed = true
            };
            if (userManager.FindByName("m2@gmail.com") == null)
            {
                userManager.Create(member2, "password2");
                userManager.AddToRole(member2.Id, "Member");
            }
            //save the cahnges
            context.SaveChanges();

            //seeding the categoreis table

            //creating categories
            var cat1 = new Category() { Name = "Motors" };
            var cat2 = new Category() { Name = "Property" };
            var cat3 = new Category() { Name = "Jobs" };
            var cat4 = new Category() { Name = "Services" };
            var cat5 = new Category() { Name = "Pets" };
            var cat6 = new Category() { Name = "ForSale" };

            //adding each category to the categories database table
            context.Categories.Add(cat1);
            context.Categories.Add(cat2);
            context.Categories.Add(cat3);
            context.Categories.Add(cat4);
            context.Categories.Add(cat5);
            context.Categories.Add(cat6);

            //save the changes
            context.SaveChanges();

            //seeding the posts tables

            var post1 = new Post()
            {
                Title = "house for sale",
                Description = "5 bedroom detatched house",
                Location = "Glasgow",
                Price = 145000m,
                DatePosted = new DateTime(2019, 1, 1, 8, 0, 15), //this is the date the post was made
                DateExpired = new DateTime(2019, 1, 1, 8, 0, 15).AddDays(14), //the post will expire after 14 days
                User = member2,
                Category = cat2
            };
            context.Posts.Add(post1);

            var post2 = new Post()
            {
                Title = "Ford Focus",
                Description = "2019 5dr",
                Location = "Edinburugh",
                Price = 13500m,
                DatePosted = new DateTime(2019, 5, 25, 8, 0, 15),
                DateExpired = new DateTime(2019, 5, 25, 8, 0, 15).AddDays(14),
                User = member2,
                Category = cat1
            };
            context.Posts.Add(post2);

            var post4 = new Post()
            {
                Title = "Labradoodle",
                Description = "Pure bread labradoodle",
                Location = "Aberdeen",
                Price = 2100m,
                DatePosted = new DateTime(2019, 1, 25, 6, 0, 15),
                DateExpired = new DateTime(2019, 1, 25, 6, 0, 15).AddDays(14),
                User = member1,
                Category = cat5
            };
            context.Posts.Add(post4);

            var post3 = new Post()
            {
                Title = "BMW",
                Description = "Flash 2019 Beamer",
                Location = "Dundee",
                Price = 34000m,
                DatePosted = new DateTime(2019, 4, 5, 5, 0, 15),
                DateExpired = new DateTime(2019, 4, 5, 5, 0, 15).AddDays(14),
                User = member2,
                Category = cat1
            };
            context.Posts.Add(post3);

            var post5 = new Post()
            {
                Title = "Merc",
                Description = "Shiney new Merc",
                Location = "Govan",
                Price = 32000m,
                DatePosted = new DateTime(2019, 4, 5, 5, 0, 15),
                DateExpired = new DateTime(2019, 4, 5, 5, 0, 15).AddDays(14),
                User = member2,
                Category = cat1
            };
            context.Posts.Add(post5);

            var post6 = new Post()
            {
                Title = "Honda CB125",
                Description = "nice 125cc bike",
                Location = "Glasgow",
                Price = 3200m,
                DatePosted = new DateTime(2018, 5, 25, 8, 0, 15),
                DateExpired = new DateTime(2018, 5, 25, 8, 0, 15).AddDays(14),
                User = member2,
                Category = cat1
            };
            context.Posts.Add(post6);

            //save the changes to database
            context.SaveChanges();
        }
    }
}