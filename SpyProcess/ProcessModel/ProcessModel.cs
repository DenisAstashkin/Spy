namespace spyprocess.processmodel
{
    public class ProcessModel
    {
        public string ProcessName { get; set; }
        public object? StartProcess { get; set; }


        public ProcessModel()
        {
            ProcessName = string.Empty;
            StartProcess = null;
        }
        public ProcessModel(string ProcessName, object? StartProcess)
        {
            this.ProcessName = ProcessName;
            this.StartProcess = StartProcess;
        }
        public bool IsEmpty()
        {
            if (ProcessName == string.Empty && StartProcess == null)
                return true;
            return false;
        }
    }
}
