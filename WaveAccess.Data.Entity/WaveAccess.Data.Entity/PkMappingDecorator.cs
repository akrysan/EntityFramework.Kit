using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using System.Reflection;
using WaveAccess.Linq;
namespace WaveAccess.Data.Entity {
    public class PkMappingDecorator<TC, T> : EntityTypeConfiguration<T>
        where TC : EntityTypeConfiguration<T>, new()
        where T : class {
        public PkMappingDecorator()
            : base() {
            object entityconfig = new TC();
            object config = typeof(TC).GetProperty("Configuration", BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.FlattenHierarchy).GetValue(entityconfig);
            var tableName = (string)config.GetType().GetProperty("TableName").GetValue(config);
            string schema = (string)config.GetType().GetProperty("SchemaName").GetValue(config);
            this.ToTable(tableName, schema); 
            
            IEnumerable<PropertyInfo> keys = (IEnumerable<PropertyInfo>)config.GetType().GetProperty("KeyProperties", BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.FlattenHierarchy).GetValue(config);
         
            Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(keys);

            var paramEx = Expression.Parameter(typeof(T), "m");
            ConstructorInfo ci = dynamicType.GetConstructor(keys.Select(k => k.PropertyType).ToArray());

            NewExpression nexp = Expression.New(ci, keys.Select(k => Expression.Property(paramEx, k)), dynamicType.GetFields());
            Expression selector = Expression.Lambda(nexp, paramEx);

            var hasKey = this.GetType().GetMethod("HasKey");
            hasKey.MakeGenericMethod(dynamicType).Invoke(this, new object[] { selector });
            var ignore = this.GetType().GetMethod("Ignore");

            foreach (var otherprop in typeof(T).GetProperties().Where(p => !keys.Any(k => k.Name == p.Name))) {
                Expression ignoreSelector = Expression.Lambda(Expression.Property(paramEx, otherprop), paramEx);
                ignore.MakeGenericMethod(otherprop.PropertyType).Invoke(this, new object[] { ignoreSelector });
            }
        }
    }
}