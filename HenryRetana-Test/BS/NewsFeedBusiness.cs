using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace HenryRetana_Test.BS
{
    public static class NewsFeedBusiness
    {
        public static List<NewsFeed> RetrieveNewsFeed()
        {
            using (var context = new NewsFeedDBEntities())
            {
                return context.NewsFeed.Where(x => x.Active == true).Include(x => x.Users).Include(a => a.Feed).ToList();
            }
        }

        public static List<NewsFeed> RetrieveNewsFeedById(int id)
        {
            using (var context = new NewsFeedDBEntities())
            {
                return context.NewsFeed.Where(x => x.Active == true && x.FeedId == id).Include(x => x.Users).Include(a => a.Feed).ToList();
            }
        }

        public static List<NewsFeed> SearchNewsFeed(string searchText)
        {
            using (var context = new NewsFeedDBEntities())
            {
                var news = context.NewsFeed.Where(x => x.Active == true).Include(x => x.Users).Include(a => a.Feed).ToList();

                return news.Where(x => x.Title.ToLower().Contains(searchText.ToLower()) ||
                                        x.Description.ToLower().Contains(searchText.ToLower()) ||
                                        x.Users.Name.ToLower().Contains(searchText.ToLower()) ||
                                        x.Feed.Name.ToLower().Contains(searchText.ToLower())
                                 ).ToList();
            }
        }

        public static List<NewsFeed> RetrieveMainNewsFeed(int userId)
        {
            var feeds = FeedBusiness.RetrieveMainFeed(userId);
            var news = RetrieveNewsFeed();

            var res = (from f in feeds
                       join n in news on f.Id equals n.FeedId
                       select n).ToList();
            return res;
        }

        #region NewsByUser
        public static List<NewsFeed> RetrieveNewsFeedByUser(int userId)
        {
            using (var context = new NewsFeedDBEntities())
            {
                return context.NewsFeed.Where(x => x.CreatedBy == userId).Include(x => x.Users).Include(a => a.Feed).ToList();
            }
        }

        public static void AddNewsFeed(NewsFeed newsFeed)
        {
            using (var context = new NewsFeedDBEntities())
            {
                context.NewsFeed.Add(newsFeed);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
