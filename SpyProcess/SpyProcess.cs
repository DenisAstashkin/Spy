using System.Diagnostics;

namespace spyprocess
{
    public class SpyProcess
    {
        public SpyProcess(){  }

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

        public IEnumerable<(string Name, DateTime StartTime)> GetProcessInfo()
        {
            foreach (var process in Process.GetProcesses())
            {
                yield return (process.ProcessName, process.StartTime);
            }
        }        
    }
}