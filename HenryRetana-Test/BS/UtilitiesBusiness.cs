using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HenryRetana_Test.BS
{
    public static class UtilitiesBusiness
    {
        public static void CreateSession(Users user)
        {
            HttpContext.Current.Session["User"] = user;
            HttpContext.Current.Session.Timeout = 5;
        }

        public static Users GetSession()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                var user = (Users)HttpContext.Current.Session["User"];
                return user; 
            }
            return null;
        }

        public static void Logout()
        {
            HttpContext.Current.Session.Remove("User");
        }

        #region ValidateAndInsertDefaultData
        public static bool ValidateData()
        {
            using (var context = new NewsFeedDBEntities())
            {
                return context.NewsFeed.Any();
            }
        }

        public static void InsertDefaultData()
        {
            using (var context = new NewsFeedDBEntities())
            {
                var user = new Users { Name = "Default", Password = "Default" };
                context.Users.Add(user);
                context.SaveChanges();

                var feed = new Feed { Name = "Sports", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true };
                context.Feed.Add(feed);
                context.SaveChanges();
                context.NewsFeed.Add(new NewsFeed { Title = "Football", Description = "World Cup Qatar 2022 is comming.", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true, FeedId = feed.Id });
                context.NewsFeed.Add(new NewsFeed { Title = "Baseball", Description = "The official news source of Major League Baseball.", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true, FeedId = feed.Id });
                context.NewsFeed.Add(new NewsFeed { Title = "Voleyball", Description = "Volleyball news, interviews, transfers, videos and more.", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true, FeedId = feed.Id });
                context.SaveChanges();

                feed = new Feed { Name = "Food", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true };
                context.Feed.Add(feed);
                context.SaveChanges();
                context.NewsFeed.Add(new NewsFeed { Title = "Chinese", Description = "Find out what Chinese dishes to try in China, soon!", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true, FeedId = feed.Id });
                context.NewsFeed.Add(new NewsFeed { Title = "Mexican", Description = "All the latest breaking news on Mexican Food.", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true, FeedId = feed.Id });
                context.NewsFeed.Add(new NewsFeed { Title = "Fast food", Description = "KFC hints at plan for plant-based chicken alternative.", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true, FeedId = feed.Id });
                context.SaveChanges();

                feed = new Feed { Name = "Vacations", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true };
                context.Feed.Add(feed);
                context.SaveChanges();
                context.NewsFeed.Add(new NewsFeed { Title = "Costa Rica", Description = "All the latest breaking news on Costa Rica travel.", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true, FeedId = feed.Id });
                context.NewsFeed.Add(new NewsFeed { Title = "Grand Canyon", Description = "If you want to really explore the Grand Canyon, you need to plan a day trip on your next vacation in Vegas.", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true, FeedId = feed.Id });
                context.NewsFeed.Add(new NewsFeed { Title = "Mexico", Description = "Cancun ranks as the top international destination this holiday season for U.S. travelers.", CreateDate = DateTime.Now, CreatedBy = user.Id, Active = true, FeedId = feed.Id });
                context.SaveChanges();
            }
        }
        #endregion
    }
}