using System.Diagnostics;

namespace UserSpyProcess
{
    public static class UserSpyProcess
    {
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