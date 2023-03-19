using System.Runtime.CompilerServices;

namespace CS_Utils.Helpers;
public static class CurrentFileUtility
{
    public static string Directory([CallerFilePath] string file = "") => System.IO.Path.GetDirectoryName(file)!;

    public static string Path([CallerFilePath] string file = "") => file;

    public static string Relative(string relative, [CallerFilePath] string file = "")
    {
        var directory = System.IO.Path.GetDirectoryName(file)!;
        return System.IO.Path.Combine(directory, relative);
    }
}