using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveAccess.Data.Entity.Test.Models;
using System.Threading;
using System.Globalization;

namespace WaveAccess.Data.Entity.Test {
    [TestClass]
    public class MigrateDatabaseWithSpecialSeed {
        [TestMethod]
        public void TestResourses() {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-GB");
            using (var db = new SimpleContext()) {
                var test = db.SimpleEntities.FirstOrDefault();
            }
        }
    }
}