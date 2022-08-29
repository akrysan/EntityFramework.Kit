using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using WaveAccess.Data.Entity.Test.Module1.Models;
using WaveAccess.Data.Entity.Test.Module2.Models;
using WaveAccess.Data.Entity.Test.Module1.Expressions;
using WaveAccess.Data.Entity.Hint;
using System.Data.Entity.Infrastructure.Interception;
using log4net;

namespace WaveAccess.Data.Entity.Test
{

    [TestClass]
    public class MultiContextTest
    {
        private static IDbInterceptor _hintInterceptor = new HintInterceptor();
        private static IDbInterceptor _log4NetInterceptor = new Log4NetCommandInterceptor();

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            DbInterception.Add(_hintInterceptor);
            DbInterception.Add(_log4NetInterceptor);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DbInterception.Remove(_hintInterceptor);
            DbInterception.Remove(_log4NetInterceptor);
        }

        [TestMethod]
        public void TestTasks()
        {
            using (var userContext = new UserContext())
            {
                userContext.Database.Initialize(true);
            }
            using (var test = new TaskContext())
            {
                test.Database.Initialize(true);
            }

            using (var db = new GenericContext())
            {
                using (var qh = new HintScope("HASH JOIN"))
                {
                    var tasks = db.Set<Task>().Include(t => t.User).Where(t => db.Set<User>().
                      Where(UserExpressions.UserByGroups(1)).Any(u => u.Id == t.UserId));
                    var testarr = tasks.ToArray();

                    Assert.AreEqual(4, testarr.Length);
                }
            }
        }

        [TestMethod]
        public void TestScope()
        {
            using (var userContext = new UserContext())
            {
                userContext.Database.Initialize(true);
            }
            using (var test = new TaskContext())
            {
                test.Database.Initialize(true);
            }

            var testing = Log4NetTestHelper.RecordLog(() =>
            {
                using (var db = new GenericContext())
                {
                    using (var qh = new JoinHintScope<GroupHierarchy>("HASH"))
                    {
                        var tasks = db.Set<Task>().Include(t => t.User).Where(t => db.Set<User>().
                          Where(UserExpressions.UserByGroups(1)).Any(u => u.Id == t.UserId));
                        var testarr = tasks.ToArray();
                    }
                }
            });
            Assert.IsTrue(testing[0].Contains("INNER HASH JOIN [dbo].[v_GroupHierarchy]"));
        }
    }
}