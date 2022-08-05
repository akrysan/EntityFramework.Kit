using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using System.Reflection;
using WaveAccess.Linq;
namespace WaveAccess.Data.Entity {
    public class EntityPrimaryKeyMap<TC, T> : EntityTypeConfiguration<T>
        where TC : EntityTypeConfiguration<T>, new()
        where T : class {

        private static PropertyInfo _configurationPropertyInfo;
        private static PropertyInfo _tableNamePropertyInfo;
        private static PropertyInfo _schemaNamePropertyInfo;
        private static PropertyInfo _keyPropertiesPropertyInfo;
        private static MethodInfo _hasKeyMethodInfo;
        private static MethodInfo _ignoreMethodInfo;
        static EntityPrimaryKeyMap() {
            _configurationPropertyInfo = typeof(TC).GetProperty("Configuration", BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            var configType = Type.GetType("System.Data.Entity.ModelConfiguration.Configuration.Types.EntityTypeConfiguration,EntityFramework");
            _tableNamePropertyInfo = configType.GetProperty("TableName");
            _tableNamePropertyInfo = configType.GetProperty("TableName");
            _schemaNamePropertyInfo = configType.GetProperty("SchemaName");
            _keyPropertiesPropertyInfo = configType.GetProperty("KeyProperties", BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            var thisType = typeof(EntityPrimaryKeyMap<TC, T>);
            _hasKeyMethodInfo = thisType.GetMethods()
                .FirstOrDefault(x => {
                    if (!x.IsGenericMethod) {
                        return false;
                    }

                    // Expression<Func<T, TKey>> where TKey is generic parameter of HasKey
                    var parameterType = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(thisType.GetGenericArguments()[1], x.GetGenericArguments()[0]));
                    return x.Name == "HasKey" && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == parameterType;
                });
            _ignoreMethodInfo = thisType.GetMethod("Ignore");
        }

        public EntityPrimaryKeyMap()
            : base() {
            object entityconfig = new TC();
            object config = _configurationPropertyInfo.GetValue(entityconfig);
            var tableName = (string)_tableNamePropertyInfo.GetValue(config);
            string schema = (string)_schemaNamePropertyInfo.GetValue(config);
            this.ToTable(tableName, schema); 
            
            IEnumerable<PropertyInfo> keys = (IEnumerable<PropertyInfo>)_keyPropertiesPropertyInfo.GetValue(config);
         
            Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(keys);

            var paramEx = Expression.Parameter(typeof(T), "m");
            ConstructorInfo ci = dynamicType.GetConstructor(keys.Select(k => k.PropertyType).ToArray());

            NewExpression nexp = Expression.New(ci, keys.Select(k => Expression.Property(paramEx, k)), dynamicType.GetFields());
            Expression selector = Expression.Lambda(nexp, paramEx);

            _hasKeyMethodInfo.MakeGenericMethod(dynamicType).Invoke(this, new object[] { selector });
            foreach (var otherprop in typeof(T).GetProperties().Where(p => !keys.Any(k => k.Name == p.Name))) {
                Expression ignoreSelector = Expression.Lambda(Expression.Property(paramEx, otherprop), paramEx);
                _ignoreMethodInfo.MakeGenericMethod(otherprop.PropertyType).Invoke(this, new object[] { ignoreSelector });
            }
        }
    }
}