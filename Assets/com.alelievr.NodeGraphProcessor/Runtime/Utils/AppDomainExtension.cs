using System.Collections.Generic;
using System.Collections;
using System;
using System.Reflection;
#if UNITY_EDITOR    
using UnityEditor;
#endif
using System.Linq;

namespace GraphProcessor
{
    public static class AppDomainExtension
    {
        public static IEnumerable<Type> GetAllTypes(this AppDomain domain)
        {
            foreach (var assembly in domain.GetAssemblies())
            {
                Type[] types = { };

                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    //just ignore it ...
                }

                foreach (var type in types)
                    yield return type;
            }
        }

        public static IEnumerable<Type> GetAllTypes(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                Type[] types = { };

                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    //just ignore it ...
                }

                foreach (var type in types)
                    yield return type;
            }
        }


        public static Type[] GetTypesDerivedFrom<T>()
        {
#if UNITY_EDITOR            
            return TypeCache.GetTypesDerivedFrom(typeof(T)).ToArray();
#endif
            return ReferencedAssemblies(typeof(T).Assembly)
                .SelectMany(o => o.GetTypes())
                .Where(o => typeof(T).IsAssignableFrom(o))
                .ToArray();

        }

        public static IEnumerable<Assembly> Referenced(IEnumerable<Assembly> assemblies, Assembly referenced)
        {
            string fullName = referenced.FullName;

            foreach (var ass in assemblies)
            {
                if (referenced == ass)
                {
                    yield return ass;
                }
                else
                {
                    foreach (var refAss in ass.GetReferencedAssemblies())
                    {
                        if (fullName == refAss.FullName)
                        {
                            yield return ass;
                            break;
                        }
                    }
                }
            }
        }
        public static IEnumerable<Assembly> ReferencedAssemblies(Assembly referenced)
        {
            return Referenced(AppDomain.CurrentDomain.GetAssemblies(), referenced);
        }
    }
}
