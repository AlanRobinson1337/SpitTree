using SpitTree.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace SpitTree.Controllers
{
    [Authorize(Roles ="Admin")] //this makes sure only the admin role can access this controller
    public class AdminController : Controller
    {    //declare an instance of the database
        private SpitTreeDbContext db = new SpitTreeDbContext();
    
        // GET: Admin
        [Authorize(Roles ="Admin")]//only admin role can call the index action
        public ActionResult Index()
        {
            return View();
        }

        //GET: Categories
        public ActionResult ViewAllCategories()
        {
            //return the view all categories view that displays a list of categories
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id) //allows nullable id
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //find category by id in the categories database table
            Category category = db.Categories.Find(id);
            if(category == null)
            {
                return HttpNotFound();
            }
            //send the category to the details view
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="CategoryId,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id) //allows nullable paramater
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if(category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include ="CategoryId,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)//allow nullable paramater
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if(category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]//this is important
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("ViewAllCategories");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Posts/Delete/5
        public ActionResult DeletePost(int? id)//allow nullable paramater
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //POST: Posts/Delete/5
        [HttpPost, ActionName("DeletePost")]//this is important
        [ValidateAntiForgeryToken]
        public ActionResult DeletePostConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts");
        }


        [Authorize(Roles ="Admin")]
        public ActionResult ViewAllPosts()
        {
            //get all the posts from the database including the category and user associated with the post
            List<Post> posts = db.Posts.Include(p => p.Category)
                .Include(p => p.User)
                .ToList();

            //send the list to the view ViewAllPosts
            return View(posts);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewUsers()
        {
            //get all the users from the database including the role and order them by last name
            List<User> users = db.Users.Include(u => u.Roles).OrderBy(u => u.LastName).ToList();

            //send the list to the view 
            return View(users);
        }
    }
}
