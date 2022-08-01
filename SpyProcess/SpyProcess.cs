using System.Diagnostics;
using spyprocess.processmodel;

namespace spyprocess
{
    public class SpyProcess
    {
        private ProcessModel _process;

        public SpyProcess()
        {
            _process = new ProcessModel();
        }

        public ProcessModel GetProcess(string ProcessName)
        {
            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName == ProcessName)
                {
                    try
                    {
                        return new ProcessModel(process.ProcessName, process.StartTime);
                    }
                    catch (Exception e)
                    {
                        return new ProcessModel(process.ProcessName, "Нет доступа");
                    }
                }
            }
            return new ProcessModel(string.Empty, null);            
        }

        public void KillsProcess(string ProcessName)
        {
            Task.Run(() =>
            {
                foreach (var process in Process.GetProcesses())
                {
                    if (process.ProcessName == ProcessName)
                    {
                        process.Kill();
                    }
                }
            });
        }

        public IEnumerable<ProcessModel> GetAllProcess()
        {
            foreach (var process in Process.GetProcesses())
            {                
                try
                {
                    _process = new ProcessModel(process.ProcessName, process.StartTime);
                }
                catch
                {
                    _process = new ProcessModel(process.ProcessName, "Нет доступа");
                }
                yield return _process;
            }
        }        
    }
}