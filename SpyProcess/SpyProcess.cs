using System.Diagnostics;

namespace spyprocess
{
    public class SpyProcess
    {
        public SpyProcess(){  }

        public (string? Name, DateTime? StartTime) GetProcess(string ProcessName)
        {
            foreach (var process in Process.GetProcesses())
            {
                if(process.ProcessName == ProcessName)
                {
                    return (process.ProcessName, process.StartTime);
                }
            }
            return (null, null);
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

        public IEnumerable<(string Name, DateTime StartTime)> GetAllProcess()
        {
            foreach (var process in Process.GetProcesses())
            {
                yield return (process.ProcessName, process.StartTime);
            }
        }        
    }
}