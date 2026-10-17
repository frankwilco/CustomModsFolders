using System.IO;
using System.Reflection;
using System.Runtime.Loader;

internal class StartupHook
{
    public static void Initialize()
    {
        // Resolve assemblies from the directory where the mod assembly is stored.
        string modAssemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        AssemblyLoadContext.Default.Resolving += (context, assemblyName) =>
        {
            string candidatePath = Path.Combine(modAssemblyDir, $"{assemblyName.Name}.dll");
            if (File.Exists(candidatePath))
            {
                return context.LoadFromAssemblyPath(candidatePath);
            }
            return null;
        };

        Doorstop.Entrypoint.Start();
    }
}
