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

        public MainApp()
        {
            InitializeComponent();
            StyleTextBox = (Style)this.Resources["txtBox"];
            Loaded += StartSpyProcess;
            Closed += MainApp_Closed;
        }

        private void MainApp_Closed(object? sender, EventArgs e)
        {
            MessageBox.Show("Окно закрылось");
        }

        private void SetGridSpyControl(Grid grid, TextBox txtbox, string spyaction)
        {
            grid.Children.Add(txtbox);
            if(grid.Children.Count - 1 > 1)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            SpyControl[spyaction] = grid.Children.Count - 1;
            Grid.SetColumn(txtbox, grid.ColumnDefinitions.Count - 1);
        }

        private void ShowProcess(SpyProcess SP)
        {
            Task.Run(() =>
            {                
                var ListProcess = new List<ProcessModel>();
                while (true)
                {
                    try
                    {
                        if (!SP.CheckOnNewProcess(ListProcess))
                        {
                            continue;
                        }
                        Dispatcher.Invoke(() =>
                        {
                            (grid.Children[SpyControl["SpyProcess"]] as TextBox).Clear();
                        });
                        ListProcess.Clear();
                        foreach (var process in SP.GetAllProcess())
                        {
                            ListProcess.Add(process);                            
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

        private void KillProcess(string Path)
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
                        Task.Delay(50);
                        if (SP.KillProcess(process))
                        {
                            Dispatcher.Invoke(() =>
                            {
                                (grid.Children[SpyControl["KillProcess"]] as TextBox).Text += $"{SP.ProcessKill} был закрыт\n";
                            });
                        }                        
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
                            var logger = new KeyLogger();
                            SetGridSpyControl(grid, new TextBox
                            {
                                Style = StyleTextBox
                            }, "KeyLogger");
                            logger.KeyHook((Key key) =>
                            {
                                (grid.Children[SpyControl["KeyLogger"]] as TextBox).Text += key;
                                logger.LoggerKeys.Add(key);
                                MessageBox.Show($"{logger.LoggerKeys[0]}");
                            }, Dispatcher);
                        }
                        if (mode.Content.Equals("Следить за процессами"))
                        {
                            SetGridSpyControl(grid, new TextBox
                            {
                                Style = StyleTextBox
                            }, "SpyProcess");
                            ShowProcess(new SpyProcess());
                        }
                        if (mode.Content.Equals("Закрывать ненужные процессы"))
                        {
                            SetGridSpyControl(grid, new TextBox
                            {
                                Style = StyleTextBox
                            }, "KillProcess");
                            var Request = new RequestWindow("Введите процессы, которые хотите закрыть");
                            Request.ShowDialog();
                            KillProcess(Request.Path);
                        }
                    }
                    break;

            }
        }
    }
}