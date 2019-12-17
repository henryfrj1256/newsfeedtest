using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HenryRetana_Test.Controllers
{               
    public class ManagementController : Controller
    {
        public ActionResult Index()
        {
            //Validates user session   
            var user = BS.UtilitiesBusiness.GetSession();
            if (user == null)  return RedirectToAction("Index", "Home", new { area = "" });  
           
            var news = BS.NewsFeedBusiness.RetrieveNewsFeedByUser(user.Id);
            return View(news);
        }
        
        public ActionResult AddNew(NewsFeed model)
        {
            try
            {
                var user = BS.UtilitiesBusiness.GetSession();
                model.CreateDate = DateTime.Now;
                model.CreatedBy = user.Id;
                BS.NewsFeedBusiness.AddNewsFeed(model);

                var news = BS.NewsFeedBusiness.RetrieveNewsFeedByUser(user.Id);
                return RedirectToAction("Index", news);
            }
            catch (Exception)
            {
                     throw new Exception();
            }             
        }

        #region Subscriptions
        public ActionResult Subscriptions()
        {
            //Validates user session   
            var user = BS.UtilitiesBusiness.GetSession();
            if (user == null) return RedirectToAction("Index", "Home", new { area = "" });

            var model = new SubscriptionsModel
            {
                FeedList = BS.FeedBusiness.RetrieveFeedByUser(user.Id),
                Subscriptions = BS.FeedBusiness.RetrieveFeedSubscriptions(user.Id)
            };
            return View(model);
        }

        public ActionResult AddFeed(Feed model)
        {
            try
            {
                var user = BS.UtilitiesBusiness.GetSession();
                model.CreateDate = DateTime.Now;
                model.CreatedBy = user.Id;
                BS.FeedBusiness.AddFeed(model);

                var res = new SubscriptionsModel
                {
                    FeedList = BS.FeedBusiness.RetrieveFeedByUser(user.Id),
                    Subscriptions = BS.FeedBusiness.RetrieveFeedSubscriptions(user.Id)
                };

                ViewBag.Create = true;
                return RedirectToAction("Subscriptions", res);
            }
            catch (Exception)
            {
                throw new Exception();
            }              
        }

        public ActionResult AddOrDeleteSubscriptions(int feedId, bool add)
        {
            try
            {
                var user = BS.UtilitiesBusiness.GetSession();
                if (add)
                {
                    BS.FeedBusiness.AddSubscription(new Subscription { UserId = user.Id, FeedId = feedId });
                }
                else
                {
                    BS.FeedBusiness.RemoveSubscription(new Subscription { UserId = user.Id, FeedId = feedId });
                }

                var res = new SubscriptionsModel
                {
                    FeedList = BS.FeedBusiness.RetrieveFeedByUser(user.Id),
                    Subscriptions = BS.FeedBusiness.RetrieveFeedSubscriptions(user.Id)
                };

                ViewBag.Create = false;
                return RedirectToAction("Subscriptions", res);
            }
            catch (Exception)
            {
                throw new Exception();
            }             
        }       
        #endregion

    }
}