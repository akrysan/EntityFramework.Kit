using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAccess.Data.Entity
{
    static class DbContextMetaData
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName(typeof(T));
        }

        public static string GetTableName(this DbContext context, Type t)
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName(t);
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            return context.GetTableName(typeof(T));
        }
        public static string GetTableName(this ObjectContext context, Type t)
        {
            string entityName = t.Name;
            ReadOnlyCollection<EntityContainerMapping> storageMetadata = context.MetadataWorkspace.GetItems<EntityContainerMapping>(DataSpace.CSSpace);

            foreach (EntityContainerMapping ecm in storageMetadata)
            {
                if (ecm.StoreEntityContainer.TryGetEntitySetByName(entityName, true, out EntitySet entitySet))
                {
                    return $"[{entitySet.Schema}].[{entitySet.Table}]";
                }
            }
            throw new NotSupportedException($"Metadat for {t.Name} is not found");
        }
    }
}
