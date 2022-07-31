using System.Diagnostics;

namespace userspyprocess
{
    public static class UserSpyProcess
    {
        public static string? GetFullPath()
        {
            return Process.GetCurrentProcess()?.MainModule?.FileName;
        }
        public static bool ShowWindow(string path, bool show)
        {            
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = false,
                    CreateNoWindow = !show
                });
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}