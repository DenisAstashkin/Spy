using System;
using savefile;
using System.IO;
using keylogger;
using spyprocess;
using spymainwindow;
using System.Windows;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Input;
using System.Threading.Tasks;
using spyprocess.processmodel;
using System.Windows.Controls;
using System.Collections.Generic;
using HtmlReport;

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
                                DetectKey = (Key key) =>
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
                            ReactionOnKillProcess((string process) =>
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
                            Dialog.Title = "Куда будут сохраняться нажатые клавиши";
                            Dialog.ShowDialog();
                            var FilePath = Dialog.FileName;
                            logger = new KeyLogger
                            {
                                DetectKey = (Key key) =>
                                {
                                    SaveFile.Save(FilePath, (StreamWriter sw) =>
                                    {
                                        sw.Write($"{key} ");
                                    });
                                }
                            };                                                                   
                            logger.KeyHook(Dispatcher);
                            Dialog.Title = "Куда будут сохраняться запущенные процессы";
                            Dialog.ShowDialog();
                            SP = new SpyProcess();
                            SaveProcess(Dialog.FileName);
                        }
                        if (mode.Content.Equals("Модерация") == true)
                        {
                            string Word = string.Empty;
                            var Request = new RequestWindow("Введите слова, которые хотите контралировать");
                            Request.TitleDialogFile = "Куда будет сохранён список слов контроля";
                            Request.ShowDialog();
                            var ListWords = SaveFile.Read(Request.Path) as List<string>;
                            Dialog.Title = "Куда будет сохраняться спец. отчёт";
                            Dialog.ShowDialog();
                            var FilePath = Dialog.FileName;
                            logger = new KeyLogger
                            {
                                DetectKey = (Key key) =>
                                {
                                    
                                    Word += key;                                    
                                    foreach (var word in ListWords)
                                    {
                                        if (Word.ToLower().IndexOf(word.ToLower()) >= 0)
                                        {
                                            SaveFile.Save(FilePath, (StreamWriter sw) =>
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
                            Dialog.Title = "Куда будет сохраняться отчёт ою закрытие процесса";
                            Dialog.ShowDialog();
                            ReactionOnKillProcess((string process) =>
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
                case '3':
                    var DialogShow = new OpenFileDialog
                    {
                        Title = "Введите файл куда записаны были процессы"
                    };
                    DialogShow.ShowDialog();
                    List<string>? Process = SaveFile.Read(DialogShow.FileName) as List<string>;
                    DialogShow.Title = "Введите файл куда сохранялись нажатия";
                    DialogShow.ShowDialog();
                    List<string>? Keys = SaveFile.Read(DialogShow.FileName) as List<string>;
                    var SaveDialog = new SaveFileDialog
                    {
                        Title = "Введите куда вы хотите сохранить отчёт",
                        Filter = "(*.html)|*.html"
                    };
                    SaveDialog.ShowDialog();
                    if(HTMLSave.SaveInHTMLFile(Process, Keys, SaveDialog.FileName))
                    {
                        MessageBox.Show("Отчёт успешно создан!");
                        Close();
                    }
                    break;
            }
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

        private void ReactionOnKillProcess(Action<string> action)
        {
            var Request = new RequestWindow("Введите процессы, которые хотите закрыть через запятую");
            Request.TitleDialogFile = "Куда будет сохранён список запрещённых процессов";
            Request.ShowDialog();
            KillProcess(Request.Path, action);
        }

        private void KillProcess(string Path, Action<string> action)
        {
            var ListProcess = SaveFile.Read(Path) as List<string>;
            if (ListProcess is null)
                return;
            Task.Run(() =>
            {                
                while (true)
                {
                    foreach (var process in ListProcess)
                    {
                        action(process);                      
                    }
                }
            });
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