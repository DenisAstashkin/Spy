using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spyprocess.processmodel;
using System.IO;

namespace spyprocess.processlog
{
    public class ProcessLog
    {
        public ProcessLog() { }

        public async Task<bool> AsyncSaveProcessInfo(string path, ProcessModel process)
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