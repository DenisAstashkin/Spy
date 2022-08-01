using System.Diagnostics;

namespace spyprocess
{
    public class SpyProcess
    {
        (string, object?) _process;

        public SpyProcess()
        {
            _process = (string.Empty, null);
        }

        public (string Name, object? TimeStart) GetProcess(string ProcessName)
        {
            foreach (var process in Process.GetProcesses())
            {
                if(process.ProcessName == ProcessName)
                {
                    try
                    {
                        return (process.ProcessName, process.StartTime);
                    }
                    catch(Exception e)
                    {
                        return (process.ProcessName, "Нет доступа");
                    }
                }
            }
            return (string.Empty, null);
        }

        public void KillsProcess(string ProcessName)
        {
            foreach (var process in Process.GetProcesses())
            {
                if(process.ProcessName == ProcessName)
                {
                    process.Kill();
                }
            }
        }

        public IEnumerable<(string Name, object? TimeStart)> GetAllProcess()
        {
            foreach (var process in Process.GetProcesses())
            {                
                try
                {
                    _process = (process.ProcessName, process.StartTime);
                }
                catch
                {
                    _process = (process.ProcessName, "Нет доступа");
                }
                yield return _process;
            }
        }        
    }
}