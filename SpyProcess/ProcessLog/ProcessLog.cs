using System.Text;
using spyprocess.processmodel;

namespace spyprocess.processlog
{
    public static class ProcessLog
    {
        public static async Task<bool> AsyncSaveProcessInfo(string path, ProcessModel process)
        {
            return await Task<bool>.Run(() =>
            {
                try
                {
                    using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                    {
                        sw.WriteLine($"[TIME] {process.StartProcess}    -   [PROCESS] {process.ProcessName}");
                    }
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            });
        }
    }
}