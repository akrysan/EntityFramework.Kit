using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using WaveAccess.Data.Entity.Test.Module1.Models;
using WaveAccess.Data.Entity.Test.Module2.Models;
using WaveAccess.Data.Entity.Test.Module1.Expressions;

namespace WaveAccess.Data.Entity.Test {

    [TestClass]
    public class MultiContextTest
    {
        [TestMethod]
        public void TestTasks() {

            using (var db = new GenericContext()) {
                var tasks = db.Set<Task>().Include( t=>t.User).Where(t => db.Set<User>().
                     Where(UserExpressions.UserByGroups(1)).Any(u => u.Id == t.UserId));
                var testarr = tasks.ToArray();
                Assert.AreEqual(4, testarr.Length);
            }
        }
    }
}