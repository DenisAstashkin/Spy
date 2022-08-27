using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace UserSpy
{
    public partial class Registration : Window
    {        
        public readonly List<CheckBox> SP1 = new List<CheckBox>
        {        
            new CheckBox
                    {
                        Content = "Следить за нажатиями",
                    },   

            new CheckBox
                    {
                        Content = "Следить за процессами",                        
                    },
            
            new CheckBox
                    {
                        Content = "Закрывать ненужные процессы",                        
                    }
        };
        public readonly List<RadioButton> SP2 = new List<RadioButton>
        {
            new RadioButton
                    {                        
                        Content = "Статика",                        
                    },
            new RadioButton
                    {
                        Content = "Модерация",                       
                    }
        };
        
        private TextBlock? TB = new TextBlock();
        public char IndexCheck { get; private set; }
        
        public Registration()
        {            
            InitializeComponent();
            Style StyleCheckBx = (Style)this.Resources["CheckBx"];
            Style StyleRadButton = (Style)this.Resources["RadButton"];
            foreach (var control in SP1)
            {
                control.Style = StyleCheckBx;
            };
            foreach (var control in SP2)
            {
                control.Style = StyleRadButton;
                control.Margin = new Thickness(5, 0, 0, 0);
            };
        }

        private void CallMainWindow(object sender, RoutedEventArgs e)
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
                    SPMode1?.Children.RemoveAt(1);
                    foreach (var checkbox in SP1)
                    {
                        SPMode1?.Children.Add(checkbox);
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
                case '3':
                    SPMode3.Children.RemoveRange(1, 3);
                    SPMode3.Children.Add(TB);
                    break;
            }
        }

        private void InitStyles(List<CheckBox> controls, Style style)
        {
            foreach (var control in controls)
            {
                control.Style = style;
            }
        }
    }
}