using keylogger;
using spyprocess;
using spyprocess.processmodel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using savefile;
using System.Threading;
using System.IO;
using Microsoft.Win32;
using spymainwindow;

namespace UserSpy
{
    public partial class MainApp : Window
    {
        public Registration? RegistrationWindow { get; set; }
        private Style StyleTextBox;
        private Dictionary<string, int> SpyControl = new Dictionary<string, int>
        {
            { "KeyLogger", 0 },
            { "SpyProcess", 0 },
            { "KillProcess", 0 },
        };
        private KeyLogger? logger;
        private SpyProcess? SP;

        public MainApp()
        {
            InitializeComponent();
            StyleTextBox = (Style)this.Resources["txtBox"];
            Loaded += StartSpyProcess;
            Closed += MainApp_Closed;
        }

        private void MainApp_Closed(object? sender, EventArgs e)
        {
            
        }

        private void SetGridSpyControl(Grid grid, TextBox txtbox, string spyaction)
        {
            grid.Children.Add(txtbox);
            if(grid.Children.Count > 1)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            SpyControl[spyaction] = grid.Children.Count - 1;
            Grid.SetColumn(txtbox, grid.ColumnDefinitions.Count - 1);
        }

        private void ShowProcess()
        {
            Task.Run(() =>
            {
                SP = new SpyProcess();
                SP.ListProcess = new List<ProcessModel>();
                while (true)
                {
                    try
                    {
                        if (!SP.CheckOnNewProcess(SP.ListProcess))
                        {
                            continue;
                        }
                        Dispatcher.Invoke(() =>
                        {
                            (grid.Children[SpyControl["SpyProcess"]] as TextBox).Clear();
                        });
                        if(SP.ListProcess.Count > 0)
                            SP.ListProcess.Clear();
                        foreach (var process in SP.GetAllProcess())
                        {
                            SP.ListProcess.Add(process);                            
                            Dispatcher.Invoke(() =>
                            {
                                (grid.Children[SpyControl["SpyProcess"]] as TextBox).Text += $"[PROCESS] {process.ProcessName}    -   [TIME] {process.StartProcess}\n";
                            });
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            });
        }

        private void KillProcess(string Path, Action<string> action)
        {
            var ListProcess = SaveFile.Read(Path) as List<string>;
            if (ListProcess is null)
                return;
            Task.Run(() =>
            {                
                var SP = new SpyProcess();                
                while (true)
                {
                    foreach (var process in ListProcess)
                    {
                        action(process);                      
                    }
                }
            });
        }

        private void StartSpyProcess(object? sender, EventArgs e)
        {
            switch (RegistrationWindow?.IndexCheck)
            {
                case '1':
                    foreach (var mode in RegistrationWindow.SP1)
                    {
                        if (mode.IsChecked == false)
                            continue;
                        if (mode.Content.Equals("Следить за нажатиями"))
                        {                            
                            SetGridSpyControl(grid, new TextBox
                            {
                                Style = StyleTextBox
                            }, "KeyLogger");
                            logger = new KeyLogger
                            {
                                Show = (Key key) =>
                                {
                                    (grid.Children[SpyControl["KeyLogger"]] as TextBox).Text += key;
                                }
                            };                         
                            logger.KeyHook(Dispatcher);
                        }
                        if (mode.Content.Equals("Следить за процессами"))
                        {
                            SetGridSpyControl(grid, new TextBox
                            {
                                Style = StyleTextBox
                            }, "SpyProcess");
                            SP = new SpyProcess();
                            ShowProcess();
                        }
                        if (mode.Content.Equals("Закрывать ненужные процессы"))
                        {
                            SetGridSpyControl(grid, new TextBox
                            {
                                Style = StyleTextBox
                            }, "KillProcess");
                            SP = new SpyProcess();
                            StartKillProcess((string process) =>
                            {                                
                                Thread.Sleep(70);
                                if (SP.KillProcess(process))
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        (grid.Children[SpyControl["KillProcess"]] as TextBox).Text += $"{SP.ProcessKill} был закрыт\n";
                                    });
                                }
                            });
                        }
                    }
                    break;
                case '2':
                    foreach (var mode in RegistrationWindow.SP2)
                    {
                        var Dialog = new SaveFileDialog
                        {
                            Filter = "(*.txt)|*.txt"
                        };
                        if (mode.IsChecked == false)
                            continue;
                        if (mode.Content.Equals("Статика") == true)
                        {
                            logger = new KeyLogger
                            {
                                SaveToFile = (StreamWriter sw, Key key) =>
                                {
                                    sw.Write($"{key} ");
                                }
                            };                            
                            Dialog.ShowDialog();
                            logger.FilePath = Dialog.FileName;       
                            logger.KeyHook(Dispatcher);
                            Dialog.ShowDialog();
                            SP = new SpyProcess();
                            SaveProcess(Dialog.FileName);
                        }
                        if (mode.Content.Equals("Модерация") == true)
                        {
                            string Word = string.Empty;
                            var Request = new RequestWindow("Введите слова, которые хотите контралировать");
                            Request.ShowDialog();
                            var ListWords = SaveFile.Read(Request.Path) as List<string>;
                            Dialog.ShowDialog();
                            logger = new KeyLogger
                            {
                                Show = (Key key) =>
                                {
                                    Word += key;
                                    foreach (var word in ListWords)
                                    {
                                        if (Word.ToLower().IndexOf(word.ToLower()) >= 0)
                                        {
                                            SaveFile.Save(Dialog.FileName, (StreamWriter sw) =>
                                            {
                                                sw.WriteLine($"Слово {word} было напечатонно пользователем");
                                            });
                                            Word = string.Empty;
                                        }
                                    }
                                }
                            };
                            logger.KeyHook(Dispatcher);
                            SP = new SpyProcess();
                            Dialog.ShowDialog();
                            StartKillProcess((string process) =>
                            {
                                if (SP.KillProcess(process))
                                {
                                    SaveFile.Save(Dialog.FileName, (StreamWriter sw) =>
                                    {
                                        sw.WriteLine($"{SP.ProcessKill} был закрыт");
                                    });
                                }
                            });
                        }
                    }
                    SpyMainWindow.CloseWindow(this);
                    break;
            }
        }     

        private void StartKillProcess(Action<string> action)
        {
            var Request = new RequestWindow("Введите процессы, которые хотите закрыть через запятую");
            Request.ShowDialog();
            KillProcess(Request.Path, action);
        }

        private void SaveProcess(string path)
        {
            Task.Run(() =>
            {
                int CountScanning = 1;
                SP.ListProcess = new List<ProcessModel>();                
                while (true)
                {
                    if (!SP.CheckOnNewProcess(SP.ListProcess))
                    {
                        continue;
                    }
                    SP.ListProcess.Clear();
                    foreach (var process in SP.GetAllProcess())
                    {
                        SP.ListProcess.Add(process);                        
                    }
                    SaveFile.Save(path, (StreamWriter sw) =>
                    {
                        sw.WriteLine($"Сканирование №{CountScanning}");
                        foreach (var process in SP.ListProcess)
                        {
                            sw.WriteLine($"[PROCESS] {process.ProcessName}      -      [TIME] {process.StartProcess}");
                        }
                        sw.WriteLine();
                    });
                    CountScanning++;
                }
            });
        }
    }
}