using System;
using System.Linq;
using System.Linq.Expressions;
using WaveAccess.Data.Entity.Test.Module1.Models;

namespace WaveAccess.Data.Entity.Test.Module1.Expressions
{
    public class UserExpressions
    {

        public static Expression<Func<User, bool>> UserByGroups(params int[] groups)
        {
            var groupsExists = groups != null && groups.Any();

            if (groupsExists)
            {
                return u => u.Groups.SelectMany(g => g.Parents).Any(h => groups.Contains(h.ParentId));
            }

            return u => true;
        }

        public static Expression<Func<User, bool>> UserWithoutGroup()
        {
            return u => !u.Groups.Any();
        }

        public static Expression<Func<User, bool>> UserByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return u => u.Login.Contains(name);
            }
            else {
                return u => true;
            }
        }
    }
}
