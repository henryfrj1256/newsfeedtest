using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HenryRetana_Test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Validates if there is information in the data base 
            if (!BS.UtilitiesBusiness.ValidateData())
            {
                BS.UtilitiesBusiness.InsertDefaultData();
            }

            //Validates user session   
            var user = BS.UtilitiesBusiness.GetSession();
            if (user == null) return RedirectToAction("Index", "Home", new { area = "" });

            var model = new MainModel
            {
                FeedList = BS.FeedBusiness.RetrieveMainFeed(user.Id),
                NewsFeedList = BS.NewsFeedBusiness.RetrieveMainNewsFeed(user.Id)
            };
            return View(model);
        }

        public ActionResult GetNewsById(int feedId)
        {
            try
            {
                //Validates user session   
                var user = BS.UtilitiesBusiness.GetSession();

                var model = new MainModel
                {
                    FeedList = BS.FeedBusiness.RetrieveMainFeed(user.Id),
                    NewsFeedList = BS.NewsFeedBusiness.RetrieveNewsFeedById(feedId)
                };
                return View("Index", model);
            }
            catch (Exception)
            {
                throw new Exception();
            }             
        }

        public ActionResult Search(string searchText)
        {
            try
            {
                //If the value is null returns all the data
                var user = BS.UtilitiesBusiness.GetSession();
                var model = new MainModel();

                if (string.IsNullOrEmpty(searchText))
                {
                    model = new MainModel
                    {
                        FeedList = BS.FeedBusiness.RetrieveMainFeed(user.Id),
                        NewsFeedList = BS.NewsFeedBusiness.RetrieveMainNewsFeed(user.Id)
                    };
                }
                else
                {
                    model = new MainModel
                    {
                        FeedList = BS.FeedBusiness.RetrieveMainFeed(user.Id),
                        NewsFeedList = BS.NewsFeedBusiness.SearchNewsFeed(searchText)
                    };
                }

                return View("Index", model);
            }
            catch (Exception)
            {
                throw new Exception();
            }               
        }

        #region UserMethods        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users user)
        {
            try
            {
                user = BS.UsersBusiness.Login(user);
                if (user == null)
                {
                    ViewBag.Invalid = true;
                    return View();
                }
                BS.UtilitiesBusiness.CreateSession(user);

                var model = new MainModel
                {
                    FeedList = BS.FeedBusiness.RetrieveMainFeed(user.Id),
                    NewsFeedList = BS.NewsFeedBusiness.RetrieveMainNewsFeed(user.Id)
                };
                return RedirectToAction("Index", model); 
            }
            catch (Exception)
            {             
                throw new Exception();
            }             
        }

        public ActionResult Register()
        {
            return View(new Users { Name = string.Empty, UserName = string.Empty });
        }

        [HttpPost]
        public ActionResult Register(Users user)
        {
            try
            {
                if (user.Password != user.ConfirmPassword)
                {
                    ViewBag.Invalid = true;
                    return View(user);
                }
                user = BS.UsersBusiness.Register(user);
                BS.UtilitiesBusiness.CreateSession(user);

                var model = new MainModel
                {
                    FeedList = BS.FeedBusiness.RetrieveMainFeed(user.Id),
                    NewsFeedList = BS.NewsFeedBusiness.RetrieveMainNewsFeed(user.Id)
                };
                return RedirectToAction("Index", model);
            }
            catch (Exception)
            {
               throw new Exception();
            }
           
        }

        public ActionResult Logout()
        {
            BS.UtilitiesBusiness.Logout();
            return RedirectToAction("Login");
        }
        #endregion
    }
}