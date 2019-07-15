using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment1.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Dynamic;

namespace Lab4.Controllers
{
    public class HomeController : Controller
    {
        private Assignment1DataContext _BlogPostContext;


        public HomeController(Assignment1DataContext context)
        {
            
            _BlogPostContext = context;
              
        }
        public void setLayout()
        {
            if (HttpContext.Session.GetInt32("userID") != null)
            {
                int userid = (int)HttpContext.Session.GetInt32("userID");
                User logged_in = _BlogPostContext.Users.Where(a => a.UserId.Equals(userid)).FirstOrDefault(); 
                ViewBag.firstname = logged_in.FirstName;
                ViewBag.lastname = logged_in.LastName;
                ViewBag.role = logged_in.RoleId;
            }
            
        }

        public IActionResult Index()
        {
            setLayout();
            return View(_BlogPostContext.Blogposts.ToList());
        }

        public IActionResult Register()
        {
            setLayout();
            return View();
        }
        public IActionResult Login()
        {
            setLayout();
            return View();
        }
        public IActionResult addUser(User user)
        {
            if(user.FirstName != null && user.LastName != null && user.EmailAddress != null && user.Password != null && user.RoleId != 0)
            {
                user.Password = CalculateMD5Hash(user.Password);
                _BlogPostContext.Users.Add(user);
                if (_BlogPostContext.SaveChanges() == 1)
                {
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Register",user);
        }
        public IActionResult AddBlogPost()
        {
            setLayout();
            int userid = (int)HttpContext.Session.GetInt32("userID");
            User logged_in = _BlogPostContext.Users.Where(a => a.UserId.Equals(userid)).FirstOrDefault();
            if (logged_in != null && logged_in.RoleId == 2)
                return View();
            else
                return RedirectToAction("Login");
        }

        public IActionResult AddPost(Blogpost post)
        {
            int userid = (int)HttpContext.Session.GetInt32("userID");
            User logged_in = _BlogPostContext.Users.Where(a => a.UserId.Equals(userid)).FirstOrDefault();
            if (post.Title != null && post.Content != null && post.Posted != null && logged_in!=null && logged_in.RoleId == 2)
            {
                post.UserId = userid;
                _BlogPostContext.Blogposts.Add(post);
                if (_BlogPostContext.SaveChanges() == 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("AddBlogPost", post);
                }
            }
            return RedirectToAction("AddBlogPost", post);
        }



        public IActionResult authenicate(User user)
        {
            if (user.EmailAddress == null || user.Password == null || user.EmailAddress == "" || user.Password =="")
            {
                return View("Login", user);
            }
            User logged_in = _BlogPostContext.Users.Where(a => a.EmailAddress.Equals(user.EmailAddress) && a.Password.Equals(CalculateMD5Hash(user.Password))).FirstOrDefault();
            if(logged_in != null)
            {
                int userid = logged_in.UserId;
                HttpContext.Session.SetInt32("userID",userid);
                return RedirectToAction("Index");
            }
            return View("Login",user);

        }

        public IActionResult EditBlogPost(int id)
        {
            
            int userid = (int)HttpContext.Session.GetInt32("userID");
            User logged_in = _BlogPostContext.Users.Where(a => a.UserId.Equals(userid)).FirstOrDefault();
            Blogpost post = _BlogPostContext.Blogposts.Where(a => a.BlogPostId.Equals(id)).FirstOrDefault();
            if (logged_in.RoleId == 2 && post != null)
            {
                setLayout();
                return View(post);
            }

            return RedirectToAction("Index");
        }
        public IActionResult ModifyBlogPost(Blogpost post)
        {
            int userid = (int)HttpContext.Session.GetInt32("userID");
            User logged_in = _BlogPostContext.Users.Where(a => a.UserId.Equals(userid)).FirstOrDefault();
            if (post.Title != null && post.Content != null && post.Posted != null && logged_in != null && logged_in.RoleId == 2)
            {
                var PostToUpdate = (from c in _BlogPostContext.Blogposts where c.BlogPostId == post.BlogPostId select c).FirstOrDefault();
                PostToUpdate.Title = post.Title;
                PostToUpdate.Content = post.Content;
                PostToUpdate.Posted = post.Posted;
                _BlogPostContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("EditBlogPost", new { id = post.BlogPostId });
        }
        public IActionResult DisplayFullBlogPost(int id)
        {
            
            Blogpost Post = (from c in _BlogPostContext.Blogposts where c.BlogPostId == id select c).FirstOrDefault();
            if(Post != null)
            {
                User auther = (from c in _BlogPostContext.Users where c.UserId == Post.UserId select c).FirstOrDefault();
                if(auther != null)
                {
                    if(HttpContext.Session.GetInt32("userID") != null)
                    {
                        int userid = (int)HttpContext.Session.GetInt32("userID");
                        User logged_in = _BlogPostContext.Users.Where(a => a.UserId.Equals(userid)).FirstOrDefault();
                        if (logged_in != null)
                        {
                            ViewBag.logged_in = true;
                        }
                    }
                    List<Comment> comments = _BlogPostContext.Comments.Where(a => a.BlogPostId.Equals(id)).ToList();
                    ViewBag.Comments= comments;
                    ViewBag.Post = Post;
                    ViewBag.auther = auther;
                    setLayout();
                    return View();
                }
            }
                
            return RedirectToAction("Index"); 
        }

        public IActionResult AddComment(Comment comment)
        {
            if (comment.Content != null && comment.Content != "" && (HttpContext.Session.GetInt32("userID") != null))
            {
                comment.UserId = (int)HttpContext.Session.GetInt32("userID");
                _BlogPostContext.Comments.Add(comment);
                if (_BlogPostContext.SaveChanges() == 1)
                {
                    return RedirectToAction("DisplayFullBlogPost", new { id = comment.BlogPostId});
                }
            }
            if (comment.BlogPostId == 0)
                return RedirectToAction("Index");
            else
                return RedirectToAction("DisplayFullBlogPost", new { id = comment.BlogPostId });
        }



            public string CalculateMD5Hash(string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();

        }
    }
}
