using System.Text;

namespace spyprocess.processlog
{
    public static class ProcessLog
    {
        public static bool SaveProcess(string path, Action<StreamWriter> SaveProcess)
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
        }
    }
}