using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HenryRetana_Test.BS
{
    public static class UsersBusiness
    {
        public static Users AddNewUser(Users user)
        {
            using (var context = new NewsFeedDBEntities())
            {
                context.Users.Add(user);
                context.SaveChanges();
                return user;
            }
        }

        public static Users Login(Users user)
        {
            using (var context = new NewsFeedDBEntities())
            {
                return context.Users.Where(x => x.UserName == user.UserName && user.Password == user.Password).FirstOrDefault();
            }
        }

        public static Users Register(Users user)
        {
            using (var context = new NewsFeedDBEntities())
            {
                context.Users.Add(user);
                context.SaveChanges();
                return user;
            }
        }
    }
}
