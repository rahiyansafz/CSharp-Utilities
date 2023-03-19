namespace CS_Utils.Helpers;
public static class DotNetUtility
{
    public static bool IsDebugMode()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }
}