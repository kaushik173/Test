using System.Reflection;
using System.Linq;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(JcatsCAAOC.EmbeddedResourceVirtualPathProviderStart), "Start")]

namespace JcatsCAAOC
{
    public static class EmbeddedResourceVirtualPathProviderStart
    {
        public static void Start()
        {
			//By default, we scan all non system assemblies for embedded resources
            var assemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies()
                .Cast<Assembly>()
                .Where(a => a.GetName().Name.StartsWith("System") == false);
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceVirtualPathProvider.Vpp(assemblies.ToArray())
            {
				//you can do a specific assembly registration too. If you provide the assemly source path, it can read
				//from the source file so you can change the content while the app is running without needing to rebuild
				//{typeof(SomeAssembly.SomeClass).Assembly, @"..\SomeAssembly"} 
            });
        }
    }
}