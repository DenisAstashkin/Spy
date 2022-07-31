using System.Text;
using spyprocess.processmodel;

namespace spyprocess.processlog
{
    public static class ProcessLog
    {
        public static async Task<bool> AsyncSaveProcess(string path, ProcessModel process, Action<StreamWriter> SaveProcess)
        {
            return await Task<bool>.Run(() =>
            {
                try
                {
                    using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                    {
                        SaveProcess(sw);
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