using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WA.Data.Entity
{
    internal class DBInitializerResolver : IDbDependencyResolver
    {

        public object GetService(System.Type type, object key)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDatabaseInitializer<>))
            {
                var contextType = type.GenericTypeArguments[0];
                var genType = typeof(MigrateDatabaseToLatestVersion<,>);
                var parrentConfigType = typeof(DbMigrationsConfiguration<>).MakeGenericType(contextType);
                Type migrateConfig = contextType.Assembly.GetTypes().FirstOrDefault(t => parrentConfigType.IsAssignableFrom(t));
                if (migrateConfig == null) return null;

                Type dbInitType = genType.MakeGenericType(contextType, migrateConfig);
                return Activator.CreateInstance(dbInitType, new object[] { true });
            }
            return null;
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(System.Type type, object key)
        {
            return new object[0];
        }
    }
}
