// See https://aka.ms/new-console-template for more information
using System.Reflection;
using System.Runtime.Loader;
using dllHell;

Console.WriteLine("Hello, World!");

Console.WriteLine(Environment.CurrentDirectory);
var asc1 = LoadAssembly("../libv1/bin/Debug/net8.0/mylib.dll");

var asc2 = LoadAssembly("../libv2/bin/Debug/net8.0/mylib.dll");


var l1 = asc1.GetExportedTypes().Select(a => a.AssemblyQualifiedName).ToList();
var l2 = asc2.GetExportedTypes().Select(a => a.AssemblyQualifiedName).ToList();

Console.WriteLine(string.Join(", ", l1));
Console.WriteLine(string.Join(", ", l2));


var tdoer1 = asc1.GetExportedTypes().First();
var tdoer2 = asc2.GetExportedTypes().First();

var doer1 = Activator.CreateInstance(tdoer1) as ITHING_DOER;
var res1 = doer1?.DoThing() ?? new List<string>();
Console.WriteLine("Result1 " + string.Join("", res1));


var doer2 = Activator.CreateInstance(tdoer2) as ITHING_DOER;
var res2 = doer2?.DoThing() ?? new List<string>();
Console.WriteLine("Result2 " + string.Join("", res2));






static Assembly LoadAssembly(string path)
{
    var n = Path.GetFileNameWithoutExtension(path);
    var ctx = new ConnectorLoadContext(path);
    return ctx.LoadFromAssemblyName(new AssemblyName(n));
}

public class ConnectorLoadContext : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resolver;
    private string _pluginPath;

    public ConnectorLoadContext(string pluginPath)
    {
        _pluginPath = pluginPath; ;
        _resolver = new AssemblyDependencyResolver(pluginPath);

    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {

        string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null && File.Exists(assemblyPath))
        {
            Console.WriteLine($"Loading ASSEMBLY {assemblyName} FROM {_pluginPath}");
            return LoadFromAssemblyPath(assemblyPath);
        }

        Console.WriteLine($"Loading Globally {assemblyName}");
        return Assembly.Load(assemblyName);
    }

}