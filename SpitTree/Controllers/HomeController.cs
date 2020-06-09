using SpitTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace SpitTree.Controllers
{
    public class HomeController : Controller
    {//create an instandce of the database context
            private SpitTreeDbContext context = new SpitTreeDbContext();

        public ActionResult Index()
        {
            //get all posts, include the category for each post, include the user who created the post
            //order the posts from most recent to oldest
            var posts = context.Posts.Include(p => p.Category)
                .Include(p => p.User)
                .OrderByDescending(p => p.DatePosted);

            //send the list of categories to the index page to display them
            ViewBag.Categories = context.Categories.ToList();

            return View(posts.ToList());
        }

        public ActionResult Details(int id)
        {
            //search the Posts table in the database, find post by Id and return post
            Post post = context.Posts.Find(id);

            //using the foreign key from UserId from the instance of post, find the user who created the post
            var user = context.Users.Find(post.UserId);

            //using the roeign key CategoryIf from the post find the category the post belongs to
            var category = context.Categories.Find(post.CategoryId);

            //assign the user to the user navigational property in post
            post.User = user;

            //assign the category to the category navigational property in Post
            post.Category = category;

            //send the post model to the details view
            return View(post);
        }

        [HttpPost]
        //this is the action that will take the search string from the index page
        //note:the name of the string paramater SearchString must be the same name with the textbox in the view
        public ViewResult Index(string Search)
        {
            var posts = context.Posts.Include(p => p.Category)
                .Include(p => p.User)
                .Where(p => p.Category.Name.Equals(Search.Trim()))
                .OrderByDescending(p => p.DatePosted);

            return View(posts.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}