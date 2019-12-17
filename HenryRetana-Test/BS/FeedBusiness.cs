using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace HenryRetana_Test.BS
{
    public static class FeedBusiness
    {
        public static List<Feed> RetrieveFeed()
        {
            using (var context = new NewsFeedDBEntities())
            {
                return context.Feed.Where(x => x.Active == true).ToList();
            }
        }

        public static List<Feed> RetrieveFeedByUser(int userId)
        {
            using (var context = new NewsFeedDBEntities())
            {
                return context.Feed.Where(x => x.CreatedBy == userId).ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetFeedList(int userId)
        {
            var data = RetrieveFeedByUser(userId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });

            return new SelectList(data, "Value", "Text");
        }

        public static void AddFeed(Feed model)
        {
            using (var context = new NewsFeedDBEntities())
            {
                context.Feed.Add(model);
                context.SaveChanges();
            }
        }

        public static List<Feed> RetrieveMainFeed(int userId)
        {
            var subs = RetrieveFeedSubscriptions(userId);   
            subs.AddRange(RetrieveFeedByUser(userId).Where(x => x.Active == true));

            return subs;
        }
        #region Subs
        public static List<Feed> RetrieveFeedSubscriptions(int userId)
        {
            using (var context = new NewsFeedDBEntities())
            {
                var subscriptions = (from feed in context.Feed
                                     join subs in context.Subscription on feed.Id equals subs.FeedId
                                     where feed.Id == subs.FeedId && subs.UserId == userId
                                     select feed).ToList(); 
                return subscriptions;
            }
        }

        public static void AddSubscription(Subscription model)
        {
            using (var context = new NewsFeedDBEntities())
            {
                context.Subscription.Add(model);
                context.SaveChanges();
            }
        }

        public static void RemoveSubscription(Subscription model)
        {
            using (var context = new NewsFeedDBEntities())
            {
                var sub = context.Subscription.Where(x => x.UserId == model.UserId && x.FeedId == model.FeedId).FirstOrDefault(); 
                context.Subscription.Remove(sub);
                context.SaveChanges();
            }
        }

        public static IEnumerable<SelectListItem> GetFeedToSubscribeList(int userId)
        {
            var subs = RetrieveFeedSubscriptions(userId);
            var data = RetrieveFeed().Where(x => x.CreatedBy != userId).ToList();

            var res = (from da in data join s in subs on da.Id equals s.Id into tempRes
                       from s in tempRes.DefaultIfEmpty()
                       where s == null
                       select da)                         
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });

            return new SelectList(res, "Value", "Text");
        }
        #endregion
    }
}