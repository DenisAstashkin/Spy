using System;
using savefile;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace UserSpy
{ 
    public partial class RequestWindow : Window
    {        
        public string Path { get; private set; }       

        public string? TitleDialogFile { get; set; }
        
        public RequestWindow(string Title)
        {            
            InitializeComponent();              
            Path = string.Empty;
            LablInfo.Content = Title;
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            var ArrayStrings = InputInfo.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | 
                                                         StringSplitOptions.TrimEntries);
            InputInfo.Clear();
            foreach (var @string in ArrayStrings)
            {
                InputInfo.Text += $"{@string}\n";
            }
            MessageBox.Show("Отформатированный текст");            
            if(MessageBox.Show("Сохранить?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var Dialog = new SaveFileDialog
                {
                    Filter = "(*.txt)|*.txt",
                    Title = TitleDialogFile
                };
                Dialog.ShowDialog();
                Path = Dialog.FileName;
                if(SaveFile.Save(Dialog.FileName, (StreamWriter sw) =>
                {
                    foreach (var @string in ArrayStrings)
                    {
                        sw.WriteLine(@string);
                    }
                }))
                {
                    MessageBox.Show("Сохранено");
                }
            }
        }               
    }
}