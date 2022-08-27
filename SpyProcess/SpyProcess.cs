using System.Diagnostics;
using spyprocess.processmodel;


namespace spyprocess
{
    public class SpyProcess
    {
        private ProcessModel _process;

        public List<ProcessModel>? ListProcess;

        public string ProcessKill { get; private set; }

        public SpyProcess()
        {
            _process = new ProcessModel();
        }

        public bool KillProcess(string ProcessName)
        {
            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName == ProcessName)
                {
                    ProcessKill = process.ProcessName;
                    process.Kill();
                    return true;
                }
            }
            return false;
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

        public bool CheckOnNewProcess(List<ProcessModel>ListProcess)
        {
            if (ListProcess.Count != Process.GetProcesses().Length)
                return true;
            return false;
        }
    }
}