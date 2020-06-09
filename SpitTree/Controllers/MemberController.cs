using SpitTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Net;

namespace SpitTree.Controllers
{
    public class MemberController : Controller
    {
        //instance of the database
        private SpitTreeDbContext db = new SpitTreeDbContext();

        // GET: Posts
        //the index action is called when a registered user clicks the "My Posts" link
        //this method is returning a list of posts that were created by the user by using the UserId
        [Authorize(Roles ="Member")] //only registered user who is a Member can access this methos
        public ActionResult Index()
        {
            //select all the posts fromt he posts table including the foreign keys of category and user
            var posts = db.Posts.Include(p => p.Category).Include(p => p.User);

            //get the Id of the logged in user using identity
            //note: UserId is a string
            var userId = User.Identity.GetUserId();

            //from the list of posts from the Posts tables select only the posts where the UserId matches the Id of the logged in user
            //returns a list of posts
            posts = posts.Where(p => p.UserId == userId);

            //send the list of posts to the index view in the Members subfolder
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id) //? creates a nullable variable
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find a post in the Posts table by Id
            Post post = db.Posts.Find(id);

            //if the post does not exist then return a not found error
            if(post == null)
            {
                return HttpNotFound();
            }

            //if all is good, send the post to the details view and siplay the values stored in the properties
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            //send the list of categories to the view using a ViewBag so the user can select a category from a dropdown
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");

            //return the Create view to the browser
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="PostId,Title,Descriptions,Location,Price,CategoryId")] Post post)
        {
            //if paramater post is not null
            if (ModelState.IsValid)
            {
                //assign todays date for date posts
                post.DatePosted = DateTime.Now;

                //set the expirey date to 14 days later
                post.DateExpired = post.DatePosted.AddDays(14);

                //the user who creates this post is assigned as the user foreign key
                post.UserId = User.Identity.GetUserId();

                //add the post to the Posts tables
                db.Posts.Add(post);

                //save changes in the databse
                db.SaveChanges();

                //return to index action in MemberController
                return RedirectToAction("Index");
            }
            //if the post paramater is null then send the list categories back to the view and try to create a post again
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);

            //send the post back to the create view
            return View(post);
        }

        // GET: Member/Edit/5
        //this method returns the Edit from with an instance of post allowing the user to make changes
        public ActionResult Edit(int? id)//? creates a nullable variable
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find the post by Id in the Posts table
            Post post = db.Posts.Find(id);

            if(post == null)
            {
                return HttpNotFound();
            }

            //get a list of all the categories from the Categories tables
            //send the list to the view using a viewbag
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);

            //also send the post to the Edit vies where the user can make changes
            return View(post);
        }

        // POST: Posts/Edit/5
        //this method gets the edited post and updates the database with the new information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include ="PostId,Title,Descriptions,Location,Price,CategoryId")] Post post)
        {
            //if the post passed as a paramated to the Edit action is not null then the edited post will be updated in the database
            if (ModelState.IsValid)
            {
                //record the new date when the post was edited
                post.DatePosted = DateTime.Now;
                //set the expiery date to 14 days from now
                post.DateExpired = post.DatePosted.AddDays(14);
                //gets the Id of the User that is logged in and assigns it as a foreign key to the post
                post.UserId = User.Identity.GetUserId();
                //updates the database
                db.Entry(post).State = EntityState.Modified;
                //saves changes to the database
                db.SaveChanges();
                //redirectes the user to the index action in the MemberControllwe that displays the list of posts
                return RedirectToAction("Index");
            }
            //otherwise, if the post paramater is null, then we send the list of categories back to the edit form
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);

            //return the post to the edit form
            return View(post);
        }

        // GET: Posts/Delete/5
        //this method will delete a post by id
        public ActionResult Delete(int? id) //nullage Id
        {
            //if the ID is null return a bad request error
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //first find a post in the posts table by Id
            Post post = db.Posts.Find(id);

            //next find the post category by searching through the categories table for a category by id which is the foreign key
            var category = db.Categories.Find(post.CategoryId);

            //assign the category to the category navigational property Category so we can display the category name
            post.Category = category;

            //if the post is null return a not found error
            if(post == null)
            {
                return HttpNotFound();
            }
            //return the delete view and send the post to the view so details can be viewed
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]//this is important
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //find post by id in Posts tables
            Post post = db.Posts.Find(id);

            //remove the post from Posts tables
            db.Posts.Remove(post);

            //save changes in the database
            db.SaveChanges();

            //redirect to index action in member controller
            return RedirectToAction("Index");
        }
    }
}
