using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using keylogger;
using keylogger.keylog;
using spyprocess;
using spyprocess.processlog;
using spyprocess.processmodel;
using spymainwindow;
using System.Diagnostics;
using System.Threading;

namespace UserSpy
{    
    public partial class Registration : Window
    {
        private List<CheckBox> SP1 = new List<CheckBox>
        {
            new CheckBox
                    {                        
                        Content = "Следить за нажатиями",
                        Margin = new Thickness(10, 0, 0, 0),
                        FontSize = 15
                    },
            new CheckBox
                    {                        
                        Content = "Следить за процессами",
                        Margin = new Thickness(10, 0, 0, 0),
                        FontSize = 15
                    },
            new CheckBox
                    {                        
                        Content = "Закрывать ненужные процессы",
                        Margin = new Thickness(10, 0, 0, 0),
                        FontSize = 15
                    }

        };
        private List<CheckBox> SP2 = new List<CheckBox>
        {
            new CheckBox
                    {                        
                        Content = "Статика",
                        Margin = new Thickness(10, 0, 0, 0),
                        FontSize = 15
                    },
            new CheckBox
                    {                        
                        Content = "Модерация",
                        Margin = new Thickness(10, 0, 0, 0),
                        FontSize = 15
                    }
        };
        private TextBlock TB = new TextBlock();
        private char IndexCheck;

        public Registration()
        {            
            InitializeComponent();                         
        }

        private void Reg(object sender, RoutedEventArgs e)
        {
            new MainApp
            {
                RegistrationWindow = this
            }.Show();
            Close();            
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radiobtn = (sender as RadioButton)?.Content.ToString();
            IndexCheck = radiobtn[radiobtn.Length - 1];            
            switch (IndexCheck)
            {
                case '1':
                    TB = SPMode1.Children[1] as TextBlock;
                    SPMode1.Children.RemoveAt(1);
                    foreach (var checkbox in SP1)
                    {
                        SPMode1.Children.Add(checkbox);
                    }
                    break;
                case '2':
                    TB = SPMode2.Children[1] as TextBlock;                    
                    SPMode2.Children.RemoveAt(1);
                    foreach (var checkbox in SP2)
                    {
                        SPMode2.Children.Add(checkbox);
                    }
                    break;                                   
            }
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            var radiobtn = (sender as RadioButton)?.Content.ToString();
            IndexCheck = radiobtn[radiobtn.Length - 1];
            switch (IndexCheck)
            {
                case '1':
                    SPMode1.Children.RemoveRange(1, 3);
                    SPMode1.Children.Add(TB);                   
                    break;
                case '2':
                    SPMode2.Children.RemoveRange(1, 2);
                    SPMode2.Children.Add(TB);
                    break;
            }
        }
    }
}