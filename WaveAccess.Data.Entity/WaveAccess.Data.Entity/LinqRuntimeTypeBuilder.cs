using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaveAccess.Linq {
    public static class LinqRuntimeTypeBuilder {
        private static AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
        private static ModuleBuilder moduleBuilder = null;
        private static Dictionary<string, Type> builtTypes = new Dictionary<string, Type>();

        static LinqRuntimeTypeBuilder() {
            moduleBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(assemblyName.Name);
        }

        private static string GetTypeKey(Dictionary<string, Type> fields) {
            //TODO: optimize the type caching -- if fields are simply reordered, that doesn't mean that they're actually different types, so this needs to be smarter
            string key = string.Empty;
            key = "<>f__AnonymousType1`1";
            foreach (var field in fields)
                key += field.Key + ";" + field.Value.Name + ";";

            return key;
        }

        public static Type GetDynamicType(Dictionary<string, Type> fields) {
            if (null == fields)
                throw new ArgumentNullException("fields");
            if (0 == fields.Count)
                throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");

            try {
                Monitor.Enter(builtTypes);
                string className = GetTypeKey(fields);

                if (builtTypes.ContainsKey(className))
                    return builtTypes[className];

                TypeBuilder typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit | TypeAttributes.NotPublic, typeof(object));
                GenericTypeParameterBuilder[] gaBuilders = typeBuilder.DefineGenericParameters(fields.Select(x => "T" + x.Value.Name).ToArray());
                int fi = 0;
                foreach (var field in fields)
                    typeBuilder.DefineField(field.Key, gaBuilders[fi++], FieldAttributes.Public);

                var parameters = fields.ToArray();

                var ctor = typeBuilder.DefineConstructor(
                    MethodAttributes.Public, CallingConventions.Standard,
                    parameters.Select(p => p.Value).ToArray());
                var ctorIl = ctor.GetILGenerator();
                ctorIl.Emit(OpCodes.Ret);

                for (int i = 0; i < parameters.Length; i++) {
                    ctor.DefineParameter(i + 1, ParameterAttributes.None, parameters[i].Key);
                }

                builtTypes[className] = typeBuilder.CreateType().MakeGenericType(fields.Select(x => x.Value).ToArray());

                return builtTypes[className];
            } catch (Exception ex) {
            } finally {
                Monitor.Exit(builtTypes);
            }

            return null;
        }


        private static string GetTypeKey(IEnumerable<PropertyInfo> fields) {
            return GetTypeKey(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }

        public static Type GetDynamicType(IEnumerable<PropertyInfo> fields) {
            return GetDynamicType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }
    }

}
